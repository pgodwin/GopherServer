using System.Collections.Generic;
using System.Text;
using System.IO;

namespace GopherServer.Core.Results
{
    public class TextResult : BaseResult
    {
        public TextResult() { }

        public TextResult(string text)
        {
            this.Text = text;
        }

        public TextResult(List<string> text)
        {
            this.Text = string.Join("\r\n", text);
        }

        public string Text { get; set; }

        public override void WriteResult(Stream stream)
        {
            using (var streamWriter = new StreamWriter(stream, Encoding.ASCII))
            {
                streamWriter.Write(this.Text);
            }
        }
    }
}
