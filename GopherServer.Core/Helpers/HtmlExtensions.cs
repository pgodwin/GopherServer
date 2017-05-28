using GopherServer.Core.Results;
using GopherServer.Core.Helpers;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace GopherServer.Core.Helpers
{
    public static class HtmlExtensions
    {
        /// <summary>
        /// Decodes the HTML into a standard string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        public static string CleanHtml(this string text)
        {
            return WebUtility.HtmlDecode(text);
        }

        /// <summary>
        /// Converts HTML into text
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static string HtmlToText(this string html)
        {
            //var htmlDoc = new HtmlDocument();
            //htmlDoc.LoadHtml(html);
            //return htmlDoc.DocumentNode.InnerText;

            var htmlConvert = new HtmlToText();
            return htmlConvert.ConvertHtml(html);

        }

        /// <summary>
        /// Converst the specified HTML in to a list of directory items
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public static List<DirectoryItem> ToDirectoryItems(this string html, 
            string urlSelector = "URL:", 
            string proxySelector = "PROXY:", 
            string gifSelector = "GIF:")
        {
            // We're going to use the HTML agility pack to parse out the HTML into info
            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var nodes = htmlDoc.DocumentNode.ChildNodes;

            return nodes.SelectMany(n=>n.ToDirectoryItems(urlSelector, proxySelector,gifSelector)).ToList();
        }

        /// <summary>
        /// Turns a HTML node into Directory items (links, images, text)
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        private static List<DirectoryItem> ToDirectoryItems(this HtmlNode node,
            string urlSelector = "URL:", 
            string proxySelector = "PROXY:", 
            string gifSelector = "GIF:")
        {
            var items = new List<DirectoryItem>();

            var item = new DirectoryItem();
            if (node.Name == "a")
            {
                // Link
                if (!string.IsNullOrEmpty(node.InnerText))
                {
                    var httpLink = new DirectoryItem();
                    httpLink.ItemType = ItemType.HTML;
                    httpLink.Description = node.InnerText.CleanHtml() + " (HTTP)";
                    httpLink.Selector = urlSelector + node.GetAttributeValue("href", "");
                    items.Add(httpLink);

                    var gopherLink = new DirectoryItem();
                    gopherLink.ItemType = ItemType.DIRECTORY;
                    gopherLink.Description = node.InnerText.CleanHtml() + " (Gopher Proxy)";
                    gopherLink.Selector = proxySelector + node.GetAttributeValue("href", "");
                    items.Add(gopherLink);
                }
            }
            else if (node.Name == "img")
            {
                // Image
                item.ItemType = ItemType.GIF;
                item.Description = node.GetAttributeValue("alt", null) ?? "Image";
                item.Selector = gifSelector + node.GetAttributeValue("src", "");
                items.Add(item);
            }
            else if (node.NodeType == HtmlNodeType.Text && node.ParentNode.Name != "a")
            {
                var text = node.InnerText.CleanHtml();
                items.AddRange(text.WrapToDirectoryItems(80));
                // items.Add(new DirectoryItem(" "));
            }

            if (node.HasChildNodes)
            {
                items.AddRange(node.ChildNodes.SelectMany(n=>n.ToDirectoryItems(urlSelector, proxySelector, gifSelector)));
            }
            return items;
        }

        public static string GetTextFromXPath(this HtmlNode node, string xPath)
        {
            return node.SelectSingleNode(xPath).InnerText;
        } 

        public static string GetUrlFromXPath(this HtmlNode node, string xPath)
        {
            return node.SelectSingleNode(xPath).Attributes["href"].Value;
        }

    }

    
}
