using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilation.bn.syntax.synnode
{
    public class PrintNode : SyntaxNode
    {
        public SyntaxNode exprForPrint;

        public PrintNode(SyntaxNode exprForPrint)
        {
            this.exprForPrint = exprForPrint;
        }

        public string toString()
        {
            return "PrintNode" + "\n";
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            // 返回当前节点的子节点：exprForPrint
            if (exprForPrint != null)
            {
                yield return exprForPrint;
            }
        }
    }
}
