using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.lex;

namespace Compilation.bn.syntax.synnode
{
    public class UnaryOPNode : SyntaxNode
    {
        public Token t;
        public SyntaxNode right;

        public UnaryOPNode(Token t, SyntaxNode right)
        {
            this.t = t;
            this.right = right;
        }

        public string toString()
        {
            return "UnaryOPNode: " + t.TokenString() + "\n";
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            // 返回当前节点的子节点：right
            if (right != null)
            {
                yield return right;
            }
        }
    }
}
