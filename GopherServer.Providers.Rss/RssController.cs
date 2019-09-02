using System;
using System.Collections.Generic;
using GopherServer.Core.Models;
using GopherServer.Core.Results;
using GopherServer.Core.Rss.Data;
using GopherServer.Core.Rss.GopherResults;

namespace GopherServer.Core.Rss
{
    public class RssController
    {
        public Db Db { get; private set; }

        public RssController(Db db) => this.Db = db;

        public FeedListingResult GetUserFeeds(string nickname)
        {
            Db.UpdateUserLogin(nickname);
            var feeds = Db.GetUserFeeds(nickname);
            return new FeedListingResult(nickname, feeds);
        }

        public DirectoryResult GetCombinedFeeds(string nickname)
        {
            throw new NotImplementedException();
        }

        public DirectoryResult GetFeed(string nickname, int feedId)
        {
            var feed = Db.GetFeedCache(nickname, feedId);
            return new FeedResult(nickname, feedId, feed);
        }

        public BaseResult RegisterUser(string nickname)
        {
            try
            {
                Db.AddUser(nickname);
                return GetUserFeeds(nickname);
            }
            catch (Exception)
            {
                // User probably already exists
                return new ErrorResult("Unable to register '" + nickname + '"');
            }
        }

        public BaseResult AddFeed(string nickname, string feedUrl)
        {
            if (Syndication.Syndication.TestValidFeed(feedUrl))
            {
                var id = Db.AddFeed(nickname, feedUrl);
                Syndication.Syndication.UpdateFeed(Db, id);
                return GetUserFeeds(nickname);
            }
            
            return new ErrorResult("Invalid Feed");
        }

        internal BaseResult DeleteFeed(string nickname, int feedId)
        {
            // TODO remove feed code
            return GetUserFeeds(nickname);
        }

        internal DirectoryResult GetHomePage()
        {
            return new DirectoryResult(new List<DirectoryItem>()
            {
                new DirectoryItem("Welcome to Gopher RSS"),
                new DirectoryItem("---------------------"),
                new DirectoryItem(ItemType.INDEXSEARCH, "Register", "/register/"),
                new DirectoryItem("Enter a nickname to register."),
                new DirectoryItem("   Note there is no security on this. If someone"),
                new DirectoryItem("   can guess your nickname then they can edit"),
                new DirectoryItem("   your feeds."),
                new DirectoryItem(""),
                new DirectoryItem(ItemType.INDEXSEARCH, "Login", "/feeds/"),
                new DirectoryItem("Enter your nickname to retrieve your feeds."),
                new DirectoryItem(""),
                new DirectoryItem("Powered by GopherServer - https://github.com/pgodwin/GopherServer/"),
            });
        }

        public BaseResult GetFeedItem(string nickname, int feedId, string itemId)
        {
            var xml = Db.GetFeedCache(nickname, feedId);
            return new FeedItemResult(nickname, feedId, xml, itemId);
        }
    }
}
