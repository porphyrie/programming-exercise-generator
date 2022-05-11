using System;
using System.Collections.Generic;
using System.Text;

namespace GrammarTest
{
    public class SyntaxTreeException : Exception
    {
        public SyntaxTreeException()
        {
        }

        public SyntaxTreeException(string message)
            : base(message)
        {
        }

        public SyntaxTreeException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
