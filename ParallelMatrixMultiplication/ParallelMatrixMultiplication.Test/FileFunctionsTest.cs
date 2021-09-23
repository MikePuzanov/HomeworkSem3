using NUnit.Framework;

namespace ParallelMatrixMultiplication.Test
{
    public class FileFunctionTest
    {
        [Test]
        public void TestNormalFilePath()
        {
            int[,] result = {{ 2, 1, 2}, {4, 1, 5}, {1, 5, 3}} ;
            var resultFromFile = FileFunctions.CreateMatrix("../../../MatrixTest1.txt");
            Assert.AreEqual(result, resultFromFile);
        }
        
        [Test]
        public void TestAbnormalFilePath()
        {
            Assert.Throws<FilePathException>(() => FileFunctions.CreateMatrix(null));
        }
        
        [Test]
        public void TestEmptyFile()
        {
            Assert.Throws<EmptyFileException>(() => FileFunctions.CreateMatrix("../../../EmptyFileTest.txt"));
        }
    }
}