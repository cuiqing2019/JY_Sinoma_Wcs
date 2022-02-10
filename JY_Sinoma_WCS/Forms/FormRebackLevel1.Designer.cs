namespace JY_Sinoma_WCS
{
    partial class FormRebackLevel1
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.cmbInPort = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.tbHazardArea = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tbBatchNo = new System.Windows.Forms.TextBox();
            this.btnCancelTask = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.tbBatchId = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.cbWasteKinds = new System.Windows.Forms.ComboBox();
            this.tbSku = new System.Windows.Forms.TextBox();
            this.tbBoxBarcode = new System.Windows.Forms.TextBox();
            this.btnGetTask = new System.Windows.Forms.Button();
            this.lvTask = new System.Windows.Forms.ListView();
            this.label1 = new System.Windows.Forms.Label();
            this.tbTaskId = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.tbGoodName = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.tbGoodsWeight = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label7);
            this.groupBox1.Controls.Add(this.tbGoodsWeight);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.tbGoodName);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.tbTaskId);
            this.groupBox1.Controls.Add(this.cmbInPort);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.tbHazardArea);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.label8);
            this.groupBox1.Controls.Add(this.tbBatchNo);
            this.groupBox1.Controls.Add(this.btnCancelTask);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.tbBatchId);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.cbWasteKinds);
            this.groupBox1.Controls.Add(this.tbSku);
            this.groupBox1.Controls.Add(this.tbBoxBarcode);
            this.groupBox1.Controls.Add(this.btnGetTask);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Location = new System.Drawing.Point(1035, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(259, 485);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            // 
            // cmbInPort
            // 
            this.cmbInPort.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.SuggestAppend;
            this.cmbInPort.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.ListItems;
            this.cmbInPort.Cursor = System.Windows.Forms.Cursors.Default;
            this.cmbInPort.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbInPort.FormattingEnabled = true;
            this.cmbInPort.Items.AddRange(new object[] {
            "--选中异常回库口--",
            "1",
            "2",
            "3",
            "4"});
            this.cmbInPort.Location = new System.Drawing.Point(89, 250);
            this.cmbInPort.Name = "cmbInPort";
            this.cmbInPort.Size = new System.Drawing.Size(139, 20);
            this.cmbInPort.TabIndex = 48;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 228);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(77, 12);
            this.label10.TabIndex = 47;
            this.label10.Text = "入库危险分区";
            // 
            // tbHazardArea
            // 
            this.tbHazardArea.Location = new System.Drawing.Point(89, 224);
            this.tbHazardArea.Name = "tbHazardArea";
            this.tbHazardArea.Size = new System.Drawing.Size(139, 21);
            this.tbHazardArea.TabIndex = 46;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(18, 254);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(65, 12);
            this.label9.TabIndex = 45;
            this.label9.Text = "选择入库口";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(30, 125);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(53, 12);
            this.label8.TabIndex = 43;
            this.label8.Text = "废物批次";
            // 
            // tbBatchNo
            // 
            this.tbBatchNo.Location = new System.Drawing.Point(89, 121);
            this.tbBatchNo.Name = "tbBatchNo";
            this.tbBatchNo.Size = new System.Drawing.Size(139, 21);
            this.tbBatchNo.TabIndex = 42;
            // 
            // btnCancelTask
            // 
            this.btnCancelTask.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancelTask.Location = new System.Drawing.Point(10, 391);
            this.btnCancelTask.Name = "btnCancelTask";
            this.btnCancelTask.Size = new System.Drawing.Size(243, 62);
            this.btnCancelTask.TabIndex = 41;
            this.btnCancelTask.Text = "异常回库任务取消";
            this.btnCancelTask.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 73);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(77, 12);
            this.label6.TabIndex = 36;
            this.label6.Text = "接运单明细ID";
            // 
            // tbBatchId
            // 
            this.tbBatchId.Location = new System.Drawing.Point(89, 69);
            this.tbBatchId.Name = "tbBatchId";
            this.tbBatchId.Size = new System.Drawing.Size(139, 21);
            this.tbBatchId.TabIndex = 35;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(30, 151);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(53, 12);
            this.label4.TabIndex = 34;
            this.label4.Text = "废物包装";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 99);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 33;
            this.label3.Text = "废物SKU";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(30, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 32;
            this.label2.Text = "托盘编号";
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
            "圆桶"});
            this.cbWasteKinds.Location = new System.Drawing.Point(89, 147);
            this.cbWasteKinds.Name = "cbWasteKinds";
            this.cbWasteKinds.Size = new System.Drawing.Size(139, 20);
            this.cbWasteKinds.TabIndex = 31;
            // 
            // tbSku
            // 
            this.tbSku.Location = new System.Drawing.Point(89, 95);
            this.tbSku.Name = "tbSku";
            this.tbSku.Size = new System.Drawing.Size(139, 21);
            this.tbSku.TabIndex = 30;
            // 
            // tbBoxBarcode
            // 
            this.tbBoxBarcode.Location = new System.Drawing.Point(89, 43);
            this.tbBoxBarcode.Name = "tbBoxBarcode";
            this.tbBoxBarcode.Size = new System.Drawing.Size(139, 21);
            this.tbBoxBarcode.TabIndex = 29;
            // 
            // btnGetTask
            // 
            this.btnGetTask.Font = new System.Drawing.Font("宋体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnGetTask.Location = new System.Drawing.Point(16, 296);
            this.btnGetTask.Name = "btnGetTask";
            this.btnGetTask.Size = new System.Drawing.Size(237, 62);
            this.btnGetTask.TabIndex = 18;
            this.btnGetTask.Text = "手动生成异常回库任务";
            this.btnGetTask.UseVisualStyleBackColor = true;
            this.btnGetTask.Click += new System.EventHandler(this.btnGetTask_Click);
            // 
            // lvTask
            // 
            this.lvTask.Dock = System.Windows.Forms.DockStyle.Left;
            this.lvTask.FullRowSelect = true;
            this.lvTask.GridLines = true;
            this.lvTask.Location = new System.Drawing.Point(0, 0);
            this.lvTask.Name = "lvTask";
            this.lvTask.Size = new System.Drawing.Size(1029, 485);
            this.lvTask.TabIndex = 48;
            this.lvTask.UseCompatibleStateImageBehavior = false;
            this.lvTask.View = System.Windows.Forms.View.Details;
            this.lvTask.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.lvTask_ItemSelectionChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(42, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 50;
            this.label1.Text = "任务号";
            // 
            // tbTaskId
            // 
            this.tbTaskId.Location = new System.Drawing.Point(89, 17);
            this.tbTaskId.Name = "tbTaskId";
            this.tbTaskId.Size = new System.Drawing.Size(139, 21);
            this.tbTaskId.TabIndex = 49;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(30, 201);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(53, 12);
            this.label5.TabIndex = 52;
            this.label5.Text = "废物名称";
            // 
            // tbGoodName
            // 
            this.tbGoodName.Location = new System.Drawing.Point(89, 197);
            this.tbGoodName.Name = "tbGoodName";
            this.tbGoodName.Size = new System.Drawing.Size(139, 21);
            this.tbGoodName.TabIndex = 51;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(30, 175);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(53, 12);
            this.label7.TabIndex = 54;
            this.label7.Text = "废物重量";
            // 
            // tbGoodsWeight
            // 
            this.tbGoodsWeight.Location = new System.Drawing.Point(89, 171);
            this.tbGoodsWeight.Name = "tbGoodsWeight";
            this.tbGoodsWeight.Size = new System.Drawing.Size(139, 21);
            this.tbGoodsWeight.TabIndex = 53;
            // 
            // FormRebackLevel1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightCyan;
            this.ClientSize = new System.Drawing.Size(1294, 485);
            this.Controls.Add(this.lvTask);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormRebackLevel1";
            this.Text = "一楼异常回库任务生成";
            this.Load += new System.EventHandler(this.FormRebackLevel1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnGetTask;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox tbBatchNo;
        private System.Windows.Forms.Button btnCancelTask;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox tbBatchId;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cbWasteKinds;
        private System.Windows.Forms.TextBox tbSku;
        private System.Windows.Forms.TextBox tbBoxBarcode;
        private DoubleBufferListView goodsPatchList;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox tbHazardArea;
        private System.Windows.Forms.ListView lvTask;
        private System.Windows.Forms.ComboBox cmbInPort;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox tbTaskId;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox tbGoodName;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox tbGoodsWeight;
    }
}