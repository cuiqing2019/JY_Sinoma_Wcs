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
    public class OutConveyorCmd:Conveyor
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
        int taskerror = 0;
        public int[] systemStatusID;
        string returnBoxcode;
        public bool isSuccess = false;
        /// <summary>
        /// 扫码扫到的托盘号
        /// </summary>
        string strBoxcode = string.Empty;
        #endregion

        public Pumping pping;
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainFrm"></param>
        /// <param name="bt"></param>
        public OutConveyorCmd(frmMain mainFrm, DataTable bt)
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
            returnBoxcode = string.Empty;
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
                conveyorName[i] =row["device_tag"].ToString();
                levelNum[i] = int.Parse(row["level_no"].ToString());
                rowNum[i] = int.Parse(row["row_no"].ToString());
                nDeviceID[i] = int.Parse(row["device_name"].ToString());
                scanner_id[i] = row["IS_SCANNER"].ToString();
                nInitX = int.Parse("0" + row["x"].ToString());
                nInitY = int.Parse("0" + row["y"].ToString());
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
                lb[i].BringToFront();
                mainFrm.pLevel11.Controls.Add(lb[i]);

                InConveyorCmdThread = new Thread(new ParameterizedThreadStart(BarCodecheck));
                InConveyorCmdThread.IsBackground = true;
                // 判断该线程是否被垃圾回收
                if (!InConveyorCmdThread.IsAlive)
                {
                    InConveyorCmdThread.Start(i);
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
                catch (Exception)
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
                        if (lb[i].BackColor == Color.Red)
                            DataBaseInterface.DeviceErrorMessage(conveyorName[i], levelNum[i], rowNum[i], deviceType[i], error[i], mainFrm.deviceStatusDic.getDesc(deviceType[i], error[i].ToString()), loadStruct[i].taskID);
                        if (systemstatus.GetAuto(levelNum[i]) == "自动")
                        {
                            if (this.loadStruct[i].loadType == 0)
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
                          //  mainFrm.speech.speech("入库口扫码器输送" + this.conveyorName[i].ToString() + mainFrm.ConveyorError(deviceType[i], error[i]));
                            if (error[i] != 15)
                                DataBaseInterface.DeviceErrorMessage(conveyorName[i], levelNum[i], rowNum[i], deviceType[i], error[i], mainFrm.deviceStatusDic.getDesc(deviceType[i], error[i].ToString()), loadStruct[i].taskID);
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

        #region 报错信息显示
        /// <summary>
        /// 报错信息显示, 
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
                    strError = "一楼出库扫码区，箱号:" + strBarcode + "\r\n" + "与辊道加载任务号：" + loadStruct[nScannerId].taskID.ToString() + "所属托盘不匹配！";
                    break;
                case 3:
                    strError = "箱号:" + strBarcode + "\r\n" + "任务下发失败！" + strReturn + "！";
                    break;
              
            }
           // mainFrm.speech.speech(strError);
            mainFrm.lvScanInfo_Insert(4, strBarcode, strError);
            mainFrm.ChangetbScanInfoIndex(1);
            if (lastStrError[nScannerId] != strError)
            {
                MessageBox.Show(strError);
                if (nError == 1)
                    DataBaseInterface.DeviceErrorMessage(conveyorName[nScannerId], levelNum[nScannerId], rowNum[nScannerId], "CON", 0, "辊道下任务失败", loadStruct[nScannerId].taskID);
            }
            nBarcodeError[nScannerId ] = nError;
            lastStrError[nScannerId ] = strError;
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
        private void DownLoadNewTask(int nScannerID, int nTaskid, int dTaskType, int mTaskType, int nloadType, int nFromAdd, int nToAdd, string strBarcode)
        {
            int i;
            for (i = 0; i < 20; i++)
            {
                WriteCmd(nScannerID, nTaskid, dTaskType, nFromAdd, nToAdd, nloadType);
                Thread.Sleep(200);
                RefreshStatus();
                if (returnStruct[nScannerID].taskID == nTaskid)
                {
                    mainFrm.lvScanInfo_Insert(1, strBarcode, "一楼出库任务报完成成功" + "任务号：" + nTaskid.ToString() + "目的地址：" + nToAdd.ToString());
                    mainFrm.ChangetbScanInfoIndex(1);
                    i = 0;
                    nBarcodeError[nScannerID] = 0;
                    break;
                }
            }
            if (i == 10)
                ReportError(nScannerID, 3, strBarcode, "");
        }
        #endregion

        #region 条码检测、辊道下任务
        public void BarCodecheck(object ScannerId)
        {
            int nScannerId = int.Parse(ScannerId.ToString());
            int nReturn = 0;
            int errorBoxcode = 0;
            string rs = "";

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
                            #region 生成空吨桶入横梁区
                            if (mainFrm.pumping.PointStatus(conn,2, out strBoxcode) != null && mainFrm.agv.auto)
                            {
                                if (strBoxcode != null && DataBaseInterface.HavingAGVTask(conn,strBoxcode) == 0 && strBoxcode != ""&&mainFrm.AGVTaskCreate)
                                {
                                    string toUnit = DataBaseInterface.SelectAGVLocation(conn);
                                    string fromUnit = mainFrm.pumping.PointStatus(conn,2, out strBoxcode);
                                    if (toUnit != null)
                                        DataBaseInterface.AGVTaskCreate(conn,strBoxcode, 1, fromUnit, toUnit, loadStruct[nScannerId].taskID.ToString(), out rs);
                                    else
                                    {
                                        MessageBox.Show("无可用空吨桶缓存库位！");
                                        Thread.Sleep(200);
                                    }
                                }
                            }
                            #endregion
                            if (mainFrm.taskType[levelNum[nScannerId] - 1] == 1)
                            {
                                RefreshStatus();
                                DataRow taskrow = null;
                                taskrow = DataBaseInterface.SelectTaskD(conn,returnStruct[nScannerId].taskID.ToString(), "CON");
                                if (taskrow == null)
                                    continue;
                                if (taskrow["status"].ToString() != "2" && loadStruct[nScannerId].loadType != 0 && returnStruct[nScannerId].status == 1 && nBarcodeError[nScannerId] == 0)
                                {
                                    if (mainFrm.ReadBarCodeFromSPs[scanner_id[nScannerId].ToString()].isStart || errorBoxcode > 0)
                                    {
                                        if (errorBoxcode == 20)//增加条码循环次数  5
                                        {
                                            ReportError(nScannerId, 1, "", "");
                                            mainFrm.ReadBarCodeFromSPs[scanner_id[nScannerId].ToString()].ScanStopRead();
                                            DataBaseInterface.ClearHaveReadRFID(conn,scanner_id[nScannerId].ToString());
                                            errorBoxcode = 0;
                                            continue;
                                        }
                                        else
                                        {
                                            errorBoxcode++;
                                            Thread.Sleep(1000);//修改线程刷新时间 100
                                            continue;
                                        }
                                    }
                                    DataBaseInterface.ClearHaveReadRFID(conn,scanner_id[nScannerId].ToString());
                                    mainFrm.ReadBarCodeFromSPs[scanner_id[nScannerId].ToString()].ScanStartRead();
                                    Thread.Sleep(500);
                                    strBoxcode = DataBaseInterface.GetCurrentBarCode(conn,scanner_id[nScannerId]);
                                    if (strBoxcode == null)//如果无条码，原地等待，手动输入托盘号
                                    {
                                        errorBoxcode = 1;
                                        continue;
                                    }
                                    errorBoxcode = 0;
                                    mainFrm.ReadBarCodeFromSPs[scanner_id[nScannerId].ToString()].ScanStopRead();
                                }
                                else if (taskrow["status"].ToString() == "2")
                                    strBoxcode = taskrow["box_barcode"].ToString();
                                else
                                    strBoxcode = string.Empty;
                                #region 出库扫码段
                                #region 一楼吨桶出库
                                if (levelNum[nScannerId] == 1 && strBoxcode != string.Empty&&error[nScannerId]==0 && loadStruct[nScannerId].loadType>0 && mainFrm.PumpingStaus) //一楼吨桶出库扫码段
                                {
                                    if (strBoxcode == taskrow["BOX_BARCODE"].ToString()||isSuccess)//查询辊道托盘号是否与任务托盘号一致
                                    {
                                        isSuccess = false;
                                        strBoxcode = taskrow["BOX_BARCODE"].ToString();
                                        returnBoxcode = strBoxcode;
                                        lock (DataBaseInterface.obLock)
                                        {
                                            if (mainFrm.taskType[levelNum[nScannerId] - 1] == 1 && mainFrm.agv.auto && strBoxcode!="")
                                            {
                                                #region 如果已经生成AGV小车任务则跳过
                                                if (DataBaseInterface.HavingAGVTask(conn,strBoxcode) == 1)
                                                {
                                                    Thread.Sleep(500);
                                                    continue;
                                                }
                                                #endregion
                                                #region 如果没有生成AGV小车任务则生成AGV小车任务
                                                else
                                                {
                                                    #region 生成出库吨桶入取液处任务
                                                    if (mainFrm.outConveyorCmd.OutBoundBoxCode() != null && mainFrm.outConveyorCmd.OutBoundBoxCode() != string.Empty && mainFrm.agv.auto&&mainFrm.AGVTaskCreate)
                                                    {
                                                        string fromUnit = "1025";
                                                        string toUnit = mainFrm.pumping.PointStatus(conn,1, out strBoxcode);
                                                        if (toUnit != null)
                                                        {
                                                            strBoxcode = mainFrm.outConveyorCmd.OutBoundBoxCode();
                                                            int nRe=DataBaseInterface.AGVTaskCreate(conn,strBoxcode, 3, fromUnit, toUnit, loadStruct[nScannerId].taskID.ToString(), out rs);
                                                            if(nRe==1)
                                                              mainFrm.pumping.PointBoxCodeUpdate(strBoxcode, toUnit);
                                                            else
                                                            {
                                                                Thread.Sleep(500);
                                                                continue;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Thread.Sleep(500);
                                                            continue;
                                                        }
                                                    }
                                                    #endregion

                                                    Thread.Sleep(200);
                                                }
                                                #endregion
                                            }
                                            if ((mainFrm.taskType[levelNum[nScannerId] - 1] == 1 || mainFrm.taskType[levelNum[nScannerId] - 1] == 3 || mainFrm.taskType[levelNum[nScannerId] - 1] == 4) && taskrow["status"].ToString() != "2")
                                            {
                                                nReturn = DataBaseInterface.TaskStatusUpdate(loadStruct[nScannerId].taskID, nDeviceID[nScannerId], conveyorName[nScannerId], 2, 2, out rs);//任务报完成
                                                if (nReturn != 1)
                                                {
                                                    MessageBox.Show("任务状态修改存储过程报错" + error);
                                                    Thread.Sleep(200);
                                                    continue;
                                                }
                                                mainFrm.lvScanInfo_Insert(int.Parse(scanner_id[nScannerId])-1, taskrow["BOX_BARCODE"].ToString(), "任务号：" + loadStruct[nScannerId].taskID.ToString() + "  出库任务完成！");
                                            }
                                        }
                                    }
                                    else if (strBoxcode != string.Empty)
                                    {
                                        returnBoxcode = string.Empty;
                                        ReportError(nScannerId, 2, taskrow["BOX_BARCODE"].ToString(), "");
                                        strBoxcode = string.Empty;
                                        Thread.Sleep(200);
                                        continue;
                                    }
                                }
                                #endregion
                                #endregion
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("扫码下任务" + ex.Message);
                        //throw;
                    }
                }
                    
                Thread.Sleep(300);
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

        #region 判断辊道是否有货
        /// <summary>
        /// 判断一楼吨桶出库口待取的托盘的托盘号
        /// </summary>
        /// <returns>货物状态为2是托盘已到agv取货口，返回托盘号，为1是托盘在站台但未到agv取货口但辊道有货，返回空字符串，0为无货，返回null</returns>
        public string OutBoundBoxCode()
        {
            if (loadStruct[0].loadType == 2 && nBarcodeError[0] == 0 && returnStruct[0].status == 1)
                return returnBoxcode;
            else if (loadStruct[0].loadType == 0)
                return null;
            else
                return string.Empty;
        }
        #endregion
    }
}
