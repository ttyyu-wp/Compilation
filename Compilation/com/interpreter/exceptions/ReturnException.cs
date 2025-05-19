using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.com.interpreter.result;

namespace Compilation.com.interpreter.exceptions
{
    class ReturnException : Exception
    {
        public RunResult result;

        public ReturnException(RunResult result)
        {
            this.result = result;
        }
    }
}
