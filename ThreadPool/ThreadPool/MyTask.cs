using System;
using System.Threading;

namespace ThreadPool
{
    /// <summary>
    /// task classs
    /// </summary>
    public class MyTask<TResult> : IMyTask<TResult>
    {
        private Func<TResult> _func;
        private TResult _result;
        private Exception _resultException = null;
        private readonly object _locker = new();
        private readonly ManualResetEvent _waiterManual = new(false);

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
        public MyTask(Func<TResult> function)
        {
            _func = function ?? throw new ArgumentException();
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
        }

        /*/// <summary>
        ///  
        /// </summary>
        public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func)*/
    }
}