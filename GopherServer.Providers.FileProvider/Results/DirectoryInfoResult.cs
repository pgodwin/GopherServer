using System.IO;
using System.Linq;
using GopherServer.Core.Models;
using GopherServer.Core.Results;

namespace GopherServer.Providers.FileProvider
{
    public class DirectoryListingResult : DirectoryResult
    {
        public DirectoryListingResult(string path, string baseDirectory)
        {
            var dir = new DirectoryInfo(path);            
            var directories = dir.GetDirectories();
            var files = dir.GetFiles();

            // List Directories first
            Items.AddRange(directories.Select(d => new DirectoryItem(d.Name, d.FullName.Replace(baseDirectory, string.Empty))));
            Items.AddRange(files.Select(f => new DirectoryItem(GetItemType(f.FullName), f.Name, f.FullName.Replace(baseDirectory, string.Empty))));
        }

        private ItemType GetItemType(string fullName)
        {
            // TODO: Handle mapping these
            //return ItemType.FILE;
            return Core.Helpers.FileTypeHelpers.GetItemTypeFromFileName(fullName);
        }
    }
}