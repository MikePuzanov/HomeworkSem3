using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelMatrixMultiplication
{
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