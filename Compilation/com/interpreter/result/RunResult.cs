using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilation.com.interpreter.result
{
    public interface RunResult
    {
        RunResult add(RunResult v);
        string toString();

    }
}
