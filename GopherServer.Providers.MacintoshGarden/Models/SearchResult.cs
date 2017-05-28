using AngleSharp;
using AngleSharp.Dom.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GopherServer.Providers.MacintoshGarden.Models
{
    public class SearchResult
    {

        public string Name { get; set; }

        public string Url { get; set; }

        public string Selector { get; set; }

        public string SearchSnippet { get; set; }

        public string Category { get; set; }


    }

    public class SearchResults
    {

        public SearchResults(string url)
        {
            this.Url = url;
            this.Parse(url);
        }

        private void Parse(string url)
        {
            var config = Configuration.Default.WithDefaultLoader();
            var doc = BrowsingContext.New(config).OpenAsync(url).Result;

            var searchText = doc.QuerySelector("#edit-keys").NodeValue;
            var currentPageNode = doc.QuerySelector("#paper > div.box > div > div > ul > li.pager-current");
            var currentPage = currentPageNode == null ? "1" : currentPageNode.TextContent;

            var nextPageNode = doc.QuerySelector("#paper > div.box > div > div > ul > li.pager-next > a") as IHtmlAnchorElement;
            var previousPageNode = doc.QuerySelector("#paper > div.box > div > div > ul > li.pager-previous > a") as IHtmlAnchorElement;

            this.Search = searchText;
            if (nextPageNode != null)
                this.NextPageLink = nextPageNode.Href;
            if (previousPageNode != null)
                this.PreviousPageLink = previousPageNode.Href;
            this.PageNumber = currentPage;

            var searchItemsNodes = doc.QuerySelectorAll("#paper > div.box > div > dl > dt.title a, dd .search-snippet");

            var results = new List<SearchResult>();
            for (int i = 0; i < searchItemsNodes.Count()/2; i=i+2)
            {
                var titleNode = ((IHtmlAnchorElement)searchItemsNodes[i]);
                var searchSnippet = searchItemsNodes[i + 1];

                var result = new SearchResult
                {
                    Name = titleNode.Text,
                    Url = titleNode.Href,
                    SearchSnippet = searchSnippet.TextContent,
                    Selector = "/app/" + titleNode.Href
                };

                results.Add(result);
            }

            this.Results = results;

        }

        public string Url { get; set; }

        public string Search { get; set; }

        public string NextPageLink { get; set; }

        public string PreviousPageLink { get; set; }

        public string PageNumber { get; set; }

        public List<SearchResult> Results { get; set; }
    }
}
