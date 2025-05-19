using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilation.com.interpreter.result
{
     public class NumRunResult : RunResult
    {
        public bool isInt;
        public long intV;
        public double floatV;

        public NumRunResult(bool isInt, long intV, double floatV)
        {
            this.isInt = isInt;
            this.intV = intV;
            this.floatV = floatV;
        }

        //加负号
        public void setNegative()
        {
            if (this.isInt)
            {
                intV = -intV;
            }
            else
            {
                floatV = -floatV;
            }
        }

        //比较运算
        public RunResult comp(RunResult v, string compOP)
        {
            double selfV = 0;
            double otherV = 0;
            NumRunResult other = (NumRunResult)v;

            if (this.isInt)
            {
                selfV = this.intV;
            }
            else
            {
                selfV = this.floatV;
            }
            if (other.isInt)
            {
                otherV = other.intV;
            }
            else
            {
                otherV = other.floatV;
            }

            if (compOP.Equals(">"))
            {
                return new BoolRunResult(selfV > otherV);
            }
            else if (compOP.Equals("<"))
            {
                return new BoolRunResult(selfV < otherV);
            }
            else if (compOP.Equals(">="))
            {
                return new BoolRunResult(selfV >= otherV);
            }
            else if (compOP.Equals("<="))
            {
                return new BoolRunResult(selfV <= otherV);
            }
            else if (compOP.Equals("=="))
            {
                return new BoolRunResult(selfV == otherV);
            }
            else if (compOP.Equals("!="))
            {
                return new BoolRunResult(selfV != otherV);
            }

            return null;
        }

        //加法运算
        public RunResult add(RunResult v)
        {
            if (v is StringRunResult)
            {
                string sori = null;
                if (this.isInt)
                {
                    sori = "" + this.intV;
                }
                else
                {
                    sori = "" + this.floatV;
                }
                return new StringRunResult(sori + ((StringRunResult)v).str);
            }
            
            if (this.isInt && (((NumRunResult)v).isInt))
            {
                return new NumRunResult(true, this.intV + ((NumRunResult)v).intV, 0);
            }
            else
            {
                double vl = 0;
                double vr = 0;
                if (this.isInt)
                {
                    vl = this.intV;
                }
                else
                {
                    vl = this.floatV;
                }
                if (((NumRunResult)v).isInt)
                {
                    vr = ((NumRunResult)v).intV;
                }
                else
                {
                    vr = ((NumRunResult)v).floatV;
                }
                return new NumRunResult(false, 0, Math.Round(vl + vr, 5, MidpointRounding.AwayFromZero));
            }
        }

        //减法运算
        public NumRunResult sub (NumRunResult v)
        {
            if (this.isInt && v.isInt)
            {
                return new NumRunResult (true, this.intV - v.intV, 0);
            }
            else
            {
                double vl = 0;
                double vr = 0;
                if (this.isInt)
                {
                    vl = this.intV;
                }
                else
                {
                    vl = this.floatV;
                }
                if (v.isInt)
                {
                    vr = v.intV;
                }
                else
                {
                    vr = v.floatV;
                }
                return new NumRunResult(false , 0, Math.Round(vl - vr, 5, MidpointRounding.AwayFromZero));
            }
        }

        //乘法运算
        public NumRunResult mul (NumRunResult v)
        {
            if (this.isInt && v.isInt)
            {
                return new NumRunResult (true, this.intV * v.intV, 0);
            }
            else
            {
                double vl = 0;
                double vr = 0;
                if (this.isInt)
                {
                    vl = this.intV;
                }
                else
                {
                    vl = this.floatV;
                }
                if (v.isInt)
                {
                    vr = v.intV;
                }
                else
                {
                    vr = v.floatV;
                }
                return new NumRunResult(false, 0, Math.Round(vl * vr, 5, MidpointRounding.AwayFromZero));
            }
        }
        
        //除法运算
        public NumRunResult div (NumRunResult v)
        {
            if (this.isInt && v.isInt)
            {
                return new NumRunResult (true, this.intV / v.intV, 0);
            }
            else
            {
                double vl = 0;
                double vr = 0;
                if (this.isInt)
                {
                    vl = this.intV;
                }
                else
                {
                    vl = this.floatV;
                }
                if (v.isInt)
                {
                    vr = v.intV;
                }
                else
                {
                    vr = v.floatV;
                }
                return new NumRunResult(false, 0, Math.Round(vl / vr, 5, MidpointRounding.AwayFromZero));
            }
        }

        //整数取模运算
        public NumRunResult modInt (NumRunResult v)
        {
            return new NumRunResult(true, this.intV % v.intV, 0);
        }
        //浮点数取模运算
        public NumRunResult modFloat (NumRunResult v)
        {
            return new NumRunResult(false, 0, Math.IEEERemainder(this.floatV, v.floatV));
        }

        //乘方运算
        public NumRunResult exp (NumRunResult v)
        {
            if (this.isInt && v.isInt)
            {
                double result = Math.Pow(this.intV, v.intV);
                if (Math.Round(result) == result)
                {
                    return new NumRunResult(true, (long)result, 0);
                }
                else
                {
                    return new NumRunResult(false, 0, result);  
                }
            }
            else
            {
                double vl = 0;
                double vr = 0;
                if (this.isInt)
                {
                    vl = this.intV;
                }
                else
                {
                    vl = this.floatV;
                }
                if (v.isInt)
                {
                    vr = v.intV;
                }
                else
                {
                    vr = v.floatV;
                }
                return new NumRunResult(false, 0, Math.Round(Math.Pow(vl, vr), 5, MidpointRounding.AwayFromZero));
            }
        }

        public string toString()
        {
            if (this.isInt)
            {
                return this.intV + "";
            }
            else
            {
                return this.floatV + "";
            }
        }
    }
}
