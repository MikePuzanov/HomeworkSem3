using System;
using NUnit.Framework;
using QSort;

namespace QSortTest
{
    public class Tests
    {
        private int[] _array;

        [SetUp]
        public void Setup()
        {
            var array = new int[100];
            var rand = new Random();
            for (int i = 0; i < 100; i++)
            {
                array[i] = rand.Next(100);
            }
            _array = array;
        }

        [Test]
        public void QSortTest()
        {
            var sorter = new QSort<int>(_array);
            sorter.Sort();
            for (int i = 0; i < _array.Length - 1; ++i)
            {
                Assert.IsTrue(_array[i] <= _array[i + 1]);
            }
        }
        
        [Test]
        public void QSortMultiTest()
        {
            var sorter = new QSort<int>(_array);
            sorter.SortMulti();
            for (int i = 0; i < _array.Length - 1; ++i)
            {
                Assert.IsTrue(_array[i] <= _array[i + 1]);
            }
        }
    }
}