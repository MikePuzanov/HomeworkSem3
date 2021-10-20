using System;
using System.Threading;
using NUnit.Framework;
using ThreadPool;

namespace ThreadPoolTest
{
    public class Tests
    {
        private ThreadPool.MyThreadPool _threadPool;
        private readonly int _numberOfThreads = Environment.ProcessorCount;

        [SetUp]
        public void Setup()
        {
            _threadPool = new(Environment.ProcessorCount);
        }

        [Test]
        public void CountingThreadsTest1()
        {
            int number = 0;
            for (int i = 0; i < 20; i++)
            {
                _threadPool.AddTask(() =>
                {
                    Interlocked.Increment(ref number);
                    Thread.Sleep(3000);
                    return "qwe";
                });
            }

            Thread.Sleep(300);
            Assert.AreEqual(_numberOfThreads, number);
        }


        [Test]
        public void CountingThreadsTest2()
        {
            int number = 0;
            _threadPool.AddTask(() =>
            {
                Interlocked.Increment(ref number);
                Thread.Sleep(3000);
                return "qwe";
            });

            Thread.Sleep(300);
            Assert.AreEqual(1, number);
        }

        [Test]
        public void ShutdownTest()
        {
            int number = 0;
            for (int i = 0; i < 20; i++)
            {
                _threadPool.AddTask(() =>
                {
                    Interlocked.Increment(ref number);
                    Thread.Sleep(1000);
                    return "qwe";
                });
            }

            Thread.Sleep(100);
            _threadPool.Shutdown();

            Thread.Sleep(500);
            Assert.AreEqual(_numberOfThreads, number);
        }

        [Test]
        public void TaskResultTest()
        {
            int number1 = 2;
            int number2 = 2;
            var task1 = _threadPool.AddTask(() =>
            {
                number1 += 2;
                return number1;
            });
            var task2 = _threadPool.AddTask(() =>
            {
                number2 *= 5;
                return number2;
            });

            Thread.Sleep(300);
            Assert.AreEqual(4, task1.Result);
            Assert.AreEqual(10, task2.Result);
        }

        [Test]
        public void ExceptionInThreadPoolConstructorTest()
        {
            Assert.Throws<ArgumentException>(() => new MyThreadPool(0));
        }

        [Test]
        public void ExceptionAfterShutdownTest()
        {
            _threadPool.Shutdown();
            Assert.Throws<InvalidOperationException>(() => _threadPool.AddTask(() => 5));
        }
    }
}