using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Compilation.bn.syntax.synnode;
using Compilation.com.interpreter.result;

namespace Compilation.lex 
{
    class MyException : Exception
    {
        public SyntaxNode sn;
        public RunResult result;

        public MyException(string msg) : base(msg)
        {
        }

        public MyException(SyntaxNode sn, string msg) : base($"{sn}: {msg}") 
        {
            this.sn = sn;
        }

        public MyException(RunResult result, string msg) : base($"{result}: {msg}")
        {
            this.result = result;
        }
    }
}
