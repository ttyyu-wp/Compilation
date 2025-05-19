using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilation.lex
{
    public class Token
    {
        //语素的类型
        public TokenType type;
        //语素的内容
        public string value;
        //语素在代码中的行列号
        public int row;
        public int col;
        
        public Token(TokenType type, int row, int col)
        {
            this.type = type;
            this.row = row;
            this.col = col;
        }

        public Token(TokenType type, int row, int col, string value)
        {
            this.type = type;
            this.value = value;
            this.row = row;
            this.col = col;
        }

        public string TokenString()
        {
            if (value != null)
            {
                return type.ToString() + "  " + row + "行" + col + "列 " + ((value.StartsWith("\"") && value.EndsWith("\"")) ? ("\"" + value + "\"") : value) + "\r\n";
            }
            else
            {
                switch (type)
                {
                    case TokenType.PLUS:
                        return "PLUS  " + row + "行" + col + "列 '+'" + "\r\n";
                    case TokenType.MINUS:
                        return "MINUS  " + row + "行" + col + "列 '-'" + "\r\n";
                    case TokenType.MUL:
                        return "MUL  " + row + "行" + col + "列 '*'" + "\r\n";
                    case TokenType.DIV:
                        return "DIV  " + row + "行" + col + "列 '/'" + "\r\n";
                    case TokenType.MODULAR:
                        return "MODULAR  " + row + "行" + col + "列 '%'" + "\r\n";
                    case TokenType.EXP:
                        return "EXP  " + row + "行" + col + "列 '**'" + "\r\n";
                    case TokenType.SIN:
                        return "SIN  " + row + "行" + col + "列 'sin'" + "\r\n";
                    case TokenType.COS:
                        return "COS  " + row + "行" + col + "列 'cos'" + "\r\n";
                    case TokenType.TAN:
                        return "TAN  " + row + "行" + col + "列 'tan'" + "\r\n";
                    case TokenType.LOG:
                        return "LOG  " + row + "行" + col + "列 'log'" + "\r\n";
                    case TokenType.AND:
                        return "AND  " + row + "行" + col + "列 '&'" + "\r\n";
                    case TokenType.OR:
                        return "OR  " + row + "行" + col + "列 '|'" + "\r\n";
                    case TokenType.NOT:
                        return "NOT  " + row + "行" + col + "列 '!'" + "\r\n";
                    case TokenType.LPAREN:
                        return "LPAREN  " + row + "行" + col + "列 '('" + "\r\n";
                    case TokenType.RPAREN:
                        return "RPAREN  " + row + "行" + col + "列 ')'" + "\r\n";
                    case TokenType.LCB:
                        return "LCB  " + row + "行" + col + "列 '{'" + "\r\n";
                    case TokenType.RCB:
                        return "RCB  " + row + "行" + col + "列 '}'" + "\r\n";
                    case TokenType.LSB:
                        return "LSB  " + row + "行" + col + "列 '['" + "\r\n";
                    case TokenType.RSB:
                        return "RSB  " + row + "行" + col + "列 ']'" + "\r\n";
                    case TokenType.ASSIGN:
                        return "ASSIGN  " + row + "行" + col + "列 '='" + "\r\n";
                    case TokenType.DF:
                        return "DF  " + row + "行" + col + "列 '$'" + "\r\n";
                    case TokenType.PRINT:
                        return "PRINT  " + row + "行" + col + "列 'print'" + "\r\n";
                    case TokenType.CIA:
                        return "CIA  " + row + "行" + col + "列 'Cia'" + "\r\n";
                    case TokenType.IF:
                        return "IF  " + row + "行" + col + "列 'if'" + "\r\n";
                    case TokenType.ELSE:
                        return "ELSE  " + row + "行" + col + "列 'else'" + "\r\n";
                    case TokenType.WHILE:
                        return "WHILE  " + row + "行" + col + "列 'while'" + "\r\n";
                    case TokenType.BREAK:
                        return "BREAK  " + row + "行" + col + "列 'break'" + "\r\n";
                    case TokenType.CONTINUE:
                        return "CONTINUE  " + row + "行" + col + "列 'continue'" + "\r\n";
                    case TokenType.RETURN:
                        return "RETURN  " + row + "行" + col + "列 'return'" + "\r\n";
                    case TokenType.RETURNNULL:
                        return "RETURNNULL  " + row + "行" + col + "列 'returnNull'" + "\r\n";
                    case TokenType.READINT:
                        return "READINT  " + row + "行" + col + "列 'readInt'" + "\r\n";
                    case TokenType.READFLOAT:
                        return "READFLOAT  " + row + "行" + col + "列 'readFloat'" + "\r\n";
                    case TokenType.READSTRING:
                        return "READSTRING  " + row + "行" + col + "列 'readString'" + "\r\n";
                    case TokenType.READBOOL:
                        return "READBOOL  " + row + "行" + col + "列 'readBool'" + "\r\n";
                    case TokenType.EOF:
                        return "EOF  " + row + "行" + col + "列 'EOF'" + "\r\n";
                    default:
                        return "" + "undecleared error!";
                }
            }
        }

        

    }
}
