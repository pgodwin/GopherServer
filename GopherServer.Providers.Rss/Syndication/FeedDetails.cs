using System;
using System.IO;
using System.ServiceModel.Syndication;
using System.Xml;
using GopherServer.Core.Helpers;

namespace GopherServer.Core.Rss.Syndication
{
    public class FeedDetails
    {
        public string Title { get; set; }
        public string FeedXml { get; set; }
        public DateTime LastUpdated { get; set; }
        public SyndicationFeed Feed { get; set; }

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

        public static FeedDetails FromUrl(string url)
        {
            return new FeedDetails(HttpHelpers.GetUrl(url));
        }
    }
}
