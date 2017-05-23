using GopherServer.Core.GopherResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GopherServer.Providers.Routes
{
    public class TypedRoute<T> : Route
    {
        public TypedRoute(string name, string pattern, Func<T, BaseResult> action)
        {
            this.Name = name;
            this.RegexString = pattern;
            this.TypedAction = action;

            this.BuildRegex(pattern);
        }


        public T GetValue(string selector)
        {
            var v = Regex.Match(selector).Groups[1].Value;
            return (T)Convert.ChangeType(v, typeof(T));
        }


        public Func<T, BaseResult> TypedAction { get; set; }

        public override BaseResult Execute(string selector)
        {
            var value = this.GetValue(selector);
            return TypedAction(value);
        }
    }
}
