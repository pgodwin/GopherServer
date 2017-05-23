using GopherServer.Core.GopherResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace GopherServer.Core.GopherResults
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
