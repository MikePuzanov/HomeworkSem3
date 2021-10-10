using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace ParallelMatrixMultiplication
{
    /// <summary>
    /// class for collecting statistics
    /// </summary>
    public class Statistics
    {
        /// <summary>
        /// Collect statistics from matrix
        /// </summary>
        public static void CollectStatisticsFromMatrix(int row, int column, int count)
        {
            (var timeParallel, var timeNotParallel) = GetTimeMatrixMultiplication(count, row, column);
            var result = GetStatistics(timeParallel, timeNotParallel);
            Console.WriteLine($"Результаты на матрицах размеров {row}*{column}.");
            Console.WriteLine($"Количество повторов: {count}.");
            Console.WriteLine("Паралельное умножение:");
            Console.WriteLine(
                $"Матожидание = {result[0].average}\nСреднеквадратичное отклонение = {result[0].standardDeviation}");
            Console.WriteLine("Обычное умножение:");
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

        private static (List<long>, List<long>) GetTimeMatrixMultiplication(int count, int countRows, int countColumns)
        {
            var timeParallel = new List<long>();
            var timeNotParallel = new List<long>();
            var timer = new Stopwatch();
            for (int i = 0; i < count; i++)
            {
                var matrix1 = GenerateMatrix(countRows, countColumns);
                var matrix2 = GenerateMatrix(countRows, countColumns);
                timer.Restart();
                MatrixFunctions.MatrixMultiplicationParallel(matrix1, matrix2);
                timer.Stop();
                timeParallel.Add(timer.ElapsedMilliseconds);
                timer.Restart();
                MatrixFunctions.MatrixMultiplication(matrix1, matrix2);
                timer.Stop();
                timeNotParallel.Add(timer.ElapsedMilliseconds);
            }

            return (timeParallel, timeNotParallel);
        }

        private static int[,] GenerateMatrix(int countRow, int countColumns)
        {
            var matrix = new int[countRow, countColumns];
            var random = new Random();
            for (int i = 0; i < countRow; i++)
            {
                for (int j = 0; j < countColumns; j++)
                {
                    matrix[i, j] = random.Next(0, 100);
                }
            }

            return matrix;
        }
    }
}