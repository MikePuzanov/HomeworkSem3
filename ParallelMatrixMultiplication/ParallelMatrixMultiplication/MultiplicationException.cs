using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelMatrixMultiplication
{
    /// <summary>
    /// then matrices cannott be multiplied
    /// </summary>
    public class MultiplicationException : Exception
    {
        public MultiplicationException()
        {
        }

        public MultiplicationException(string message)
            : base(message)
        {
        }
    }
}