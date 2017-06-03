using System;
using System.IO;
using GopherServer.Core.Results;

namespace GopherServer.Providers.FileProvider
{
    internal class FileResult : BaseResult
    {
        private string baseDirectory;
        private string path;

        public FileResult(string path, string baseDirectory)
        {
            this.path = path;
            this.baseDirectory = baseDirectory;
        }

        public override void WriteResult(Stream stream)
        {
            File.OpenRead(this.path).CopyTo(stream);
        }
    }
}