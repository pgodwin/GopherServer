using SQLite.Net.Attributes;
using SQLiteNetExtensions.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GopherServer.Core.Rss.Data
{
    public class User
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        [MaxLength(16)]
        public string NickName { get; set; }

        public DateTime Created { get; set; }

        public DateTime LastLogin { get; set; }


        [ManyToMany(typeof(UserFeed))]
        public List<Feed> Feeds { get; set; }

        
    }

    public class Feed
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [MaxLength(100)]
        public string Feedname { get; set; }

        [Indexed]
        [MaxLength(255)]
        public string Url { get; set; }

        [OneToOne]
        public FeedCache FeedCache { get; set; }

        [ManyToMany(typeof(User))]
        public List<User> Users { get; set; }

    }

    public class FeedCache
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string CacheData { get; set; }

        [ForeignKey(typeof(Feed))]
        public int FeedId { get; set; }
        
        public DateTime LastRefreshed { get; set; }

        [OneToOne]
        public Feed Feed { get; set; }
    }


    public class UserFeed
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(User))]
        public int UserId { get; set; }

        [ForeignKey(typeof(Feed))]
        public int FeedId { get; set; }

        [ManyToOne]
        public Feed Feed { get; set; }

        [ManyToOne]
        public User User { get; set; }
    }

    
}
