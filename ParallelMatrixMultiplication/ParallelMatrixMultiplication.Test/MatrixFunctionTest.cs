using NUnit.Framework;

namespace ParallelMatrixMultiplication.Test
{
    public class MatrixFunctionTest
    {
        [Test]
        public void TestNormalDataParallel()
        {
            var matrix1 = new int[1000,1000];
            var matrix2 = new int[1000,1000];
            var result = new int[1000,1000];
            var matrix = MatrixFunctions.MatrixMultiplicationParallel(matrix1, matrix2);
            Assert.AreEqual(result, matrix);
        }
        
        [Test]
        public void TestNormalDataNotParallel()
        {
            var matrix1 = new int[1000,1000];
            var matrix2 = new int[1000,1000];
            var result = new int[1000,1000];
            var matrix = MatrixFunctions.MatrixMultiplication(matrix1, matrix2);
            Assert.AreEqual(result, matrix);
        }
        
        [Test]
        public void TestAbnormalData()
        {
            var matrix1 = new int[3,4];
            var matrix2 = new int[3,3];
            Assert.Throws<MultiplicationException>(() => MatrixFunctions.MatrixMultiplicationParallel(matrix1, matrix2));
        }
    }
}