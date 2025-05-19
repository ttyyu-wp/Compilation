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

        public static int GetDecimalPlaces(double value)
        {
            string s = value.ToString("G17", CultureInfo.InvariantCulture).TrimEnd('0');
            if (s.Contains("."))
            {
                return s.Split('.')[1].Length;
            }
            return 0;
        }

        public static double RoundToSignificantDigits(double value, int digits)
        {
            if (value == 0) return 0;

            double scale = Math.Pow(10, Math.Floor(Math.Log10(Math.Abs(value))) + 1);
            return Math.Round(value / scale, digits - 1) * scale;
        }

        public static double getResult(double a, double b, double result, string operation, int maxPrecision = 15)
        {
           

            // 根据输入值的小数位数决定输出精度
            int decimalA = GetDecimalPlaces(a);
            int decimalB = GetDecimalPlaces(b);

            int totalDecimal = Math.Max(decimalA, decimalB); // 取最大作为参考
            int precision = Math.Min(totalDecimal + 1, maxPrecision); // 动态调整精度

            // 对于加减法：保留与最大小数位一致
            // 对于乘除法/取模/乘方：可考虑用有效数字控制
            if (operation.Equals("add") || operation.Equals("sub"))
            {
                return Math.Round(result, totalDecimal);
            }
            else
            {
                // 对于其他运算（如乘除、幂、模），保留一定数量的有效数字
                return RoundToSignificantDigits(result, precision);
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
