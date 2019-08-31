using System;
using System.Text.RegularExpressions;
using GopherServer.Core.Results;

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

        internal Regex Regex { get; set; }
        public string Name { get; set; }
        public string RegexString { get; set; }
        public Func<BaseResult> Action { get; set; }

        internal void BuildRegex(string pattern) => this.Regex = new Regex(pattern);
        public bool IsMatch(string selector) => Regex.IsMatch(selector);
        public virtual BaseResult Execute(string selector) => Action();
    }
}
