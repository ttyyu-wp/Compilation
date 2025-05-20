using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilation.bn.syntax.synnode
{
    public class AssignNode : SyntaxNode
    {
        public SyntaxNode left;
        public SyntaxNode right;

        public AssignNode(SyntaxNode left, SyntaxNode right)
        {
            this.left = left;
            this.right = right;
        }

        public string toString()
        {
            return "Assign \n";
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
