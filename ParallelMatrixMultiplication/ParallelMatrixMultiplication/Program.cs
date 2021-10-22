using System;
using System.Diagnostics;
using System.Threading;

namespace ParallelMatrixMultiplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Statistics.CollectStatisticsFromMatrix(128, 128, 100);
            /*
            Результаты на матрицах размеров 128*128.
            Количество повторов: 100.
            Паралельное умножение:
            Матожидание = 13,75 ms
            Среднеквадратичное отклонение = 3,89 ms
            Обычное умножение:
            Матожидание = 37,86 ms
            Среднеквадратичное отклонение = 9,04 ms
            */

            Statistics.CollectStatisticsFromMatrix(256, 256, 100);
            /*
            Результаты на матрицах размеров 256*256.
            Количество повторов: 100.
            Паралельное умножение:
            Матожидание = 72,4 ms
            Среднеквадратичное отклонение = 10,58 ms
            Обычное умножение:
            Матожидание = 169,91 ms
            Среднеквадратичное отклонение = 20,95 ms
            */

            Statistics.CollectStatisticsFromMatrix(512, 512, 100);
            /*
            Результаты на матрицах размеров 512*512.
            Количество повторов: 100.
            Паралельное умножение:
            Матожидание = 602,98 ms
            Среднеквадратичное отклонение = 39,31 ms
            Обычное умножение:
            Матожидание = 2049,14 ms
            Среднеквадратичное отклонение = 361,01 ms
            */

            var matrixFirst = FunctionsForFile.CreateMatrix(args[0]);
            var matrixSecond = FunctionsForFile.CreateMatrix(args[1]);
            var matrix = MatrixFunctions.MatrixMultiplicationParallel(matrixFirst, matrixSecond);
            FunctionsForFile.CreateFileWithMatrix(args[2], matrix);
        }
    }
}