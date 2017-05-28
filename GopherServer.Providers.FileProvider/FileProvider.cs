using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GopherServer.Core.Results;
using GopherServer.Core.Providers;

namespace GopherServer.Core
{
    public class FileProvider : ServerProviderBase
    {
        public FileProvider(string hostname, int port) : base(hostname, port)
        {
        }

        public override BaseResult GetResult(string selector)
        {
            throw new NotImplementedException();
        }

        public override void Init()
        {
            throw new NotImplementedException();
        }
    }
}
