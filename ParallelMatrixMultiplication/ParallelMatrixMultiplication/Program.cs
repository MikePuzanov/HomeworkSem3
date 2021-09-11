using System;
using System.Diagnostics;
using System.Threading;

namespace ParallelMatrixMultiplication
{
    class Program
    {
        static void Main(string[] args)
        {
            var threads = new Thread[2];
            var chunkSize = args.Length / threads.Length;
            var results = new int[threads.Length];
            var matrixFirst = FileFunctions.CreateMatrix((args[0]));
            var matrixSecond = FileFunctions.CreateMatrix(((args[1])));
            Stopwatch stopWatch2 = new Stopwatch();
            stopWatch2.Start();
            var matrixAnother = MatrixFunctions.MatrixMultiplication(matrixFirst, matrixSecond);
            stopWatch2.Stop();
            TimeSpan time = stopWatch2.Elapsed;
            Stopwatch stopWatch1 = new Stopwatch();
            stopWatch1.Start();
            var matrix = MatrixFunctions.MatrixMultiplicationParallel(matrixFirst, matrixSecond);
            stopWatch1.Stop();
            TimeSpan timeParallel = stopWatch1.Elapsed;
            Console.WriteLine(time); 
            Console.WriteLine(timeParallel);
            FileFunctions.CreateFileWithMatrix(args[2], matrix);
        }
    }
}
