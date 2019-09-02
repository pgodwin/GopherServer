using System.Configuration;

namespace GopherServer.Core.Configuration
{
    public static class ServerSettings
    {
        private static ExtensionMappingCollection extensionMappings;

        public static string BoundIP => ConfigurationManager.AppSettings["boundIP"];
        public static int BoundPort => int.Parse(ConfigurationManager.AppSettings["boundPort"]);
        public static string ProviderName => ConfigurationManager.AppSettings["providerName"];
        public static string PublicHostname => ConfigurationManager.AppSettings["publicHostname"];
        public static int PublicPort => int.Parse(ConfigurationManager.AppSettings["publicPort"]);
        public static bool ResampleImages => bool.Parse(ConfigurationManager.AppSettings["resampleImages"]);
        public static bool ResizeImages => bool.Parse(ConfigurationManager.AppSettings["resizeImages"]);

        public static ExtensionMappingCollection FileTypeMappings
        {
            get
            {
                if (extensionMappings != null)
                    return extensionMappings;
   
                var config =
                          ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);

                var section = config.GetSection("gopherFileMappings") as ExtensionMappingConfigSection;

                extensionMappings = section.ExtensionMappings;

                return extensionMappings;                
            }
        }

        public static int? MaximumWidth
        {
            get
            {
                int value;
                if (int.TryParse(ConfigurationManager.AppSettings["maximumWidth"], out value))
                    return value;
                return null;
            }
        }

        public static int? MaximumHeight
        {
            get
            {
                int value;
                if (int.TryParse(ConfigurationManager.AppSettings["maximumHeight"], out value))
                    return value;
                return null;
            }
        }

        public static int? MaximumBitDepth
        {
            get
            {
                int value;
                if (int.TryParse(ConfigurationManager.AppSettings["maximumBitDepth"], out value))
                    return value;
                return null;
            }
        }
    }
}
