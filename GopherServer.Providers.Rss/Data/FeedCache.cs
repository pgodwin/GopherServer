using System;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GopherServer.Core.Rss.Data
{
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
}
