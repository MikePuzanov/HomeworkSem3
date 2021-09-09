namespace ParallelMatrixMultiplication
{
    /// <summary>
    /// Class with matrix multiplication
    /// </summary>
    public class MatrixFunctions
    {
        public static int[,] MatrixMuptiplication(int[,] matrix1, int[,] matrix2)
        {
            var matrix = new int[matrix1.Rank, matrix1.GetLength(0)];
            for (int i = 0; i < matrix1.GetLength(0); i++)
            {
                for (int j = 0; j < matrix1.GetLength(i); j++)
                {
                    for (int k = 0; k < matrix1.GetLength((0)); ++k)
                    {
                        matrix[i, j] += matrix1[i, k] * matrix2[k, j];   
                    }
                }
            }

            return matrix;
        }
    }
}