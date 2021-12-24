using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace QSort
{
    public  class QSort<T> where T:IComparable
    {
        private IList<T> List;
        
        public QSort(IList<T> list)
        {
            this.List = list;
        }
        
        private int Partition(int left, int right)
        {
            int start = right;
            int end = left;
            while (start != end)
            {
                if (List[end].CompareTo(List[start]) <= 0)
                {
                    end++;
                }
                else
                {
                    Swap(end, start);
                    Swap(end, start - 1);
                    start--;
                }
            }
            return end;
        }

        private void Swap(int posOne, int posTwo)
        {
            (List[posOne], List[posTwo]) = (List[posTwo], List[posOne]);
        }

        private void SortCall(int start, int end)
        {
            if (start >= end)
            {
                return;
            }
            int partition = Partition(start, end);
            SortCall(start, partition - 1);
            SortCall(partition + 1, end);
        }

        public void Sort()
        {
            SortCall(0, List.Count - 1);
        }
        
        private void SortCallMulti(int start, int end)
        {
            int partition = Partition(start, end);
            var task = new Task[2];
            task[0] = Task.Run(() => SortCallMulti(start, partition - 1));
            task[0].Wait();
            task[1] = Task.Run(() => SortCallMulti(partition + 1, end));
        }
        
        public void SortMulti()
        {
            SortCallMulti(0, List.Count - 1);
        }
    }
}