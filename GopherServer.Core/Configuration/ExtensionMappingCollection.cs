using System.Configuration;

namespace GopherServer.Core.Configuration
{
    public class ExtensionMappingCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement() => new ExtensionMappingElement();
        protected override object GetElementKey(ConfigurationElement element) => ((ExtensionMappingElement)element).FileExtension;
    }
}
