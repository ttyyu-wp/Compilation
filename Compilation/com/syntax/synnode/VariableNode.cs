using System.Collections.Generic;
using System.Linq;
using Compilation.bn.syntax.synnode;
using Compilation.lex;

namespace Compilation.bn.syntax
{
    public class VariableNode : SyntaxNode
    {
        public Token t;

        public VariableNode(Token t)
        {
            this.t = t;
        }

        public string toString()
        {
            return "VariableNode: " + t.TokenString() + "\n";
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            // VariableNode没有子节点，返回空集合
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}