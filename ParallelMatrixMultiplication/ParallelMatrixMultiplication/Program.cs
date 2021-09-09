using System;
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
            /*int[,] matrixFirst;
            int[,] matrixSecond;*/
            var matrixFirst = FileFunctions.CreateMatrix((args[0]));
            var matrixSecond = FileFunctions.CreateMatrix((args[1]));
            /*threads[0] = new Thread(() =>
            {
                matrixFirst = FileFunctions.CreateMatrix((args[0]));
            });
            threads[1] = new Thread(() =>
            {
                matrixSecond = FileFunctions.CreateMatrix((args[1]));
            });            */
            var matrix = MatrixFunctions.MatrixMuptiplication(matrixFirst, matrixSecond);
            FileFunctions.CreateFileWithMatrix(args[2], matrix);
        }
    }
}
