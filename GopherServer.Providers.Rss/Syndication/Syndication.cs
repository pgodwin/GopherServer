using System;
using GopherServer.Core.Rss.Data;

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
                catch (Exception)
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
            catch (Exception)
            {
                return false;
            }
        }
    }
}
