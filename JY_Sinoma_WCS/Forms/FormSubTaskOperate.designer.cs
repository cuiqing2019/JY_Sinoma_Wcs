namespace JY_Sinoma_WCS
{
    partial class FormSubTaskOperate
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSubTaskOperate));
            this.label1 = new System.Windows.Forms.Label();
            this.tbDTaskID = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tbDTaskType = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.tbFromUnit = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.tbToUnit = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cmbStep = new System.Windows.Forms.ComboBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbChannel = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.tbDevice = new System.Windows.Forms.TextBox();
            this.btCancel = new System.Windows.Forms.Button();
            this.btUpdate = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.tbMTaskType = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(29, 40);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "任务号";
            // 
            // tbDTaskID
            // 
            this.tbDTaskID.Enabled = false;
            this.tbDTaskID.Location = new System.Drawing.Point(76, 36);
            this.tbDTaskID.Name = "tbDTaskID";
            this.tbDTaskID.Size = new System.Drawing.Size(100, 21);
            this.tbDTaskID.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(5, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "子任务类型";
            // 
            // tbDTaskType
            // 
            this.tbDTaskType.Enabled = false;
            this.tbDTaskType.Location = new System.Drawing.Point(76, 75);
            this.tbDTaskType.Name = "tbDTaskType";
            this.tbDTaskType.Size = new System.Drawing.Size(100, 21);
            this.tbDTaskType.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(17, 157);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 4;
            this.label3.Text = "起始地址";
            // 
            // tbFromUnit
            // 
            this.tbFromUnit.Enabled = false;
            this.tbFromUnit.Location = new System.Drawing.Point(75, 153);
            this.tbFromUnit.Name = "tbFromUnit";
            this.tbFromUnit.Size = new System.Drawing.Size(100, 21);
            this.tbFromUnit.TabIndex = 5;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(261, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 6;
            this.label4.Text = "目的地址";
            // 
            // tbToUnit
            // 
            this.tbToUnit.Enabled = false;
            this.tbToUnit.Location = new System.Drawing.Point(324, 114);
            this.tbToUnit.Name = "tbToUnit";
            this.tbToUnit.Size = new System.Drawing.Size(100, 21);
            this.tbToUnit.TabIndex = 7;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(41, 196);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(29, 12);
            this.label5.TabIndex = 8;
            this.label5.Text = "步骤";
            // 
            // cmbStep
            // 
            this.cmbStep.FormattingEnabled = true;
            this.cmbStep.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4"});
            this.cmbStep.Location = new System.Drawing.Point(75, 192);
            this.cmbStep.Name = "cmbStep";
            this.cmbStep.Size = new System.Drawing.Size(100, 20);
            this.cmbStep.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 118);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 10;
            this.label6.Text = "执行设备";
            // 
            // tbChannel
            // 
            this.tbChannel.Enabled = false;
            this.tbChannel.Location = new System.Drawing.Point(324, 75);
            this.tbChannel.Name = "tbChannel";
            this.tbChannel.Size = new System.Drawing.Size(100, 21);
            this.tbChannel.TabIndex = 13;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(285, 157);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(29, 12);
            this.label9.TabIndex = 16;
            this.label9.Text = "状态";
            // 
            // cmbStatus
            // 
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Items.AddRange(new object[] {
            "新生成",
            "执行中",
            "已完成"});
            this.cmbStatus.Location = new System.Drawing.Point(324, 153);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(100, 20);
            this.cmbStatus.TabIndex = 17;
            // 
            // tbDevice
            // 
            this.tbDevice.Enabled = false;
            this.tbDevice.Location = new System.Drawing.Point(76, 114);
            this.tbDevice.Name = "tbDevice";
            this.tbDevice.Size = new System.Drawing.Size(100, 21);
            this.tbDevice.TabIndex = 18;
            // 
            // btCancel
            // 
            this.btCancel.Location = new System.Drawing.Point(248, 243);
            this.btCancel.Name = "btCancel";
            this.btCancel.Size = new System.Drawing.Size(140, 41);
            this.btCancel.TabIndex = 20;
            this.btCancel.Text = "取消操作";
            this.btCancel.UseVisualStyleBackColor = true;
            this.btCancel.Click += new System.EventHandler(this.btCancel_Click);
            // 
            // btUpdate
            // 
            this.btUpdate.Location = new System.Drawing.Point(75, 243);
            this.btUpdate.Name = "btUpdate";
            this.btUpdate.Size = new System.Drawing.Size(111, 41);
            this.btUpdate.TabIndex = 21;
            this.btUpdate.Text = "修改任务";
            this.btUpdate.UseVisualStyleBackColor = true;
            this.btUpdate.Click += new System.EventHandler(this.btUpdate_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(282, 79);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(29, 12);
            this.label7.TabIndex = 12;
            this.label7.Text = "巷道";
            // 
            // tbMTaskType
            // 
            this.tbMTaskType.Enabled = false;
            this.tbMTaskType.Location = new System.Drawing.Point(324, 36);
            this.tbMTaskType.Name = "tbMTaskType";
            this.tbMTaskType.Size = new System.Drawing.Size(100, 21);
            this.tbMTaskType.TabIndex = 25;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(253, 40);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 24;
            this.label8.Text = "主任务类型";
            // 
            // FormSubTaskOperate
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(446, 307);
            this.Controls.Add(this.tbMTaskType);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btUpdate);
            this.Controls.Add(this.btCancel);
            this.Controls.Add(this.tbDevice);
            this.Controls.Add(this.cmbStatus);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.tbChannel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.cmbStep);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.tbToUnit);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tbFromUnit);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.tbDTaskType);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbDTaskID);
            this.Controls.Add(this.label1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSubTaskOperate";
            this.Text = "编辑子任务";
            this.Load += new System.EventHandler(this.FormSubTaskOperate_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbDTaskID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox tbDTaskType;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox tbFromUnit;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox tbToUnit;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox cmbStep;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbChannel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbStatus;
        private System.Windows.Forms.TextBox tbDevice;
        private System.Windows.Forms.Button btCancel;
        private System.Windows.Forms.Button btUpdate;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbMTaskType;
        private System.Windows.Forms.Label label8;
    }
}