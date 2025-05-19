using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.com.interpreter.result;

namespace Compilation.com.interpreter
{
    public class Variable
    {
        public VariableTypes type;
        public long intV;
        public double floatV;
        public string stringV;
        public bool boolV;

        public bool initFlag;

        public Variable(string typeStr)
        {
            if (typeStr.Equals("int"))
            {
                this.type = VariableTypes.INT;
            } 
            if (typeStr.Equals("float"))
            {
                this.type = VariableTypes.FLOAT;
            } 
            if (typeStr.Equals("string"))
            {
                this.type = VariableTypes.STRING;
            } 
            if (typeStr.Equals("bool"))
            {
                this.type = VariableTypes.BOOL;
            } 

        }

        public Variable(VariableTypes type)
        {
            this.type = type;
            initFlag = false;
        }

        public void setValue(NumRunResult nrr, string stringV, bool boolV)
        {
            initFlag = true;
            if (type == VariableTypes.INT)
            {
                this.intV = nrr.intV;
            }
            else if (type == VariableTypes.FLOAT)
            {
                this.floatV = nrr.floatV;
            }
            else if (type == VariableTypes.STRING)
            {
                this.stringV = stringV;
            }
            else if (type == VariableTypes.BOOL)
            {
                this.boolV = boolV;
            }
        }

        public RunResult getValue()
        {
            if (type == VariableTypes.INT)
            {
                return new NumRunResult(true, intV, 0);
            }
            else if (type == VariableTypes.FLOAT)
            {
                return new NumRunResult(false, 0, floatV);
            }
            else if (type == VariableTypes.STRING)
            {
                return new StringRunResult(stringV);
            }
            else if (type == VariableTypes.BOOL)
            {
                return new BoolRunResult(boolV);
            }
            return null;
        }
    }
}
