using System;
using System.Collections.Generic;
using System.Threading;

namespace Test3
{
    public class QueuePriory
    {
        private List<(int value, int priority)> _queue;
        private readonly Object _lockAdded = new();
        private readonly Object _lockDelete = new();
        private int _size = 0;
        private bool flag = false;

        public QueuePriory()
        { 
            _queue = new List<(int, int)>();
        }

        public void Enqueue(int value, int priority)
        {
            var index = 1;
            lock (_lockAdded)
            {
                if (_size == 0 && !flag)
                {
                    _queue.Insert(0, (value, priority));
                    _size++;
                    //Monitor.PulseAll(_lockDelete);
                    return;
                }

                if (flag)
                {
                    _queue.Insert(0, (value, priority));
                    _size++;
                    Monitor.PulseAll(_lockDelete);
                    return;
                }
                foreach (var t in _queue)
                {
                    if (priority > t.priority)
                    {
                        _queue.Insert(index, (value, priority));
                        _size++;
                        return;
                    }

                    index++;
                }
            }
        }

        public int Dequeue()
        {
            var value = 0;
            lock (_lockDelete)
            {
                while (_size == 0)
                {
                    Volatile.Write(ref flag, true);
                    Monitor.Wait(_lockAdded);
                }

                value = _queue[1].value;
                _queue.RemoveRange(1, 1);
                Volatile.Write(ref flag, false);
                _size--;
                return value;
            }
        }

        public int Size()
            => _size;
    }
}
