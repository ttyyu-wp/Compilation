using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.lex;

namespace Compilation.bn.syntax.synnode
{
    public class NumberNode : SyntaxNode
    {
        public Token t;

        public NumberNode(Token t)
        {
            this.t = t;
        }

        public string toString()
        {
            return "NumberNode: " + t.TokenString() + "\n";
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}
