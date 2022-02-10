namespace JY_Sinoma_WCS.Forms
{
    partial class FormWorkModeLevel2
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
            this.cmbTaskType = new System.Windows.Forms.ComboBox();
            this.lbTaskType = new System.Windows.Forms.Label();
            this.btnChangeTaskType = new System.Windows.Forms.Button();
            this.btnStop = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // cmbTaskType
            // 
            this.cmbTaskType.AutoCompleteCustomSource.AddRange(new string[] {
            "--请选择任务模式--",
            "出库",
            "空托入库",
            "异常回库"});
            this.cmbTaskType.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.cmbTaskType.FormattingEnabled = true;
            this.cmbTaskType.Items.AddRange(new object[] {
            "空闲模式",
            "出库模式",
            "空托入库模式",
            "异常回库模式"});
            this.cmbTaskType.Location = new System.Drawing.Point(45, 92);
            this.cmbTaskType.Name = "cmbTaskType";
            this.cmbTaskType.Size = new System.Drawing.Size(128, 22);
            this.cmbTaskType.TabIndex = 0;
            // 
            // lbTaskType
            // 
            this.lbTaskType.AutoSize = true;
            this.lbTaskType.Location = new System.Drawing.Point(48, 47);
            this.lbTaskType.Name = "lbTaskType";
            this.lbTaskType.Size = new System.Drawing.Size(89, 12);
            this.lbTaskType.TabIndex = 1;
            this.lbTaskType.Text = "当前任务模式：";
            // 
            // btnChangeTaskType
            // 
            this.btnChangeTaskType.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnChangeTaskType.Location = new System.Drawing.Point(201, 82);
            this.btnChangeTaskType.Name = "btnChangeTaskType";
            this.btnChangeTaskType.Size = new System.Drawing.Size(137, 42);
            this.btnChangeTaskType.TabIndex = 2;
            this.btnChangeTaskType.Text = "切 换";
            this.btnChangeTaskType.UseVisualStyleBackColor = true;
            this.btnChangeTaskType.Click += new System.EventHandler(this.btnChangeTaskType_Click);
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStop.Location = new System.Drawing.Point(201, 32);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(137, 40);
            this.btnStop.TabIndex = 42;
            this.btnStop.Text = "停止生成任务";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // FormWorkModeLevel2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(399, 162);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnChangeTaskType);
            this.Controls.Add(this.lbTaskType);
            this.Controls.Add(this.cmbTaskType);
            this.Name = "FormWorkModeLevel2";
            this.Text = "二楼工作模式设置";
            this.Load += new System.EventHandler(this.FormWorkModeLevel2_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox cmbTaskType;
        private System.Windows.Forms.Label lbTaskType;
        private System.Windows.Forms.Button btnChangeTaskType;
        private System.Windows.Forms.Button btnStop;
    }
}