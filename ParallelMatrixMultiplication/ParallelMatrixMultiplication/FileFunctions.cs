using System;
using System.IO;

namespace ParallelMatrixMultiplication
{
    /// <summary>
    /// File's function
    /// </summary>
    public static class FileFunctions
    {
        private static (int, int) CountSizeMatrix(string filePath)
        {
            using var file = new StreamReader(filePath);
            string line = file.ReadLine();
            int size = 0;
            if (line == null)
            {
                throw new Exception();
            }
            string[] lineDrop = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            while (line != null)
            {
                size++;
                line = file.ReadLine();
            }
            return (size, lineDrop.Length);
        }

        /// <summary>
        /// Create Marrix from file
        /// </summary>
        public static int[,] CreateMatrix(string filePath)
        {
            if (filePath == "")
            {
                throw new Exception();
            }

            (int lenght, int width) size = CountSizeMatrix((filePath));
            var matrix = new int[size.lenght, size.width];
            using var file = new StreamReader(filePath);
            string line = file.ReadLine();
            while (line != null)
            {
                var index = 0;
                string[] lineDrop = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < size.lenght; ++i)
                {
                    matrix[index, i] = Int32.Parse(lineDrop[i]);
                }
                line = file.ReadLine();
            }

            return matrix;
        }

        /// <summary>
        /// Create file and return matrix
        /// </summary>
        public static void CreateFileWithMatrix(string filePath, int[,] matrix)
        {
            if (filePath == "")
            {
                throw new Exception();
            }
            var fileOut = new FileInfo(filePath);
            if (fileOut.Exists)
            {
                fileOut.Delete();
            }
            using var newFile = new FileStream(filePath, FileMode.Create);
            var file = new StreamWriter(newFile);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                var line = "";
                for (int j = 0; j < matrix.Length; j++)
                {
                    line = $"{matrix[i, j]} ";
                }
                file.WriteLine(line);
            }
            file.Close();
            if (fileOut.Exists)
            {
                fileOut.MoveTo(filePath);
            }
        }
    }
}