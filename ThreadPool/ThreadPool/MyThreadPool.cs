using System;
using System.Threading;

namespace ThreadPool
{
    public class MyThreadPool
    {
        private Thread[] _threads;
        
        MyThreadPool(int countThreads)
        {
            if (countThreads < 1)
            {
                throw new ArgumentException();
            }

            _threads = new Thread[countThreads];
        }

        public void Shutdown()
        {
            
        }
    }
}