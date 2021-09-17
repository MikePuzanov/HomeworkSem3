using System.Diagnostics;
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
        
        [Test]
        public void TestIsParallelFaster()
        {
            var matrixTest = new int[1000,1000];
            Stopwatch stopWatch1 = new Stopwatch();
            stopWatch1.Start();
            for (int i = 0; i < 5; i++)
            {
                var testTime =MatrixFunctions.MatrixMultiplicationParallel(matrixTest, matrixTest);
            }
            stopWatch1.Stop();
            var timeParallel = stopWatch1.ElapsedMilliseconds;
            Stopwatch stopWatch2 = new Stopwatch();
            stopWatch2.Start();
            for (int i = 0; i < 5; i++)
            {
                var matrixAnother = MatrixFunctions.MatrixMultiplication(matrixTest, matrixTest);
            }
            stopWatch2.Stop();
            var time = stopWatch2.ElapsedMilliseconds;
            Assert.IsTrue(timeParallel < time);
        }

    }
}