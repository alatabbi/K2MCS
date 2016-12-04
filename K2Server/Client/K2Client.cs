using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace K2Server.Client
{
    public class K2Client: IK2Client
    {
        public string IpAddress { get; set; }

        public string Port { get; set; }

        public K2Client(string ipAddress, string port)
        {
            IpAddress = ipAddress;
            Port = port;
        }

        private TcpClient Connect(string server, string port)
        {
            TcpClient client = new TcpClient(server, int.Parse(port));
            return client;
        }

        public string PutInCache(string key, string value)
        {
            return this.Send(string.Format("PUT\n{0}\n{1}", key, value));
        }

        public string GetFromCache(string key)
        {
            return this.Send(string.Format("GET\n{0}", key));
        }

        private string Send(string message)
        {
            using (TcpClient client = Connect(IpAddress, Port))
            {
                using (NetworkStream stream = client.GetStream())
                {
                    byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    data = new byte[256];
                    int bytes = stream.Read(data, 0, data.Length);
                    string res = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    return res;
                }
            }
        }
    }
}
