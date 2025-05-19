namespace Compilation
{
    partial class InputForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.InputButton = new System.Windows.Forms.Button();
            this.InputLabel = new System.Windows.Forms.Label();
            this.InputTextBox = new System.Windows.Forms.TextBox();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // InputButton
            // 
            this.InputButton.AutoSize = true;
            this.InputButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.InputButton.Location = new System.Drawing.Point(97, 131);
            this.InputButton.Name = "InputButton";
            this.InputButton.Size = new System.Drawing.Size(131, 47);
            this.InputButton.TabIndex = 0;
            this.InputButton.Text = "确认";
            this.InputButton.UseVisualStyleBackColor = true;
            // 
            // InputLabel
            // 
            this.InputLabel.AutoSize = true;
            this.InputLabel.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.InputLabel.Location = new System.Drawing.Point(41, 44);
            this.InputLabel.Name = "InputLabel";
            this.InputLabel.Size = new System.Drawing.Size(130, 24);
            this.InputLabel.TabIndex = 1;
            this.InputLabel.Text = "InputLabel";
            this.InputLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // InputTextBox
            // 
            this.InputTextBox.Location = new System.Drawing.Point(45, 86);
            this.InputTextBox.Name = "InputTextBox";
            this.InputTextBox.Size = new System.Drawing.Size(441, 28);
            this.InputTextBox.TabIndex = 2;
            // 
            // button1
            // 
            this.button1.AutoSize = true;
            this.button1.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.button1.Location = new System.Drawing.Point(291, 131);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(131, 47);
            this.button1.TabIndex = 3;
            this.button1.Text = "取消";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // InputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.ClientSize = new System.Drawing.Size(532, 190);
            this.ControlBox = false;
            this.Controls.Add(this.button1);
            this.Controls.Add(this.InputTextBox);
            this.Controls.Add(this.InputLabel);
            this.Controls.Add(this.InputButton);
            this.Name = "InputForm";
            this.Text = "InputForm";
            this.Resize += new System.EventHandler(this.InputForm_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.Button InputButton;
        public System.Windows.Forms.Label InputLabel;
        public System.Windows.Forms.TextBox InputTextBox;
        public System.Windows.Forms.Button button1;
    }
}