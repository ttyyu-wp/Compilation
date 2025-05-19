using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilation.bn.syntax.synnode
{
    public class ReadNode : SyntaxNode
    {
        public ReadNodeType rnt;

        public ReadNode(ReadNodeType rnt)
        {
            this.rnt = rnt;
        }

        public string toString()
        {
            return "ReadNode" + "\n";
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}
