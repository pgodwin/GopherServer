using System;

namespace GopherServer.Core.Helpers
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Converts string to the specified type. Useful for regex matches!
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static T ToType<T>(this string value) => (T)Convert.ChangeType(value, typeof(T));

        public static object ToType(this string value, Type type) => Convert.ChangeType(value, type);
    }
}
