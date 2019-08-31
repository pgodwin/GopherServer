using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace GopherServer.Core.Helpers
{
    public static class ImageToGif
    {
        /// <summary>
        /// Converts the specified image to a gif
        /// </summary>
        /// <param name="image"></param>
        /// <returns></returns>
        public static byte[] ToGif(this Image image)
        {
            using (var stream = new MemoryStream())
            {
                ToGif(image, stream);
                return stream.ToArray();
            }
        }

        
        /// <summary>
        /// Converts the image byte array to gif
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] ConvertToGif(this byte[] bytes)
        {
            using (var stream = new MemoryStream(bytes))
            {
                using (var image = Image.FromStream(stream))
                    return image.ToGif();
            }
        }

        /// <summary>
        /// Saves the passed URL to gif
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public static byte[] ConvertImageToGif(string url)
        {
            return HttpHelpers.DownloadFile(url).ConvertToGif();
        }

        /// <summary>
        /// Saves the specified image to the specified stream
        /// </summary>
        /// <param name="image"></param>
        /// <param name="stream"></param>
        public static void ToGif(this Image image, Stream stream)
        {
            if (Configuration.ServerSettings.ResizeImages == false &&
                Configuration.ServerSettings.ResampleImages == false)
            {
                image.Save(stream, ImageFormat.Gif);
                image.Dispose();
                return;
            }

            Bitmap original;
            if (Configuration.ServerSettings.ResizeImages)
            {
                original = new Bitmap(image, ResizeKeepAspect(image.Size,
                    Configuration.ServerSettings.MaximumWidth.GetValueOrDefault(512),
                    Configuration.ServerSettings.MaximumHeight.GetValueOrDefault(512)));

                if (Configuration.ServerSettings.ResampleImages == false)
                    original.Save(stream, ImageFormat.Gif);
            }
            else original = new Bitmap(image);

            if (Configuration.ServerSettings.ResampleImages)
            {
                // This might not be the best way...no differing here...
                var rectangle = new Rectangle(0, 0, original.Width, original.Height);

                PixelFormat format;
                switch (Configuration.ServerSettings.MaximumBitDepth)
                {
                    case 1:
                        format = PixelFormat.Format1bppIndexed;
                        break;
                    case 4:
                        format = PixelFormat.Format4bppIndexed;
                        break;
                    case 8:
                        format = PixelFormat.Format8bppIndexed;
                        break;
                    default:
                        throw new Exception("Invalid MaximumBitDepth specified in configuration. Must 1, 4 or 8.");
                }

                using (var bmp1bpp = original.Clone(rectangle, format))
                    bmp1bpp.Save(stream, ImageFormat.Gif);
            }

            if (original != null)
                original.Dispose();

            if (image != null)
                image.Dispose();
        }

        public static Size ResizeKeepAspect(Size src, int maxWidth, int maxHeight)
        {
            var rnd = Math.Min(maxWidth / (decimal)src.Width, maxHeight / (decimal)src.Height);
            return new Size((int)Math.Round(src.Width * rnd), (int)Math.Round(src.Height * rnd));
        }
    }
}
