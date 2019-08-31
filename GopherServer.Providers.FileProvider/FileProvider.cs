using System.IO;
using System.Linq;
using GopherServer.Core.Providers;
using GopherServer.Core.Results;

namespace GopherServer.Providers.FileProvider
{
    public class FileProvider : ServerProviderBase
    {
        public FileProvider(string hostname, int port) : base(hostname, port)
        { }

        public string BaseDirectory { get; set; }

        public override void Init() => this.BaseDirectory = Settings.RootDirectory; 

        public override BaseResult GetResult(string selector)
        {
            if (string.IsNullOrEmpty(selector))
                return new DirectoryListingResult(BaseDirectory, BaseDirectory);

            if (selector.Contains(".."))
                return new ErrorResult("Invalid Path");

            selector = selector.TrimStart(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);

            var path = Path.Combine(BaseDirectory, selector);

            var indexPath = Path.Combine(path, "index.gopher");

            if (File.Exists(indexPath))
                return new TextResult(File.ReadAllLines(indexPath).ToList());

            if (File.Exists(path))
            {
                return new FileResult(path, BaseDirectory);
            }

            if (Directory.Exists(path))
                return new DirectoryListingResult(path, BaseDirectory);

            return new ErrorResult("Invalid Path");
        }       
    }
}
