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
            var single = LazyFactory<int>.CreateSingleLazy(() =>
            {
                number1 += number1;
                return number1;
            });
            var multi = LazyFactory<int>.CreateMultiLazy(() =>
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
                var get = single.Get();
                Console.WriteLine(get);
                Console.WriteLine("!!!");
            }
            foreach (var thread  in threads)
            {
                thread.Start();
            }
            foreach (var thread  in threads)
            {
                thread.Join();
            }
            
        }
    }
}