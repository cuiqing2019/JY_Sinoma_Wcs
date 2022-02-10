namespace JY_Sinoma_WCS
{
    partial class FrmChangeGoodsKind
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
            this.cmbGoodsKindOld = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.btnExc = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.cmbGoodsKindNew = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbLocation = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // cmbGoodsKindOld
            // 
            this.cmbGoodsKindOld.Enabled = false;
            this.cmbGoodsKindOld.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbGoodsKindOld.FormattingEnabled = true;
            this.cmbGoodsKindOld.Items.AddRange(new object[] {
            "--请选择库位类型--",
            "吨桶",
            "圆桶",
            "空托盘组"});
            this.cmbGoodsKindOld.Location = new System.Drawing.Point(118, 77);
            this.cmbGoodsKindOld.Name = "cmbGoodsKindOld";
            this.cmbGoodsKindOld.Size = new System.Drawing.Size(142, 22);
            this.cmbGoodsKindOld.TabIndex = 51;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F);
            this.label3.Location = new System.Drawing.Point(32, 82);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(77, 14);
            this.label3.TabIndex = 50;
            this.label3.Text = "原库位类型";
            // 
            // btnExc
            // 
            this.btnExc.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExc.Location = new System.Drawing.Point(189, 166);
            this.btnExc.Name = "btnExc";
            this.btnExc.Size = new System.Drawing.Size(94, 38);
            this.btnExc.TabIndex = 49;
            this.btnExc.Text = "取消返回";
            this.btnExc.UseVisualStyleBackColor = true;
            this.btnExc.Click += new System.EventHandler(this.btnExc_Click);
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(49, 166);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(94, 38);
            this.btnOK.TabIndex = 48;
            this.btnOK.Text = "确认修改";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // cmbGoodsKindNew
            // 
            this.cmbGoodsKindNew.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbGoodsKindNew.FormattingEnabled = true;
            this.cmbGoodsKindNew.Items.AddRange(new object[] {
            "--请选择库位类型--",
            "吨桶",
            "圆桶",
            "空托盘组"});
            this.cmbGoodsKindNew.Location = new System.Drawing.Point(119, 120);
            this.cmbGoodsKindNew.Name = "cmbGoodsKindNew";
            this.cmbGoodsKindNew.Size = new System.Drawing.Size(142, 22);
            this.cmbGoodsKindNew.TabIndex = 47;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F);
            this.label1.Location = new System.Drawing.Point(32, 125);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 14);
            this.label1.TabIndex = 46;
            this.label1.Text = "新库位类型";
            // 
            // tbLocation
            // 
            this.tbLocation.Enabled = false;
            this.tbLocation.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbLocation.Location = new System.Drawing.Point(118, 30);
            this.tbLocation.Name = "tbLocation";
            this.tbLocation.Size = new System.Drawing.Size(142, 23);
            this.tbLocation.TabIndex = 45;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(46, 34);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 44;
            this.label2.Text = "库位编码";
            // 
            // FrmChangeGoodsKind
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(339, 234);
            this.Controls.Add(this.cmbGoodsKindOld);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.btnExc);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.cmbGoodsKindNew);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbLocation);
            this.Controls.Add(this.label2);
            this.Name = "FrmChangeGoodsKind";
            this.Text = "调整库位类型";
            this.Load += new System.EventHandler(this.FrmChangeGoodsKind_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbGoodsKindOld;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button btnExc;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.ComboBox cmbGoodsKindNew;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbLocation;
        private System.Windows.Forms.Label label2;
    }
}