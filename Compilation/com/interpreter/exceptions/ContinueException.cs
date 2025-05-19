using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.bn.syntax.synnode;

namespace Compilation.com.interpreter.exceptions
{
    class ContinueException : Exception
    {
        public SyntaxNode sn { get; }

        public ContinueException(SyntaxNode sn) : base("A break exception occurred.")
        {
            this.sn = sn;
        }
    }
}
