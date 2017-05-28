using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GopherServer.Core.Helpers
{
    public static class HttpHelpers
    {
        private static HttpClient client = new HttpClient();

        public static byte[] DownloadFile(string url)
        {
            return client.GetByteArrayAsync(url).Result;
        }

        public static string GetUrl(string url)
        {
            // return client.GetStringAsync(url).Result;
            return System.Text.Encoding.UTF8.GetString(client.GetByteArrayAsync(url).Result);
        }
    }
}
