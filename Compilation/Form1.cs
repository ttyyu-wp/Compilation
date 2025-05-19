using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Compilation.bn.printToTest;
using Compilation.bn.syntax;
using Compilation.bn.syntax.synnode;
using Compilation.com.interpreter;
using Compilation.com.utils;
using Compilation.lex;
using ScintillaNET;
using static System.Windows.Forms.LinkLabel;

namespace Compilation
{
    public partial class MainMenu: Form
    {
        public static MainMenu form1;

        public MainMenu()
        {
            InitializeComponent();
            x = this.Width;
            y = this.Height;
            setTag(this);
            form1 = this;

            ScintillaInitialize();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        //Scintilla 初始化及其功能实现
        private void ScintillaInitialize()
        {
            // 设置边距（行号）
            codeArea.Margins[0].Width = 20;
            codeArea.Margins[0].Type = MarginType.Number;

            // 设置缩进
            codeArea.IndentationGuides = IndentView.LookBoth;
            codeArea.TabWidth = 4;
            codeArea.UseTabs = false; // 使用空格代替制表符

            // 设置自动完成
            
            autoPutinCode();
        }

        private string _keywords;
        private void autoPutinCode()
        {
            // 基本自动完成设置
            codeArea.AutoCAutoHide = false;       // 输入不匹配时不自动隐藏
            codeArea.AutoCDropRestOfWord = true;  // 补全时删除已输入部分后面的字符
            codeArea.AutoCIgnoreCase = true;      // 忽略大小写

            // 关键字列表
            _keywords = "int bool string float true false " +
                "break continue return returnNull " +
                "if else while Cia main " +
                "print() readInt() readFloat() readString() readBool() sin() cos() tan() log() ";

            // 注册自动完成事件
            codeArea.CharAdded += OnCharAddedForAutoComplete;
        }

        private void OnCharAddedForAutoComplete(object sender, CharAddedEventArgs e)
        {
            int currentPos = codeArea.CurrentPosition;
            int wordStartPos = codeArea.WordStartPosition(currentPos, true);
            char addedChar = (char)e.Char;


            // 基本触发条件：输入至少2个字符
            if (currentPos - wordStartPos >= 2)
            {
                string currentWord = codeArea.GetTextRange(wordStartPos, currentPos - wordStartPos);

                // 过滤匹配的关键字
                var matches = _keywords.Split(' ')
                                     .Where(k => k.StartsWith(currentWord, StringComparison.OrdinalIgnoreCase))
                                     .ToArray();

                if (matches.Length > 0)
                {
                    codeArea.AutoCShow(currentPos - wordStartPos, string.Join(" ", matches));
                }
            }
        }

        //窗体内控件调整大小 功能实现
        private float x;
        private float y;

        private void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                // true 表示自动缩放，false 则不缩放
                bool autoScale = true; 
                string autoScaleStr = autoScale ? "true" : "false";
                con.Tag = con.Width + ";" + con.Height + ";" + con.Left + ";" + con.Top + ";" + con.Font.Size + ";" + autoScaleStr;
                if (con.Controls.Count > 0)
                {
                    setTag(con);
                }
            }
        }

        private void setControls(float newx, float newy, Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                if (con.Tag != null)
                {
                    string[] mytag = con.Tag.ToString().Split(new char[] { ';' });
                    bool autoScale = mytag[5] == "true"; // 获取是否自动缩放的标志

                    if (autoScale)
                    {
                        con.Width = Convert.ToInt32(System.Convert.ToSingle(mytag[0]) * newx);
                        con.Height = Convert.ToInt32(System.Convert.ToSingle(mytag[1]) * newy);
                        con.Left = Convert.ToInt32(System.Convert.ToSingle(mytag[2]) * newx);
                        con.Top = Convert.ToInt32(System.Convert.ToSingle(mytag[3]) * newy);
                        Single currentSize = System.Convert.ToSingle(mytag[4]) * newy;
                        con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    }
                    // 如果控件包含子控件，则递归调用
                    if (con.Controls.Count > 0)
                    {
                        setControls(newx, newy, con);
                    }
                }
            }
        }

        private void Form1_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / x;
            float newy = (this.Height) / y;
            setControls(newx, newy, this);

        }

        //运行代码 按钮功能实现
        private void run_btn_Click(object sender, EventArgs e)
        {
            outArea.Text = null;
            tokenOutArea.Text = null;
            syntaxNodeOutArea.Text = null;

            List<Token> tokens = new List<Token>();

            String code = codeArea.Text;

            SyntaxParser parser = new SyntaxParser();
            Interpreter interpreter = new Interpreter();
            try
            {
                tokens = lex.Lexer.analyzeTokenFromCode(code);
                parser.init(tokens);
                SyntaxNode syntaxNodes = parser.parse();
                interpreter.runAST(syntaxNodes);

                PrintSyntaxNode.print(syntaxNodes);
                PrintToken.print(tokens);
            }
            catch (Exception exc)
            {
                outArea.Text = exc.ToString();
                Console.WriteLine(exc);
                return;
            }
        }

        //保存代码 按钮功能实现
        private void btn_save_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog saveFileDialog = new SaveFileDialog())
            {
                saveFileDialog.Filter = "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*";
                saveFileDialog.FilterIndex = 1;
                saveFileDialog.RestoreDirectory = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                {
                    SaveFile(codeArea, saveFileDialog.FileName);
                }
            }
        }

        public void SaveFile(Scintilla scintilla, string filePath)
        {
            try
            {
                // 获取编辑器中的全部文本
                string content = scintilla.Text;

                // 写入文件
                File.WriteAllText(filePath, content, Encoding.UTF8);

                MessageBox.Show("文件保存成功！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存文件时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //读取代码 按钮功能实现
        private void btn_load_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    LoadFile(codeArea, openFileDialog.FileName);
                }
            }
        }

        public void LoadFile(Scintilla scintilla, string filePath)
        {
            try
            {
                // 检查文件是否存在
                if (!File.Exists(filePath))
                {
                    MessageBox.Show("文件不存在！", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // 读取文件内容
                string content = File.ReadAllText(filePath, Encoding.UTF8);

                // 设置到编辑器中
                scintilla.Text = content;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"读取文件时出错: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
