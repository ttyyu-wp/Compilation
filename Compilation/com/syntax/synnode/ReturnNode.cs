using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.lex;

namespace Compilation.bn.syntax.synnode
{
    public class ReturnNode : SyntaxNode
    {
        public Token t;
        public SyntaxNode expr;

        public ReturnNode(Token t, SyntaxNode expr)
        {
            this.t = t;
            this.expr = expr;
        }

        public string toString()
        {
            return "ReturnNode" + t.TokenString() + "\n";
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            // 返回当前节点的子节点：expr
            if (expr != null)
            {
                yield return expr;
            }
        }
    }
}
