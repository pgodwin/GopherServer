using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace GopherServer.Core.Configuration
{
    public static class ServerSettings
    {
        private static ExtensionMappingCollection extensionMappings;

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

        public static string BoundIP
        {
            get { return ConfigurationManager.AppSettings["boundIP"]; }
        }

        public static int BoundPort
        {
            get { return int.Parse(ConfigurationManager.AppSettings["boundPort"]); }
        }


        public static string PublicHostname
        {
            get { return ConfigurationManager.AppSettings["publicHostname"]; }
        }

        public static int PublicPort
        {
            get { return int.Parse(ConfigurationManager.AppSettings["publicPort"]); }
        }

        public static bool ResizeImages
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["resizeImages"]); }
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

        public static bool ResampleImages
        {
            get { return bool.Parse(ConfigurationManager.AppSettings["resampleImages"]); }
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


        public static string ProviderName
        {
            get
            {
                return ConfigurationManager.AppSettings["providerName"];
            }
        }

    }
}
