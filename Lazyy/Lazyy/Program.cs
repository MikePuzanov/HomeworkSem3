using System;
using System.Threading;

namespace Lazyy
{
    class Program
    {
        static void Main(string[] args)
        {
            var number1 = 2;
            var number2 = 2;
            var single = LazyFactory.CreateMultiLazy<int>(() =>
            {
                number1 += number1;
                return number1;
            });
            var multi = LazyFactory.CreateMultiLazy<int>(() =>
            {
                number2 *= number2;
                return number2;
            });
            
            var threads = new Thread[5];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() => multi.Get());
            }

            foreach (var t in threads)
            {
                Console.WriteLine(multi.Get());
                Console.WriteLine("!!!");
                Console.WriteLine(single.Get());
                Console.WriteLine("!!!");
            }
            foreach (var thread in threads)
            {
                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }
        }
    }
}