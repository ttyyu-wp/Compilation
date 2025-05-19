using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.lex;

namespace Compilation.bn.syntax.synnode
{
    public class CallNode : SyntaxNode
    {
        public Token funName;
        public List<SyntaxNode> paraname = new List<SyntaxNode>();

        public CallNode(Token funName)
        {
            this.funName = funName;
        }

        public void addParam(SyntaxNode param)
        {
            paraname.Add(param);
        }

        public string toString()
        {
            return "CallName: " + funName.TokenString() + "\n";
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            // 返回当前节点的所有子节点：paraname中的所有元素
            foreach (var param in paraname)
            {
                yield return param;
            }
        }


    }
}
