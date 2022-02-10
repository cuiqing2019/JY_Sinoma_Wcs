using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataBase;

namespace JY_Sinoma_WCS
{
    public partial class FrmStacker : Form
    {
        public Stacker sk;
        private int fromRow1;
        private int fromBay1;
        private int fromLay1;
        private int toRow1;
        private int toBay1;
        private int toLay1;
       
        #region 构造函数
        /// <summary>
        /// 初始化堆垛机手动处理窗体
        /// </summary>
        /// <param name="sk"></param>
        public FrmStacker(Stacker sk)
        {
            InitializeComponent();

            this.sk = sk;
            this.Text = "堆垛机" + sk.deviceId.ToString() + "的状态及手动控制窗口";
        }
        #endregion

        /// <summary>
        /// Load载入页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FrmStacker_Load(object sender, EventArgs e)
        {
            try
            {
                lvTask.Columns.Add("子任务号", (int)(lvTask.Width * 0.1), HorizontalAlignment.Center);
                lvTask.Columns.Add("主任务号", (int)(lvTask.Width * 0.15), HorizontalAlignment.Center);
                lvTask.Columns.Add("托盘条码", (int)(lvTask.Width * 0.1), HorizontalAlignment.Center);
                lvTask.Columns.Add("起始地址", (int)(lvTask.Width * 0.15), HorizontalAlignment.Center);
                lvTask.Columns.Add("目的地址", (int)(lvTask.Width * 0.15), HorizontalAlignment.Center);
                lvTask.Columns.Add("主任务类型", (int)(lvTask.Width * 0.1), HorizontalAlignment.Center);
                lvTask.Columns.Add("子任务类型", (int)(lvTask.Width * 0.1), HorizontalAlignment.Center);
                lvTask.Columns.Add("状态", (int)(lvTask.Width * 0.1), HorizontalAlignment.Center);
                RefreshStatus();
                bindListView();
                timer1.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        public void bindListView()
        {

            DataRow dr = DataBaseInterface.GetTempStackerTk(this.sk.nChannelNo, this.sk.statusStruct.taskId);
            lvTask.BeginUpdate();
            lvTask.Items.Clear();
            if (dr != null)
            {
                String[] items = new String[lvTask.Columns.Count];
                items[0] = dr["task_id"].ToString();
                items[1] = dr["TASK_ID"].ToString();
                items[2] = dr["BOX_BARCODE"].ToString();
                items[3] = dr["from_unit"].ToString();
                items[4] = dr["to_unit"].ToString();
                items[5] = DecodeMTaskType(int.Parse(dr["task_type"].ToString()));
                items[6] = sk.frmMain.DecodeDTaskType(int.Parse(dr["task_type"].ToString()), int.Parse(dr["step"].ToString()));
                items[7] = int.Parse(dr["status"].ToString()) == 1 ? "执行中" : "已完成";
                lvTask.Items.Add(new ListViewItem(items));
            }
            lvTask.EndUpdate();
        
        }

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
                return "入库";
            else if (nType == 2)//出库
                return "出库";
            else if (nType == 3)
                return "空托入库";
            else if (nType == 4)
                return "退库";
            else if (nType == 5)
                return "异常回库";
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
        }
        /// <summary>
        /// 刷新状态
        /// </summary>
        private void RefreshStatus()
        {
            String strWorkMode;
            if (sk.pcWorkMode == 0)
                strWorkMode = "手工任务";
            else
                strWorkMode = "自动任务";
            if (sk.connStatus)
                strWorkMode += ",心跳信号正常";
            else
                strWorkMode += ",心跳信号异常";
            if (sk.isBindPLC)
                strWorkMode += ",PLC连接正常";
            else
                strWorkMode += ",PLC连接异常";
            lbSKStatus.Text = "堆垛机状态：" + strWorkMode;
            lbCmdTaskId1.Text = sk.plcCmdStruct.taskId.ToString();
            lbCmdFromRow1.Text = sk.plcCmdStruct.fromRow.ToString();
            lbCmdFromBay1.Text = sk.plcCmdStruct.fromBay.ToString();
            lbCmdFromLay1.Text = sk.plcCmdStruct.fromLevel.ToString();
            lbCmdToRow1.Text = sk.plcCmdStruct.toRow.ToString();
            lbCmdToBay1.Text = sk.plcCmdStruct.toBay.ToString();
            lbCmdToLay1.Text = sk.plcCmdStruct.toLevel.ToString();
            if (sk.cmdDic.ContainsKey(sk.plcCmdStruct.cmd.ToString()))
                lbCmd.Text = sk.cmdDic[sk.plcCmdStruct.cmd.ToString()];
            else
                lbCmd.Text = "未定义" + sk.plcCmdStruct.cmd.ToString();
            lbCmdStatus.Text = sk.plcCmdStruct.downloadStatus.ToString();

            if (sk.workModeStatusDic.ContainsKey(sk.statusStruct.workMode.ToString()))
                lbStatusWorkMode.Text = sk.workModeStatusDic[sk.statusStruct.workMode.ToString()]
                    + sk.statusStruct.workMode.ToString();
            else
                lbStatusWorkMode.Text = "未定义" + sk.statusStruct.workMode.ToString();
            if (sk.ErrorCode() !=string.Empty)
                lbStatusDeviceStastus.Text = sk.ErrorCode()
                    + sk.statusStruct.taskState.ToString();
            else
                lbStatusDeviceStastus.Text = "未定义" + sk.statusStruct.taskState.ToString();
            lbStatusTaskId.Text = sk.statusStruct.taskId.ToString();

            if (sk.forkStatusDic.ContainsKey(sk.statusStruct.loadStatus.ToString()))
                lbStatusLoadStatus.Text = sk.forkStatusDic[sk.statusStruct.loadStatus.ToString()]
                    + sk.statusStruct.loadStatus.ToString();
            else
                lbStatusLoadStatus.Text = "未定义" + sk.statusStruct.loadStatus.ToString();

            if (sk.taskStatusDic.ContainsKey(sk.statusStruct.taskStep.ToString()))
                lbStatusTaskStep.Text = sk.taskStatusDic[sk.statusStruct.taskStep.ToString()]
                    + sk.statusStruct.taskStep.ToString();
            else
                lbStatusTaskStep.Text = "未定义" + sk.statusStruct.taskStep.ToString();
            lbStatusCurrentBay.Text = sk.statusStruct.currentBay.ToString();
            lbStatusCurrentLay.Text = sk.statusStruct.currentLevel.ToString();
            lbStatusToBay.Text = sk.plcCmdStruct.toBay.ToString();
            lbStatusToLay.Text = sk.plcCmdStruct.toLevel.ToString();
            lbStatusToRow.Text = sk.plcCmdStruct.toRow.ToString();
            X.Text = sk.statusStruct.currentX.ToString();
            Y.Text = sk.statusStruct.currentY.ToString();
            Z.Text = sk.statusStruct.currentZ.ToString();
            lbTaskStatus.Text = sk.statusStruct.taskState == 0 ? "空闲" : sk.statusStruct.taskState == 1 ? "执行中" : "未知的任务状态";
            if (sk.statusStruct.workMode == 1 && sk.statusStruct.taskState == 0 && sk.statusStruct.taskStep == 5)
            {
                lbStatusWorkStatus.Text = "10";
            }
            else
            {
                lbStatusWorkStatus.Text = "1";
            }
            lbStatusHeartBeat.Text = sk.statusStruct.heartBeat.ToString();
            if (sk.pcWorkMode == 1)
            {
                button1.Text = "自动模式";
            }
            else
            {
                button1.Text = "手动模式";
            }

        }
        /// <summary>
        /// 形成搬运指令
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btGetAndPut_Click(object sender, EventArgs e)
        {
            //堆垛机全自动，空闲，无货，手工任务，连接状态
            if (sk.statusStruct.workMode == 1 && sk.pcWorkMode == 0 && sk.connStatus && sk.isBindPLC && (((sk.statusStruct.taskStep == 0 || sk.statusStruct.taskStep == 5) && sk.statusStruct.taskState == 0)||(sk.manualCmdStruct.cmd == 7 ||sk.manualCmdStruct.cmd == 8)))
            {
                if (sk.manualStatus == 1)
                {
                    string str = "上个手工命令尚未执行，是否覆盖上个命令？";
                    DialogResult dialogResult = MessageBox.Show(str, "系统提示", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
                GetData();
                string strM = CheckData(5);
                if (strM != string.Empty)
                {
                    MessageBox.Show(strM);
                    return;
                }
                if ((fromRow1 == 0 && fromBay1 == 0 && fromLay1 == 0 && toRow1 == 0 && toBay1 == 0 && toLay1 == 0) || (sk.manualCmdStruct.cmd == 7 || sk.manualCmdStruct.cmd == 8))
                {
                    sk.ClearManualCMdStruct();
                    sk.manualCmdStruct.fromBay = sk.cmdStruct.fromBay;
                    sk.manualCmdStruct.fromLevel = sk.cmdStruct.fromLevel;
                    sk.manualCmdStruct.fromRow = sk.cmdStruct.fromRow;
                    sk.manualCmdStruct.taskId = sk.cmdStruct.taskId;
                    sk.manualCmdStruct.taskType = sk.cmdStruct.taskType;
                    sk.manualCmdStruct.toBay = sk.cmdStruct.toBay;
                    sk.manualCmdStruct.toLevel = sk.cmdStruct.toLevel;
                    sk.manualCmdStruct.toRow = sk.cmdStruct.toRow;
                    sk.manualCmdStruct.cmd = 5;
                    sk.manualCmdStruct.downloadStatus = 0;
                    sk.manualStatus = 0;
                    sk.WriteCmdData(sk.CreateManualCmdString());
                    sk.ClearManualCMdStruct(); 
                }
                else
                {
                    sk.ClearManualCMdStruct();
                    if (sk.manualTaskId >= 200)
                        sk.manualTaskId = 1;
                    sk.manualTaskId++;
                    sk.manualCmdStruct.taskId = sk.manualTaskId;
                    sk.manualCmdStruct.fromRow = fromRow1;
                    sk.manualCmdStruct.fromBay = fromBay1;
                    sk.manualCmdStruct.fromLevel = fromLay1;
                    sk.manualCmdStruct.toRow = toRow1;
                    sk.manualCmdStruct.toBay = toBay1;
                    sk.manualCmdStruct.toLevel = toLay1;
                    sk.manualCmdStruct.downloadStatus = 1;
                    sk.manualCmdStruct.cmd = 5;
                    sk.manualStatus = 1;
                }
            }
            else
            {
                if (sk.pcWorkMode != 0)
                {
                    MessageBox.Show("堆垛机不在手动状态，无法执行该指令！");
                }
                else
                {
                    MessageBox.Show("堆垛机不在可写任务状态，无法执行该指令！");
                }
            }
        }

        private void btSetWorkMode_Click(object sender, EventArgs e)
        {
            //堆垛机允许写新任务，工作在联机状态，手工任务，连接状态
            if (sk.statusStruct.workMode == 1 && sk.pcWorkMode == 0 && sk.connStatus && sk.isBindPLC)
            {
                if (sk.manualStatus == 1)
                {
                    string str = "上个手工命令尚未执行，是否覆盖上个命令？";
                    DialogResult dialogResult = MessageBox.Show(str, "系统提示", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
                sk.ClearManualCMdStruct();
                sk.manualCmdStruct.fromBay = sk.cmdStruct.fromBay;
                sk.manualCmdStruct.fromLevel = sk.cmdStruct.fromLevel;
                sk.manualCmdStruct.fromRow = sk.cmdStruct.fromRow;
                sk.manualCmdStruct.taskId = sk.cmdStruct.taskId;
                sk.manualCmdStruct.taskType = sk.cmdStruct.taskType;
                sk.manualCmdStruct.toBay = int.Parse(lbStatusToBay.Text);
                sk.manualCmdStruct.toLevel = int.Parse(lbStatusToLay.Text);
                sk.manualCmdStruct.toRow = int.Parse(lbStatusToRow.Text); 
                sk.manualCmdStruct.cmd = 8;
                sk.manualCmdStruct.downloadStatus = 0;
                sk.manualStatus = 1;
            }
            else
            {
                if (sk.pcWorkMode != 0)
                {
                    MessageBox.Show("堆垛机不在手动状态，无法执行该指令！");
                }
                else
                {
                    MessageBox.Show("堆垛机不在可写任务状态，无法执行该指令！");
                }
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

        private void btHp_Click(object sender, EventArgs e)
        {
            //堆垛机允许写新任务，工作在联机状态，手工任务，连接状态
            if (sk.statusStruct.workMode == 1 && sk.pcWorkMode == 0 && sk.connStatus && sk.isBindPLC && (sk.statusStruct.taskStep == 0 || sk.statusStruct.taskStep == 5) && sk.statusStruct.taskState == 0)
            {
                if (sk.manualStatus == 1)
                {
                    string str = "上个手工命令尚未执行，是否覆盖上个命令？";
                    DialogResult dialogResult = MessageBox.Show(str, "系统提示", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
                sk.ClearManualCMdStruct();
                if (sk.manualTaskId >= 200)
                    sk.manualTaskId = 1;
                sk.manualTaskId++;
                sk.manualCmdStruct.taskId = sk.manualTaskId;
                sk.manualCmdStruct.cmd = 6;
                sk.manualCmdStruct.downloadStatus = 1;
                sk.manualStatus = 1;
            }
            else
            {
                if ( sk.pcWorkMode != 0)
                {
                    MessageBox.Show("堆垛机不在手动状态，无法执行该指令！");
                }
                else
                {
                    MessageBox.Show("堆垛机不在可写任务状态，无法执行该指令！");
                }
            }
        }
        /// <summary>
        /// 取货行走，行走到起始货位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btGetWalk_Click(object sender, EventArgs e)
        {
            //堆垛机允许写新任务，工作在联机状态，手工任务，连接状态
            if (sk.statusStruct.workMode == 1 && sk.pcWorkMode == 0 && sk.connStatus && sk.isBindPLC && (sk.statusStruct.taskStep == 0 || sk.statusStruct.taskStep == 5) && sk.statusStruct.taskState == 0)
            {
                if (sk.manualStatus == 1)
                {
                    string str = "上个手工命令尚未执行，是否覆盖上个命令？";
                    DialogResult dialogResult = MessageBox.Show(str, "系统提示", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
                GetData();
                string strM = CheckData(1);
                if (strM != string.Empty)
                {
                    MessageBox.Show(strM);
                    return;
                }
                sk.ClearManualCMdStruct();
                if (sk.manualTaskId >= 200)
                    sk.manualTaskId = 1;
                sk.manualTaskId++;
                sk.manualCmdStruct.taskId = sk.manualTaskId;
                sk.manualCmdStruct.fromRow = fromRow1;
                sk.manualCmdStruct.fromBay = fromBay1;
                sk.manualCmdStruct.fromLevel = fromLay1;
                sk.manualCmdStruct.cmd = 1;
                sk.manualCmdStruct.downloadStatus = 1;
                sk.manualStatus = 1;
            }
            else
            {
                if ( sk.pcWorkMode != 0)
                {
                    MessageBox.Show("堆垛机不在手动状态，无法执行该指令！");
                }
                else
                {
                    MessageBox.Show("堆垛机不在可写任务状态，无法执行该指令！");
                }
            }
        }
        /// <summary>
        /// 放货行走，行走到目标货位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPutWalk_Click(object sender, EventArgs e)
        {
            //堆垛机允许写新任务，工作在联机状态，手工任务，连接状态
            if (sk.statusStruct.workMode == 1 && sk.pcWorkMode == 0 && sk.connStatus && sk.isBindPLC && (sk.statusStruct.taskStep == 0 || sk.statusStruct.taskStep == 5) && sk.statusStruct.taskState == 0)
            {
                if (sk.manualStatus == 1)
                {
                    string str = "上个手工命令尚未执行，是否覆盖上个命令？";
                    DialogResult dialogResult = MessageBox.Show(str, "系统提示", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
                GetData();
                string strM = CheckData(2);
                if (strM != string.Empty)
                {
                    MessageBox.Show(strM);
                    return;
                }
                sk.ClearManualCMdStruct();
                if (sk.manualTaskId >= 200)
                    sk.manualTaskId = 1;
                sk.manualTaskId++;
                sk.manualCmdStruct.taskId = sk.manualTaskId;
                sk.manualCmdStruct.toRow = toRow1;
                sk.manualCmdStruct.toBay= toBay1;
                sk.manualCmdStruct.toLevel = toLay1;
                sk.manualCmdStruct.cmd = 3;
                sk.manualCmdStruct.downloadStatus = 1;
                sk.manualStatus = 1;
            }
            else
            {
                if (sk.pcWorkMode != 0)
                {
                    MessageBox.Show("堆垛机不在手动状态，无法执行该指令！");
                }
                else
                {
                    MessageBox.Show("堆垛机不在可写任务状态，无法执行该指令！");
                }
            }
        }
        /// <summary>
        /// 取货，行走到起始货位并将货物取到货叉上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btGet_Click(object sender, EventArgs e)
        {
            //堆垛机允许写新任务，工作在联机状态，手工任务，连接状态
            if (sk.statusStruct.workMode == 1 && sk.pcWorkMode == 0 && sk.connStatus && sk.isBindPLC && (sk.statusStruct.taskStep == 0 || sk.statusStruct.taskStep == 5) && sk.statusStruct.taskState == 0)
            {
                if (sk.manualStatus == 1)
                {
                    string str = "上个手工命令尚未执行，是否覆盖上个命令？";
                    DialogResult dialogResult = MessageBox.Show(str, "系统提示", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
                GetData();
                string strM = CheckData(3);
                if (strM != string.Empty)
                {
                    MessageBox.Show(strM);
                    return;
                }
                sk.ClearManualCMdStruct();
                if (sk.manualTaskId >= 200)
                    sk.manualTaskId = 1;
                sk.manualTaskId++;
                sk.manualCmdStruct.taskId = sk.manualTaskId;
                sk.manualCmdStruct.fromRow = fromRow1;
                sk.manualCmdStruct.fromBay = fromBay1;
                sk.manualCmdStruct.fromLevel = fromLay1;
                sk.manualCmdStruct.cmd = 2;
                sk.manualCmdStruct.downloadStatus = 1;
                sk.manualStatus = 1;
            }
            else
            {
                if (sk.pcWorkMode != 0)
                {
                    MessageBox.Show("堆垛机不在手动状态，无法执行该指令！");
                }
                else
                {
                    MessageBox.Show("堆垛机不在可写任务状态，无法执行该指令！");
                }
            }
        }
        /// <summary>
        /// 放货，行走到目标货位并将货物放到货位上
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btPut_Click(object sender, EventArgs e)
        {
            //堆垛机允许写新任务，工作在联机状态，手工任务，连接状态
            if (sk.statusStruct.workMode == 1 && sk.pcWorkMode == 0 && sk.connStatus && sk.isBindPLC && (sk.statusStruct.taskStep == 0 || sk.statusStruct.taskStep == 5) && sk.statusStruct.taskState == 0)
            {
                if (sk.manualStatus == 1)
                {
                    string str = "上个手工命令尚未执行，是否覆盖上个命令？";
                    DialogResult dialogResult = MessageBox.Show(str, "系统提示", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
                GetData();
                string strM = CheckData(4);
                if (strM != string.Empty)
                {
                    MessageBox.Show(strM);
                    return;
                }
                sk.ClearManualCMdStruct();
                if (sk.manualTaskId >= 200)
                    sk.manualTaskId = 1;
                sk.manualTaskId++;
                sk.manualCmdStruct.taskId = sk.manualTaskId;
                sk.manualCmdStruct.toRow = toRow1;
                sk.manualCmdStruct.toBay = toBay1;
                sk.manualCmdStruct.toLevel = toLay1;
                sk.manualCmdStruct.cmd = 4;
                sk.manualCmdStruct.downloadStatus = 1;
                sk.manualStatus = 1;
            }
            else
            {
                if ( sk.pcWorkMode != 0)
                {
                    MessageBox.Show("堆垛机不在手动状态，无法执行该指令！");
                }
                else
                {
                    MessageBox.Show("堆垛机不在可写任务状态，无法执行该指令！");
                }
            }
        }
        /// <summary>
        /// 清错
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btClearError_Click(object sender, EventArgs e)
        {
            //堆垛机允许写新任务，工作在联机状态，手工任务，连接状态
            if (sk.statusStruct.workMode == 1 && sk.pcWorkMode == 0 && sk.connStatus && sk.isBindPLC )
            {
                if (sk.manualStatus == 1)
                {
                    string str = "上个手工命令尚未执行，是否覆盖上个命令？";
                    DialogResult dialogResult = MessageBox.Show(str, "系统提示", MessageBoxButtons.YesNo,
                        MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                    if (dialogResult == DialogResult.No)
                    {
                        return;
                    }
                }
                sk.ClearManualCMdStruct();
                sk.manualCmdStruct.taskId = 0;
                sk.manualCmdStruct.cmd = 7;
                sk.manualCmdStruct.downloadStatus = 0;
                sk.manualStatus = 1;
            }
            else
            {
                if (sk.pcWorkMode != 0)
                {
                    MessageBox.Show("堆垛机不在手动状态，无法执行该指令！");
                }
                else
                {
                    MessageBox.Show("堆垛机不在可写任务状态，无法执行该指令！");
                }
            }

        }
        private void GetData()
        {
            fromRow1 = (int)NuFromRow1.Value;
            fromBay1 = (int)NuFromBay1.Value;
            fromLay1 = (int)NuFromLay1.Value;
            toRow1 = (int)NuToRow1.Value;
            toBay1 = (int)NuToBay1.Value;
            toLay1 = (int)NuToLay1.Value;
        }
        /// <summary>
        /// 判断数据合法性
        /// </summary>
        /// <returns></returns>
        private string CheckData(int cmd)
        {
            if ((cmd == 1 || cmd == 2 || cmd == 5) && (fromRow1 != sk.deviceId * 2 - 1 || fromRow1 != sk.deviceId * 2) && (fromBay1 < 0 || fromBay1 > 26) && (fromLay1 < 0 || fromLay1 > 5))
            {
                if (fromBay1 == 0)
                    if (fromLay1 == 1 && (fromRow1 % 2 == 1 || fromRow1 == 2))
                        return string.Empty;
                    else
                        return "起始地址不合法";
                else if (fromBay1 == 26)
                {
                    if (fromLay1 == 5 && (fromBay1 % 2 == 0))
                        return string.Empty;
                    else
                        return "起始地址不合法";
                }
                return "起始地址不合法";
            }
            if ((cmd == 3 || cmd == 4 || cmd == 5) && (toRow1 != sk.deviceId * 2 - 1 || toRow1 != sk.deviceId * 2) && (toBay1 < 0 || toBay1 > 26) && (toLay1 < 0 || toLay1 > 5))
            {
                if (fromBay1 == 0)
                    if (fromLay1 == 1 && (fromRow1 % 2 == 1 || fromRow1 == 2))
                        return string.Empty;
                    else
                        return "目的地址不合法";
                else if (fromBay1 == 26)
                {
                    if (fromLay1 == 5 && (fromBay1 % 2 == 0))
                        return string.Empty;
                    else
                        return "目的地址不合法";
                }
                return "目的地址不合法";
            }
            return string.Empty;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (sk.pcWorkMode == 1)
            {
                sk.pcWorkMode = 0;
                button1.Text = "手动模式";
            }
            else
            {
                sk.pcWorkMode = 1;
                button1.Text = "自动模式";
               
            }
        }

        private void btSetWorkMode_Click_1(object sender, EventArgs e)
        {

        }




        //private void lvTask_DoubleClick(object sender, EventArgs e)
        //]
        //    ListView lv = sender as ListView;
        //    if (lv.SelectedItems.Count < 1) return;
        //    if (Convert.ToInt32(lv.SelectedItems[0].SubItems[8].Text) == 10)
        //    {
        //        MessageBox.Show("已经完成的任务不能再进行操作!");
        //        return;
        //    }
        //    string routeTaskSeq = ((ListView)sender).SelectedItems[0].SubItems[10].Text;
        //    if (MessageBox.Show("是否将任务号为" + " " + routeTaskSeq + "的任务报完成！", "提示",
        //        MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
        //    {
        //        //DataBaseInterface.StackerFinishedTask(Convert.ToInt32(routeTaskSeq));
        //    }
           
        //}



    }
}
