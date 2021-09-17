using System;
using System.Diagnostics;
using System.Threading;

namespace ParallelMatrixMultiplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var matrixTest = new int[1000, 1000];
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            for (int i = 0; i < 10; i++)
            {
                var testTime =MatrixFunctions.MatrixMultiplicationParallel(matrixTest, matrixTest);
            }
            stopWatch.Stop();
            var timeParallel = stopWatch.ElapsedMilliseconds;
            stopWatch.Restart();
            for (int i = 0; i < 10; i++)
            {
                var matrixAnother = MatrixFunctions.MatrixMultiplication(matrixTest, matrixTest);
            }
            stopWatch.Stop();
            var time = stopWatch.ElapsedMilliseconds;
            Console.WriteLine($"Среднее время обычного умножения матриц 1000*1000: {time / 10} ms"); 
            Console.WriteLine($"Среднее время параллельного умножения матриц 1000*1000: {timeParallel / 10} ms");
            var matrixFirst = FileFunctions.CreateMatrix((args[0]));
            var matrixSecond = FileFunctions.CreateMatrix(((args[1])));
            var matrix = MatrixFunctions.MatrixMultiplicationParallel(matrixFirst, matrixSecond);
            FileFunctions.CreateFileWithMatrix(args[2], matrix);
        }
    }
}
