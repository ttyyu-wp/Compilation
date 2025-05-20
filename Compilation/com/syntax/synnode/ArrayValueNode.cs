using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.lex;

namespace Compilation.bn.syntax.synnode
{
    public class ArrayValueNode : SyntaxNode
    {
        public Token id;
        public SyntaxNode index;

        public ArrayValueNode (Token id, SyntaxNode index)
        {
            this.id = id;
            this.index = index;
        }

        public string toString()
        {
            return "ArrayValue：" + id.TokenString();
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            // 返回当前节点的子节点：index
            if (index != null)
            {
                yield return index;
            }
        }
    }
}
