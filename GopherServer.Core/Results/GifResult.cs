using GopherServer.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GopherServer.Core.Results
{
    /// <summary>
    /// Converts an Image URL to a GifResult
    /// </summary>
    public class GifResult : ByteResult
    {
        public GifResult(string url)
        {
            this.ItemType = Models.ItemType.GIF;
            this.ResultBytes = ImageToGif.ConvertImageToGif(url);
        }
    }
}
