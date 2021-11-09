using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Test1._1
{
    /// <summary>
    /// класс клиента
    /// </summary>
    public class Client
    {
        private TcpClient _client;
        
        public Client(string host, int port)
        {
            _client = new TcpClient(host, port);
        }

        public void Working()
        {
            while (true)
            {
                Task.Run(async () =>
                {
                    while (true) {
                        await Writer(_client.GetStream());
                        await Reader(_client.GetStream());
                    }
                });
            }
        }
        
        private async Task Writer(NetworkStream stream)
        {
            await Task.Run(async () =>
            {
                var writer = new StreamWriter(stream) { AutoFlush = true };
                while (true)
                {
                    var data = Console.ReadLine();
                    await writer.WriteAsync(data + "\n");
                }
            });
        }
        
        private async Task Reader(NetworkStream stream)
        {
            await Task.Run(async () =>
            {
                var reader = new StreamReader(stream);
                while (true)
                {
                    var data = await reader.ReadLineAsync();
                    Console.WriteLine(data + "\n");
                }
            });
        }
    }
}