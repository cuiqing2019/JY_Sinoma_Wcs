namespace JY_Sinoma_WCS
{
    partial class FormChangeGoods
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
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.修改当前执行任务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.出入库任务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.空托盘入库任务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.异常回库任务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退库任务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.结束任务ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textJoinCount = new System.Windows.Forms.TextBox();
            this.dtpStart = new System.Windows.Forms.DateTimePicker();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.tbHazardArea = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.txtUninboundNum = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.tbWasteName = new System.Windows.Forms.TextBox();
            this.btnStop = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.tbEnterWeight = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbWillWeight = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tbShipmentdetalId = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbWasteKinds = new System.Windows.Forms.ComboBox();
            this.tbWasteCode = new System.Windows.Forms.TextBox();
            this.tbVehicleorderNo = new System.Windows.Forms.TextBox();
            this.lbAllWeight = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.inboundStop = new System.Windows.Forms.Button();
            this.inboundStart = new System.Windows.Forms.Button();
            this.goodsPatchList = new JY_Sinoma_WCS.DoubleBufferListView();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.修改当前执行任务ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1297, 25);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // 修改当前执行任务ToolStripMenuItem
            // 
            this.修改当前执行任务ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.出入库任务ToolStripMenuItem,
            this.空托盘入库任务ToolStripMenuItem,
            this.异常回库任务ToolStripMenuItem,
            this.退库任务ToolStripMenuItem,
            this.结束任务ToolStripMenuItem});
            this.修改当前执行任务ToolStripMenuItem.Name = "修改当前执行任务ToolStripMenuItem";
            this.修改当前执行任务ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.修改当前执行任务ToolStripMenuItem.Text = "修改任务模式";
            // 
            // 出入库任务ToolStripMenuItem
            // 
            this.出入库任务ToolStripMenuItem.Name = "出入库任务ToolStripMenuItem";
            this.出入库任务ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.出入库任务ToolStripMenuItem.Tag = "1";
            this.出入库任务ToolStripMenuItem.Text = "出入库任务";
            this.出入库任务ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // 空托盘入库任务ToolStripMenuItem
            // 
            this.空托盘入库任务ToolStripMenuItem.Name = "空托盘入库任务ToolStripMenuItem";
            this.空托盘入库任务ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.空托盘入库任务ToolStripMenuItem.Tag = "2";
            this.空托盘入库任务ToolStripMenuItem.Text = "空托盘入库任务";
            this.空托盘入库任务ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // 异常回库任务ToolStripMenuItem
            // 
            this.异常回库任务ToolStripMenuItem.Name = "异常回库任务ToolStripMenuItem";
            this.异常回库任务ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.异常回库任务ToolStripMenuItem.Tag = "4";
            this.异常回库任务ToolStripMenuItem.Text = "异常回库任务";
            this.异常回库任务ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // 退库任务ToolStripMenuItem
            // 
            this.退库任务ToolStripMenuItem.Name = "退库任务ToolStripMenuItem";
            this.退库任务ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.退库任务ToolStripMenuItem.Tag = "3";
            this.退库任务ToolStripMenuItem.Text = "退库任务";
            this.退库任务ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // 结束任务ToolStripMenuItem
            // 
            this.结束任务ToolStripMenuItem.Name = "结束任务ToolStripMenuItem";
            this.结束任务ToolStripMenuItem.Size = new System.Drawing.Size(160, 22);
            this.结束任务ToolStripMenuItem.Tag = "0";
            this.结束任务ToolStripMenuItem.Text = "结束任务";
            this.结束任务ToolStripMenuItem.Click += new System.EventHandler(this.ToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label12);
            this.groupBox1.Controls.Add(this.textJoinCount);
            this.groupBox1.Controls.Add(this.dtpStart);
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.tbHazardArea);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.txtUninboundNum);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.tbWasteName);
            this.groupBox1.Controls.Add(this.btnStop);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tbEnterWeight);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbWillWeight);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.tbShipmentdetalId);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbWasteKinds);
            this.groupBox1.Controls.Add(this.tbWasteCode);
            this.groupBox1.Controls.Add(this.tbVehicleorderNo);
            this.groupBox1.Controls.Add(this.lbAllWeight);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.inboundStop);
            this.groupBox1.Controls.Add(this.inboundStart);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(1038, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 524);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(8, 353);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(113, 12);
            this.label12.TabIndex = 52;
            this.label12.Text = "已入库入库托盘数量";
            // 
            // textJoinCount
            // 
            this.textJoinCount.Location = new System.Drawing.Point(136, 344);
            this.textJoinCount.Name = "textJoinCount";
            this.textJoinCount.Size = new System.Drawing.Size(111, 21);
            this.textJoinCount.TabIndex = 51;
            // 
            // dtpStart
            // 
            this.dtpStart.CustomFormat = "yyyy-MM-dd HH:mm:ss";
            this.dtpStart.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtpStart.Location = new System.Drawing.Point(90, 184);
            this.dtpStart.Name = "dtpStart";
            this.dtpStart.ShowUpDown = true;
            this.dtpStart.Size = new System.Drawing.Size(143, 21);
            this.dtpStart.TabIndex = 50;
            this.dtpStart.Value = new System.DateTime(2021, 1, 18, 0, 0, 0, 0);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(43, 193);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(29, 12);
            this.label11.TabIndex = 49;
            this.label11.Text = "时间";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(31, 271);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(53, 12);
            this.label10.TabIndex = 47;
            this.label10.Text = "危废代码";
            // 
            // tbHazardArea
            // 
            this.tbHazardArea.Location = new System.Drawing.Point(90, 262);
            this.tbHazardArea.Name = "tbHazardArea";
            this.tbHazardArea.Size = new System.Drawing.Size(139, 21);
            this.tbHazardArea.TabIndex = 46;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(8, 386);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(89, 12);
            this.label9.TabIndex = 45;
            this.label9.Text = "未入库托盘数量";
            // 
            // txtUninboundNum
            // 
            this.txtUninboundNum.Location = new System.Drawing.Point(136, 377);
            this.txtUninboundNum.Name = "txtUninboundNum";
            this.txtUninboundNum.Size = new System.Drawing.Size(111, 21);
            this.txtUninboundNum.TabIndex = 44;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(43, 128);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(29, 12);
            this.label8.TabIndex = 43;
            this.label8.Text = "颜色";
            // 
            // tbWasteName
            // 
            this.tbWasteName.Location = new System.Drawing.Point(90, 119);
            this.tbWasteName.Name = "tbWasteName";
            this.tbWasteName.Size = new System.Drawing.Size(139, 21);
            this.tbWasteName.TabIndex = 42;
            // 
            // btnStop
            // 
            this.btnStop.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnStop.Location = new System.Drawing.Point(10, 472);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(243, 40);
            this.btnStop.TabIndex = 41;
            this.btnStop.Text = "停止生成任务";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(135, 226);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 40;
            this.label7.Text = "已入重量";
            // 
            // tbEnterWeight
            // 
            this.tbEnterWeight.Location = new System.Drawing.Point(194, 223);
            this.tbEnterWeight.Name = "tbEnterWeight";
            this.tbEnterWeight.Size = new System.Drawing.Size(43, 21);
            this.tbEnterWeight.TabIndex = 39;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(31, 232);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 38;
            this.label5.Text = "预计总重";
            // 
            // tbWillWeight
            // 
            this.tbWillWeight.Location = new System.Drawing.Point(90, 223);
            this.tbWillWeight.Name = "tbWillWeight";
            this.tbWillWeight.Size = new System.Drawing.Size(43, 21);
            this.tbWillWeight.TabIndex = 37;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(31, 62);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(53, 12);
            this.label6.TabIndex = 36;
            this.label6.Text = "危废名称";
            // 
            // tbShipmentdetalId
            // 
            this.tbShipmentdetalId.Location = new System.Drawing.Point(90, 53);
            this.tbShipmentdetalId.Name = "tbShipmentdetalId";
            this.tbShipmentdetalId.Size = new System.Drawing.Size(139, 21);
            this.tbShipmentdetalId.TabIndex = 35;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(31, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 34;
            this.label4.Text = "废物包装";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(31, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(53, 12);
            this.label3.TabIndex = 33;
            this.label3.Text = "废物性状";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(31, 29);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 32;
            this.label2.Text = "产废单位";
            // 
            // cbWasteKinds
            // 
            this.cbWasteKinds.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cbWasteKinds.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cbWasteKinds.Cursor = System.Windows.Forms.Cursors.Default;
            this.cbWasteKinds.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbWasteKinds.FormattingEnabled = true;
            this.cbWasteKinds.Items.AddRange(new object[] {
            "吨桶",
            "圆桶",
            "空托盘",
            "铁桶"});
            this.cbWasteKinds.Location = new System.Drawing.Point(90, 152);
            this.cbWasteKinds.Name = "cbWasteKinds";
            this.cbWasteKinds.Size = new System.Drawing.Size(139, 20);
            this.cbWasteKinds.TabIndex = 31;
            // 
            // tbWasteCode
            // 
            this.tbWasteCode.Location = new System.Drawing.Point(90, 86);
            this.tbWasteCode.Name = "tbWasteCode";
            this.tbWasteCode.Size = new System.Drawing.Size(139, 21);
            this.tbWasteCode.TabIndex = 30;
            // 
            // tbVehicleorderNo
            // 
            this.tbVehicleorderNo.Location = new System.Drawing.Point(90, 20);
            this.tbVehicleorderNo.Name = "tbVehicleorderNo";
            this.tbVehicleorderNo.Size = new System.Drawing.Size(139, 21);
            this.tbVehicleorderNo.TabIndex = 29;
            // 
            // lbAllWeight
            // 
            this.lbAllWeight.AutoSize = true;
            this.lbAllWeight.Font = new System.Drawing.Font("宋体", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbAllWeight.Location = new System.Drawing.Point(90, 323);
            this.lbAllWeight.Name = "lbAllWeight";
            this.lbAllWeight.Size = new System.Drawing.Size(43, 21);
            this.lbAllWeight.TabIndex = 28;
            this.lbAllWeight.Text = "NaN";
            this.lbAllWeight.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(49, 294);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(180, 19);
            this.label1.TabIndex = 27;
            this.label1.Text = "当前入库批次总重量";
            // 
            // inboundStop
            // 
            this.inboundStop.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.inboundStop.Location = new System.Drawing.Point(139, 413);
            this.inboundStop.Name = "inboundStop";
            this.inboundStop.Size = new System.Drawing.Size(114, 40);
            this.inboundStop.TabIndex = 26;
            this.inboundStop.Text = "废料结束入库";
            this.inboundStop.UseVisualStyleBackColor = true;
            this.inboundStop.Click += new System.EventHandler(this.inboundStop_Click);
            // 
            // inboundStart
            // 
            this.inboundStart.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.inboundStart.Location = new System.Drawing.Point(8, 413);
            this.inboundStart.Name = "inboundStart";
            this.inboundStart.Size = new System.Drawing.Size(112, 40);
            this.inboundStart.TabIndex = 25;
            this.inboundStart.Text = "废料开始入库";
            this.inboundStart.UseVisualStyleBackColor = true;
            this.inboundStart.Click += new System.EventHandler(this.inboundStart_Click);
            // 
            // goodsPatchList
            // 
            this.goodsPatchList.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.goodsPatchList.Dock = System.Windows.Forms.DockStyle.Left;
            this.goodsPatchList.FullRowSelect = true;
            this.goodsPatchList.GridLines = true;
            this.goodsPatchList.HoverSelection = true;
            this.goodsPatchList.Location = new System.Drawing.Point(0, 25);
            this.goodsPatchList.Name = "goodsPatchList";
            this.goodsPatchList.Size = new System.Drawing.Size(1029, 524);
            this.goodsPatchList.TabIndex = 0;
            this.goodsPatchList.UseCompatibleStateImageBehavior = false;
            this.goodsPatchList.View = System.Windows.Forms.View.Details;
            this.goodsPatchList.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.goodsPatchList_MouseDoubleClick);
            // 
            // FormChangeGoods
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(1297, 549);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.goodsPatchList);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FormChangeGoods";
            this.Text = "一楼任务模式设置";
            this.Load += new System.EventHandler(this.FormChangeGoods_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 修改当前执行任务ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 出入库任务ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 空托盘入库任务ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 异常回库任务ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退库任务ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 结束任务ToolStripMenuItem;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbWasteName;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbEnterWeight;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbWillWeight;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbShipmentdetalId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbWasteKinds;
        private System.Windows.Forms.TextBox tbWasteCode;
        private System.Windows.Forms.TextBox tbVehicleorderNo;
        private System.Windows.Forms.Label lbAllWeight;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button inboundStop;
        private System.Windows.Forms.Button inboundStart;
        private DoubleBufferListView goodsPatchList;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox txtUninboundNum;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbHazardArea;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.DateTimePicker dtpStart;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox textJoinCount;
    }
}