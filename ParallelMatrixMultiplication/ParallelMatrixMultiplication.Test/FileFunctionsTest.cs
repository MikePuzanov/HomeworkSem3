using NUnit.Framework;

namespace ParallelMatrixMultiplication.Test
{
    public class FileFunctionTest
    {
        [Test]
        public void Test1()
        {
            int[,] result = {{ 2, 1, 2}, {4, 1, 5}, {1, 5, 3}} ;
            var resultFromFile = FileFunctions.CreateMatrix("../../../MatrixTest.txt");
            Assert.AreEqual(result, resultFromFile);
        }
    }
}