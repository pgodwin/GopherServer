using SQLite;
using SQLiteNetExtensions.Attributes;

namespace GopherServer.Core.Rss.Data
{
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
