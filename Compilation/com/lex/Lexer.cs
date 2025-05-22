using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compilation.lex
{
    public class Lexer
    {
        //词法分析结果语素列表
        public static List<Token> lexResult;
        //当前行号
        public static int currLineNo;
        //当前行字符索引
        public static int currCharIndex;
        //当前行内容
        public static string currLine;

        //判断字符是否为大小写字母、下划线、数字
        public static bool isIdentifier(char c)
        {
            if (c >= '0' && c <= '9')
            {
                return true;
            }
            else if (c >= 'A' && c <= 'Z')
            {
                return true;
            }
            else if (c >= 'a' && c <= 'z')
            {
                return true;
            }
            else if (c == '_')
            {
                return true;
            }
            return false;
        }

        //下一个字符
        public static char? watchNextChar()
        {
            if (currCharIndex < currLine.Length)
            {
                char c = currLine[currCharIndex];
                return c;
            }
            else
            {
                return null;
            }
        }
        public static void error(string msg)
        {
            StringBuilder errMsg = new StringBuilder();
            errMsg.AppendLine("错误代码：\n");
            errMsg.AppendLine(currLine);
            errMsg.AppendLine("\n");
            errMsg.AppendLine("错误位置：");
            errMsg.AppendLine($"行 {currLineNo + 1}");
            errMsg.AppendLine($"列\n {currCharIndex + 1}");
            errMsg.AppendLine("错误信息：");
            errMsg.AppendLine(msg);
            throw new MyException(errMsg.ToString());
        }

        //分析Token序列
        public static List<Token> analyzeTokenFromCode(string code)
        {
            lexResult = new List<Token>();

            String[] lines = code.Split('\n');
            for (int i = 0; i < lines.Length; i++)
            {
                currLineNo = i;
                currCharIndex = 0;
                currLine = lines[i].Replace("\r", "");
                analyzeTokenFromLine();
            }
            lexResult.Add(new Token(TokenType.EOF, -1, -1));
            return lexResult;
        }

        private static void analyzeTokenFromLine()
        {
            while (watchNextChar() != null)
            {
                var ch = watchNextChar();
                if (ch >= '0' && ch <= '9')
                {
                    extractNumberToken();
                }
                else if (ch == '+')
                {
                    currCharIndex++;
                    lexResult.Add(new Token(TokenType.PLUS, currLineNo, currCharIndex));
                }
                else if (ch == '-')
                {
                    currCharIndex++;
                    lexResult.Add(new Token(TokenType.MINUS, currLineNo, currCharIndex));
                }
                else if (ch == '*')
                {
                    currCharIndex++;
                    ch = watchNextChar();
                    if (ch == null)
                    {
                        lexResult.Add(new Token(TokenType.MUL, currLineNo, currCharIndex));
                    }
                    else if (ch == '*')
                    {
                        currCharIndex++;
                        lexResult.Add(new Token(TokenType.EXP, currLineNo, currCharIndex));
                    }
                    else
                    {
                        lexResult.Add(new Token(TokenType.MUL, currLineNo, currCharIndex));
                    }

                }
                else if (ch == '/')
                {
                    currCharIndex++;
                    ch = watchNextChar();
                    if (ch == '/')
                    {
                        while (ch != null)
                        {
                            currCharIndex++;
                            ch = watchNextChar();
                        }
                    }
                    else
                    {
                        lexResult.Add(new Token(TokenType.DIV, currLineNo, currCharIndex));
                    }
                }
                else if (ch == '%')
                {
                    currCharIndex++;
                    lexResult.Add(new Token(TokenType.MODULAR, currLineNo, currCharIndex));
                }
                else if (ch == '&')
                {
                    currCharIndex++;
                    lexResult.Add(new Token(TokenType.AND, currLineNo, currCharIndex));
                }
                else if (ch == '|')
                {
                    currCharIndex++;
                    lexResult.Add(new Token(TokenType.OR, currLineNo, currCharIndex));
                }
                else if (ch == '>')
                {
                    currCharIndex++;
                    ch = watchNextChar();
                    if (ch == '=')
                    {
                        currCharIndex++;
                        lexResult.Add(new Token(TokenType.COMP, currLineNo, currCharIndex, ">="));
                    }
                    else
                    {
                        lexResult.Add(new Token(TokenType.COMP, currLineNo, currCharIndex, ">"));
                    }
                }
                else if (ch == '<')
                {
                    currCharIndex++;
                    ch = watchNextChar();
                    if (ch == '=')
                    {
                        currCharIndex++;
                        lexResult.Add(new Token(TokenType.COMP, currLineNo, currCharIndex, "<="));
                    }
                    else
                    {
                        lexResult.Add(new Token(TokenType.COMP, currLineNo, currCharIndex, "<"));
                    }
                }
                else if (ch == '!')
                {
                    currCharIndex++;
                    ch = watchNextChar();
                    if (ch == '=')
                    {
                        lexResult.Add(new Token(TokenType.COMP, currLineNo, currCharIndex, "!="));
                    }
                    else
                    {
                        lexResult.Add(new Token(TokenType.NOT, currLineNo, currCharIndex));
                    }
                }
                else if (ch == '(')
                {
                    currCharIndex++;
                    lexResult.Add(new Token(TokenType.LPAREN, currLineNo, currCharIndex));
                }
                else if (ch == ')')
                {
                    currCharIndex++;
                    lexResult.Add(new Token(TokenType.RPAREN, currLineNo, currCharIndex));
                }
                else if (ch == '{')
                {
                    currCharIndex++;
                    lexResult.Add(new Token(TokenType.LCB, currLineNo, currCharIndex));
                }
                else if (ch == '}')
                {
                    currCharIndex++;
                    lexResult.Add(new Token(TokenType.RCB, currLineNo, currCharIndex));
                }
                else if (ch == '[')
                {
                    currCharIndex++;
                    lexResult.Add(new Token(TokenType.LSB, currLineNo, currCharIndex));
                }
                else if (ch == ']')
                {
                    currCharIndex++;
                    lexResult.Add(new Token(TokenType.RSB, currLineNo, currCharIndex));
                }
                else if (ch == '=')
                {
                    currCharIndex++;
                    ch = watchNextChar();
                    if (ch == '=')
                    {
                        currCharIndex++;
                        lexResult.Add(new Token(TokenType.COMP, currLineNo, currCharIndex, "=="));
                    }
                    else
                    {
                        lexResult.Add(new Token(TokenType.ASSIGN, currLineNo, currCharIndex));
                    }
                }
                else if (ch == '$')
                {
                    currCharIndex++;
                    lexResult.Add(new Token(TokenType.DF, currLineNo, currCharIndex));
                }
                else if ((ch >= 'a' && ch <= 'z') ||
                    (ch >= 'A' && ch <= 'Z'))
                {
                    StringBuilder buf = new StringBuilder();
                    buf.Append(ch);
                    currCharIndex++;
                    ch = watchNextChar();
                    while (ch != null && ch != ' ' && ch != '\t' && isIdentifier((char)ch))
                    {
                        buf.Append(ch);
                        currCharIndex++;
                        ch = watchNextChar();
                    }
                    String nr = buf.ToString();
                    if (nr.Equals("int"))
                    {
                        lexResult.Add(new Token(TokenType.KW, currLineNo, currCharIndex, nr));
                    }
                    else if (nr.Equals("float"))
                    {
                        lexResult.Add(new Token(TokenType.KW, currLineNo, currCharIndex, nr));
                    }
                    else if (nr.Equals("string"))
                    {
                        lexResult.Add(new Token(TokenType.KW, currLineNo, currCharIndex, nr));
                    }
                    else if (nr.Equals("bool"))
                    {
                        lexResult.Add(new Token(TokenType.KW, currLineNo, currCharIndex, nr));
                    }
                    else if (nr.Equals("true") || nr.Equals("false"))
                    {
                        lexResult.Add(new Token(TokenType.BOOLV, currLineNo, currCharIndex, nr));
                    }
                    //TODO: lexer SIN 等语句处
                    else if (nr.Equals("sin"))
                    {
                        lexResult.Add(new Token(TokenType.SIN, currLineNo, currCharIndex));
                    }else if (nr.Equals("cos"))
                    {
                        lexResult.Add(new Token(TokenType.COS, currLineNo, currCharIndex));
                    }else if (nr.Equals("tan"))
                    {
                        lexResult.Add(new Token(TokenType.TAN, currLineNo, currCharIndex));
                    }else if (nr.Equals("log"))
                    {
                        lexResult.Add(new Token(TokenType.LOG, currLineNo, currCharIndex));
                    }
                    else if (nr.Equals("print"))
                    {
                        lexResult.Add(new Token(TokenType.PRINT, currLineNo, currCharIndex));
                    }
                    else if (nr.Equals("Cia"))
                    {
                        lexResult.Add(new Token(TokenType.CIA, currLineNo, currCharIndex));
                    }
                    else if (nr.Equals("if"))
                    {
                        lexResult.Add(new Token(TokenType.IF, currLineNo, currCharIndex));
                    }
                    else if (nr.Equals("else"))
                    {
                        lexResult.Add(new Token(TokenType.ELSE, currLineNo, currCharIndex));
                    }
                    else if (nr.Equals("while"))
                    {
                        lexResult.Add(new Token(TokenType.WHILE, currLineNo, currCharIndex));
                    }
                    else if (nr.Equals("break"))
                    {
                        lexResult.Add(new Token(TokenType.BREAK, currLineNo, currCharIndex));
                    }
                    else if (nr.Equals("continue"))
                    {
                        lexResult.Add(new Token(TokenType.CONTINUE, currLineNo, currCharIndex));
                    }
                    else if (nr.Equals("return"))
                    {
                        lexResult.Add(new Token(TokenType.RETURN, currLineNo, currCharIndex));
                    }
                    else if (nr.Equals("returnNull"))
                    {
                        lexResult.Add(new Token(TokenType.RETURNNULL, currLineNo, currCharIndex));
                    }
                    else if (nr.Equals("readInt"))
                    {
                        lexResult.Add(new Token(TokenType.READINT, currLineNo, currCharIndex));
                    }
                    else if (nr.Equals("readFloat"))
                    {
                        lexResult.Add(new Token(TokenType.READFLOAT, currLineNo, currCharIndex));
                    }
                    else if (nr.Equals("readString"))
                    {
                        lexResult.Add(new Token(TokenType.READSTRING, currLineNo, currCharIndex));
                    }
                    else if (nr.Equals("readBool"))
                    {
                        lexResult.Add(new Token(TokenType.READBOOL, currLineNo, currCharIndex));
                    }
                    else
                    {
                        lexResult.Add(new Token(TokenType.ID, currLineNo, currCharIndex, nr));
                    }
                }
                else if (ch == '"')
                {
                    StringBuilder buf = new StringBuilder();
                    currCharIndex++;
                    ch = watchNextChar();
                    while (ch != null && ch != '"')
                    {
                        buf.Append(ch);
                        currCharIndex++;
                        ch = watchNextChar();
                    }
                    if (ch != null && ch == '"')
                    {
                        currCharIndex++;
                        String nr = buf.ToString();
                        lexResult.Add(new Token(TokenType.STRING, currLineNo, currCharIndex, nr));
                    }
                    else
                    {
                        error("缺少“\"”! ");
                    }
                }
                else if (ch == ' ' ||  ch == '\t')
                {
                    currCharIndex++;
                } 
                else
                {
                    error("不能识别!");
                }
            }
        }

        public static void extractNumberToken()
        {
            StringBuilder sb = new StringBuilder();
            int dotCount = 0;
            var ch = watchNextChar();
            while (ch != null && (ch >= '0' && ch <= '9' || ch == '.'))
            {
                if (ch == '.')
                {
                    if (dotCount > 0)
                    {
                        error("重复的小数点! ");
                    }
                    else
                    {
                        dotCount = 1;
                        sb.Append(ch);
                    }
                }
                else
                {
                    sb.Append(ch);
                }
                currCharIndex++;
                ch = watchNextChar();
            }

            ch = watchNextChar();
            if (ch != null)
            {
                if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z'))
                {
                    error("非法字符！");
                }
            }
            if (dotCount == 0)
            {
                lexResult.Add(new Token(TokenType.INT, currLineNo, currCharIndex - sb.ToString().Length + 1, sb.ToString()));
            }
            else
            {
                lexResult.Add(new Token(TokenType.FLOAT, currLineNo, currCharIndex - sb.ToString().Length + 1, sb.ToString()));
            }
        }
    }
}
