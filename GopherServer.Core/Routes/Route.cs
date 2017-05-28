using GopherServer.Core.Results;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GopherServer.Core.Routes
{
    /// <summary>
    /// Helper class to assist with providers handling selectors
    /// </summary>
    public class Route
    {
        public Route() { }

        /// <summary>
        /// Create a route
        /// </summary>
        /// <param name="name">Name of route</param>
        /// <param name="pattern">Regex partern for matching selectors</param>
        /// <param name="action">Action to perform if this route is found</param>
        public Route(string name, string pattern, Func<BaseResult> action)
        {
            this.Name = name;
            this.RegexString = pattern;
            this.Action = action;

            this.BuildRegex(pattern);
        }

        internal void BuildRegex(string pattern)
        {
            this.Regex = new Regex(pattern);
        }

        internal Regex Regex { get; set; }

        public string Name { get; set; }

        public string RegexString { get; set; }

        public bool IsMatch(string selector)
        {
            return Regex.IsMatch(selector);
        }

        public Func<BaseResult> Action { get; set; }

        public virtual BaseResult Execute(string selector)
        {
            return Action();
        }
    }
}
