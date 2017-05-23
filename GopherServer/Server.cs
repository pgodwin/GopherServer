using GopherServer.Core.Configuration;
using GopherServer.Core.GopherResults;
using GopherServer.Core.Providers;
using GopherServer.Providers;
using GopherServer.Providers.WpJson;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GopherServer.Server
{
    public class StateObject
    {
        // Client  socket.  
        public Socket workSocket = null;
        // Size of receive buffer.  
        public const int BufferSize = 1024;
        // Receive buffer.  
        public byte[] buffer = new byte[BufferSize];
        // Received data string.  
        public StringBuilder sb = new StringBuilder();
    }

    public class Server
    {
        // Thread signal.  
        public static ManualResetEvent allDone = new ManualResetEvent(false);

        public IServerProvider provider;

        public Server()
        {
            
            this.IPAddress = IPAddress.Parse(ServerSettings.BoundIP);
            this.Port = ServerSettings.BoundPort;
            this.ExternalHostname = ServerSettings.PublicHostname;
            this.ExternalPort = ServerSettings.PublicPort;

            provider = ProviderFactory.GetProvider(this.ExternalHostname, this.ExternalPort);

            provider.Init();
        }

        public IPAddress IPAddress { get; private set; }

        public int Port { get; private set; }

        public string ExternalHostname { get; set; }

        public int ExternalPort { get; set; }

        public void StartListening()
        {
            // Data buffer for incoming data.  
            byte[] bytes = new Byte[1024];

            // Establish the local endpoint for the socket.  
            // The DNS name of the computer  
            // running the listener is "host.contoso.com".  
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            IPAddress ipAddress = this.IPAddress;
            IPEndPoint localEndPoint = new IPEndPoint(ipAddress, 70);
            Console.WriteLine("Listening on {0}:70", ipAddress.ToString());

            // Create a TCP/IP socket.  
            Socket listener = new Socket(AddressFamily.InterNetwork,
                SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    // Set the event to nonsignaled state.  
                    allDone.Reset();

                    // Start an asynchronous socket to listen for connections.  
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(
                        new AsyncCallback(AcceptCallback),
                        listener);

                    // Wait until a connection is made before continuing.  
                    allDone.WaitOne();
                }

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            Console.WriteLine("\nPress ENTER to continue...");
            Console.Read();

        }

        public void AcceptCallback(IAsyncResult ar)
        {
            // Signal the main thread to continue.  
            allDone.Set();

            // Get the socket that handles the client request.  
            Socket listener = (Socket)ar.AsyncState;
            Socket handler = listener.EndAccept(ar);

            // Create the state object.  
            StateObject state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                new AsyncCallback(ReadCallback), state);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            String selector = String.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            StateObject state = (StateObject)ar.AsyncState;
            Socket handler = state.workSocket;

            // Read data from the client socket.   
            int bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(
                    state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read   
                // more data.  
                selector = state.sb.ToString();
                if (selector.IndexOf("\r\n") > -1)
                {
                    // All the data has been read from the   
                    // client. Display it on the console.  
                    Console.WriteLine("Read {0} bytes from socket. \n Data : {1}",
                        selector.Length, selector);
                    
                    // Trim the trailng CRLF
                    selector = selector.TrimEnd('\r', '\n');

                    // Tell the provider to return a result for the selector
                    var result  = provider.GetResult(selector);

                    // This is a bit of a hack - I need a beeter 
                    if (result is DirectoryResult)
                    {
                        var dir = result as DirectoryResult;
                        foreach (var item in dir.Items)
                        {
                            if (string.IsNullOrEmpty(item.Host))
                            {
                                item.Host = this.ExternalHostname;
                                item.Port = this.Port;
                            }
                        }
                    }

                    using (var stream = new MemoryStream())
                    {
                        result.WriteResult(stream);
                        Send(handler, stream.ToArray());
                    }
                    
                    
                }   
                else
                {
                    // Not all data received. Get more.  
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0,
                    new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            byte[] byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0,
                new AsyncCallback(SendCallback), handler);

        }

        private static void Send(Socket handler, byte[] data)
        {
            

            // Begin sending the data to the remote device.  
            handler.BeginSend(data, 0, data.Length, 0,
                new AsyncCallback(SendCallback), handler);

        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                Socket handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                int bytesSent = handler.EndSend(ar);
                Console.WriteLine("Sent {0} bytes to client.", bytesSent);

                handler.Shutdown(SocketShutdown.Both);
                handler.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
        }

      
    }
}
