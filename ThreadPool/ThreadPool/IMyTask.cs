using System;

namespace ThreadPool
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TResult"></typeparam>
    public interface IMyTask<TResult>
    {
        /// <summary>
        /// 
        /// </summary>
        public bool IsCompleted { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public TResult Result { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="func"></param>
        /// <typeparam name="TNewResult"></typeparam>
        /// <returns></returns>
        public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func);
    }
}