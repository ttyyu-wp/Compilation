using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.lex;

namespace Compilation.bn.syntax.synnode
{
    public class ArrayDeclareNode : SyntaxNode
    {
        public Token kw;
        public Token id;
        public long length;

        public ArrayDeclareNode(Token kw, Token id, long length)
        {
            this.kw = kw;
            this.id = id;
            this.length = length;
        }

        public string toString()
        {
            return "ArrayDeclare: " + kw.TokenString() + " " + id.TokenString() + "[ " + length + " ]\n";
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}
