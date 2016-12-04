using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace K2Server
{
    public interface ICacheServer
    {
        int NumberOfItem { get; }
        string Get(string key);
        string Put(string key, string value);
        Message Perform(Message m);
    }
}
