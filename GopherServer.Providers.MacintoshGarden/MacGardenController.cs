using GopherServer.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GopherServer.Providers.MacintoshGarden.Results;
using GopherServer.Providers.MacintoshGarden.Models;
using System.Net;

namespace GopherServer.Providers.MacintoshGarden
{
    public class MacGardenController
    {
        string[] validHosts = new string[] { "www.macintoshgarden.org", "macintoshgarden.org", "mirror.macintoshgarden.org" };
       
        internal DirectoryResult Search(string search)
        {
            var searchUrl = "http://macintoshgarden.org/search/node/" + WebUtility.UrlEncode(search + " type:app,game");

            var searchResults = new Models.SearchResults(searchUrl);

            return new Results.SearchResult(searchResults);
        }

        internal DirectoryResult SearchPage(string url)
        {
            
            var searchResults = new Models.SearchResults(url);

            return new Results.SearchResult(searchResults);
        }

        public BaseResult DoDownload(string url)
        {
            var uri = new Uri(url);

            if (!validHosts.Contains(uri.Host))
                return new ErrorResult("Invalid host");
            return new ProxyResult(url, "http://macintoshgarden.org");
        }

        public BaseResult ShowApp(string url)
        {
            return new SoftwareResult(new SoftwareItem(url));
        }

        public DirectoryResult ShowHome()
        {
            var result = new DirectoryResult();
            result.Items.Add(new DirectoryItem("Macintosh Garden - Gopher Edition"));
            result.Items.Add(new DirectoryItem("================================="));
            result.Items.Add(new DirectoryItem(""));

            result.Items.Add(new DirectoryItem(ItemType.INDEXSEARCH, "Search the Garden", "/search/"));

            return result;
        }
    }
}
