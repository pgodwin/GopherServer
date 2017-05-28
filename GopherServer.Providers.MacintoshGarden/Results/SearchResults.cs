using GopherServer.Core.Helpers;
using GopherServer.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GopherServer.Providers.MacintoshGarden.Results
{
    public class SearchResult : DirectoryResult
    {
        public SearchResult(Models.SearchResults results)
        {
            this.Items.Add(new DirectoryItem("Search Results"));
            this.Items.Add(new DirectoryItem("--------------"));

            this.Items.Add(new DirectoryItem(ItemType.INDEXSEARCH, "New Search", "/search/"));

            foreach (var result in results.Results)
            {
                this.Items.Add(new DirectoryItem(result.Name, result.Selector));
                this.Items.AddRange(result.SearchSnippet.CleanString().WrapToDirectoryItems());
                this.Items.Add(new DirectoryItem("----"));
            }

            this.Items.Add(new DirectoryItem("Current Page: " + results.PageNumber));

            if (!string.IsNullOrEmpty(results.PreviousPageLink))
                this.Items.Add(new DirectoryItem("Previous Page", "/search/" + results.PreviousPageLink));
            if (!string.IsNullOrEmpty(results.NextPageLink))
                this.Items.Add(new DirectoryItem("Next Page", "/search/" + results.NextPageLink));
            
        }

    }
}
