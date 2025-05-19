namespace Compilation
{
    partial class MainMenu
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.run_btn = new System.Windows.Forms.Button();
            this.codeArea = new ScintillaNET.Scintilla();
            this.outArea = new ScintillaNET.Scintilla();
            this.tokenOutArea = new ScintillaNET.Scintilla();
            this.btn_save = new System.Windows.Forms.Button();
            this.btn_load = new System.Windows.Forms.Button();
            this.syntaxNodeOutArea = new ScintillaNET.Scintilla();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(696, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(124, 28);
            this.label1.TabIndex = 2;
            this.label1.Text = "输出结果";
            // 
            // run_btn
            // 
            this.run_btn.Location = new System.Drawing.Point(59, 12);
            this.run_btn.Name = "run_btn";
            this.run_btn.Size = new System.Drawing.Size(156, 62);
            this.run_btn.TabIndex = 3;
            this.run_btn.Text = "运行";
            this.run_btn.UseVisualStyleBackColor = true;
            this.run_btn.Click += new System.EventHandler(this.run_btn_Click);
            // 
            // codeArea
            // 
            this.codeArea.Location = new System.Drawing.Point(12, 101);
            this.codeArea.MultipleSelection = true;
            this.codeArea.Name = "codeArea";
            this.codeArea.Size = new System.Drawing.Size(321, 715);
            this.codeArea.TabIndex = 6;
            // 
            // outArea
            // 
            this.outArea.Location = new System.Drawing.Point(339, 101);
            this.outArea.Name = "outArea";
            this.outArea.Size = new System.Drawing.Size(358, 715);
            this.outArea.TabIndex = 7;
            // 
            // tokenOutArea
            // 
            this.tokenOutArea.Location = new System.Drawing.Point(703, 101);
            this.tokenOutArea.Name = "tokenOutArea";
            this.tokenOutArea.Size = new System.Drawing.Size(363, 715);
            this.tokenOutArea.TabIndex = 8;
            // 
            // btn_save
            // 
            this.btn_save.Location = new System.Drawing.Point(221, 12);
            this.btn_save.Name = "btn_save";
            this.btn_save.Size = new System.Drawing.Size(156, 62);
            this.btn_save.TabIndex = 9;
            this.btn_save.Text = "保存";
            this.btn_save.UseVisualStyleBackColor = true;
            this.btn_save.Click += new System.EventHandler(this.btn_save_Click);
            // 
            // btn_load
            // 
            this.btn_load.Location = new System.Drawing.Point(383, 12);
            this.btn_load.Name = "btn_load";
            this.btn_load.Size = new System.Drawing.Size(156, 62);
            this.btn_load.TabIndex = 10;
            this.btn_load.Text = "读取";
            this.btn_load.UseVisualStyleBackColor = true;
            this.btn_load.Click += new System.EventHandler(this.btn_load_Click);
            // 
            // syntaxNodeOutArea
            // 
            this.syntaxNodeOutArea.Location = new System.Drawing.Point(1091, 101);
            this.syntaxNodeOutArea.Name = "syntaxNodeOutArea";
            this.syntaxNodeOutArea.Size = new System.Drawing.Size(363, 715);
            this.syntaxNodeOutArea.TabIndex = 14;
            // 
            // MainMenu
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 18F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1475, 933);
            this.Controls.Add(this.syntaxNodeOutArea);
            this.Controls.Add(this.btn_load);
            this.Controls.Add(this.btn_save);
            this.Controls.Add(this.tokenOutArea);
            this.Controls.Add(this.outArea);
            this.Controls.Add(this.codeArea);
            this.Controls.Add(this.run_btn);
            this.Controls.Add(this.label1);
            this.Name = "MainMenu";
            this.Text = "编译器主界面";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Resize += new System.EventHandler(this.Form1_Resize);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button run_btn;
        private ScintillaNET.Scintilla codeArea;
        public ScintillaNET.Scintilla outArea;
        public ScintillaNET.Scintilla tokenOutArea;
        public System.Windows.Forms.Button btn_save;
        public System.Windows.Forms.Button btn_load;
        public ScintillaNET.Scintilla syntaxNodeOutArea;
    }
}

