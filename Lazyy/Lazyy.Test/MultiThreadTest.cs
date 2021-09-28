using System;
using System.Threading;
using NUnit.Framework;

namespace Lazyy.Test
{
    public class MultiThreadTest
    {
        [Test]
        public void NormalWorkForMultiTest()
        {
            var number = 2;
            var lazyMulti = LazyFactory<int>.CreateMultiLazy(() =>
            {
                number *= number;
                return number;
            });
            
            var threads = new Thread[10];
            for (int i = 0; i < threads.Length; i++)
            {
                threads[i] = new Thread(() => lazyMulti.Get());
            }

            foreach (var thread  in threads)
            {
                thread.Start();
            }
            foreach (var thread  in threads)
            {
                thread.Join();
            }
            Assert.AreEqual(4, lazyMulti.Get());
        }

        [Test]
        public void NullExceptionTest()
        {
            Assert.Throws<NullReferenceException>(() => LazyFactory<int>.CreateMultiLazy(null));
        }
    }
}