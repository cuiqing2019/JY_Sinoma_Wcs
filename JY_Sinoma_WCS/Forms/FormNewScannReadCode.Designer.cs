namespace JY_Sinoma_WCS
{
    partial class FormNewScannReadCode
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
            this.label1 = new System.Windows.Forms.Label();
            this.oneBoxCode = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBoxCode = new System.Windows.Forms.TextBox();
            this.AddScanRead = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 31);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "单托盘标签";
            // 
            // oneBoxCode
            // 
            this.oneBoxCode.Location = new System.Drawing.Point(83, 28);
            this.oneBoxCode.Name = "oneBoxCode";
            this.oneBoxCode.Size = new System.Drawing.Size(189, 21);
            this.oneBoxCode.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "托盘组标签";
            // 
            // groupBoxCode
            // 
            this.groupBoxCode.AcceptsReturn = true;
            this.groupBoxCode.Location = new System.Drawing.Point(83, 91);
            this.groupBoxCode.Multiline = true;
            this.groupBoxCode.Name = "groupBoxCode";
            this.groupBoxCode.Size = new System.Drawing.Size(189, 209);
            this.groupBoxCode.TabIndex = 3;
            this.groupBoxCode.WordWrap = false;
            // 
            // AddScanRead
            // 
            this.AddScanRead.Location = new System.Drawing.Point(12, 326);
            this.AddScanRead.Name = "AddScanRead";
            this.AddScanRead.Size = new System.Drawing.Size(260, 33);
            this.AddScanRead.TabIndex = 4;
            this.AddScanRead.Text = "添加标签";
            this.AddScanRead.UseVisualStyleBackColor = true;
            this.AddScanRead.Click += new System.EventHandler(this.AddScanRead_Click);
            // 
            // FormNewScannReadCode
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(284, 371);
            this.Controls.Add(this.AddScanRead);
            this.Controls.Add(this.groupBoxCode);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.oneBoxCode);
            this.Controls.Add(this.label1);
            this.Name = "FormNewScannReadCode";
            this.Text = "添加新托盘号";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox oneBoxCode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox groupBoxCode;
        private System.Windows.Forms.Button AddScanRead;
    }
}