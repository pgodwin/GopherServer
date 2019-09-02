using System.IO;
using GopherServer.Core.Models;

namespace GopherServer.Core.Results
{
    public abstract class BaseResult
    {
        public ItemType ItemType { get; internal set; }
        
        public abstract void WriteResult(Stream stream);
    }
}
