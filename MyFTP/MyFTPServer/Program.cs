global using System;
global using System.Threading.Tasks;

try
{
    var server = new Server(args[0], Convert.ToInt32(args[1]));
    await server.StartServer();
}
catch (Exception)
{
    Console.WriteLine("Ошибка!");
}