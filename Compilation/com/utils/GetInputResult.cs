using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Compilation.com.utils
{
    public class GetInputResult
    {
        public static string get(string str)
        {
            InputForm inputForm = new InputForm(str);
            var result = inputForm.ShowDialog(); // 显示弹出窗口

            if (result == DialogResult.OK)
            {
                string userInput = inputForm.UserInput; // 获取用户输入

                inputForm.Dispose();
                return userInput;
            }
            inputForm.Dispose();
            return null;
        }

        
    }
}
