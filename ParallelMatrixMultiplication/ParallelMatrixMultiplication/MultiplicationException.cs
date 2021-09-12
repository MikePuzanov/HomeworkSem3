using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelMatrixMultiplication
{
    /// <summary>
    /// then we couldn't multiplication matrices
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