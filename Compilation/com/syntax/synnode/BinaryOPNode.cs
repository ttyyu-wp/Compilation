using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.lex;

namespace Compilation.bn.syntax.synnode
{
    public class BinaryOPNode : SyntaxNode
    {
        public Token t;
        public SyntaxNode left;
        public SyntaxNode right;

        public BinaryOPNode(SyntaxNode left, Token t, SyntaxNode right)
        {
            this.t = t;
            this.left = left;
            this.right = right;
        }

        public string toString()
        {
            return "BinaryOPNode: " + t.TokenString() + "\n";
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            // 返回当前节点的所有子节点：left 和 right
            if (left != null)
            {
                yield return left;
            }
            if (right != null)
            {
                yield return right;
            }
        }
    }
}
