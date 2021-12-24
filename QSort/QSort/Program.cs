using System;
using System.Diagnostics;

namespace QSort
{
    class Program
    {
        static void Main(string[] args)
        {
            Statistics.CollectStatisticsFromMatrix(1000, 1000);
            /*
            Результаты на массивах размером 1000.
            Количество повторов: 1000.
            Паралельная сортировка:
            Матожидание = 0,005
            Среднеквадратичное отклонение = 0,1580348062927937
            Обычная сортировка:
            Матожидание = 0,136
            Среднеквадратичное отклонение = 0,5399111037939502
            #1#

            Statistics.CollectStatisticsFromMatrix(1000, 500);
            /*
            Результаты на массивах размером 1000.
            Количество повторов: 500.
            Паралельная сортировка:
            Матожидание = 0
            Среднеквадратичное отклонение = 0
            Обычная сортировка:
            Матожидание = 0,03
            Среднеквадратичное отклонение = 0,1705872210923191
            #1#
            
            Statistics.CollectStatisticsFromMatrix(500, 250);
            /*
            Результаты на массивах размером 500.
            Количество повторов: 250.
            Паралельная сортировка:
            Матожидание = 0
            Среднеквадратичное отклонение = 0
            Обычная сортировка:
            Матожидание = 0
            Среднеквадратичное отклонение = 0
             #1#
             */

            var array = new int[100];
            var rand = new Random();
            for (int i = 0; i < 100; i++)
            {
                array[i] = rand.Next(100);
            }
            var array1 = array;
            var sorter = new QSort<int>(array);
            var timer = new Stopwatch();
            timer.Start();
            sorter.SortMulti();
            timer.Stop();
            for (int i = 0; i < array.Length; i++)
            {
                Console.Write($"{array[i]}  ");
            }
        }
    }
}