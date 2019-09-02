using System.IO;
using GopherServer.Core.Models;

namespace GopherServer.Core.Results
{
    public class ByteResult : BaseResult
    {
        public ByteResult()
        { }

        public ByteResult(byte[] resultBytes, ItemType type)
        {
            this.ItemType = type;
            this.ResultBytes = resultBytes;
        }

        public byte[] ResultBytes { get; set; }

        public override void WriteResult(Stream stream)
        {
            using (var writer = new BinaryWriter(stream))
            {
                writer.Write(this.ResultBytes);
            }
        }
    }
}
