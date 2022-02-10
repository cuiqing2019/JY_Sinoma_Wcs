namespace JY_Sinoma_WCS
{
    partial class FrmChangeLocation
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
            this.label2 = new System.Windows.Forms.Label();
            this.tbContainerNo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tbStartLocation = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbEndLocation = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.btnExc = new System.Windows.Forms.Button();
            this.label4 = new System.Windows.Forms.Label();
            this.tbBatch = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtGoodsCode = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.txtLoacteType = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.Location = new System.Drawing.Point(28, 35);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(63, 14);
            this.label2.TabIndex = 31;
            this.label2.Text = "周转箱号";
            // 
            // tbContainerNo
            // 
            this.tbContainerNo.Enabled = false;
            this.tbContainerNo.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbContainerNo.Location = new System.Drawing.Point(100, 31);
            this.tbContainerNo.Name = "tbContainerNo";
            this.tbContainerNo.Size = new System.Drawing.Size(142, 23);
            this.tbContainerNo.TabIndex = 32;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(28, 122);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 14);
            this.label1.TabIndex = 33;
            this.label1.Text = "原始库位";
            // 
            // tbStartLocation
            // 
            this.tbStartLocation.Enabled = false;
            this.tbStartLocation.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbStartLocation.Location = new System.Drawing.Point(100, 118);
            this.tbStartLocation.Name = "tbStartLocation";
            this.tbStartLocation.Size = new System.Drawing.Size(142, 23);
            this.tbStartLocation.TabIndex = 34;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.Location = new System.Drawing.Point(336, 122);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 14);
            this.label3.TabIndex = 35;
            this.label3.Text = "调整库位";
            // 
            // tbEndLocation
            // 
            this.tbEndLocation.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbEndLocation.Location = new System.Drawing.Point(408, 118);
            this.tbEndLocation.Name = "tbEndLocation";
            this.tbEndLocation.Size = new System.Drawing.Size(142, 23);
            this.tbEndLocation.TabIndex = 36;
            // 
            // btnOK
            // 
            this.btnOK.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.Location = new System.Drawing.Point(144, 181);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(98, 36);
            this.btnOK.TabIndex = 37;
            this.btnOK.Text = "确认修改";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnExc
            // 
            this.btnExc.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnExc.Location = new System.Drawing.Point(339, 181);
            this.btnExc.Name = "btnExc";
            this.btnExc.Size = new System.Drawing.Size(103, 36);
            this.btnExc.TabIndex = 38;
            this.btnExc.Text = "取消返回";
            this.btnExc.UseVisualStyleBackColor = true;
            this.btnExc.Click += new System.EventHandler(this.btnExc_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.Location = new System.Drawing.Point(336, 31);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 14);
            this.label4.TabIndex = 39;
            this.label4.Text = "派车单号";
            // 
            // tbBatch
            // 
            this.tbBatch.Enabled = false;
            this.tbBatch.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.tbBatch.Location = new System.Drawing.Point(408, 27);
            this.tbBatch.Name = "tbBatch";
            this.tbBatch.Size = new System.Drawing.Size(142, 23);
            this.tbBatch.TabIndex = 40;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.Location = new System.Drawing.Point(336, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 14);
            this.label5.TabIndex = 43;
            this.label5.Text = "废料编号 ";
            // 
            // txtGoodsCode
            // 
            this.txtGoodsCode.Enabled = false;
            this.txtGoodsCode.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtGoodsCode.Location = new System.Drawing.Point(408, 71);
            this.txtGoodsCode.Name = "txtGoodsCode";
            this.txtGoodsCode.Size = new System.Drawing.Size(142, 23);
            this.txtGoodsCode.TabIndex = 44;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label8.Location = new System.Drawing.Point(28, 75);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 14);
            this.label8.TabIndex = 47;
            this.label8.Text = "货格类型";
            // 
            // txtLoacteType
            // 
            this.txtLoacteType.Enabled = false;
            this.txtLoacteType.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtLoacteType.Location = new System.Drawing.Point(100, 71);
            this.txtLoacteType.Name = "txtLoacteType";
            this.txtLoacteType.Size = new System.Drawing.Size(142, 23);
            this.txtLoacteType.TabIndex = 48;
            // 
            // FrmChangeLocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(572, 234);
            this.Controls.Add(this.txtLoacteType);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.txtGoodsCode);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbBatch);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.btnExc);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.tbEndLocation);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbStartLocation);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.tbContainerNo);
            this.Controls.Add(this.label2);
            this.Name = "FrmChangeLocation";
            this.Text = "库位调整";
            this.Load += new System.EventHandler(this.FrmChangeLocation_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbContainerNo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbStartLocation;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbEndLocation;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnExc;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbBatch;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtGoodsCode;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox txtLoacteType;
    }
}