namespace JY_Sinoma_WCS
{
    partial class FormConveyorNoLoad
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
            this.btinit = new System.Windows.Forms.Button();
            this.btReset = new System.Windows.Forms.Button();
            this.btAuto = new System.Windows.Forms.Button();
            this.btmanul = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.lbLoadStatus = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.BtnControl = new System.Windows.Forms.Button();
            this.CmbControl = new System.Windows.Forms.ComboBox();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btOK
            // 
            this.btOK.Font = new System.Drawing.Font("宋体", 10.5F);
            this.btOK.Location = new System.Drawing.Point(700, 68);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(110, 45);
            this.btOK.TabIndex = 6;
            this.btOK.Text = "关闭";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // btinit
            // 
            this.btinit.Location = new System.Drawing.Point(186, 68);
            this.btinit.Name = "btinit";
            this.btinit.Size = new System.Drawing.Size(110, 45);
            this.btinit.TabIndex = 13;
            this.btinit.Text = "初始化";
            this.btinit.UseVisualStyleBackColor = true;
            this.btinit.Click += new System.EventHandler(this.btinit_Click);
            // 
            // btReset
            // 
            this.btReset.Location = new System.Drawing.Point(186, 121);
            this.btReset.Name = "btReset";
            this.btReset.Size = new System.Drawing.Size(110, 45);
            this.btReset.TabIndex = 12;
            this.btReset.Text = "清错";
            this.btReset.UseVisualStyleBackColor = true;
            this.btReset.Click += new System.EventHandler(this.btReset_Click);
            // 
            // btAuto
            // 
            this.btAuto.Location = new System.Drawing.Point(22, 121);
            this.btAuto.Name = "btAuto";
            this.btAuto.Size = new System.Drawing.Size(110, 45);
            this.btAuto.TabIndex = 16;
            this.btAuto.Text = "自动";
            this.btAuto.UseVisualStyleBackColor = true;
            this.btAuto.Click += new System.EventHandler(this.btAuto_Click);
            // 
            // btmanul
            // 
            this.btmanul.Location = new System.Drawing.Point(22, 68);
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
            this.panel2.Location = new System.Drawing.Point(12, 12);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(807, 45);
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
            this.button1.Location = new System.Drawing.Point(525, 121);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(121, 45);
            this.button1.TabIndex = 157;
            this.button1.Text = "停止点动命令";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BtnControl
            // 
            this.BtnControl.ForeColor = System.Drawing.Color.Black;
            this.BtnControl.Location = new System.Drawing.Point(525, 68);
            this.BtnControl.Name = "BtnControl";
            this.BtnControl.Size = new System.Drawing.Size(121, 45);
            this.BtnControl.TabIndex = 156;
            this.BtnControl.Text = "执行点动命令";
            this.BtnControl.UseVisualStyleBackColor = true;
            this.BtnControl.Click += new System.EventHandler(this.BtnControl_Click);
            // 
            // CmbControl
            // 
            this.CmbControl.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CmbControl.FormattingEnabled = true;
            this.CmbControl.Location = new System.Drawing.Point(350, 80);
            this.CmbControl.Name = "CmbControl";
            this.CmbControl.Size = new System.Drawing.Size(121, 20);
            this.CmbControl.TabIndex = 155;
            // 
            // FormConveyorNoLoad
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(825, 177);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.BtnControl);
            this.Controls.Add(this.CmbControl);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.btmanul);
            this.Controls.Add(this.btAuto);
            this.Controls.Add(this.btinit);
            this.Controls.Add(this.btReset);
            this.Controls.Add(this.btOK);
            this.Name = "FormConveyorNoLoad";
            this.Text = "辊道";
            this.Load += new System.EventHandler(this.FormConveyor_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.Button btinit;
        private System.Windows.Forms.Button btReset;
        private System.Windows.Forms.Button btAuto;
        private System.Windows.Forms.Button btmanul;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label lbLoadStatus;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button BtnControl;
        private System.Windows.Forms.ComboBox CmbControl;
    }
}