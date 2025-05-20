using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.lex;

namespace Compilation.com.interpreter.result
{
    public class StringRunResult : RunResult
    {
        public string str;

        public StringRunResult(string str)
        {
            this.str = str;
        }

        public RunResult add(RunResult v)
        {
            string sin = null;
            if (v is StringRunResult)
            {
                sin = ((StringRunResult)v).str;
            }
            else if (v is BoolRunResult)
            {
                sin = ((BoolRunResult)v).b + "";
            }
            else if (v is TriAndLogRunResult)
            {
                sin = ((TriAndLogRunResult)v).num + "";
            }
            else
            {
                NumRunResult nrr = (NumRunResult)v;
                if (nrr.isInt)
                {
                    sin = "" + nrr.intV;
                }
                else
                {
                    sin = "" + nrr.floatV;
                }
            }
            return new StringRunResult(str + sin);
        }

        //字符串乘方
        public RunResult exp(RunResult v)
        {
            StringBuilder sb = new StringBuilder();
            NumRunResult nrr = (NumRunResult)v;
            for (long i = 0; i < nrr.intV; i++)
            {
                sb.Append(str);
            }
            return new StringRunResult(sb.ToString());
        }

        public string toString()
        {
            return str;
        }
    }
}
