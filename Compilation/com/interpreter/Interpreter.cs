using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Compilation.bn.syntax;
using Compilation.bn.syntax.synnode;
using Compilation.com.interpreter.exceptions;
using Compilation.com.interpreter.result;
using Compilation.com.interpreter.stack;
using Compilation.com.utils;
using Compilation.lex;

namespace Compilation.com.interpreter
{
    public class Interpreter
    {
        public static Dictionary<string, GlobalArray> globalArray;

        public static Dictionary<string, FunctionNode> fun;

        

        //执行AST
        public void runAST(SyntaxNode root)
        {
            globalArray = new Dictionary<string, GlobalArray>();
            fun = new Dictionary<string, FunctionNode>();
            RunStackUtil.init();

            ProgramNode pn = (ProgramNode)root;

            foreach (SyntaxNode sn in pn.arrayList)
            {
                //检查全局数组是否重复
                ArrayDeclareNode an = (ArrayDeclareNode)sn;
                GlobalArray ga = new GlobalArray(an.kw.value.Trim(), an.length);
                if (globalArray.ContainsKey(an.id.value.Trim()))
                {
                    throw new MyException("数组 " +  an.id.value.Trim() + " 已存在 " + an.id.TokenString());
                }
                globalArray.Add(an.id.value.Trim(), ga);
            }
            //检查main方法以及方法名称是否重复
            bool hasMainFun = false;
            HashSet<string> fname = new HashSet<string>();
            foreach (SyntaxNode sn in pn.funList)
            {
                FunctionNode fn = (FunctionNode)sn;
                if (fname.Contains(fn.funName.value.Trim()))
                {
                    throw new MyException("已存在此方法名称：" + fn.funName.TokenString());
                }
                if (fn.funName.value.Trim().Equals("main"))
                {
                    hasMainFun = true;
                    if (fn.paraList.Count != 0)
                    {
                        throw new MyException("main方法中不可含有参数！");
                    }
                }
                fname.Add(fn.funName.value.Trim());
                fun.Add(fn.funName.value.Trim(), fn);
            }

            if (!hasMainFun)
            {
                throw new MyException("不存在main方法！");
            }

            SingleRunStack currRs = new SingleRunStack(SingleRunStackType.CIA);
            RunStackUtil.push(currRs);
            try
            {
                traversalASTForRun(fun["main"], SingleRunStackType.CIA);
            }
            catch (ReturnException)
            {

                //throw new MyException(e.Message);
            }
        }

        //方法调用函数
        public static RunResult callFun(CallNode root)
        {
            CallNode cn = root;
            if (!fun.TryGetValue(cn.funName.value.Trim(), out FunctionNode v))
            {
                throw new MyException("方法 " + cn.funName.value.Trim() + " 未声明");
            }
            FunctionNode fn = fun[cn.funName.value.Trim()];
            if (fn.paraList.Count != cn.paraname.Count)
            {
                throw new MyException("参数数量不匹配 需要 " +  fn.paraList.Count + " 提供了 "
                    +  cn.paraname.Count + " 个 " + cn.toString());
            }
            //新方法的局部变量栈
            SingleRunStack currRSTemp = new SingleRunStack(SingleRunStackType.CIA);
            //传参
            for (int i = 0; i < fn.paraList.Count; i++)
            {
                //TODO: 调用函数变量不存在问题
                //形参
                DeclareNode dn = (DeclareNode)fn.paraList[i];
                //形参类型
                string typeStr = dn.id.value.Trim();
                //新变量
                Variable variable = new Variable(typeStr);
                //形参名称
                string nameStr = dn.kw.value.Trim();
                //值参
                RunResult rr = traversalArithORStrBoolExprForRun(cn.paraname[i]);
                //数值型参数
                if (typeStr.Equals("int") || typeStr.Equals("float"))
                {
                    if (rr is NumRunResult)
                    {
                        NumRunResult nrr = (NumRunResult)rr;
                        if (typeStr.Equals("int") && (!nrr.isInt))
                        {
                            throw new MyException("第" + (i + 1) + "个参数类型不匹配：" + cn.funName.TokenString());
                        }
                        variable.setValue((NumRunResult)rr, null, false);
                        currRSTemp.currVmap.Add(nameStr, variable);
                    }
                    else
                    {
                        throw new MyException("第" + (i + 1) + "个参数类型不匹配" + cn.funName.TokenString());
                    }
                }
                //字符串参数
                if (typeStr.Equals("string"))
                {
                    if (rr is StringRunResult)
                    {
                        StringRunResult srr = (StringRunResult)rr;
                        variable.setValue(null, srr.str, false);
                        currRSTemp.currVmap.Add(nameStr, variable);
                    }
                    else
                    {
                        throw new MyException("第" + (i + 1) + "个参数类型不匹配" + cn.funName.TokenString());
                    }
                }
                //布尔型参数
                if (typeStr.Equals("bool"))
                {
                    if (rr is BoolRunResult)
                    {
                        BoolRunResult brr = (BoolRunResult)rr;
                        variable.setValue(null, null, brr.b);
                        currRSTemp.currVmap.Add(nameStr, variable);
                    }
                    else
                    {
                        throw new MyException("第" + (i + 1) + "个参数类型不匹配" + cn.funName.TokenString());
                    }
                }
            }

            RunResult result = null;
            //调用新方法将原来的方法入栈
            RunStackUtil.push(currRSTemp);
            try
            {
                result = traversalASTForRun(fn, SingleRunStackType.CIA);
            }
            catch (ReturnException e)
            {
                //TODO: MyException e.result 可能存在问题
                result = e.result;
            }
            RunStackUtil.pop();
            return result;
        }

        //创建新变量
        public static void newVariable(string typeStr, string vName, SyntaxNode root)
        {
            Variable v = null;
            if (typeStr.Equals("int"))
            {
                v = new Variable(VariableTypes.INT);
            }
            else if (typeStr.Equals("float"))
            {
                v = new Variable(VariableTypes.FLOAT);
            }
            else if (typeStr.Equals("string"))
            {
                v = new Variable(VariableTypes.STRING);
            }
            else if (typeStr.Equals("bool"))
            {
                v = new Variable(VariableTypes.BOOL);
            } 
            else if (typeStr.Equals("void"))
            {
                throw new MyException("void仅可作为方法的声明 " + root.toString());
            }
            else
            {
                throw new MyException("创建新的变量出现问题 " + root.toString());
            }

            if (RunStackUtil.isVarExist(vName))
            {
                throw new MyException("变量 " +  vName + " 已经存在 " + root.toString());
            }

            RunStackUtil.addVar(vName, v);
        }

        //处理赋值节点
        public static void processAssignNode(AssignNode an)
        {
            if (an.left is DeclareNode)
            {
                traversalASTForRun(an.left, SingleRunStackType.OTHER);
            }
            else if (an.left is VariableNode)
            {
                VariableNode vn = (VariableNode) an.left;
                Variable variable = RunStackUtil.getVar(vn.t.value.Trim());
                if (variable == null)
                {
                    throw new MyException("变量未声明 " + vn.toString());
                }
            }
            else
            {
                throw new MyException("赋值运算符左侧必须是变量名 " +  an.toString());
            }

            if (an.right is BinaryOPNode || an.right is NumberNode || 
                an.right is UnaryOPNode || an.right is VariableNode || 
                an.right is StringNode || an.right is BoolNode || 
                an.right is CallNode || an.right is ReadNode || 
                an.right is ArrayValueNode)//TODO: || an.right is TriAndLogNode 
            {
                RunResult rr = traversalArithORStrBoolExprForRun(an.right);
                if (rr == null)
                {
                    throw new MyException("调用的方法没有返回值 " + an.toString());
                }
                Variable v = null;
                if (an.left is DeclareNode)
                {
                    DeclareNode dn = (DeclareNode) an.left;
                    v = RunStackUtil.getVar(dn.id.value.Trim());
                }
                else
                {
                    VariableNode vn = (VariableNode) an.left;
                    v = RunStackUtil.getVar(vn.t.value.Trim());
                }

                if (rr is NumRunResult)
                {
                    if (v.type == VariableTypes.STRING || v.type == VariableTypes.BOOL)
                    {
                        throw new MyException("不能将数值赋值给非数值变量 " + an.toString());
                    }
                    NumRunResult nrr = (NumRunResult) rr;
                    if (v.type == VariableTypes.INT && (!nrr.isInt))
                    {
                        throw new MyException("不能将浮点值赋值给整型变量 " + an.toString());
                    }
                    v.setValue(nrr, null, false);
                }
                else if (rr is StringRunResult)
                {
                    if (v.type != VariableTypes.STRING)
                    {
                        throw new MyException("不能将字符串赋值给非字符串型变量 " + an.toString());
                    }
                    v.setValue(null, ((StringRunResult)rr).str, false);
                }
                else if (rr is BoolRunResult)
                {
                    if (v.type != VariableTypes.BOOL)
                    {
                        throw new MyException("不能将布尔值赋值给非布尔值变量 " + an.toString());
                    }
                    BoolRunResult brr = (BoolRunResult) rr;
                    v.setValue(null, null, brr.b);
                } 
            } 
            else
            {
                throw new MyException("赋值运算符右侧必须是表达式 " + an.toString());
            }
        }

        //处理数组赋值节点
        public static void processArrayAssignNode(ArrayAssignNode aan)
        {
            string name = aan.id.value.Trim();
            if (!globalArray.ContainsKey(name))
            {
                throw new MyException("数组 " +  name + " 不存在 " + aan.toString());
            }
            GlobalArray ga = globalArray[name];
            RunResult rr = traversalArithORStrBoolExprForRun(aan.index);
            if (!(rr is NumRunResult))
            {
                throw new MyException("数组索引必须为整数 " + aan.index.toString());
            }
            NumRunResult nrr = (NumRunResult) rr;
            if (!nrr.isInt)
            {
                throw new MyException("数组索引必须为整数 " + aan.index.toString());
            }
            rr = traversalArithORStrBoolExprForRun(aan.value);

            if (ga.type == VariableTypes.INT && (rr is NumRunResult) && (((NumRunResult)rr).isInt))
            {
                try
                {
                    ga.setValue(nrr.intV, (NumRunResult)rr, null, false);
                }
                catch (ArrayIndexOutOfBoundsException)
                {

                    throw new MyException("数组下标越界 " + aan.index.toString());
                }
            }
            else if (ga.type == VariableTypes.FLOAT && (rr is NumRunResult))
            {
                try
                {
                    ga.setValue(nrr.intV, (NumRunResult)rr, null, false);
                }
                catch (ArrayIndexOutOfBoundsException)
                {

                    throw new MyException("数组下标越界 " + aan.index.toString());
                }
            }
            else if (ga.type == VariableTypes.STRING && (rr is StringRunResult))
            {
                try
                {
                    ga.setValue(nrr.intV, null, ((StringRunResult)rr).str, false);
                }
                catch (ArrayIndexOutOfBoundsException)
                {

                    throw new MyException("数组下标越界 " + aan.index.toString());
                }
            }
            else if (ga.type == VariableTypes.BOOL && (rr is BoolRunResult))
            {
                try
                {
                    ga.setValue(nrr.intV, null, null, ((BoolRunResult)rr).b);
                }
                catch (ArrayIndexOutOfBoundsException)
                {

                    throw new MyException("数组下标越界 " + aan.index.toString());
                }
            }
            else
            {
                throw new MyException("赋值类型不匹配！" + aan.toString());
            }
        }

        //遍历AST并执行
        public static RunResult traversalASTForRun(SyntaxNode root, SingleRunStackType srstUpper)
        {
            if (root is CallNode)
            {
                return callFun((CallNode)root);
            }
            else if (root is FunctionNode)
            {
                FunctionNode fn = (FunctionNode)root;
                return traversalASTForRun(fn.funBody, SingleRunStackType.CIA);
            }
            else if (root is AssignNode)
            {
                AssignNode an = (AssignNode)root;
                processAssignNode(an);
            }
            else if (root is ArrayAssignNode)
            {
                ArrayAssignNode aan = (ArrayAssignNode)root;
                processArrayAssignNode(aan);
            }
            else if (root is DeclareNode)
            {
                DeclareNode dn = (DeclareNode)root;
                newVariable(dn.kw.value.Trim(), dn.id.value.Trim(), root);
            }
            else if (root is DeclareMULNode)
            {
                DeclareMULNode dmn = (DeclareMULNode)root;
                string typeStr = dmn.kw.value.Trim();
                foreach (SyntaxNode sn in dmn.assignList)
                {
                    VariableNode v = null;
                    if (sn is AssignNode)
                    {
                        AssignNode an = (AssignNode)sn;
                        v = (VariableNode)an.left;
                        newVariable(typeStr, v.t.value.Trim(), root);
                        processAssignNode(an);
                    }
                    else
                    {
                        v = (VariableNode)sn;
                        newVariable(typeStr, v.t.value.Trim(), root);
                    }
                }
            }
            else if (root is PrintNode)
            {
                PrintNode pn = (PrintNode)root;
                if (!(pn.exprForPrint is BinaryOPNode || 
                    pn.exprForPrint is NumberNode || 
                    pn.exprForPrint is UnaryOPNode || 
                    pn.exprForPrint is VariableNode || 
                    pn.exprForPrint is StringNode || 
                    pn.exprForPrint is BoolNode || 
                    pn.exprForPrint is CallNode ||
                    pn.exprForPrint is ArrayValueNode ||
                    pn.exprForPrint is ReadNode  //TODO: pn.exprForPrint is TriAndLogNode
                    /*||pn.exprForPrint is TriAndLogNode*/))
                {
                    throw new MyException("只能打印表达式！" + root.toString());
                }

                RunResult rr = traversalArithORStrBoolExprForRun(pn.exprForPrint);
                
                if (rr is StringRunResult)
                {
                    MainMenu.form1.outArea.AppendText(rr.toString().Replace("\\n", "\r\n"));
                } 
                else
                {
                    MainMenu.form1.outArea.AppendText(rr.toString());
                }
                
            }
            else if (root is ExprListNode)
            {
                ExprListNode eln = (ExprListNode)root;
                if (eln.father == null)
                {
                    SingleRunStack srs = new SingleRunStack(srstUpper);
                    RunStackUtil.push(srs);
                }

                

                foreach (SyntaxNode sn in eln.list)
                {
                    traversalASTForRun(sn, SingleRunStackType.OTHER);
                }
                if (eln.father == null)
                {
                    RunStackUtil.pop();
                }
            }
            else if (root is IfElseNode)
            {
                IfElseNode ien = (IfElseNode)root;
                RunResult rr = traversalArithORStrBoolExprForRun(ien.logicExpr);
                if (!(rr is BoolRunResult))
                {
                    throw new MyException("需要逻辑表达式 " + root.toString());
                }
                if (((BoolRunResult)rr).b)
                {
                    traversalASTForRun(ien.ifExpr, SingleRunStackType.OTHER);
                }
                else if (ien.elseExpr != null)
                {
                    traversalASTForRun(ien.elseExpr, SingleRunStackType.OTHER);
                }
            }
            else if (root is WhileNode)
            {
                WhileNode wn = (WhileNode)root;
                while (true)
                {
                    RunResult rr = traversalArithORStrBoolExprForRun(wn.logicExpr);
                    if (!(rr is BoolRunResult))
                    {
                        throw new MyException("需要逻辑表达式 " + root);
                    }
                    BoolRunResult brr = (BoolRunResult)rr;  
                    if (brr.b)
                    {
                        try
                        {
                            traversalASTForRun(wn.whileExpr, SingleRunStackType.WHILE);
                        }
                        //TODO: BreakException 与 ContinueException 待检查
                        catch (BreakException)
                        {
                            break;
                        }
                        catch (ContinueException)
                        {
                            continue;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
            }
            else if (root is BreakNode)
            {
                while (!(RunStackUtil.topP().type == SingleRunStackType.WHILE))
                {
                    RunStackUtil.pop();
                }
                RunStackUtil.pop();
                throw new BreakException(root);
            }
            else if (root is ContinueNode)
            {
                while (!(RunStackUtil.topP().type == SingleRunStackType.WHILE))
                {
                    RunStackUtil.pop();
                }
                RunStackUtil.pop();
                throw new ContinueException(root);
            }
            else if (root is ReturnNode)
            {
                ReturnNode rn = (ReturnNode)root;
                if (rn.t.type == TokenType.RETURNNULL)
                {
                    while (!(RunStackUtil.topP().type == SingleRunStackType.CIA))
                    {
                        RunStackUtil.pop();
                    }
                    throw new ReturnException(null);
                }
                RunResult rr = traversalArithORStrBoolExprForRun(rn.expr);
                while (!(RunStackUtil.topP().type == SingleRunStackType.CIA))
                {
                    RunStackUtil.pop();
                }
                throw new ReturnException(rr);
            }
            //TODO: 待添加TriAndLogNode分支
            else
            {
                throw new MyException("语法错误！ " +  root.toString());
            }
            return null;
        }

        //遍历AST并执行表达式计算
        public static RunResult traversalArithORStrBoolExprForRun(SyntaxNode root)
        {
            if (root is CallNode)
            {
                return callFun((CallNode)root);
            }
            else if (root is NumberNode)
            {
                NumberNode nn = (NumberNode)root;
                if (nn.t.type == TokenType.INT)
                {
                    return new NumRunResult(true, long.Parse(nn.t.value), 0);
                }
                else
                {
                    return new NumRunResult(false, 0, float.Parse(nn.t.value));
                }
            }
            else if (root is StringNode)
            {
                return new StringRunResult(((StringNode)root).t.value);
            }
            else if (root is BoolNode)
            {
                bool b = (((BoolNode)root).t.value.Trim()).Equals("true");
                return new BoolRunResult(b);
            }
            else if (root is UnaryOPNode) 
            {
                UnaryOPNode uon = (UnaryOPNode)root;
                RunResult rr = traversalArithORStrBoolExprForRun(((UnaryOPNode)root).right);
                if (rr is BoolRunResult)
                {
                    BoolRunResult brr = (BoolRunResult)rr;
                    if (uon.t.type == TokenType.NOT)
                    {
                        brr.not();
                        return brr;
                    }
                    else
                    {
                        throw new MyException("类型与运算不匹配！ " + ((UnaryOPNode)root).t.TokenString());
                    }
                }
                if (!(rr is NumRunResult))
                {
                    throw new MyException("类型与运算不匹配！ " + ((UnaryOPNode)root).t.TokenString());
                }
                NumRunResult v = (NumRunResult)rr;

                if (uon.t.type == TokenType.PLUS)
                {
                    return v;
                }
                else
                {
                    v.setNegative();
                    return v;
                }
            }
            else if (root is BinaryOPNode)
            {
                RunResult rrl = traversalArithORStrBoolExprForRun(((BinaryOPNode)root).left);
                RunResult rrr = traversalArithORStrBoolExprForRun(((BinaryOPNode)root).right);
                BinaryOPNode bon = (BinaryOPNode)root;

                if ((rrl is BoolRunResult) && (!(rrr is BoolRunResult)) && (bon.t.type != TokenType.PLUS))
                {
                    throw new MyException("布尔值只能与布尔值进行运算! " + ((BinaryOPNode)root).t.TokenString());
                }
                else if ((rrr is BoolRunResult) && (!(rrl is BoolRunResult)) && (bon.t.type != TokenType.PLUS))
                {
                    throw new MyException("布尔值只能与布尔值进行运算! " + ((BinaryOPNode)root).t.TokenString());
                }
                else if ((rrl is BoolRunResult) && (rrr is BoolRunResult))
                {
                    BoolRunResult bl = (BoolRunResult)rrl;
                    BoolRunResult br = (BoolRunResult)rrr;
                    if (bon.t.type == TokenType.AND)
                    {
                        return new BoolRunResult(bl.b & br.b);
                    }
                    else if (bon.t.type == TokenType.OR)
                    {
                        return new BoolRunResult(bl.b | br.b);
                    }
                    else
                    {
                        throw new MyException("布尔值只能进行逻辑运算 " + ((BinaryOPNode)root).t.TokenString());
                    }
                }
                else if (bon.t.type == TokenType.COMP)
                {
                    if ((rrl is StringRunResult) && (!(rrr is StringRunResult)))
                    {
                        throw new MyException("字符串只能与字符串进行比较 " + ((BinaryOPNode)root).t.TokenString());
                    }
                    if (!(rrl is StringRunResult) && ((rrr is StringRunResult)))
                    {
                        throw new MyException("字符串只能与字符串进行比较 " + ((BinaryOPNode)root).t.TokenString());
                    }
                    if ((rrl is NumRunResult) && (!(rrr is NumRunResult)))
                    {
                        throw new MyException("数值只能与数值进行比较 " + ((BinaryOPNode)root).t.TokenString());
                    }

                    if (rrl is StringRunResult)
                    {
                        StringRunResult sl = (StringRunResult)rrl;
                        StringRunResult sr = (StringRunResult)rrr;
                        int compS = sl.str.CompareTo(sr.str);
                        if (bon.t.value.Trim().Equals(">"))
                        {
                            return new BoolRunResult(compS > 0);
                        }
                        else if (bon.t.value.Trim().Equals("<"))
                        {
                            return new BoolRunResult(compS < 0);
                        }
                        else if (bon.t.value.Trim().Equals(">="))
                        {
                            return new BoolRunResult(compS >= 0);
                        }
                        else if (bon.t.value.Trim().Equals("<="))
                        {
                            return new BoolRunResult(compS <= 0);
                        }
                        else if (bon.t.value.Trim().Equals("=="))
                        {
                            return new BoolRunResult(compS == 0);
                        }
                        else if (bon.t.value.Trim().Equals("!="))
                        {
                            return new BoolRunResult(compS != 0);
                        }
                    }
                    else
                    {
                        NumRunResult nl = (NumRunResult)rrl;
                        NumRunResult nr = (NumRunResult)rrr;
                        return nl.comp(nr, bon.t.value.Trim());
                    }
                }
                else if (bon.t.type == TokenType.PLUS)
                {
                    if ((rrl is StringRunResult) || (rrr is StringRunResult) || 
                        ((rrl is NumRunResult) && (rrr is NumRunResult)))
                    {
                        
                        return rrl.add(rrr);
                    }
                    else
                    {
                        throw new MyException("加法运算不合语法：" + ((BinaryOPNode)root).t.TokenString());
                    }
                }
                else
                {
                    if ((rrl is StringRunResult) || (rrr is StringRunResult))
                    {
                        //字符串乘方
                        if (bon.t.type == TokenType.MUL)
                        {
                            if ((rrl is StringRunResult) && (rrr is NumRunResult))
                            {
                                StringRunResult srr = (StringRunResult)rrl;
                                NumRunResult nrr = (NumRunResult)rrr;
                                if (!nrr.isInt)
                                {
                                    throw new MyException("字符串乘方的幂只能为整数");
                                }
                                return srr.exp(nrr);
                            }
                        }
                        throw new MyException("字符串不能进行乘法外的运算：" + ((BinaryOPNode)root).t.TokenString());
                    }

                    NumRunResult vl = (NumRunResult)rrl;
                    NumRunResult vr = (NumRunResult)rrr;

                    if (bon.t.type == TokenType.MINUS)
                    {
                        return vl.sub(vr);
                    }
                    else if (bon.t.type == TokenType.MUL)
                    {
                        return vl.mul(vr);
                    }
                    else if (bon.t.type == TokenType.DIV || bon.t.type == TokenType.MODULAR)
                    {
                        if (vr.isInt)
                        {
                            if (vr.intV == 0)
                            {
                                throw new MyException("不能除以0 " + ((BinaryOPNode)root).t.TokenString());
                            }
                        }
                        else
                        {
                            if (vr.floatV == 0)
                            {
                                throw new MyException("不能除以0 " + ((BinaryOPNode)root).t.TokenString());
                            }
                        }
                        if (bon.t.type == TokenType.DIV)
                        {
                            return vl.div(vr);
                        }
                        else
                        {
                            //整数及浮点数取模
                            if (vl.isInt && vr.isInt)
                            {
                                return vl.modInt(vr);
                            }
                            else
                            {
                                return vl.modFloat(vr);
                            }
                        }
                    }
                    else if (bon.t.type == TokenType.EXP)
                    {
                        return vl.exp(vr);
                    }
                }
            }
            else if (root is VariableNode)
            {
                VariableNode vn = (VariableNode)root;
                Variable v = RunStackUtil.getVar(vn.t.value.Trim());

                if (v == null)
                {
                    throw new MyException("变量不存在！ " + root.toString());
                }
                else if (!v.initFlag)
                {
                    throw new MyException("变量未初始化");
                }
                else
                {
                    return v.getValue();
                }
            }
            else if (root is ArrayValueNode)
            {
                ArrayValueNode avn = (ArrayValueNode)root;
                RunResult indexRr = traversalArithORStrBoolExprForRun(avn.index);
                if (!(indexRr is NumRunResult))
                {
                    throw new MyException("数组的索引必须为整数 " + avn.toString());
                }
                if (!((NumRunResult)indexRr).isInt)
                {
                    throw new MyException("数组的索引必须为整数 " + avn.toString());
                }
                int index = (int)((NumRunResult)indexRr).intV;
                GlobalArray ga = globalArray[avn.id.value.Trim()];
                if (ga.type == VariableTypes.INT)
                {
                    if (index >= ga.intV.Length)
                    {
                        throw new MyException("数组下标越界 " + avn.toString());
                    }
                    return new NumRunResult(true, ga.intV[index], 0);
                }
                else if (ga.type == VariableTypes.FLOAT)
                {
                    if (index >= ga.floatV.Length)
                    {
                        throw new MyException("数组下标越界 " + avn.toString());
                    }
                    return new NumRunResult(false, 0, ga.floatV[index]);
                }
                else if (ga.type == VariableTypes.STRING)
                {
                    if (index >= ga.stringV.Length)
                    {
                        throw new MyException("数组下标越界 " + avn.toString());
                    }
                    return new StringRunResult(ga.stringV[index]);
                }
                else if (ga.type == VariableTypes.BOOL)
                {
                    if (index >= ga.boolV.Length)
                    {
                        throw new MyException("数组下标越界 " + avn.toString());
                    }
                    return new BoolRunResult(ga.boolV[index]);
                }
            }
            else if (root is ReadNode)
            {
                ReadNode rn = (ReadNode) root;
                if (rn.rnt == ReadNodeType.INT)
                {
                    long intV = 0;
                    while (true)
                    {
                        string str = GetInputResult.get("请输入一个整数");

                        try
                        {
                            intV = long.Parse(str);
                            break;
                        }
                        catch (Exception)
                        {

                            MessageBox.Show("输入的数据不合规，重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    return new NumRunResult(true, intV, 0);
                }
                else if (rn.rnt == ReadNodeType.FLOAT)
                {
                    double doubleV = 0;
                    while (true)
                    {
                        string str = GetInputResult.get("请输入一个浮点数");
                        try
                        {
                            doubleV = double.Parse(str);
                            break;
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("输入的数据不合规，重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    return new NumRunResult(false, 0, doubleV);
                }
                else if (rn.rnt == ReadNodeType.STRING)
                {
                    string strV = "";
                    while (true)
                    {
                        strV = GetInputResult.get("请输入一个字符串");
                        break;
                    }
                    return new StringRunResult(strV);
                }
                else if (rn.rnt == ReadNodeType.BOOL)
                {
                    bool boolV = false;
                    while (true)
                    {
                        string str = GetInputResult.get("请输入一个布尔值true或false");
                        if (str.Equals("true") || str.Equals("false"))
                        {
                            if (str.Equals("true"))
                            {
                                boolV = true;
                            }
                            else
                            {
                                boolV = false;
                            }
                            break;
                        }
                        else
                        {
                            MessageBox.Show("输入的数据不合规，重新输入！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                    }
                    return new BoolRunResult(boolV);
                }
            }
            //TODO: TriAndLogNode待写
            else
            {
                throw new MyException("语法错误 " + root.toString());
            }
            return new NumRunResult(false, 0, 0);
        }


    }
}
