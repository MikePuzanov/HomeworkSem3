using System;
using NUnit.Framework;
using ThreadPool;

namespace ThreadPoolTest
{
    public class Tests
    {
        private MyThreadPool _threadPool;
        private readonly int _numberOfThreads = 4;

        [SetUp]
        public void Setup()
            => _threadPool = new(Environment.ProcessorCount);

        [Test]
        public void NullFunctionShouldThrowException()
        {
            Assert.Throws<ArgumentNullException>(() => _threadPool.AddTask<object>(null));
            _threadPool.Shutdown();
        }

        [Test]
        public void ResultShouldThrowException()
        {
            var task = _threadPool.AddTask<object>(() => throw new ArgumentOutOfRangeException());
            Assert.Throws<AggregateException>(() => Result(task));
            _threadPool.Shutdown();
        }

        private object Result<TResult>(IMyTask<TResult> task)
            => task.Result;

        [Test]
        public void ExceptionInThreadPoolConstructorTest()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() => new MyThreadPool(0));
            _threadPool.Shutdown();
        }

        [Test]
        public void ExceptionAfterShutdownTest()
        {
            _threadPool.Shutdown();
            Assert.Throws<InvalidOperationException>(() => _threadPool.AddTask(() => 5));
        }

        [Test]
        public void GetTaskAfterShutdown()
        {
            var task1 = _threadPool.AddTask(() => 3 * _numberOfThreads);
            var task2 = _threadPool.AddTask(() => _numberOfThreads * _numberOfThreads);
            _threadPool.Shutdown();
            Assert.AreEqual(12, task1.Result);
            Assert.AreEqual(16, task2.Result);
        }

        [Test]
        public void TaskResultTest()
        {
            int number1 = 2;
            int number2 = 2;
            var task1 = _threadPool.AddTask(() => number1 + 2);
            var task2 = _threadPool.AddTask(() => number2 * 5);
            _threadPool.Shutdown();
            Assert.AreEqual(4, task1.Result);
            Assert.AreEqual(10, task2.Result);
        }

        [Test]
        public void ContinueWithTest()
        {
            var task1 = _threadPool.AddTask(() => 3 * _numberOfThreads); //12
            var task2 = task1.ContinueWith(x => x + _numberOfThreads); //12 + 4
            var task3 = task1.ContinueWith(x => x + task2.Result); // 12 + 16
            _threadPool.Shutdown();
            Assert.AreEqual(12, task1.Result);
            Assert.AreEqual(16, task2.Result);
            Assert.AreEqual(28, task3.Result);
        }
    }
}