namespace MyFTPServer;

using System;
using System.Threading.Tasks;

public class Program
{
    public static async Task Main(string[] args)
    {
        if (args.Length != 2)
        {
            Console.WriteLine("Вы не передали параметры или передали неверные.");
            return;
        }
        try
        {
            var server = new Server(args[0], Convert.ToInt32(args[1]));
            var serverStop = server.StartServer();
            Console.WriteLine("Введите !exit, чтобы остановить сервер");
            var command = "";
            while (command != "!exit")
            {
                command = Console.ReadLine();
            }
            server.StopServer();
            await serverStop;
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Ошибка!");
        }
    }
}
