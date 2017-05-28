using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GopherServer.Core.Results
{
    /// <summary>
    /// Returns a HTML page with a link to the request url
    /// </summary>
    public class UrlResult : TextResult
    {
        public UrlResult(string url)
        {
            this.Text = string.Format("<html><head><meta http-equiv=\"refresh\" content=\"2; url = {0}\"></head><body><p>Follow <a href=\"{0}\">{0}</a></body></html>", url);
        }
    }
}
