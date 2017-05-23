using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GopherServer.Core.GopherResults
{
    public class ItemType
    {
        public ItemType(char? gopherType, string name, string shortName)
        {
            this.Code = gopherType;
            this.Name = name;
            this.ShortName = shortName;
        }

        public char? Code { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public static ItemType FILE = new ItemType('0', "FILE", "TXT");
        public static ItemType DIRECTORY = new ItemType('1', "DIRECTORY", "DIR");
        public static ItemType PHONEBOOK = new ItemType('2', "PHONEBOOK", "PHO");
        public static ItemType ERROR = new ItemType('3', "ERROR", "ERR");
        public static ItemType BINHEX = new ItemType('4', "BINHEX", "HEX");
        public static ItemType DOSARCHIVE = new ItemType('5', "DOSARCHIVE", "ARC");
        public static ItemType UUENCODED = new ItemType('6', "UUENCODED", "UUE");
        public static ItemType INDEXSEARCH = new ItemType('7', "INDEXSEARCH", "QRY");
        public static ItemType TELNET = new ItemType('8', "TELNET", "TEL");
        public static ItemType BINARY = new ItemType('9', "BINARY", "BIN");

        public static ItemType REDUNDANT = new ItemType('+', "REDUNDANT", "DUP");
        public static ItemType TN3270 = new ItemType('T', "TN3270", "TN3");
        public static ItemType GIF = new ItemType('g', "GIF", "GIF");
        public static ItemType IMAGE = new ItemType('I', "IMAGE", "IMG");

        public static ItemType INFO = new ItemType('i', "INFO", "NFO");
        public static ItemType HTML = new ItemType('h', "HTML", "HTM");
        public static ItemType AUDIO = new ItemType('s', "AUDIO", "SND");
        public static ItemType PNG = new ItemType('p', "PNG", "PNG");
        public static ItemType DOC = new ItemType('d', "DOC", "DOC");

        public static ItemType COMMENT = new ItemType(null, "COMMENT", "COMMENT");


        private static Dictionary<char, ItemType> _typeDictionary;

        /// <summary>
        /// Builds a Gopher type dictionary from our static ItemType Listing.
        /// This allows us to things like Gophertypes.Types['0'] to get the FILE type.
        /// </summary>
        private static void BuildDictionary()
        {
            _typeDictionary = new Dictionary<char, ItemType>();
            var type = typeof(ItemType);
            var fields = type.GetFields(System.Reflection.BindingFlags.Static)
                             .Where(t => t.FieldType == typeof(ItemType));

            foreach (var p in fields)
            {
                var itemType = p.GetValue(null) as ItemType;
                if (itemType != null && itemType.Code != null)
                    _typeDictionary.Add(itemType.Code.Value, itemType);
            }
        }

        /// <summary>
        /// Returns a gopher Types by the thier character representations.
        /// Eg GopherTypes.Types['0'] is the same as GopherTypes.FILE
        /// </summary>
        public static Dictionary<char, ItemType> Types
        {
            get
            {
                if (_typeDictionary == null)
                    BuildDictionary();
                return _typeDictionary;
            }
        }

    }


}
