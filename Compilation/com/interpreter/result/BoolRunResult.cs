using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilation.com.interpreter.result
{
    public class BoolRunResult : RunResult
    {
        public bool b;

        public BoolRunResult(bool b)
        {
            this.b = b;
        }

        public void not()
        {
            this.b = !this.b;
        }

        public RunResult add(RunResult v)
        {
            string sin = null;
            if (v is StringRunResult)
            {
                sin = ((StringRunResult)v).str;
            }
            return new StringRunResult(b + sin);
        }

        public string toString()
        {
            return b + "";
        }
    }
}
