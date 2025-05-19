using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.lex;

namespace Compilation.bn.syntax.synnode
{
    public class ContinueNode : SyntaxNode
    {
        public Token t;

        public ContinueNode(Token t)
        {
            this.t = t;
        }

        public string toString()
        {
            return "ContinueNode: " + t.TokenString() + "\n";
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}
