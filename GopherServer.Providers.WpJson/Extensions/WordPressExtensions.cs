using System.Collections.Generic;
using WordPressRestApiStandard.Models;
using GopherServer.Core.Helpers;
using GopherServer.Core.Models;

namespace GopherServer.Core.WpJson.Extensions
{
    public static class WordPressExtensions
    {
        /// <summary>
        /// Converts the specified posts to Directory Items
        /// </summary>
        /// <param name="posts"></param>
        /// <returns></returns>
        public static List<DirectoryItem> ToDirectoryItems(this List<Post> posts)
        {
            //return posts.Select(p => new DirectoryItem
            //{
            //    Description = p.Title.Rendered,
            //    ItemType = ItemType.DIRECTORY,
            //    Selector = "/posts/" + p.Id
            //}).ToList();
            var items = new List<DirectoryItem>();

            foreach (var p in posts)
            {
                // Add a directory for the title
                items.Add(new DirectoryItem(ItemType.DIRECTORY, p.Title.Rendered.CleanHtml(), "/posts/" + p.Id));

                // Add the blurb
                items.AddRange(p.Excerpt.Rendered.CleanHtml().HtmlToText().WrapToDirectoryItems(80));
            }

            return items;
        }
    }
}
