using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.bn.syntax.synnode;
using Compilation.lex;

namespace Compilation.bn.printToTest
{
    public class PrintSyntaxNode
    {
        public static void print(SyntaxNode node, int depth = 0)
        {
            MainMenu.form1.syntaxNodeOutArea.AppendText(new string(' ', depth * 2) + node.toString());

            var children = node.getChild();
            if (children != null)
            {
                foreach (var child in children)
                {
                    print(child, depth + 1);
                }
            }

        }

        
    }
}
