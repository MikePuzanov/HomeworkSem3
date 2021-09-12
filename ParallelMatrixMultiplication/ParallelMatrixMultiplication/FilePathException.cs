using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelMatrixMultiplication
{
    public class FilePathException : Exception
    {
        /// <summary>
        /// then filePath is abnormal
        /// </summary>
        public FilePathException()
        {
        }

        public FilePathException(string message)
            : base(message)
        {
        }
    }
}