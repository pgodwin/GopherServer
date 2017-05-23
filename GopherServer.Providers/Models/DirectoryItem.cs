using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GopherServer.Core.GopherResults
{
    public class DirectoryItem 
    {
        public DirectoryItem()
        {

        }

        public DirectoryItem(string infoText)
        {
            this.ItemType = ItemType.INFO;
            this.Description = infoText;
        }

        public DirectoryItem(ItemType type, string description, string selector)
        {
            this.ItemType = type;
            this.Description = description;
            this.Selector = selector;
        }

        public ItemType ItemType { get; set; }

        public string Description { get; set; }

        /// <summary>
        /// Selector shouldn't be longer than 255 characters according to the RFC
        /// Look into limiting it - we might need to implement a selector cache
        /// </summary>
        public string Selector { get; set; }

        public string Host { get; set; }

        public int Port { get; set; }

        public string[] Extras { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            if (this.ItemType != null)
            {
                sb.Append(this.ItemType.Code);
                sb.Append(this.Description);
                sb.Append("\t");
                sb.Append(this.Selector);
                sb.Append("\t");
                sb.Append(this.Host);
                sb.Append("\t");
                sb.Append(this.Port);

                if (this.Extras != null)
                {
                    foreach (var i in Extras)
                    {
                        sb.Append("\t");
                        sb.Append(i);
                    }
                }
            }
            else
            {
                sb.Append(this.Description);
            }
            return sb.ToString();

        }

        public static DirectoryItem Parse(string line)
        {
            var item = new DirectoryItem();
            var parts = line.Split('\t');
            if (parts.Length == 0)
                throw new Exception("No item type: " + line);

            item.ItemType = ItemType.Types[parts[0][0]];
            item.Description = string.Concat(parts[0].Skip(1));

            if (parts.Length > 1)
                item.Selector = parts[1];
            else
                item.Selector = "";

            if (parts.Length > 2)
                item.Host = parts[2];
            else
                item.Host = "null.host";

            if (parts.Length > 3)
            {
                int port;
                if (int.TryParse(parts[3], out port))
                    item.Port = port;
                else
                    item.Port = 0;
            }
            else
                item.Port = 0;

            if (parts.Length >= 4)
            {
                item.Extras = parts.Skip(4).ToArray();
            }

            return item;
        }
    }
}
