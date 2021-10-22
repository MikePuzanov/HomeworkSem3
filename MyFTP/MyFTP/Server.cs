using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;

namespace MyFTP
{
    public class Server
    {
        private int _port;
        private readonly TcpListener _listener;
        private CancellationTokenSource _cancellationToken;

        Server(int port, string host, CancellationTokenSource cancellationToken)
        {
            _port = port;
            _listener = new TcpListener(IPAddress.Parse(host), port);
        }

        public async Task StartServer()
        {
            _listener.Start();
            while (_cancellationToken.IsCancellationRequested)
            {
                var client = await _listener.AcceptTcpClientAsync();
                await Task.Run(() => Working(client));
            }
        }

        public void StopServer()
             => _cancellationToken.Cancel();

        public async Task Working(TcpClient client)
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
                    await Get(writer, path); 
                break;
                default:
                    throw new ArgumentException();
            }
        }
        
        public async Task List(StreamWriter writer,string path)
        {
            if (!Directory.Exists(path))
            {
                await writer.WriteLineAsync("-1");
                return;
            }

            var files = Directory.GetFiles(path);
            var directories = Directory.GetDirectories(path);
            var size = files.Length + directories.Length;
            await writer.WriteAsync("{size}");

            foreach (var file in files)
            {
                await writer.WriteAsync($" {file} false");
            }
            
            foreach (var directory in directories)
            {
                await writer.WriteAsync($" {directory} true");
            }
        }

        public async Task Get(StreamWriter writer,string path)
        {
            if (!File.Exists(path))
            {
                await writer.WriteLineAsync("-1");
                return;
            }

            var size = new FileInfo(path).Length;
            await writer.WriteAsync($"{size}");
            /// write file
        }
    }
}