using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace MyFTP
{
    /// <summary>
    /// класс клиента для общения с сервером
    /// </summary>
    public class Client
    {
        private readonly int _port;
        private readonly string _host;

        public Client(int port, string host)
        {
            _port = port;
            _host = host;
        }

        /// <summary>
        /// запрос на листинг файлов в папке по пути
        /// </summary>
        public async Task<(string name, bool isDir)[]> List(string path)
        { 
            var client = new TcpClient(_host, _port);
            using var stream = client.GetStream();
            var writer = new StreamWriter(stream);
            await writer.WriteLineAsync($"1 {path}");
            await writer.FlushAsync();
            var reader = new StreamReader(stream);
            var info = await reader.ReadLineAsync();
            var infoArray = info.Split(' ');
            var size = Convert.ToInt32(infoArray[0]);
            if (size == -1)
            {
                return null;
            }

            var data = new (string name, bool isDir)[size];

            for (int i = 1; i < infoArray.Length; i += 2)
            {
                var name = infoArray[i];
                var isDir = Convert.ToBoolean(infoArray[i + 1]);
                data[(i - 1) / 2] = (name, isDir);
            }

            return data;
        }

        /// <summary>
        /// запрос на скачивание нужного файла
        /// </summary>
        public async Task<(long size, byte[] content)> Get(string path, Stream fileStream)
        {
            var client = new TcpClient(_host, _port);
            using var stream = client.GetStream();
            var writer = new StreamWriter(stream);
            await writer.WriteLineAsync($"2 {path}");
            await writer.FlushAsync();
            var reader = new StreamReader(stream);
            var size = Convert.ToInt32(await reader.ReadLineAsync());
            if (size == -1)
            {
                throw new FileNotFoundException();
            }

            var content = new byte[size];
            await reader.BaseStream.ReadAsync(content, 0, size);
            fileStream.Position = 0;
            return (size, content);
        }
    }
}