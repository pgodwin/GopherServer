using GopherServer.Core.Helpers;
using GopherServer.Core.Rss.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace GopherServer.Core.Rss.Syndication
{
    public static class Syndication
    {
        public static void UpdateFeeds(Db db)
        {
            foreach (var feed in db.GetFeeds())
            {
                try
                {
                    var detail = FeedDetails.FromUrl(feed.Url);
                    db.UpdateFeed(feed.Id, detail);
                }
                catch (Exception ex)
                {
                    // ahh logging where are you!
                }
            }
        }

        public static void UpdateFeed(Db db, int feedId)
        {
            var feed = db.GetFeed(feedId);

            var detail = FeedDetails.FromUrl(feed.Url);
            db.UpdateFeed(feed.Id, detail);
        }
           
        public static bool TestValidFeed(string url)
        {
            try
            {
                var detail = FeedDetails.FromUrl(url);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        
    }
   
    public class FeedDetails
    {
        public static FeedDetails FromUrl(string url)
        {
            return new FeedDetails(HttpHelpers.GetUrl(url));
        }

        public FeedDetails(string xml)
        {
            this.FeedXml = xml;


            using (XmlReader reader = XmlReader.Create(new StringReader(xml)))
            {
                this.Feed = SyndicationFeed.Load(reader);
            }
            

            this.Title = this.Feed.Title.Text;
            this.LastUpdated = this.Feed.LastUpdatedTime.UtcDateTime;

        }
        public string Title { get; set; }

        public string FeedXml { get; set; }

        public DateTime LastUpdated { get; set; }

        public SyndicationFeed Feed { get; set; }
    }
}
