using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.bn.syntax.synnode;
using Compilation.lex;

namespace Compilation.com.interpreter.exceptions
{
    class BreakException : Exception
    {
        public SyntaxNode sn { get; }

        public BreakException(SyntaxNode sn) : base("A break exception occurred.")
        {
            this.sn = sn;
        }
    }
}
