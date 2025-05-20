using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.lex;

namespace Compilation.com.interpreter.result
{
    class TriAndLogRunResult : RunResult
    {
        public double num;

        public TriAndLogRunResult(double num)
        {
            this.num = num;
        }

        //加
        public RunResult add(RunResult v)
        {
            if (v is StringRunResult)
            {
                string str = null;
                str = "" + this.num;

                return new StringRunResult(str + ((StringRunResult)v).str);
            }
            else if (v is NumRunResult)
            {
                double vNum = 0;
                if (((NumRunResult)v).isInt)
                {
                    vNum = ((NumRunResult)v).intV;
                }
                else
                {
                    vNum = ((NumRunResult)v).floatV;
                }
                return new NumRunResult(false, 0, Math.Round(num + vNum, 5, MidpointRounding.AwayFromZero));
            }
            else if (v is TriAndLogRunResult)
            {
                return new NumRunResult(false, 0, Math.Round(num + ((TriAndLogRunResult)v).num, 5, MidpointRounding.AwayFromZero));
            }
            else
            {
                throw new MyException("TriAndLogRunResult error!");
            }
        }

        public string toString()
        {
            return "" + this.num;
        }

        internal RunResult div(RunResult rrr)
        {
            if (rrr is NumRunResult)
            {
                NumRunResult vr = (NumRunResult)rrr;
                if (vr.isInt)
                {
                    return new NumRunResult(false, 0, Math.Round(this.num / vr.intV, 5, MidpointRounding.AwayFromZero));
                }
                else
                {
                    return new NumRunResult(false, 0, Math.Round(this.num / vr.floatV, 5, MidpointRounding.AwayFromZero));
                }
            }
            else
            {
                return new NumRunResult(false, 0, Math.Round(this.num / ((TriAndLogRunResult)rrr).num, 5, MidpointRounding.AwayFromZero));
            }
        }

        internal RunResult exp(RunResult rrr)
        {
            if (rrr is NumRunResult)
            {
                NumRunResult vr = (NumRunResult)rrr;
                if (vr.isInt)
                {
                    return new NumRunResult(false, 0, Math.Round(Math.Pow(this.num, vr.intV), 5, MidpointRounding.AwayFromZero));
                }
                else
                {
                    return new NumRunResult(false, 0, Math.Round(Math.Pow(this.num, vr.floatV), 5, MidpointRounding.AwayFromZero));
                }
            }
            else
            {
                return new NumRunResult(false, 0, Math.Round(Math.Pow(this.num, ((TriAndLogRunResult)rrr).num), 5, MidpointRounding.AwayFromZero));
            }
        }

        internal RunResult mod(RunResult rrr)
        {
            if (rrr is NumRunResult)
            {
                NumRunResult vr = (NumRunResult)rrr;
                if (vr.isInt)
                {
                    return new NumRunResult(false, 0, Math.IEEERemainder(this.num, vr.intV));
                }
                else
                {
                    return new NumRunResult(false, 0, Math.IEEERemainder(this.num, vr.floatV));
                }
            }
            else
            {
                return new NumRunResult(false, 0, Math.IEEERemainder(this.num, ((TriAndLogRunResult)rrr).num));
            }
        }

        internal RunResult mul(RunResult rrr)
        {
            if (rrr is NumRunResult)
            {
                NumRunResult vr = (NumRunResult)rrr;
                if (vr.isInt)
                {
                    return new NumRunResult(false, 0, Math.Round(this.num * vr.intV, 5, MidpointRounding.AwayFromZero));
                }
                else
                {
                    return new NumRunResult(false, 0, Math.Round(this.num * vr.floatV, 5, MidpointRounding.AwayFromZero));
                }
            }
            else
            {
                return new NumRunResult(false, 0, Math.Round(this.num * ((TriAndLogRunResult)rrr).num, 5, MidpointRounding.AwayFromZero));
            }
        }

        internal RunResult sub(RunResult rrr)
        {
            if (rrr is NumRunResult)
            {
                NumRunResult vr = (NumRunResult)rrr;
                if (vr.isInt)
                {
                    return new NumRunResult(false, 0, Math.Round(this.num - vr.intV, 5, MidpointRounding.AwayFromZero));
                }
                else
                {
                    return new NumRunResult(false, 0, Math.Round(this.num - vr.floatV, 5, MidpointRounding.AwayFromZero));
                }
            }
            else
            {
                return new NumRunResult(false, 0, Math.Round(this.num - ((TriAndLogRunResult)rrr).num, 5, MidpointRounding.AwayFromZero));
            }
        }
    }
}
