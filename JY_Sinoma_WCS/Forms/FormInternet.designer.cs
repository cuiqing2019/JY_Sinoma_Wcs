namespace JY_Sinoma_WCS
{
    partial class FormInternet
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormInternet));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.checkSource = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbSub5 = new System.Windows.Forms.CheckBox();
            this.cbSub4 = new System.Windows.Forms.CheckBox();
            this.cbSub3 = new System.Windows.Forms.CheckBox();
            this.cbSub2 = new System.Windows.Forms.CheckBox();
            this.cbSub1 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.checkSource);
            this.groupBox1.Font = new System.Drawing.Font("宋体", 10.5F);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(429, 67);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "主站信息";
            // 
            // checkSource
            // 
            this.checkSource.AutoSize = true;
            this.checkSource.Enabled = false;
            this.checkSource.Location = new System.Drawing.Point(17, 31);
            this.checkSource.Name = "checkSource";
            this.checkSource.Size = new System.Drawing.Size(103, 18);
            this.checkSource.TabIndex = 0;
            this.checkSource.Text = "主站PLC故障";
            this.checkSource.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbSub5);
            this.groupBox2.Controls.Add(this.cbSub4);
            this.groupBox2.Controls.Add(this.cbSub3);
            this.groupBox2.Controls.Add(this.cbSub2);
            this.groupBox2.Controls.Add(this.cbSub1);
            this.groupBox2.Font = new System.Drawing.Font("宋体", 10.5F);
            this.groupBox2.Location = new System.Drawing.Point(12, 88);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(429, 122);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "从站掉站或模块错误";
            // 
            // cbSub5
            // 
            this.cbSub5.AutoSize = true;
            this.cbSub5.Enabled = false;
            this.cbSub5.Location = new System.Drawing.Point(17, 73);
            this.cbSub5.Name = "cbSub5";
            this.cbSub5.Size = new System.Drawing.Size(82, 18);
            this.cbSub5.TabIndex = 4;
            this.cbSub5.Text = "总线故障";
            this.cbSub5.UseVisualStyleBackColor = true;
            // 
            // cbSub4
            // 
            this.cbSub4.AutoSize = true;
            this.cbSub4.Enabled = false;
            this.cbSub4.Location = new System.Drawing.Point(333, 33);
            this.cbSub4.Name = "cbSub4";
            this.cbSub4.Size = new System.Drawing.Size(61, 18);
            this.cbSub4.TabIndex = 3;
            this.cbSub4.Text = "从站4";
            this.cbSub4.UseVisualStyleBackColor = true;
            // 
            // cbSub3
            // 
            this.cbSub3.AutoSize = true;
            this.cbSub3.Enabled = false;
            this.cbSub3.Location = new System.Drawing.Point(228, 33);
            this.cbSub3.Name = "cbSub3";
            this.cbSub3.Size = new System.Drawing.Size(61, 18);
            this.cbSub3.TabIndex = 2;
            this.cbSub3.Text = "从站3";
            this.cbSub3.UseVisualStyleBackColor = true;
            // 
            // cbSub2
            // 
            this.cbSub2.AutoSize = true;
            this.cbSub2.Enabled = false;
            this.cbSub2.Location = new System.Drawing.Point(115, 33);
            this.cbSub2.Name = "cbSub2";
            this.cbSub2.Size = new System.Drawing.Size(61, 18);
            this.cbSub2.TabIndex = 1;
            this.cbSub2.Text = "从站2";
            this.cbSub2.UseVisualStyleBackColor = true;
            // 
            // cbSub1
            // 
            this.cbSub1.AutoSize = true;
            this.cbSub1.Enabled = false;
            this.cbSub1.Location = new System.Drawing.Point(17, 33);
            this.cbSub1.Name = "cbSub1";
            this.cbSub1.Size = new System.Drawing.Size(61, 18);
            this.cbSub1.TabIndex = 0;
            this.cbSub1.Text = "从站1";
            this.cbSub1.UseVisualStyleBackColor = true;
            // 
            // FormInternet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(457, 222);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormInternet";
            this.Text = "PLC连接状态";
            this.Load += new System.EventHandler(this.FormStation_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox checkSource;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox cbSub4;
        private System.Windows.Forms.CheckBox cbSub3;
        private System.Windows.Forms.CheckBox cbSub2;
        private System.Windows.Forms.CheckBox cbSub1;
        private System.Windows.Forms.CheckBox cbSub5;
    }
}