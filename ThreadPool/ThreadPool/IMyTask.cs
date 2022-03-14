namespace ThreadPool;

using System;

/// <summary>
/// interface for object from MyThreadPool
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

    /// <summary>
    ///  continues calculating
    /// </summary>
    public IMyTask<TNewResult> ContinueWith<TNewResult>(Func<TResult, TNewResult> func);
}

