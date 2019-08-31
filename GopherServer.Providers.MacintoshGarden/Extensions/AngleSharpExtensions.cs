using AngleSharp.Dom;
using AngleSharp.Html.Dom;

namespace GopherServer.Providers.MacintoshGarden.Extensions
{
    public static class AngleSharpExtensions
    {
        public static string TryGetContent(this IElement element, string defaultText = "")
        {
            if (element == null)
                return defaultText;

            return element.TextContent ?? defaultText;
        }

        public static string TryGetHref(this IElement element, string defaultText = "")
        {
            var href = element as IHtmlAnchorElement;
            if (href == null)
                return defaultText;

            return href.Href ?? defaultText;
        }
    }
}
