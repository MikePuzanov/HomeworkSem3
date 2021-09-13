using System;
using System.Diagnostics;
using System.Threading;

namespace ParallelMatrixMultiplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var matrixFirst = FileFunctions.CreateMatrix((args[0]));
            var matrixSecond = FileFunctions.CreateMatrix(((args[1])));
            var matrixTest = new int[1000, 1000];
            Stopwatch stopWatch2 = new Stopwatch();
            stopWatch2.Start();
            var matrixAnother = MatrixFunctions.MatrixMultiplication(matrixTest, matrixTest);
            stopWatch2.Stop();
            TimeSpan time = stopWatch2.Elapsed;
            Stopwatch stopWatch1 = new Stopwatch();
            stopWatch1.Start();
            var matrixTest1 = MatrixFunctions.MatrixMultiplicationParallel(matrixTest, matrixTest);
            stopWatch1.Stop();
            TimeSpan timeParallel = stopWatch1.Elapsed;
            Console.WriteLine($"Время обычного умножения матриц 1000*1000: {time}"); 
            Console.WriteLine($"Время параллельного умножения матриц 1000*1000: {timeParallel}");
            var matrix = MatrixFunctions.MatrixMultiplicationParallel(matrixFirst, matrixSecond);
            FileFunctions.CreateFileWithMatrix(args[2], matrix);
        }
    }
}
