using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpcRcw.Da;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Diagnostics;
using DataBase;
using System.Collections;

using MySql.Data.MySqlClient;

namespace JY_Sinoma_WCS
{
    public class SecondFloorInConveyorCmd : Conveyor
    {

        #region 自定义变量
        /// <summary>
        /// 线程
        /// </summary>
        public Thread InConveyorCmdThread;
        public Thread ElectricConveyorThread;
        public int[] nBarcodeError;
        public string[] lastStrError;
        public int[] nTask;
        public int[] fromadd;
        public int[] toadd;
        public int[] mTasktype;//zhu ren wu lei xing
        public int[] dTasktype;// zi renwu leixing 
        public int[] loadtype;
        public int[] systemStatusID;
        /// <summary>
        /// 任务错误类型 1-未扫到条码 2-无入库单 3-为找到可用库位 4-仓库中已有相同的托盘号 5-正在执行的任务中有相同的托盘号
        /// </summary>
        public int taskError;

        public string[] weightDB;//对应辊道的重量DB块地址
        public int[] weightHandle;//辊道控制的OPCServer的Handle 
        public int goodsWeight;//货物重量

        string str_unit;
        string str_units;
        int pd = 0;//判断出库口
        int attempts = 0;//未扫到条码判断
        int taskErrorNumber = 0;//下任务出错次数
        public Dictionary<string, int> inConveyorCmdId = new Dictionary<string, int>();
        /// <summary>
        /// 人工输入托盘号
        /// </summary>
        public string inputBoxCode = string.Empty;
        public bool inventoryOver = false;
        public int lastTaskID = 0;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainFrm"></param>
        /// <param name="bt"></param>
        public SecondFloorInConveyorCmd(frmMain mainFrm, DataTable bt)
            : base(mainFrm, bt)
        {
            nBarcodeError = new int[nCount];
            lastStrError = new String[nCount];
            loadDB = new String[nCount];
            loadHandle = new int[nCount];
            returnDB = new String[nCount];
            returnHandle = new int[nCount];
            writeDB = new String[nCount];
            writeHandle = new int[nCount];


            loadStruct = new LoadStruct[nCount];
            taskStruct = new TaskStruct[nCount];
            returnStruct = new ReturnStruct[nCount];
            channelNum = new int[nCount];
            levelNum = new int[nCount];

            nTask = new int[nCount];
            fromadd = new int[nCount];
            toadd = new int[nCount];
            mTasktype = new int[nCount];
            dTasktype = new int[nCount];
            loadtype = new int[nCount];
            systemStatusID = new int[nCount];

            int i = 0;
            foreach (DataRow row in bt.Rows)
            {
                nTask[i] = 0;
                fromadd[i] = 0;
                toadd[i] = 0;
                mTasktype[i] = 0;
                dTasktype[i] = 0;
                nBarcodeError[i] = 0;
                loadtype[i] = 0;
                lastStrError[i] = string.Empty;
                errorDB[i] = row["error_db"].ToString();
                controlDB[i] = row["control_db"].ToString();
                loadDB[i] = row["load_db"].ToString();
                returnDB[i] = row["return_db"].ToString();
                writeDB[i] = row["write_db"].ToString();
                nDeviceID[i] = int.Parse(row["device_name"].ToString());
                levelNum[i] = int.Parse(row["level_no"].ToString());
                rowNum[i] = int.Parse(row["row_no"].ToString());
                conveyorName[i] =  row["device_tag"].ToString();
                nInitX = int.Parse("0" + row["x"].ToString());
                nInitY = int.Parse("0" + row["y"].ToString());
                deviceType[i] = row["device_mold"].ToString();
                systemStatusID[i] = int.Parse(row["system_status_id"].ToString());
                //定义图片
                lb[i] = new Label();
                lb[i].Size = new Size(int.Parse(row["length"].ToString()), int.Parse(row["width"].ToString()));
                lb[i].Location = new Point(int.Parse(row["x"].ToString()), int.Parse(row["y"].ToString()));
                lb[i].BackColor = Color.Gray;
                lb[i].Font = new Font("宋体", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                lb[i].TextAlign = ContentAlignment.MiddleCenter;
                lb[i].DoubleClick += new System.EventHandler(this.lb_DoubleClick);
                lb[i].Tag = i.ToString();
                lb[i].BringToFront();
                mainFrm.pLevel21.Controls.Add(lb[i]);
               
                InConveyorCmdThread = new Thread(new  ThreadStart(BarCodecheck));
                InConveyorCmdThread.IsBackground = true;
                // 判断该线程是否被垃圾回收
                if (!InConveyorCmdThread.IsAlive)
                {
                    InConveyorCmdThread.Start();
                }

                i++;

            }
        }
        #endregion

        #region 连接PLC
        /// <summary>
        /// 连接PLC
        /// </summary>
        /// <returns></returns>
        public override bool BindToPLC()
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

        #region 刷新辊道状态
        /// <summary>
        /// 刷新辊道状态
        /// </summary>
        public override void RefreshStatus()
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
                    catch (Exception ex)
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
                catch (Exception ex)
                {
                    return;
                    //   MessageBox.Show("InConveyorCmd类：RefreshStatus方法" + ex.Message);
                }

            }
        }
        #endregion

        #region 显示辊道图片
        /// <summary>
        /// 刷新辊道图片
        /// </summary>
        public override void DisplayStatus()
        {
            for (int i = 0; i < this.nCount; i++)
            {
                if (!isBindToPLC)
                    lb[i].BackColor = Color.DeepSkyBlue;
                else
                {
                    if ((error[i] == 0 && nBarcodeError[i] == 0) || (error[i] == 15 && nBarcodeError[i] == 0))
                    {
                        if (systemstatus.GetAuto(levelNum[i]) == "自动")
                        {
                            if (lb[i].BackColor == Color.Red)
                                DataBaseInterface.DeviceErrorMessage(conveyorName[i], levelNum[i], 0, deviceType[i], error[i], mainFrm.deviceStatusDic.getDesc(deviceType[i], error[i].ToString()), loadStruct[i].taskID);
                            if (loadStruct[i].loadType == 1 || loadStruct[i].loadType == 2 || loadStruct[i].loadType == 3)//是否有货  1：货物托盘 2:单个托盘 3：成摞托盘 0：无货
                                lb[i].BackColor = Color.Gold;
                            else

                                lb[i].BackColor = Color.Green;
                        }
                        else
                            lb[i].BackColor = Color.LightGreen;

                    }
                    else
                    {
                        lb[i].BackColor = Color.Red;
                        if (lastError[i] != error[i])
                        {
                            // mainFrm.speech.speech("入库口扫码器输送" + this.conveyorName[i].ToString() + mainFrm.ConveyorError(deviceType[i], error[i]));
                            if (error[i] != 15)
                                DataBaseInterface.DeviceErrorMessage(conveyorName[i], levelNum[i], 0, deviceType[i], error[i], mainFrm.deviceStatusDic.getDesc(deviceType[i], error[i].ToString()), loadStruct[i].taskID);
                        }
                    }
                    lastError[i] = error[i];
                }

            }
        }
        #endregion

        #region 标签的双击事件
        public override void lb_DoubleClick(object sender, EventArgs e)
        {
            string strTag = Convert.ToString(((Label)sender).Tag);
            int Index = int.Parse(strTag);
            FormConveyorCmd frmConveyor = new FormConveyorCmd(this, Index, mainFrm.systemStatus);
            frmConveyor.ShowDialog();
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
        private bool DownLoadNewTask(int nScannerID, int nTaskid, int dTaskType, int mTaskType, int nloadType, int nFromAdd, int nToAdd, string strBarcode)
        {
            int i;
            string rs = string.Empty;
            for (i = 0; i < 10; i++)
            {
                WriteCmd(nScannerID, nTaskid, dTaskType, nFromAdd, nToAdd, nloadType);
                Thread.Sleep(200);
                RefreshStatus();
                if (returnStruct[nScannerID].taskID == nTaskid)
                {
                    i = 0;
                    inputBoxCode = string.Empty;
                    nBarcodeError[nScannerID] = 0;
                    break;
                }
            }
            if (i >= 10)
            {
                MessageBox.Show("二楼1007辊道箱号:" + strBarcode + "任务下发失败！");
                nBarcodeError[nScannerID] = 1;
                return false;
            }
           else
                return true;
        }
        #endregion
      
        #region 条码检测、辊道下任务
        public void BarCodecheck()
        {
            int nScannerId = 0;



            while (!closing)
            {

                Thread.Sleep(100);
                if (!isBindToPLC || !mainFrm.deviceLinkIsOK)
                {
                    Thread.Sleep(1000);
                    continue;
                }
                try
                {
                    if (mainFrm.bPicking)
                    {

                        RefreshStatus();
                        #region 经过该段输送出库任务设置标志位
                        if (lastTaskID != this.loadStruct[nScannerId].taskID)
                        {
                            if (DataBaseInterface.UpdateTaskIsOut(this.loadStruct[nScannerId].taskID) == 1)
                            {
                                lastTaskID = this.loadStruct[nScannerId].taskID;
                            }
                        }
                        #endregion
                        using (MySqlConnection conn = dbConn.GetConnectFromPool())
                        {
                            if (this.returnStruct[nScannerId].status == 10 && mainFrm.bPicking && nBarcodeError[nScannerId] == 0 && !mainFrm.stopTaskCreate[levelNum[nScannerId] - 1])
                            {
                                string strBarcode = "";

                                nTask[nScannerId] = int.Parse(DataBaseInterface.GetTaskM(conn))+1;
                                //DataRow rowTask = DataBaseInterface.SelectTaskD(conn, nTask[nScannerId].ToString(), "CON");

                                #region 空托盘组入库
                                if (returnStruct[nScannerId].status == 10 && mainFrm.taskType[1] == 2)
                                {

                               // nTask[nScannerId] = loadStruct[nScannerId].taskID +1;
                               // nTask[nScannerId] = int.Parse(rowTask["task_id"].ToString());
                                    fromadd[nScannerId] = 1007;
                                    toadd[nScannerId] = 1001;
                                    mTasktype[nScannerId] = 3;
                                    dTasktype[nScannerId] = 3;
                                    loadtype[nScannerId] = 3;
                                    DownLoadNewTask(nScannerId, nTask[nScannerId], dTasktype[nScannerId], mTasktype[nScannerId], loadtype[nScannerId], fromadd[nScannerId], toadd[nScannerId], strBarcode);
                                    DataBaseInterface.UpdateCounts();
                                }
                               
                                #endregion


                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                //  MessageBox.Show("扫码下任务" + ex.Message);
                    Thread.Sleep(300);
                }
            }
        }



        #endregion

        #region 辊道写入控制命令
        /// <summary>
        /// 辊道写入控制命令
        /// </summary>
        /// <param name="nCol"></param> 列
        /// <param name="nLevel"></param> 层
        /// <param name="nAction"></param> 控制代码  1 清错；2 初始化
        public override void WriteSingleAction(int nIndex, int nAction)
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

        #region 断开PLC
        /// <summary>
        /// 断开PLC
        /// </summary>
        public override void DisConnectPLC()
        {
            if (isBindToPLC)
            {
                DisConnect();
                isBindToPLC = false;
            }
        }
        #endregion

        /// <summary>
        /// 判断下任务辊道是否允许下任务
        /// </summary>
        /// <param name="nScannerId">下任务辊道编号</param>
        /// <returns>是否属于下任务辊道</returns>
        public bool isWrite(int nScannerId)
        {
            if (this.returnStruct[nScannerId].status == 10 && mainFrm.bPicking && nBarcodeError[nScannerId] == 0)
                return true;
            else
                return false;

        }

    }
}
