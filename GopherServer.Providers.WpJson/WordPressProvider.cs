using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GopherServer.Core.GopherResults;
using System.Text.RegularExpressions;
using GopherServer.Core.Providers;
using GopherServer.Providers.Routes;
using System.Configuration;

namespace GopherServer.Providers.WpJson
{
    /// <summary>
    /// Provides a Gopher Provider to the Wordpress REST API
    /// </summary>
    public class WordPressProvider : ServerProviderBase
    {
        internal WordPressClient client;

        // The routes we're going to use later to perform actions
        private List<Route> routes;

        public string Hostname { get; private set; }
        public int Port { get; private set; }
        public string WordPressUrl { get; private set; }

        public WordPressProvider(string hostname, int port) : base(hostname, port)
        {
            this.Hostname = hostname;
            this.Port = port;
        }

        public override void Init()
        {
            this.WordPressUrl = ConfigurationManager.AppSettings[this.GetType().Name + ".Url"];

            // TODO Read in Config
            client = new WordPressClient(this.WordPressUrl);

            // Build our route list for teh selector
            routes = new List<Route>()
            {
                // Get Post
                new TypedRoute<int>("Posts", @"\/posts\/(\d+)", client.GetPost),

                // Get Post as Text
                new TypedRoute<int>("Posts", @"\/posts\/text\/(\d+)", client.GetPostText),

                // Get Image
                new TypedRoute<string>("Gif", @"\/gif\/(.+)", client.GetGif),

                // Categories List
                new Route("Categories", @"\/categories\/", client.GetCategories),

                // Posts by Category
                new TypedRoute<int>("CategoryPosts", @"\/category\/(\d+)", client.GetCategoryPosts),
                
                // Search
                new TypedRoute<string>("Search", @"\/search\/*\t(.+)", client.Search),

                // External Link
                new TypedRoute<string>("Url", @"URL:(.+)", client.Redirect),

                // Proxy Link
                new TypedRoute<string>("Proxy", @"\/proxy\/(.+)", client.ProxyPage),

            };
        }

       
        /// <summary>
        /// Processes the selector and performs the appropriate action
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        public override BaseResult GetResult(string selector)
        {
            // This is where we read our selectors...
            // it's a shame we can't reuse the route code out of MVC (or can we ?)

            try
            {
                if (string.IsNullOrEmpty(selector) || selector == "1") // some clients seem to use 1
                    return client.GetHomePage();

                // Check our routes
                var route = routes.FirstOrDefault(r => r.IsMatch(selector));

                if (route == null)
                    return new ErrorResult("Selector '" + selector + "' was not found/is not supported.");
                else
                    return route.Execute(selector);
            }
            catch (Exception ex)
            {
                // TODO: Some kind of common logging?
                Console.WriteLine(ex);
                return new ErrorResult("Error occurred processing your request.");
            }      
        }

        
    }
}
