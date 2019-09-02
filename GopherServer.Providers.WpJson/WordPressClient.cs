
using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using WordPressRestApiStandard;
using WordPressRestApiStandard.Models;
using WordPressRestApiStandard.QueryModel;
using GopherServer.Core.Helpers;
using GopherServer.Core.Models;
using GopherServer.Core.Results;
using GopherServer.Core.WpJson.Extensions;

namespace GopherServer.Core.WpJson
{
    public class WordPressClient
    {
        public static WordPressApiClient client;

        public WordPressClient(string url)
        {
            client = new WordPressApiClient(url);
        }
      
        /// <summary>
        /// Produces a basic Homepage for the blog
        /// </summary>
        /// <returns></returns>
        public DirectoryResult GetHomePage()
        {
            var result = new DirectoryResult();
            result.Items.Add(new DirectoryItem("Welcome"));
            result.Items.Add(new DirectoryItem("-------"));

            result.Items.Add(new DirectoryItem("Latest Posts"));

            var latestPosts = client.GetPosts(new PostsQuery { PerPage = 10 }).Result;
            result.Items.AddRange(latestPosts.ToDirectoryItems());

            result.Items.Add(new DirectoryItem("---"));
            result.Items.Add(new DirectoryItem(ItemType.DIRECTORY, "Categories", "/categories/"));
            result.Items.Add(new DirectoryItem(ItemType.INDEXSEARCH, "Search", "/search/"));

            // TODO: Add Tags and Pages

            return result;
        }

        /// <summary>
        /// Peforms a search of the blog
        /// </summary>
        /// <param name="q"></param>
        /// <returns></returns>
        public DirectoryResult Search(string q)
        {
            var posts = client.GetPosts(new PostsQuery { Search = q, PerPage = 100, OrderBy = OrderBy.title.ToString() }).Result;
            var directory = new DirectoryResult();

            directory.Items.Add(new DirectoryItem("Search for '" + q + "'"));

            if (posts.Count > 0)
                directory.Items.AddRange(posts.ToDirectoryItems());
            else
                directory.Items.Add(new DirectoryItem("No results found."));

            // TODO - add paging!

            return directory;
        }

        /// <summary>
        /// Gets the first 100 categories from the blog ordered by their use.
        /// </summary>
        /// <returns></returns>
        public DirectoryResult GetCategories()
        {
            List<Category> result = client.GetCategories(new CategoriesQuery() { HideEmpty = true, OrderBy = "count", Order="desc", PerPage = 100 }).Result;

            var directory = new DirectoryResult();
            directory.Items.Add(new DirectoryItem("Categories"));

            directory.Items.AddRange(result.Select(c => new DirectoryItem()
            {
                Description = string.Format("{0} ({1})", c.Name, c.Count),
                ItemType = ItemType.DIRECTORY,
                Selector = "/category/" + c.Id
            }));

            return directory;
            
        }

        /// <summary>
        /// Returns the post as a DirectoryListing for the spcified ID
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DirectoryResult GetPost(int id)
        {
            var result = new DirectoryResult();

            var post = client.GetPost(new PostQuery { }, id).Result;
            result.Items.Add(new DirectoryItem(post.Title.Rendered.CleanHtml()));
            result.Items.Add(new DirectoryItem("---"));
            result.Items.Add(new DirectoryItem("Author: " + post.Author));
            result.Items.Add(new DirectoryItem("Date Posted: " + post.DateGmt.ToString()));
            result.Items.Add(new DirectoryItem(" "));
            result.Items.Add(new DirectoryItem(ItemType.DOC, "Text Version", "/posts/text/" + id));
            result.Items.Add(new DirectoryItem(ItemType.HTML, "Web Link", "URL:" + post.Link));
            result.Items.Add(new DirectoryItem("---"));
            
            result.Items.AddRange(post.Content.Rendered.ToDirectoryItems());

            result.Items.Add(new DirectoryItem("----------------------------"));

            return result;
        }

        public TextResult GetPostText(int id)
        {
            var post = client.GetPost(new PostQuery { }, id).Result;
            return new TextResult(post.Content.Rendered.CleanHtml().HtmlToText().WrapText(80));
        }

     
        /// <summary>
        /// Gets the posts for the specified category.
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public DirectoryResult GetCategoryPosts(int category)
        {
            var posts = client.GetPosts(new PostsQuery { Categories = new List<int>() { category }, PerPage = 100 }).Result;
            return new DirectoryResult(posts.ToDirectoryItems());

        }

        /// <summary>
        /// Converts the specified URL to a GIF
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public ByteResult GetGif(string url)
        {
            return new ByteResult(ImageToGif.ConvertImageToGif(url), ItemType.GIF);
        }

        /// <summary>
        /// Returns a HTML page to redirect to the propper address
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public UrlResult Redirect(string url)
        {
            return new UrlResult(url);
        }

        public DirectoryResult ProxyPage(string url)
        {
            var result = new DirectoryResult();

            var html = HttpHelpers.GetUrl(url);

            HtmlDocument doc = new HtmlDocument();
            doc.LoadHtml(html);
            var title = doc.DocumentNode.Descendants("title").SingleOrDefault();

            result.Items.Add(new DirectoryItem("Title: " + title));
            result.Items.Add(new DirectoryItem("Url:   " + url));
            result.Items.Add(new DirectoryItem("---------------"));
            result.Items.Add(new DirectoryItem(""));
            result.Items.AddRange(html.ToDirectoryItems());

            return result;
        }
    }
}
