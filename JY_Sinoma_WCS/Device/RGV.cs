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
using PLC;
using MySql.Data.MySqlClient;

namespace JY_Sinoma_WCS
{
    public class RGV : Conveyor
    {

        #region 自定义变量
        /// <summary>
        /// 线程
        /// </summary>
        public Thread RGVThread;
        public int[] nBarcodeError;
        public string[] lastStrError;
        public int[] nTask;
        public int[] fromadd;
        public int[] toadd;
        public int[] mTasktype;//zhu ren wu lei xing
        public int[] dTasktype;// zi renwu leixing 
        public int[] loadtype;
        public int[] systemStatusID;
        string str_unit;
        string str_units;
        int attempts = 0;
        public Dictionary<string, int> inConveyorCmdId = new Dictionary<string, int>();
        /// <summary>
        /// 人工输入托盘号
        /// </summary>
        public string inputBoxCode = string.Empty;
        public bool isSuccess = false;
        int row = 0;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainFrm"></param>
        /// <param name="bt"></param>
        public RGV(frmMain mainFrm, DataTable bt)
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
            nDeviceID = new int[nCount];
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
                conveyorName[i] = row["device_tag"].ToString();
                nDeviceID[i] = int.Parse(row["device_name"].ToString());
                nInitX = int.Parse("0" + row["x"].ToString());
                nInitY = int.Parse("0" + row["y"].ToString());
                scanner_id[i] = row["IS_SCANNER"].ToString();
                deviceType[i] = row["device_mold"].ToString();
                systemStatusID[i] = int.Parse(row["system_status_id"].ToString());
                //定义图片
                lb[i] = new Label();
                lb[i].Size = new Size( int.Parse(row["length"].ToString()),int.Parse(row["width"].ToString()));
                lb[i].Location = new Point(int.Parse(row["x"].ToString()), int.Parse(row["y"].ToString()));
                lb[i].BackColor = Color.Gray;
                lb[i].Font = new Font("宋体", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                lb[i].TextAlign = ContentAlignment.MiddleCenter;
                lb[i].DoubleClick += new System.EventHandler(this.lb_DoubleClick);
                lb[i].Tag = i.ToString();
                mainFrm.pRGV.Controls.Add(lb[i]);
                lb[i].BringToFront();
                if (scanner_id[i] != null)
                {
                    RGVThread = new Thread(new ParameterizedThreadStart(BarCodecheck));
                    RGVThread.IsBackground = true;
                    // 判断该线程是否被垃圾回收
                    if (!RGVThread.IsAlive)
                    {
                        RGVThread.Start(i);
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
                    if (error[i] == 0 && nBarcodeError[i] == 0)
                    {
                        if (lb[i].BackColor == Color.Red)
                            DataBaseInterface.DeviceErrorMessage(conveyorName[i], levelNum[i], 0, deviceType[i], error[i], mainFrm.deviceStatusDic.getDesc(deviceType[i], error[i].ToString()), loadStruct[i].taskID);
                        if (systemstatus.GetAuto(levelNum[i]) == "自动")
                        {
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
                      //      mainFrm.speech.speech("入库口扫码器输送" + this.conveyorName[i].ToString() + mainFrm.ConveyorError(deviceType[i], error[i]));
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
        public bool DownLoadNewTask(int RGVID, int nTaskid, int dTaskType, int mTaskType, int nloadType, int nFromAdd, int nToAdd, string strBarcode)
        {
            int i;
            string rs;
            string taskDesc = "";
            for (i = 0; i < 10; i++)
            {
                WriteCmd(RGVID, nTaskid, dTaskType, nFromAdd, nToAdd, nloadType);
                Thread.Sleep(1000);
                RefreshStatus();


                if (returnStruct[RGVID].taskID == nTaskid)
                {
                    if (mTaskType == 2 && nToAdd != 1003)//圆桶出库、退库
                    {
                        taskDesc = "二楼圆桶出库任务报完成成功";
                        DataBaseInterface.TaskStatusUpdate(nTaskid-1000,nDeviceID[RGVID],conveyorName[RGVID],2,2,out rs);//修改任务状态为已完成

                    }
                    else if(mTaskType==2&&nToAdd==1003)
                    {
                        taskDesc = "二楼圆桶出库任务报执行中成功";
                        DataBaseInterface.TaskStatusUpdate(nTaskid, nDeviceID[RGVID], conveyorName[RGVID], 2, 1, out rs);//修改任务状态为执行中
                    }
                    else if (mTaskType == 3 && nToAdd !=1003)//空托盘入库
                    {
                        taskDesc = "二楼空托入库任务报执行中成功";
                        DataBaseInterface.TaskStatusUpdate(nTaskid-1000, nDeviceID[RGVID], conveyorName[RGVID], 1, 1, out rs);//修改任务状态为执行中
                        DataBaseInterface.UpdateRFIDStatus(scanner_id[RGVID]);//将已经读到的rfid标识清空
                    }
                    else if (mTaskType == 5 && nToAdd != 1003)//异常回库
                    {
                        taskDesc = "二楼异常回库任务报执行中成功";
                        DataBaseInterface.TaskStatusUpdate(nTaskid-1000, nDeviceID[RGVID], conveyorName[RGVID], 1, 1, out rs);//修改任务状态为已完成
                    }
                    mainFrm.lvScanInfo_Insert(int.Parse(scanner_id[RGVID])-1, strBarcode, taskDesc + "任务号：" + nTaskid.ToString() + "目的地址：" + nToAdd.ToString());
                    mainFrm.ChangetbScanInfoIndex(int.Parse(scanner_id[RGVID]) - 1);
                    i = 0;
                    nBarcodeError[RGVID] = 0;
                    return true;
                }
            }
            if (i == 10)
            {
                ReportError(RGVID, 3, strBarcode, "");
                return false;
            }
            return false;
        }
        #endregion
        
        #region 报错信息显示
        /// <summary>
        /// 报错信息显示
        /// </summary>
        /// <param name="nError"></param>
        /// <param name="strBarcode"></param>
        /// <param name="strReturn"></param>
        private void ReportError(int RGVID, int nError, string strBarcode, string strReturn)
        {
            string strError = string.Empty;
            switch (nError)
            {
                case 1:
                    strError = "扫码异常：辊道有货，未正确扫描到条码";
                    break;
                case 2:
                    strError = "二楼出库扫码区，箱号:" + strBarcode + "\r\n" + "与RGV加载任务号：" + loadStruct[RGVID].taskID.ToString() + "所属托盘不匹配！";
                    break;
                case 3:
                    strError = "箱号:" + strBarcode + "\r\n" + "任务下发失败！" + strReturn + "！";
                    break;
            }
            //    mainFrm.speech.speech(strError);
            mainFrm.lvScanInfo_Insert(int.Parse(scanner_id[RGVID]) - 1, strBarcode, strError);
            mainFrm.ChangetbScanInfoIndex(int.Parse(scanner_id[RGVID]) - 1);
            if (lastStrError[RGVID] != strError)
            {
                MessageBox.Show(strError);
                if (nError == 1)
                    DataBaseInterface.DeviceErrorMessage(conveyorName[RGVID], levelNum[RGVID], 0, "CON", 0, "辊道下任务失败", loadStruct[RGVID].taskID);
            }

            nBarcodeError[RGVID] = nError;
            lastStrError[RGVID] = strError;
        }
        #endregion

        #region 辊道下任务
        public void BarCodecheck(object inConveyorCmdId)
        {
            int RGVID = int.Parse(inConveyorCmdId.ToString());
            int nReturn = 1;
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
                            if ((mainFrm.bPicking && nBarcodeError[RGVID] == 0) && !mainFrm.stopTaskCreate[levelNum[RGVID] - 1])
                            {
                                #region 判断任务类型 1-出库任务 2-托盘入库 4-异常回库
                                switch (mainFrm.taskType[1])
                                {
                                    #region 如果是出库任务(主任务报完成)
                                    case 1://出入库
                                        if (returnStruct[RGVID].status == 10 && loadStruct[RGVID].loadType == 0 && nBarcodeError[RGVID] == 0)
                                        {
                                            DataRow rgvTask = DataBaseInterface.SelectTaskD(conn);
                                            if (rgvTask != null)
                                            {
                                                if (returnStruct[RGVID].status == 10 && int.Parse(rgvTask["TASK_ID"].ToString()) == mainFrm.outElectricsConveyor.IsHaveGoods(int.Parse(rgvTask["from_unit"].ToString()), 2).taskID && mainFrm.outElectricsConveyor.IsHaveGoods(int.Parse(rgvTask["from_unit"].ToString()), 2).loadType != 0)
                                                {
                                                    nTask[RGVID] = int.Parse(rgvTask["task_id"].ToString());
                                                    fromadd[RGVID] = int.Parse(rgvTask["from_unit"].ToString());
                                                    toadd[RGVID] = 1003;
                                                    mTasktype[RGVID] = int.Parse(rgvTask["TASK_TYPE"].ToString());
                                                    dTasktype[RGVID] = int.Parse(rgvTask["TASK_TYPE"].ToString());
                                                    loadtype[RGVID] = 2;
                                                    DownLoadNewTask(RGVID, nTask[RGVID], dTasktype[RGVID], mTasktype[RGVID], loadtype[RGVID], fromadd[RGVID], toadd[RGVID], rgvTask["BOX_BARCODE"].ToString());
                                                }
                                                if (returnStruct[RGVID].status == 10 && mainFrm.allElectricsConveyor.IsHaveGoods(int.Parse(rgvTask["from_unit"].ToString()), 2).loadType != 0)
                                                {
                                                    nTask[RGVID] = int.Parse(rgvTask["task_id"].ToString());
                                                    fromadd[RGVID] = int.Parse(rgvTask["from_unit"].ToString());
                                                    toadd[RGVID] = 1003;
                                                    mTasktype[RGVID] = int.Parse(rgvTask["TASK_TYPE"].ToString());
                                                    dTasktype[RGVID] = int.Parse(rgvTask["TASK_TYPE"].ToString());
                                                    loadtype[RGVID] = 2;
                                                    DownLoadNewTask(RGVID, nTask[RGVID], dTasktype[RGVID], mTasktype[RGVID], loadtype[RGVID], fromadd[RGVID], toadd[RGVID], rgvTask["BOX_BARCODE"].ToString());
                                                }
                                            }
                                        }
                                        else if (returnStruct[RGVID].status == 11 && nBarcodeError[RGVID] == 0 && loadStruct[RGVID].loadType != 0 && mainFrm.conveyorLoad.IsHaveGoods(1004, 2).loadType == 0)
                                        {
                                            if (mainFrm.ReadBarCodeFromSPs[scanner_id[RGVID].ToString()].isStart || attempts > 0)
                                            {
                                                if (attempts == 5)
                                                {
                                                    ReportError(RGVID, 1, "", "");
                                                    mainFrm.ReadBarCodeFromSPs[scanner_id[RGVID].ToString()].ScanStopRead();
                                                    DataBaseInterface.ClearHaveReadRFID(conn, scanner_id[RGVID].ToString());
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
                                            DataBaseInterface.ClearHaveReadRFID(conn, scanner_id[RGVID].ToString());
                                            mainFrm.ReadBarCodeFromSPs[scanner_id[RGVID].ToString()].ScanStartRead();
                                            Thread.Sleep(500);
                                            string boxCode = DataBaseInterface.GetCurrentBarCode(conn,scanner_id[RGVID]);
                                            if (boxCode == null)//如果无条码，原地等待，手动输入托盘号
                                            {
                                                attempts = 1;
                                                continue;
                                            }
                                            attempts = 0;
                                            mainFrm.ReadBarCodeFromSPs[scanner_id[RGVID].ToString()].ScanStopRead();

                                            string spError = string.Empty;
                                            DataRow rgvTask = DataBaseInterface.SelectTaskD(conn,returnStruct[RGVID].taskID.ToString(), "CON");
                                            if (rgvTask != null)
                                            {
                                                if ( isSuccess||boxCode == rgvTask["BOX_BARCODE"].ToString() && boxCode != string.Empty && rgvTask["BOX_BARCODE"].ToString() != string.Empty)
                                                {
                                                    isSuccess = false;
                                                    if (nReturn == 1)
                                                    {
                                                        nTask[RGVID] = int.Parse(rgvTask["task_id"].ToString())+1000;
                                                        fromadd[RGVID] = 1003;
                                                        toadd[RGVID] = int.Parse(rgvTask["TO_UNIT"].ToString());
                                                        mTasktype[RGVID] = int.Parse(rgvTask["TASK_TYPE"].ToString());
                                                        dTasktype[RGVID] = int.Parse(rgvTask["TASK_TYPE"].ToString());
                                                        loadtype[RGVID] = 2;
                                                        DownLoadNewTask(RGVID, nTask[RGVID], dTasktype[RGVID], mTasktype[RGVID], loadtype[RGVID], fromadd[RGVID], toadd[RGVID], rgvTask["BOX_BARCODE"].ToString());
                                                    }
                                                    else
                                                    {
                                                        MessageBox.Show("出库存储过程报错");
                                                    }
                                                }
                                                if (boxCode != rgvTask["BOX_BARCODE"].ToString() && boxCode != string.Empty && rgvTask["BOX_BARCODE"].ToString() != string.Empty)
                                                    ReportError(0, 2, boxCode, "");
                                            }
                                        }
                                        break;
                                    #endregion

                                    #region 空托盘入库
                                    case 2://空托盘入库//conveyorLoad
                                        if (nBarcodeError[RGVID] == 0 && returnStruct[RGVID].status == 10 && loadStruct[RGVID].loadType == 0 && mainFrm.conveyorLoad.IsHaveGoods(1004, 2).loadType != 0)
                                        {
                                            nTask[RGVID] = int.Parse(DataBaseInterface.GetTaskM(conn));
                                            //DataRow rowTask = DataBaseInterface.SelectTaskD(conn, nTask[RGVID].ToString(), "CON");
                                            //nTask[RGVID] = loadStruct[RGVID].taskID+1;
                                            //nTask[RGVID] = int.Parse(rowTask["task_id"].ToString())-1;
                                            fromadd[RGVID] = 1004;
                                            toadd[RGVID] = 1003;
                                            mTasktype[RGVID] = 3;
                                            dTasktype[RGVID] = 3;
                                            loadtype[RGVID] = 3;
                                            DownLoadNewTask(RGVID, nTask[RGVID], dTasktype[RGVID], mTasktype[RGVID], loadtype[RGVID], fromadd[RGVID], toadd[RGVID], "");
                                        }
                                        else if (returnStruct[RGVID].status == 11 && loadStruct[RGVID].loadType != 0 && mainFrm.allElectricsConveyor.IsHaveGoods(1001, 2).loadType == 0)
                                        {

                                            lock (DataBaseInterface.obLock)
                                            {
                                                string spError = string.Empty;
                                                //nTask[RGVID] = int.Parse(DataBaseInterface.GetTaskM(conn));

                                                nReturn = DataBaseInterface.CreatInboundTaskM(conn,nTask[RGVID].ToString(), 3, mainFrm.batchNo[levelNum[RGVID] - 1], mainFrm.batchid[levelNum[RGVID] - 1], mainFrm.goodsKind[levelNum[RGVID] - 1], mainFrm.goodsSku[levelNum[RGVID] - 1], 0, levelNum[RGVID], nTask[RGVID], mainFrm.dealWay[levelNum[RGVID] - 1], mainFrm.goodsName[levelNum[RGVID] - 1], mainFrm.hazardArea[levelNum[RGVID] - 1], int.Parse(scanner_id[RGVID]), out spError);
                                                if (nReturn == 1) //货位生成 
                                                {
                                                    DataRow rowTask = DataBaseInterface.SelectTaskD(conn,nTask[RGVID].ToString(), "CON");
                                                    if (rowTask != null)
                                                    {
                                                        //nTask[RGVID] = int.Parse(rowTask["task_id"].ToString());
                                                        nTask[RGVID] = loadStruct[RGVID].taskID +1000;
                                                        fromadd[RGVID] = 1003;//1003
                                                        toadd[RGVID] = int.Parse(rowTask["to_unit"].ToString());//1001
                                                        mTasktype[RGVID] = int.Parse(rowTask["task_type"].ToString());
                                                        dTasktype[RGVID] = int.Parse(rowTask["task_type"].ToString());
                                                        loadtype[RGVID] = 3;
                                                        DownLoadNewTask(RGVID, nTask[RGVID], dTasktype[RGVID], mTasktype[RGVID], loadtype[RGVID], fromadd[RGVID], toadd[RGVID], nTask[RGVID].ToString());
                                                    }
                                                }
                                                else if (nReturn == -1)
                                                {
                                                    MessageBox.Show("入库存储过程报错  ");
                                                }
                                                else
                                                {
                                                    mainFrm.lvScanInfo_Insert(2, nTask[RGVID].ToString(), spError);
                                                    mainFrm.ChangetbScanInfoIndex(2);
                                                }
                                            }
                                        }
                                        break;
                                    #endregion

                                    #region 异常回库入库
                                    case 4://异常回库入库

                                        if (returnStruct[RGVID].status == 11 && loadStruct[RGVID].loadType != 0 && nBarcodeError[RGVID] == 0)
                                        {
                                            lock (DataBaseInterface.obLock)
                                            {

                                                if (mainFrm.ReadBarCodeFromSPs[scanner_id[RGVID].ToString()].isStart || attempts > 0)
                                                {
                                                    if (attempts == 5)
                                                    {
                                                        ReportError(RGVID, 1, "", "");
                                                        mainFrm.ReadBarCodeFromSPs[scanner_id[RGVID].ToString()].ScanStopRead();
                                                        DataBaseInterface.ClearHaveReadRFID(conn,scanner_id[RGVID].ToString());
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
                                                DataBaseInterface.ClearHaveReadRFID(conn,scanner_id[RGVID].ToString());
                                                mainFrm.ReadBarCodeFromSPs[scanner_id[RGVID].ToString()].ScanStartRead();
                                                Thread.Sleep(500);
                                                string boxCode = DataBaseInterface.GetCurrentBarCode(conn,scanner_id[RGVID]);
                                                if (boxCode == null)//如果无条码，原地等待，手动输入托盘号
                                                {
                                                    attempts = 1;
                                                    continue;
                                                }
                                                attempts = 0;
                                                mainFrm.ReadBarCodeFromSPs[scanner_id[RGVID].ToString()].ScanStopRead();

                                                DataRow dr = DataBaseInterface.GetErrorRebackTask(conn,levelNum[RGVID], "6");//获取异常回库托盘标识
                                                if (dr != null) //货位生成 
                                                {
                                                    if (dr["BOX_BARCODE"].ToString() != boxCode)
                                                    {
                                                        MessageBox.Show("异常回库箱号与扫描箱号不符！");
                                                        continue;
                                                    }
                                                    nTask[RGVID] = int.Parse(dr["task_id"].ToString())+1000;
                                                    fromadd[RGVID] = int.Parse(dr["FROM_UNIT"].ToString());
                                                    toadd[RGVID] = int.Parse(dr["to_unit"].ToString());
                                                    mTasktype[RGVID] = int.Parse(dr["task_type"].ToString());
                                                    dTasktype[RGVID] = int.Parse(dr["task_type"].ToString());
                                                    loadtype[RGVID] = int.Parse(dr["GOODS_KIND"].ToString());
                                                    DownLoadNewTask(RGVID, nTask[RGVID], dTasktype[RGVID], mTasktype[RGVID], loadtype[RGVID], fromadd[RGVID], toadd[RGVID], boxCode);
                                                }
                                                else
                                                {
                                                    mainFrm.lvScanInfo_Insert(2, "未知", "未找到当前辊道异常回库任务");
                                                    mainFrm.ChangetbScanInfoIndex(2);
                                                }
                                            }
                                        }
                                        #endregion
                                        break;
                                    default:
                                        break;
                                }
                                #endregion

                            }

                            Thread.Sleep(200);
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
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

        #region 辊道下任务
        /// <summary>
        /// 辊道下任务
        /// </summary>
        public new void WriteCmd(int index, int nTaskID, int nTaskType, int FromLev, int ToLev, int loadType)
        {
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
    }
}
