using System.Configuration;

namespace GopherServer.Providers.FileProvider
{
    public static class Settings
    {
        public static string RootDirectory => ConfigurationManager.AppSettings["FileProvider.RootDirectory"];
    }
}
