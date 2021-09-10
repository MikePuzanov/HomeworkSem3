using System;
using System.Threading;

namespace ParallelMatrixMultiplication
{
    /// <summary>
    /// Class with matrix multiplication
    /// </summary>
    public static class MatrixFunctions
    {
        public static int[,] MatrixMultiplication(int[,] matrix1, int[,] matrix2)
        {
            if (matrix1.GetLength(1) != matrix2.GetLength(0))
            {
                throw new MultiplicationException("Number of columns in the first matrix are not equal to the rows in the second matrix!");
            }
            var matrix = new int[matrix1.GetLength(1), matrix1.GetLength(0)];
            var threads = new Thread[matrix1.GetLength(0)];
            var results = new int[threads.Length, threads.Length];
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                var line = i;
                threads[i] = new Thread(() =>
                {
                    for (int j = 0; j < matrix2.GetLength(1); j++)
                    {
                        for (int k = 0; k < matrix1.GetLength((0)); ++k)
                        {
                            matrix[line, j] += matrix1[line, k] * matrix2[k, j];
                        }
                    }
                });
            }
            foreach (var thread in threads)
                thread.Start();
            foreach (var thread in threads)
                thread.Join();

            return matrix;
        }

    }
    
}