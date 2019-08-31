using GopherServer.Core.Results;

namespace GopherServer.Core.Routes
{
    /// <summary>
    /// Contains pre-built routes you can use directly in your Provider
    /// </summary>
    public static class PrebuiltRoutes
    {
        /// <summary>
        /// Returns the client a link to the specfied URL
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public static TypedRoute<string> UrlResult() => new TypedRoute<string>("Url", @"URL:(.+)", url => new UrlResult(url));

        public static TypedRoute<string> GifRoute() => new TypedRoute<string>("Gif", @"GIF:(.+)", url => new GifResult(url));

        public static TypedRoute<string> HtmlProxy() => new TypedRoute<string>("Html", @"HTML:(.+)", url => new HtmlResult(url));

        public static TypedRoute<string> ProxyRoute() => new TypedRoute<string>("Proxy", @"PROXY:(.+)", url => new ProxyResult(url));
    }
}
