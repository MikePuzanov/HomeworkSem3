using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace QSort
{
    public class Statistics
    {
        public static void CollectStatisticsFromMatrix(int lenght, int count)
        {
            (var timeParallel, var timeNotParallel) = GetTimeArraySort(count,lenght);
            var result = GetStatistics(timeParallel, timeNotParallel);
            Console.WriteLine($"Результаты на массивах размером {lenght}.");
            Console.WriteLine($"Количество повторов: {count}.");
            Console.WriteLine("Паралельная сортировка:");
            Console.WriteLine(
                $"Матожидание = {result[0].average}\nСреднеквадратичное отклонение = {result[0].standardDeviation}");
            Console.WriteLine("Обычная сортировка:");
            Console.WriteLine(
                $"Матожидание = {result[1].average}\nСреднеквадратичное отклонение = {result[1].standardDeviation}");
        }

        private static (double average, double standardDeviation)[] GetStatistics(List<long> timeParallel,
            List<long> timeNotParallel)
        {
            var averageParallel = timeParallel.Average();
            var averageNotParallel = timeNotParallel.Average();
            var dispersionParallel = timeParallel.Select(x => Math.Pow(x - averageParallel, 2)).Average();
            var dispersionNotParallel = timeNotParallel.Select(x => Math.Pow(x - averageNotParallel, 2)).Average();
            var standardDeviationParallel = Math.Sqrt(dispersionParallel);
            var standardDeviationNotParallel = Math.Sqrt(dispersionNotParallel);
            var results = new (double, double)[2];
            results[0] = (averageParallel, standardDeviationParallel);
            results[1] = (averageNotParallel, standardDeviationNotParallel);
            return results;
        }

        private static (List<long>, List<long>) GetTimeArraySort(int count, int lenght)
        {
            var timeParallel = new List<long>();
            var timeNotParallel = new List<long>();
            var timer = new Stopwatch();
            for (int i = 0; i < count; i++)
            {
                var array = GenerateArray(lenght);
                var sorter = new QSort<int>(array);
                timer.Restart();
                sorter.SortMulti();
                timer.Stop();
                timeParallel.Add(timer.ElapsedMilliseconds);
                array = GenerateArray(lenght);
                sorter = new QSort<int>(array);
                timer.Restart();
                sorter.Sort();
                timer.Stop();
                timeNotParallel.Add(timer.ElapsedMilliseconds);
            }

            return (timeParallel, timeNotParallel);
        }

        private static int[] GenerateArray(int lenght)
        {
            var array = new int[lenght];
            var random = new Random();
            for (int i = 0; i < lenght; i++)
            {
                array[i] = random.Next(0, 100);
            }

            return array;
        }
    }
}