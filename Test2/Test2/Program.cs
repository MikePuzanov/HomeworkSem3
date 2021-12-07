using System;

namespace Test2
{
    class Program
    {
        static void Main(string[] args)
        {
            var hash = MD5.CheckSum("C:\\Users\\89803\\Files\\MatMech\\Algebra");
            Console.WriteLine(hash);
            Console.WriteLine("\n");
            hash = MD5.CheckSumMulti("C:\\Users\\89803\\Files\\MatMech\\Algebra");
            Console.WriteLine(hash);
        }
    }
}