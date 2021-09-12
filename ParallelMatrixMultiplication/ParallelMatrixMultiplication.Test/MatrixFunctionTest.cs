using NUnit.Framework;

namespace ParallelMatrixMultiplication.Test
{
    public class MatrixFunctionTest
    {
        [Test]
        public void TestNormalData()
        {
            int[,] matrix1 = {{ 2, 1, 2}, {4, 1, 5}, {1, 5, 3}} ;
            int[,] matrix2 = {{ 9, 3, 1}, {4, 6, 9}, {1, 5, 7}} ;
            int[,] result = {{24, 22, 25}, {45, 43, 48}, {32, 48, 67}} ;
            var matrix = MatrixFunctions.MatrixMultiplicationParallel(matrix1, matrix2);
            Assert.AreEqual(result, matrix);
        }
        
        [Test]
        public void TestAbnormalData()
        {
            int[,] matrix1 = {{ 2, 1, 2}, {4, 1, 5}, {1, 5, 3}, {1, 5, 5}} ;
            int[,] matrix2 = {{ 9, 3, 1}, {4, 6, 9}, {1, 5, 7}} ;
            Assert.Throws<MultiplicationException>(() => MatrixFunctions.MatrixMultiplicationParallel(matrix1, matrix2));
        }
    }
}