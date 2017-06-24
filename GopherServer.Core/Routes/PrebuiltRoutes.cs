using GopherServer.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static TypedRoute<string> UrlResult()
        {
            return new TypedRoute<string>("Url", @"URL:(.+)", (url)=> { return new UrlResult(url); });
        }

        public static TypedRoute<string> GifRoute()
        {
            return new TypedRoute<string>("Gif", @"GIF:(.+)", (url) => { return new GifResult(url); });
        }

        public static TypedRoute<string> HtmlProxy()
        {
            return new TypedRoute<string>("Html", @"HTML:(.+)", (url) => { return new HtmlResult(url); });
        }

        public static TypedRoute<string> ProxyRoute()
        {
            return new TypedRoute<string>("Proxy", @"PROXY:(.+)", (url) => { return new ProxyResult(url); });
        }
    }
}
