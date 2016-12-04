using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Server.Client
{
    interface IK2Client
    {
        string Port { get; set; }
        string IpAddress { get; set; }
        string PutInCache(string key, string value);
        string GetFromCache(string key);
    }
}
