using System;
using System.IO;
using System.Linq;
using GopherServer.Core.Configuration;
using GopherServer.Core.Models;

namespace GopherServer.Core.Helpers
{
    public static class FileTypeHelpers
    {
        public static ItemType GetItemTypeFromFileName(string filename)
        {
            var types = ServerSettings.FileTypeMappings.OfType<ExtensionMappingElement>();
            var extension = Path.GetExtension(filename);
            var fileType = types.FirstOrDefault(f => string.Equals(f.FileExtension, extension, StringComparison.InvariantCultureIgnoreCase));

            if (fileType == null)
                return ItemType.BINARY; // File tends to return asci in Seamonkey at least...
            var gopherType = ItemType.Types.Values.FirstOrDefault(p => p.Name == fileType.GopherType);
            if (gopherType == null)
                return ItemType.BINARY;

            return gopherType;
        }
    }
}
