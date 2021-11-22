using System;
using System.Threading.Tasks;

namespace MyFTPServer
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Введите IP сервер:");
            var ipAddress = Console.ReadLine();
            Console.WriteLine("Введите порт сервера:");
            var port = int.Parse(Console.ReadLine());
            try
            {
                var server = new Server(ipAddress, port);
                await server.StartServer();
                Console.WriteLine("Введите /exit, чтобы остановить сервер");
                var command = "";
                while (command != "/exit")
                {
                    command = Console.ReadLine();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Ошибка!");
            }
        }
    }
}