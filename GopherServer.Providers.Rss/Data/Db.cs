using System;
using System.Collections.Generic;
using System.Linq;
using SQLite;
using SQLiteNetExtensions.Extensions;
using GopherServer.Core.Rss.Syndication;

namespace GopherServer.Core.Rss.Data
{
    public class Db
    {
        SQLiteConnection connection;

        public Db(string dbPath)
        {
            connection = new SQLiteConnection(dbPath);
            CreateDb();
        }

        public void CreateDb()
        {            
            connection.CreateTable<User>();
            connection.CreateTable<Feed>();
            connection.CreateTable<FeedCache>();
            connection.CreateTable<UserFeed>();
        }

        public User GetUser(string nickname)
        {
            var user = connection.Table<User>().FirstOrDefault(u => u.NickName == nickname);
            if (user == null)
                throw new Exception("No user found for nickanme '" + nickname + "'");

            return user;
        }

        public Feed GetFeed(int feedId)
        {
            return connection.Find<Feed>(feedId);
        }

        public string GetFeedCache(string nickname, int feedId)
        {
            var user = this.GetUser(nickname);
            // Make sure the user has a feed
            if (!connection.Table<UserFeed>().Any(f => f.FeedId == feedId && f.UserId == user.Id))
                throw new Exception("Invalid Feed Id for this user.");

            return connection.Table<FeedCache>().FirstOrDefault(f => f.FeedId == feedId).CacheData;
        }

        internal IEnumerable<Feed> GetFeeds()
        {
            return connection.Table<Feed>();
        }

        public IEnumerable<User> GetUsers()
        {
            return connection.Table<User>().AsEnumerable();
        }

        public IEnumerable<Feed> GetUserFeeds(string nickName)
        {
            var userId = this.GetUser(nickName).Id;
            var user = connection.GetWithChildren<User>(userId);
            return user.Feeds;
        }

        public void AddUser(string nickName)
        {
            connection.Insert(new User()
            {
                NickName = nickName,
                Created = DateTime.Now,
                LastLogin = DateTime.Now,
            });
        }

        public int AddFeed(string nickName, string url)
        {
            // Get the User
            var user = GetUser(nickName);

            // Check if the URL already exists
            var feed = connection.Table<Feed>().FirstOrDefault(f => f.Url == url);
            if (feed == null)
            {
                // Create the feed
                feed = new Feed();
                feed.Feedname = "TBA";
                feed.Url = url;

                connection.Insert(feed);
            }
            // Check if the user has the feed
            if (connection.GetAllWithChildren<UserFeed>().Any(f => f.FeedId == feed.Id && f.User.NickName == nickName))
                return feed.Id; // user already has feed


            UserFeed userFeed = new UserFeed()
            {
                FeedId = feed.Id,
                UserId = user.Id
            };

            connection.Insert(userFeed);

            return feed.Id;
        }

        public void UpdateCache(int feedId, string cacheData)
        {
            // grab the existing cache (if it exists)
            var cache = connection.Table<FeedCache>().FirstOrDefault(c => c.FeedId == feedId);
            if (cache == null)
            {
                cache = new FeedCache();
                cache.FeedId = feedId;
                cache.CacheData = cacheData;
                cache.LastRefreshed = DateTime.Now;
                connection.Insert(cache);
                return;
            }
            cache.CacheData = cacheData;
            cache.LastRefreshed = DateTime.Now;
            connection.Update(cache);
        }

        public void UpdateFeed(int feedId, FeedDetails detail)
        {
            var feed = connection.Find<Feed>(feedId);
            feed.Feedname = detail.Title;
            connection.Update(feed);
            this.UpdateCache(feedId, detail.FeedXml);
        }

        public void UpdateUserLogin(string nickName)
        {
            var user = GetUser(nickName);
            user.LastLogin = DateTime.Now;
            connection.Update(user);
        }

        public List<string> CachedFeedDataForUser(string nickName)
        {
            var user = GetUser(nickName);
            return user.Feeds.Select(f => f.FeedCache.CacheData).ToList();
        }

    }
}
