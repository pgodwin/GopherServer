using System;
using System.Collections.Generic;
using SQLite;
using SQLiteNetExtensions.Attributes;

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
}
