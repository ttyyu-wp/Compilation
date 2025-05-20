using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.lex;

namespace Compilation.bn.syntax.synnode
{
    public class BreakNode : SyntaxNode
    {
        public Token t;

        public BreakNode(Token t)
        {
            this.t = t;
        }

        public string toString()
        {
            return "Break: " + t.TokenString() + "\n";
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            // BreakNode没有子节点，返回空集合
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}
