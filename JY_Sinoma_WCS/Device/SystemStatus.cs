
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Runtime.InteropServices;
using OpcRcw.Da;
using PLC;
using MySql.Data.MySqlClient;
using DataBase;

namespace JY_Sinoma_WCS
{
    public class SystemStatus : OPCServer
    {
        #region 自定义变量
        public frmMain mainFrm;
        public ConnectPool dbConn;
        public static int nSubNum = 3;//从站数量
        public string[] strDeviceName;
        public int[] nDeviceID;
        private Label[] lb = new Label[3];
        public string[] statusDB;//状态DB
        private int[] statusHandle;//系统状态的OPCServer的Handle
        private int[] statusClientHandle;
        public string[] workModeDB;//手自动控制DB
        private int[] workModeHandle;

        public string[] faultClearDB;//故障总清
        private int[] faultClearHandle;

        public string[] dataClearDB;//总初始化
        private int[] dataClearHandle;

        public string[] taskModeDB;//任务模式DB
        private int[] taskModeHandle;

        public string[] heartDB;//心跳
        private int[] heartHandle;

        private bool isBindToPLC = false;  //是否已初始化OPCServer

        public string[] returnDB = new string[1];//对应的Return块地址
        public int[] returnHandle = new int[1];//辊道控制的OPCServer的Handle

        public string[] writeDB = new string[3];//对应辊道的Write块地址
        public int[] writeHandle = new int[3];//辊道控制的OPCServer的Handle
        private int[] internetClientHandle = new int[1];
        public  string[] diepanCount;
        private int[] diepanCountHandle;//系统状态的OPCServer的Handle
        /// <summary>
        /// 是否正在关闭
        /// </summary>
        public bool closing = false;
        public static int[] counts;
        /// <summary>
        /// 线程
        /// </summary>
        public Thread SystemThread;
        Thread DCSTread;
        public struct StatusStruct//load块读取数据
        {
            public int systemRun;      //系统运行
            public int systemError;        //有故障
            public int warning;        //启动预警
            public int auto;           //自动方式
            public int gasWarning;     //气源故障
            public int StopCarbinet;   //急停（机柜）
            public int StopSpot;       //急停（现场）
        }
        public struct InternetStruct//load块读取数据
        {
            public int errorDP;       //故障数量
            public int mainDP;        //主站
            public int subDP;         //总线存在故障
            public int subDP1;        //子站1
            public int subDP2;        //子站2
            public int subDP3;        //子站3
            public int subDP4;        //子站4
        }
        public StatusStruct[] statusStruct = new StatusStruct[2];
        public InternetStruct internetStruct;
        public int nCount;
        public int nHeartNum = 0;//心跳计数
        #endregion

        public SystemStatus(frmMain mainFrm, DataTable bt)
        {
            this.mainFrm = mainFrm;
            this.dbConn = mainFrm.dbConn;
            this.nCount = bt.Rows.Count;
            this.nDeviceID = new int[nCount];
            this.strDeviceName = new string[nCount];
            this.statusDB = new string[nCount-5];
            this.statusHandle = new int[nCount-5];
            this.statusClientHandle = new int[nCount-5];
            this.workModeDB = new string[nCount - 6];
            this.workModeHandle = new int[nCount - 6];
            this.faultClearDB = new string[nCount - 6];
            this.faultClearHandle = new int[nCount - 6];
            this.dataClearDB = new string[nCount - 6];
            this.dataClearHandle = new int[nCount -6];
            this.taskModeDB = new string[nCount - 6];
            this.taskModeHandle = new int[nCount - 6];
            this.heartDB = new string[nCount - 6];
            this.heartHandle = new int[nCount - 6];
            this.diepanCount = new string[nCount - 7];
            this.diepanCountHandle = new int[nCount - 7];
            int i = 0;
            foreach (DataRow row in bt.Rows)
            {
                if(row["device_type"].ToString() == "DCS_WRITE")
                {
                    writeDB[i - 3] = row["status_db"].ToString();
                }
                else if(row["device_type"].ToString() == "DCS_RETURN")
                {
                    returnDB[i-6]= row["status_db"].ToString();
                }
                else if (row["device_type"].ToString() == "Null_Count")
                {
                    diepanCount[i-7] = row["count_db"].ToString();
                }
                else
                {
                    statusDB[i] = row["status_db"].ToString();
                    strDeviceName[i] = row["device_name"].ToString();
                    nDeviceID[i] = int.Parse(row["device_id"].ToString());
                    if (row["device_type"].ToString() == "STATUS")
                    {
                        workModeDB[i] = row["workmode_db"].ToString();
                        faultClearDB[i] = row["faultclear_db"].ToString();
                        dataClearDB[i] = row["dataclear_db"].ToString();
                        taskModeDB[i] = row["taskmode_db"].ToString();
                        heartDB[i] = row["heart_db"].ToString();
                    }
                  
                    lb[i] = new Label();
                    lb[i].Size = new Size(int.Parse(row["width"].ToString()), int.Parse(row["length"].ToString()));
                    lb[i].Location = new Point(int.Parse(row["x"].ToString()), int.Parse(row["y"].ToString()));
                    lb[i].BackColor = Color.Gray;
                    lb[i].Font = new Font("宋体", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                    lb[i].Tag = row["device_id"].ToString();
                    lb[i].Text = "";
                    lb[i].TextAlign = ContentAlignment.MiddleCenter;
                    lb[i].DoubleClick += new System.EventHandler(this.lb_DoubleClick);
                    mainFrm.gpSystemStatus.Controls.Add(lb[i]);
                    lb[i].BringToFront();
                }
              
                i++;
            }
        }

        #region DCS系统状态
        public void DcsStatus()
        {
            while(!closing)
            {
                try
                {
                    if(mainFrm.deviceLinkIsOK)
                    {
                        if(!isBindToPLC)
                        {
                            if(!BindToPLC())
                            {
                                mainFrm.DeviceDisConnection();
                                mainFrm.SetClosing(true);
                            }
                        }
                        using (MySqlConnection conn = dbConn.GetConnectFromPool())
                        {
                            #region 读取DCS启停消息
                            int[] value = new int[1];
                            object[] readValues = new object[returnDB.Length];
                            try
                            {
                                if (!SyncRead(readValues, returnHandle))
                                {
                                    MessageBox.Show("readValues报错");
                                    return;
                                }
                                for (int i = 0; i < returnDB.Length; i++)
                                {
                                    try
                                    {
                                        //if (int.Parse(readValues[i].ToString()) == 2)
                                        //{
                                        //    mainFrm.tsbStop_Click(new object(), new EventArgs());
                                        //    DataBase.DataBaseInterface.AGVstop(conn,"1");
                                        //    mainFrm.Text = "济源中材立库监控系统:DCS紧急停止";
                                        //}
                                        //else 
                                        //if (int.Parse(readValues[i].ToString()) == 0)
                                        //{
                                        //    DataBase.DataBaseInterface.AGVstop(conn,"0");
                                        //    mainFrm.Text = "济源中材立库监控系统:DCS正常";
                                        //}
                                        //else
                                        //{
                                        //    mainFrm.tsbStop_Click(new object(), new EventArgs());
                                        //    mainFrm.Text = "济源中材立库监控系统:DCS停止工作";
                                        //}
                                    }
                                    catch (Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                return;
                            }
                            #endregion

                            #region 写入DCS系统状态信息
                            //写入小车报警
                            DataTable item = DataBase.DataBaseInterface.AGVstatus();
                            if (item != null)
                            {
                                if (int.Parse(item.Rows[0]["STATUS_ID"].ToString()) == 0 && int.Parse(item.Rows[1]["STATUS_ID"].ToString()) == 0)
                                    WriteSingleAction(0, 1);//自动
                                if (int.Parse(item.Rows[0]["STATUS_ID"].ToString()) == 0 && int.Parse(item.Rows[1]["STATUS_ID"].ToString()) == 0)
                                    WriteSingleAction(0, 2);//自动
                                else
                                    WriteSingleAction(0, 3);//异常
                            }
                            //写入小车任务状态
                            DataRow row = DataBase.DataBaseInterface.GetAgvTaskStatus(conn);
                            if (row == null || row["taskstatus"].ToString() == "0")
                                WriteSingleAction(2, 0);
                            else
                                WriteSingleAction(2, 1);
                            //写入系统工作状态
                            if (mainFrm.bPicking)
                                WriteSingleAction(1, 1);
                            else
                                WriteSingleAction(1, 0);
                            #endregion
                        }

                    }
                            

                }
                catch(Exception ex)
                {
                    MessageBox.Show("DCS状态更新出错："+ex.Message);
                }
            }
        }
        #endregion
        #region 标签的双击事件
        public void lb_DoubleClick(object sender, EventArgs e)
        {
            int i = Convert.ToInt32(((Label)sender).Tag);
            if (i<3)//系统状态
            {
                FormSystemStatus frmControl = new FormSystemStatus(this, i);
                frmControl.ShowDialog();
            }
            else
            {
                FormInternet frmInternet = new FormInternet(this);
                frmInternet.ShowDialog();
            }
        }
        #endregion


        /// <summary>
        /// 设置工作模式，1：自动，0：手动
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void WriteWorkModelCmd(int nIndex, int value)
        {
            int[] handle = new int[1];
            handle[0] = this.workModeHandle[nIndex];
            string[] writeValues = new string[1];
            writeValues[0] = value.ToString();
            AsyncWrite(writeValues, handle);
        }
        /// <summary>
        /// 故障总清
        /// </summary>
        /// <param name="index"></param>
        public void WriteFaultClearCmd(int nIndex)
        {
            int[] handle = new int[1];
            handle[0] = this.faultClearHandle[nIndex];
            string[] writeValues = new string[1];
            writeValues[0] = "1";
            AsyncWrite(writeValues, handle);
        }

        /// <summary>
        /// 总初始化
        /// </summary>
        /// <param name="index"></param>
        public void WriteDataClearCmd(int nIndex)
        {
            int[] handle = new int[1];
            handle[0] = this.dataClearHandle[nIndex];
            string[] writeValues = new string[1];
            writeValues[0] = "1";
            AsyncWrite(writeValues, handle);
        }
        /// <summary>
        /// 设置任务模式，1 入库 2 出库 3 空托盘入库 4 退库 5 托盘出库异常回库
        /// </summary>
        /// <param name="index"></param>
        /// <param name="value"></param>
        public void WriteTaskModelCmd(int nIndex, int value)
        {
            int[] handle = new int[1];
            handle[0] = this.taskModeHandle[nIndex];
            string[] writeValues = new string[1];
            writeValues[0] = value.ToString();
            AsyncWrite(writeValues, handle);
  
        }


        /// <summary>
        /// 写心跳
        /// </summary>
        /// <param name="index"></param>
        public void WriteHeartCmd(int nIndex)
        {
            int[] handle = new int[1];
            handle[0] = this.heartHandle[nIndex];
            string[] writeValues = new string[1];
            if (nHeartNum < 2000)
                nHeartNum = nHeartNum + 1;
            else
                nHeartNum = 0;
            writeValues[0] = nHeartNum.ToString();
            AsyncWrite(writeValues, handle);
        }

        #region 辊道写入DCS控制命令
      /// <summary>
      /// 辊道写入DCS控制命令
      /// </summary>
      /// <param name="nIndex"></param>
      /// <param name="nAction"></param>
        public void WriteSingleAction(int nIndex, int nAction)
        {
            int[] handle = new int[1];
            handle[0] = writeHandle[nIndex];
            string[] writeValues = new string[1];
            writeValues[0] = nAction.ToString();
            if (!SyncWrite(writeValues, handle))
            {
                mainFrm.DeviceDisConnection();
                mainFrm.SetClosing(true);
            }
        }
        #endregion


        public bool BindToPLC()
        {
            if (!AsyncAddGroup()) return false;
            int client = 1;
            OpcRcw.Da.OPCITEMDEF[] diepanItems = new OPCITEMDEF[this.diepanCount.Length];
            for (int i = 0; i < this.diepanCount.Length; i++)
            {
                diepanItems[i].szAccessPath = "";
                diepanItems[i].bActive = 1;
                diepanItems[i].hClient = client;
                diepanItems[i].dwBlobSize = 1;
                diepanItems[i].pBlob = IntPtr.Zero;
                diepanItems[i].vtRequestedDataType = (int)VarEnum.VT_I2;
                diepanItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}", diepanCount[i]); //计数DB
                client++;
            }
        
            if (!AsyncAddItems(diepanItems, this.diepanCountHandle)) return false;

            OpcRcw.Da.OPCITEMDEF[] controlItems = new OPCITEMDEF[this.statusDB.Length];
            for (int i = 0; i < statusDB.Length; i++)
            {
                controlItems[i].szAccessPath = "";
                controlItems[i].bActive = 1;
                controlItems[i].hClient = client;
                controlItems[i].dwBlobSize = 1;
                controlItems[i].pBlob = IntPtr.Zero;
                controlItems[i].vtRequestedDataType = (int)VarEnum.VT_BSTR;
                controlItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}", statusDB[i]); //状态DB
                statusClientHandle[i] = client;
                client++;
            }
            if (!AsyncAddItems(controlItems, this.statusHandle)) return false;

            OpcRcw.Da.OPCITEMDEF[] autoItems = new OPCITEMDEF[this.workModeDB.Length];
            for (int i = 0; i < this.workModeDB.Length; i++)
            {
                autoItems[i].szAccessPath = "";
                autoItems[i].bActive = 1;
                autoItems[i].hClient = client;
                autoItems[i].dwBlobSize = 1;
                autoItems[i].pBlob = IntPtr.Zero;
                autoItems[i].vtRequestedDataType = (int)VarEnum.VT_I2;
                autoItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}", workModeDB[i]); //工作模式DB
                client++;
            }
            if (!AsyncAddItems(autoItems, this.workModeHandle)) return false;


            OpcRcw.Da.OPCITEMDEF[] faultClearItems = new OPCITEMDEF[this.faultClearDB.Length];
            for (int i = 0; i < this.faultClearDB.Length; i++)
            {
                faultClearItems[i].szAccessPath = "";
                faultClearItems[i].bActive = 1;
                faultClearItems[i].hClient = client;
                faultClearItems[i].dwBlobSize = 1;
                faultClearItems[i].pBlob = IntPtr.Zero;
                faultClearItems[i].vtRequestedDataType = (int)VarEnum.VT_I2;
                faultClearItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}", faultClearDB[i]); //故障总清DB
                client++;
            }
            if (!AsyncAddItems(faultClearItems, this.faultClearHandle)) return false;

            OpcRcw.Da.OPCITEMDEF[] dataClearItems = new OPCITEMDEF[this.dataClearDB.Length];
            for (int i = 0; i < this.dataClearDB.Length; i++)
            {
                dataClearItems[i].szAccessPath = "";
                dataClearItems[i].bActive = 1;
                dataClearItems[i].hClient = client;
                dataClearItems[i].dwBlobSize = 1;
                dataClearItems[i].pBlob = IntPtr.Zero;
                dataClearItems[i].vtRequestedDataType = (int)VarEnum.VT_I2;
                dataClearItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}", dataClearDB[i]); //总初始化DB
                client++;
            }
            if (!AsyncAddItems(dataClearItems, this.dataClearHandle)) return false;

            OpcRcw.Da.OPCITEMDEF[] taskModeItems = new OPCITEMDEF[this.taskModeDB.Length];
            for (int i = 0; i < this.taskModeDB.Length; i++)
            {
                taskModeItems[i].szAccessPath = "";
                taskModeItems[i].bActive = 1;
                taskModeItems[i].hClient = client;
                taskModeItems[i].dwBlobSize = 1;
                taskModeItems[i].pBlob = IntPtr.Zero;
                taskModeItems[i].vtRequestedDataType = (int)VarEnum.VT_I2;
                taskModeItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}", taskModeDB[i]); //总初始化DB
                client++;
            }
            if (!AsyncAddItems(taskModeItems, this.taskModeHandle)) return false;

            OpcRcw.Da.OPCITEMDEF[] heartItems = new OPCITEMDEF[this.heartDB.Length];
            for (int i = 0; i < this.heartDB.Length; i++)
            {
                heartItems[i].szAccessPath = "";
                heartItems[i].bActive = 1;
                heartItems[i].hClient = client;
                heartItems[i].dwBlobSize = 1;
                heartItems[i].pBlob = IntPtr.Zero;
                heartItems[i].vtRequestedDataType = (int)VarEnum.VT_I2;
                heartItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}", heartDB[i]); //总初始化DB
                client++;
            }
            if (!AsyncAddItems(heartItems, this.heartHandle)) return false;
            OpcRcw.Da.OPCITEMDEF[] returnItems = new OPCITEMDEF[returnDB.Length];
            for (int i = 0; i < returnDB.Length; i++)
            {
                returnItems[i].szAccessPath = "";
                returnItems[i].bActive = 1;
                returnItems[i].hClient = client;
                returnItems[i].dwBlobSize = 1;
                returnItems[i].pBlob = IntPtr.Zero;
                returnItems[i].vtRequestedDataType = (int)VarEnum.VT_I2;
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
                writeItems[i].vtRequestedDataType = (int)VarEnum.VT_I2;
                writeItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}", writeDB[i]);
                client++;
            }
            if (!SyncAddItems(writeItems, writeHandle)) return false;
            SetState(true);
            isBindToPLC = true;
            DCSTread = new Thread(DcsStatus);
            DCSTread.IsBackground = true;
            // 判断该线程是否被垃圾回收
            if (!DCSTread.IsAlive)
            {
                DCSTread.Start();
            }
            return true;
        }

        public void RefreshStatus()
        {

            if (!isBindToPLC)
            {
                if (!BindToPLC())
                    mainFrm.DeviceDisConnection();
            }
            else
            {
                WriteHeartCmd(0);//写心跳
            }
        }

        /// <summary>
        /// 非命令设备订阅返回的信息
        /// </summary>
        public override void OnDataChange(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
        {
            for (int i = 0; i < phClientItems.Length; i++)
            {
                bool bFind = false;
                int value1;
                for (int j = 0; j < this.nCount; j++)
                {
                    if (j < 3)
                    {
                        if (phClientItems[i] == statusClientHandle[j])
                        {
                            switch (j)
                            {
                                case 0:
                                    value1 = int.Parse(pvValues[i].ToString()) / 256;
                                    statusStruct[j].systemRun = (value1 & 0x01) / 0x01;
                                    statusStruct[j].systemError = (value1 & 0x02) / 0x02;
                                    statusStruct[j].warning = (value1 & 0x04) / 0x04;
                                    statusStruct[j].auto = (value1 & 0x08) / 0x08;
                                    statusStruct[j].gasWarning = (value1 & 0x10) / 0x10;
                                    statusStruct[j].StopCarbinet = (value1 & 0x020) / 0x20;
                                    statusStruct[j].StopSpot = (value1 & 0x40) / 0x40;
                                    break;
                                case 1:
                                    value1 = int.Parse(pvValues[i].ToString()) / 256;
                                    statusStruct[j].systemRun = (value1 & 0x01) / 0x01;
                                    statusStruct[j].systemError = (value1 & 0x02) / 0x02;
                                    statusStruct[j].warning = (value1 & 0x04) / 0x04;
                                    statusStruct[j].auto = (value1 & 0x08) / 0x08;
                                    statusStruct[j].gasWarning = (value1 & 0x10) / 0x10;
                                    statusStruct[j].StopCarbinet = (value1 & 0x020) / 0x20;
                                    statusStruct[j].StopSpot = (value1 & 0x40) / 0x40;
                                    break;
                                case 2:
                                    int[] value = new int[nSubNum];
                                    GetValue(pvValues[i].ToString(), value);
                                    internetStruct.errorDP = value[0];
                                    internetStruct.mainDP = value[1];
                                    internetStruct.subDP = (value[2] & 0x01) / 0x01;
                                    internetStruct.subDP1 = (value[2] & 0x02) / 0x02;
                                    internetStruct.subDP2 = (value[2] & 0x04) / 0x04;
                                    internetStruct.subDP3 = (value[2] & 0x08) / 0x08;
                                    internetStruct.subDP4 = (value[2] & 0x10) / 0x10;
                                    break;
                            }
                            bFind = true;
                            break;
                        }
                    }

                    if (bFind)
                        break;
                }
            }

        }

        public string  GetAuto(int level)
        {
            switch (statusStruct[level - 1].auto)
            {
                case 0:
                    return "手动";
                case 1:
                    return "自动";
                default:
                    return "未知";
            }
        }

        private void GetValue(string str, int[] value)
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

        public void DisConnectPLC()
        {
            if (isBindToPLC)
            {
                  //  SystemThread.Abort();
                    DisConnect();
                    isBindToPLC = false;
            }
        }

        #region 显示入库站台图片
        public void DisplayStatus()
        {
            //for (int i = 0; i < lb.Length; i++)
            //{
            //    if (!isBindToPLC)
            //        lb[i].BackColor = Color.DeepSkyBlue;
            //    else
            //    {
            //        int nError = (i == 0 || i == 1) ? ( statusStruct[i].StopCarbinet + statusStruct[i].StopSpot) : internetStruct.errorDP;
            //        if (nError >=2)
            //            lb[i].BackColor = Color.Red;
            //        else
            //        {
            //            if (i < 2)
            //            {
            //                if (statusStruct[i].auto == 1)
            //                    lb[i].BackColor = Color.Green;
            //                else
            //                    lb[i].BackColor = Color.LightGreen;
            //            }
            //            else
            //                lb[i].BackColor = Color.Green;
            //        }
            //    }
            //}
            for (int i = 0; i < lb.Length; i++)
            {
                if (!isBindToPLC)
                    lb[i].BackColor = Color.DeepSkyBlue;
                else
                {
                    int nError = (i == 0 || i == 1) ? statusStruct[i].systemError : internetStruct.errorDP;

                    if (nError >= 1)
                        lb[i].BackColor = Color.Red;
                    else
                    {
                        if (i < 2)
                        {
                            if (statusStruct[i].auto == 1)
                                lb[i].BackColor = Color.Green;
                            else
                                lb[i].BackColor = Color.LightGreen;
                        }
                        else
                            lb[i].BackColor = Color.Green;

                    }
                }
            }
        }
        #endregion
    }
}
