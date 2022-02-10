namespace JY_Sinoma_WCS
{
    partial class FrmChangeHazardArea
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
            this.btnExc = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cmbAreaNew = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbLocation = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbAreaOld = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnExc
            // 
            this.btnExc.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExc.Location = new System.Drawing.Point(199, 173);
            this.btnExc.Name = "btnExc";
            this.btnExc.Size = new System.Drawing.Size(94, 38);
            this.btnExc.TabIndex = 41;
            this.btnExc.Text = "取消返回";
            this.btnExc.UseVisualStyleBackColor = true;
            this.btnExc.Click += new System.EventHandler(this.btnExc_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(59, 173);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(94, 38);
            this.btnOK.TabIndex = 40;
            this.btnOK.Text = "确认修改";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cmbAreaNew
            // 
            this.cmbAreaNew.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbAreaNew.FormattingEnabled = true;
            this.cmbAreaNew.Items.AddRange(new object[] {
            "--请选择危险分区--",
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H"});
            this.cmbAreaNew.Location = new System.Drawing.Point(129, 127);
            this.cmbAreaNew.Name = "cmbAreaNew";
            this.cmbAreaNew.Size = new System.Drawing.Size(142, 22);
            this.cmbAreaNew.TabIndex = 39;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F);
            this.label1.Location = new System.Drawing.Point(42, 132);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 14);
            this.label1.TabIndex = 38;
            this.label1.Text = "新危险分区";
            // 
            // tbLocation
            // 
            this.tbLocation.Enabled = false;
            this.tbLocation.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbLocation.Location = new System.Drawing.Point(128, 37);
            this.tbLocation.Name = "tbLocation";
            this.tbLocation.Size = new System.Drawing.Size(142, 23);
            this.tbLocation.TabIndex = 37;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(56, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 36;
            this.label2.Text = "库位编码";
            // 
            // cmbAreaOld
            // 
            this.cmbAreaOld.Enabled = false;
            this.cmbAreaOld.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbAreaOld.FormattingEnabled = true;
            this.cmbAreaOld.Items.AddRange(new object[] {
            "--请选择危险分区--",
            "A",
            "B",
            "C",
            "D",
            "E",
            "F",
            "G",
            "H"});
            this.cmbAreaOld.Location = new System.Drawing.Point(128, 84);
            this.cmbAreaOld.Name = "cmbAreaOld";
            this.cmbAreaOld.Size = new System.Drawing.Size(142, 22);
            this.cmbAreaOld.TabIndex = 43;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F);
            this.label3.Location = new System.Drawing.Point(42, 89);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 14);
            this.label3.TabIndex = 42;
            this.label3.Text = "原危险分区";
            // 
            // FrmChangeHazardArea
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(350, 234);
            this.Controls.Add(this.cmbAreaOld);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnExc);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cmbAreaNew);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbLocation);
            this.Controls.Add(this.label2);
            this.Name = "FrmChangeHazardArea";
            this.Text = "调整库位危险分区";
            this.Load += new System.EventHandler(this.FrmChangeHazardArea_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnExc;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cmbAreaNew;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLocation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbAreaOld;
        private System.Windows.Forms.Label label3;
    }
}