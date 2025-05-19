using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.com.interpreter.result;

namespace Compilation.com.interpreter
{
    public class GlobalArray
    {
        public VariableTypes type;
        public long[] intV;
        public double[] floatV;
        public string[] stringV;
        public bool[] boolV;

        public GlobalArray(string typeStr, long length)
        {
            if (typeStr.Equals("int"))
            {
                this.type = VariableTypes.INT;
                intV = new long[(int)length];
            }
            else if (typeStr.Equals("float")) 
            {
                this.type = VariableTypes.FLOAT;
                floatV = new double[(int)length];
            }
            else if (typeStr.Equals("string"))
            {
                this.type = VariableTypes.STRING;
                stringV = new string[(int)length];
            }
            else if (typeStr.Equals("bool")) 
            {
                this.type = VariableTypes.BOOL;
                boolV = new bool[(int)length];
            }
        }

        public void setValue(long index, NumRunResult nrr, string stringV, bool boolV)
        {
            if (type == VariableTypes.INT)
            {
                intV[(int)index] = nrr.intV;
            }
            //TODO: float数组赋值精度问题
            else if (type == VariableTypes.FLOAT)
            {
                floatV[(int)index] = Math.Round(nrr.floatV, 7, MidpointRounding.AwayFromZero);
            }
            else if (type == VariableTypes.STRING)
            {
                this.stringV[(int)index] = stringV;
            }
            else if (type == VariableTypes.BOOL)
            {
                this.boolV[(int)index] = boolV;
            }
        }
    }
}
