using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class Client
    {
        private readonly int _port;
        private readonly string _host;

        public Client(int port, string host)
        {
            _port = port;
            _host = host;
        }

        public async Task<(string name, bool isDir)[]> List(string path)
        {
            using var client = new TcpClient(_host, _port);
            await using var stream = client.GetStream();
            var writer = new StreamWriter(stream) {AutoFlush = true};
            await writer.WriteLineAsync($"1 {path}");
            var reader = new StreamReader(stream);
            var size = Convert.ToInt32(await reader.ReadLineAsync());

            if (size == -1)
            {
                return null;
            }

            var data = new (string name, bool isDir)[size];

            for (int i = 0; i < size; i++)
            {
                var name = await reader.ReadLineAsync();
                var isDir = Convert.ToBoolean(await reader.ReadLineAsync());
                data[i] = (name, isDir);
            }

            return data;
        }

        public async Task Get(string path, Stream fileStream)
        {
            var client = new TcpClient(_host, _port);
            await using var stream = client.GetStream();
            var writer = new StreamWriter(stream) {AutoFlush = true};
            await writer.WriteLineAsync($"2 {path}");
            var reader = new StreamReader(stream);
            var size = Convert.ToInt32(await reader.ReadLineAsync());
            if (size == -1)
            {
                throw new FileNotFoundException();
            }

            var content = new byte[size];
            await reader.BaseStream.ReadAsync(content, 0, size);
            await stream.CopyToAsync(fileStream);
            fileStream.Position = 0;
        }
    }
}