using AngleSharp;
using AngleSharp.Dom.Html;
using AngleSharp.Parser.Html;
using GopherServer.Core.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GopherServer.Providers.MacintoshGarden.Models
{
    public class SoftwareItem
    {
        public SoftwareItem(string url)
        {
            var html = HttpHelpers.GetUrl(url);
            this.Url = url;
            this.Parse(url);
        }

        public void Parse(string url)
        {
            //var parser = new HtmlParser();

            //Just get the DOM representation
            // var doc = parser.Parse(html);

            var config = Configuration.Default.WithDefaultLoader();
            var doc = BrowsingContext.New(config).OpenAsync(url).Result; 


            this.Title = doc.QuerySelector("#paper > h1").TextContent.Trim();
            this.Rating = doc.QuerySelector("#edit-vote-wrapper > div.description > div > span.average-rating > span").TextContent.CleanString();


            this.Category = doc.QuerySelector("#paper > div.game-preview > div.descr > table > tbody > tr:nth-child(2) > td:nth-child(2) > ul > li > a").TextContent;
            this.CategoryLink = doc.QuerySelector("#paper > div.game-preview > div.descr > table > tbody > tr:nth-child(2) > td:nth-child(2) > ul > li > a").GetAttribute("href");

            // For the screenshots we just want to link to full images
            var screenNodes = doc.QuerySelectorAll("#paper > div.game-preview > div.images a.thickbox");
            this.Screenshots = screenNodes.OfType<IHtmlAnchorElement>().Select(n => n.Href).ToArray();

            this.Description = string.Join("\r\n", doc.QuerySelectorAll("#paper > p").Select(n => n.TextContent));
            //this.ManualUrl = doc.QuerySelector("#paper .note.manual a").GetAttribute("href");

            var downloadNodes = doc.QuerySelectorAll("#paper > div.game-preview > div.descr .note.download");

            List<DownloadDetails> downloads = new List<Models.DownloadDetails>();

            foreach (var downloadElement in downloadNodes)
            {
                // Grab the filename > div:nth-child(2) > small
                var fileName = downloadElement.QuerySelector("br + small").FirstChild.TextContent.CleanString();
                var fileSize = downloadElement.QuerySelector("br + small > i").TextContent.CleanString();
                var os = downloadElement.LastChild.TextContent.CleanString();

                // Grab the links with in each element
                //var links = downloadElement.QuerySelectorAll("a");
                var links = downloadElement.QuerySelectorAll("a").OfType<IHtmlAnchorElement>();

                downloads.Add(new DownloadDetails()
                {
                    Title = fileName,
                    Size = fileSize,
                    Os = os,
                    Links = links.Select(l => new DownloadLink() { Text = l.Text, Url = l.Href }).ToArray()
                });

            }

            this.Downloads = downloads.ToArray();

        }



        public string Title { get; set; }
        public string Url { get; set; }

        public string Rating { get; set; }

        public string Category { get; set; }
        public string CategoryLink { get; set; }

        public string YearReleased { get; set; }
        public string YearReleasedLink { get; set; }

        public string Author { get; set; }
        public string AuthorLink { get; set; }
        public string Publisher { get; set; }
        public string PublisherLink { get; set; }



        public string[] Screenshots { get; set; }

        public string Description { get; set; }

        public string ManualUrl { get; set; }

        public DownloadDetails[] Downloads { get; set; }
    }

    
    public class DownloadDetails
    {
        public string Title { get; set; }
        public string Size { get; set; }
        public string Os { get; set; }

        public DownloadLink[] Links { get;set;}
    }

    public class DownloadLink
    {
        public string Text { get; set; }
        public string Url { get; set; }
    }
}
