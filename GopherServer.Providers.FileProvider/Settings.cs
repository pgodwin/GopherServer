using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GopherServer.Providers.FileProvider
{
    public static class Settings
    {
        public static string RootDirectory
        {
            get
            {
                return ConfigurationManager.AppSettings["FileProvider.RootDirectory"];
            }
        }
    }
}
