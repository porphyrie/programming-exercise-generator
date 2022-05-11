﻿using System;
using System.Collections.Generic;
using System.Text;

namespace GrammarTest
{
    public class GrammarException : Exception
    {
        public GrammarException()
        {
        }

        public GrammarException(string message)
            : base(message)
        {
        }

        public GrammarException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}
