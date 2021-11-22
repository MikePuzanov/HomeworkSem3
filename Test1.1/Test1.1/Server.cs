using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace Test1._1
{
    /// <summary>
    /// класс серва
    /// </summary>
    public class Server
    {
        private int _port;
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();
        public Server(int port)
        {
            _port = port;
        }

        /// <summary>
        /// метод в котором происходит отправка и принятие сообщение
        /// </summary>
        public async Task Working()
        {
            var listener = new TcpListener(IPAddress.Any, _port);
            listener.Start();
            var client = await listener.AcceptTcpClientAsync();
            while (!_cancellationToken.IsCancellationRequested)
            {
                await Task.Run(async () =>
                {
                    while (true) {
                        await Writer(client.GetStream());
                        await Reader(client.GetStream());
                    }
                });
            }
            listener.Stop();
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
                    if (data == "exit")
                    {
                        StopServer();
                    }
                }
            });
        }
        
        private void StopServer()
            => _cancellationToken.Cancel();
    }
}