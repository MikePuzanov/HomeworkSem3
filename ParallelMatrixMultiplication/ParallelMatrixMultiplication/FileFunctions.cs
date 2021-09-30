﻿using System;
using System.IO;

namespace ParallelMatrixMultiplication
{
    /// <summary>
    /// Functions for working with files
    /// </summary>
    public static class FileFunctions
    {
        private static void CheckFilePath(string filePath)
        {
            if (string.IsNullOrEmpty(filePath))
            {
                throw new FilePathException("Error file path!");
            }
        }
        
        private static (int, int) CountMatrixSize(string filePath)
        {
            CheckFilePath((filePath));
            using var file = new StreamReader(filePath);
            string line = file.ReadLine();
            int size = 0;
            if (string.IsNullOrEmpty(line))
            {
                throw new EmptyFileException("File is empty!");
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
        /// Create Matrix from file
        /// </summary>
        public static int[,] CreateMatrix(string filePath)
        {
            CheckFilePath((filePath));
            (int length, int width) size = CountMatrixSize((filePath));
            var matrix = new int[size.length, size.width];
            using var file = new StreamReader(filePath);
            string line = file.ReadLine();
            var index = 0;
            while (line != null)
            {
                string[] lineDrop = line.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < size.length; ++i)
                {
                    matrix[index, i] = Int32.Parse(lineDrop[i]);
                }
                index++;
                line = file.ReadLine();
            }
            return matrix;
        }

        /// <summary>
        /// Create file and return matrix
        /// </summary>
        public static void CreateFileWithMatrix(string filePath, int[,] matrix)
        {
            CheckFilePath((filePath));
            var fileOut = new FileInfo(filePath);
            if (fileOut.Exists)
            {
                fileOut.Delete();
            }
            using var newFile = new FileStream(filePath, FileMode.Create);
            using var file = new StreamWriter(newFile);
            for (int i = 0; i < matrix.GetLength(0); i++)
            {
                string line = "";
                for (int j = 0; j < matrix.GetLength(1); j++)
                {
                    line += $"{matrix[i, j]} ";
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