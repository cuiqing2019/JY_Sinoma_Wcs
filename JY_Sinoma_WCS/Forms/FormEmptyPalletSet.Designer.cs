namespace JY_Sinoma_WCS.Forms
{
    partial class FormEmptyPalletSet
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
            this.lbCurrentType = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnAuto = new System.Windows.Forms.Button();
            this.btnManual = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lbCurrentType
            // 
            this.lbCurrentType.AutoSize = true;
            this.lbCurrentType.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbCurrentType.Location = new System.Drawing.Point(75, 42);
            this.lbCurrentType.Name = "lbCurrentType";
            this.lbCurrentType.Size = new System.Drawing.Size(88, 16);
            this.lbCurrentType.TabIndex = 0;
            this.lbCurrentType.Text = "当前模式：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(137, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(0, 12);
            this.label2.TabIndex = 1;
            // 
            // btnAuto
            // 
            this.btnAuto.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnAuto.Location = new System.Drawing.Point(33, 94);
            this.btnAuto.Name = "btnAuto";
            this.btnAuto.Size = new System.Drawing.Size(155, 50);
            this.btnAuto.TabIndex = 2;
            this.btnAuto.Text = "自动回库模式";
            this.btnAuto.UseVisualStyleBackColor = true;
            this.btnAuto.Click += new System.EventHandler(this.btnAuto_Click);
            // 
            // btnManual
            // 
            this.btnManual.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnManual.Location = new System.Drawing.Point(213, 94);
            this.btnManual.Name = "btnManual";
            this.btnManual.Size = new System.Drawing.Size(155, 50);
            this.btnManual.TabIndex = 3;
            this.btnManual.Text = "人工回库模式";
            this.btnManual.UseVisualStyleBackColor = true;
            this.btnManual.Click += new System.EventHandler(this.btnManual_Click);
            // 
            // label1
            // 
            this.label1.ForeColor = System.Drawing.Color.Red;
            this.label1.Location = new System.Drawing.Point(9, 162);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(381, 90);
            this.label1.TabIndex = 4;
            this.label1.Text = "注：自动回库模式：一次出8个托盘或小于8个托盘，出完之后自动转换\r\n\r\n系统工作模式为空托入库，待空托入库完成，切换出库模式；\r\n\r\n人工回库模式：托盘出库后，人" +
    "工叉车取下，等待出库完成后，母托\r\n\r\n盘垛由人工叉车放回输送回库。";
            // 
            // FormEmptyPalletSet
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(393, 261);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnManual);
            this.Controls.Add(this.btnAuto);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbCurrentType);
            this.Name = "FormEmptyPalletSet";
            this.Text = "母托盘回库模式设置";
            this.Load += new System.EventHandler(this.FormEmptyPalletSet_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lbCurrentType;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnAuto;
        private System.Windows.Forms.Button btnManual;
        private System.Windows.Forms.Label label1;
    }
}