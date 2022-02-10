using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.IO.Ports;
using System.IO;
using OpcRcw.Da;
using OpcRcw.Comn;
using DataBase;
using PLC;
using MySql.Data.MySqlClient;

namespace JY_Sinoma_WCS
{
    /// <summary>
    /// 弗莱瑞达堆垛机处理类
    /// </summary>
    public class Stacker : OPCServer
    {
        #region 自定义变量
        public String Error = "";
        public static ConnectPool dbConn;//定义数据库连接
        public int pdtask = 0;
        int[] lastErrorCode = new int[6] { 0, 0, 0, 0, 0, 0 };
        /// <summary>
        /// 自动命令线程
        /// </summary>
        private Thread autoCmdThread;
        /// <summary>
        /// 主窗体
        /// </summary>
        public frmMain frmMain;
        public int nChannelNo;          //巷道编号
        /// <summary>
        /// PLC的绑定状态,PLC连接状态
        /// </summary>
        public bool isBindPLC = false;
        /// <summary>
        /// 根据心跳信号判断 连接状态true or false
        /// </summary>
        public bool connStatus = true;
        /// <summary>
        /// 状态结构体变量
        /// </summary>
        public StatusStruct statusStruct;
        /// <summary>
        /// 自动任务命令结构体变量
        /// </summary>
        public CmdStruct cmdStruct;
        /// <summary>
        /// 手工命令结构体变量
        /// </summary>
        public CmdStruct manualCmdStruct;
        /// <summary>
        /// PLC返回的命令结构体变量
        /// </summary>
        public CmdStruct plcCmdStruct;
        /// <summary>
        /// 堆垛机当前的位置（像素点）
        /// </summary>
        public Int32 skX, skY;
        /// <summary>
        /// 堆垛机的宽度和长度；（实际是托盘的宽度和长度）
        /// </summary>
        private Int32 skWidth, skHegiht;
        /// <summary>
        /// chushi Yzhi zuobiao
        /// </summary>
        public Int32 initskX = 0;
        /// <summary>
        /// 堆垛机的原点；x0为原点的x坐标，y0为原点的y坐标
        /// </summary>
        private Int32 x0, y0;
        ///<summary>
        /// 出库辊道下任务次数
        /// </summary>
        private int exit_electricsConveyor_count = 0;
        /// <summary>
        /// 状态类型
        /// </summary>
        public String statusType;
        /// <summary>
        /// 上位机设定的堆垛机工作模式：1自动任务，0人工任务
        /// </summary>
        public int pcWorkMode = 1;
        /// <summary>
        /// 堆垛机图标
        /// </summary>
        public Label lb;
        public bool closing = false;
        private LogFile skOutLog;
        private string logname;
        int nTaskType = 1;//第一次查找任务查找出库任务
        /// <summary>
        /// 堆垛机当前任务执行步数
        /// </summary>
        int step = 0;
        public string oracleError;
        /// <summary>
        /// 出入库任务标志位
        /// </summary>
        int taskTypeFlag = 1;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数，初始化堆垛机
        /// </summary>
        /// <param name="mainFrm">主窗体对象</param>
        /// <param name="row"></param>
        public Stacker(frmMain mainFrm, DataRow row)
            : base()
        {
            InitStacker(mainFrm, row);
        }
        private void InitStacker(frmMain mainFrm, DataRow row)
        {
            ThreadExceptionDialog.CheckForIllegalCrossThreadCalls = false;//允许直接访问线程之间的控件
            frmMain = mainFrm;
            dbConn = mainFrm.dbConn;
            deviceId = int.Parse(row["device_id"].ToString());
            deviceName = row["device_name"].ToString();
            statusType = "STACK";
            nChannelNo = int.Parse(row["CHANNEL_NUM"].ToString());
            x0 = Convert.ToInt32("0" + row["x"].ToString());
            y0 = Convert.ToInt32("0" + row["y"].ToString());
            skWidth = Convert.ToInt32("0" + row["width"].ToString());
            skHegiht = Convert.ToInt32("0" + row["LENGTH"].ToString());
            skX = x0;
            skY = y0;
            initskX = x0;
            statusStruct.returnDB = row["return_db"].ToString();
            statusStruct.returnItemHandle = new int[1];
            statusStruct.lastHeartBeatTime = DateTime.Now.Ticks;
            statusStruct.lastHeartBeat = 2;
            cmdStruct.cmdDB = row["write_db"].ToString();
            cmdStruct.cmdItemHandle = new int[1];
            lb = new Label();
            lb.Size = new Size(skWidth, skHegiht);
            lb.Location = new Point(x0, y0);
            lb.BackColor = Color.Gray;
            lb.Font = new Font("宋体", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
            lb.TextAlign = ContentAlignment.MiddleCenter;
            lb.DoubleClick += new System.EventHandler(this.lb_DoubleClick);
            lb.Tag = int.Parse(row["device_id"].ToString());
            lb.Text = nChannelNo.ToString();
            lb.Name = row["device_name"].ToString();
            lb.BringToFront();
            switch (nChannelNo)
            {
                case 1: mainFrm.pChannel1.Controls.Add(lb); break;
                case 2: mainFrm.pChannel2.Controls.Add(lb); break;
                case 3: mainFrm.pChannel3.Controls.Add(lb); break;
                default:
                    break;
            }
            InitDic();
            logname = "skOutLog" + nChannelNo.ToString();
            skOutLog = new LogFile(logname);
            //fs = new FileStream(DateTime.Now.ToString("yyyyMMdd") + deviceId + ".log", FileMode.Append, FileAccess.Write);
            BindPLC();
            autoCmdThread = new Thread(new ThreadStart(AutoCmd));
            autoCmdThread.IsBackground = true;
            if (!autoCmdThread.IsAlive)
            {
                autoCmdThread.Start();
            }
        }
        #endregion
        /// <summary>
        /// 堆垛机线程
        /// </summary>
        private void AutoCmd()
        {
            while (!closing)
            {

                Thread.Sleep(100);
                try
                {
                    if (isBindPLC && frmMain.deviceLinkIsOK)
                    {
                        RefreshStatus();
                        RefreshDisplay();
                        if (frmMain.bPicking)
                        {
                            StackerRun();
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("堆垛机线程" + ex.Message);
                }
                Thread.Sleep(500);
            }
        }
        #region 标签的双击事件
        public void lb_DoubleClick(object sender, EventArgs e)
        {
            string strTag = Convert.ToString(((Label)sender).Tag);
            int Index = int.Parse(strTag);
            FrmStacker frmStacker = new FrmStacker(this);
            frmStacker.ShowDialog();
        }
        #endregion
        /// <summary>
        /// 刷新读取PLC状态
        /// </summary>
        /// <returns></returns>
        public bool RefreshStatus()
        {

            try
            {
                if (!isBindPLC && !BindPLC())
                {
                    connStatus = false;
                    return false;
                }
                string[] returnReadValues = new string[1];
                try
                {
                    SyncRead(returnReadValues, statusStruct.returnItemHandle);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                DecodePLCStatus(returnReadValues);

                if (statusStruct.heartBeat != statusStruct.lastHeartBeat)
                {
                    statusStruct.lastHeartBeat = statusStruct.heartBeat;
                    statusStruct.lastHeartBeatTime = DateTime.Now.Ticks;
                    connStatus = true;
                }
                //10000000为1秒，11秒没有检测的心跳信号的变动，认为连接已经断开
                // if (DateTime.Now.Ticks - statusStruct.lastHeartBeatTime > 110000000)
                //     connStatus = false;
                if (ErrorCode() != string.Empty)
                {
                    if (statusStruct.workMode != 1)
                    {
                        pcWorkMode = 0;
                        WriteLogFile("堆垛机状态"
                            + ErrorCode()
                            + "操作方式" + statusStruct.workMode.ToString());

                        if (statusStruct.taskId != pdtask)
                        {
                            //DataBaseInterface.DeviceErrorMessage("STACK", statusStruct.taskId1, deviceName, 0, frmMain.deviceStatusDic[statusType + "," + statusStruct.deviceStatus.ToString()].statusDesc, nChannelNo);
                            Error = ErrorCode();
                            pdtask = statusStruct.taskId;
                        }
                    }
                }
                string[] readValues = new string[1];
                SyncRead(readValues, cmdStruct.cmdItemHandle);
                DecodePLCCmd(readValues);

            }
            catch (System.Exception error)
            {
                MessageBox.Show(String.Format("更新堆垛机状态失败:-{0}", error.Message),
                    "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                isBindPLC = false;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 刷新堆垛机显示状态
        /// </summary>
        public void RefreshDisplay()
        {
            try
            {
                if (!isBindPLC)
                {
                    lb.BackColor = Color.Blue;
                    return;
                }
                skX = initskX - (statusStruct.currentBay) * 21;
                lb.Location = new Point(skX, skY);
                #region 记录设备错误信息
                int errorCode = 0;
                if (ErrorCode() != "正常")
                {
                    if (statusStruct.errorCode1 != 0)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            int j = (int)Math.Pow(2, i);
                            if ((statusStruct.errorCode1 & (0x01 * j)) / (0x01 * j) == 1)
                            {

                                if (lastErrorCode[0] != errorCode + i)
                                {
                                    DataBaseInterface.DeviceErrorMessage(deviceName, 0, 0, "STACK", errorCode + i, errorCode1Select(i), statusStruct.taskId);
                                    lastErrorCode[0] = errorCode + i;
                                }
                            }
                        }
                    }
                    errorCode = 100;
                    if (statusStruct.errorCode2 != 0)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            int j = (int)Math.Pow(2, i);
                            if ((statusStruct.errorCode2 & (0x01 * j)) / (0x01 * j) == 1)
                            {

                                if (lastErrorCode[1] != errorCode + i)
                                {
                                    DataBaseInterface.DeviceErrorMessage(deviceName, 0, 0, "STACK", errorCode + i, errorCode2Select(i), statusStruct.taskId);
                                    lastErrorCode[1] = errorCode + i;
                                }
                            }
                        }
                    }
                    errorCode = 200;
                    if (statusStruct.errorCode3 != 0)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            int j = (int)Math.Pow(2, i);
                            if ((statusStruct.errorCode3 & (0x01 * j)) / (0x01 * j) == 1)
                            {

                                if (lastErrorCode[2] != errorCode + i)
                                {
                                    DataBaseInterface.DeviceErrorMessage(deviceName, 0, 0, "STACK", errorCode + i, errorCode3Select(i), statusStruct.taskId);
                                    lastErrorCode[2] = errorCode + i;
                                }
                            }
                        }
                    }
                    errorCode = 300;
                    if (statusStruct.errorCode4 != 0)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            int j = (int)Math.Pow(2, i);
                            if ((statusStruct.errorCode4 & (0x01 * j)) / (0x01 * j) == 1)
                            {

                                if (lastErrorCode[3] != errorCode + i)
                                {
                                    DataBaseInterface.DeviceErrorMessage(deviceName, 0, 0, "STACK", errorCode + i, errorCode4Select(i), statusStruct.taskId);
                                    lastErrorCode[3] = errorCode + i;
                                }
                            }
                        }
                    }
                    errorCode = 400;
                    if (statusStruct.errorCode5 != 0)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            int j = (int)Math.Pow(2, i);
                            if ((statusStruct.errorCode5 & (0x01 * j)) / (0x01 * j) == 1)
                            {

                                if (lastErrorCode[4] != errorCode + i)
                                {
                                    DataBaseInterface.DeviceErrorMessage(deviceName, 0, 0, "STACK", errorCode + i, errorCode5Select(i), statusStruct.taskId);
                                    lastErrorCode[4] = errorCode + i;
                                }
                            }
                        }
                    }
                    errorCode = 500;
                    if (statusStruct.errorCode6 != 0)
                    {
                        for (int i = 0; i < 16; i++)
                        {
                            int j = (int)Math.Pow(2, i);
                            if ((statusStruct.errorCode6 & (0x01 * j)) / (0x01 * j) == 1)
                            {

                                if (lastErrorCode[5] != errorCode + i)
                                {
                                    DataBaseInterface.DeviceErrorMessage(deviceName, 0, 0, "STACK", errorCode + i, errorCode6Select(i), statusStruct.taskId);
                                    lastErrorCode[5] = errorCode + i;
                                }
                            }
                        }
                    }
                    lb.BackColor = Color.Red;
                    return;
                }
                else if (lb.BackColor == Color.Red)
                {
                    bool lasterrorOver = false;
                    for (int i = 0; i < lastErrorCode.Length; i++)
                    {
                        if (lastErrorCode[i] != 0)
                        {
                            lastErrorCode[i] = 0;
                            lasterrorOver = true;
                        }
                    }
                    if (lasterrorOver)
                        DataBaseInterface.DeviceErrorMessage(deviceName, 0, 0, "STACK", 0, ErrorCode(), statusStruct.taskId);
                }
                #endregion
                if (connStatus && pcWorkMode == 1 && isBindPLC && statusStruct.taskState == 0) //联机正常空闲
                    lb.BackColor = Color.Green;
                else if (connStatus && pcWorkMode == 1 && isBindPLC && statusStruct.taskState == 1) //联机正常执行中
                    lb.BackColor = Color.Gold;
                else if (connStatus && pcWorkMode == 0 && isBindPLC) //手动正常
                    lb.BackColor = Color.LightGreen;
                else
                    lb.BackColor = Color.Red;

            }
            catch (Exception ex)
            {
                string estr = ex.Message.ToString();
            }
        }

        /// <summary>
        /// 堆垛机自动运行
        /// </summary>
        public void StackerRun()
        {
            string rs;
            bool isSendOut = true;
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                //堆垛机任务报完成

                if (statusStruct.workMode == 1 && (statusStruct.taskStep == 5 || statusStruct.taskStep == 0) && statusStruct.taskId > 200 && statusStruct.taskState == 0 && pcWorkMode == 1)
                {
                    DataRow dr = DataBaseInterface.SelectTaskD(conn, statusStruct.taskId.ToString(), "STACK");
                    if (dr != null)
                    {
                        if (dr != null && dr["row_num"].ToString() == nChannelNo.ToString() && dr["task_id"].ToString() == statusStruct.taskId.ToString())
                        {
                            if (DataBaseInterface.TaskStatusUpdate(statusStruct.taskId, deviceId, deviceName, int.Parse(dr["step"].ToString()), 2, out rs) != 1)
                                MessageBox.Show(statusStruct.taskId.ToString() + " 任务报完成错误!  " + rs);
                        }
                        int goodsKinds = int.Parse(dr["goods_kind"].ToString());
                        int toChannel = int.Parse(dr["TO_UNIT"].ToString().Substring(0, 2));
                        int toRow = int.Parse(dr["TO_UNIT"].ToString().Substring(2, 3));
                        int toLevel = int.Parse(dr["TO_UNIT"].ToString().Substring(8, 3));
                        int toColumn = int.Parse(dr["TO_UNIT"].ToString().Substring(5, 3));
                        if (int.Parse(dr["step"].ToString()) == 1 && int.Parse(dr["task_type"].ToString()) == 2)// 如果子任务为出库任务 ,则给出库辊道下任务
                        {
                            Thread.Sleep(1000);
                            if (!IsSendOut(statusStruct.taskId))
                            {
                                isSendOut = false;
                                if (goodsKinds == 1)//吨桶出库
                                {
                                    if (frmMain.outConveyorCmd.returnStruct[0].taskID != statusStruct.taskId)
                                    {
                                        exit_electricsConveyor_count++;
                                        if (exit_electricsConveyor_count >= 30)
                                        {
                                            exit_electricsConveyor_count = 0;
                                            Thread.Sleep(1000);
                                            Form_EC_Task_issued fet = new Form_EC_Task_issued(this);
                                            fet.ShowDialog();

                                        }
                                        for (int i = 0; i < 50; i++)
                                        {
                                            if (frmMain.outConveyorCmd.returnStruct[0].status == 10)
                                            {
                                                if (DownloadWareTask(nChannelNo, toRow, toLevel, statusStruct.taskId))
                                                {
                                                    isSendOut = true;

                                                    i++;
                                            Thread.Sleep(500);
                                            continue;
                                                }
                                            }
                                        }
                                        //else
                                        //{
                                        //    if (frmMain.outConveyorCmd.returnStruct[0].status != 10)
                                        //        MessageBox.Show(nChannelNo.ToString() + "巷道" + statusStruct.taskId.ToString() + "号出库任务已完成，但出库辊道不允许下任务！！");
                                        //}
                                    }
                                }
                                else //圆桶或空托
                                {
                                    if ((goodsKinds == 2 || goodsKinds==4 )&& nChannelNo == 1) //一巷道圆桶出库
                                    {

                                        if (frmMain.allElectricsConveyor.returnStruct[0].taskID != statusStruct.taskId)
                                        {
                                            exit_electricsConveyor_count++;
                                            if (exit_electricsConveyor_count >= 30)
                                            {
                                                exit_electricsConveyor_count = 0;
                                                Thread.Sleep(1000);
                                                Form_EC_Task_issued fet = new Form_EC_Task_issued(this);
                                                fet.ShowDialog();

                                            }
                                            if (frmMain.allElectricsConveyor.returnStruct[0].status == 10)
                                            {

                                                if (DownloadWareTask(nChannelNo, toRow, toLevel, statusStruct.taskId))
                                                {
                                                    isSendOut = true;
                                                }
                                            }
                                            else
                                            {
                                                if (frmMain.allElectricsConveyor.returnStruct[0].status != 10)
                                                    MessageBox.Show(nChannelNo.ToString() + "巷道" + statusStruct.taskId.ToString() + "号出库任务已完成，但出库辊道不允许下任务！！");
                                            }
                                        }
                                    }
                                    else
                                    {

                                        if (frmMain.outElectricsConveyor.returnStruct[toChannel-1].taskID != statusStruct.taskId)
                                        {
                                            exit_electricsConveyor_count++;
                                            if (exit_electricsConveyor_count >= 30)
                                            {
                                                exit_electricsConveyor_count = 0;
                                                Thread.Sleep(1000);
                                                Form_EC_Task_issued fet = new Form_EC_Task_issued(this);
                                                fet.ShowDialog();

                                            }

                                                for (int i = 0; i < 50; i++)
                                                {
                                                    if (frmMain.outElectricsConveyor.returnStruct[toChannel - 1].status == 10)
                                                    {
                                                    if (DownloadWareTask(nChannelNo, toRow, toLevel, statusStruct.taskId))
                                                    {
                                                        isSendOut = true;

                                                    }
                                                    }
                                                i++;
                                                Thread.Sleep(500);
                                                continue;
                                            }
                                               
                                            
                                         
                                            //else
                                            //{
                                            //    if (frmMain.outElectricsConveyor.returnStruct[toChannel - 1].status != 10)
                                            //        MessageBox.Show(nChannelNo.ToString() + "巷道" + statusStruct.taskId.ToString() + "号出库任务已完成，但出库辊道不允许下任务！！");
                                            //}
                                        }
                                    }

                                }

                            }

                        }
                        if ((int.Parse(dr["step"].ToString()) == 1 && int.Parse(dr["task_type"].ToString()) == 4) || (int.Parse(dr["step"].ToString()) == 2 && int.Parse(dr["task_type"].ToString()) == 5 && toColumn == 0))// 如果子任务为退库或第二步到入库动力站台的异常回库任务 ,则给一楼出入库动力站台辊道下任务
                        {
                            Thread.Sleep(1000);
                            if (!IsSendOut(statusStruct.taskId))
                            {
                                isSendOut = false;


                                if (frmMain.inElectricsConveyor.returnStruct[toChannel - 1].taskID != statusStruct.taskId)
                                {
                                    exit_electricsConveyor_count++;
                                    if (exit_electricsConveyor_count >= 30)
                                    {
                                        //dyw Update
                                        exit_electricsConveyor_count = 0;
                                        Thread.Sleep(1000);
                                        Form_EC_Task_issued fet = new Form_EC_Task_issued(this);
                                        fet.ShowDialog();

                                    }
                                    if (frmMain.inElectricsConveyor.returnStruct[toChannel - 1].status == 10)
                                    {
                                        if (DownloadWareTask(nChannelNo, toRow, toLevel, statusStruct.taskId))
                                        {
                                            isSendOut = true;
                                        }
                                    }
                                    else
                                    {
                                        if (frmMain.inElectricsConveyor.returnStruct[toChannel - 1].status != 10)
                                            MessageBox.Show(nChannelNo.ToString() + "巷道" + statusStruct.taskId.ToString() + "号出库任务已完成，但出库辊道不允许下任务！！");
                                    }
                                }
                            }
                        }
                    }
                }
                else if (statusStruct.workMode == 1 && (statusStruct.taskStep != 5 && statusStruct.taskStep != 0) && statusStruct.taskState == 1 && pcWorkMode == 1)
                {
                    DataRow dr1 = DataBaseInterface.SelectTaskD(conn, statusStruct.taskId.ToString(), "STACK");
                    if (dr1 != null && DataBaseInterface.TaskStatusUpdate(int.Parse(dr1["TASK_ID"].ToString()), int.Parse(lb.Tag.ToString()), lb.Name, int.Parse(dr1["step"].ToString()), 1, out rs) != 1)
                    {
                        MessageBox.Show(statusStruct.taskId.ToString() + " 任务报执行错误!  ");
                    }

                }
                //堆垛机允许下发任务，联机就可以下命令
                //数据库任务有三种状态 1新生成，时可下发新指令 1时可报完成    2完成 
                if (statusStruct.workMode == 1 && pcWorkMode == 1 && (statusStruct.taskStep == 0 || statusStruct.taskStep == 5) && statusStruct.taskState == 0 && HaveExcuteTask(nChannelNo) && isSendOut)
                {
                    GetStackTask(nChannelNo);
                }
                if (statusStruct.workMode == 1 && pcWorkMode == 0 && connStatus && isBindPLC && (((statusStruct.taskStep == 0 || statusStruct.taskStep == 5) && statusStruct.taskState == 0) || (manualCmdStruct.cmd == 8 || manualCmdStruct.cmd == 7)) && manualStatus == 1)
                {
                    WriteCmdData(CreateManualCmdString());
                    manualStatus = 0;
                }
            }


        }

        public bool IsSendOut(int taskid)
        {
            using (MySqlConnection conn = frmMain.dbConn.GetConnectFromPool())
            {
                if (conn == null)
                {
                    MessageBox.Show("ISSENDOUTCONN");
                    return true;
                }
                DataSet ds = new DataSet();
                try
                {
                    string strSQL = string.Empty;
                    strSQL = "select count(*) from tb_plt_task_d where step =(select t.step+1 from tb_plt_task_d t where t.device_type='STACK' and t.task_id=" + taskid + ") and status>0 and task_id=" + taskid + "";
                    ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                }
                catch
                {
                    throw;
                }
                if (int.Parse(ds.Tables[0].Rows[0]["count(*)"].ToString()) == 1)
                    return true;
                else
                    return false;
            }
        }
        public void ClearManualCMdStruct()
        {
            manualCmdStruct.downloadStatus = 0;
            manualCmdStruct.cmd = 0;
            manualCmdStruct.taskType = 1;
            manualCmdStruct.fromRow = 0;
            manualCmdStruct.fromBay = 0;
            manualCmdStruct.fromLevel = 0;
            manualCmdStruct.toRow = 0;
            manualCmdStruct.toBay = 0;
            manualCmdStruct.toLevel = 0;
            manualCmdStruct.taskId = 0;

        }
        public void ClearCmdStruct()
        {
            cmdStruct.downloadStatus = 0;
            cmdStruct.cmd = 0;
            cmdStruct.taskType = 1;
            cmdStruct.fromRow = 0;
            cmdStruct.fromBay = 0;
            cmdStruct.fromLevel = 0;
            cmdStruct.toRow = 0;
            cmdStruct.toBay = 0;
            cmdStruct.toLevel = 0;
            cmdStruct.taskId = 0;
        }
        /// <summary>
        /// 获取设备接口地址
        /// </summary>
        /// <returns></returns>
        public bool GetRowBayLay(string interfaceDevice, out int row, out int bay, out int lay)
        {
            row = 0;
            bay = 0;
            lay = 0;
            //string addr;
            //if (frmMain.deviceIntf.ContainsKey(deviceId + ',' + interfaceDevice))
            //    addr = frmMain.deviceIntf[deviceId + ',' + interfaceDevice].ToString();
            //else
            //    addr = interfaceDevice;
            //if (addr.Length == 11)  //WH020500101
            //{

            //    //row = (int.Parse(addr.Substring(4, 2))+1)%2 + 1;
            //    row = int.Parse(addr.Substring(4, 2));
            //    bay = int.Parse(addr.Substring(6, 3));
            //    lay = int.Parse(addr.Substring(9, 2));
            //    return true;
            //}
            return false;
        }
        /// <summary>
        /// 设置上位自动
        /// </summary>
        /// <returns></returns>
        public bool SetOnLine()
        {
            pcWorkMode = 1;//自动
            return false;
        }
        /// <summary>
        /// 设置上位手动
        /// </summary>
        /// <returns></returns>
        public bool SetOffLine()
        {
            pcWorkMode = 0;//手动
            return false;
        }
        /// <summary>
        /// 将自动命令结构体转为字符串
        /// </summary>
        /// <returns></returns>
        private string CreateAutoCmdString()
        {
            string str = "{";
            str += cmdStruct.downloadStatus.ToString() + "|";
            str += cmdStruct.cmd.ToString() + "|";
            str += cmdStruct.taskType.ToString() + "|";
            str += ((cmdStruct.fromRow == 0) ? "0" : ((cmdStruct.fromRow % 2) == 0) ? "2" : "1").ToString() + "|";
            str += cmdStruct.fromBay.ToString() + "|";
            str += cmdStruct.fromLevel.ToString() + "|";
            str += ((cmdStruct.toRow == 0) ? "0" : ((cmdStruct.toRow % 2) == 0) ? "2" : "1").ToString() + "|";
            str += cmdStruct.toBay.ToString() + "|";
            str += cmdStruct.toLevel.ToString() + "|";
            str += (cmdStruct.taskId / 0x010000).ToString() + "|";
            str += (cmdStruct.taskId % 0x010000).ToString() + "}";
            return str;
        }

        /// <summary>
        /// 将手动命令结构体转为字符串
        /// </summary>
        /// <returns></returns>
        public string CreateManualCmdString()
        {
            string str = "{";
            str += manualCmdStruct.downloadStatus.ToString() + "|";
            str += manualCmdStruct.cmd.ToString() + "|";
            str += manualCmdStruct.taskType.ToString() + "|";
            str += ((manualCmdStruct.fromRow == 0) ? "0" : ((manualCmdStruct.fromRow % 2) == 0) ? "2" : "1").ToString() + "|";
            str += manualCmdStruct.fromBay.ToString() + "|";
            str += manualCmdStruct.fromLevel.ToString() + "|";
            str += ((manualCmdStruct.toRow == 0) ? "0" : ((manualCmdStruct.toRow % 2) == 0) ? "2" : "1").ToString() + "|";
            str += manualCmdStruct.toBay.ToString() + "|";
            str += manualCmdStruct.toLevel.ToString() + "|";
            str += (manualCmdStruct.taskId / 0x010000).ToString() + "|";
            str += (manualCmdStruct.taskId % 0x010000).ToString() + "}";
            return str;
        }

        /// <summary>
        /// 向堆垛写指令数据
        /// </summary>
        public void WriteCmdData(string str)
        {
            try
            {
                string[] writeValues = new string[1];
                writeValues[0] = str;
                SyncWrite(writeValues, cmdStruct.cmdItemHandle);
                WriteLogFile("写任务" + writeValues[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        /// <summary>
        /// 将读到的状态串拆解到状态结构体中
        /// </summary>
        /// <param name="readValues"></param>
        private void DecodePLCStatus(string[] returnReadValues)
        {
            string[] strDB;
            strDB = returnReadValues[0].Substring(1, returnReadValues[0].Length - 2).Split('|');
            statusStruct.workMode = Int32.Parse(strDB[0].ToString());
            statusStruct.taskState = Int32.Parse(strDB[1].ToString());
            statusStruct.taskStep = Int32.Parse(strDB[2].ToString());
            statusStruct.loadStatus = Int32.Parse(strDB[3].ToString());
            statusStruct.heartBeat = Int32.Parse(strDB[4].ToString());
            statusStruct.taskId = Int32.Parse(strDB[5].ToString()) * 0X010000 + Int32.Parse(strDB[6].ToString());
            statusStruct.currentBay = Int32.Parse(strDB[7].ToString());
            statusStruct.currentLevel = Int32.Parse(strDB[8].ToString());
            statusStruct.currentX = Int32.Parse(strDB[9].ToString()) * 0X010000 + Int32.Parse(strDB[10].ToString());
            statusStruct.currentY = Int32.Parse(strDB[11].ToString()) * 0X010000 + Int32.Parse(strDB[12].ToString());
            statusStruct.currentZ = Int32.Parse(strDB[13].ToString()) * 0X010000 + Int32.Parse(strDB[14].ToString());
            statusStruct.errorCode1 = Int32.Parse(strDB[15].ToString());
            statusStruct.errorCode2 = Int32.Parse(strDB[16].ToString());
            statusStruct.errorCode3 = Int32.Parse(strDB[17].ToString());
            statusStruct.errorCode4 = Int32.Parse(strDB[18].ToString());
            statusStruct.errorCode5 = Int32.Parse(strDB[19].ToString());
            statusStruct.errorCode6 = Int32.Parse(strDB[20].ToString());
        }

        /// <summary>
        /// 将读到的状态串拆解到状态结构体中
        /// </summary>
        /// <param name="readValues"></param>
        private void DecodePLCCmd(string[] readValues)
        {
            string[] strDB;
            strDB = readValues[0].Substring(1, readValues[0].Length - 2).Split('|');
            plcCmdStruct.downloadStatus = Int32.Parse(strDB[0].ToString());
            plcCmdStruct.cmd = Int32.Parse(strDB[1].ToString());
            plcCmdStruct.taskType = Int32.Parse(strDB[2].ToString());
            plcCmdStruct.fromRow = Int32.Parse(strDB[3].ToString());
            plcCmdStruct.fromBay = Int32.Parse(strDB[4].ToString());
            plcCmdStruct.fromLevel = Int32.Parse(strDB[5].ToString());
            plcCmdStruct.toRow = Int32.Parse(strDB[6].ToString());
            plcCmdStruct.toBay = Int32.Parse(strDB[7].ToString());
            plcCmdStruct.toLevel = Int32.Parse(strDB[8].ToString());
            plcCmdStruct.taskId = Int32.Parse(strDB[9].ToString()) * 0X010000 + Int32.Parse(strDB[10].ToString());
        }

        #region 错误代码识别
        public string ErrorCode()
        {
            string errorMessage = string.Empty;
            for (int i = 0; i < 16; i++)
            {
                int j = (int)Math.Pow(2, i);
                if ((statusStruct.errorCode1 & (0x01 * j)) / (0x01 * j) == 1)
                {
                    errorMessage += errorCode1Select(i) + ";";
                }
            }
            for (int i = 0; i < 16; i++)
            {
                int j = (int)Math.Pow(2, i);
                if ((statusStruct.errorCode2 & (0x01 * j)) / (0x01 * j) == 1)
                {
                    errorMessage += errorCode2Select(i) + ";";
                }
            }
            for (int i = 0; i < 16; i++)
            {
                int j = (int)Math.Pow(2, i);
                if ((statusStruct.errorCode3 & (0x01 * j)) / (0x01 * j) == 1)
                {
                    errorMessage += errorCode3Select(i) + ";";
                }
            }
            for (int i = 0; i < 16; i++)
            {
                int j = (int)Math.Pow(2, i);
                if ((statusStruct.errorCode4 & (0x01 * j)) / (0x01 * j) == 1)
                {
                    errorMessage += errorCode4Select(i) + ";";
                }
            }
            for (int i = 0; i < 16; i++)
            {
                int j = (int)Math.Pow(2, i);
                if ((statusStruct.errorCode5 & (0x01 * j)) / (0x01 * j) == 1)
                {
                    errorMessage += errorCode5Select(i) + ";";
                }
            }
            for (int i = 0; i < 16; i++)
            {
                int j = (int)Math.Pow(2, i);
                if ((statusStruct.errorCode6 & (0x01 * j)) / (0x01 * j) == 1)
                {
                    errorMessage += errorCode6Select(i) + ";";
                }
            }
            return (errorMessage == string.Empty) ? "正常" : errorMessage;
        }
        #endregion

        /// <summary>
        /// 绑定PLC
        /// </summary>
        /// <returns></returns>
        public bool BindPLC()
        {
            isBindPLC = false;
            OpcRcw.Da.OPCITEMDEF[] items = new OPCITEMDEF[1];
            items[0].szAccessPath = "";
            items[0].bActive = 1;
            items[0].dwBlobSize = 1;
            items[0].pBlob = IntPtr.Zero;

            // 同步通讯绑定，堆垛机采用同步通讯
            if (!SyncAddGroup()) return false;
            items[0].hClient = 1;
            items[0].vtRequestedDataType = (int)VarEnum.VT_BSTR;
            items[0].szItemID = "S7:[S7 connection_1]" + cmdStruct.cmdDB;
            if (!SyncAddItems(items, cmdStruct.cmdItemHandle)) return false;
            items[0].hClient = 2;
            items[0].vtRequestedDataType = (int)VarEnum.VT_BSTR;
            items[0].szItemID = "S7:[S7 connection_1]" + statusStruct.returnDB;
            if (!SyncAddItems(items, statusStruct.returnItemHandle)) return false;

            isBindPLC = true;
            return isBindPLC;
        }
        #region 错误代码
        public string errorCode1Select(int errorCode)
        {
            switch (errorCode)
            {
                case 8: return "前限位故障";
                case 9: return "后限位故障";
                case 10: return "前软限位故障";
                case 11: return "后软限位故障";
                case 12: return "水平目标位置错误";
                case 13: return "行走变频报警";
                case 14: return "水平定位超时";
                case 15: return "水平测距出错";
                case 0: return "水平未对准禁止伸叉";
                case 1: return "行走限速报警";
                default: return "1未知错误";
            }
        }
        public string errorCode2Select(int errorCode)
        {
            switch (errorCode)
            {
                case 8: return "上限位故障";
                case 9: return "下限位故障";
                case 10: return "上软限位故障";
                case 11: return "下软限位故障";
                case 12: return "升降目标位置错误";
                case 13: return "升降变频报警";
                case 14: return "升降定位超时";
                case 15: return "垂直测距出错";
                case 0: return "松绳故障";
                case 1: return "失速报警";
                case 2: return "超重故障";
                case 3: return "升降未对准禁止伸叉";
                case 4: return "升降限速报警";
                case 5: return "安全钳松绳报警";
                default: return "2未知错误";
            }
        }
        public string errorCode3Select(int errorCode)
        {
            switch (errorCode)
            {
                case 8: return "取货时货叉有货";
                case 9: return "取货后货叉无货";
                case 10: return "放货时货架有货_近伸";
                case 11: return "放货后货叉有货";
                case 12: return "货物左超宽";
                case 13: return "货物右超宽";
                case 14: return "货物前超长";
                case 15: return "货物后超长";
                case 0: return "货物超高1";
                case 1: return "货物超高2";
                case 2: return "货叉不居中";
                case 3: return "远伸取货近伸有货";
                case 4: return "远伸放货近伸有货";
                case 5: return "货叉变频报警";
                case 6: return "货叉中位光电异常";
                default: return "3未知错误";
            }
        }
        public string errorCode4Select(int errorCode)
        {
            switch (errorCode)
            {
                case 8: return "急停";
                case 10: return "WCS源地址错误";
                case 11: return "WCS目地址错误";
                case 12: return "屏源地址错误";
                case 13: return "屏目地址错误";
                case 15: return "货叉1编码器故障";
                case 0: return "货叉2编码器故障";
                case 1: return "货叉1伸叉超时";
                case 2: return "货叉2伸叉超时";
                default: return "4未知错误";
            }
        }
        public string errorCode5Select(int errorCode)
        {
            switch (errorCode)
            {
                default: return "5未知错误";
            }
        }
        public string errorCode6Select(int errorCode)
        {
            switch (errorCode)
            {
                default: return "6未知错误";
            }
        }

        #endregion


        /// <summary>
        /// 初始化描述字典
        /// </summary>
        private void InitDic()
        {
            cmdDic.Add("0", "无命令");
            cmdDic.Add("1", "取货行走");
            cmdDic.Add("2", "取货");
            cmdDic.Add("3", "放货行走");
            cmdDic.Add("4", "放货");
            cmdDic.Add("5", "取放");
            cmdDic.Add("6", "召回");
            cmdDic.Add("7", "初始化");
            cmdDic.Add("8", "清错");


            taskStatusDic.Add("0", "空闲");
            taskStatusDic.Add("1", "无货行走");
            taskStatusDic.Add("2", "取货");
            taskStatusDic.Add("3", "放货行走");
            taskStatusDic.Add("4", "放货");
            taskStatusDic.Add("5", "完成");

            forkStatusDic.Add("0", "货叉无货");
            forkStatusDic.Add("1", "货叉有货");

            workModeStatusDic.Add("0", "未知");
            workModeStatusDic.Add("1", "自动");
            workModeStatusDic.Add("2", "半自动");
            workModeStatusDic.Add("3", "手动");
            workModeStatusDic.Add("4", "应急维修");
        }
        /// <summary>
        /// 写日志文件,有变化才写
        /// </summary>
        /// <param name="textType"></param>
        /// <param name="data"></param>
        private void WriteLogFile(string logText)
        {
            //if (lastLogText == logText) return;
            //string str = DateTime.Now.ToLongTimeString() + logText+"\r\n";
            //byte[] aa = Encoding.Default.GetBytes(str);
            //fs.Write(aa, 0, aa.Length);
            //lastLogText = logText;
            //fs.Flush();
        }

        #region 定义变量和类型
        /// <summary>
        /// 手动任务的状态,0没有手工任务1存在未下发的手工任务
        /// </summary>
        public Int32 manualStatus = 0;
        /// <summary>
        /// 手动任务的任务号1...200
        /// </summary>
        public Int32 manualTaskId = 1;

        /// <summary>
        /// PLC报告给上位机的状态数据结构
        /// </summary>
        public struct StatusStruct
        {
            /// <summary>
            /// 操作方式 1自动 2半自动 3手动 4维修应急 
            /// </summary>
            public Int32 workMode;
            /// <summary>
            /// 任务状态，0空闲 1正在执行 其他为故障
            /// </summary>
            public Int32 taskState;
            /// <summary>
            /// 执行步骤 0空闲；1取货行走；2取货；3送货行走；4放货；5完成；
            /// </summary>
            public Int32 taskStep;
            /// <summary>
            /// 载货状态 0都无货，1有货
            /// </summary>
            public Int32 loadStatus;
            /// <summary>
            /// 心跳
            /// </summary>
            public Int32 heartBeat;
            /// <summary>
            /// 任务号  1-99为手动任务号 2000以上为自动任务号 100-1999 为异常回库任务号
            /// </summary>
            public Int32 taskId;
            /// <summary>
            /// 当前列
            /// </summary>
            public Int32 currentBay;
            /// <summary>
            /// 当前层
            /// </summary>
            public Int32 currentLevel;
            /// <summary>
            /// 当前水平位置
            /// </summary>
            public Int32 currentX;
            /// <summary>
            /// 当前垂直位置
            /// </summary>
            public Int32 currentY;
            /// <summary>
            /// 当前货叉位置
            /// </summary>
            public Int32 currentZ;
            /// <summary>
            /// 报警代码1
            /// </summary>
            public Int32 errorCode1;
            /// <summary>
            /// 报警代码2
            /// </summary>
            public Int32 errorCode2;
            /// <summary>
            /// 报警代码3
            /// </summary>
            public Int32 errorCode3;
            /// <summary>
            /// 报警代码4
            /// </summary>
            public Int32 errorCode4;
            /// <summary>
            /// 报警代码5
            /// </summary>
            public Int32 errorCode5;
            /// <summary>
            /// 报警代码6
            /// </summary>
            public Int32 errorCode6;
            /// <summary>
            /// 当前巷道
            /// </summary>
            public Int32 tunnel;
            //上面是状态块数据
            /// <summary>
            /// PLC对应的状态数据块
            /// </summary>
            public String returnDB;
            /// <summary>
            /// 状态handle
            /// </summary>
            public int[] returnItemHandle;
            /// <summary>
            /// 上次读到的心跳信号
            /// </summary>
            public int lastHeartBeat;
            /// <summary>
            /// 上次读到的心跳变化时间
            /// </summary>
            public long lastHeartBeatTime;

        }
        /// <summary>
        /// PC写给PLC命令的数据结构
        /// </summary>
        public struct CmdStruct
        {
            /// <summary>
            /// 下达完成 1-下达完成
            /// </summary>
            public Int32 downloadStatus;
            /// <summary>
            /// 命令方式 1无货行走；2有货行走 3取货 4放货 5取放 6召回 7清错
            /// </summary>
            public Int32 cmd;
            /// <summary>
            /// 任务类型
            /// </summary>
            public Int32 taskType;
            /// <summary>
            /// 起始排；
            /// </summary>
            public Int32 fromRow;
            /// <summary>
            /// 起始列1 0为站台列
            /// </summary>
            public Int32 fromBay;
            /// <summary>
            /// 起始层1；YCH1
            /// </summary>
            public Int32 fromLevel;
            /// <summary>
            /// 目的排1 MBPH1
            /// </summary>
            public Int32 toRow;
            /// <summary>
            /// 目的列1 MBLH1
            /// </summary>
            public Int32 toBay;
            /// <summary>
            /// 目的层1 MBCH1
            /// </summary>
            public Int32 toLevel;
            /// <summary>
            /// 任务号
            /// </summary>
            public Int32 taskId;
            /// <summary>
            /// PLC对应的命令数据块,
            /// </summary>
            public String cmdDB;
            /// <summary>
            /// 命令handle
            /// </summary>
            public int[] cmdItemHandle;
        }
        /// <summary>
        /// 定义命令描述字典
        /// </summary>
        public Dictionary<string, string> cmdDic = new Dictionary<string, string>();
        /// <summary>
        /// 定义货叉状态字典
        /// </summary>
        public Dictionary<string, string> forkStatusDic = new Dictionary<string, string>();
        /// <summary>
        /// 定义任务状态字典
        /// </summary>
        public Dictionary<string, string> taskStatusDic = new Dictionary<string, string>();
        public Dictionary<string, string> workModeStatusDic = new Dictionary<string, string>();
        private string lastLogText = string.Empty;

        #endregion
        public void GetStackTask(int nDeviceid)
        {
            string strSql = "";
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                try
                {
                    DataSet ds = new DataSet();
                    if (frmMain.taskType[0] == 4 || frmMain.taskType[1] == 4)//异常回库
                    {
                        //先找入库堆垛机任务
                        strSql = "select * from tb_plt_task_d  where device_type='STACK' and row_num=" + nDeviceid.ToString() + " and task_type= 1  and status =0 order by create_time";
                        ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSql);
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            //是否存在未完成的入库辊道任务
                            strSql = "select t.* from tb_plt_task_m t  where t.task_status < 2 and (t.task_type = 1 or t.task_type = 3) order by create_time";
                            ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSql);
                            if (ds.Tables[0].Rows.Count == 0)
                            {
                                //确保没有入库任务之后再进行异常入库任务
                                strSql = "select * from tb_plt_task_d  where device_type='STACK' and row_num=" + nDeviceid.ToString() + " and task_type= 5  and status =0 order by create_time";
                                ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSql);
                                if (ds.Tables[0].Rows.Count == 0)
                                {
                                    return;
                                }
                            }
                        }
                    }
                    else if (frmMain.taskType[0] == 3)//退库
                    {
                        strSql = "select * from tb_plt_task_d  where device_type='STACK' and row_num=" + nDeviceid.ToString() + " and task_type= 4  and status =0 order by create_time";
                        ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSql);
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            return;
                        }
                    }
                    else if (frmMain.taskType[0] == 2 || frmMain.taskType[1] == 2)//空托盘入库
                    {
                        strSql = "select * from tb_plt_task_d  where device_type='STACK' and row_num=" + nDeviceid.ToString() + " and task_type= 3  and status =0 order by create_time";
                        ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSql);
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            return;
                        }
                    }
                    else if (frmMain.taskType[0] == 1 || frmMain.taskType[1] == 1)
                    {

                        //找普通入库任务
                        strSql = "select * from tb_plt_task_d  where device_type='STACK' and row_num=" + nDeviceid.ToString() + " and task_type = 1 and status =0 order by create_time";
                        ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSql);
                        if (ds.Tables[0].Rows.Count < 3 && ds.Tables[0].Rows.Count > 0)
                        {
                            //找空托盘出库
                            strSql = "select t.*, m.* from tb_plt_task_d t,tb_plt_task_m m where t.task_id = m.task_id and t.status = 0 and t.step = 1 and t.task_type = 2 and m.task_type = 2 and m.task_status = 0 and m.goods_kind = 3";
                            DataSet ds1 = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSql);
                            if (ds1.Tables[0].Rows.Count != 0 && nChannelNo == 1)
                            {
                                ds = ds1;

                            }
                        }
                        else
                        {
                            string strSql1 = "select * from tb_plt_task_d  where device_type='STACK' and row_num=" + nDeviceid.ToString() + " and task_type = 1 and status < 2 order by create_time";
                            if (DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSql).Tables[0].Rows.Count < 3)
                            {
                                strSql = "select * from tb_plt_task_d  where device_type='STACK' and row_num=" + nDeviceid.ToString() + " and task_type = 2 and status =0 order by create_time";
                                DataSet ds1 = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSql);
                                if (ds1.Tables[0].Rows.Count == 0 && ds.Tables[0].Rows.Count == 0)
                                {
                                    return;
                                }
                                else if (ds1.Tables[0].Rows.Count == 0)
                                    taskTypeFlag = 2;
                                else if (ds.Tables[0].Rows.Count == 0)
                                {
                                    if (nChannelNo == 1)
                                    {
                                        if (frmMain.allElectricsConveyor.IsHaveGoods(1001, 2).loadType == 0)
                                        {
                                            ds = ds1;
                                            taskTypeFlag = 1;
                                        }
                                        else
                                            taskTypeFlag = 2;
                                    }
                                    else if (nChannelNo == 2)
                                    {
                                        if (frmMain.outElectricsConveyor.canGetUp(nDeviceid) == 1)
                                        {
                                            ds = ds1;
                                            taskTypeFlag = 1;
                                        }
                                        else
                                            taskTypeFlag = 2;
                                    }
                                    else if (nChannelNo == 3)
                                    {
                                        if (frmMain.outConveyorCmd.OutBoundBoxCode() == null)
                                        {
                                            ds = ds1;
                                            taskTypeFlag = 1;
                                        }
                                        else
                                            taskTypeFlag = 2;

                                    }
                                }
                                else if (taskTypeFlag == 1)
                                    taskTypeFlag = 2;
                                else if (taskTypeFlag == 2)
                                {
                                    if (nChannelNo == 1)
                                    {
                                        if (frmMain.allElectricsConveyor.IsHaveGoods(1001, 1).loadType == 0)
                                        {
                                            ds = ds1;
                                            taskTypeFlag = 1;
                                        }
                                        else
                                            taskTypeFlag = 2;
                                    }
                                    else if (nChannelNo == 2)
                                    {
                                        if (frmMain.outElectricsConveyor.canGetUp(nDeviceid) == 1)
                                        {
                                            ds = ds1;
                                            taskTypeFlag = 1;
                                        }
                                        else
                                            taskTypeFlag = 2;
                                    }
                                    else if (nChannelNo == 3)
                                    {
                                        if (frmMain.outConveyorCmd.OutBoundBoxCode() == null)
                                        {
                                            ds = ds1;
                                            taskTypeFlag = 1;
                                        }
                                        else
                                            taskTypeFlag = 2;
                                    }
                                }
                            }

                        }
                    }
                    if (ds.Tables.Count != 0 && ds.Tables[0].Rows.Count != 0)
                    {
                        foreach (DataRow dr in ds.Tables[0].Rows)
                        {
                            if (int.Parse(dr["task_type"].ToString()) == 2 && dr["to_unit"].ToString() == "010020270021")//一巷道圆桶出库
                            {
                                if (frmMain.allElectricsConveyor.loadStruct[0].loadType != 0)
                                {
                                    continue;
                                }
                            }
                            else if (int.Parse(dr["task_type"].ToString()) == 2 && dr["to_unit"].ToString() == "030060270011")//3巷道吨桶出库
                            {
                                if (frmMain.outConveyorCmd.loadStruct[0].loadType != 0)
                                {
                                    continue;
                                }
                            }
                            else if (int.Parse(dr["task_type"].ToString()) == 2)
                            {
                                if (frmMain.outElectricsConveyor.loadStruct[int.Parse(dr["from_unit"].ToString().Substring(0, 2)) - 1].loadType != 0)
                                {
                                    continue;
                                }
                            }
                            else if (int.Parse(dr["task_type"].ToString()) == 1 || int.Parse(dr["task_type"].ToString()) == 3)
                            {
                                if (int.Parse(dr["from_unit"].ToString().Substring(10, 1)) == 1)
                                {
                                    if (frmMain.inElectricsConveyor.loadStruct[int.Parse(dr["from_unit"].ToString().Substring(0, 2)) - 1].taskID != int.Parse(dr["task_id"].ToString()))
                                    {
                                        continue;
                                    }
                                    
                                    //    if (frmMain.outElectricsConveyor.loadStruct[int.Parse(dr["from_unit"].ToString().Substring(0, 2)) - 1].taskID != int.Parse(dr["task_id"].ToString()))
                                    //{
                                    //    continue;
                                    //}

                                    if (!HaveunExcuteNewTask(this.deviceId, dr["task_type"].ToString(), frmMain.inElectricsConveyor.loadStruct[int.Parse(dr["from_unit"].ToString().Substring(0, 2)) - 1].taskID))//判断是否有生成时间更早但未下发的任务
                                    {
                                        continue;
                                    }
                                    //if (!HaveunExcuteNewTask(this.deviceId, dr["task_type"].ToString(), frmMain.outElectricsConveyor.loadStruct[int.Parse(dr["from_unit"].ToString().Substring(0, 2)) - 1].taskID))//判断是否有生成时间更早但未下发的任务
                                    //{
                                    //    continue;
                                    //}
                                }
                                else if (int.Parse(dr["from_unit"].ToString().Substring(10, 1)) == 4)
                                {
                                    if (frmMain.allElectricsConveyor.loadStruct[int.Parse(dr["from_unit"].ToString().Substring(0, 2)) - 1].taskID-1000 != int.Parse(dr["task_id"].ToString()))
                                    {
                                        continue;
                                    }

                                    if (!HaveunExcuteNewTask(this.deviceId, dr["task_type"].ToString(), frmMain.allElectricsConveyor.loadStruct[int.Parse(dr["from_unit"].ToString().Substring(0, 2)) - 1].taskID))//判断是否有生成时间更早但未下发的任务
                                    {
                                        continue;
                                    }
                                }
                                else
                                    continue;
                            }
                            else if ((int.Parse(dr["task_type"].ToString()) == 5 && int.Parse(dr["step"].ToString()) == 2 && frmMain.allElectricsConveyor.loadStruct[0].taskID-1000 != int.Parse(dr["task_id"].ToString())) || (int.Parse(dr["task_type"].ToString()) == 5 && int.Parse(dr["step"].ToString()) == 4 && frmMain.inElectricsConveyor.loadStruct[int.Parse(dr["from_unit"].ToString().Substring(0, 2)) - 1].taskID != int.Parse(dr["task_id"].ToString())))
                            {
                                continue;
                            }
                            if (!ToUnitHaveGoods(dr["to_unit"].ToString()))
                            {
                                cmdStruct.taskId = int.Parse(dr["task_id"].ToString());
                                cmdStruct.fromRow = (int.Parse(dr["from_unit"].ToString().Substring(2, 3)));
                                cmdStruct.fromBay = int.Parse(dr["from_unit"].ToString().Substring(5, 3));
                                cmdStruct.fromLevel = int.Parse(dr["from_unit"].ToString().Substring(8, 3));
                                cmdStruct.toRow = (int.Parse(dr["to_unit"].ToString().Substring(2, 3)));
                                cmdStruct.toBay = int.Parse(dr["to_unit"].ToString().Substring(5, 3));
                                cmdStruct.toLevel = int.Parse(dr["to_unit"].ToString().Substring(8, 3));
                                cmdStruct.taskType = 1;
                                cmdStruct.cmd = 5;
                                cmdStruct.downloadStatus = 1;
                                WriteCmdData(CreateAutoCmdString());
                            }

                            // DataBaseInterface.UpdateTaskStatus(cmdStruct.taskId1, "STACK");
                            break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return;
                }
            }
            return;
        }
        public bool DownloadWareTask(int nChannel, int row, int nLevel, int task_id)
        {
            string rs;
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return false;
                string strSql = "select t.*, m.goods_kind from tb_plt_task_d t,tb_plt_task_m m where t.task_id = m.task_id and t.status = 0 and t.task_id=" + task_id + " and t.device_type = 'CON' order by t.create_time";
                try
                {
                    DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSql);
                    if (ds.Tables[0].Rows.Count != 1)
                        MessageBox.Show("出库辊道下任务异常");
                    else
                    {
                        int fromadd = int.Parse(ds.Tables[0].Rows[0]["from_unit"].ToString());
                        int toadd = int.Parse(ds.Tables[0].Rows[0]["to_unit"].ToString()) == 1005 ? 1003 : int.Parse(ds.Tables[0].Rows[0]["to_unit"].ToString());
                        int goodsKind = int.Parse(ds.Tables[0].Rows[0]["goods_kind"].ToString());
                        int dTasktype = int.Parse(ds.Tables[0].Rows[0]["task_type"].ToString());
                        if (int.Parse(ds.Tables[0].Rows[0]["task_type"].ToString()) == 2)//出库
                        {
                            for (int i = 0; i <= 4; i++)
                            {
                                if (goodsKind == 1)//若为吨桶出库
                                {
                                    frmMain.outConveyorCmd.WriteCmd(0, task_id, dTasktype, fromadd, toadd, goodsKind);
                                    skOutLog.Write("巷道" + nChannelNo + "\t" + "层" + nLevel + "\t" + "任务号" + task_id + "\t" + "起始地址" + fromadd + "\t" + "目的地址" + toadd);
                                    Thread.Sleep(200);
                                    if (frmMain.outConveyorCmd.returnStruct[0].taskID == statusStruct.taskId)
                                    {
                                        DataBaseInterface.TaskStatusUpdate(statusStruct.taskId, deviceId, deviceName, int.Parse(ds.Tables[0].Rows[0]["step"].ToString()), 1, out rs);
                                        return true;
                                    }
                                }
                                else
                                {
                                    if ((goodsKind == 2 || goodsKind==4) && nChannel == 1)
                                    {
                                        frmMain.allElectricsConveyor.WriteCmd(nChannelNo - 1, task_id, dTasktype, fromadd, toadd, goodsKind);
                                        skOutLog.Write("巷道" + nChannelNo + "\t" + "层" + nLevel + "\t" + "任务号" + task_id + "\t" + "起始地址" + fromadd + "\t" + "目的地址" + toadd);
                                        Thread.Sleep(200);
                                        if (frmMain.allElectricsConveyor.returnStruct[nChannel - 1].taskID == statusStruct.taskId)
                                        {
                                            DataBaseInterface.TaskStatusUpdate(statusStruct.taskId, deviceId, deviceName, int.Parse(ds.Tables[0].Rows[0]["step"].ToString()), 1, out rs);
                                            return true;
                                        }
                                    }
                                    else
                                    {
                                        frmMain.outElectricsConveyor.WriteCmd(nChannelNo, task_id, dTasktype, fromadd, toadd, goodsKind);
                                        skOutLog.Write("巷道" + nChannelNo + "\t" + "层" + nLevel + "\t" + "任务号" + task_id + "\t" + "起始地址" + fromadd + "\t" + "目的地址" + toadd);
                                        Thread.Sleep(200);
                                        if (frmMain.outElectricsConveyor.returnStruct[nChannel - 1].taskID == statusStruct.taskId)
                                        {
                                            DataBaseInterface.TaskStatusUpdate(statusStruct.taskId, deviceId, deviceName, int.Parse(ds.Tables[0].Rows[0]["step"].ToString()), 1, out rs);
                                            return true;
                                        }

                                    }
                                }
                                
                                

                            }
                        }
                        if (int.Parse(ds.Tables[0].Rows[0]["task_type"].ToString()) == 4 || (int.Parse(ds.Tables[0].Rows[0]["step"].ToString()) == 3 && int.Parse(ds.Tables[0].Rows[0]["task_type"].ToString()) == 5))
                        {
                            for (int i = 0; i <= 4; i++)
                            {
                                frmMain.inElectricsConveyor.WriteCmd(nChannelNo - 1, task_id, dTasktype, fromadd, toadd, goodsKind);
                                skOutLog.Write("巷道" + nChannelNo + "\t" + "层" + nLevel + "\t" + "任务号" + task_id + "\t" + "起始地址" + fromadd + "\t" + "目的地址" + toadd);
                                Thread.Sleep(200);
                                if (frmMain.inElectricsConveyor.returnStruct[nChannel - 1].taskID == statusStruct.taskId)
                                {
                                    DataBaseInterface.TaskStatusUpdate(statusStruct.taskId, deviceId, deviceName, int.Parse(ds.Tables[0].Rows[0]["step"].ToString()), 1, out rs);
                                    return true;
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                return false;
            }
        }
        #region 断开PLC
        /// <summary>
        /// 断开PLC
        /// </summary>
        public void DisConnectPLC()
        {
            if (isBindPLC)
            {
                autoCmdThread.Abort();
                DisConnect();
                isBindPLC = false;
            }
        }
        #endregion

        public bool HaveExcuteTask(int nDeviceid)
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return false;
                string strSql = "select task_id,task_type,from_unit,to_unit from tb_plt_task_d  where device_type='STACK' and device_seq='" + nDeviceid + "' and status =1 order by create_time";
                DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSql);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }

        }

        public bool HaveunExcuteNewTask(int nDeviceid, string taskType, int task_id)
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return false;
                string strSql = "select * from tb_plt_task_d  where device_type='STACK' and device_seq=" + nDeviceid + " and status < 2 and task_type=" + taskType + " and task_id<" + task_id.ToString();
                DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSql);
                if (ds.Tables[0].Rows.Count == 0)
                {
                    return true;
                }
                else
                {
                    MessageBox.Show(nDeviceid.ToString() + "巷道存在多条新生成任务，请查实");
                    return false;
                }
            }

        }

        #region 堆垛机是否有放货任务
        public bool getUpTask()
        {
            if (cmdStruct.toRow == 1 && cmdStruct.toLevel == 1 && cmdStruct.toBay == 0 && (statusStruct.taskStep != 0 || statusStruct.taskStep != 5))
            {
                return true;
            }
            return false;
        }
        #endregion

        #region 判断放货地址是否有货

        public bool ToUnitHaveGoods(string toUnit)
        {
            switch (toUnit)
            {
                case "010010000011":
                    if (frmMain.conveyorLoad.IsHaveGoods(1001, 1).loadType > 0)
                        return true;
                    else return false;
                case "010020000011":
                    if (frmMain.inElectricsConveyor.IsHaveGoods(1024, 1).loadType > 0)
                        return true;
                    else return false;
                case "020040000011":
                    if (frmMain.inElectricsConveyor.IsHaveGoods(1022, 1).loadType > 0)
                        return true;
                    else return false;
                case "030060000011":
                    if (frmMain.inElectricsConveyor.IsHaveGoods(1020, 1).loadType > 0)
                        return true;
                    else return false;
                case "030060270011":
                    if (frmMain.outConveyorCmd.OutBoundBoxCode() != null)
                        return true;
                    else return false;
                case "010020270041":
                    if (frmMain.allElectricsConveyor.IsHaveGoods(1001, 2).loadType > 0)
                        return true;
                    else return false;
                case "020040270041":
                    if (frmMain.outElectricsConveyor.IsHaveGoods(1002, 2).loadType > 0)
                        return true;
                    else return false;
            }
            return false;
        }
        #endregion
    }
}
