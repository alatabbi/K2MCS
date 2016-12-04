using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace K2Server
{
    public class K2Host : IK2Host
    {
        public string Port { get; set; }

        public string IpAddress { get; set; }

        readonly ICacheServer _cacheServer;

        public K2Host(ICacheServer cacheServer)
        {
            _cacheServer = cacheServer;
        }

        public async Task StartAsync()
        {
            try
            {
                Int32 port = int.Parse(Port);
                IPAddress ipAddr = IPAddress.Parse(this.IpAddress);
                TcpListener listener = new TcpListener(ipAddr, port);
                listener.Start();
                Console.WriteLine("waiting for a connection...");
                while (true)
                {
                    try
                    {
                        TcpClient client = await listener.AcceptTcpClientAsync();
                        Console.WriteLine("client connected.");
                        Task task = ChatAsync(client);
                        task.Wait();
                    }
                    catch (Exception ex)
                    {
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public async Task ChatAsync(TcpClient client)
        {
            Byte[] bytes = new Byte[256];
            string text = null;

            using (var stream = client.GetStream())
            {
                int length = 0;

                StringBuilder sb = new StringBuilder();


                while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    // convert data bytes to a ASCII string.
                    text = System.Text.Encoding.ASCII.GetString(bytes, 0, length);
                    Console.WriteLine("Received: {0}", text);
                    sb.AppendLine(text);


                    if (sb.ToString().Split('\n').Length > 3)
                        sb.Clear();
                    Message m = new Message(sb.ToString());
                    if (m.IsValid())
                    {
                        // we recived a valid message now process it 
                        m = _cacheServer.Perform(m);
                        byte[] msg = System.Text.Encoding.ASCII.GetBytes(m.ToString());
                        // send back a response.
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Sent: {0}", text);
                        sb.Clear();

                    }
                    else
                    {
                        byte[] res = System.Text.Encoding.ASCII.GetBytes(">");
                        // send back a acknowledgement.
                        stream.Write(res, 0, res.Length);
                        Console.WriteLine("Sent: {0}", ">");
                    }
                }
            }
        }
    }
}
