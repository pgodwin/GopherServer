using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using GopherServer.Core.Configuration;
using GopherServer.Core.Results;
using GopherServer.Core.Providers;

namespace GopherServer
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

        public IPAddress IPAddress { get; private set; }
        public int Port { get; private set; }
        public string ExternalHostname { get; set; }
        public int ExternalPort { get; set; }

        public Server()
        {
            IPAddress = IPAddress.Parse(ServerSettings.BoundIP);
            Port = ServerSettings.BoundPort;
            ExternalHostname = ServerSettings.PublicHostname;
            ExternalPort = ServerSettings.PublicPort;
            provider = ProviderFactory.GetProvider(this.ExternalHostname, this.ExternalPort);
            provider.Init();
        }

        public void StartListening()
        {
            // Data buffer for incoming data.  
            var bytes = new byte[1024];

            // Establish the local endpoint for the socket.  
            // The DNS name of the computer  
            // running the listener is "host.contoso.com".  
            //IPHostEntry ipHostInfo = Dns.Resolve(Dns.GetHostName());
            var ipAddress = IPAddress;
            var localEndPoint = new IPEndPoint(ipAddress, Port);

            Console.WriteLine($"Listening on {ipAddress}:{Port}");

            // Create a TCP/IP socket.  
            var listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind the socket to the local endpoint and listen for incoming connections.  
            try
            {
                listener.Bind(localEndPoint);
                listener.Listen(100);

                while (true)
                {
                    allDone.Reset(); // Set the event to nonsignaled state                    
                    Console.WriteLine("Waiting for a connection...");
                    listener.BeginAccept(new AsyncCallback(AcceptCallback), listener); // Start an asynchronous socket to listen for connections                     
                    allDone.WaitOne(); // Wait until a connection is made before continuing
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
            var listener = (Socket)ar.AsyncState;
            var handler = listener.EndAccept(ar);

            // Create the state object.  
            var state = new StateObject();
            state.workSocket = handler;
            handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
        }

        public void ReadCallback(IAsyncResult ar)
        {
            var selector = string.Empty;

            // Retrieve the state object and the handler socket  
            // from the asynchronous state object.  
            var state = (StateObject)ar.AsyncState;
            var handler = state.workSocket;

            // Read data from the client socket.   
            var bytesRead = handler.EndReceive(ar);

            if (bytesRead > 0)
            {
                // There  might be more data, so store the data received so far.  
                state.sb.Append(Encoding.ASCII.GetString(state.buffer, 0, bytesRead));

                // Check for end-of-file tag. If it is not there, read more data.  
                selector = state.sb.ToString();

                if (selector.IndexOf("\r\n") > -1)
                {
                    // All the data has been read from the client. Display it on the console.  
                    Console.WriteLine($"Read {selector.Length} bytes from socket.\nData: {selector}");
                    
                    // Trim the trailng CRLF
                    selector = selector.TrimEnd('\r', '\n');

                    // a whole bunch of legacy clients seem to be doing might strip that too
                    selector = selector.TrimStart('\r', '\n');

                    if (selector == ".") selector = "";

                    // Tell the provider to return a result for the selector
                    var result  = provider.GetResult(selector);

                    // This is a bit of a hack - I need a better way of doing this (push it back to the provider I think)
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

                    //using (var stream = new MemoryStream())
                    //{
                    //    result.WriteResult(stream);
                    //    Send(handler, stream.ToArray());
                    //}

                    WriteResult(handler, result);
                }   
                else
                {
                    // Not all data received. Get more.  
                    handler.BeginReceive(state.buffer, 0, StateObject.BufferSize, 0, new AsyncCallback(ReadCallback), state);
                }
            }
        }

        private static void Send(Socket handler, String data)
        {
            // Convert the string data to byte data using ASCII encoding.  
            var byteData = Encoding.ASCII.GetBytes(data);

            // Begin sending the data to the remote device.  
            handler.BeginSend(byteData, 0, byteData.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private static void Send(Socket handler, byte[] data)
        {
            // Begin sending the data to the remote device.  
            handler.BeginSend(data, 0, data.Length, 0, new AsyncCallback(SendCallback), handler);
        }

        private static void WriteResult(Socket handler, BaseResult result)
        {
            try
            {
                using (var netStream = new NetworkStream(handler))
                    result.WriteResult(netStream);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error writing result. {0}", ex);
            }
            finally
            {
                try
                {
                    handler.Shutdown(SocketShutdown.Both);
                    handler.Close();
                }
                catch (Exception handlerException)
                {
                    Console.WriteLine("Error attempting to close the handler: {0}", handlerException);
                }
            }
        }

        private static void SendCallback(IAsyncResult ar)
        {
            try
            {
                // Retrieve the socket from the state object.  
                var handler = (Socket)ar.AsyncState;

                // Complete sending the data to the remote device.  
                var bytesSent = handler.EndSend(ar);
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
