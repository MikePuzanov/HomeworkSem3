using System;
using System.Threading;

namespace ParallelMatrixMultiplication
{
    /// <summary>
    /// Class with matrix multiplication
    /// </summary>
    public static class MatrixFunctions
    {
        /// <summary>
        /// Parallel matrix multiplication
        /// </summary>
        public static int[,] MatrixMultiplicationParallel(int[,] matrix1, int[,] matrix2)
        {
            if (matrix1.GetLength(1) != matrix2.GetLength(0))
            {
                throw new MultiplicationException(
                    "Number of columns in the first matrix are not equal to the rows in the second matrix!");
            }

            var matrix = new int[matrix1.GetLength(0), matrix2.GetLength(1)];
            var size = matrix1.GetLength(0) < Environment.ProcessorCount
                ? matrix1.GetLength(0)
                : Environment.ProcessorCount;
            var threads = new Thread[size];
            var chunkSize = matrix.GetLength(0) / threads.Length + 1;
            for (int i = 0; i < threads.Length; ++i)
            {
                var localI = i;
                threads[i] = new Thread(() =>
                {
                    for (var l = localI * chunkSize; l < (localI + 1) * chunkSize && l < matrix1.GetLength(0); ++l)
                    {
                        for (int j = 0; j < matrix2.GetLength(1); j++)
                        {
                            for (int k = 0; k < matrix1.GetLength((1)); k++)
                            {
                                matrix[l, j] += matrix1[l, k] * matrix2[k, j];
                            }
                        }
                    }
                });
            }

            foreach (var thread in threads)
            {
                thread.Start();
            }

            foreach (var thread in threads)
            {
                thread.Join();
            }

            return matrix;
        }

        /// <summary>
        /// Not parallel matrix multiplication
        /// </summary>
        public static int[,] MatrixMultiplication(int[,] matrix1, int[,] matrix2)
        {
            if (matrix1.GetLength(1) != matrix2.GetLength(0))
            {
                throw new MultiplicationException(
                    "Number of columns in the first matrix are not equal to the rows in the second matrix!");
            }

            var matrix = new int[matrix1.GetLength(0), matrix2.GetLength(1)];
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix2.GetLength(1); j++)
                {
                    for (int k = 0; k < matrix1.GetLength((1)); k++)
                    {
                        matrix[i, j] += matrix1[i, k] * matrix2[k, j];
                    }
                }
            }

            return matrix;
        }
    }
}