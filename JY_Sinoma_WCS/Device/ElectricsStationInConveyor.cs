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
    /// <summary>
    /// 堆垛机取货链条机类
    /// </summary>
    public class ElectricsStationInConveyor : Conveyor
    {
        #region 自定义变量
        public int[] nTask;
        public int[] fromadd;
        public int[] toadd;
        public int[] mTasktype;//zhu ren wu lei xing
        public int[] dTasktype;// zi renwu leixing 
        public int[] loadtype;
        public int[] systemStatusID;
        public string[] lastStrError;
        /// <summary>
        /// 任务更新
        /// </summary>
        Thread electricsThread;
        #endregion

        #region 构造函数
        public ElectricsStationInConveyor(frmMain mainFrm, DataTable bt) : base(mainFrm, bt)
        {
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
            lastStrError = new String[nCount];
            int i = 0;
            foreach (DataRow row in bt.Rows)
            {
                nTask[i] = 0;
                fromadd[i] = 0;
                toadd[i] = 0;
                mTasktype[i] = 0;
                dTasktype[i] = 0;
                loadtype[i] = 0;
                lastStrError[i] = string.Empty;
                errorDB[i] = row["error_db"].ToString();
                controlDB[i] = row["control_db"].ToString();
                loadDB[i] = row["load_db"].ToString();
                writeDB[i] = row["write_db"].ToString();
                returnDB[i] = row["return_db"].ToString();

                nInitX = int.Parse(row["x"].ToString());
                nInitY = int.Parse(row["y"].ToString());
                deviceType[i] = row["device_type"].ToString();
                conveyorName[i] = row["device_tag"].ToString();
                levelNum[i] = int.Parse(row["level_no"].ToString());
                rowNum[i] = int.Parse(row["row_no"].ToString());
                channelNum[i] = int.Parse(row["channel_no"].ToString());
                nDeviceID[i] = int.Parse(row["device_name"].ToString());
                deviceType[i] = row["device_mold"].ToString();
                systemStatusID[i] = int.Parse(row["system_status_id"].ToString());

                lb[i] = new Label();
                lb[i].Size = new Size(int.Parse(row["length"].ToString()), int.Parse(row["width"].ToString()));
                lb[i].Location = new Point(int.Parse(row["x"].ToString()), int.Parse(row["y"].ToString()));
                lb[i].BackColor = Color.Gray;
                lb[i].Font = new Font("宋体", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                lb[i].TextAlign = ContentAlignment.MiddleCenter;
                lb[i].DoubleClick += new System.EventHandler(this.lb_DoubleClick);
                lb[i].Tag = i.ToString();
                mainFrm.PLevel1.Controls.Add(lb[i]);
                lb[i].BringToFront();

                electricsThread = new Thread(new ParameterizedThreadStart(AutoUpdate));
                electricsThread.IsBackground = true;
                // 判断该线程是否被垃圾回收
                if (!electricsThread.IsAlive)
                {
                    electricsThread.Start(i);
                }
                i++;
            }
        }
        #endregion

        #region 图片双击事件
        public override void lb_DoubleClick(object sender, EventArgs e)
        {
            string strTag = Convert.ToString(((Label)sender).Tag);
            int Index = int.Parse(strTag);
            //mainFrm.ChangetbScanInfoIndex((rowNum[Index] - 1) * 2 + levelNum[Index]-1);
            FormElectricsConveyor frmConveyor = new FormElectricsConveyor(this, Index, mainFrm.systemStatus);
            frmConveyor.StartPosition = FormStartPosition.CenterParent;
            frmConveyor.ShowDialog();
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

            SetState(true);
            isBindToPLC = true;
            return isBindToPLC;
        }
        #endregion

        #region PLC读取数据字符串拆分方法
        /// <summary>
        /// PLC读取数据字符串拆分方法
        /// </summary>
        /// <param name="str"></param>
        /// <param name="value"></param>
        protected void GetValue(string str, int[] value)
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

        #region 刷新辊道状态
        /// <summary>
        /// 刷新辊道状态
        /// </summary>
        public override void RefreshStatus()
        {
            if (isBindToPLC && mainFrm.deviceLinkIsOK)
            {
                try
                {
                    //读取辊道状态                       
                    object[] readValues = new object[returnDB.Length];
                    if (!SyncRead(readValues, returnHandle))
                    {
                        mainFrm.DeviceDisConnection();
                        mainFrm.SetClosing(true);
                        return;
                    }
                    int[] value = new int[5];
                    for (int i = 0; i < returnDB.Length; i++)
                    {
                        GetValue(readValues[i].ToString(), value);
                        returnStruct[i].taskID = int.Parse(value[0].ToString());
                        returnStruct[i].taskType = int.Parse(value[1].ToString());
                        returnStruct[i].from = int.Parse(value[2].ToString());
                        returnStruct[i].to = int.Parse(value[3].ToString());
                        returnStruct[i].status = int.Parse(value[4].ToString());
                    }

                    readValues = new object[loadDB.Length];
                    if (!SyncRead(readValues, loadHandle))
                    {
                        mainFrm.DeviceDisConnection();
                        mainFrm.SetClosing(true);
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
                    MessageBox.Show("没有连接PLC" + ex.Message);
                    mainFrm.DeviceDisConnection();
                    mainFrm.SetClosing(true);
                }
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
                        break;
                    }
                }
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

      

        #region 显示出库站台图片
        /// <summary>
        /// 刷新辊道图片
        /// </summary>
        public override void DisplayStatus()
        {
            for (int i = 0; i < lb.Length; i++)
            {
               
                    if (!isBindToPLC)
                        lb[i].BackColor = Color.DeepSkyBlue;
                    else
                    {
                        try
                        {
                            if (error[i] == 0)
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
                                  //  mainFrm.speech.speech("辊道编号" + this.conveyorName[i].ToString() + mainFrm.ConveyorError(deviceType[i], error[i]));
                                    if (error[i] != 15)
                                        DataBaseInterface.DeviceErrorMessage(conveyorName[i], levelNum[i], rowNum[i], deviceType[i], error[i], mainFrm.deviceStatusDic.getDesc(deviceType[i], error[i].ToString()), loadStruct[i].taskID);
                                }
                            }
                            lastError[i] = error[i];
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                }
        }
        #endregion

      

        #region 辊道任务状态更新
        public void AutoUpdate(object conveyorCmdId)
        {
            int nScannerId = int.Parse(conveyorCmdId.ToString());
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
                            if (this.loadStruct[nScannerId].loadType != 0 && mainFrm.bPicking)
                            {
                                DataRow dr = DataBaseInterface.SelectTaskD(conn,loadStruct[nScannerId].taskID.ToString(), "CON");
                                if (dr != null && int.Parse(dr["task_type"].ToString()) == loadStruct[nScannerId].taskType)
                                {
                                    int i = 0;
                                    switch (loadStruct[nScannerId].taskType)
                                    {
                                        case 1://入库
                                            i = DataBaseInterface.TaskStatusUpdate(loadStruct[nScannerId].taskID, nDeviceID[nScannerId], conveyorName[nScannerId], int.Parse(dr["step"].ToString()), 2, out rs);
                                            if (i != 1)
                                            {
                                                MessageBox.Show("堆垛机取货站台入库辊道任务报完成失败！" + rs);
                                            }
                                            break;
                                        case 2: break; //出库
                                        case 3://空托盘入库
                                            i = DataBaseInterface.TaskStatusUpdate(loadStruct[nScannerId].taskID, nDeviceID[nScannerId], conveyorName[nScannerId], int.Parse(dr["step"].ToString()), 2, out rs);
                                            if (i != 1)
                                            {
                                                MessageBox.Show("堆垛机取货站台空托入库辊道任务报完成失败！" + rs);
                                            }
                                            break;
                                        case 4: //退库
                                            fromadd[nScannerId] = int.Parse(dr["from_unit"].ToString());
                                            toadd[nScannerId] = int.Parse(dr["to_unit"].ToString());
                                            mTasktype[nScannerId] = int.Parse(dr["task_type"].ToString());
                                            dTasktype[nScannerId] = int.Parse(dr["task_type"].ToString());
                                            loadtype[nScannerId] = mainFrm.goodsKind[levelNum[nScannerId] - 1];
                                            DownLoadNewTask(nScannerId, loadStruct[nScannerId].taskID, dTasktype[nScannerId], mTasktype[nScannerId], loadtype[nScannerId], fromadd[nScannerId], toadd[nScannerId], dr["box_barcode"].ToString());
                                            break;
                                        case 5://托盘出库异常回库
                                            if (int.Parse(dr["step"].ToString()) == 2) //一楼吨桶异常回库
                                            {
                                                i = DataBaseInterface.TaskStatusUpdate(loadStruct[nScannerId].taskID, nDeviceID[nScannerId], conveyorName[nScannerId], int.Parse(dr["step"].ToString()), 2, out rs);
                                                if (i != 1)
                                                {
                                                    MessageBox.Show("堆垛机取货站台异常回库辊道任务报完成失败！" + rs);
                                                }
                                            }
                                            else if (int.Parse(dr["step"].ToString()) == 3)//二楼圆桶异常回库
                                            {
                                                if (int.Parse(dr["status"].ToString()) == 0)//若任务为新生成，则写任务到输送并修改执行中
                                                {
                                                    fromadd[nScannerId] = int.Parse(dr["from_unit"].ToString());
                                                    toadd[nScannerId] = int.Parse(dr["to_unit"].ToString());
                                                    mTasktype[nScannerId] = int.Parse(dr["task_type"].ToString());
                                                    dTasktype[nScannerId] = int.Parse(dr["task_type"].ToString());
                                                    loadtype[nScannerId] = mainFrm.goodsKind[levelNum[nScannerId] - 1];
                                                    DownLoadNewTask(nScannerId, loadStruct[nScannerId].taskID, dTasktype[nScannerId], mTasktype[nScannerId], loadtype[nScannerId], fromadd[nScannerId], toadd[nScannerId], dr["box_barcode"].ToString());
                                                }
                                                else
                                                {
                                                    i = DataBaseInterface.TaskStatusUpdate(loadStruct[nScannerId].taskID, nDeviceID[nScannerId], conveyorName[nScannerId], int.Parse(dr["step"].ToString()), 2, out rs);
                                                    if (i != 1)
                                                    {
                                                        MessageBox.Show("堆垛机取货站台异常回库辊道任务报完成失败！" + rs);
                                                    }
                                                }
                                            }

                                            break;
                                        default:
                                            break;
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Thread.Sleep(500);
                        MessageBox.Show(ex.Message);
                        continue;
                    }
                }
                   
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
            for (i = 0; i < 10; i++)
            {
                WriteCmd(nScannerID, nTaskid, dTaskType, nFromAdd, nToAdd, nloadType);
                Thread.Sleep(200);
                RefreshStatus();
                if (returnStruct[nScannerID].taskID == nTaskid)
                {
                    if (mTaskType == 4)
                    {
                        taskDesc = "一楼退库任务下发成功";
                        DataBaseInterface.TaskStatusUpdate(nTaskid, nDeviceID[nScannerID], conveyorName[nScannerID], 2, 1, out taskDesc);//修改任务状态为执行中
                    }
                    else if (mTaskType ==5)
                    {
                        taskDesc = "一楼异常回库任务生成成功！";
                        DataBaseInterface.TaskStatusUpdate(nTaskid, nDeviceID[nScannerID], conveyorName[nScannerID], 3, 1, out taskDesc);//修改任务状态为执行中
                    }
                    i = 0;
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
            }
            if (lastStrError[nScannerId] != strError)
            {
                MessageBox.Show(strError);
                if (nError == 2)
                    DataBaseInterface.DeviceErrorMessage(conveyorName[nScannerId], levelNum[nScannerId], 0, "CON", 0, "辊道下任务失败", loadStruct[nScannerId].taskID);
            }
            lastStrError[nScannerId] = strError;
        }
        #endregion

        #region 判断辊道货物类型
        public LoadStruct IsHaveGoods(int name, int level)
        {
            for (int i = 0; i < loadStruct.Length; i++)
            {
                if (nDeviceID[i] == name && level == levelNum[i])
                    return loadStruct[i];
            }
            return new LoadStruct { from = 0, to = 0, loadType = 10, taskID = 0, taskType = 0 };
        }
        #endregion

        #region 辊道手动写入任务号（load块）
        /// <summary>
        /// 辊道手动写入任务号
        /// </summary>
        /// <param name="nRow"></param> 列
        /// <param name="nLevel"></param> 层
        /// <param name="ID"></param> 任务号，int32
        /// <param name="FromLev"></param> 起始层 1-12 
        /// <param name="ToLev"></param> 目的层 1-12
        /// <param name="Reserve"></param> 预留位，默认0
        public virtual void WriteID(int index, int nTaskID, int nTaskType, int FromLev, int ToLev, int loadType)
        {
            int[] handle = new int[1];
            handle[0] = loadHandle[index];
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
