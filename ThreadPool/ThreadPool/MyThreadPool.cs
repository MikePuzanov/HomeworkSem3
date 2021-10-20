using System;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading;

namespace ThreadPool
{
    public class MyThreadPool
    {
        private readonly Thread[] _threads;
        private readonly CancellationTokenSource _token = new();
        private readonly ConcurrentQueue<Action> _tasksQueue = new();
        private readonly AutoResetEvent _waiterNewTask = new AutoResetEvent(false);
        private readonly AutoResetEvent _waiterTaskDone = new AutoResetEvent(false);
        private readonly object _lockObject = new();

        /// <summary>
        /// constructor
        /// </summary>
        public MyThreadPool(int countThreads)
        {
            if (countThreads < 1)
            {
                throw new ArgumentException();
            }

            _threads = new Thread[countThreads];
            for (int i = 0; i < countThreads; i++)
            {
                _threads[i] = new Thread(() =>
                {
                    while (!_token.IsCancellationRequested || !_tasksQueue.IsEmpty)
                    {
                        if (_tasksQueue.TryDequeue(out Action action))
                        {
                            action();
                        }
                        else
                        {
                            _waiterNewTask.WaitOne();
                            if (!_tasksQueue.IsEmpty)
                            {
                                _waiterNewTask.Set();
                            }
                        }
                    }

                    _waiterTaskDone.Set();
                });

                _threads[i].Start();
            }
        }

        /// <summary>
        /// add task 
        /// </summary>
        public IMyTask<TResult> AddTask<TResult>(Func<TResult> function)
        {
            lock (_lockObject)
            {
                if (_token.IsCancellationRequested)
                {
                    throw new InvalidOperationException();
                }
            }

            var task = new MyTask<TResult>(function);
            _tasksQueue.Enqueue(task.Count);
            _waiterNewTask.Set();
            return task;
        }

        /// <summary>
        /// close threads
        /// </summary>
        public void Shutdown()
        {
            lock (_lockObject)
            {
                _token.Cancel();
            }
        }
    }
}