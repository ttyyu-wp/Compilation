using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilation.bn.syntax.synnode
{
    public interface SyntaxNode
    {
        string toString();
        IEnumerable<SyntaxNode> getChild();
    }
}
