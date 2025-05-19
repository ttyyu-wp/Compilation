using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.lex;

namespace Compilation.bn.syntax.synnode
{
    public class BoolNode : SyntaxNode
    {
        public Token t;

        public BoolNode(Token t)
        {
            this.t = t;
        }

        public string toString()
        {
            return "BoolNode: " + t.TokenString() + "\n";
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}
