namespace JY_Sinoma_WCS.Forms
{
    partial class FormPortSet
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
            this.components = new System.ComponentModel.Container();
            this.lvPort = new System.Windows.Forms.ListView();
            this.cmsStatus = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiChangeUnitStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiChangeTaskType = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvPort
            // 
            this.lvPort.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lvPort.FullRowSelect = true;
            this.lvPort.GridLines = true;
            this.lvPort.Location = new System.Drawing.Point(0, 0);
            this.lvPort.Name = "lvPort";
            this.lvPort.Size = new System.Drawing.Size(677, 266);
            this.lvPort.TabIndex = 7;
            this.lvPort.UseCompatibleStateImageBehavior = false;
            this.lvPort.View = System.Windows.Forms.View.Details;
            // 
            // cmsStatus
            // 
            this.cmsStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiChangeUnitStatus,
            this.tsmiChangeTaskType});
            this.cmsStatus.Name = "cmsStatus";
            this.cmsStatus.Size = new System.Drawing.Size(153, 70);
            // 
            // tsmiChangeUnitStatus
            // 
            this.tsmiChangeUnitStatus.Name = "tsmiChangeUnitStatus";
            this.tsmiChangeUnitStatus.Size = new System.Drawing.Size(152, 22);
            this.tsmiChangeUnitStatus.Text = "可用性修改";
            this.tsmiChangeUnitStatus.Click += new System.EventHandler(this.tsmiChangeUnitStatus_Click);
            // 
            // tsmiChangeTaskType
            // 
            this.tsmiChangeTaskType.Name = "tsmiChangeTaskType";
            this.tsmiChangeTaskType.Size = new System.Drawing.Size(152, 22);
            this.tsmiChangeTaskType.Text = "任务类型修改";
            this.tsmiChangeTaskType.Click += new System.EventHandler(this.tsmiChangeTaskType_Click);
            // 
            // FormPortSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(677, 266);
            this.Controls.Add(this.lvPort);
            this.Name = "FormPortSet";
            this.Text = "出入库口设置";
            this.Load += new System.EventHandler(this.FormPortSet_Load);
            this.cmsStatus.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvPort;
        private System.Windows.Forms.ContextMenuStrip cmsStatus;
        private System.Windows.Forms.ToolStripMenuItem tsmiChangeUnitStatus;
        private System.Windows.Forms.ToolStripMenuItem tsmiChangeTaskType;
    }
}