using System.Net.Sockets;
using System.Threading.Tasks;

/// <summary>
/// класс клиента для общения с сервером
/// </summary>
public class Client
{
    private readonly int _port;
    private readonly string _host;

    public Client(string host, int port )
    {
        _host = host;
        _port = port;
    }

    /// <summary>
    /// запрос на листинг файлов в папке по пути
    /// </summary>
    public async Task<(string name, bool isDir)[]> List(string path, CancellationToken cancellationToken)
    {
        using var client = new TcpClient();
        await client.ConnectAsync(_host, _port, cancellationToken);
        await using var stream = client.GetStream();
        await using var writer = new StreamWriter(stream);
        using var reader = new StreamReader(stream);
        await writer.WriteLineAsync($"1 {path}");
        await writer.FlushAsync();
        var info = await reader.ReadLineAsync();
        var infoArray = info.Split(' ');
        var size = Convert.ToInt32(infoArray[0]);
        if (size == -1)
        {
            throw new Exception();
        }

        var data = new (string name, bool isDir)[size];

        for (int i = 1; i < infoArray.Length; i += 2)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                return null;
            }
            var isDir = Convert.ToBoolean(infoArray[i + 1]);
            data[(i - 1) / 2] = (infoArray[i], isDir);
        }

        return data;
    }

    /// <summary>
    /// запрос на скачивание нужного файла
    /// </summary>
    public async Task<long> Get(string path, Stream fileStream, CancellationToken cancellationToken)
    {
        using var client = new TcpClient(_host, _port);
        await using var stream = client.GetStream();
        await using var writer = new StreamWriter(stream);
        using var reader = new StreamReader(stream);
        await writer.WriteLineAsync($"2 {path}");
        await writer.FlushAsync();
        var size = Convert.ToInt32(await reader.ReadLineAsync());
        if (cancellationToken.IsCancellationRequested)
        {
            throw new ArgumentException();
        }
        if (size == -1)
        {
            throw new ArgumentException();
        }

        await stream.CopyToAsync(fileStream);
            
        return size;
    }
}