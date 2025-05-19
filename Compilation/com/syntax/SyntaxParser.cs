using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using Compilation.bn.syntax.synnode;
using Compilation.lex;

namespace Compilation.bn.syntax
{
    public class SyntaxParser
    {
        public static List<Token> tokens;
        public static int tokenIndex;

        //初始化
        public void init(List<Token> tokens)
        {
            SyntaxParser.tokens = tokens;
            tokenIndex = -1;
        }

        //返回下一个Token
        private static Token currToken()
        {
            if (tokenIndex < tokens.Count)
            {
                return tokens[tokenIndex];
            } 
            else
            {
                return null;
            }
        }

        //查看指定Token
        private static Token seeSpecToken(int index)
        {
            if (index < tokens.Count)
            {
                return tokens[index];
            }
            else
            {
                return null;
            }
        }

        //语法分析
        public SyntaxNode parse()
        {
            tokenIndex++;
            SyntaxNode resultAST = parseProgram();

            if (currToken().type != TokenType.EOF)
            {
                throw new MyException("语法错误，程序异常中断！" + currToken().TokenString());
            }

            return resultAST;
        }

        //program -> (arrayDeclare)* fun(arrayDeclare | fun)*
        public static SyntaxNode parseProgram()
        {
            ProgramNode pn = new ProgramNode();

            while (currToken().type == TokenType.KW)
            {
                pn.addArray(parseArrayDeclare());
            }

            pn.addFun(parseFun());

            while (currToken().type != TokenType.EOF)
            {
                if (currToken().type == TokenType.KW)
                {
                    pn.addArray(parseArrayDeclare());
                }
                else
                {
                    pn.addFun(parseFun());
                }
            }
            return pn;
        }

        //arrayDeclare -> KW ID INT RSB
        public static SyntaxNode parseArrayDeclare()
        {
            Token kw = currToken();
            tokenIndex++;
            if (currToken().type != TokenType.ID)
            {
                throw new MyException("缺少数组名称: " + currToken().TokenString());
            }
            Token id = currToken();
            tokenIndex++;
            if (currToken().type != TokenType.LSB)
            {
                throw new MyException("缺少: '[' " + currToken().TokenString());
            }
            tokenIndex++;
            if (currToken().type != TokenType.INT)
            {
                throw new MyException("缺少整数: " + currToken().TokenString());
            }
            long length =long.Parse(currToken().value);
            tokenIndex++;
            if(currToken().type != TokenType.RSB)
            {
                throw new MyException("缺少: ']' " + currToken().TokenString());
            }
            tokenIndex++;
            
            return new ArrayDeclareNode(kw, id, length);
        }

        //fun -> FUN ID LPAREN (declare)* RPAREN block
        public static SyntaxNode parseFun()
        {
            if (currToken().type != TokenType.CIA)
            {
                throw new MyException("非法的函数声明: " + currToken().TokenString());
            }
            tokenIndex++;
            if (currToken().type != TokenType.ID)
            {
                if (currToken().type == TokenType.PRINT ||
                    currToken().type == TokenType.READINT || 
                    currToken().type == TokenType.READFLOAT ||
                    currToken().type == TokenType.READSTRING ||
                    currToken().type == TokenType.READBOOL)
                {
                    throw new MyException("函数名不能与内置函数重名: " + currToken().TokenString());
                }
                throw new MyException("需要函数名称: " + currToken().TokenString());
            }

            FunctionNode fn = new FunctionNode(currToken());
            tokenIndex++;
            Token curr = currToken();
            if (curr.type != TokenType.LPAREN)
            {
                throw new MyException("需要 '(' " + currToken().TokenString());
            }
            tokenIndex++;
            curr = currToken();
            while (curr.type == TokenType.KW)
            {
                SyntaxNode declarePara = parseDeclare();
                fn.addParam(declarePara);
                //TODO: 索引自增存疑 与原不符
                //tokenIndex++;
                curr = currToken();
            }
            curr = currToken();
            if (curr.type != TokenType.RPAREN)
            {
                throw new MyException("缺少 ')' " + currToken().TokenString());
            }
            tokenIndex++;
            SyntaxNode funBlock = parseBLOCK(fn);
            fn.setBody(funBlock);
            return fn;
        }

        //exprList -> expr(expr)*
        public static SyntaxNode parseExprList(SyntaxNode father)
        {
            ExprListNode eln = new ExprListNode(father);
            while (tokens[tokenIndex].type != TokenType.EOF)
            {
                SyntaxNode expr = parseExpr();
                if (expr != null)
                {
                    eln.add(expr);
                }
                else
                {
                    break;
                }
            }
            return eln;
        }

        //block -> LCB exprList RCB
        public static SyntaxNode parseBLOCK(SyntaxNode father)
        {
            Token curr = currToken();
            if (curr.type == TokenType.LCB)
            {
                tokenIndex++;
                SyntaxNode blockExprList = parseExprList(father);
                curr = currToken();
                if (curr.type != TokenType.RCB)
                {
                    throw new MyException("缺少 ')' 语句块应该结束: " + currToken().TokenString());
                }
                else
                {
                    tokenIndex++;
                    return blockExprList;
                }
            }
            else
            {
                throw new MyException("缺少 '(' : " + currToken().TokenString());
            }
            
        }

        /*expr -> WHILE arithORStrORBoolEpr block
        -> IF arithORStrORBoolExpr block (ELSE block)(1)
        -> PRINT LPAREN arithORStrORBoolExpr PPAREN
        -> simpleAssign
        -> ID LSB arithORStrORBoolExpr RSB ASSIGN arithORStrORBoolExpr 
        -> ID call
        -> KW (simpleAssign|ID) + DF
        -> BREAK 
        -> CONTINUE 
        -> BACK
        -> RETURN(arithORStrORBoolExpr)(1)*/
        public static SyntaxNode parseExpr()
        {
            Token curr = currToken();
            if (curr.type == TokenType.RETURNNULL)
            {
                tokenIndex++;
                return new ReturnNode(curr, null);
            }
            else if (curr.type == TokenType.RETURN)
            {
                tokenIndex++;
                SyntaxNode expr = parseArithORStrORBoolExpr();
                return new ReturnNode(curr, expr);
            }
            else if (curr.type == TokenType.CONTINUE)
            {
                tokenIndex++;
                return new ContinueNode(curr);
            }
            else if (curr.type == TokenType.BREAK)
            {
                tokenIndex++;
                return new BreakNode(curr);
            }
            else if (curr.type == TokenType.WHILE)
            {
                tokenIndex++;
                SyntaxNode logicExpr = parseArithORStrORBoolExpr();
                SyntaxNode whileExprList = parseBLOCK(null);
                return new WhileNode(logicExpr, whileExprList);
            }
            else if (curr.type == TokenType.IF)
            {
                Token ifToken = curr;
                tokenIndex++;
                SyntaxNode logicExpr = parseArithORStrORBoolExpr();
                SyntaxNode ifExprList = parseBLOCK(null);

                curr = currToken();
                if (curr.type == TokenType.ELSE)
                {
                    tokenIndex++;
                    SyntaxNode elseExprList = parseBLOCK(null);
                    return new IfElseNode(logicExpr, ifExprList, elseExprList, ifToken);
                }
                else
                {
                    return new IfElseNode(logicExpr, ifExprList, null, ifToken);
                }
            }
            else if (curr.type == TokenType.PRINT) 
            {
                tokenIndex++;
                curr = currToken();
                if (curr.type != TokenType.LPAREN)
                {
                    throw new MyException("缺少 '(' : " + currToken().TokenString());
                }
                tokenIndex++;
                SyntaxNode expr = parseArithORStrORBoolExpr();
                curr = currToken();
                if (curr.type != TokenType.RPAREN)
                {
                    throw new MyException("缺少 ')' : " + currToken().TokenString());
                }
                tokenIndex++;
                return new PrintNode(expr);
            }
            else if (curr.type == TokenType.ID)
            {
                Token var = curr;
                tokenIndex++;
                curr = currToken();
                if (curr.type != TokenType.ASSIGN && 
                    curr.type != TokenType.LPAREN && 
                    curr.type != TokenType.LSB)
                {
                    throw new MyException("缺少 '=' 或 '(' 或 '[' : " + currToken().TokenString());
                }
                else if (curr.type == TokenType.ASSIGN)
                {
                    return parseSimpleAssign(var);
                }
                else if (curr.type == TokenType.LSB)
                {
                    tokenIndex++;
                    SyntaxNode indexExpr = parseArithORStrORBoolExpr();
                    //TODO: 可能存在问题：curr取值
                    //curr = currToken();
                    if (curr.type == TokenType.RSB)
                    {
                        throw new MyException("缺少 ']' : " + currToken().TokenString());
                    }
                    tokenIndex++;
                    if (curr.type == TokenType.ASSIGN)
                    {
                        throw new MyException("缺少 '=' : " + currToken().TokenString());
                    }
                    tokenIndex++;
                    try
                    {
                        SyntaxNode valueExpr = parseArithORStrORBoolExpr();
                        return new ArrayAssignNode(var, indexExpr, valueExpr);
                    }
                    catch (Exception)
                    {
                        throw new MyException("语法错误parseArithORStrORBoolExpr()！ " + var.TokenString());
                    }
                }
                else if (curr.type == TokenType.LPAREN)
                {
                    return parseCall(var);
                }
                else
                {
                    throw new MyException("语法错误parseCall(var)！ " + currToken().TokenString());
                }
            }
            else if (curr.type == TokenType.KW)
            {
                tokenIndex++;
                Token kw = curr;
                DeclareMULNode dmn = new DeclareMULNode(kw);
                while (currToken().type == TokenType.ID)
                {
                    if (seeSpecToken(tokenIndex + 1).type == TokenType.ASSIGN)
                    {
                        Token var = currToken();
                        tokenIndex++;
                        SyntaxNode idEQexpr = parseSimpleAssign(var);
                        dmn.assignList.Add(idEQexpr);
                    }
                    else
                    {
                        Token var = currToken();
                        tokenIndex++;
                        //TODO: Variable待更改
                        dmn.assignList.Add(new VariableNode(var));
                    }
                    if (currToken().type == TokenType.DF)
                    {
                        tokenIndex++;
                        break;
                    }
                }
                return dmn;
            }
            else
            {
                return null;
            }

        }

        //call -> ID LPAREN (arithORStrORBoolExpr)* RPAREN
        public static SyntaxNode parseCall(Token var)
        {
            tokenIndex++;
            Token curr = currToken();
            CallNode cn = new CallNode(curr);
            while (curr.type != TokenType.RPAREN)
            {
                SyntaxNode exprParam = parseArithORStrORBoolExpr();
                cn.addParam(exprParam);
                //TODO: tokenIndex++; 是否需要
                curr = currToken();
            }
            if (curr.type == TokenType.RPAREN)
            {
                tokenIndex++;
                return cn;
            }
            else
            {
                throw new MyException("缺少 ')' : " + currToken().TokenString());
            }
        }

        //declare -> KW ID
        public static SyntaxNode parseDeclare()
        {
            Token curr = currToken();
            if (curr.type == TokenType.KW)
            {
                tokenIndex++;
                Token kw = currToken();
                if (kw.type != TokenType.ID)
                {
                    throw new MyException("缺少变量名称: " + curr.TokenString());
                }
                tokenIndex++;
                Token id = curr;
                return new DeclareNode(kw, id);
            }
            else
            {
                throw new MyException("语法错误KW！ " + curr.TokenString());
            }
        }

        //simpleAssign -> ID ASSIGN arithORStrORBoolExpr
        public static SyntaxNode parseSimpleAssign(Token var)
        {
            tokenIndex++;
            SyntaxNode expr = parseArithORStrORBoolExpr();
            return new AssignNode(new VariableNode(var), expr);
        }

        //arithORStrORBoolExpr -> termA((OR)termA)*
        public static SyntaxNode parseArithORStrORBoolExpr()
        {
            SyntaxNode termLeft = parseTermA();
            Token opt = currToken();
            while (opt.type == TokenType.OR)
            {
                tokenIndex++;
                SyntaxNode termRight = parseTermA();
                termLeft = new BinaryOPNode(termLeft, opt, termRight);
                opt = currToken();
            }
            return termLeft;
        }
        
        //termA -> termB((AND)termB)*
        public static SyntaxNode parseTermA()
        {
            SyntaxNode termLeft = parseTermB();
            Token opt = currToken();
            while (opt.type == TokenType.AND)
            {
                tokenIndex++;
                SyntaxNode termRight = parseTermB();
                termLeft = new BinaryOPNode(termLeft, opt, termRight);
                opt = currToken();
            }
            return termLeft;
        }

        //termB -> termC((COMP)termC)*
        public static SyntaxNode parseTermB()
        {
            SyntaxNode termLeft = parseTermC();
            Token opt = currToken();
            while (opt.type == TokenType.COMP)
            {
                tokenIndex++;
                SyntaxNode termRight = parseTermC();
                termLeft = new BinaryOPNode(termLeft, opt, termRight);
                opt = currToken();
            }
            return termLeft;
        }
        
        //termC -> termD((PLUS|MINUS)termD)*
        public static SyntaxNode parseTermC()
        {
            SyntaxNode termLeft = parseTermD();
            Token opt = currToken();
            while (opt.type == TokenType.PLUS || 
                opt.type == TokenType.MINUS)
            {
                tokenIndex++;
                SyntaxNode termRight = parseTermD();
                termLeft = new BinaryOPNode(termLeft, opt, termRight);
                opt = currToken();
            }
            return termLeft;
        }

        //termD -> termE((MUL | DIV |MODULAR)termE)*
        public static SyntaxNode parseTermD()
        {
            SyntaxNode facLeft = parseTermE();
            Token opt = currToken();
            while (opt.type == TokenType.MUL || 
                opt.type == TokenType.DIV ||
                opt.type == TokenType.MODULAR)
            {
                tokenIndex++;
                SyntaxNode facRight = parseTermE();
                facLeft = new BinaryOPNode(facLeft, opt, facRight);
                opt = currToken();
            }
            return facLeft;
        }

        //termE -> atom((EXP)atom)*
        public static SyntaxNode parseTermE()
        {
            SyntaxNode atomLeft = parseAtom();
            Token opt = currToken();
            while (opt.type == TokenType.EXP ||
                opt.type == TokenType.SIN || 
                opt.type == TokenType.COS || 
                opt.type == TokenType.TAN || 
                opt.type == TokenType.LOG)
            {
                
                if (opt.type == TokenType.EXP)
                {
                    tokenIndex++;
                    //右递归调用解析乘方的操作数
                    SyntaxNode facRight = parseTermE();
                    atomLeft = new BinaryOPNode(atomLeft, opt, facRight);
                    opt = currToken();
                }
                else
                {
                    tokenIndex++;
                    SyntaxNode facRight = parseAtom();
                    atomLeft = new UnaryOPNode(opt, facRight);
                    opt = currToken();
                }

            }
            return atomLeft;
        }

        /*atom -> INT | FLOAT | STRING | BOOLV
        -> (PLUS | MINUS | NOT)atom
        -> LPAREN arithORStrORBoolExpr RSB) | call)*
        -> READ# LPAREN RPAREN */
        public static SyntaxNode parseAtom()
        {
            Token curr = currToken();
            if (curr.type == TokenType.INT || 
                curr.type == TokenType.FLOAT)
            {
                tokenIndex++;
                return new NumberNode(curr);
            } 
            else if (curr.type == TokenType.STRING)
            {
                tokenIndex++;
                return new StringNode(curr);
            }
            else if (curr.type == TokenType.BOOLV)
            {
                tokenIndex++;
                return new BoolNode(curr);
            }
            else if (curr.type == TokenType.PLUS ||
                curr.type == TokenType.MINUS || 
                curr.type == TokenType.NOT)
            {
                tokenIndex++;
                return new UnaryOPNode(curr, parseAtom());
            }
            else if (curr.type == TokenType.LPAREN)
            {
                tokenIndex++;
                SyntaxNode expr = parseArithORStrORBoolExpr();
                Token newCurr = currToken();
                if (newCurr.type == TokenType.RPAREN)
                {
                    tokenIndex++;
                    return expr;
                }
                else
                {
                    throw new MyException("括号不匹配: " + curr.TokenString());
                }
            }
            else if (curr.type == TokenType.ID)
            {
                tokenIndex++;
                Token newCurr = currToken();
                if (newCurr.type == TokenType.LPAREN)
                {
                    return parseCall(curr);
                }
                else if (newCurr.type == TokenType.LSB)
                {
                    tokenIndex++;
                    SyntaxNode exprIndex = parseArithORStrORBoolExpr();
                    if (currToken().type != TokenType.RSB)
                    {
                        throw new MyException("缺少右方括号 ']' : " + currToken().TokenString());
                    }
                    tokenIndex++;
                    return new ArrayValueNode(curr, exprIndex);
                }
                else
                {
                    return new VariableNode(curr);
                }
            }
            else if (curr.type == TokenType.READINT)
            {
                tokenIndex++;
                if (currToken().type != TokenType.LPAREN)
                {
                    throw new MyException("缺少 '(' : " + curr.TokenString());
                }
                tokenIndex++;
                if (currToken().type != TokenType.RPAREN)
                {
                    throw new MyException("缺少 ')' : " + curr.TokenString());
                }
                tokenIndex++;
                return new ReadNode(ReadNodeType.INT);
            }
            else if (curr.type == TokenType.READFLOAT)
            {
                tokenIndex++;
                if (currToken().type != TokenType.LPAREN)
                {
                    throw new MyException("缺少 '(' : " + curr.TokenString());
                }
                tokenIndex++;
                if (currToken().type != TokenType.RPAREN)
                {
                    throw new MyException("缺少 ')' : " + curr.TokenString());
                }
                tokenIndex++;
                return new ReadNode(ReadNodeType.FLOAT);
            }
            else if (curr.type == TokenType.READSTRING)
            {
                tokenIndex++;
                if (currToken().type != TokenType.LPAREN)
                {
                    throw new MyException("缺少 '(' : " + curr.TokenString());
                }
                tokenIndex++;
                if (currToken().type != TokenType.RPAREN)
                {
                    throw new MyException("缺少 ')' : " + curr.TokenString());
                }
                tokenIndex++;
                return new ReadNode(ReadNodeType.STRING);
            }
            else if (curr.type == TokenType.READBOOL)
            {
                tokenIndex++;
                if (currToken().type != TokenType.LPAREN)
                {
                    throw new MyException("缺少 '(' : " + curr.TokenString());
                }
                tokenIndex++;
                if (currToken().type != TokenType.RPAREN)
                {
                    throw new MyException("缺少 ')' : " + curr.TokenString());
                }
                tokenIndex++;
                return new ReadNode(ReadNodeType.BOOL);
            }
            else if (curr.type == TokenType.SIN)
            {
                tokenIndex++;
                if (currToken().type != TokenType.LPAREN)
                {
                    throw new MyException("缺少 '(' : " + curr.TokenString());
                }
                tokenIndex++;
                SyntaxNode arg = parseArithORStrORBoolExpr();
                if (currToken().type != TokenType.RPAREN)
                {
                    throw new MyException("缺少 ')' : " + curr.TokenString());
                }
                tokenIndex++;
                return new TriAndLogNode(TriAndLogNodeType.SIN, curr, arg);
            }
            else if (curr.type == TokenType.COS)
            {
                tokenIndex++;
                if (currToken().type != TokenType.LPAREN)
                {
                    throw new MyException("缺少 '(' : " + curr.TokenString());
                }
                tokenIndex++;
                SyntaxNode arg = parseArithORStrORBoolExpr();
                if (currToken().type != TokenType.RPAREN)
                {
                    throw new MyException("缺少 ')' : " + curr.TokenString());
                }
                tokenIndex++;
                return new TriAndLogNode(TriAndLogNodeType.COS, curr, arg);
            }
            else if (curr.type == TokenType.TAN)
            {
                tokenIndex++;
                if (currToken().type != TokenType.LPAREN)
                {
                    throw new MyException("缺少 '(' : " + curr.TokenString());
                }
                tokenIndex++;
                SyntaxNode arg = parseArithORStrORBoolExpr();
                if (currToken().type != TokenType.RPAREN)
                {
                    throw new MyException("缺少 ')' : " + curr.TokenString());
                }
                tokenIndex++;
                return new TriAndLogNode(TriAndLogNodeType.TAN, curr, arg);
            }
            else if (curr.type == TokenType.LOG)
            {
                tokenIndex++;
                if (currToken().type != TokenType.LPAREN)
                {
                    throw new MyException("缺少 '(' : " + curr.TokenString());
                }
                tokenIndex++;
                SyntaxNode arg = parseArithORStrORBoolExpr();
                if (currToken().type != TokenType.RPAREN)
                {
                    throw new MyException("缺少 ')' : " + curr.TokenString());
                }
                tokenIndex++;
                return new TriAndLogNode(TriAndLogNodeType.LOG, curr, arg);
            }

            throw new MyException("语法错误! " + curr.TokenString());
        }


    }
}
