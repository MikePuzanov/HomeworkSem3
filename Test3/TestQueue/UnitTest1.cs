using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using NUnit.Framework;
using Test3;

namespace TestQueue
{
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var array = new (int, int)[50];
            for (int i = 1; i < 51; ++i)
            {
                array[i - 1] = (51 - i, i);
            }
            var count = 0;
            var queue = new QueuePriory();
            var threads = new Thread[5];

            for (int i = 0; i < threads.Length; ++i)
            {
                var localI = i;
                Task.Run(() =>
                    {
                        for (int j = localI; j < 50; j += 5)
                        {
                            queue.Enqueue(array[j].Item1, array[j].Item2);
                        }
                    }
                );
            }

            array = array.Reverse().ToArray();
            Assert.AreEqual(50, queue.Size());
            Assert.AreEqual(array[0].Item1, queue.Dequeue());
            Assert.Pass();
        }
    }
}