using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilation.com.interpreter.stack
{
    public class RunStackUtil
    {
        public static List<SingleRunStack> callStack;
        public static int top;

        public static void init()
        {
            callStack = new List<SingleRunStack>();
            top = -1;
        }

        public static void push (SingleRunStack currRS)
        {
            callStack.Add(currRS);
            top++;
        }

        public static void pop ()
        {
            callStack.RemoveAt(top);
            top--;
        }

        public static SingleRunStack topP ()
        {
            return callStack[top];
        }

        public static bool isVarExist(string varName)
        {
            int tempIndex = top;
            while (true)
            {
                if (callStack[tempIndex].currVmap.ContainsKey(varName))
                {
                    return true;
                }
                //TODO: callStack[tempIndex].type 可能存在问题
                if (callStack[tempIndex].type == SingleRunStackType.CIA)
                {
                    break;
                }
                tempIndex--;
            }
            return false;
        }

        public static void addVar(string varName, Variable v)
        {
            callStack[top].currVmap.Add(varName, v);
        } 

        public static Variable getVar(string varName)
        {
            int tempIndex = top;
            while (true)
            {
                if (callStack[tempIndex].currVmap.ContainsKey(varName))
                {
                    return callStack[tempIndex].currVmap[varName];
                }
                //TODO: callStack[tempIndex].type 可能存在问题
                if (callStack[tempIndex].type == SingleRunStackType.CIA)
                {
                    break;
                }
                tempIndex--;
            }
            return null;
        }


    }
}
