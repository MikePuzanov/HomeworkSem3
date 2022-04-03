namespace MyFTPServer;

using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// класс сервера для приема запросов от клиента
/// </summary>
public class Server
{
    private readonly TcpListener _listener;
    private readonly CancellationTokenSource _tokenSource = new ();

    public Server(string host, int port)
    {
        _listener = new TcpListener(IPAddress.Parse(host), port);
    }

    /// <summary>
    /// старт приема запросов
    /// </summary>
    public async Task StartServer()
    {
        var task = new List<Task>();
        _listener.Start();
        while (!_tokenSource.IsCancellationRequested)
        {
            using var client = await _listener.AcceptTcpClientAsync();
            task.Add(Working(client));
        }
        await Task.WhenAll(task);
        _listener.Stop();
    }

    /// <summary>
    /// остановка приема запросов
    /// </summary>
    public void StopServer()
        => _tokenSource.Cancel();

    /// <summary>
    /// метод для распределения запросов
    /// </summary>
    private async Task Working(TcpClient client)
    {
        using var stream = client.GetStream();
        using var reader = new StreamReader(stream);
        using var writer = new StreamWriter(stream);
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
            case "!exit":
                StopServer();
            break;
            default:
                await writer.WriteAsync("Ваш протокол сломан!");
            break;
        }
    }

    private async Task List(StreamWriter writer, string path)
    {
        if (!Directory.Exists(path))
        {
            await writer.WriteLineAsync("-1");
            return;
        }
            
        var files = Directory.GetFiles(path);
        var directories = Directory.GetDirectories(path);
        var size = files.Length + directories.Length;
        var result = new StringBuilder();
        result.Append(size).ToString();
        foreach (var file in files)
        {
            result.Append($" {file} false");
        }

        foreach (var dir in directories)
        {
            result.Append($" {dir} true");
        }

        await writer.WriteLineAsync(size.ToString() + result);
        await writer.FlushAsync();
    }

    private async Task Get(StreamWriter writer, string path, NetworkStream stream)
    {
        if (!File.Exists(path))
        {
            await writer.WriteLineAsync("-1");
            return;
        }

        var file = new FileStream(path, FileMode.Open);
        await writer.WriteLineAsync($"{file.Length} ");
        await writer.FlushAsync();
        await file.CopyToAsync(stream);
        await writer.FlushAsync();
    }
}