using System;
using System.Text;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Net.NetworkInformation;

namespace AvalonLogViewer
{
    public class GetSocket
    {
        public static Socket ConnectSocket(string server, int port)
        {
            Socket s = null;
            IPHostEntry hostEntry = null;

            // Loop through the AddressList to obtain the supported AddressFamily. This is to avoid
            // an exception that occurs when the host IP Address is not compatible with the address family
            // (typical in the IPv6 case).
            try
            {

                Socket tempSocket = new Socket(SocketType.Stream, ProtocolType.Tcp);

                tempSocket.Connect(server, port);

                if (tempSocket.Connected)
                {
                    s = tempSocket;
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }
            catch (ArgumentNullException e)
            {
                Console.WriteLine("ArgumentNullException caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }
            catch (NullReferenceException e)
            {
                Console.WriteLine("NullReferenceException caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception caught!!!");
                Console.WriteLine("Source : " + e.Source);
                Console.WriteLine("Message : " + e.Message);
            }
            return s;
        }


        // This method requests the home page content for the specified server.
        public static string SocketSendReceive(string server, int port, string command = "estats")
        {
            if (ping(server))
            { 
                Byte[] bytesSent = Encoding.ASCII.GetBytes(command);
                Byte[] bytesReceived = new Byte[256];
                string page = "";

                // Create a socket connection with the specified server and port.
                using (Socket s = ConnectSocket(server, port))
                {

                    if (s == null)
                        return ("Connection failed");

                    s.NoDelay = true;

                    // Set the receive buffer size to 8k
                    //s.ReceiveBufferSize = 8192;

                    // Set the timeout for synchronous receive methods to
                    // 1 second (1000 milliseconds.)
                    s.ReceiveTimeout = 3000;

                    // Set the send buffer size to 8k.
                    //s.SendBufferSize = 8192;

                    // Set the timeout for synchronous send methods
                    // to 1 second (1000 milliseconds.)
                    s.SendTimeout = 3000;
                    // Send request to the server.
                    s.Send(bytesSent, bytesSent.Length, 0);

                    // Receive the server home page content.
                    int bytes = 0;
                    page = "";

                    // The following will block until the page is transmitted.
                    do
                    {
                        bytes = s.Receive(bytesReceived, bytesReceived.Length, 0);
                        page = page + Encoding.ASCII.GetString(bytesReceived, 0, bytes);
                    }
                    while (bytes > 0);
                    s.Close();
                }

                return page;
            }
            return null;
        }

        public static bool ping(string args)
        {
            Ping pingSender = new Ping();
            PingOptions options = new PingOptions();

            // Use the default Ttl value which is 128,
            // but change the fragmentation behavior.
            options.DontFragment = true;

            // Create a buffer of 32 bytes of data to be transmitted.
            string data = "aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa";
            byte[] buffer = Encoding.ASCII.GetBytes(data);
            int timeout = 500;
            PingReply reply = pingSender.Send(args, timeout, buffer, options);
            if (reply.Status == IPStatus.Success)
            {
                System.Diagnostics.Debug.WriteLine("Address: {0}", reply.Address.ToString());
                System.Diagnostics.Debug.WriteLine("RoundTrip time: {0}", reply.RoundtripTime);
                System.Diagnostics.Debug.WriteLine("Time to live: {0}", reply.Options.Ttl);
                System.Diagnostics.Debug.WriteLine("Don't fragment: {0}", reply.Options.DontFragment);
                System.Diagnostics.Debug.WriteLine("Buffer size: {0}", reply.Buffer.Length);
                return true;
            }
            else
                return false;
        }
    }
}
