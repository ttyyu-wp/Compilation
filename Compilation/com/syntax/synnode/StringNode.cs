using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.lex;

namespace Compilation.bn.syntax.synnode
{
    public class StringNode : SyntaxNode
    {
        public Token t;

        public StringNode(Token t)
        {
            this.t = t;
        }

        public string toString()
        {
            return "String: " + t.TokenString() + "\n";
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            // StringNode没有子节点，返回空集合
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}
