namespace JY_Sinoma_WCS
{
    partial class FormSubTask
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormSubTask));
            this.lvTask = new System.Windows.Forms.ListView();
            this.cmsOperate = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiChange = new System.Windows.Forms.ToolStripMenuItem();
            this.btOK = new System.Windows.Forms.Button();
            this.cmsOperate.SuspendLayout();
            this.SuspendLayout();
            // 
            // lvTask
            // 
            this.lvTask.ContextMenuStrip = this.cmsOperate;
            this.lvTask.FullRowSelect = true;
            this.lvTask.GridLines = true;
            this.lvTask.Location = new System.Drawing.Point(12, 0);
            this.lvTask.Name = "lvTask";
            this.lvTask.Size = new System.Drawing.Size(757, 213);
            this.lvTask.TabIndex = 0;
            this.lvTask.UseCompatibleStateImageBehavior = false;
            this.lvTask.View = System.Windows.Forms.View.Details;
            // 
            // cmsOperate
            // 
            this.cmsOperate.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiChange});
            this.cmsOperate.Name = "contextMenuStrip1";
            this.cmsOperate.Size = new System.Drawing.Size(137, 26);
            // 
            // tsmiChange
            // 
            this.tsmiChange.Name = "tsmiChange";
            this.tsmiChange.Size = new System.Drawing.Size(136, 22);
            this.tsmiChange.Text = "修改子任务";
            this.tsmiChange.Click += new System.EventHandler(this.tsmiChange_Click);
            // 
            // btOK
            // 
            this.btOK.Location = new System.Drawing.Point(351, 223);
            this.btOK.Name = "btOK";
            this.btOK.Size = new System.Drawing.Size(75, 23);
            this.btOK.TabIndex = 1;
            this.btOK.Text = "确定";
            this.btOK.UseVisualStyleBackColor = true;
            this.btOK.Click += new System.EventHandler(this.btOK_Click);
            // 
            // FormSubTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(781, 256);
            this.Controls.Add(this.btOK);
            this.Controls.Add(this.lvTask);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormSubTask";
            this.Text = "子任务显示";
            this.Load += new System.EventHandler(this.FrmSubTask_Load);
            this.cmsOperate.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListView lvTask;
        private System.Windows.Forms.Button btOK;
        private System.Windows.Forms.ContextMenuStrip cmsOperate;
        private System.Windows.Forms.ToolStripMenuItem tsmiChange;
    }
}