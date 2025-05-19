using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.lex;

namespace Compilation.bn.syntax.synnode
{
    public class DeclareNode : SyntaxNode
    {
        public Token kw;
        public Token id;
        public DeclareNode(Token kw, Token id)
        {
            this.kw = kw;
            this.id = id;
        }

        public string toString()
        {
            return "声明: " + kw.TokenString() + " " + id.TokenString() + "\n";
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            // DeclareNode没有子节点，返回空集合
            return Enumerable.Empty<SyntaxNode>();
        }
    }
}
