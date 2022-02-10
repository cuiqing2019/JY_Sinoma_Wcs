namespace JY_Sinoma_WCS
{
    partial class FrmWLocation
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
            this.btRequery = new System.Windows.Forms.Button();
            this.lvLocation = new System.Windows.Forms.ListView();
            this.cmsStatus = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiChangeUnitStatus = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiChangeLocation = new System.Windows.Forms.ToolStripMenuItem();
            this.人工出库ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aGVToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.目的1003ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.等待处1004ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.清空库存ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.所有ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.仅单个ToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.立库ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.出库ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.按批次ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全部ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.仅单个ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.退库ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.按批次ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.相同废料ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.全部ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.仅单个ToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiChangeGoodsKind = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiChangeHazardArea = new System.Windows.Forms.ToolStripMenuItem();
            this.修改废物名称ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tbLocation = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tb_count_storage_location_null = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtpallet = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.btnLocatinView = new System.Windows.Forms.Button();
            this.txtBatchNo = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.cmsStatus.SuspendLayout();
            this.SuspendLayout();
            // 
            // btRequery
            // 
            this.btRequery.Location = new System.Drawing.Point(909, 6);
            this.btRequery.Name = "btRequery";
            this.btRequery.Size = new System.Drawing.Size(75, 23);
            this.btRequery.TabIndex = 7;
            this.btRequery.Text = "查询";
            this.btRequery.UseVisualStyleBackColor = true;
            this.btRequery.Click += new System.EventHandler(this.btRequery_Click);
            // 
            // lvLocation
            // 
            this.lvLocation.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lvLocation.ContextMenuStrip = this.cmsStatus;
            this.lvLocation.FullRowSelect = true;
            this.lvLocation.GridLines = true;
            this.lvLocation.HideSelection = false;
            this.lvLocation.Location = new System.Drawing.Point(3, 81);
            this.lvLocation.Name = "lvLocation";
            this.lvLocation.Size = new System.Drawing.Size(1200, 389);
            this.lvLocation.TabIndex = 6;
            this.lvLocation.UseCompatibleStateImageBehavior = false;
            this.lvLocation.View = System.Windows.Forms.View.Details;
            // 
            // cmsStatus
            // 
            this.cmsStatus.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiChangeUnitStatus,
            this.tsmiChangeLocation,
            this.人工出库ToolStripMenuItem,
            this.tsmiChangeGoodsKind,
            this.tsmiChangeHazardArea,
            this.修改废物名称ToolStripMenuItem});
            this.cmsStatus.Name = "cmsStatus";
            this.cmsStatus.Size = new System.Drawing.Size(149, 136);
            // 
            // tsmiChangeUnitStatus
            // 
            this.tsmiChangeUnitStatus.Name = "tsmiChangeUnitStatus";
            this.tsmiChangeUnitStatus.Size = new System.Drawing.Size(148, 22);
            this.tsmiChangeUnitStatus.Text = "修改库位状态";
            this.tsmiChangeUnitStatus.Click += new System.EventHandler(this.tsmiChangeUnitStatus_Click);
            // 
            // tsmiChangeLocation
            // 
            this.tsmiChangeLocation.Name = "tsmiChangeLocation";
            this.tsmiChangeLocation.Size = new System.Drawing.Size(148, 22);
            this.tsmiChangeLocation.Text = "调整库位";
            this.tsmiChangeLocation.Click += new System.EventHandler(this.tsmiChangeLocation_Click);
            // 
            // 人工出库ToolStripMenuItem
            // 
            this.人工出库ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.aGVToolStripMenuItem,
            this.立库ToolStripMenuItem});
            this.人工出库ToolStripMenuItem.Name = "人工出库ToolStripMenuItem";
            this.人工出库ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.人工出库ToolStripMenuItem.Text = "人工出库";
            // 
            // aGVToolStripMenuItem
            // 
            this.aGVToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.目的1003ToolStripMenuItem,
            this.等待处1004ToolStripMenuItem,
            this.清空库存ToolStripMenuItem});
            this.aGVToolStripMenuItem.Name = "aGVToolStripMenuItem";
            this.aGVToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.aGVToolStripMenuItem.Text = "AGV单任务出库";
            // 
            // 目的1003ToolStripMenuItem
            // 
            this.目的1003ToolStripMenuItem.Name = "目的1003ToolStripMenuItem";
            this.目的1003ToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.目的1003ToolStripMenuItem.Text = "等待处1003";
            this.目的1003ToolStripMenuItem.Click += new System.EventHandler(this.目的1003ToolStripMenuItem_Click);
            // 
            // 等待处1004ToolStripMenuItem
            // 
            this.等待处1004ToolStripMenuItem.Name = "等待处1004ToolStripMenuItem";
            this.等待处1004ToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.等待处1004ToolStripMenuItem.Text = "等待处1004";
            this.等待处1004ToolStripMenuItem.Click += new System.EventHandler(this.等待处1004ToolStripMenuItem_Click);
            // 
            // 清空库存ToolStripMenuItem
            // 
            this.清空库存ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.所有ToolStripMenuItem,
            this.仅单个ToolStripMenuItem2});
            this.清空库存ToolStripMenuItem.Name = "清空库存ToolStripMenuItem";
            this.清空库存ToolStripMenuItem.Size = new System.Drawing.Size(140, 22);
            this.清空库存ToolStripMenuItem.Text = "清空库存";
            // 
            // 所有ToolStripMenuItem
            // 
            this.所有ToolStripMenuItem.Name = "所有ToolStripMenuItem";
            this.所有ToolStripMenuItem.Size = new System.Drawing.Size(112, 22);
            this.所有ToolStripMenuItem.Text = "所有";
            this.所有ToolStripMenuItem.Click += new System.EventHandler(this.所有ToolStripMenuItem_Click);
            // 
            // 仅单个ToolStripMenuItem2
            // 
            this.仅单个ToolStripMenuItem2.Name = "仅单个ToolStripMenuItem2";
            this.仅单个ToolStripMenuItem2.Size = new System.Drawing.Size(112, 22);
            this.仅单个ToolStripMenuItem2.Text = "仅单个";
            this.仅单个ToolStripMenuItem2.Click += new System.EventHandler(this.仅单个ToolStripMenuItem2_Click);
            // 
            // 立库ToolStripMenuItem
            // 
            this.立库ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.出库ToolStripMenuItem,
            this.退库ToolStripMenuItem});
            this.立库ToolStripMenuItem.Name = "立库ToolStripMenuItem";
            this.立库ToolStripMenuItem.Size = new System.Drawing.Size(161, 22);
            this.立库ToolStripMenuItem.Text = "立库";
            // 
            // 出库ToolStripMenuItem
            // 
            this.出库ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.按批次ToolStripMenuItem,
            this.仅单个ToolStripMenuItem});
            this.出库ToolStripMenuItem.Name = "出库ToolStripMenuItem";
            this.出库ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.出库ToolStripMenuItem.Text = "出库";
            // 
            // 按批次ToolStripMenuItem
            // 
            this.按批次ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.全部ToolStripMenuItem});
            this.按批次ToolStripMenuItem.Name = "按批次ToolStripMenuItem";
            this.按批次ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.按批次ToolStripMenuItem.Text = "废料名称";
            // 
            // 全部ToolStripMenuItem
            // 
            this.全部ToolStripMenuItem.Name = "全部ToolStripMenuItem";
            this.全部ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.全部ToolStripMenuItem.Text = "全部";
            this.全部ToolStripMenuItem.Click += new System.EventHandler(this.全部ToolStripMenuItem_Click);
            // 
            // 仅单个ToolStripMenuItem
            // 
            this.仅单个ToolStripMenuItem.Name = "仅单个ToolStripMenuItem";
            this.仅单个ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.仅单个ToolStripMenuItem.Text = "仅单个";
            this.仅单个ToolStripMenuItem.Click += new System.EventHandler(this.仅单个ToolStripMenuItem_Click);
            // 
            // 退库ToolStripMenuItem
            // 
            this.退库ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.按批次ToolStripMenuItem1,
            this.仅单个ToolStripMenuItem1});
            this.退库ToolStripMenuItem.Name = "退库ToolStripMenuItem";
            this.退库ToolStripMenuItem.Size = new System.Drawing.Size(100, 22);
            this.退库ToolStripMenuItem.Text = "退库";
            // 
            // 按批次ToolStripMenuItem1
            // 
            this.按批次ToolStripMenuItem1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.相同废料ToolStripMenuItem,
            this.全部ToolStripMenuItem1});
            this.按批次ToolStripMenuItem1.Name = "按批次ToolStripMenuItem1";
            this.按批次ToolStripMenuItem1.Size = new System.Drawing.Size(112, 22);
            this.按批次ToolStripMenuItem1.Text = "按批次";
            // 
            // 相同废料ToolStripMenuItem
            // 
            this.相同废料ToolStripMenuItem.Name = "相同废料ToolStripMenuItem";
            this.相同废料ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.相同废料ToolStripMenuItem.Text = "相同废料";
            this.相同废料ToolStripMenuItem.Click += new System.EventHandler(this.相同废料ToolStripMenuItem_Click);
            // 
            // 全部ToolStripMenuItem1
            // 
            this.全部ToolStripMenuItem1.Name = "全部ToolStripMenuItem1";
            this.全部ToolStripMenuItem1.Size = new System.Drawing.Size(124, 22);
            this.全部ToolStripMenuItem1.Text = "全部";
            this.全部ToolStripMenuItem1.Click += new System.EventHandler(this.全部ToolStripMenuItem1_Click);
            // 
            // 仅单个ToolStripMenuItem1
            // 
            this.仅单个ToolStripMenuItem1.Name = "仅单个ToolStripMenuItem1";
            this.仅单个ToolStripMenuItem1.Size = new System.Drawing.Size(112, 22);
            this.仅单个ToolStripMenuItem1.Text = "仅单个";
            this.仅单个ToolStripMenuItem1.Click += new System.EventHandler(this.仅单个ToolStripMenuItem1_Click);
            // 
            // tsmiChangeGoodsKind
            // 
            this.tsmiChangeGoodsKind.Name = "tsmiChangeGoodsKind";
            this.tsmiChangeGoodsKind.Size = new System.Drawing.Size(148, 22);
            this.tsmiChangeGoodsKind.Text = "修改库位类型";
            this.tsmiChangeGoodsKind.Click += new System.EventHandler(this.tsmiChangeGoodsKind_Click);
            // 
            // tsmiChangeHazardArea
            // 
            this.tsmiChangeHazardArea.Name = "tsmiChangeHazardArea";
            this.tsmiChangeHazardArea.Size = new System.Drawing.Size(148, 22);
            this.tsmiChangeHazardArea.Text = "修改危险分区";
            this.tsmiChangeHazardArea.Click += new System.EventHandler(this.tsmiChangeHazardArea_Click);
            // 
            // 修改废物名称ToolStripMenuItem
            // 
            this.修改废物名称ToolStripMenuItem.Name = "修改废物名称ToolStripMenuItem";
            this.修改废物名称ToolStripMenuItem.Size = new System.Drawing.Size(148, 22);
            this.修改废物名称ToolStripMenuItem.Text = "修改废物名称";
            this.修改废物名称ToolStripMenuItem.Click += new System.EventHandler(this.修改废物名称ToolStripMenuItem_Click);
            // 
            // tbLocation
            // 
            this.tbLocation.Location = new System.Drawing.Point(75, 10);
            this.tbLocation.Name = "tbLocation";
            this.tbLocation.Size = new System.Drawing.Size(145, 21);
            this.tbLocation.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(10, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 4;
            this.label1.Text = "库位编码：";
            // 
            // tb_count_storage_location_null
            // 
            this.tb_count_storage_location_null.Enabled = false;
            this.tb_count_storage_location_null.Location = new System.Drawing.Point(548, 42);
            this.tb_count_storage_location_null.Name = "tb_count_storage_location_null";
            this.tb_count_storage_location_null.Size = new System.Drawing.Size(167, 21);
            this.tb_count_storage_location_null.TabIndex = 12;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(495, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 8;
            this.label2.Text = "库位数：";
            // 
            // txtpallet
            // 
            this.txtpallet.Location = new System.Drawing.Point(295, 10);
            this.txtpallet.Name = "txtpallet";
            this.txtpallet.Size = new System.Drawing.Size(167, 21);
            this.txtpallet.TabIndex = 14;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(230, 14);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(65, 12);
            this.label3.TabIndex = 13;
            this.label3.Text = "托盘条码：";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.Color.Red;
            this.label4.Location = new System.Drawing.Point(1022, 46);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(173, 12);
            this.label4.TabIndex = 15;
            this.label4.Text = "修改库位状态时请停止工作！！";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(295, 46);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 23;
            this.label8.Text = "label8";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(109, 46);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 22;
            this.label7.Text = "label7";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(230, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(65, 12);
            this.label6.TabIndex = 21;
            this.label6.Text = "已禁库位：";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 46);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(89, 12);
            this.label5.TabIndex = 20;
            this.label5.Text = "剩余可用库位：";
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(874, 44);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(144, 16);
            this.checkBox1.TabIndex = 24;
            this.checkBox1.Text = "仅查看有货和禁用库位";
            this.checkBox1.UseVisualStyleBackColor = true;
            // 
            // btnLocatinView
            // 
            this.btnLocatinView.Location = new System.Drawing.Point(730, 38);
            this.btnLocatinView.Name = "btnLocatinView";
            this.btnLocatinView.Size = new System.Drawing.Size(139, 29);
            this.btnLocatinView.TabIndex = 25;
            this.btnLocatinView.Text = "库位视图";
            this.btnLocatinView.UseVisualStyleBackColor = true;
            this.btnLocatinView.Click += new System.EventHandler(this.button1_Click);
            // 
            // txtBatchNo
            // 
            this.txtBatchNo.Location = new System.Drawing.Point(548, 10);
            this.txtBatchNo.Name = "txtBatchNo";
            this.txtBatchNo.Size = new System.Drawing.Size(167, 21);
            this.txtBatchNo.TabIndex = 31;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(471, 14);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 30;
            this.label9.Text = "废品名称：";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(728, 14);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(65, 12);
            this.label10.TabIndex = 32;
            this.label10.Text = "是否有货：";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "全部",
            "有货",
            "无货"});
            this.comboBox1.Location = new System.Drawing.Point(799, 9);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(79, 20);
            this.comboBox1.TabIndex = 34;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(1058, 14);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 12);
            this.label11.TabIndex = 35;
            this.label11.Text = "总重量：";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(1117, 13);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(11, 12);
            this.label12.TabIndex = 36;
            this.label12.Text = "0";
            // 
            // FrmWLocation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(1204, 472);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.txtBatchNo);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.btnLocatinView);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.txtpallet);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lvLocation);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.tb_count_storage_location_null);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.tbLocation);
            this.Controls.Add(this.btRequery);
            this.Controls.Add(this.label1);
            this.Name = "FrmWLocation";
            this.Text = "库位信息查询";
            this.Load += new System.EventHandler(this.FrmWLocation_Load);
            this.cmsStatus.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btRequery;
        private System.Windows.Forms.ListView lvLocation;
        private System.Windows.Forms.TextBox tbLocation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ContextMenuStrip cmsStatus;
        private System.Windows.Forms.ToolStripMenuItem tsmiChangeUnitStatus;
        private System.Windows.Forms.TextBox tb_count_storage_location_null;
        private System.Windows.Forms.ToolStripMenuItem tsmiChangeLocation;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtpallet;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.ToolStripMenuItem 人工出库ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aGVToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 立库ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 出库ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 按批次ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 仅单个ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 退库ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 按批次ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 相同废料ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 全部ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 仅单个ToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem 目的1003ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 等待处1004ToolStripMenuItem;
        private System.Windows.Forms.Button btnLocatinView;
        private System.Windows.Forms.TextBox txtBatchNo;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ToolStripMenuItem tsmiChangeGoodsKind;
        private System.Windows.Forms.ToolStripMenuItem tsmiChangeHazardArea;
        private System.Windows.Forms.ToolStripMenuItem 清空库存ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 所有ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 仅单个ToolStripMenuItem2;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.ToolStripMenuItem 修改废物名称ToolStripMenuItem;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
    }
}