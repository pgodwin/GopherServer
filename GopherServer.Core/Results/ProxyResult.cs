using System;
using System.IO;

namespace GopherServer.Core.Results
{
    public class ProxyResult : BaseResult
    {
        public ProxyResult(string url) : base()
        {
            this.Url = url;
        }

        public ProxyResult(string url, string referrer)
        {
            this.Url = url;
            this.Referrer = referrer;
        }

        public string Referrer { get; private set; }
        public string Url { get; private set; }

        public override void WriteResult(Stream stream)
        {
            System.Net.HttpWebRequest ProxyRequest = (System.Net.HttpWebRequest)System.Net.HttpWebRequest.Create(Url);
            System.Net.WebResponse ServerResponse = null;
            if (!string.IsNullOrEmpty(this.Referrer))
                ProxyRequest.Referer = this.Referrer;

            /* Send the proxy request to the remote server or fail. */
            try
            {
                ServerResponse = ProxyRequest.GetResponse();
            }
            catch (System.Net.WebException WebEx)
            {
                new ErrorResult("Error: " + WebEx.ToString()).WriteResult(stream);
            }

            if (ServerResponse != null)
            {
                using (var instream = ServerResponse.GetResponseStream())
                {
                    try
                    {
                        // Copy with 512K Buffer
                        instream.CopyTo(stream, 524288);
                    }
                    catch (System.IO.IOException ioException)
                    {
                        // User probably canncelled
                        Console.WriteLine("IOException: {0}", ioException);
                    }
                    finally
                    {
                        instream.Close();
                    }
                }
            }           
        }
    }
}
