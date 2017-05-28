using GopherServer.Core.Providers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GopherServer.Core.Results;
using GopherServer.Providers.MacintoshGarden.Results;
using GopherServer.Providers.MacintoshGarden.Models;
using GopherServer.Core.Routes;

namespace GopherServer.Providers.MacintoshGarden
{
    public class MacintoshGardenProvider : ServerProviderBase
    {
        public List<Route> Routes { get; private set; }

        public MacGardenController Controller { get; set; }

        public MacintoshGardenProvider(string hostname, int port) : base(hostname, port)
        {

        }

        

        public override void Init()
        {

            Controller = new MacintoshGarden.MacGardenController();

            this.Routes = new List<Route>()
            {
                // Proxy Download
                new TypedRoute<string>("Bin", "BIN:(.+)", Controller.DoDownload),

                // Search
                new TypedRoute<string>("Search", @"\/search\/*\t(.+)", Controller.Search),

                new TypedRoute<string>("Search", @"\/search\/(http://macintoshgarden.org\/.+)", Controller.SearchPage),

                // App Result
                new TypedRoute<string>("App", @"\/app\/(.+)", Controller.ShowApp),

                // Screenshot
                PrebuiltRoutes.GifRoute(),

              

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
                    return Controller.ShowHome();

                // Check our routes
                var route = Routes.FirstOrDefault(r => r.IsMatch(selector));

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
