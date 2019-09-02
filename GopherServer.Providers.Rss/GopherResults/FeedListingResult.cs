using System.Collections.Generic;
using System.Linq;
using GopherServer.Core.Models;
using GopherServer.Core.Results;
using GopherServer.Core.Rss.Data;

namespace GopherServer.Core.Rss.GopherResults
{
    public class FeedListingResult : DirectoryResult
    {
        public FeedListingResult(string nickname, IEnumerable<Feed> feeds) : base()
        {
            this.Items.Add(new DirectoryItem("Feeds for '" + nickname + "'"));
            this.Items.Add(new DirectoryItem(""));
            this.Items.Add(new DirectoryItem(ItemType.INDEXSEARCH, "Add Feed", string.Format("/user/{0}/add/", nickname)));
            this.Items.Add(new DirectoryItem("  Enter the URL of the feed when prompted."));
            this.Items.Add(new DirectoryItem(""));

            if (feeds == null || feeds.Count() == 0)
            {
                this.Items.Add(new DirectoryItem("No feeds found."));
                return;
            }

            this.Items.Add(new DirectoryItem("Combined View", "/feeds/" + nickname + "/all/"));

            foreach (var feed in feeds)
            {
                this.Items.Add(new DirectoryItem(feed.Feedname, string.Format("/feeds/{0}/{1}/", nickname, feed.Id)));
                this.Items.Add(new DirectoryItem(feed.Url));               
            }
        }
    }
}
