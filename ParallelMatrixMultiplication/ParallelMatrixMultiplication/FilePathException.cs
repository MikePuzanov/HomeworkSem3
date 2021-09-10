using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelMatrixMultiplication
{
    public class FilePathException : Exception
    {
        public FilePathException()
        {
        }

        public FilePathException(string message)
            : base(message)
        {
        }
    }
}