using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilation.com.interpreter.stack
{
    public class SingleRunStack
    {
        public Dictionary<string, Variable> currVmap;
        public SingleRunStackType type;

        public SingleRunStack(SingleRunStackType type)
        {
            currVmap = new Dictionary<string, Variable>();
            this.type = type;
        }
    }
}
