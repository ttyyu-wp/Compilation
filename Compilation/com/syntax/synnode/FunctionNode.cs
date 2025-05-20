using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.lex;

namespace Compilation.bn.syntax.synnode
{
    public class FunctionNode : SyntaxNode
    {
        public Token funName;
        public List<SyntaxNode> paraList = new List<SyntaxNode>();
        public SyntaxNode funBody;

        public FunctionNode(Token funName)
        {
            this.funName = funName;
        }

        public void addParam(SyntaxNode dn)
        {
            paraList.Add(dn);
        }

        public void setBody(SyntaxNode funBody)
        {
            this.funBody = funBody;
        }

        public string toString()
        {
            return "Function: " + funName.TokenString() + "\n";
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            // 返回当前节点的所有子节点：先paraList后funBody
            foreach (var param in paraList)
            {
                yield return param;
            }
            if (funBody != null)
            {
                yield return funBody;
            }
        }
    }
}
