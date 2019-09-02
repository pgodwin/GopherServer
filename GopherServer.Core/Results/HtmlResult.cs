using GopherServer.Core.Helpers;

namespace GopherServer.Core.Results
{
    /// <summary>
    /// Returns a HTML document as a DirectoryResult
    /// </summary>
    public class HtmlResult : DirectoryResult
    {
        public HtmlResult(string url)
        {
            var html = HttpHelpers.GetUrl(url);
            this.Items = html.CleanHtml().ToDirectoryItems();
        }
    }
}
