using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compilation
{
    public partial class InputForm: Form
    {

        private static InputForm inputForm;
        public string UserInput => InputTextBox.Text;

        public InputForm()
        {
            InitializeComponent();
            x = this.Width;
            y = this.Height;
            setTag(this);
            inputForm = this;
            this.StartPosition = FormStartPosition.CenterScreen;
        }

        public InputForm(string labelText) : this()
        {
            
            InputLabel.Text = labelText;
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

        private void InputForm_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / x;
            float newy = (this.Height) / y;
            setControls(newx, newy, this);
        }
    }
}
