namespace JY_Sinoma_WCS
{
    partial class FormTray
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
            this.btWriteId = new System.Windows.Forms.Button();
            this.btinit = new System.Windows.Forms.Button();
            this.btReset = new System.Windows.Forms.Button();
            this.btAuto = new System.Windows.Forms.Button();
            this.btmanul = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbLoadStatus = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.BtnControl = new System.Windows.Forms.Button();
            this.CmbControl = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.lbDeviceStatus = new System.Windows.Forms.Label();
            this.cmbLoadType = new System.Windows.Forms.ComboBox();
            this.cmbTaskType = new System.Windows.Forms.ComboBox();
            this.tbStop = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // btOK
            // 
            this.btOK.Font = new System.Drawing.Font("宋体", 10.5F);
            this.btOK.Location = new System.Drawing.Point(272, 177);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(110, 45);
            this.btOK.TabIndex = 6;
            this.btOK.Text = "关闭";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btWriteId
            // 
            this.btWriteId.Location = new System.Drawing.Point(272, 108);
            this.btWriteId.Name = "btWriteId";
            this.btWriteId.Size = new System.Drawing.Size(180, 45);
            this.btWriteId.TabIndex = 14;
            this.btWriteId.Text = "备用入库口放托盘";
            this.btWriteId.UseVisualStyleBackColor = true;
            this.btWriteId.Click += new System.EventHandler(this.btWriteId_Click);
            // 
            // btinit
            // 
            this.btinit.Location = new System.Drawing.Point(139, 108);
            this.btinit.Name = "btinit";
            this.btinit.Size = new System.Drawing.Size(110, 45);
            this.btinit.TabIndex = 13;
            this.btinit.Text = "初始化";
            this.btinit.UseVisualStyleBackColor = true;
            this.btinit.Click += new System.EventHandler(this.btinit_Click);
            // 
            // btReset
            // 
            this.btReset.Location = new System.Drawing.Point(139, 178);
            this.btReset.Name = "btReset";
            this.btReset.Size = new System.Drawing.Size(110, 45);
            this.btReset.TabIndex = 12;
            this.btReset.Text = "清错";
            this.btReset.UseVisualStyleBackColor = true;
            this.btReset.Click += new System.EventHandler(this.btReset_Click);
            // 
            // btAuto
            // 
            this.btAuto.Location = new System.Drawing.Point(12, 178);
            this.btAuto.Name = "btAuto";
            this.btAuto.Size = new System.Drawing.Size(110, 45);
            this.btAuto.TabIndex = 16;
            this.btAuto.Text = "自动";
            this.btAuto.UseVisualStyleBackColor = true;
            this.btAuto.Click += new System.EventHandler(this.btAuto_Click);
            // 
            // btmanul
            // 
            this.btmanul.Location = new System.Drawing.Point(12, 108);
            this.btmanul.Name = "btmanul";
            this.btmanul.Size = new System.Drawing.Size(110, 45);
            this.btmanul.TabIndex = 17;
            this.btmanul.Text = "手动";
            this.btmanul.UseVisualStyleBackColor = true;
            this.btmanul.Click += new System.EventHandler(this.btmanul_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel2.Controls.Add(this.lbLoadStatus);
            this.panel2.Location = new System.Drawing.Point(12, 57);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(923, 45);
            this.panel2.TabIndex = 148;
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
            // button1
            // 
            this.button1.ForeColor = System.Drawing.Color.Black;
            this.button1.Location = new System.Drawing.Point(825, 182);
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
            this.BtnControl.Location = new System.Drawing.Point(817, 108);
            this.BtnControl.Name = "BtnControl";
            this.BtnControl.Size = new System.Drawing.Size(110, 39);
            this.BtnControl.TabIndex = 156;
            this.BtnControl.Text = "执行点动命令";
            this.BtnControl.UseVisualStyleBackColor = true;
            this.BtnControl.Click += new System.EventHandler(this.BtnControl_Click);
            // 
            // CmbControl
            // 
            this.CmbControl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbControl.FormattingEnabled = true;
            this.CmbControl.Items.AddRange(new object[] {
            "--选择点动命令--",
            "输送电机正传",
            "提升上升",
            "提升下降",
            "伸叉气缸伸",
            "伸叉气缸缩",
            "输送电机反转"});
            this.CmbControl.Location = new System.Drawing.Point(655, 118);
            this.CmbControl.Name = "CmbControl";
            this.CmbControl.Size = new System.Drawing.Size(121, 20);
            this.CmbControl.TabIndex = 155;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(449, 182);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 163;
            this.label2.Text = "货物类型";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(450, 211);
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
            this.panel1.Size = new System.Drawing.Size(923, 45);
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
            "货物托盘",
            "单个空托盘",
            "整摞空托盘"});
            this.cmbLoadType.Location = new System.Drawing.Point(508, 177);
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
            "出库"});
            this.cmbTaskType.Location = new System.Drawing.Point(508, 207);
            this.cmbTaskType.Name = "cmbTaskType";
            this.cmbTaskType.Size = new System.Drawing.Size(102, 20);
            this.cmbTaskType.TabIndex = 167;
            // 
            // tbStop
            // 
            this.tbStop.Location = new System.Drawing.Point(480, 108);
            this.tbStop.Name = "tbStop";
            this.tbStop.Size = new System.Drawing.Size(130, 45);
            this.tbStop.TabIndex = 168;
            this.tbStop.Text = "停止放托盘";
            this.tbStop.UseVisualStyleBackColor = true;
            this.tbStop.Click += new System.EventHandler(this.stop_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(655, 166);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(110, 40);
            this.button2.TabIndex = 169;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // FormTray
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(947, 234);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.tbStop);
            this.Controls.Add(this.cmbTaskType);
            this.Controls.Add(this.cmbLoadType);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BtnControl);
            this.Controls.Add(this.CmbControl);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btmanul);
            this.Controls.Add(this.btAuto);
            this.Controls.Add(this.btWriteId);
            this.Controls.Add(this.btinit);
            this.Controls.Add(this.btReset);
            this.Controls.Add(this.btOK);
            this.Name = "FormTray";
            this.Text = "辊道";
            this.Load += new System.EventHandler(this.FormConveyor_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btWriteId;
        private System.Windows.Forms.Button btinit;
        private System.Windows.Forms.Button btReset;
        private System.Windows.Forms.Button btAuto;
        private System.Windows.Forms.Button btmanul;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbLoadStatus;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button BtnControl;
        private System.Windows.Forms.ComboBox CmbControl;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label lbDeviceStatus;
        private System.Windows.Forms.ComboBox cmbLoadType;
        private System.Windows.Forms.ComboBox cmbTaskType;
        private System.Windows.Forms.Button tbStop;
        private System.Windows.Forms.Button button2;
    }
}