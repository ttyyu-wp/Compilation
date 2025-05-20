using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.lex;

namespace Compilation.bn.syntax.synnode
{
    public class ArrayAssignNode : SyntaxNode
    {
        public Token id;
        public SyntaxNode index;
        public SyntaxNode value;

        public ArrayAssignNode(Token id, SyntaxNode index, SyntaxNode value)
        {
            this.id = id;
            this.index = index;
            this.value = value;
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            yield return index;
            yield return value;
        }

        public string toString()
        {
            return "ArrayAssign: " + id.TokenString() + "\n";
        }
    }
}
