using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataBase;
using MySql.Data.MySqlClient;

namespace JY_Sinoma_WCS
{
    public partial class FrmAGV : Form
    {
        public AGV agv;
        public ConnectPool dbConn;
        public Pumping pumping;
        //private int fromRow1;
        //private int fromBay1;
        //private int fromLay1;
        //private int toRow1;
        //private int toBay1;
        //private int toLay1;
       
        #region 构造函数
        /// <summary>
        /// 初始化堆垛机手动处理窗体
        /// </summary>
        /// <param name="sk"></param>
        public FrmAGV(AGV agv,Pumping pumping)
        {
            InitializeComponent();

            this.agv = agv;
            this.dbConn = agv.dbConn;
            this.pumping = pumping;
            this.Text = "AGV的状态及手动控制窗口";
        }
        #endregion

        /// <summary>
        /// Load载入页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmStacker_Load(object sender, EventArgs e)
        {
            lvTask.Columns.Add("子任务号", (int)(lvTask.Width * 0.1), HorizontalAlignment.Center);
            lvTask.Columns.Add("主任务号", (int)(lvTask.Width * 0.15), HorizontalAlignment.Center);
            lvTask.Columns.Add("托盘条码", (int)(lvTask.Width * 0.2), HorizontalAlignment.Center);
            lvTask.Columns.Add("起始地址", (int)(lvTask.Width * 0.1), HorizontalAlignment.Center);
            lvTask.Columns.Add("目的地址", (int)(lvTask.Width * 0.1), HorizontalAlignment.Center);
            lvTask.Columns.Add("货物类型", (int)(lvTask.Width * 0.1), HorizontalAlignment.Center);
            lvTask.Columns.Add("主任务类型", (int)(lvTask.Width * 0.15), HorizontalAlignment.Center);
            lvTask.Columns.Add("状态", (int)(lvTask.Width * 0.1), HorizontalAlignment.Center);
            RefreshStatus();
            bindListView();
            if (agv.auto)
            {
                waiting1.Enabled = false;
                waiting2.Enabled = false;
                pai.Enabled = false;
                lie.Enabled = false;
                ceng.Enabled = false;
                btSetWorkMode.Enabled = false;
                boxcodeText1.Enabled = false;
                boxcodeText2.Enabled = false;
                updateWaitingBoxCode.Enabled = false;
                freeUnit.Enabled = false;
                toPai.Enabled = false;
                toLie.Enabled = false;
                toCeng.Enabled = false;
            }
            else
            {
                waiting1.Enabled = true;
                waiting2.Enabled = true;
                pai.Enabled = true;
                lie.Enabled = true;
                ceng.Enabled = true;
                waiting1.Checked = true;
                boxcodeText1.Enabled = true;
                boxcodeText2.Enabled = true;
                btSetWorkMode.Enabled = true;
                updateWaitingBoxCode.Enabled = true;
                freeUnit.Enabled = true;
                toPai.Enabled = true;
                toLie.Enabled = true;
                toCeng.Enabled = true;
            }
            waiting2.Parent = groupBox2;
            timer1.Start();
        }
        public void bindListView()
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                DataSet ds = DataBaseInterface.GetAgvTask(conn);
                lvTask.BeginUpdate();
                lvTask.Items.Clear();
                if (ds != null)
                {
                    foreach(DataRow dr in ds.Tables[0].Rows)
                    {
                        String[] items = new String[lvTask.Columns.Count];
                        items[0] = dr["TASKID"].ToString();
                        items[1] = dr["TASK_ID"].ToString();
                        items[2] = dr["BOX_CODE"].ToString();
                        items[3] = dr["TASKSTARTPOINT"].ToString();
                        items[4] = dr["TASKENDPOINT"].ToString();
                        items[5] = dr["TASKSTARTPOINT"].ToString() == "1025" ? "吨桶" : "空吨桶";
                        items[6] = DecodeMTaskType(int.Parse(dr["TASK_TYPE"].ToString()));
                        items[7] = DecodeDTaskStatus(int.Parse(dr["task_status"].ToString()));
                        lvTask.Items.Add(new ListViewItem(items));
                    }
                   
                }
                lvTask.EndUpdate();
            }
               

        }

        #region 刷新抽液处状态

        public void bindPumpingStatus()
        {

            pumpingStatusMessage1.Text = string.Empty;
            if (agv.mainFrm.pumping.PointStatus("1001") != null)
            {
                if (agv.mainFrm.pumping.PointStatus("1001").Value.completion == 1)
                    pumpingStatusMessage1.Text += "取液完成";
                if (agv.mainFrm.pumping.PointStatus("1001").Value.permit == 1)
                    pumpingStatusMessage1.Text += "允许放桶";
            }
            pumpingStatusMessage2.Text = string.Empty;
            if (agv.mainFrm.pumping.PointStatus("1002") != null)
            {
                if (agv.mainFrm.pumping.PointStatus("1002").Value.completion == 1)
                    pumpingStatusMessage2.Text += "取液完成";
                if (agv.mainFrm.pumping.PointStatus("1002").Value.permit == 1)
                    pumpingStatusMessage2.Text += "允许放桶";
            }
            
            if (agv.auto)
            {
                using (MySqlConnection conn = dbConn.GetConnectFromPool())
                {
                    if (agv.mainFrm.pumping.PointBoxCodeSelect(conn,"1001") == string.Empty)
                    {
                        boxcodeText1.Text = "无托盘";
                    }
                    else if (agv.mainFrm.pumping.PointBoxCodeSelect(conn,"1001") != null)
                    {
                        boxcodeText1.Text = agv.mainFrm.pumping.PointBoxCodeSelect(conn,"1001");
                    }
                    if (agv.mainFrm.pumping.PointBoxCodeSelect(conn,"1002") == string.Empty)
                    {
                        boxcodeText2.Text = "无托盘";
                    }
                    else if (agv.mainFrm.pumping.PointBoxCodeSelect(conn,"1002") != null)
                    {
                        boxcodeText2.Text = agv.mainFrm.pumping.PointBoxCodeSelect(conn,"1002");
                    }
                }
                   
            }
           
        }

        #endregion

        #region 子任务状态识别
        /// <summary>
        /// 子任务状态识别
        /// </summary>
        /// <param name="strMsgType"></param>
        /// <param name="nType"></param>
        /// <returns></returns>
        public string DecodeDTaskStatus(int nStatus)
        {
            if (nStatus == 0)//入库
                return "新生成";
            else if (nStatus == 1)//出库
                return "执行中";
            else if (nStatus == 2)
                return "已完成";
            else
                return "未知";
        }
        #endregion

        #region 主任务类型识别
        /// <summary>
        /// 主任务类型识别
        /// </summary>
        /// <param name="strMsgType"></param>
        /// <param name="nType"></param>
        /// <returns></returns>
        public string DecodeMTaskType(int nType)
        {
            if (nType == 1)//入库
                return "入缓存区";
            else if (nType == 2)//出库
                return "出缓存区";
            else if (nType == 3)
                return "入抽液处";
            else
                return "未知";
        }
        #endregion
        /// <summary>
        /// Timer执行事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer1_Tick(object sender, EventArgs e)
        {
            RefreshStatus();
            if (CbRefreshTaskView.Checked == true)
            {
                bindListView();
            }
            if (updatePumpingStatus.Checked == true)
            {
                bindPumpingStatus();
            }
        }
        /// <summary>
        /// 刷新状态
        /// </summary>
        private void RefreshStatus()
        {
            String strWorkMode;
            if (!agv.auto)
                strWorkMode = "手工任务";
            else
                strWorkMode = "自动任务";

            if (!agv.mainFrm.AGVTaskCreate)
            {
                button2.Text = "停止生成任务";
                button2.BackColor = Color.Gray;
            }
            else
            {
                button2.Text = "开始生成任务";
                button2.BackColor = Color.Yellow;
            }
            DataTable dr = DataBaseInterface.AGVstatus();
            lbSKStatus.Text = "AGV任务下发状态：" + strWorkMode;
            if (dr != null)
            {
                if (dr.Rows[1]["STATUS_ID"].ToString() == "0")
                    lbSKStatus.Text += "；AGV设备状态：自动运行";
                else if (dr.Rows[1]["STATUS_ID"].ToString() == "1")
                    lbSKStatus.Text += "；AGV设备状态：手动运行";
                if (dr.Rows[0]["STATUS_ID"].ToString() == "0")
                    lbSKStatus.Text += "；AGV设备运行状态：正常";
                else
                    lbSKStatus.Text += "；AGV设备运行状态：" + dr.Rows[0]["STATUS_DESC"].ToString();
            }
        }

        private void btSetWorkMode_Click(object sender, EventArgs e)
        {
            if ((pai.SelectedIndex < 0 || lie.SelectedIndex < 0 || ceng .SelectedIndex < 0 )||(freeUnit.Checked && (toPai.SelectedIndex < 0 || toCeng.SelectedIndex < 0 || toLie.SelectedIndex < 0)))
            {
                MessageBox.Show("请先选择起始地址与目的地址！");
                return;
            }
            string locNo = "01" + pai.SelectedItem.ToString().PadLeft(3, '0') + lie.SelectedItem.ToString().PadLeft(3, '0') + ceng.SelectedItem.ToString().PadLeft(3, '0') + "1";
            string toUnit = string.Empty;
            string rs;
            if (waiting1.Checked)
            {
                toUnit = "1003";
            }
            else if (waiting2.Checked)
            {
                toUnit = "1004";
            }
            else if (freeUnit.Checked)
            {
                toUnit = "01" + toPai.SelectedItem.ToString().PadLeft(3, '0') + toLie.SelectedItem.ToString().PadLeft(3, '0') + toCeng.SelectedItem.ToString().PadLeft(3, '0') + "1";
            }
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                DataRow dr = DataBaseInterface.AgvLocationStatus(conn,locNo);
                if ((dr["use_status"].ToString() == "0" || dr["use_status"].ToString() == "2") && dr["unit_status"].ToString() == "1" && dr["box_code"].ToString() != string.Empty && toUnit != string.Empty)
                {
                   
                    if (DataBaseInterface.AGVTaskCreate(conn, dr["box_code"].ToString(), 2, locNo, toUnit, "0", out rs) != 1)
                        MessageBox.Show(rs);
                }
                else
                    MessageBox.Show("出库库位不满足出库条件！");
            }
               
        }

        private void FrmStacker_FormClosing(object sender, FormClosingEventArgs e)
        {
            timer1.Stop();
        }

        private void btClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

 
      
      
       
        /// <summary>
        /// 单选按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                if ((sender as CheckBox).Checked == true)
                {
                    foreach (CheckBox chk in (sender as CheckBox).Parent.Controls)
                    {
                        if (chk != sender)
                        {
                            chk.Checked = false;
                        }
                    }
                }
                if (freeUnit.Checked)
                {
                    toPai.Enabled = true;
                    toLie.Enabled = true;
                    toCeng.Enabled = true;
                }
                else
                {
                    toPai.Enabled = false;
                    toLie.Enabled = false;
                    toCeng.Enabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void workStatusButton_Click(object sender, EventArgs e)
        {
            if (workStatusButton.Text == "自动状态")
            {
                workStatusButton.Text = "手动状态";
                agv.auto = false;
                pai.Enabled = true;
                lie.Enabled = true;
                ceng.Enabled = true;
                waiting1.Enabled = true;
                waiting2.Enabled = true;
                waiting1.Checked = true;
                btSetWorkMode.Enabled = true;
                boxcodeText1.Enabled = true;
                boxcodeText2.Enabled = true;
                updateWaitingBoxCode.Enabled = true;
                freeUnit.Enabled = true;
            }
            else if (workStatusButton.Text == "手动状态")
            {
                workStatusButton.Text = "自动状态";
                agv.auto = true;
                pai.Enabled = false;
                lie.Enabled = false;
                ceng.Enabled = false;
                waiting1.Enabled = false;
                waiting2.Enabled = false;
                btSetWorkMode.Enabled = false;
                boxcodeText1.Enabled = false;
                boxcodeText2.Enabled = false;
                updateWaitingBoxCode.Enabled = false;
                freeUnit.Enabled = false;
               
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            agv.showError = true;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            agv.mainFrm.pumping.PointBoxCodeUpdate(boxcodeText1.Text, "1001");
            agv.mainFrm.pumping.PointBoxCodeUpdate(boxcodeText2.Text, "1002");
        }
        private void boxcodeText1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'a' && e.KeyChar <= 'z') || (e.KeyChar >= 'A' && e.KeyChar <= 'Z') || (e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == 8))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void boxcodeText2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if ((e.KeyChar >= 'a' && e.KeyChar <= 'z') || (e.KeyChar >= 'A' && e.KeyChar <= 'Z') || (e.KeyChar >= '0' && e.KeyChar <= '9') || (e.KeyChar == 8))
                e.Handled = false;
            else
                e.Handled = true;
        }

        private void locationAdminister_Click(object sender, EventArgs e)
        {
            FrmWLocation frmwlocation = new FrmWLocation(agv.mainFrm, "Waiting");
            frmwlocation.ShowDialog();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {

            if (agv.mainFrm.AGVTaskCreate)
            {
                button2.Text = "停止生成任务";
                agv.mainFrm.AGVTaskCreate = false;
                button2.BackColor = Color.Gray;
            }
            else
            {
                button2.Text = "开始生成任务";
                agv.mainFrm.AGVTaskCreate = true;
                button2.BackColor = Color.Green;
            }
        }
    }
}
