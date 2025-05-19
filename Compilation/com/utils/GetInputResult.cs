using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Compilation.com.utils
{
    public class GetInputResult
    {
        private static MainMenu menu;

        public static string get(string str)
        {
            controllersInit(str);

            string returnStr = menu.InputTextBox.Text;

            controllersClose();

            return returnStr;
        }

        private static void controllersClose()
        {
            menu.InputLabel.Visible = false;
            menu.InputTextBox.Visible = false;
            menu.InputButton.Visible = false;
            menu.InputTextBox.Text = "";
        }

        private static void controllersInit(string str)
        {
            menu.InputLabel.Visible = true;
            menu.InputTextBox.Visible = true;
            menu.InputButton.Visible = true;
            menu.InputLabel.Text = str;
        }
    }
}
