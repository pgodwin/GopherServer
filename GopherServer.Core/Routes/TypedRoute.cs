using System;
using GopherServer.Core.Results;

namespace GopherServer.Core.Routes
{
    /// <summary>
    /// For Regex patterns which returns a *single* group
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TypedRoute<T> : Route
    {
        public TypedRoute(string name, string pattern, Func<T, BaseResult> action)
        {
            this.Name = name;
            this.RegexString = pattern;
            this.TypedAction = action;
            this.BuildRegex(pattern);
        }

        public Func<T, BaseResult> TypedAction { get; set; }

        public T GetValue(string selector)
        {
            var v = Regex.Match(selector).Groups[1].Value;
            return (T)Convert.ChangeType(v, typeof(T));
        }

        public override BaseResult Execute(string selector)
        {
            var value = this.GetValue(selector);
            return TypedAction(value);
        }
    }
}
