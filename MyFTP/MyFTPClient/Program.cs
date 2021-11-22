using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MyFTPClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Введите IP сервера:");
            var ip = Console.ReadLine();
            Console.WriteLine("Введите порт:");
            int.TryParse(Console.ReadLine(), out var port);
            var client = new Client(port, ip);
            Console.WriteLine("Cписок запросов:");
            Console.WriteLine(
                "List:\nФормат запроса:1 <path: String>\npath - путь к директории относительно того места, где запущен сервер");
            Console.WriteLine(
                "Get:\nФормат запроса:2 <path1: String> <path2: String>\npath1 - путь к файлу относительно того места, где запущен сервер\n" +
                "npath2 - путь к файлу на локальной машине, где запущен клиент");
            Console.WriteLine("Введите exit, если хотите закрыть соединение с сервером");
            var request = Console.ReadLine().Split(' ');
            var token = new CancellationTokenSource();
            while (request[0] != "exit" || !token.IsCancellationRequested)
            {
                if (request[0] == "1")
                {
                    try
                    {
                        var response = await client.List(request[1], token);
                        foreach (var file in response)
                        {
                            Console.WriteLine($"{file.Item1} {file.Item2}");
                        }
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine("Ошибка!");
                    }
                }

                if (request[0] == "2")
                {
                    using (var fstream = new FileStream(request[2], FileMode.OpenOrCreate))
                    {
                        try
                        {
                            var response = client.Get(request[1], fstream, token);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine("Ошибка!");
                        }
                    }
                }
            }
        }
    }
}