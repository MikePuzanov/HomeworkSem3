﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ParallelMatrixMultiplication
{
    /// <summary>
    /// then file is empty
    /// </summary>
    public class EmptyFileException : Exception
    {
        public EmptyFileException()
        {
        }

        public EmptyFileException(string message)
            : base(message)
        {
        }
    }
}