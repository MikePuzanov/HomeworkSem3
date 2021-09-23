using System;
using System.Collections.Generic;
using System.Diagnostics;
using NUnit.Framework;

namespace ParallelMatrixMultiplication.Test
{
    public class MatrixFunctionTest
    {
        [TestCaseSource(nameof(FunctionsForTest))]
        public void TestNormalSquareData(Func<int[,], int[,], int[,]> multiplication)
        {
            int[,] result = {{ 15, 27, 8}, {24, 57, 13}, {41, 26, 24}} ;
            int[,] matrix1 = {{ 2, 1, 2}, {4, 1, 5}, {1, 5, 3}};
            int[,] matrix2 = {{ 3, 9, 1}, {7, 1, 4}, {1, 4, 1}};
            var matrix = multiplication(matrix1, matrix2);
            Assert.AreEqual(result, matrix);
        }
        
        [TestCaseSource(nameof(FunctionsForTest))]
        public void TestNormalNotSquareData(Func<int[,], int[,], int[,]> multiplication)
        {
            int[,] result = {{ 36, 71, 29}, {30, 22, 23}, {49, 87, 28}} ;
            int[,] matrix1 = {{ 6, 1, 3, 4}, {1, 3, 2, 2}, {7, 3, 5, 1}};
            int[,] matrix2 = {{ 3, 9, 1}, {7, 1, 4}, {1, 4, 1}, {2, 1, 4}};
            var matrix = multiplication(matrix1, matrix2);
            Assert.AreEqual(result, matrix);
        }
        
        [TestCaseSource(nameof(FunctionsForTest))]
        public void TestAbnormalData(Func<int[,], int[,], int[,]> multiplication)
        {
            var matrix1 = new int[3,4];
            var matrix2 = new int[3,3];
            Assert.Throws<MultiplicationException>(() => multiplication(matrix1, matrix2));
        }
        
        private static IEnumerable<Func<int[,], int[,], int[,]>> FunctionsForTest()
        {
            yield return MatrixFunctions.MatrixMultiplicationParallel;
            yield return MatrixFunctions.MatrixMultiplication;
        }
    }
}