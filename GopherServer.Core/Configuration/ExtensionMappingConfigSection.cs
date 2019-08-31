using System.Configuration;

namespace GopherServer.Core.Configuration
{
    public class ExtensionMappingConfigSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsDefaultCollection = true)]
        [ConfigurationCollection(typeof(ExtensionMappingCollection), AddItemName = "add", ClearItemsName = "clear", RemoveItemName = "remove")]
        public ExtensionMappingCollection ExtensionMappings
        {
            get => (ExtensionMappingCollection)this[""];
            set => this[""] = value;
        }
    }
}
