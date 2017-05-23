
using GopherServer.Core.GopherResults;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GopherServer.Core.Providers
{
    public abstract class ServerProviderBase : IServerProvider
    {
        public ServerProviderBase(string hostname, int port)
        {

        }

        public abstract void Init();

        public abstract BaseResult GetResult(string selector);
    }
}
