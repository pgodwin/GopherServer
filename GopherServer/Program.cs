namespace GopherServer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var server = new Server();
            server.StartListening();
        }
    }
}
