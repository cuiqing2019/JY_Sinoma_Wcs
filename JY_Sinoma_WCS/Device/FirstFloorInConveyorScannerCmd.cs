using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PLC;
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
    public class FirstFloorInConveyorScannerCmd : Conveyor
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
        public double[] goodsWeight;//货物重量

        string str_unit;
        string str_units;
        /// <summary>
        /// 判断出库口
        /// </summary>
        int pd = 0;//判断出库口
        /// <summary>
        /// 未扫到条码判断
        /// </summary>
        int attempts = 0;//未扫到条码判断
        /// <summary>
        /// 下任务出错次数
        /// </summary>
        int taskErrorNumber = 0;//下任务出错次数
        public Dictionary<string, int> inConveyorCmdId = new Dictionary<string,int>();
        public int taskID = 0;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainFrm"></param>
        /// <param name="bt"></param>
        public FirstFloorInConveyorScannerCmd(frmMain mainFrm, DataTable bt)
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
            weightHandle = new int[nCount];
            weightDB = new string[nCount];

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
            goodsWeight = new double[nCount];

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
                weightDB[i] = row["weight_db"].ToString();
                nDeviceID[i] = int.Parse(row["device_name"].ToString());
                levelNum[i] = int.Parse(row["level_no"].ToString());
                rowNum[i] = int.Parse(row["row_no"].ToString());
                conveyorName[i] = row["device_tag"].ToString();
                scanner_id[i] = row["IS_SCANNER"].ToString();
                nInitX = int.Parse("0" + row["x"].ToString());
                nInitY = int.Parse("0" + row["y"].ToString());
                inConveyorCmdId.Add(row["IS_SCANNER"].ToString(), i);
                deviceType[i] = row["device_mold"].ToString();
                systemStatusID[i] = int.Parse(row["system_status_id"].ToString());
                goodsWeight[i] = 0;
                //定义图片
                lb[i] = new Label();
                lb[i].Size = new Size( int.Parse(row["length"].ToString()),int.Parse(row["width"].ToString()));
                lb[i].Location = new Point(int.Parse(row["x"].ToString()), int.Parse(row["y"].ToString()));
                lb[i].BackColor = Color.Gray;
                lb[i].Font = new Font("宋体", 8F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                lb[i].TextAlign = ContentAlignment.MiddleCenter;
                lb[i].DoubleClick += new System.EventHandler(this.lb_DoubleClick);
                lb[i].Tag = i.ToString();
                lb[i].BringToFront();
                mainFrm.PLevel1.Controls.Add(lb[i]);
                if (scanner_id[i] != null)
                {
                    InConveyorCmdThread = new Thread(new ParameterizedThreadStart(BarCodecheck));
                    InConveyorCmdThread.IsBackground = true;
                    // 判断该线程是否被垃圾回收
                    if (!InConveyorCmdThread.IsAlive)
                    {
                        InConveyorCmdThread.Start(i);
                    }
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

            OpcRcw.Da.OPCITEMDEF[] weightItems = new OPCITEMDEF[weightDB.Length];
            for (int i = 0; i < loadDB.Length; i++)
            {
                weightItems[i].szAccessPath = "";
                weightItems[i].bActive = 1;
                weightItems[i].hClient = client;
                weightItems[i].dwBlobSize = 1;
                weightItems[i].pBlob = IntPtr.Zero;
                weightItems[i].vtRequestedDataType = (int)VarEnum.VT_BSTR;
                weightItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}", weightDB[i]);
                client++;
            }
            if (!SyncAddItems(weightItems, weightHandle)) return false;

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

                    readValues = new object[weightDB.Length];
                    try
                    {
                        if (!SyncRead(readValues, weightHandle))
                        {
                            //mainFrm.DeviceDisConnection();
                            //mainFrm.SetClosing(true);
                            MessageBox.Show("readValues报错");
                            return;
                        }
                        for (int i = 0; i < weightDB.Length; i++)
                        {
                            goodsWeight[i] = Convert.ToDouble(readValues[i].ToString()) / 1000;
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
                lb[i].Text = "重量:" + goodsWeight[i];
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
        private bool DownLoadNewTask1(int nScannerID, int nTaskid, int dTaskType, int mTaskType, int nloadType, int nFromAdd, int nToAdd, string strBarcode)
        {
            int i;
            string taskDesc = "";
            if (mTaskType == 4)
            {
                taskDesc = "退库任务报完成成功！";
                DataBaseInterface.TaskStatusUpdate(nTaskid, nDeviceID[nScannerID], conveyorName[nScannerID], 2, 2, out taskDesc);//修改任务状态为已完成
                return true;                                                                                                          // DataBaseInterface.UpdateRFIDStatus(scanner_id[nScannerID]);//将已经读到的rfid标识清空            
            }
            if (mTaskType == 1 || mTaskType==3)
            {
                taskDesc = "外形检测异常清除库存信息";
                DataBaseInterface.taskupdateyichang(nTaskid, nDeviceID[nScannerID], conveyorName[nScannerID], 1, 1, out taskDesc);//修改任务状态为执行中
                return true;
            }

            else
            {
                return false;
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
        private bool DownLoadNewTask(int nScannerID, int nTaskid, int dTaskType, int mTaskType, int nloadType, int nFromAdd, int nToAdd, string strBarcode)
        {
            int i;
            string taskDesc = "";
            RefreshStatus();
            if (mTaskType == 4)
            {
                taskDesc = "退库任务报完成成功！";
                DataBaseInterface.TaskStatusUpdate(nTaskid, nDeviceID[nScannerID], conveyorName[nScannerID], 2, 2, out taskDesc);//修改任务状态为已完成
                return true;                                                                                                          // DataBaseInterface.UpdateRFIDStatus(scanner_id[nScannerID]);//将已经读到的rfid标识清空            
            }
            else
            {
                for (i = 0; i < 10; i++)
                {
                    WriteCmd(nScannerID, nTaskid, dTaskType, nFromAdd, nToAdd, nloadType);
                    Thread.Sleep(200);
                    RefreshStatus();

                    if (returnStruct[nScannerID].taskID == nTaskid)
                    {
                        if (mTaskType == 1)
                        {
                            taskDesc = "一楼入库任务下发成功";
                            DataBaseInterface.TaskStatusUpdate(nTaskid, nDeviceID[nScannerID], conveyorName[nScannerID], 1, 1, out taskDesc);//修改任务状态为执行中
                        }
                       
                        else if (mTaskType == 3)
                        {
                            taskDesc = "空托盘入库任务生成成功！";
                            DataBaseInterface.TaskStatusUpdate(nTaskid, nDeviceID[nScannerID], conveyorName[nScannerID], 1, 1, out taskDesc);//修改任务状态为执行中
                                                                                                                                             // DataBaseInterface.UpdateRFIDStatus(scanner_id[nScannerID]);//将已经读到的rfid标识清空
                        }

                        if (mTaskType == 5)
                        {
                            taskDesc = "一楼异常回库任务下发成功";
                            DataBaseInterface.TaskStatusUpdate(nTaskid, nDeviceID[nScannerID], conveyorName[nScannerID], 1, 1, out taskDesc);//修改任务状态为执行中
                        }
                        mainFrm.lvScanInfo_Insert(nScannerID, strBarcode, taskDesc + "任务号：" + nTaskid.ToString() + ",目的地址：" + nToAdd.ToString());
                        mainFrm.ChangetbScanInfoIndex(nScannerID);
                        i = 0;
                        nBarcodeError[nScannerID] = 0;
                        break;
                    }
                }
                if (i >= 10)
                {
                    ReportError(0, 2, strBarcode, "");
                    return false;
                }
                else
                    return true;
            }
        }
        #endregion

        #region 报错信息显示
        /// <summary>
        /// 报错信息显示
        /// </summary>
        /// <param name="nError"></param>
        /// <param name="strBarcode"></param>
        /// <param name="strReturn"></param>
        private void ReportError(int nScannerId, int nError, string strBarcode, string strReturn)
        {
            string strError = string.Empty;
            switch (nError)
            {
                case 1:
                    strError = "扫码异常：辊道有货，未正确扫描到条码";
                    break;
                case 2:
                    strError = "箱号:" + strBarcode + "\r\n" + "任务下发失败！" + strReturn + "！";
                    break;
                case 3:
                    strError = "入库废料重量超重！";
                    break;

            }
           // mainFrm.speech.speech(strError);
            mainFrm.lvScanInfo_Insert(nScannerId, strBarcode, strError);
            mainFrm.ChangetbScanInfoIndex(nScannerId);
            if (lastStrError[nScannerId] != strError)
            {
                MessageBox.Show(strError);
                if (nError == 2)
                    DataBaseInterface.DeviceErrorMessage(conveyorName[nScannerId], levelNum[nScannerId], 0, "CON", 0, "辊道下任务失败", loadStruct[nScannerId].taskID);
            }
            nBarcodeError[nScannerId] = nError;
            lastStrError[nScannerId ] = strError;
        }
        #endregion

        #region 条码检测、辊道下任务
        public void BarCodecheck(object inConveyorCmdId)
        {
            int nScannerId = int.Parse(inConveyorCmdId.ToString());
            int nReturn = 0;
            DataRow taskrow = null;

            while (!closing)
            {

                Thread.Sleep(100);
                if (!isBindToPLC || !mainFrm.deviceLinkIsOK)
                {
                    Thread.Sleep(1000);
                    continue;
                }

                using (MySqlConnection conn = dbConn.GetConnectFromPool())
                {
                    try
                    {
                        if (mainFrm.bPicking)
                        {

                            RefreshStatus();
                            
                            if (mainFrm.taskType[0] == 3 && loadStruct[nScannerId].loadType != 0)
                            {
                                lock (DataBaseInterface.obLock)
                                {
                                    if (this.loadStruct[nScannerId].loadType != 0 && mainFrm.bPicking)
                                    {
                                        DataRow dr = DataBaseInterface.SelectTaskD(conn, loadStruct[nScannerId].taskID.ToString(), "CON");
                                        if (dr != null && int.Parse(dr["task_type"].ToString()) == loadStruct[nScannerId].taskType)
                                        {
                                            int i = 0;
                                            switch (loadStruct[nScannerId].taskType)
                                            {
                                                case 4: //退库
                                                    fromadd[nScannerId] = int.Parse(dr["from_unit"].ToString());
                                                    toadd[nScannerId] = int.Parse(dr["to_unit"].ToString());
                                                    mTasktype[nScannerId] = int.Parse(dr["task_type"].ToString());
                                                    dTasktype[nScannerId] = int.Parse(dr["task_type"].ToString());
                                                    loadtype[nScannerId] = mainFrm.goodsKind[levelNum[nScannerId] - 1];
                                                    DownLoadNewTask1(nScannerId, loadStruct[nScannerId].taskID, dTasktype[nScannerId], mTasktype[nScannerId], loadtype[nScannerId], fromadd[nScannerId], toadd[nScannerId], dr["box_barcode"].ToString());
                                                    break;
                                                default:
                                                    break;
                                            }
                                        }
                                    }
                                }
                            }

                            if (this.returnStruct[nScannerId].status == 10 &&loadStruct[nScannerId].loadType>0 && mainFrm.bPicking && nBarcodeError[nScannerId] == 0 && !mainFrm.stopTaskCreate[levelNum[nScannerId] - 1]&&mainFrm.systemStatus.taskModeDB[0]!=3.ToString())
                            {
                                if (nScannerId == 3 && mainFrm.goodsKind[levelNum[nScannerId] - 1] != 1)
                                {
                                    MessageBox.Show("4号入库口只能入吨桶！");
                                    continue;
                                }
                                if (nScannerId == 2 && mainFrm.goodsKind[levelNum[nScannerId] - 1] == 3)
                                {
                                    MessageBox.Show("3号入库口不能入空托");
                                    continue;
                                }
                                #region 废料入库扫码段
                                if (mainFrm.taskType[0] == 1 && loadStruct[nScannerId].loadType != 0)
                                {
                                    if (mainFrm.goodsKind[levelNum[nScannerId] - 1] == 0 || mainFrm.goodsSku[levelNum[nScannerId] - 1] == string.Empty)
                                        continue;

                                    if (mainFrm.ReadBarCodeFromSPs[scanner_id[nScannerId].ToString()].isStart || attempts > 0)
                                    {
                                        if (attempts == 5)
                                        {
                                            ReportError(nScannerId, 1, "", "");
                                            mainFrm.ReadBarCodeFromSPs[scanner_id[nScannerId].ToString()].ScanStopRead();
                                            DataBaseInterface.ClearHaveReadRFID(conn, scanner_id[nScannerId].ToString());
                                            attempts = 0;
                                            continue;
                                        }
                                        else
                                        {
                                            attempts++;
                                            Thread.Sleep(100);
                                            continue;
                                        }
                                    }
                                    DataBaseInterface.ClearHaveReadRFID(conn, scanner_id[nScannerId].ToString());
                                    mainFrm.ReadBarCodeFromSPs[scanner_id[nScannerId].ToString()].ScanStartRead();
                                    Thread.Sleep(1000);
                                    string boxCode = DataBaseInterface.GetCurrentBarCode(conn, scanner_id[nScannerId]);
                                    if (boxCode == null)//如果无条码，原地等待，手动输入托盘号
                                    {
                                        attempts = 1;
                                        continue;
                                    }
                                    attempts = 0;
                                    mainFrm.ReadBarCodeFromSPs[scanner_id[nScannerId].ToString()].ScanStopRead();                    
                                    lock (DataBaseInterface.obLock)
                                    {

                                        taskrow = DataBaseInterface.SelectTaskM(conn, boxCode);
                                        if (taskrow != null)
                                        {
                                            mainFrm.ChangetbScanInfoIndex(nScannerId);

                                            DataRow rowTask = DataBaseInterface.SelectTaskD(conn, returnStruct[nScannerId].taskID.ToString(), "CON");
                                            fromadd[nScannerId] = int.Parse(rowTask["from_unit"].ToString());
                                            toadd[nScannerId] = int.Parse(rowTask["to_unit"].ToString());
                                            mTasktype[nScannerId] = int.Parse(rowTask["task_type"].ToString()); 
                                            dTasktype[nScannerId] = int.Parse(rowTask["task_type"].ToString());
                                            loadtype[nScannerId] = mainFrm.goodsKind[levelNum[nScannerId] - 1];
                                            DownLoadNewTask1(nScannerId, returnStruct[nScannerId].taskID, dTasktype[nScannerId], mTasktype[nScannerId], loadtype[nScannerId], fromadd[nScannerId], toadd[nScannerId], boxCode);

                                        }
                                        else
                                        {

                                            mainFrm.batchWeight += goodsWeight[nScannerId];
                                            mainFrm.ChangeWeight(mainFrm.batchWillWeight, mainFrm.batchWeight);
                                            if ((mainFrm.batchWillWeight * 1.03) < mainFrm.batchWeight && mainFrm.taskType[0] == 1)
                                            {
                                                DialogResult dialogResult = MessageBox.Show("实际入库" + mainFrm.batchWeight.ToString() + "千克，超出预计入库" + (mainFrm.batchWillWeight * 1.03) + "千克，请核实！继续入库请点确定，否则请点取消", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                                if (dialogResult == DialogResult.No)
                                                {
                                                    ReportError(nScannerId, 3, "", "");
                                                    Thread.Sleep(200);
                                                    continue;
                                                }
                                            }
                                            string spError = string.Empty;
                                            nTask[nScannerId] = int.Parse(DataBaseInterface.GetTaskM(conn));
                                            nReturn = DataBaseInterface.CreatInboundTaskM(conn, boxCode, 1, mainFrm.batchNo[levelNum[nScannerId] - 1], mainFrm.batchid[levelNum[nScannerId] - 1], mainFrm.goodsKind[levelNum[nScannerId] - 1], mainFrm.goodsSku[levelNum[nScannerId] - 1], goodsWeight[nScannerId], levelNum[nScannerId], nTask[nScannerId], mainFrm.dealWay[levelNum[nScannerId] - 1], mainFrm.goodsName[levelNum[nScannerId] - 1], mainFrm.hazardArea[levelNum[nScannerId] - 1], int.Parse(scanner_id[nScannerId]), out spError);
                                            if (nReturn == 1) //货位生成 
                                            {
                                                mainFrm.lvScanInfo_Insert(nScannerId, boxCode, spError);
                                                mainFrm.ChangetbScanInfoIndex(nScannerId);
                                                DataRow rowTask = DataBaseInterface.SelectTaskD(conn, nTask[nScannerId].ToString(), "CON");
                                                fromadd[nScannerId] = int.Parse(rowTask["from_unit"].ToString());
                                                toadd[nScannerId] = int.Parse(rowTask["to_unit"].ToString());
                                                mTasktype[nScannerId] = int.Parse(rowTask["task_type"].ToString());
                                                dTasktype[nScannerId] = int.Parse(rowTask["task_type"].ToString());
                                                loadtype[nScannerId] = mainFrm.goodsKind[levelNum[nScannerId] - 1];
                                                DownLoadNewTask(nScannerId, nTask[nScannerId], dTasktype[nScannerId], mTasktype[nScannerId], loadtype[nScannerId], fromadd[nScannerId], toadd[nScannerId], boxCode);
                                                DataBaseInterface.UpdateBatchWeight(conn, mainFrm.batchWeight, mainFrm.batchNo[levelNum[nScannerId] - 1], mainFrm.goodsSku[levelNum[nScannerId] - 1]);
                                            }
                                            else
                                            {
                                                MessageBox.Show("入库存储过程报错" + spError);
                                                continue;
                                            }
                                        }
                                    }
                                

                                }
                                #endregion

                                #region 空托盘入库扫码
                                if (mainFrm.taskType[0] == 2 && loadStruct[nScannerId].loadType != 0)
                                {
                                    if (mainFrm.goodsKind[levelNum[nScannerId] - 1] == 0 || mainFrm.goodsSku[levelNum[nScannerId] - 1] == string.Empty)
                                        continue;



                                    lock (DataBaseInterface.obLock)
                                    {
                                        string m = loadStruct[nScannerId].taskID.ToString();

                                        taskrow = DataBaseInterface.SelectTaskM2(conn, loadStruct[nScannerId].taskID.ToString());
                                        if (taskrow != null)
                                        {
                                            DataRow rowTask = DataBaseInterface.SelectTaskD(conn, m, "CON");
                                            fromadd[nScannerId] = int.Parse(rowTask["from_unit"].ToString());
                                            toadd[nScannerId] = int.Parse(rowTask["to_unit"].ToString());
                                            mTasktype[nScannerId] = int.Parse(rowTask["task_type"].ToString());
                                            dTasktype[nScannerId] = int.Parse(rowTask["task_type"].ToString());
                                            loadtype[nScannerId] = mainFrm.goodsKind[levelNum[nScannerId] - 1];
                                            DownLoadNewTask1(nScannerId, loadStruct[nScannerId].taskID, dTasktype[nScannerId], mTasktype[nScannerId], loadtype[nScannerId], fromadd[nScannerId], toadd[nScannerId], nTask[nScannerId].ToString());

                                        }
                                        else
                                        {

                                            string spError = string.Empty;
                                            nTask[nScannerId] = int.Parse(DataBaseInterface.GetTaskM(conn));
                                            nReturn = DataBaseInterface.CreatInboundTaskM(conn, nTask[nScannerId].ToString(), 3, mainFrm.batchNo[levelNum[nScannerId] - 1], mainFrm.batchid[levelNum[nScannerId] - 1], mainFrm.goodsKind[levelNum[nScannerId] - 1], mainFrm.goodsSku[levelNum[nScannerId] - 1], 0, levelNum[nScannerId], nTask[nScannerId], mainFrm.dealWay[levelNum[nScannerId] - 1], mainFrm.goodsName[levelNum[nScannerId] - 1], mainFrm.hazardArea[levelNum[nScannerId] - 1], int.Parse(scanner_id[nScannerId]), out spError);
                                            if (nReturn == 1) //货位生成 
                                            {
                                                DataRow rowTask = DataBaseInterface.SelectTaskD(conn, nTask[nScannerId].ToString(), "CON");
                                                fromadd[nScannerId] = int.Parse(rowTask["from_unit"].ToString());
                                                toadd[nScannerId] = int.Parse(rowTask["to_unit"].ToString());
                                                mTasktype[nScannerId] = int.Parse(rowTask["task_type"].ToString());
                                                dTasktype[nScannerId] = int.Parse(rowTask["task_type"].ToString());
                                                loadtype[nScannerId] = mainFrm.goodsKind[levelNum[nScannerId] - 1];
                                                DownLoadNewTask(nScannerId, nTask[nScannerId], dTasktype[nScannerId], mTasktype[nScannerId], loadtype[nScannerId], fromadd[nScannerId], toadd[nScannerId], nTask[nScannerId].ToString());
                                            }
                                            else if (nReturn == -1)
                                            {
                                                MessageBox.Show("入库存储过程报错  ");
                                            }
                                            else
                                            {
                                                mainFrm.lvScanInfo_Insert(nScannerId, nTask[nScannerId].ToString(), spError);
                                                mainFrm.ChangetbScanInfoIndex(nScannerId);
                                                nBarcodeError[nScannerId] = 4;
                                            }
                                        }
                                    }
                                }
                                #endregion

                                #region 异常回库入库
                                if (mainFrm.taskType[0] == 4 && loadStruct[nScannerId].loadType != 0)
                                {
                                    if (mainFrm.ReadBarCodeFromSPs[scanner_id[nScannerId].ToString()].isStart || attempts > 0)
                                    {
                                        if (attempts == 5)
                                        {
                                            ReportError(nScannerId, 1, "", "");
                                            mainFrm.ReadBarCodeFromSPs[scanner_id[nScannerId].ToString()].ScanStopRead();
                                            DataBaseInterface.ClearHaveReadRFID(conn,scanner_id[nScannerId].ToString());
                                            attempts = 0;
                                            continue;
                                        }
                                        else
                                        {
                                            attempts++;
                                            Thread.Sleep(100);
                                            continue;
                                        }
                                    }
                                    DataBaseInterface.ClearHaveReadRFID(conn,scanner_id[nScannerId].ToString());
                                    mainFrm.ReadBarCodeFromSPs[scanner_id[nScannerId].ToString()].ScanStartRead();
                                    Thread.Sleep(500);
                                    string boxCode = DataBaseInterface.GetCurrentBarCode(conn,scanner_id[nScannerId]);
                                    if (boxCode == null)//如果无条码，原地等待，手动输入托盘号
                                    {
                                        attempts = 1;
                                        continue;
                                    }
                                    attempts = 0;
                                    mainFrm.ReadBarCodeFromSPs[scanner_id[nScannerId].ToString()].ScanStopRead();

                                    lock (DataBaseInterface.obLock)
                                    {
                                        DataRow dr = DataBaseInterface.GetErrorRebackTask(conn,levelNum[nScannerId], scanner_id[nScannerId]);//获取托盘组编号
                                        if (dr != null) //货位生成 
                                        {
                                            if (dr["BOX_BARCODE"].ToString() != boxCode)
                                            {
                                                MessageBox.Show("异常回库箱号与扫描箱号不符！");
                                                continue;
                                            }
                                            nTask[nScannerId] = int.Parse(dr["task_id"].ToString());
                                            fromadd[nScannerId] = int.Parse(dr["FROM_UNIT"].ToString());
                                            toadd[nScannerId] = int.Parse(dr["to_unit"].ToString());
                                            mTasktype[nScannerId] = int.Parse(dr["task_type"].ToString());
                                            dTasktype[nScannerId] = int.Parse(dr["task_type"].ToString());
                                            loadtype[nScannerId] = int.Parse(dr["GOODS_KIND"].ToString());
                                            DownLoadNewTask(nScannerId, nTask[nScannerId], dTasktype[nScannerId], mTasktype[nScannerId], loadtype[nScannerId], fromadd[nScannerId], toadd[nScannerId], dr["box_barcode"].ToString());

                                        }
                                        else
                                        {
                                            mainFrm.lvScanInfo_Insert(nScannerId, "未知", "托盘异常回库任务！");
                                            mainFrm.ChangetbScanInfoIndex(nScannerId);
                                            nBarcodeError[nScannerId] = 5;
                                        }
                                    }
                                }
                                #endregion
                            }
                            if (loadStruct[nScannerId].loadType != 0 && loadStruct[nScannerId].taskID != returnStruct[nScannerId].taskID)
                            {
                                DataBaseInterface.UpdateTaskIsOut2(loadStruct[nScannerId].taskID);
                                
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

        #region 判断辊道货物类型
        public ReturnStruct IsTaskStatus()
        {
            return this.returnStruct[0];
        }
        #endregion

        #region 返回可用退库辊道
        public  int GoodsStatus()
        {
            for (int i = 0; i < nDeviceID.Length; i++)
            {
                if ((nDeviceID[i] == 1014 || nDeviceID[i] == 1018 || nDeviceID[i] == 1010) && levelNum[i] == 1 && loadStruct[i].loadType == 0)
                {
                    return nDeviceID[i];
                }
            }
            return 0;
        }
        #endregion

    }
} 