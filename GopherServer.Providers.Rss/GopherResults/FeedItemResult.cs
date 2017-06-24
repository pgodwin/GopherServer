using GopherServer.Core.Results;
using GopherServer.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using GopherServer.Core.Models;

namespace GopherServer.Core.Rss.GopherResults
{
    public class FeedItemResult : DirectoryResult
    {
        public FeedItemResult(string nickname, int feedId, string xml, string itemId) : base()
        {
            var feed = new Syndication.FeedDetails(xml);
            // Find the item
            var item = feed.Feed.Items.FirstOrDefault(i => i.Id == itemId);
            string text;
            var content = item.Content as TextSyndicationContent;
            if (content != null)
                text = content.Text;
            else
                text = item.Summary.Text;

            this.Items.Add(new DirectoryItem(item.Title.Text));
            this.Items.Add(new DirectoryItem("------------"));
            this.Items.Add(new DirectoryItem(item.PublishDate.UtcDateTime.ToString()));
            this.Items.AddRange(text.HtmlToText().WrapToDirectoryItems());

            if (item.Links.Any())
                this.Items.Add(new ExternalUrlItem("Read More...", item.Links.First().Uri.ToString()));

            this.Items.Add(new DirectoryItem("---"));

            this.Items.Add(new DirectoryItem("Return to '" + feed.Title + "'...", string.Format("/feeds/{0}/{1}/", nickname, feedId)));
            this.Items.Add(new DirectoryItem("Return to Feed List...", string.Format("/feeds/{0}/", nickname)));

        }
    }
}
