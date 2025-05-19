using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Compilation.lex;

namespace Compilation.com.utils
{
    public class PrintToken
    {
        public static void print(List<Token> tokenList)
        {
            foreach (Token token in tokenList)
            {
                MainMenu.form1.tokenOutArea.AppendText(token.TokenString());
            }
        }
    }
}
