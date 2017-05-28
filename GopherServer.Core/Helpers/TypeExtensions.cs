using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        public static T ToType<T>(this string value)
        {
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public static object ToType(this string value, Type type)
        {
            return Convert.ChangeType(value, type);
        }
    }
}
