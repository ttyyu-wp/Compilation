using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.lex;

namespace Compilation.bn.syntax.synnode
{
    public class IfElseNode : SyntaxNode
    {
        public SyntaxNode logicExpr;
        public SyntaxNode ifExpr;
        public SyntaxNode elseExpr;
        public Token ifToken;

        public IfElseNode(SyntaxNode logicExpr, SyntaxNode ifExpr, SyntaxNode elseExpr, Token ifToken)
        {
            this.logicExpr = logicExpr;
            this.ifExpr = ifExpr;
            this.elseExpr = elseExpr;
            this.ifToken = ifToken;
        }

        public string toString()
        {
            return "IfElseNode: " + ifToken.TokenString() + "\n";
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            // 返回当前节点的所有子节点：logicExpr, ifExpr, 和 elseExpr
            if (logicExpr != null)
            {
                yield return logicExpr;
            }
            if (ifExpr != null)
            {
                yield return ifExpr;
            }
            if (elseExpr != null)
            {
                yield return elseExpr;
            }
        }
    }
}
