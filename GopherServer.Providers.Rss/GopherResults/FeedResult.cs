using GopherServer.Core.Results;
using GopherServer.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GopherServer.Core.Models;

namespace GopherServer.Core.Rss.GopherResults
{
    public class FeedResult : DirectoryResult
    {
        public FeedResult(string nickname, int feedId, string xml) : base()
        {
            try
            {
                var feed = new Syndication.FeedDetails(xml);



                this.Items.Add(new DirectoryItem(feed.Title));
                this.Items.Add(new DirectoryItem("---------------"));
                this.Items.Add(new DirectoryItem("Last Updated: " + feed.LastUpdated.ToString()));
                this.Items.Add(new DirectoryItem("Delete Feed", string.Format("/user/{0}/delete/{1}/", nickname, feedId)));
                this.Items.Add(new DirectoryItem(""));
                this.Items.Add(new DirectoryItem("Return to Feed List", string.Format("/feeds/{0}/", nickname)));
                this.Items.Add(new DirectoryItem(""));

                foreach (var item in feed.Feed.Items)
                {
                    this.Items.Add(new DirectoryItem(item.Title.Text, string.Format("/feeds/{0}/{1}/{2}", nickname, feedId, item.Id)));
                    //this.Items.Add(new DirectoryItem("Author(s): " + string.Join(", ", item.Authors.Select(a => a.Name))));
                    this.Items.Add(new DirectoryItem("Published: " + item.PublishDate.UtcDateTime.ToString()));
                    this.Items.AddRange(item.Summary.Text.HtmlToText().WrapToDirectoryItems());

                    this.Items.Add(new DirectoryItem("---"));
                }
            }
            catch (Exception)
            {
                this.Items.Add(new DirectoryItem("Error Processing Feed."));
            }

            this.Items.Add(new DirectoryItem("Return to Feed List", string.Format("/feeds/{0}/", nickname)));
        }
    }
}
