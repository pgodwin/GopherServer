using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GopherServer.Core.Results
{
    public class ErrorResult : DirectoryResult
    {
        public ErrorResult(string error) : base()
        {
            this.Items.Add(new DirectoryItem(ItemType.ERROR, error, ""));
            this.Items.Add(new DirectoryItem("Return Home", ""));
        }
    }
}
