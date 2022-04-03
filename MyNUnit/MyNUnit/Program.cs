namespace MyNUnit;

using System;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        if (args.Length == 0)
        {
            Console.WriteLine("Путь к папке не указан");
            return;
        }

        if (Directory.Exists(args[0]))
        {
            Console.WriteLine("По данному пути ниичего не найдено.");
            return;
        }

        var myNUnit = new MyNUnit();
        var result = myNUnit.RunTests(args[0]);
        foreach (var test in result)
        {
            Console.WriteLine(test.Item1);
            if (test.Item2 != null)
            {
                Console.WriteLine(test.Item2);
            }
        }
    }
}