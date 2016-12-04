using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace K2Server
{
    public interface IK2Host
    {
        string Port { get; set; }
        string IpAddress { get; set; }
        Task ChatAsync(TcpClient client);
        Task StartAsync();
    }
}
