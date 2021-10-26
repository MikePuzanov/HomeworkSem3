using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MyFTP
{
    /// <summary>
    /// класс сервера для приема запросов от клиента
    /// </summary>
    public class Server
    {
        private int _port;
        private readonly TcpListener _listener;
        private readonly CancellationTokenSource _cancellationToken = new CancellationTokenSource();

        public Server(int port, string host)
        {
            _port = port;
            _listener = new TcpListener(IPAddress.Parse(host), port);
        }

        /// <summary>
        /// старт приема запросов
        /// </summary>
        public async Task StartServer()
        {
            _listener.Start();
            while (!_cancellationToken.IsCancellationRequested)
            {
                var client = await _listener.AcceptTcpClientAsync();
                Working(client);
            }

            _listener.Stop();
        }

        /// <summary>
        /// остановка приема запросов
        /// </summary>
        public void StopServer()
            => _cancellationToken.Cancel();

        /// <summary>
        /// метод для распределения запросов
        /// </summary>
        private async void Working(TcpClient client)
        {
            var stream = client.GetStream();
            var reader = new StreamReader(stream);
            var writer = new StreamWriter(stream);
            var request = await reader.ReadLineAsync();
            var (command, path) = (request?.Split()[0], request?.Split()[1]);
            switch (command)
            {
                case "1":
                    await List(writer, path);
                    break;
                case "2":
                    await Get(writer, path, stream);
                    break;
                default:
                    throw new ArgumentException();
            }
        }

        private async Task List(StreamWriter writer, string path)
        {
            if (!Directory.Exists(path))
            {
                await writer.WriteLineAsync("-1");
                await writer.FlushAsync();
            }

            var files = Directory.GetFiles(path);
            var directories = Directory.GetDirectories(path);
            var size = files.Length + directories.Length;

            string result = "";
            foreach (var file in files)
            {
                result += $" {file} false";
            }

            foreach (var dir in directories)
            {
                result += $" {dir} true";
            }

            await writer.WriteLineAsync(size.ToString() + result);
            await writer.FlushAsync();
        }

        private async Task Get(StreamWriter writer, string path, NetworkStream stream)
        {
            if (!File.Exists(path))
            {
                await writer.WriteLineAsync("-1");
                await writer.FlushAsync();
            }

            var file = new FileStream(path, FileMode.Open);
            await writer.WriteLineAsync($"{file.Length} ");
            await writer.FlushAsync();
            await file.CopyToAsync(stream);
            await writer.FlushAsync();
        }
    }
}