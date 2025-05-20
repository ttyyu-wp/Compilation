using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilation.bn.syntax.synnode
{
    public class WhileNode : SyntaxNode
    {
        public SyntaxNode logicExpr;
        public SyntaxNode whileExpr;

        public WhileNode(SyntaxNode logicExpr, SyntaxNode whileExpr)
        {
            this.logicExpr = logicExpr;
            this.whileExpr = whileExpr;
        }

        public string toString()
        {
            return "While \n";
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            // 返回当前节点的子节点：logicExpr 和 whileExpr
            yield return logicExpr;
            yield return whileExpr;
        }
    }
}
