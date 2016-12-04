using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Server
{
    public class K2CacheServer : ICacheServer
    {
        public string Port { get; set; }

        public string IpAddress { get; set; }

        readonly Dictionary<string, string> _memory;

        readonly Queue<string> _queue;

        public K2CacheServer(int memorySize)
        {
            MemorySize = memorySize;
            _memory = new Dictionary<string, string>();
            this._queue = new Queue<string>();
        }

        public int MemorySize { get; private set; }

        public int NumberOfItem => _memory.Count;

        public string Put(string key, string value)
        {
            try
            {
                lock (_memory)
                {
                    if (_memory.ContainsKey((key)))
                    {
                        _memory[key] = value;
                    }
                    else if (_memory.Count < this.MemorySize)
                    {
                        _memory.Add(key, value);
                        this._queue.Enqueue(key);
                    }
                    else
                    {
                        string oldkey = _queue.Peek();
                        _queue.Dequeue();
                        _memory.Remove(oldkey);
                        _queue.Enqueue(key);
                        _memory.Add(key, value);
                    }
                }
                return Results.OK.ToString();
            }
            catch (Exception)
            {
                return Results.FAILD.ToString();
            }
        }

        public string Get(string key)
        {
            lock (_memory)
            {
                if (_memory.ContainsKey(key))
                    return _memory[key];
                return "";
            }
        }

        public Message Perform(Message m)
        {
            if (!m.IsValid())
            {
                m.Result = Results.FAILD.ToString();
                return m;
            }

            if (m.CommandName == CommandNames.GET.ToString())
            {
                var r = this.Get(m.Key);
                m.Result = string.IsNullOrEmpty(r) ? Results.FAILD.ToString() : Results.OK.ToString();
                m.Value = r;
            }
            else if (m.CommandName == CommandNames.PUT.ToString())
            {
                var r = this.Put(m.Key, m.Value);
                m.Result = r;
            }
            return m;
        }

    }
}
