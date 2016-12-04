using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace K2ClientApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var task = Task.Run(async () => { await ConnectAsync("127.0.0.1", "3000"); });
            task.Wait();

        }

        static async Task ConnectAsync(String server, string port)
        {
            try
            {
                TcpClient client = new TcpClient(server, int.Parse(port));
                NetworkStream stream = client.GetStream();
                string message = Console.ReadLine();
                while (message != "bye")
                {
                    if (string.IsNullOrEmpty(message))
                        continue;
                    byte[] data = System.Text.Encoding.ASCII.GetBytes(message);
                    stream.Write(data, 0, data.Length);
                    Console.WriteLine("Sent: {0}", message);
                    data = new byte[256];
                    string res = "";
                    int bytes = stream.Read(data, 0, data.Length);
                    res = System.Text.Encoding.ASCII.GetString(data, 0, bytes);
                    Console.WriteLine("Received: {0}", res);
                    message = Console.ReadLine();

                }
                stream.Close();
                client.Close();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {
                Console.WriteLine("Press Enter to continue...");
                Console.Read();
            }


        }

    }
}
