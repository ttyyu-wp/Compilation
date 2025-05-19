using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilation.lex
{
    public enum TokenType
    {
        ID,         //标识符
        INT,        //整数
        FLOAT,      //浮点数
        BOOLV,      //布尔值
        STRING,     //字符串
        PLUS,       //加号"+"
        MINUS,      //减号"-"
        MUL,        //乘号"+"
        EXP,        //乘方(**)
        DIV,        //除号"/"
        MODULAR,    //取模"%"
        SIN,        //SIN
        COS,        //COS
        TAN,        //TAN
        LOG,        //LOG
        AND,        //与运算"&"
        OR,         //或运算"|"
        COMP,       //比较运算
        NOT,        //非"!"
        LPAREN,     //左圆括号"("
        RPAREN,     //右圆括号")"
        LCB,        //左花括号"{"
        RCB,        //右花括号"}"
        LSB,        //左方括号“[”
        RSB,        //右方括号“]”
        KW,         //关键词
        DF,         //声明语句结束符‘$’
        IF,         //if语句的if
        ELSE,       //if语句的else
        WHILE,      //while语句的while
        BREAK,      //循环break
        RETURN,     //return
        RETURNNULL, //returnNull
        CONTINUE,   //循环Continue
        ASSIGN,     //赋值运算符
        PRINT,      //输出
        READINT,    //读入Int
        READFLOAT,  //读入Float
        READSTRING, //读入String
        READBOOL,   //读入Bool
        CIA,        //方法声明（Cia）
        EOF         //终止符
        

    }
}
