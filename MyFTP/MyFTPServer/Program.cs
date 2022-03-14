namespace MyFTPServer;

using System;
using System.Threading.Tasks;

public class Program
{
    public async void main(string[] args)
    {
        try
        {
            var server = new Server(args[0], Convert.ToInt32(args[1]));
            await server.StartServer();
            Console.WriteLine("Введите !exit, чтобы остановить сервер");
            var command = "";
            while (command != "!exit")
            {
                command = Console.ReadLine();
            }
        }
        catch (ArgumentException)
        {
            Console.WriteLine("Ошибка!");
        }
    }
}
