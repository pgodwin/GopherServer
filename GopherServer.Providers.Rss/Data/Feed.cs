using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GopherServer.Core.Rss.Data
{
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
}
