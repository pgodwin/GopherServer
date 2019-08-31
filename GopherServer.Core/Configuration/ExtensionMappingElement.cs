using System.Configuration;

namespace GopherServer.Core.Configuration
{
    public class ExtensionMappingElement : ConfigurationElement
    {
        [ConfigurationProperty("fileExtension", IsKey = true, IsRequired = true)]
        public string FileExtension
        {
            get => (string)base["fileExtension"];
            set => base["fileExtension"] = value;
        }

        [ConfigurationProperty("gopherType", IsRequired = true)]
        public string GopherType
        {
            get => (string)base["gopherType"];
            set => base["gopherType"] = value;
        }
    }
}
