using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DataBase;
using Sinoma_WCS;
using MySql.Data.MySqlClient;

namespace JY_Sinoma_WCS
{
    public partial class FormChangeGoods : Form
    {
        public ConnectPool dbConn;//定义数据库连接
        frmMain mainFrm;
        bool auto = true;
        double batchWillWeight;
        double batchWeight;
        public int nLevel;

        public FormChangeGoods(frmMain mainFrm, int nLevel)
        {
            this.mainFrm = mainFrm;
            dbConn = mainFrm.dbConn;
            InitializeComponent();
            this.nLevel = nLevel;
            batchWeight = Math.Round(mainFrm.batchWeight, 2);
            batchWillWeight = mainFrm.batchWillWeight;
            lbAllWeight.Text = batchWillWeight.ToString() + "/" + Math.Round(batchWeight, 2).ToString();

            //if (!auto)
            //{
            //    tbShipmentdetalId.Enabled = true;
            //    tbVehicleorderNo.Enabled = true;
            //    tbWasteCode.Enabled = true;
            //    tbWasteName.Enabled = true;
            //    tbHazardArea.Enabled = true;
            //    dtpStart.Enabled = true;
            //    autoBotton.Text = "自动生成入库任务";
            //}
            //else
            //{
            //    tbShipmentdetalId.Enabled = false;
            //    tbVehicleorderNo.Enabled = false;
            //    tbWasteCode.Enabled = false;
            //    tbWasteName.Enabled = false;
            //    tbHazardArea.Enabled = false;
            //    dtpStart.Enabled = false;
            //    autoBotton.Text = "手动生成入库任务";
            //}
            goodsPatchList.Columns.Add("序号", (int)(goodsPatchList.Width * 0.05), HorizontalAlignment.Center);
            // goodsPatchList.Columns.Add("派车单id", (int)(goodsPatchList.Width * 0.2), HorizontalAlignment.Center);
            goodsPatchList.Columns.Add("废品单位", (int)(goodsPatchList.Width * 0.1), HorizontalAlignment.Center);
            // goodsPatchList.Columns.Add("对应接运单入库详情id", (int)(goodsPatchList.Width * 0.2), HorizontalAlignment.Center);
            goodsPatchList.Columns.Add("废品名称", (int)(goodsPatchList.Width * 0.2), HorizontalAlignment.Center);
            goodsPatchList.Columns.Add("生成时间", (int)(goodsPatchList.Width * 0.15), HorizontalAlignment.Center);
            goodsPatchList.Columns.Add("颜色", (int)(goodsPatchList.Width * 0.1), HorizontalAlignment.Center);
            goodsPatchList.Columns.Add("废物性状", (int)(goodsPatchList.Width * 0.1), HorizontalAlignment.Center);

            goodsPatchList.Columns.Add("废物包装", (int)(goodsPatchList.Width * 0.1), HorizontalAlignment.Center);

            //  goodsPatchList.Columns.Add("复核总重量", (int)(goodsPatchList.Width * 0.2), HorizontalAlignment.Center);
            //   goodsPatchList.Columns.Add("客户单位id", (int)(goodsPatchList.Width * 0.2), HorizontalAlignment.Center);
            //    goodsPatchList.Columns.Add("运输单位名称", (int)(goodsPatchList.Width * 0.05), HorizontalAlignment.Center);
            //   goodsPatchList.Columns.Add("客户公司名称", (int)(goodsPatchList.Width * 0.05), HorizontalAlignment.Center);
            // //   goodsPatchList.Columns.Add("五联单号", (int)(goodsPatchList.Width * 0.2), HorizontalAlignment.Center);

            goodsPatchList.Columns.Add("已入重量", (int)(goodsPatchList.Width * 0.1), HorizontalAlignment.Center);
            goodsPatchList.Columns.Add("危废代码", (int)(goodsPatchList.Width * 0.1), HorizontalAlignment.Center);
            tbWasteCode.Text = mainFrm.goodsSku[0];
            tbWasteName.Text = mainFrm.goodsName[0];
            cbWasteKinds.SelectedIndex = mainFrm.goodsKind[0] == 1 ? 0 : (mainFrm.goodsKind[0] == 2 ? 1 :  (mainFrm.goodsKind[0] == 4 ? 1 : -1));
            tbShipmentdetalId.Text = mainFrm.batchid[0];
            tbVehicleorderNo.Text = mainFrm.batchNo[0];
            tbHazardArea.Text = mainFrm.hazardArea[0];
            tbEnterWeight.Text = batchWeight.ToString();
            tbWillWeight.Text = batchWillWeight.ToString();
          
        }

        private int goodsKindsInt(string subItem)
        {
            switch (subItem)
            {
                case "吨桶": return 1;
                case "圆桶": return 2;

                default:
                    return 0;
            };
        }

        #region 废料开始入库
        private void inboundStart_Click(object sender, EventArgs e)
        {
            if (mainFrm.taskType[nLevel] != 1)
            {
                MessageBox.Show("当前非出入库模式！");
                return;
            }
            if (cbWasteKinds.SelectedIndex < 0 || tbWillWeight.Text == string.Empty || tbShipmentdetalId.Text == string.Empty || tbVehicleorderNo.Text == string.Empty || tbWasteCode.Text == string.Empty || tbWasteName.Text == string.Empty || tbEnterWeight.Text == string.Empty)
            {
                if (auto)
                    MessageBox.Show("请先选择当前入库批次！");
                else
                    MessageBox.Show("请先输入当前入库批次！");
                return;
            }
            if (mainFrm.goodsKind[0] == (cbWasteKinds.SelectedIndex + 1) && mainFrm.goodsSku[0] == tbWasteCode.Text && mainFrm.batchNo[0] == tbVehicleorderNo.Text && mainFrm.batchid[0] == tbShipmentdetalId.Text)
            {
                MessageBox.Show("当前入库批次为该批次！");
                return;
            }
            else if (DataBaseInterface.SelectComMessageCount(tbVehicleorderNo.Text, tbWasteCode.Text) == 1 && mainFrm.goodsKind[0] != 0 && mainFrm.goodsSku[0] != string.Empty && mainFrm.batchNo[0] != string.Empty && mainFrm.batchid[0] != string.Empty)
            {
                EndInbound();
                if (mainFrm.batchNo[0] != string.Empty)
                    return;
            }
            mainFrm.goodsKind[0] = cbWasteKinds.SelectedIndex + 1;
            mainFrm.goodsSku[0] = tbWasteCode.Text;
            mainFrm.goodsName[0] = tbWasteName.Text;
            mainFrm.batchNo[0] = tbVehicleorderNo.Text;
            mainFrm.batchid[0] = tbShipmentdetalId.Text;
            mainFrm.batchWeight = double.Parse(tbEnterWeight.Text);
            mainFrm.batchWillWeight = double.Parse(tbWillWeight.Text);
            mainFrm.batchWeight = double.Parse(tbEnterWeight.Text);
            mainFrm.ChangeWeight(mainFrm.batchWillWeight, mainFrm.batchWeight);
            mainFrm.hazardArea[0] = tbHazardArea.Text;
            mainFrm.Maketime = dtpStart.Text;
            mainFrm.dealWay[0] = auto ? 0 : 1;
            mainFrm.UpdateInboundShow();
            DataBaseInterface.UpdateComMessageState(mainFrm.batchNo[0], mainFrm.batchid[0], mainFrm.goodsSku[0], 1, mainFrm.Maketime);
            MessageBox.Show("开始入库！");

            DataBaseInterface.Insertshoudong(mainFrm.goodsSku[0], mainFrm.goodsName[0], mainFrm.batchNo[0], mainFrm.batchid[0], mainFrm.batchWeight.ToString(), mainFrm.hazardArea[0], mainFrm.Maketime, cbWasteKinds.Text);


            lbAllWeight.Text = batchWillWeight.ToString() + "/" + Math.Round(batchWeight, 2).ToString();
            FormChangeGoods_Load(new object(), new EventArgs());
            if (mainFrm.stopTaskCreate[nLevel])
                btnStop.Text = "开始生成任务";
            else
                btnStop.Text = "停止生成任务";

        }
        #endregion

        public void EndInbound()
        {
            if (mainFrm.taskType[nLevel] != 1)
                return;
            if (mainFrm.batchNo[0] != "")
            {
                DialogResult dialogResult = MessageBox.Show("是否结束该批次入库?结束后无法再次入库该批次", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.No)
                    return;
                else
                {
                    if ((mainFrm.batchWillWeight * 0.97) > mainFrm.batchWeight)
                    {
                        DialogResult dialogResults = MessageBox.Show("实际入库" + Math.Round(mainFrm.batchWeight * 0.97, 2).ToString() + "千克，小与预计入库" + (mainFrm.batchWillWeight * 1.03) + "千克，请核实！继续入库请点确定，否则请点取消", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (dialogResults == DialogResult.No)
                            return;
                    }
                    int num = DataBaseInterface.SelectTrayNum(mainFrm.batchNo[0], mainFrm.goodsSku[0]) - 1;
                    int all;
                    if (int.TryParse(txtUninboundNum.Text, out all))
                    {
                        DataBaseInterface.UpdateComMessageState(mainFrm.batchNo[0], mainFrm.batchid[0], mainFrm.goodsSku[0], 2, num, num + all);
                        mainFrm.goodsKind[0] = 0;
                        mainFrm.goodsSku[0] = string.Empty;
                        mainFrm.batchNo[0] = string.Empty;
                        mainFrm.batchid[0] = string.Empty;
                        mainFrm.goodsName[0] = string.Empty;
                        mainFrm.hazardArea[0] = string.Empty;
                        mainFrm.batchWeight = 0;
                        mainFrm.batchWillWeight = 0;
                        mainFrm.ChangeWeight(0, 0);
                        mainFrm.dealWay[0] = 0;
                    }
                    else
                    {
                        MessageBox.Show("输入未入托盘数量有误！");
                        return;
                    }
                }
                MessageBox.Show("结束入库完成！");
            }
        }
        #region 废料结束入库
        private void inboundStop_Click(object sender, EventArgs e)
        {
            EndInbound();
            if (mainFrm.goodsSku[0] == string.Empty)
            {
                lbAllWeight.Text = batchWillWeight.ToString() + "/" + Math.Round(batchWeight, 2).ToString();
                FormChangeGoods_Load(new object(), new EventArgs());
            }
            if (mainFrm.stopTaskCreate[nLevel])
                btnStop.Text = "开始生成任务";
            else
                btnStop.Text = "停止生成任务";
        }

        #endregion

        #region 页面加载
        private void FormChangeGoods_Load(object sender, EventArgs e)
        {
            dtpStart.Value = DateTime.Now;
            goodsPatchList.Items.Clear();
            int i = 0;
            DataSet ds = DataBaseInterface.SelectComMessage();
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                i++;
                string[] task = new string[goodsPatchList.Columns.Count];
                task[0] = i.ToString();
                //     task[1] = item["VEHICLEORDERID"].ToString();
                task[1] = item["VEHICLEORDERNO"].ToString();
                task[2] = item["SHIPMENTDETAILID"].ToString();
                //     task[4] = item["SHIPMENTCODE"].ToString();
                task[3] = item["SHIPMENTDATE"].ToString();
                task[4] = item["WASTENAME"].ToString();
                task[5] = item["WASTECODE"].ToString();
                //   task[8] = item["TOTALWEIGHT"].ToString();
                // task[9] = item["CUSTOMERCOMPANYID"].ToString();
                //  task[10] = item["TRANSPORTCOMPANYNAME"].ToString();
                //  task[11] = item["CUSTOMERCOMPANYNAME"].ToString();
                //   task[12] = item["FIVECOUPLETS"].ToString();
                //   task[13] = item["DOWN_DATE"].ToString();
                task[6] = item["boxtype"].ToString();
                task[7] = item["WASTEWEIGHT"].ToString();
                task[8] = item["HAZARDAREA"].ToString();
                ListViewItem lv = new ListViewItem(task);
                if (item["VEHICLEORDERSTATE"].ToString() == "1")
                    lv.BackColor = Color.Yellow;
                goodsPatchList.Items.Add(lv);
            }
        }
        #endregion

        private void ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem t = (ToolStripMenuItem)sender;
            if (mainFrm.taskType[nLevel] != 0)
            {
                if (!mainFrm.stopTaskCreate[nLevel] && int.Parse(t.Tag.ToString()) != 0 && int.Parse(t.Tag.ToString()) != mainFrm.taskType[nLevel])
                {
                    MessageBox.Show("请先停止当前工作模式！");
                    return;
                }
            }
            if (int.Parse(t.Tag.ToString()) == 1)
            {
                if (DataBaseInterface.SelectOtherTaskCount("1", 1) > 0)
                {
                    MessageBox.Show("存在正在执行的其他任务，请等待执行完成后切换状态！");
                    return;
                }
                mainFrm.workModeBotton.Text = "一楼出入库";
                //mainFrm.ReadBarCodeFromSPs["1"].UpdateRfidWorkPower(9000);
                //mainFrm.ReadBarCodeFromSPs["3"].UpdateRfidWorkPower(9000);

            }
            else if (int.Parse(t.Tag.ToString()) == 2)
            {
                if (DataBaseInterface.SelectOtherTaskCount("3", 1) > 0)
                {
                    MessageBox.Show("存在正在执行的其他任务，请等待执行完成后切换状态！");
                    return;
                }
                mainFrm.goodsKind[0] = 3;
                mainFrm.goodsSku[0] = "000000";
                mainFrm.batchNo[0] = "000000";
                mainFrm.batchid[0] = "000000";
                mainFrm.goodsName[0] = "空托盘组";
                mainFrm.batchWeight = 0;
                mainFrm.ChangeWeight(0, 0);
                mainFrm.dealWay[0] = 1;
                mainFrm.hazardArea[0] = "A";
                mainFrm.workModeBotton.Text = "一楼托盘组";

            }
            else if (int.Parse(t.Tag.ToString()) == 3)
            {
                if (DataBaseInterface.SelectOtherTaskCount("4", 1) > 0)
                {
                    MessageBox.Show("存在正在执行的其他任务，请等待执行完成后切换状态！");
                    return;
                }
                mainFrm.workModeBotton.Text = "一楼退库";

            }
            else if (int.Parse(t.Tag.ToString()) == 4)
            {
                if (DataBaseInterface.SelectOtherTaskCount("5", 1) > 0)
                {
                    MessageBox.Show("存在正在执行的其他任务，请等待执行完成后切换状态！");
                    return;
                }
                mainFrm.workModeBotton.Text = "一楼异常任务";
            }
            else if (int.Parse(t.Tag.ToString()) == 0)
            {
                mainFrm.goodsKind[0] = 0;
                mainFrm.batchid[0] = string.Empty;
                mainFrm.batchNo[0] = string.Empty;
                mainFrm.batchWeight = 0;
                mainFrm.batchWillWeight = 0;
                mainFrm.ChangeWeight(0, 0);
                mainFrm.goodsSku[0] = string.Empty;
                mainFrm.goodsName[0] = string.Empty;
                mainFrm.hazardArea[0] = string.Empty;
            }
            if (int.Parse(t.Tag.ToString()) != 0)
            {
                mainFrm.taskType[nLevel] = int.Parse(t.Tag.ToString());
                if (int.Parse(t.Tag.ToString()) == 1)
                {
                    mainFrm.systemStatus.WriteTaskModelCmd(nLevel, 1);
                }

                else if (int.Parse(t.Tag.ToString()) == 2)
                {
                    mainFrm.systemStatus.WriteTaskModelCmd(nLevel, 3);
                }

                else if (int.Parse(t.Tag.ToString()) == 3)
                {
                    mainFrm.systemStatus.WriteTaskModelCmd(nLevel, 4);
                }

                else if (int.Parse(t.Tag.ToString()) == 4)
                {
                    mainFrm.systemStatus.WriteTaskModelCmd(nLevel, 5);
                }
                mainFrm.stopTaskCreate[nLevel] = false;

            }
            else
            {
                mainFrm.stopTaskCreate[nLevel] = true;
                mainFrm.workModeBotton.Text = "一楼工作模式";
            }
            if (mainFrm.stopTaskCreate[nLevel])
                btnStop.Text = "开始生成任务";
            else
                btnStop.Text = "停止生成任务";

        }


        private void TextChangedThis(object sender, EventArgs e)
        {
            int i = 0;
            if (tbShipmentdetalId.Text != string.Empty)
            {
                foreach (ListViewItem item in goodsPatchList.Items)
                {
                    if (item.SubItems[3].Text == tbShipmentdetalId.Text)
                    {
                        goodsPatchList.Select();
                        goodsPatchList.Items[i].Selected = true;
                        return;
                    }
                    i++;
                }
            }
            i = 0;
            if (tbVehicleorderNo.Text != string.Empty)
            {
                foreach (ListViewItem item in goodsPatchList.Items)
                {
                    if (item.SubItems[2].Text == tbShipmentdetalId.Text)
                    {
                        goodsPatchList.Select();
                        goodsPatchList.Items[i].Selected = true;
                    }
                    i++;
                }
            }
        }

        private void KeyDownThis(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                int i = 0;
                if (tbShipmentdetalId.Text != string.Empty)
                {
                    foreach (ListViewItem item in goodsPatchList.Items)
                    {
                        if (item.SubItems[3].Text == tbShipmentdetalId.Text)
                        {
                            goodsPatchList.Select();
                            goodsPatchList.Items[i].Selected = true;
                            return;
                        }
                        i++;
                    }
                }
                i = 0;
                if (tbVehicleorderNo.Text != string.Empty)
                {
                    foreach (ListViewItem item in goodsPatchList.Items)
                    {
                        if (item.SubItems[2].Text == tbShipmentdetalId.Text)
                        {
                            goodsPatchList.Select();
                            goodsPatchList.Items[i].Selected = true;
                        }
                        i++;
                    }
                }
            }
        }

        private void goodsPatchList_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            try
            {
                if (goodsPatchList.SelectedItems.Count == 1)
                {
                    tbVehicleorderNo.Text = goodsPatchList.SelectedItems[0].SubItems[1].Text;
                    tbShipmentdetalId.Text = goodsPatchList.SelectedItems[0].SubItems[2].Text;
                    tbWasteName.Text = goodsPatchList.SelectedItems[0].SubItems[4].Text;
                    tbWasteCode.Text = goodsPatchList.SelectedItems[0].SubItems[5].Text;
                    // tbWillWeight.Text = goodsPatchList.SelectedItems[0].SubItems[6].Text;
                    tbEnterWeight.Text = goodsPatchList.SelectedItems[0].SubItems[7].Text;
                    tbHazardArea.Text = goodsPatchList.SelectedItems[0].SubItems[8].Text;
                    inCount();
                }
            }
            catch (Exception ex)
            {

            }
        }

        //#region 手自动生成入库单
        //private void autoBotton_Click(object sender, EventArgs e)
        //{
        //    if (auto)
        //    {
        //        tbShipmentdetalId.Enabled = true;
        //        tbVehicleorderNo.Enabled = true;
        //        tbWasteCode.Enabled = true;
        //        tbWasteName.Enabled = true;
        //        dtpStart.Enabled = true;
        //        tbHazardArea.Enabled = true;
        //        auto = false;
        //        autoBotton.Text = "自动生成入库任务";
        //    }
        //    else
        //    {
        //        tbShipmentdetalId.Enabled = false;
        //        tbVehicleorderNo.Enabled = false;
        //        tbWasteCode.Enabled = false;
        //        tbWasteName.Enabled = false;
        //        dtpStart.Enabled = false;
        //        tbHazardArea.Enabled = true;
        //        auto = true;
        //        autoBotton.Text = "手动生成入库任务";
        //    }
        //}

        //#endregion

        #region 启动停止生成任务
        private void btnStop_Click(object sender, EventArgs e)
        {
            if (mainFrm.stopTaskCreate[nLevel])
            {
                btnStop.Text = "停止生成任务";
                mainFrm.stopTaskCreate[nLevel] = false;

            }
            else
            {
                btnStop.Text = "开始生成任务";
                mainFrm.stopTaskCreate[nLevel] = true;
                mainFrm.systemStatus.WriteTaskModelCmd(nLevel, 0);

            }
        }


        #endregion

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        public void inCount()
        {

            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;

                string strSQL = "";
                try
                {
                    string sql = "select count(1) from td_plt_location_dic t where t.batch_id = '" + goodsPatchList.SelectedItems[0].SubItems[2].Text + "'";
                    DataSet ds2 = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
                    textJoinCount.Text = ds2.Tables[0].Rows[0]["count(1)"].ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("查询接口表收到信息出错：" + ex.Message);
                    return;
                }


            }
          
        }
    }
}
