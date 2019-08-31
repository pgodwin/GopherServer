namespace GopherServer.Providers.MacintoshGarden.Models
{
    public class DownloadDetails
    {
        public string Title { get; set; }
        public string Size { get; set; }
        public string Os { get; set; }
        public DownloadLink[] Links { get;set;}
    }
}
