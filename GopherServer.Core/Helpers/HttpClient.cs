using System.Net.Http;

namespace GopherServer.Core.Helpers
{
    public static class HttpHelpers
    {
        private static HttpClient client = new HttpClient();

        public static byte[] DownloadFile(string url) => client.GetByteArrayAsync(url).Result;

        public static string GetUrl(string url) => System.Text.Encoding.UTF8.GetString(client.GetByteArrayAsync(url).Result);
            // return client.GetStringAsync(url).Result;
    }
}
