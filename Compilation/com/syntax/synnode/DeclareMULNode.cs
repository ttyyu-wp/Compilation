using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.lex;

namespace Compilation.bn.syntax.synnode
{
    public class DeclareMULNode : SyntaxNode
    {
        public Token kw;
        public List<SyntaxNode> assignList = new List<SyntaxNode>();

        public DeclareMULNode(Token kw)
        {
            this.kw = kw;
        }

        public string toString()
        {
            return "DeclareMULNode: 声明 " + kw.TokenString() + "\n";
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            // 返回当前节点的所有子节点：assignList中的所有元素
            foreach (var node in assignList)
            {
                yield return node;
            }
        }
    }
}
