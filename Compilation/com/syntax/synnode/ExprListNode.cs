using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilation.bn.syntax.synnode
{
    public class ExprListNode : SyntaxNode
    {
        public List<SyntaxNode> list = new List<SyntaxNode>();
        public SyntaxNode father;
        public ExprListNode(SyntaxNode father)
        {
            this.father = father;
        }
        public void add(SyntaxNode node)
        {
            list.Add(node);
        }
        public string toString()
        {
            return "ExprList: " + list.Count + "\n";
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            // 返回当前节点的所有子节点：list中的所有元素
            foreach (var node in list)
            {
                yield return node;
            }
        }
    }
}
