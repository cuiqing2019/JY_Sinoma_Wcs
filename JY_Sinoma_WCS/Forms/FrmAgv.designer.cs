namespace JY_Sinoma_WCS
{
    partial class FrmAGV
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FrmAGV));
            this.btSetWorkMode = new System.Windows.Forms.Button();
            this.btClose = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.panel3 = new System.Windows.Forms.Panel();
            this.lbSKStatus = new System.Windows.Forms.Label();
            this.lvTask = new System.Windows.Forms.ListView();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.CbRefreshTaskView = new System.Windows.Forms.CheckBox();
            this.workStatusButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.locationAdminister = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.freeUnit = new System.Windows.Forms.CheckBox();
            this.waiting2 = new System.Windows.Forms.CheckBox();
            this.waiting1 = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.toCeng = new System.Windows.Forms.ComboBox();
            this.ceng = new System.Windows.Forms.ComboBox();
            this.toPai = new System.Windows.Forms.ComboBox();
            this.pai = new System.Windows.Forms.ComboBox();
            this.toLie = new System.Windows.Forms.ComboBox();
            this.lie = new System.Windows.Forms.ComboBox();
            this.pumpingStatus1 = new System.Windows.Forms.Label();
            this.pumpingStatus2 = new System.Windows.Forms.Label();
            this.pumpingStatusMessage1 = new System.Windows.Forms.Label();
            this.pumpingStatusMessage2 = new System.Windows.Forms.Label();
            this.updatePumpingStatus = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.updateWaitingBoxCode = new System.Windows.Forms.Button();
            this.boxcodeText1 = new System.Windows.Forms.TextBox();
            this.boxcodeText2 = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.panel3.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // btSetWorkMode
            // 
            this.btSetWorkMode.Location = new System.Drawing.Point(541, 360);
            this.btSetWorkMode.Margin = new System.Windows.Forms.Padding(4);
            this.btSetWorkMode.Name = "btSetWorkMode";
            this.btSetWorkMode.Size = new System.Drawing.Size(88, 31);
            this.btSetWorkMode.TabIndex = 4;
            this.btSetWorkMode.Text = "手工任务";
            this.btSetWorkMode.UseVisualStyleBackColor = true;
            this.btSetWorkMode.Click += new System.EventHandler(this.btSetWorkMode_Click);
            // 
            // btClose
            // 
            this.btClose.Location = new System.Drawing.Point(790, 361);
            this.btClose.Margin = new System.Windows.Forms.Padding(4);
            this.btClose.Name = "btClose";
            this.btClose.Size = new System.Drawing.Size(88, 31);
            this.btClose.TabIndex = 7;
            this.btClose.Text = "关闭";
            this.btClose.UseVisualStyleBackColor = true;
            this.btClose.Click += new System.EventHandler(this.btClose_Click);
            // 
            // timer1
            // 
            this.timer1.Interval = 500;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel3.Controls.Add(this.lbSKStatus);
            this.panel3.Location = new System.Drawing.Point(6, 4);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(894, 34);
            this.panel3.TabIndex = 49;
            // 
            // lbSKStatus
            // 
            this.lbSKStatus.AutoSize = true;
            this.lbSKStatus.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lbSKStatus.Location = new System.Drawing.Point(13, 9);
            this.lbSKStatus.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lbSKStatus.Name = "lbSKStatus";
            this.lbSKStatus.Size = new System.Drawing.Size(70, 14);
            this.lbSKStatus.TabIndex = 24;
            this.lbSKStatus.Text = "AGV状态：";
            // 
            // lvTask
            // 
            this.lvTask.FullRowSelect = true;
            this.lvTask.GridLines = true;
            this.lvTask.Location = new System.Drawing.Point(6, 45);
            this.lvTask.MultiSelect = false;
            this.lvTask.Name = "lvTask";
            this.lvTask.Size = new System.Drawing.Size(894, 96);
            this.lvTask.SmallImageList = this.imageList1;
            this.lvTask.TabIndex = 51;
            this.lvTask.UseCompatibleStateImageBehavior = false;
            this.lvTask.View = System.Windows.Forms.View.Details;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(1, 18);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // CbRefreshTaskView
            // 
            this.CbRefreshTaskView.AutoSize = true;
            this.CbRefreshTaskView.Location = new System.Drawing.Point(12, 371);
            this.CbRefreshTaskView.Name = "CbRefreshTaskView";
            this.CbRefreshTaskView.Size = new System.Drawing.Size(155, 20);
            this.CbRefreshTaskView.TabIndex = 52;
            this.CbRefreshTaskView.Text = "自动刷新任务列表";
            this.CbRefreshTaskView.UseVisualStyleBackColor = true;
            // 
            // workStatusButton
            // 
            this.workStatusButton.Location = new System.Drawing.Point(489, 321);
            this.workStatusButton.Margin = new System.Windows.Forms.Padding(4);
            this.workStatusButton.Name = "workStatusButton";
            this.workStatusButton.Size = new System.Drawing.Size(164, 31);
            this.workStatusButton.TabIndex = 53;
            this.workStatusButton.Text = "自动状态";
            this.workStatusButton.UseVisualStyleBackColor = true;
            this.workStatusButton.Click += new System.EventHandler(this.workStatusButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.locationAdminister);
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.toCeng);
            this.groupBox1.Controls.Add(this.ceng);
            this.groupBox1.Controls.Add(this.toPai);
            this.groupBox1.Controls.Add(this.pai);
            this.groupBox1.Controls.Add(this.toLie);
            this.groupBox1.Controls.Add(this.lie);
            this.groupBox1.Location = new System.Drawing.Point(6, 147);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(409, 205);
            this.groupBox1.TabIndex = 54;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "手动出缓存区任务生成";
            // 
            // locationAdminister
            // 
            this.locationAdminister.Location = new System.Drawing.Point(230, 167);
            this.locationAdminister.Margin = new System.Windows.Forms.Padding(4);
            this.locationAdminister.Name = "locationAdminister";
            this.locationAdminister.Size = new System.Drawing.Size(164, 31);
            this.locationAdminister.TabIndex = 67;
            this.locationAdminister.Text = "缓存区库位管理";
            this.locationAdminister.UseVisualStyleBackColor = true;
            this.locationAdminister.Click += new System.EventHandler(this.locationAdminister_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(370, 142);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(24, 16);
            this.label7.TabIndex = 66;
            this.label7.Text = "层";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.freeUnit);
            this.groupBox2.Controls.Add(this.waiting2);
            this.groupBox2.Controls.Add(this.waiting1);
            this.groupBox2.Location = new System.Drawing.Point(118, 66);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(276, 67);
            this.groupBox2.TabIndex = 63;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "等待区";
            // 
            // freeUnit
            // 
            this.freeUnit.AutoSize = true;
            this.freeUnit.Location = new System.Drawing.Point(85, 41);
            this.freeUnit.Name = "freeUnit";
            this.freeUnit.Size = new System.Drawing.Size(107, 20);
            this.freeUnit.TabIndex = 67;
            this.freeUnit.Text = "自定义库位";
            this.freeUnit.UseVisualStyleBackColor = true;
            this.freeUnit.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // waiting2
            // 
            this.waiting2.AutoSize = true;
            this.waiting2.Enabled = false;
            this.waiting2.Location = new System.Drawing.Point(159, 20);
            this.waiting2.Name = "waiting2";
            this.waiting2.Size = new System.Drawing.Size(107, 20);
            this.waiting2.TabIndex = 1;
            this.waiting2.Text = "等待处1004";
            this.waiting2.UseVisualStyleBackColor = true;
            this.waiting2.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // waiting1
            // 
            this.waiting1.AutoSize = true;
            this.waiting1.Enabled = false;
            this.waiting1.Location = new System.Drawing.Point(46, 20);
            this.waiting1.Name = "waiting1";
            this.waiting1.Size = new System.Drawing.Size(107, 20);
            this.waiting1.TabIndex = 0;
            this.waiting1.Text = "等待处1003";
            this.waiting1.UseVisualStyleBackColor = true;
            this.waiting1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(40, 90);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(72, 16);
            this.label5.TabIndex = 62;
            this.label5.Text = "目的地址";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(285, 142);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(24, 16);
            this.label9.TabIndex = 65;
            this.label9.Text = "列";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(40, 28);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 16);
            this.label4.TabIndex = 61;
            this.label4.Text = "起始地址";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(370, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(24, 16);
            this.label3.TabIndex = 60;
            this.label3.Text = "层";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(200, 142);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(24, 16);
            this.label10.TabIndex = 64;
            this.label10.Text = "排";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(285, 28);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(24, 16);
            this.label2.TabIndex = 59;
            this.label2.Text = "列";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(200, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(24, 16);
            this.label1.TabIndex = 58;
            this.label1.Text = "排";
            // 
            // toCeng
            // 
            this.toCeng.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.toCeng.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.toCeng.Enabled = false;
            this.toCeng.FormattingEnabled = true;
            this.toCeng.Items.AddRange(new object[] {
            "1",
            "2"});
            this.toCeng.Location = new System.Drawing.Point(315, 139);
            this.toCeng.Name = "toCeng";
            this.toCeng.Size = new System.Drawing.Size(49, 24);
            this.toCeng.TabIndex = 63;
            // 
            // ceng
            // 
            this.ceng.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.ceng.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.ceng.Enabled = false;
            this.ceng.FormattingEnabled = true;
            this.ceng.Items.AddRange(new object[] {
            "1",
            "2"});
            this.ceng.Location = new System.Drawing.Point(315, 25);
            this.ceng.Name = "ceng";
            this.ceng.Size = new System.Drawing.Size(49, 24);
            this.ceng.TabIndex = 2;
            // 
            // toPai
            // 
            this.toPai.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.toPai.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.toPai.Enabled = false;
            this.toPai.FormattingEnabled = true;
            this.toPai.Items.AddRange(new object[] {
            "1"});
            this.toPai.Location = new System.Drawing.Point(145, 139);
            this.toPai.Name = "toPai";
            this.toPai.Size = new System.Drawing.Size(49, 24);
            this.toPai.TabIndex = 62;
            // 
            // pai
            // 
            this.pai.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.pai.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.pai.Enabled = false;
            this.pai.FormattingEnabled = true;
            this.pai.Items.AddRange(new object[] {
            "1"});
            this.pai.Location = new System.Drawing.Point(145, 25);
            this.pai.Name = "pai";
            this.pai.Size = new System.Drawing.Size(49, 24);
            this.pai.TabIndex = 1;
            // 
            // toLie
            // 
            this.toLie.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.toLie.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.toLie.Enabled = false;
            this.toLie.FormattingEnabled = true;
            this.toLie.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24"});
            this.toLie.Location = new System.Drawing.Point(230, 139);
            this.toLie.Name = "toLie";
            this.toLie.Size = new System.Drawing.Size(49, 24);
            this.toLie.TabIndex = 61;
            // 
            // lie
            // 
            this.lie.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.lie.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.lie.Enabled = false;
            this.lie.FormattingEnabled = true;
            this.lie.Items.AddRange(new object[] {
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12",
            "13",
            "14",
            "15",
            "16",
            "17",
            "18",
            "19",
            "20",
            "21",
            "22",
            "23",
            "24"});
            this.lie.Location = new System.Drawing.Point(230, 25);
            this.lie.Name = "lie";
            this.lie.Size = new System.Drawing.Size(49, 24);
            this.lie.TabIndex = 0;
            // 
            // pumpingStatus1
            // 
            this.pumpingStatus1.AutoSize = true;
            this.pumpingStatus1.Location = new System.Drawing.Point(471, 172);
            this.pumpingStatus1.Name = "pumpingStatus1";
            this.pumpingStatus1.Size = new System.Drawing.Size(136, 16);
            this.pumpingStatus1.TabIndex = 0;
            this.pumpingStatus1.Text = "抽液处1001状态：";
            // 
            // pumpingStatus2
            // 
            this.pumpingStatus2.AutoSize = true;
            this.pumpingStatus2.Location = new System.Drawing.Point(471, 249);
            this.pumpingStatus2.Name = "pumpingStatus2";
            this.pumpingStatus2.Size = new System.Drawing.Size(136, 16);
            this.pumpingStatus2.TabIndex = 55;
            this.pumpingStatus2.Text = "抽液处1002状态：";
            // 
            // pumpingStatusMessage1
            // 
            this.pumpingStatusMessage1.AutoSize = true;
            this.pumpingStatusMessage1.Location = new System.Drawing.Point(597, 201);
            this.pumpingStatusMessage1.Name = "pumpingStatusMessage1";
            this.pumpingStatusMessage1.Size = new System.Drawing.Size(56, 16);
            this.pumpingStatusMessage1.TabIndex = 56;
            this.pumpingStatusMessage1.Text = "未接收";
            // 
            // pumpingStatusMessage2
            // 
            this.pumpingStatusMessage2.AutoSize = true;
            this.pumpingStatusMessage2.Location = new System.Drawing.Point(597, 278);
            this.pumpingStatusMessage2.Name = "pumpingStatusMessage2";
            this.pumpingStatusMessage2.Size = new System.Drawing.Size(56, 16);
            this.pumpingStatusMessage2.TabIndex = 57;
            this.pumpingStatusMessage2.Text = "未接收";
            // 
            // updatePumpingStatus
            // 
            this.updatePumpingStatus.AutoSize = true;
            this.updatePumpingStatus.Location = new System.Drawing.Point(244, 371);
            this.updatePumpingStatus.Name = "updatePumpingStatus";
            this.updatePumpingStatus.Size = new System.Drawing.Size(171, 20);
            this.updatePumpingStatus.TabIndex = 58;
            this.updatePumpingStatus.Text = "自动刷新抽液处状态";
            this.updatePumpingStatus.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(637, 361);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(145, 31);
            this.button1.TabIndex = 59;
            this.button1.Text = "下任务故障诊断";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // updateWaitingBoxCode
            // 
            this.updateWaitingBoxCode.Location = new System.Drawing.Point(661, 321);
            this.updateWaitingBoxCode.Margin = new System.Windows.Forms.Padding(4);
            this.updateWaitingBoxCode.Name = "updateWaitingBoxCode";
            this.updateWaitingBoxCode.Size = new System.Drawing.Size(167, 31);
            this.updateWaitingBoxCode.TabIndex = 62;
            this.updateWaitingBoxCode.Text = "修改等待区箱号";
            this.updateWaitingBoxCode.UseVisualStyleBackColor = true;
            this.updateWaitingBoxCode.Click += new System.EventHandler(this.button2_Click);
            // 
            // boxcodeText1
            // 
            this.boxcodeText1.Location = new System.Drawing.Point(651, 169);
            this.boxcodeText1.Name = "boxcodeText1";
            this.boxcodeText1.Size = new System.Drawing.Size(134, 26);
            this.boxcodeText1.TabIndex = 63;
            this.boxcodeText1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.boxcodeText1_KeyPress);
            // 
            // boxcodeText2
            // 
            this.boxcodeText2.Location = new System.Drawing.Point(651, 246);
            this.boxcodeText2.Name = "boxcodeText2";
            this.boxcodeText2.Size = new System.Drawing.Size(134, 26);
            this.boxcodeText2.TabIndex = 64;
            this.boxcodeText2.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.boxcodeText2_KeyPress);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(597, 172);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(48, 16);
            this.label6.TabIndex = 64;
            this.label6.Text = "箱号:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(597, 249);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(48, 16);
            this.label8.TabIndex = 66;
            this.label8.Text = "箱号:";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(433, 360);
            this.button2.Margin = new System.Windows.Forms.Padding(4);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(88, 31);
            this.button2.TabIndex = 67;
            this.button2.Text = "手工任务";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click_1);
            // 
            // FrmAGV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(907, 405);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.boxcodeText2);
            this.Controls.Add(this.boxcodeText1);
            this.Controls.Add(this.updateWaitingBoxCode);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.updatePumpingStatus);
            this.Controls.Add(this.pumpingStatusMessage2);
            this.Controls.Add(this.pumpingStatusMessage1);
            this.Controls.Add(this.pumpingStatus2);
            this.Controls.Add(this.pumpingStatus1);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.workStatusButton);
            this.Controls.Add(this.CbRefreshTaskView);
            this.Controls.Add(this.lvTask);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.btClose);
            this.Controls.Add(this.btSetWorkMode);
            this.Font = new System.Drawing.Font("宋体", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FrmAGV";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AGV状态";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.FrmStacker_FormClosing);
            this.Load += new System.EventHandler(this.FrmStacker_Load);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btSetWorkMode;
        private System.Windows.Forms.Button btClose;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lbSKStatus;
        private System.Windows.Forms.ListView lvTask;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.CheckBox CbRefreshTaskView;
        private System.Windows.Forms.Button workStatusButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label pumpingStatus1;
        private System.Windows.Forms.Label pumpingStatus2;
        private System.Windows.Forms.Label pumpingStatusMessage1;
        private System.Windows.Forms.Label pumpingStatusMessage2;
        private System.Windows.Forms.ComboBox ceng;
        private System.Windows.Forms.ComboBox pai;
        private System.Windows.Forms.ComboBox lie;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox waiting2;
        private System.Windows.Forms.CheckBox waiting1;
        private System.Windows.Forms.CheckBox updatePumpingStatus;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button updateWaitingBoxCode;
        private System.Windows.Forms.TextBox boxcodeText1;
        private System.Windows.Forms.TextBox boxcodeText2;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button locationAdminister;
        private System.Windows.Forms.CheckBox freeUnit;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox toCeng;
        private System.Windows.Forms.ComboBox toPai;
        private System.Windows.Forms.ComboBox toLie;
        private System.Windows.Forms.Button button2;
    }
}