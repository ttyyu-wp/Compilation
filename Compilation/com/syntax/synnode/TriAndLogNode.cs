using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.lex;

namespace Compilation.bn.syntax.synnode
{
    public class TriAndLogNode : SyntaxNode
    {
        public TriAndLogNodeType Type;
        public SyntaxNode Node;
        public Token t;

        public TriAndLogNode(TriAndLogNodeType type, Token token, SyntaxNode node)
        {
            Type = type;
            t = token;
            Node = node;
        }

        public IEnumerable<SyntaxNode> getChild()
        {
            if (Node != null)
            {
                yield return Node;
            }
        }

        public string toString()
        {
            return "TriAndLogNode" + t.TokenString() + "\n";
        }
    }
}
