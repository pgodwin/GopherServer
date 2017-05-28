using GopherServer.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GopherServer.Core.Results
{
    public class ExternalUrlItem : DirectoryItem
    {
        public ExternalUrlItem(string text, string url)
        {
            this.ItemType = ItemType.HTML;
            this.Description = text;
            this.Selector = "URL:" + url;
        }
    }
}
