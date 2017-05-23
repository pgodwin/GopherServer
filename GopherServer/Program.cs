using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GopherServer.Server
{
    class Program
    {
        static void Main(string[] args)
        {
            Server server = new Server();
            server.StartListening();
            
        }
    }
}
