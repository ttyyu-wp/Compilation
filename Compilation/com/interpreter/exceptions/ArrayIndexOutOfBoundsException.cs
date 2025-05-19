using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilation.com.interpreter.exceptions
{
    class ArrayIndexOutOfBoundsException : Exception
    {
        public ArrayIndexOutOfBoundsException(string msg) : base(msg)
        {

        }
    }
}
