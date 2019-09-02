namespace GopherServer.Core.Models
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
