using System;

namespace ThreadPool
{
    /// <summary>
    /// task interface
    /// </summary>
    public interface IMyTask<out TResult>
    {
        /// <summary>
        /// shows whether the task is completed or not
        /// </summary>
        public bool IsCompleted { get; set; }

        /// <summary>
        /// returns the result of the task
        /// </summary>
        public TResult Result { get; }

        /*/// <summary>
        ///  
        /// </summary>
        public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func)*/
    }
}