﻿namespace MyFTPClient;

using System.Threading;
using System;
using System.IO;

public class Program
{
    public async void main(string[] args)
    {
        var client = new Client(args[0], Convert.ToInt32(args[1]));
        var token = new CancellationToken();
        while (args[0] != "exit" || !token.IsCancellationRequested)
        {
            if (args[0] == "1")
            {
                try
                {
                    var response = await client.List(args[1], token);
                    foreach (var file in response)
                    {
                        Console.WriteLine($"{file.Item1} {file.Item2}");
                    }
                }
                catch (ArgumentException)
                {
                    Console.WriteLine("Ошибка!");
                }
            }

            if (args[0] == "2")
            {
                using (var fstream = new FileStream(args[2], FileMode.OpenOrCreate))
                {
                    try
                    {
                        var response = client.Get(args[1], fstream, token);
                    }
                    catch (ArgumentException)
                    {
                        Console.WriteLine("Ошибка!");
                    }
                }
            }
        }
    }
}