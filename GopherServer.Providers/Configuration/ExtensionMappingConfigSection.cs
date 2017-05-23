using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;


namespace GopherServer.Core.Configuration
{
    public class ExtensionMappingConfigSection : ConfigurationSection
    {
        public ExtensionMappingCollection ExtensionMappings
        {
            get { return (ExtensionMappingCollection)this[""]; }
            set { this[""] = value; }
        }
    }

    public class ExtensionMappingCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new ExtensionMappingElement();
        }
        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((ExtensionMappingElement)element).FileExtension;
        }
    }

    public class ExtensionMappingElement : ConfigurationElement
    {
        [ConfigurationProperty("fileExtension", IsKey = true, IsRequired = true)]
        public string FileExtension
        {
            get { return (string)base["fileExtension"]; }
            set { base["fileExtension"] = value; }
        }

        [ConfigurationProperty("gopherType", IsRequired = true)]
        public string GopherType
        {
            get { return (string)base["gopherType"]; }
            set { base["gopherType"] = value; }
        }

    }
}
