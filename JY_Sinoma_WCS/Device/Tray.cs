using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DataBase;
using System.Windows.Forms;
using System.Drawing;
using System.Data;
using OpcRcw.Da;
using System.Runtime.InteropServices;
using System.Threading;
using MySql.Data.MySqlClient;
using PLC;
namespace JY_Sinoma_WCS
{
    /// <summary>
    /// 拆盘机控制
    /// </summary>
    public class Tray:OPCServer
    {
        #region 自定义变量
        public frmMain mainFrm;
        public ConnectPool dbConn;
        public SystemStatus systemstatus;
        public int nCount = 0;
        public int[] error;//辊道故障
        public int[] lastError;//辊道故障
        public string[] errorDB;//对应辊道故障的DB块地址
        public int[] errorHandle;//辊道故障的OPCServer的Handle
        public int[] errorClientHandle;

        public string[] controlDB;//对应辊道的控制DB块地址
        public int[] controlHandle;//辊道控制的OPCServer的Handle

        public string[] loadDB;//对应辊道的loadDB块地址
        public int[] loadHandle;//辊道控制的OPCServer的Handle

        public string[] returnDB;//对应辊道的loadDB块地址
        public int[] returnHandle;//辊道控制的OPCServer的Handle

        public string[] writeDB;//对应辊道的loadDB块地址
        public int[] writeHandle;//辊道控制的OPCServer的Handle
        public bool IsLoad = true;
        /// <summary>
        /// 设备错误详情
        /// </summary>
        public string[] deviceError;
        /// <summary>
        /// 原点位X
        /// </summary>
        public int nInitX;
        /// <summary>
        /// 原点位Y
        /// </summary>
        public int nInitY;
        /// <summary>
        /// 是否正在关闭
        /// </summary>
        public bool closing = false;
        /// <summary>
        /// 是否放托盘
        /// </summary>
        public bool running = false;
        /// <summary>
        /// 是否将托盘放置在右侧辊道
        /// </summary>
        public bool isRight = true;
        /// <summary>
        /// 拆盘机图标
        /// </summary>
        public Label[] lb;
        /// <summary>
        /// 是否已初始化OPCServer
        /// </summary>
        public bool isBindToPLC = false;
        /// <summary>
        /// 拆盘机名称
        /// </summary>
        public string[] nDeviceName;
        /// <summary>
        /// 是否正常生成出托盘任务
        /// </summary>
        public int nDownLoadTask = 0;
        /// <summary>
        /// load块数据结构体
        /// </summary>
        public struct LoadStruct
        {
            public int taskID;            //任务号
            public int taskType;          //任务类型
            public int from;              //起始地址
            public int to;                //目的地址
            public int loadType;          //货物类型 0-无信息 1-吨桶 2-圆桶 空托盘组 4-单个空托盘
        }
        /// <summary>
        /// TASK块读取数据结构体
        /// </summary>
        public struct TaskStruct
        {
            public int taskID;            //任务号
            public int taskType;          //任务类型
            public int from;              //起始地址
            public int to;                //目的地址
            public int loadType;          //货物类型 0-无信息 1-吨桶 2-圆桶 空托盘组 4-单个空托盘
        }
        /// <summary>
        /// RETURN块读取数据结构体
        /// </summary>
        public struct ReturnStruct
        {
            public int taskID;            //任务号
            public int taskType;          //任务类型
            public int from;              //起始地址
            public int to;                //目的地址
            public int status;            //辊道状态
        }
        public LoadStruct[] loadStruct;
        public TaskStruct[] taskStruct;
        public ReturnStruct[] returnStruct;
        public int taskid = 0;
        public Thread trayOutboundThread;
        public int[] levelNum;
        public int[] rowNum;
        public int[] systemStatusID;
        public int[] lastTaskId;
        /// <summary>
        /// 托盘放置位置
        /// </summary>
        public int dismantlerAbout = 1;

        /// <summary>
        /// 是否放托盘
        /// </summary>
        public bool playTray = false;
        /// 是否能下空托盘任务
        /// </summary>
        public bool empetTask = true;
        #endregion

        #region 构造函数
        public Tray(frmMain mainFrm,DataTable dt)
        {
            this.mainFrm = mainFrm;
            dbConn = mainFrm.dbConn;
            systemstatus = mainFrm.systemStatus;
            nCount = dt.Rows.Count;
            error = new int[dt.Rows.Count];
            lastError = new int[dt.Rows.Count];
            errorDB = new String[dt.Rows.Count];
            errorHandle = new int[dt.Rows.Count];
            errorClientHandle = new int[dt.Rows.Count];
            controlDB = new String[dt.Rows.Count];
            controlHandle = new int[dt.Rows.Count];
            nDeviceName = new string[dt.Rows.Count];
            lb = new Label[dt.Rows.Count];
            loadDB = new String[nCount];
            loadHandle = new int[nCount];
            returnDB = new String[nCount];
            returnHandle = new int[nCount];
            writeDB = new String[nCount];
            writeHandle = new int[nCount];
            deviceError = new string[nCount];
            loadStruct = new LoadStruct[nCount];
            taskStruct = new TaskStruct[nCount];
            returnStruct = new ReturnStruct[nCount];
            levelNum = new int[nCount];
            rowNum = new int[nCount];
            systemStatusID = new int[nCount];
            lastTaskId = new int[nCount];
            int i = 0;
            foreach (DataRow row in dt.Rows)
            {
               
                errorDB[i] = row["error_db"].ToString();
                controlDB[i] = row["control_db"].ToString();
                loadDB[i] = row["load_db"].ToString();
                returnDB[i] = row["return_db"].ToString();
                writeDB[i] = row["write_db"].ToString();
                nInitX = int.Parse("0" + row["x"].ToString());
                nInitY = int.Parse("0" + row["y"].ToString());
                nDeviceName[i] = row["device_name"].ToString();
                levelNum[i]=int.Parse(row["level_no"].ToString());
                rowNum[i] = int.Parse(row["row_no"].ToString());
                systemStatusID[i] = int.Parse(row["system_status_id"].ToString());
                lastTaskId[i] = 0;
                //定义图片
                lb[i] = new Label();
                lb[i].Size = new Size( int.Parse(row["length"].ToString()),int.Parse(row["width"].ToString()));
                lb[i].Location = new Point(int.Parse(row["x"].ToString()), int.Parse(row["y"].ToString()));
                lb[i].BackColor = Color.Gray;
                lb[i].Font = new Font("宋体", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                lb[i].TextAlign = ContentAlignment.MiddleCenter;
                lb[i].DoubleClick += new System.EventHandler(this.lb_DoubleClick);
                lb[i].Tag = i.ToString();
                mainFrm.PLevel1.Controls.Add(lb[i]);
                lb[i].BringToFront();
                i++;
            }

            ThreadExceptionDialog.CheckForIllegalCrossThreadCalls = false;//允许直接访问线程之间的控件
            this.mainFrm = mainFrm;
            dbConn = mainFrm.dbConn;

            #region 定义出库货位分配线程
            trayOutboundThread = new Thread(new ThreadStart(OutboundAssigning));
            trayOutboundThread.IsBackground = true;
            if (!trayOutboundThread.IsAlive)
                trayOutboundThread.Start();
            #endregion
        }
        #endregion

        #region 连接PLC
        /// <summary>
        /// 连接PLC
        /// </summary>
        /// <returns></returns>
        public bool BindToPLC()
        {
            if (!SyncAddGroup()) return false;
            if (!AsyncAddGroup()) return false;
            int client = 1;

            OpcRcw.Da.OPCITEMDEF[] errorItems = new OPCITEMDEF[errorDB.Length];
            for (int i = 0; i < errorDB.Length; i++)
            {
                errorItems[i].szAccessPath = "";
                errorItems[i].bActive = 1;
                errorItems[i].hClient = client;
                errorItems[i].dwBlobSize = 1;
                errorItems[i].pBlob = IntPtr.Zero;
                errorItems[i].vtRequestedDataType = (int)VarEnum.VT_I2;
                errorItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}", errorDB[i]);
                errorClientHandle[i] = client;
                client++;
            }
            if (!AsyncAddItems(errorItems, errorHandle)) return false;


            OpcRcw.Da.OPCITEMDEF[] controlItems = new OPCITEMDEF[controlDB.Length];
            for (int i = 0; i < controlDB.Length; i++)
            {
                controlItems[i].szAccessPath = "";
                controlItems[i].bActive = 1;
                controlItems[i].hClient = client;
                controlItems[i].dwBlobSize = 1;
                controlItems[i].pBlob = IntPtr.Zero;
                controlItems[i].vtRequestedDataType = (int)VarEnum.VT_I2;
                controlItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}", controlDB[i]);
                client++;
            }
            if (!SyncAddItems(controlItems, controlHandle)) return false;

            OpcRcw.Da.OPCITEMDEF[] loadItems = new OPCITEMDEF[loadDB.Length];
            for (int i = 0; i < loadDB.Length; i++)
            {
                loadItems[i].szAccessPath = "";
                loadItems[i].bActive = 1;
                loadItems[i].hClient = client;
                loadItems[i].dwBlobSize = 1;
                loadItems[i].pBlob = IntPtr.Zero;
                loadItems[i].vtRequestedDataType = (int)VarEnum.VT_BSTR;
                loadItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}", loadDB[i]);
                client++;
            }
            if (!SyncAddItems(loadItems, loadHandle)) return false;

            OpcRcw.Da.OPCITEMDEF[] returnItems = new OPCITEMDEF[returnDB.Length];
            for (int i = 0; i < returnDB.Length; i++)
            {
                returnItems[i].szAccessPath = "";
                returnItems[i].bActive = 1;
                returnItems[i].hClient = client;
                returnItems[i].dwBlobSize = 1;
                returnItems[i].pBlob = IntPtr.Zero;
                returnItems[i].vtRequestedDataType = (int)VarEnum.VT_BSTR;
                returnItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}", returnDB[i]);
                client++;
            }
            if (!SyncAddItems(returnItems, returnHandle)) return false;

            OpcRcw.Da.OPCITEMDEF[] writeItems = new OPCITEMDEF[writeDB.Length];
            for (int i = 0; i < writeDB.Length; i++)
            {
                writeItems[i].szAccessPath = "";
                writeItems[i].bActive = 1;
                writeItems[i].hClient = client;
                writeItems[i].dwBlobSize = 1;
                writeItems[i].pBlob = IntPtr.Zero;
                writeItems[i].vtRequestedDataType = (int)VarEnum.VT_BSTR;
                writeItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}", writeDB[i]);
                client++;
            }
            if (!SyncAddItems(writeItems, writeHandle)) return false;

            //开始接收订阅数据项的事件
            SetState(true);
            isBindToPLC = true;


            return isBindToPLC;
        }
        #endregion

        #region 断开PLC
        /// <summary>
        /// 断开PLC
        /// </summary>
        public virtual void DisConnectPLC()
        {
            if (isBindToPLC)
            {
                DisConnect();
                isBindToPLC = false;
            }
        }
        #endregion 

        #region 异步读订阅
        /// <summary>
        /// 非命令设备订阅返回的信息
        /// </summary>
        public override void OnDataChange(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
        {
            for (int i = 0; i < phClientItems.Length; i++)
            {
                for (int j = 0; j < errorClientHandle.Length; j++)
                {
                    if (phClientItems[i] == errorClientHandle[j]) //状态数据
                    {
                        this.error[j] = int.Parse(pvValues[i].ToString());
                    }
                }
            }
        }
        #endregion 

        #region 刷新辊道状态
        /// <summary>
        /// 刷新辊道状态
        /// </summary>
        public void RefreshStatus()
        {
            if (mainFrm.deviceLinkIsOK)
            {
                if (!isBindToPLC)
                {
                    if (!BindToPLC())
                    {
                        mainFrm.DeviceDisConnection();
                        mainFrm.SetClosing(true);
                    }
                }
                try
                {
                    int[] value = new int[15];
                    object[] readValues = new object[loadDB.Length];
                    try
                    {
                        if (!SyncRead(readValues, loadHandle))
                        {
                            //mainFrm.DeviceDisConnection();
                            //mainFrm.SetClosing(true);
                            MessageBox.Show("readValues报错");
                            return;
                        }
                        for (int i = 0; i < loadDB.Length; i++)
                        {
                            GetValue(readValues[i].ToString(), value);
                            loadStruct[i].taskID = int.Parse(value[0].ToString());
                            loadStruct[i].taskType = int.Parse(value[1].ToString());
                            loadStruct[i].from = int.Parse(value[2].ToString());
                            loadStruct[i].to = int.Parse(value[3].ToString());
                            loadStruct[i].loadType = int.Parse(value[4].ToString());
                        }
                    }
                    catch (Exception )
                    {
                        return;
                        // MessageBox.Show("InConveyorCmd类：RefreshStatus方法" + ex.Message);
                    }


                    readValues = new object[returnDB.Length];
                    if (!SyncRead(readValues, returnHandle))
                    {
                        mainFrm.DeviceDisConnection();
                        mainFrm.SetClosing(true);
                        return;
                    }
                    for (int i = 0; i < returnDB.Length; i++)
                    {
                        GetValue(readValues[i].ToString(), value);
                        returnStruct[i].taskID = int.Parse(value[0].ToString());
                        returnStruct[i].taskType = int.Parse(value[1].ToString());
                        returnStruct[i].from = int.Parse(value[2].ToString());
                        returnStruct[i].to = int.Parse(value[3].ToString());
                        returnStruct[i].status = int.Parse(value[4].ToString());

                    }
                }
                catch (Exception )
                {
                    return;
                    //   MessageBox.Show("InConveyorCmd类：RefreshStatus方法" + ex.Message);
                }

            }
        }
        #endregion

        #region 拆分读取的PLC字符串
        /// <summary>
        /// 拆分读取的PLC字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        public void GetValue(string str, int[] value)
        {
            int nBegin = 1;
            int nIndex = str.IndexOf('|', nBegin);
            int nRe = 0;
            while (nIndex > 0)
            {
                value[nRe] = int.Parse(str.Substring(nBegin, nIndex - nBegin));
                nBegin = nIndex + 1;
                nRe++;
                nIndex = str.IndexOf('|', nBegin);
            }
            value[nRe] = int.Parse(str.Substring(nBegin, str.Length - 1 - nBegin));
        }
        #endregion 

        #region 显示辊道图片
        /// <summary>
        /// 刷新辊道图片
        /// </summary>
        public void DisplayStatus()
        {
            for (int i = 0; i < this.nCount; i++)
            {
                if (!isBindToPLC)
                    lb[i].BackColor = Color.DeepSkyBlue;
                else
                {
                    if (error[i] == 0)
                    {
                        if (systemstatus.GetAuto(levelNum[i]) == "自动")
                        {
                            if (loadStruct[0].loadType == 0)//是否有货  1：货物托盘 2:单个托盘 3：成摞托盘 0：无货
                                lb[i].BackColor = Color.Green;
                            else

                                lb[i].BackColor = Color.Gold;
                        }
                        else
                            lb[i].BackColor = Color.LightGreen;
                    }
                    else
                    {
                        lb[i].BackColor = Color.Red;
                        if (lastError[i] != error[i])
                        {
                          string s;
                     //     mainFrm.speech.speech("拆盘机" + this.nDeviceName[i].ToString() + mainFrm.ConveyorError("DISM", error[i]));
                                if (error[i] != 15)
                                    DataBaseInterface.DeviceErrorMessage(nDeviceName[i], levelNum[i], 0, "DP", error[i], mainFrm.deviceStatusDic.getDesc("DP", error[i].ToString()), loadStruct[i].taskID);
                        }
                    }
                    lastError[i] = error[i];
                }

            }
        }
        #endregion

        #region 出托盘主线程
        public void OutboundAssigning()
        {
            while (!closing)
            {
                Thread.Sleep(100);
                if (mainFrm.bPicking && mainFrm.taskType[0] == 1)//开始分拣
                {
                    using (MySqlConnection conn = dbConn.GetConnectFromPool())
                    {
                        if (conn == null)
                        {
                            Thread.Sleep(1000);
                            continue;
                        }
                        string strSQL = string.Empty;
                        try
                        {
                            RefreshStatus();
                            //strSQL = "select t.* from tb_plt_task_m t where t.task_type=6 and t.task_status < 2";//查找是否有未完成的去入库口的任务
                            //DataSet ds1 = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                     



                                #region 如果拆盘机内没有没有托盘组
                                if (loadStruct[0].loadType == 0 /*&& ds1.Tables[0].Rows.Count == 0*/)//没有去入库口的任务，并且拆盘机无货

                            {
                                strSQL = "select t.* from tb_plt_task_m t where t.goods_kind = 3 and task_type=2 and t.task_status < 2";//查找是否有未完成的出托盘任务
                                DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                                if (ds.Tables[0].Rows.Count > 0)
                                {
                                    Thread.Sleep(500);
                                    continue;
                                }
                                else
                                {
                                    // if (mainFrm.outElectricsConveyor.canGetUp(1)==1)//判断出托盘辊道是否有货
                                      if (mainFrm.outElectricsConveyor.canGetUp(1)==1)
                                    {
                                        string rs = string.Empty;
                                        string boxCode = DataBaseInterface.SelectTray(conn);
                                        if (boxCode == null)
                                        {
                                            // MessageBox.Show("库中已无空托盘！请先入空托盘！");
                                        }
                                        else
                                        {
                                            strSQL = "select empty_status from td_empty_status ";//
                                            ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                                            if (int.Parse(ds.Tables[0].Rows[0]["empty_status"].ToString()) ==1)
                                            {
                                               
                                                DataBaseInterface.CreateOutBoundTask(conn, boxCode, "", 2, mainFrm.dealWay[0], out rs);
                                            }
                                            else {
                                                continue;
                                            }
                                          
                                        }
                                    }
                                }
                                continue;
                            }
                            #endregion

                            #region 如果拆盘机内有空托盘组
                            else 
                            {
                                if(mainFrm.taskType[0]==1)
                                {
                                    //if (mainFrm.batchNo[0] != string.Empty && playTray && DataBaseInterface.SelectUseLocationNum(conn,3) > 5)
                                    if (mainFrm.batchNo[0] != string.Empty && playTray && DataBaseInterface.SelectUseLocationNum(conn,3)>0 )
                                    {
                                        RefreshStatus();
                                        //获取待补空托盘的入库口
                                        DataSet ds = DataBaseInterface.SelectInPort(conn);
                                        if (ds != null && ds.Tables[0].Rows.Count > 0)
                                        {
                                            foreach (DataRow row in ds.Tables[0].Rows)
                                            {
                                                int nPort = int.Parse(row["port_id"].ToString());
                                                if (mainFrm.inConveyorScannerCmd.loadStruct[nPort - 1].loadType == 0 && mainFrm.batchNo[0] != null && returnStruct[0].status == 10 &&mainFrm.inConveyorScannerCmd.returnStruct[nPort-1].to==0)
                                                {
                                                    strSQL = "select * from tb_plt_task_m t where t.goods_kind = 3 and task_type=6 and t.task_status < 2 and t.to_unit='" + row["unit"].ToString() + "'";
                                                    ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                                                    if (ds.Tables[0].Rows.Count > 0)
                                                    {
                                                        Thread.Sleep(500);
                                                        continue;
                                                    }
                                                    else
                                                    {
                                                        //if (mainFrm.inConveyorScannerCmd.returnStruct[nPort - 1].status != 10)
                                                        //    continue;
                                                        int taskID = int.Parse(DataBaseInterface.GetTaskM(conn));
                                                        DownLoadNewTask(conn, 0, taskID, 1, 1, 4, 1004, int.Parse(row["unit"].ToString()), "null");
                                                        continue;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                            if (loadStruct[0].loadType != 0 && loadStruct[0].to == 1004 && loadStruct[0].taskID!=lastTaskId[0])
                            {
                                string rs = string.Empty;
                                if (DataBaseInterface.TaskStatusUpdate(loadStruct[0].taskID, 1004, "RKSS1004", 2, 2, out rs) != 1)
                                    MessageBox.Show("拆盘机空托盘组出库任务报完成失败：" + rs);
                                else
                                    lastTaskId[0] = loadStruct[0].taskID;
                            }
                            #endregion
                            Thread.Sleep(300);
                            continue;

                        }
                        catch (Exception ex)
                        {

                            Thread.Sleep(300);
                            continue;
                        }
                    }
                }
                else
                {
                    Thread.Sleep(300);
                    continue;
                }
            }
        }
        #endregion

        #region 辊道写任务
        /// <summary>
        /// 辊道写任务
        /// </summary>-
        public void WriteCmd(int index, int nTaskID, int nTaskType, int FromLev, int ToLev, int loadType)
        {
            if (returnStruct[index].taskID == nTaskID)//返回的任务号和下载的任务号相同 则停止写任务
                return;
            int[] handle = new int[1];
            handle[0] = writeHandle[index];


            string[] writeValues = new string[1];
            writeValues[0] = "{";
            writeValues[0] += (nTaskID).ToString() + "|";
            writeValues[0] += (nTaskType).ToString() + "|";
            writeValues[0] += (FromLev).ToString() + "|";
            writeValues[0] += (ToLev).ToString() + "|";
            writeValues[0] += (loadType).ToString();
            writeValues[0] += "}";
            if (!SyncWrite(writeValues, handle))
            {
                mainFrm.DeviceDisConnection();
                mainFrm.SetClosing(true);
            }
        }
        #endregion

        #region 辊道下发任务，并检测是否下发成功
        /// <summary>
        /// 辊道下发任务，并检测是否下发成功
        /// </summary>
        /// <param name="conn"></param>
        /// <param name="nTask"></param>
        /// <param name="nAdd"></param>
        /// <param name="strBarcode"></param>
        private void DownLoadNewTask(MySqlConnection conn, int nScannerID, int nTaskid, int dTaskType, int mTaskType, int nloadType, int nFromAdd, int nToAdd, string strBarcode)
        {
            int i;
            for (i = 0; i < 20; i++)
            {
                if (mTaskType == 1)
                {
                     WriteCmd(nScannerID , nTaskid, dTaskType, nFromAdd, nToAdd, nloadType);
                    Thread.Sleep(200);
                    RefreshStatus();
                    if (returnStruct[nScannerID].taskID == nTaskid)
                    {
                        DataBaseInterface.InsertTaskM(conn,nTaskid, 6, nFromAdd.ToString(), nToAdd.ToString(), 3, 1);
                        i = 0;
                        break;
                    }
                }
              
            }
            if (i == 20)
                MessageBox.Show("未能成功写入出托盘任务！");

        }
        #endregion

        #region 辊道写入控制命令
        /// <summary>
        /// 辊道写入控制命令
        /// </summary>
        /// <param name="nCol"></param> 列
        /// <param name="nLevel"></param> 层
        /// <param name="nAction"></param> 控制代码  1 清错；2 初始化
        public void WriteSingleAction(int nIndex, int nAction)
        {
            int[] handle = new int[1];
            handle[0] = controlHandle[nIndex];
            string[] writeValues = new string[1];
            writeValues[0] = nAction.ToString();
            if (!SyncWrite(writeValues, handle))
            {
                mainFrm.DeviceDisConnection();
                mainFrm.SetClosing(true);
            }
        }
        #endregion

        #region 判断入库放货辊道上是否已有托盘
        /// <summary>
        /// 判断入库放货辊道上是否已有托盘（主入库口需要判断扫码段加测重段，子入库口需要判断放货辊道站台和测重段）
        /// </summary>
        /// <param name="about">是左侧还是右侧放托盘(1-主入库口 2-子入库口)</param>
        /// <returns>能否放托盘</returns>
        public bool conveyorLoad(int about)
        {
            return true;
        }
        #endregion



        #region 图片双击事件
        public virtual void lb_DoubleClick(object sender, EventArgs e)
        {
            string strTag = Convert.ToString(((Label)sender).Tag);
            int Index = int.Parse(strTag);
            //mainFrm.ChangetbScanInfoIndex((rowNum[Index] - 1) * 2 + levelNum[Index]-1);
            FormTray frmConveyor = new FormTray(this, Index, mainFrm.systemStatus);
            frmConveyor.StartPosition = FormStartPosition.CenterParent;
            frmConveyor.ShowDialog();
        }
        #endregion
    }
}
