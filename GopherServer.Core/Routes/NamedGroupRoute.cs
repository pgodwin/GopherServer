using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using GopherServer.Core.Helpers;
using GopherServer.Core.Results;

namespace GopherServer.Core.Routes
{
    public class NamedGroupRoute : Route
    {
        public NamedGroupRoute(string name, string pattern, Delegate resultMethod)
        {
            this.Name = name;
            this.RegexString = pattern;
            this.Delegate = resultMethod;
            this.BuildRegex(pattern);
        }

        protected Delegate Delegate { get; set; }

        /// <summary>
        /// Returns a dictionary of groups and their values for the route regex on the specified selector.
        /// </summary>
        /// <param name="selector"></param>
        /// <returns></returns>
        private Dictionary<string, string> GetValues(string selector)
        {
            var groupValues = new Dictionary<string, string>();
            // Get the groupnames
            var groups = Regex.Match(selector).Groups;
            foreach (var groupName in Regex.GetGroupNames())
                groupValues.Add(groupName, groups[groupName].Value);

            return groupValues;
        }

        public override BaseResult Execute(string selector)
        {
            var methodParams = this.Delegate.Method.GetParameters();

            // Build our arguments
            var selectorValues = this.GetValues(selector);

            var args = methodParams.Select(p => selectorValues[p.Name].ToType(p.ParameterType)).ToArray();
           
            return (BaseResult)this.Delegate.DynamicInvoke(args);
        }
    }
}
