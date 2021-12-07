using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;

namespace Lazyy.Test
{
    public class SingleThreadTest
    {
        private static IEnumerable<TestCaseData> Lazy()
        {
            int countForSingle = 0;
            int countForMulti = 0;
            yield return new TestCaseData(LazyFactory.CreateSingleLazy(() => ++countForSingle));
            yield return new TestCaseData(LazyFactory.CreateMultiLazy(() => Interlocked.Increment(ref countForMulti)));
        }
        
        [Test]
        public void SupplierCannotBeNull()
        {
            Assert.Throws<ArgumentNullException>(() => LazyFactory.CreateSingleLazy<int>(null));
            Assert.Throws<ArgumentNullException>(() => LazyFactory.CreateMultiLazy<int>(null));
        }        
        
        [TestCaseSource(nameof(Lazy))]
        public void TestLazyLaunchFunctionOnly1Time<T>(ILazy<T> lazy)
        {
            for (int i = 0; i < 100; i++)
            {
                Assert.AreEqual(1, lazy.Get());
            }
        }
        
        [TestCaseSource(nameof(Lazy))]
        public void GetShouldNotChangeValue<T>(ILazy<T> lazy)
        {
            var value = lazy.Get();
            Assert.AreEqual(1, value);
            for (int i = 0; i < 25; i++)
            {
                Assert.AreEqual(value, lazy.Get());
            }
        }

        [Test]
        public void RaceConditionsCheck()
        {
            var count = 0;
            var lazy = LazyFactory.CreateMultiLazy(() => Interlocked.Increment(ref count));
            var threads = new Thread[10];

            for (int i = 0; i < threads.Length; ++i)
            {
                threads[i] = new Thread(() => lazy.Get());
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }
            foreach (var thread in threads)
            {
                thread.Join();
            }

            Assert.AreEqual(1, count);
        }
    }
}