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
    public class FirstFloorInConveyorCmd : Conveyor
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

        string str_unit;
        string str_units;
        int pd = 0;//判断出库口
        int attempts = 0;//未扫到条码判断
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
        public FirstFloorInConveyorCmd(frmMain mainFrm, DataTable bt)
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
                lb[i].Size = new Size( int.Parse(row["length"].ToString()),int.Parse(row["width"].ToString()));
                lb[i].Location = new Point(int.Parse(row["x"].ToString()), int.Parse(row["y"].ToString()));
                lb[i].BackColor = Color.Gray;
                lb[i].Font = new Font("宋体", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                lb[i].TextAlign = ContentAlignment.MiddleCenter;
                lb[i].DoubleClick += new System.EventHandler(this.lb_DoubleClick);
                lb[i].Tag = i.ToString();
                lb[i].BringToFront();
                mainFrm.PLevel1.Controls.Add(lb[i]);
            
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
            string taskDesc = "";
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
                    }
                    else if (mTaskType == 3)
                    {
                        taskDesc = "空托盘入库任务生成成功！";
                    }
                    else if (mTaskType == 5)
                    {
                        taskDesc = "一楼异常回库任务下发成功";
                    }
                    DataBaseInterface.TaskStatusUpdate(nTaskid, nDeviceID[nScannerID], conveyorName[nScannerID], 2, 1, out taskDesc);//修改任务状态为执行中
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
            string rs_ret="";
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

                            RefreshStatus();
                            //if (error[nScannerId] == 31 || error[nScannerId] == 32)  //外形超高，异常排出
                            //{
                            //    DataBaseInterface.UpdateRFIDStatus(scanner_id[nScannerId]);//将已经读到的rfid标识清空
                            //
                            //    continue;
                            //}
                            //else
                            //{
                            //}
                            if (this.returnStruct[nScannerId].status == 10 && mainFrm.bPicking && nBarcodeError[nScannerId] == 0 && !mainFrm.stopTaskCreate[levelNum[nScannerId] - 1])
                            {
                                if (loadStruct[nScannerId].loadType != 0)
                                {
                                    if (mainFrm.goodsKind[levelNum[nScannerId] - 1] == 0 || mainFrm.goodsSku[levelNum[nScannerId] - 1] == string.Empty)
                                        continue;

                                    string boxCode = DataBaseInterface.GetBarCode(conn,loadStruct[nScannerId].taskID);
                                    nReturn = DataBaseInterface.TaskStatusUpdate(loadStruct[nScannerId].taskID, nDeviceID[nScannerId], conveyorName[nScannerId], 1, 2, out rs_ret);
                                    if (nReturn == 1)
                                    {
                                        DataRow rowTask = DataBaseInterface.SelectTaskD(conn,loadStruct[nScannerId].taskID.ToString(), "CON");
                                        fromadd[nScannerId] = int.Parse(rowTask["from_unit"].ToString());
                                        toadd[nScannerId] = int.Parse(rowTask["to_unit"].ToString());
                                        mTasktype[nScannerId] = int.Parse(rowTask["task_type"].ToString());
                                        dTasktype[nScannerId] = int.Parse(rowTask["task_type"].ToString());
                                        loadtype[nScannerId] = mainFrm.goodsKind[levelNum[nScannerId] - 1];
                                        DownLoadNewTask(nScannerId, loadStruct[nScannerId].taskID, dTasktype[nScannerId], mTasktype[nScannerId], loadtype[nScannerId], fromadd[nScannerId], toadd[nScannerId], boxCode);
                                    }
                                    else
                                    {
                                        MessageBox.Show("入库任务报完成过程报错:" + rs_ret);
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("入库下任务：" + ex.Message);
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

    }
} 