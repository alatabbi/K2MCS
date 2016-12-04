using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace K2Server
{
    class Program
    {
        public static void Main(string[] args)
        {

            try
            {
                int memoreSize = 1000;
                if (args != null && args.Length > 1)
                {
                    memoreSize = int.Parse(args[0]);
                }

                if (memoreSize == 0)
                {
                    System.Console.WriteLine("Please enter a numeric argument.");
                    return;
                }

                K2CacheServer cacheServer = new K2CacheServer(memoreSize);
                K2Host host = new K2Host(cacheServer);
                host.IpAddress = "127.0.0.1";
                host.Port = "3000";
                var task = Task.Run(async () => { await host.StartAsync(); });
                task.Wait();
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.Message);
            }
        }
    }
}
