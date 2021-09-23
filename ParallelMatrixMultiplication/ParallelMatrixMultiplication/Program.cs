using System;
using System.Diagnostics;
using System.Threading;

namespace ParallelMatrixMultiplication
{
    class Program
    {
        static void Main(string[] args)
        {
            Statistics.CollectStaticsFromMatrix(128, 128, 100);
            /*
            Результаты на матрицах размеров 128*128.
            Количество повторов: 100.
            Паралельное умножение:
            Матожидание = 13,75
            Среднеквадратичное отклонение = 3,897114317029974
            Обычное умножение:
            Матожидание = 37,86
            Среднеквадратичное отклонение = 9,046568410176315
            */
            
            Statistics.CollectStaticsFromMatrix(256, 256, 100);
            /*
            Результаты на матрицах размеров 256*256.
            Количество повторов: 100.
            Паралельное умножение:
            Матожидание = 72,4
            Среднеквадратичное отклонение = 10,583950113261116
            Обычное умножение:
            Матожидание = 169,91
            Среднеквадратичное отклонение = 20,951417613135394
            */
            
            Statistics.CollectStaticsFromMatrix(512, 512, 100);
            /*
            Результаты на матрицах размеров 512*512.
            Количество повторов: 100.
            Паралельное умножение:
            Матожидание = 602,98
            Среднеквадратичное отклонение = 39,318947086614614
            Обычное умножение:
            Матожидание = 2049,14
            Среднеквадратичное отклонение = 361,013213608588
            */
            
            var matrixFirst = FileFunctions.CreateMatrix((args[0]));
            var matrixSecond = FileFunctions.CreateMatrix(((args[1])));
            var matrix = MatrixFunctions.MatrixMultiplicationParallel(matrixFirst, matrixSecond);
            FileFunctions.CreateFileWithMatrix(args[2], matrix);
        }
    }
}
