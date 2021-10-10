using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelMatrixMultiplication
{
    public class WrongFilePathException : Exception
    {
        /// <summary>
        /// then filePath is abnormal
        /// </summary>
        public WrongFilePathException()
        {
        }

        public WrongFilePathException(string message)
            : base(message)
        {
        }
    }
}