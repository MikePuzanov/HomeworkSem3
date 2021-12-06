using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ThreadPool
{
    public class MyThreadPool
    {
        private readonly Thread[] _threads;
        private readonly CancellationTokenSource _token = new();
        private readonly ConcurrentQueue<Action> _tasksQueue = new();
        private readonly AutoResetEvent _waiterNewTask = new AutoResetEvent(false);
        private readonly AutoResetEvent _waiterTaskDone = new AutoResetEvent(false);
        private int _freeThreadsCount = 0;
        private readonly object _lockObject = new();

        /// <summary>
        /// constructor
        /// </summary>
        public MyThreadPool(int countThreads)
        {
            if (countThreads < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(countThreads));
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

                    Interlocked.Increment(ref _freeThreadsCount);
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
            if (_token.IsCancellationRequested)
            {
                throw new InvalidOperationException();
            }
            lock (_lockObject)
            {
                var task = new MyTask<TResult>(function, this);
                _tasksQueue.Enqueue(task.Count);
                _waiterNewTask.Set();
                return task;
            }
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

            while (_freeThreadsCount != _threads.Length)
            {
                _waiterNewTask.Set();
                _waiterTaskDone.WaitOne();
                _waiterTaskDone.Reset();
            }
        }

        /// <summary>
        /// class for parallel tasks
        /// </summary>
        private class MyTask<TResult> : IMyTask<TResult>
        {
            private Func<TResult> _func;
            private TResult _result;
            private Exception _resultException = null;
            private readonly object _locker = new();
            private readonly ManualResetEvent _waiterManual = new(false);
            private Queue<Action> _continueWithTasksQueue = new();
            private readonly MyThreadPool _myThreadPool;

            /// <summary>
            /// shows whether the task is completed or not
            /// </summary>
            public bool IsCompleted { get; set; }

            /// <summary>
            /// returns the result of the task
            /// </summary>
            public TResult Result
            {
                get
                {
                    _waiterManual.WaitOne();
                    if (_resultException != null)
                    {
                        throw new AggregateException(_resultException);
                    }

                    return _result;
                }
            }

            /// <summary>
            ///  constructor
            /// </summary>
            public MyTask(Func<TResult> function, MyThreadPool myThreadPool)
            {
                _func = function ?? throw new ArgumentNullException(nameof(function));
                _myThreadPool = myThreadPool;
            }

            /// <summary>
            /// counting result of task
            /// </summary>
            public void Count()
            {
                try
                {
                    _result = _func();
                }
                catch (Exception funcException)
                {
                    _resultException = funcException;
                }

                lock (_locker)
                {
                    _func = null;
                    IsCompleted = true;
                    _waiterManual.Set();
                }

                lock (_locker)
                {
                    while (_continueWithTasksQueue.Count > 0)
                    {
                        var action = _continueWithTasksQueue.Dequeue();
                        lock (_myThreadPool._lockObject)
                        {
                            _myThreadPool._tasksQueue.Enqueue(action);
                            _myThreadPool._waiterNewTask.Set();
                        }
                    }
                }
            }

            /// <summary>
            ///  continues calculating
            /// </summary>
            public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func)
            {
                lock (_locker)
                {
                    if (_myThreadPool._token.IsCancellationRequested)
                    {
                        throw new InvalidOperationException();
                    }

                    var task = new MyTask<TNewResult>(() => func(Result), _myThreadPool);
                    _continueWithTasksQueue.Enqueue(task.Count);
                    if (IsCompleted)
                    {
                        while (_continueWithTasksQueue.Count > 0)
                        {
                            var action = _continueWithTasksQueue.Dequeue();
                            _myThreadPool._tasksQueue.Enqueue(action);
                            _waiterManual.Set();
                        }
                    }

                    return task;
                }
            }
        }
    }
}