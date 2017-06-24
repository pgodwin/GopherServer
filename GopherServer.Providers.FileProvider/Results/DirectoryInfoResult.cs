using GopherServer.Core.Results;
using System.IO;
using System.Linq;
using System;
using GopherServer.Core.Models;

namespace GopherServer.Providers.FileProvider
{
    public class DirectoryListingResult : DirectoryResult
    {
        private string baseDirectory;
        private string path;

        public DirectoryListingResult(string path, string baseDirectory)
        {
            this.path = path;
            this.baseDirectory = baseDirectory;

            var dir = new DirectoryInfo(path);

            
            var directories = dir.GetDirectories();
            var files = dir.GetFiles();

            // List Directories first
            this.Items.AddRange(directories.Select(d => 
                new DirectoryItem(d.Name, d.FullName.Replace(baseDirectory, string.Empty))));

            this.Items.AddRange(files.Select(f =>
                new DirectoryItem(this.GetItemType(f.FullName), f.Name, f.FullName.Replace(baseDirectory, string.Empty))));

        }

        private ItemType GetItemType(string fullName)
        {
            // TODO: Handle mapping these
            //return ItemType.FILE;
            return Core.Helpers.FileTypeHelpers.GetItemTypeFromFileName(fullName);
        }
    }
}