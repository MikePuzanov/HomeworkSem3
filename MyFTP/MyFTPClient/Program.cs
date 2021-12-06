global using System.Threading;
global using System;
global using System.IO;

var client = new Client(args[0], Convert.ToInt32(args[1]));
var request = Console.ReadLine().Split(' ');
var token = new CancellationToken();
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
        catch (Exception)
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
            catch (Exception)
            {
                Console.WriteLine("Ошибка!");
            }
        }
    }
}