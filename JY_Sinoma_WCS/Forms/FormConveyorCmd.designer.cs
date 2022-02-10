namespace JY_Sinoma_WCS
{
    partial class FormConveyorCmd
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
            this.btOK = new System.Windows.Forms.Button();
            this.tbWriteId = new System.Windows.Forms.TextBox();
            this.btWriteId = new System.Windows.Forms.Button();
            this.btinit = new System.Windows.Forms.Button();
            this.btReset = new System.Windows.Forms.Button();
            this.btAuto = new System.Windows.Forms.Button();
            this.btmanul = new System.Windows.Forms.Button();
            this.btn_Eorror_check = new System.Windows.Forms.Button();
            this.lbLoadStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.BtnControl = new System.Windows.Forms.Button();
            this.CmbControl = new System.Windows.Forms.ComboBox();
            this.lbFrom = new System.Windows.Forms.Label();
            this.txtFrom = new System.Windows.Forms.TextBox();
            this.lbTo = new System.Windows.Forms.Label();
            this.txtTo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbDeviceStatus = new System.Windows.Forms.Label();
            this.cmbLoadType = new System.Windows.Forms.ComboBox();
            this.cmbTaskType = new System.Windows.Forms.ComboBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.btnDel = new System.Windows.Forms.Button();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btOK
            // 
            this.btOK.Font = new System.Drawing.Font("宋体", 10.5F);
            this.btOK.Location = new System.Drawing.Point(272, 180);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(110, 45);
            this.btOK.TabIndex = 6;
            this.btOK.Text = "关闭";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // tbWriteId
            // 
            this.tbWriteId.Location = new System.Drawing.Point(480, 107);
            this.tbWriteId.Name = "tbWriteId";
            this.tbWriteId.Size = new System.Drawing.Size(77, 21);
            this.tbWriteId.TabIndex = 15;
            this.tbWriteId.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbWriteId_KeyPress);
            // 
            // btWriteId
            // 
            this.btWriteId.Location = new System.Drawing.Point(272, 110);
            this.btWriteId.Name = "btWriteId";
            this.btWriteId.Size = new System.Drawing.Size(110, 45);
            this.btWriteId.TabIndex = 14;
            this.btWriteId.Text = "写任务";
            this.btWriteId.UseVisualStyleBackColor = true;
            this.btWriteId.Click += new System.EventHandler(this.btWriteId_Click);
            // 
            // btinit
            // 
            this.btinit.Location = new System.Drawing.Point(139, 110);
            this.btinit.Name = "btinit";
            this.btinit.Size = new System.Drawing.Size(110, 45);
            this.btinit.TabIndex = 13;
            this.btinit.Text = "初始化";
            this.btinit.UseVisualStyleBackColor = true;
            this.btinit.Click += new System.EventHandler(this.btinit_Click);
            // 
            // btReset
            // 
            this.btReset.Location = new System.Drawing.Point(139, 180);
            this.btReset.Name = "btReset";
            this.btReset.Size = new System.Drawing.Size(110, 45);
            this.btReset.TabIndex = 12;
            this.btReset.Text = "清错";
            this.btReset.UseVisualStyleBackColor = true;
            this.btReset.Click += new System.EventHandler(this.btReset_Click);
            // 
            // btAuto
            // 
            this.btAuto.Location = new System.Drawing.Point(12, 180);
            this.btAuto.Name = "btAuto";
            this.btAuto.Size = new System.Drawing.Size(110, 45);
            this.btAuto.TabIndex = 16;
            this.btAuto.Text = "自动";
            this.btAuto.UseVisualStyleBackColor = true;
            this.btAuto.Click += new System.EventHandler(this.btAuto_Click);
            // 
            // btmanul
            // 
            this.btmanul.Location = new System.Drawing.Point(12, 110);
            this.btmanul.Name = "btmanul";
            this.btmanul.Size = new System.Drawing.Size(110, 45);
            this.btmanul.TabIndex = 17;
            this.btmanul.Text = "手动";
            this.btmanul.UseVisualStyleBackColor = true;
            this.btmanul.Click += new System.EventHandler(this.btmanul_Click);
            // 
            // btn_Eorror_check
            // 
            this.btn_Eorror_check.ForeColor = System.Drawing.Color.Black;
            this.btn_Eorror_check.Location = new System.Drawing.Point(614, 180);
            this.btn_Eorror_check.Name = "btn_Eorror_check";
            this.btn_Eorror_check.Size = new System.Drawing.Size(110, 45);
            this.btn_Eorror_check.TabIndex = 147;
            this.btn_Eorror_check.Text = "出库异常处理";
            this.btn_Eorror_check.UseVisualStyleBackColor = true;
            this.btn_Eorror_check.Click += new System.EventHandler(this.btn_Eorror_check_Click);
            // 
            // lbLoadStatus
            // 
            this.lbLoadStatus.AutoSize = true;
            this.lbLoadStatus.Font = new System.Drawing.Font("宋体", 10.5F);
            this.lbLoadStatus.Location = new System.Drawing.Point(4, 14);
            this.lbLoadStatus.Name = "lbLoadStatus";
            this.lbLoadStatus.Size = new System.Drawing.Size(77, 14);
            this.lbLoadStatus.TabIndex = 1;
            this.lbLoadStatus.Text = "设备：正常";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(433, 110);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 153;
            this.label1.Text = "任务号";
            // 
            // button1
            // 
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(753, 180);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(110, 45);
            this.button1.TabIndex = 157;
            this.button1.Text = "停止点动命令";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnControl
            // 
            this.BtnControl.ForeColor = System.Drawing.Color.Black;
            this.BtnControl.Location = new System.Drawing.Point(753, 110);
            this.BtnControl.Name = "BtnControl";
            this.BtnControl.Size = new System.Drawing.Size(110, 45);
            this.BtnControl.TabIndex = 156;
            this.BtnControl.Text = "执行点动命令";
            this.BtnControl.UseVisualStyleBackColor = true;
            this.BtnControl.Click += new System.EventHandler(this.BtnControl_Click);
            // 
            // CmbControl
            // 
            this.CmbControl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbControl.FormattingEnabled = true;
            this.CmbControl.Location = new System.Drawing.Point(609, 122);
            this.CmbControl.Name = "CmbControl";
            this.CmbControl.Size = new System.Drawing.Size(121, 20);
            this.CmbControl.TabIndex = 155;
            // 
            // lbFrom
            // 
            this.lbFrom.AutoSize = true;
            this.lbFrom.Location = new System.Drawing.Point(421, 133);
            this.lbFrom.Name = "lbFrom";
            this.lbFrom.Size = new System.Drawing.Size(53, 12);
            this.lbFrom.TabIndex = 159;
            this.lbFrom.Text = "起始地址";
            // 
            // txtFrom
            // 
            this.txtFrom.Location = new System.Drawing.Point(480, 130);
            this.txtFrom.Name = "txtFrom";
            this.txtFrom.Size = new System.Drawing.Size(77, 21);
            this.txtFrom.TabIndex = 158;
            // 
            // lbTo
            // 
            this.lbTo.AutoSize = true;
            this.lbTo.Location = new System.Drawing.Point(421, 154);
            this.lbTo.Name = "lbTo";
            this.lbTo.Size = new System.Drawing.Size(53, 12);
            this.lbTo.TabIndex = 161;
            this.lbTo.Text = "目的地址";
            // 
            // txtTo
            // 
            this.txtTo.Location = new System.Drawing.Point(480, 151);
            this.txtTo.Name = "txtTo";
            this.txtTo.Size = new System.Drawing.Size(77, 21);
            this.txtTo.TabIndex = 160;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(420, 184);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 163;
            this.label2.Text = "货物类型";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(421, 213);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 165;
            this.label3.Text = "任务类型";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel1.Controls.Add(this.lbDeviceStatus);
            this.panel1.Location = new System.Drawing.Point(12, 6);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(978, 45);
            this.panel1.TabIndex = 149;
            // 
            // lbDeviceStatus
            // 
            this.lbDeviceStatus.AutoSize = true;
            this.lbDeviceStatus.Font = new System.Drawing.Font("宋体", 10.5F);
            this.lbDeviceStatus.Location = new System.Drawing.Point(4, 14);
            this.lbDeviceStatus.Name = "lbDeviceStatus";
            this.lbDeviceStatus.Size = new System.Drawing.Size(77, 14);
            this.lbDeviceStatus.TabIndex = 1;
            this.lbDeviceStatus.Text = "设备：正常";
            // 
            // cmbLoadType
            // 
            this.cmbLoadType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbLoadType.FormattingEnabled = true;
            this.cmbLoadType.Items.AddRange(new object[] {
            "--请选择--",
            "吨桶",
            "圆桶",
            "整摞空托盘",
            "单空托盘"});
            this.cmbLoadType.Location = new System.Drawing.Point(479, 179);
            this.cmbLoadType.Name = "cmbLoadType";
            this.cmbLoadType.Size = new System.Drawing.Size(102, 20);
            this.cmbLoadType.TabIndex = 166;
            // 
            // cmbTaskType
            // 
            this.cmbTaskType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTaskType.FormattingEnabled = true;
            this.cmbTaskType.Items.AddRange(new object[] {
            "--请选择--",
            "入库",
            "出库",
            "空托盘入库"});
            this.cmbTaskType.Location = new System.Drawing.Point(479, 209);
            this.cmbTaskType.Name = "cmbTaskType";
            this.cmbTaskType.Size = new System.Drawing.Size(102, 20);
            this.cmbTaskType.TabIndex = 167;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.label4);
            this.panel2.Location = new System.Drawing.Point(12, 57);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(978, 45);
            this.panel2.TabIndex = 168;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("宋体", 10.5F);
            this.label4.Location = new System.Drawing.Point(4, 14);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(77, 14);
            this.label4.TabIndex = 1;
            this.label4.Text = "设备：正常";
            // 
            // btnDel
            // 
            this.btnDel.Enabled = false;
            this.btnDel.ForeColor = System.Drawing.Color.Black;
            this.btnDel.Location = new System.Drawing.Point(880, 110);
            this.btnDel.Name = "btnDel";
            this.btnDel.Size = new System.Drawing.Size(110, 115);
            this.btnDel.TabIndex = 169;
            this.btnDel.Text = "入库任务取消";
            this.btnDel.UseVisualStyleBackColor = true;
            this.btnDel.Click += new System.EventHandler(this.btnDel_Click);
            // 
            // FormConveyorCmd
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(1002, 245);
            this.Controls.Add(this.btnDel);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.cmbTaskType);
            this.Controls.Add(this.cmbLoadType);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbTo);
            this.Controls.Add(this.txtTo);
            this.Controls.Add(this.lbFrom);
            this.Controls.Add(this.txtFrom);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BtnControl);
            this.Controls.Add(this.CmbControl);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btn_Eorror_check);
            this.Controls.Add(this.btmanul);
            this.Controls.Add(this.btAuto);
            this.Controls.Add(this.tbWriteId);
            this.Controls.Add(this.btWriteId);
            this.Controls.Add(this.btinit);
            this.Controls.Add(this.btReset);
            this.Controls.Add(this.btOK);
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.Name = "FormConveyorCmd";
            this.Text = "辊道";
            this.Load += new System.EventHandler(this.FormConveyor_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.TextBox tbWriteId;
        private System.Windows.Forms.Button btWriteId;
        private System.Windows.Forms.Button btinit;
        private System.Windows.Forms.Button btReset;
        private System.Windows.Forms.Button btAuto;
        private System.Windows.Forms.Button btmanul;
        private System.Windows.Forms.Button btn_Eorror_check;
        private System.Windows.Forms.Label lbLoadStatus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button BtnControl;
        private System.Windows.Forms.ComboBox CmbControl;
        private System.Windows.Forms.Label lbFrom;
        private System.Windows.Forms.TextBox txtFrom;
        private System.Windows.Forms.Label lbTo;
        private System.Windows.Forms.TextBox txtTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbDeviceStatus;
        private System.Windows.Forms.ComboBox cmbLoadType;
        private System.Windows.Forms.ComboBox cmbTaskType;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.Button btnDel;
    }
}