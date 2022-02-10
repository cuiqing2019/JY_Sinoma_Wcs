namespace JY_Sinoma_WCS
{
    partial class FormSystemStatus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSystemStatus));
            this.gb_SystemStatus = new System.Windows.Forms.GroupBox();
            this.checkStopSpot = new System.Windows.Forms.CheckBox();
            this.checkStopCabinet = new System.Windows.Forms.CheckBox();
            this.checkAuto = new System.Windows.Forms.CheckBox();
            this.checkWarning = new System.Windows.Forms.CheckBox();
            this.checkSystemRun = new System.Windows.Forms.CheckBox();
            this.gb_control = new System.Windows.Forms.GroupBox();
            this.bt_initSystem = new System.Windows.Forms.Button();
            this.bt_OK = new System.Windows.Forms.Button();
            this.bt_Auto = new System.Windows.Forms.Button();
            this.bt_ClearAll = new System.Windows.Forms.Button();
            this.gb_SystemStatus.SuspendLayout();
            this.gb_control.SuspendLayout();
            this.SuspendLayout();
            // 
            // gb_SystemStatus
            // 
            this.gb_SystemStatus.Controls.Add(this.checkStopSpot);
            this.gb_SystemStatus.Controls.Add(this.checkStopCabinet);
            this.gb_SystemStatus.Controls.Add(this.checkAuto);
            this.gb_SystemStatus.Controls.Add(this.checkWarning);
            this.gb_SystemStatus.Controls.Add(this.checkSystemRun);
            this.gb_SystemStatus.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gb_SystemStatus.Location = new System.Drawing.Point(26, 21);
            this.gb_SystemStatus.Name = "gb_SystemStatus";
            this.gb_SystemStatus.Size = new System.Drawing.Size(140, 170);
            this.gb_SystemStatus.TabIndex = 0;
            this.gb_SystemStatus.TabStop = false;
            this.gb_SystemStatus.Text = "系统状态";
            // 
            // checkStopSpot
            // 
            this.checkStopSpot.AutoSize = true;
            this.checkStopSpot.Enabled = false;
            this.checkStopSpot.Location = new System.Drawing.Point(19, 139);
            this.checkStopSpot.Name = "checkStopSpot";
            this.checkStopSpot.Size = new System.Drawing.Size(82, 18);
            this.checkStopSpot.TabIndex = 13;
            this.checkStopSpot.Text = "现场急停";
            this.checkStopSpot.UseVisualStyleBackColor = true;
            // 
            // checkStopCabinet
            // 
            this.checkStopCabinet.AutoSize = true;
            this.checkStopCabinet.Enabled = false;
            this.checkStopCabinet.Location = new System.Drawing.Point(19, 111);
            this.checkStopCabinet.Name = "checkStopCabinet";
            this.checkStopCabinet.Size = new System.Drawing.Size(82, 18);
            this.checkStopCabinet.TabIndex = 12;
            this.checkStopCabinet.Text = "机柜急停";
            this.checkStopCabinet.UseVisualStyleBackColor = true;
            // 
            // checkAuto
            // 
            this.checkAuto.AutoSize = true;
            this.checkAuto.Enabled = false;
            this.checkAuto.Location = new System.Drawing.Point(19, 83);
            this.checkAuto.Name = "checkAuto";
            this.checkAuto.Size = new System.Drawing.Size(54, 18);
            this.checkAuto.TabIndex = 10;
            this.checkAuto.Text = "自动";
            this.checkAuto.UseVisualStyleBackColor = true;
            // 
            // checkWarning
            // 
            this.checkWarning.AutoSize = true;
            this.checkWarning.Enabled = false;
            this.checkWarning.Location = new System.Drawing.Point(19, 55);
            this.checkWarning.Name = "checkWarning";
            this.checkWarning.Size = new System.Drawing.Size(82, 18);
            this.checkWarning.TabIndex = 9;
            this.checkWarning.Text = "启动预警";
            this.checkWarning.UseVisualStyleBackColor = true;
            // 
            // checkSystemRun
            // 
            this.checkSystemRun.AutoSize = true;
            this.checkSystemRun.Enabled = false;
            this.checkSystemRun.Location = new System.Drawing.Point(19, 27);
            this.checkSystemRun.Name = "checkSystemRun";
            this.checkSystemRun.Size = new System.Drawing.Size(82, 18);
            this.checkSystemRun.TabIndex = 7;
            this.checkSystemRun.Text = "系统运行";
            this.checkSystemRun.UseVisualStyleBackColor = true;
            // 
            // gb_control
            // 
            this.gb_control.Controls.Add(this.bt_initSystem);
            this.gb_control.Controls.Add(this.bt_OK);
            this.gb_control.Controls.Add(this.bt_Auto);
            this.gb_control.Controls.Add(this.bt_ClearAll);
            this.gb_control.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.gb_control.Location = new System.Drawing.Point(202, 21);
            this.gb_control.Name = "gb_control";
            this.gb_control.Size = new System.Drawing.Size(148, 170);
            this.gb_control.TabIndex = 1;
            this.gb_control.TabStop = false;
            this.gb_control.Text = "系统控制";
            // 
            // bt_initSystem
            // 
            this.bt_initSystem.Location = new System.Drawing.Point(31, 98);
            this.bt_initSystem.Name = "bt_initSystem";
            this.bt_initSystem.Size = new System.Drawing.Size(75, 23);
            this.bt_initSystem.TabIndex = 3;
            this.bt_initSystem.Text = "总初始化";
            this.bt_initSystem.UseVisualStyleBackColor = true;
            this.bt_initSystem.Click += new System.EventHandler(this.bt_initSystem1_Click);
            // 
            // bt_OK
            // 
            this.bt_OK.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.bt_OK.Location = new System.Drawing.Point(31, 132);
            this.bt_OK.Name = "bt_OK";
            this.bt_OK.Size = new System.Drawing.Size(75, 23);
            this.bt_OK.TabIndex = 2;
            this.bt_OK.Text = "关闭";
            this.bt_OK.UseVisualStyleBackColor = true;
            this.bt_OK.Click += new System.EventHandler(this.bt_OK_Click);
            // 
            // bt_Auto
            // 
            this.bt_Auto.Location = new System.Drawing.Point(31, 64);
            this.bt_Auto.Name = "bt_Auto";
            this.bt_Auto.Size = new System.Drawing.Size(75, 23);
            this.bt_Auto.TabIndex = 2;
            this.bt_Auto.Text = "手动";
            this.bt_Auto.UseVisualStyleBackColor = true;
            this.bt_Auto.Click += new System.EventHandler(this.bt_Auto_Click);
            // 
            // bt_ClearAll
            // 
            this.bt_ClearAll.Location = new System.Drawing.Point(31, 30);
            this.bt_ClearAll.Name = "bt_ClearAll";
            this.bt_ClearAll.Size = new System.Drawing.Size(75, 23);
            this.bt_ClearAll.TabIndex = 0;
            this.bt_ClearAll.Text = "故障总清";
            this.bt_ClearAll.UseVisualStyleBackColor = true;
            this.bt_ClearAll.Click += new System.EventHandler(this.bt_ClearAll_Click);
            // 
            // FormSystemStatus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(381, 216);
            this.Controls.Add(this.gb_control);
            this.Controls.Add(this.gb_SystemStatus);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSystemStatus";
            this.Text = "系统状态信息";
            this.Load += new System.EventHandler(this.FormSystemStatus_Load);
            this.gb_SystemStatus.ResumeLayout(false);
            this.gb_SystemStatus.PerformLayout();
            this.gb_control.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gb_SystemStatus;
        private System.Windows.Forms.GroupBox gb_control;
        private System.Windows.Forms.Button bt_Auto;
        private System.Windows.Forms.Button bt_ClearAll;
        private System.Windows.Forms.Button bt_OK;
        private System.Windows.Forms.CheckBox checkStopCabinet;
        private System.Windows.Forms.CheckBox checkAuto;
        private System.Windows.Forms.CheckBox checkWarning;
        private System.Windows.Forms.CheckBox checkSystemRun;
        private System.Windows.Forms.CheckBox checkStopSpot;
        private System.Windows.Forms.Button bt_initSystem;
    }
}