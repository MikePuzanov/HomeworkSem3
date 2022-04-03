namespace MyFTPClient;

using System.Threading;
using System;
using System.IO;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        var client = new Client(args[0], Convert.ToInt32(args[1]));
        var token = new CancellationToken();
        Console.WriteLine("1 - List — листинг файлов в директории на сервере\nНапример, 1 ./Test/Files\n");
        Console.WriteLine("2 - Get — скачивание файла с сервера\n2 ./Test/Files/file1.txt\n");
        Console.WriteLine("Введите !exit, чтобы остановить сервер\n");
        Console.WriteLine("Введите команду:");
        var request = Console.ReadLine().Split(' ');
        while (request[0] != "!exit" || !token.IsCancellationRequested)
        {
            if (request[0] == "1" && request.Length == 2)
            {
                try
                {
                    var response = await client.List(request[1], token);
                    Console.WriteLine(response.Length);
                    foreach (var file in response)
                    {
                        Console.WriteLine($"{file.Item1} {file.Item2}");
                    }
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("-1");
                }
            }
            else if (request[0] == "2" && request.Length == 2)
            {
                using var fstream = new FileStream(request[2], FileMode.OpenOrCreate);
                try
                {
                    var response = await client.Get(request[1], fstream, token);
                    Console.WriteLine(response);
                }
                catch (FileNotFoundException)
                {
                    Console.WriteLine("-1");
                }
            }
            else
            {
                Console.WriteLine("Некорректная команда!");
            }
            Console.WriteLine("\nВведите команду:");
            request = Console.ReadLine().Split(' ');
        }
    }
}