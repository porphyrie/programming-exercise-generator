using System;
using System.Collections.Generic;
using System.Text;

namespace GrammarTest
{
    public class GeneratorException : Exception
    {
        public GeneratorException()
        {
        }

        public GeneratorException(string message)
            : base(message)
        {
        }

        public GeneratorException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
