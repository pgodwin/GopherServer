using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GopherServer.Core.GopherResults
{
    public abstract class BaseResult
    {
        public ItemType ItemType { get; internal set; }
        
        public abstract void WriteResult(Stream stream);
    }
}
