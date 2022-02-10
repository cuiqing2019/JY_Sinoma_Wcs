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

namespace JY_Sinoma_WCS
{
    /// <summary>
    /// 堆垛机放货站台位
    /// </summary>
    public class ElectricsConveyorCmd:Conveyor
    {
        #region 自定义变量
        public int[] systemStatusID;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainFrm"></param>
        /// <param name="bt"></param>
        public ElectricsConveyorCmd(frmMain mainFrm, DataTable bt)
            : base(mainFrm, bt)
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
            nDeviceID = new int[nCount];
            systemStatusID = new int[nCount];
            int i= 0;
            foreach (DataRow row in bt.Rows)
            {
                errorDB[i] = row["error_db"].ToString();
                controlDB[i] = row["control_db"].ToString();
                loadDB[i] = row["load_db"].ToString();
                returnDB[i] = row["return_db"].ToString();
                writeDB[i] = row["write_db"].ToString();
                conveyorName[i] = row["device_tag"].ToString();
                channelNum[i] = int.Parse(row["channel_no"].ToString());
                levelNum[i] = int.Parse(row["level_no"].ToString());
                rowNum[i] = int.Parse(row["row_no"].ToString());
                nDeviceID[i] = int.Parse(row["device_name"].ToString());
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
                if (levelNum[i] == 1)
                    mainFrm.PLevel1.Controls.Add(lb[i]);
                else
                    mainFrm.pLevel2.Controls.Add(lb[i]);
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

           OpcRcw.Da.OPCITEMDEF[] reurnItems = new OPCITEMDEF[returnDB.Length];
           for (int i = 0; i < returnDB.Length; i++)
           {
               reurnItems[i].szAccessPath = "";
               reurnItems[i].bActive = 1;
               reurnItems[i].hClient = client;
               reurnItems[i].dwBlobSize = 1;
               reurnItems[i].pBlob = IntPtr.Zero;
               reurnItems[i].vtRequestedDataType = (int)VarEnum.VT_BSTR;
               reurnItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}", returnDB[i]);
               client++;
           }
           if (!SyncAddItems(reurnItems, returnHandle)) return false;

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

        #region 标签的双击事件
        public override void lb_DoubleClick(object sender, EventArgs e)
        {
            string strTag = Convert.ToString(((Label)sender).Tag);
            int Index = int.Parse(strTag);
            //mainFrm.ChangetbScanInfoIndex((rowNum[Index] - 1) * 2 + levelNum[Index]-1);
            FormElectricsConveyorCmd frmConveyor = new FormElectricsConveyorCmd(this, Index, mainFrm.systemStatus);
            frmConveyor.StartPosition = FormStartPosition.CenterParent;
            frmConveyor.ShowDialog();
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
                            returnStruct[i].taskID =  int.Parse(value[0].ToString());
                            returnStruct[i].taskType = int.Parse(value[1].ToString());
                            returnStruct[i].from = int.Parse(value[2].ToString());
                            returnStruct[i].to =int.Parse(value[3].ToString());
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
                            loadStruct[i].taskID =  int.Parse(value[0].ToString());
                            loadStruct[i].taskType =  int.Parse(value[1].ToString());
                            loadStruct[i].from = int.Parse(value[2].ToString());
                            loadStruct[i].to =  int.Parse(value[3].ToString());
                            loadStruct[i].loadType =  int.Parse(value[4].ToString());
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

        #region 获取辊道任务号
        /// <summary>
        /// 获取任务号
        /// </summary>
        public int GetTask(int i)
        {
            return returnStruct[i].taskID;
        }
        #endregion

        #region 辊道下任务
        /// <summary>
        /// 辊道下任务
        /// </summary>
        public override void WriteCmd(int nChannel, int nTaskID, int nTaskType, int FromLev, int ToLev, int loadType)
        {
            if (GetTask(nChannel-1) == nTaskID)//返回的任务号和下载的任务号相同 则停止写任务
                return;
            int[] handle = new int[1];
            handle[0] = writeHandle[nChannel - 1];
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
            //string start_status = (nCol + 1 + (mainFrm.GetSystemSeq() - 1) * 6).ToString() + "排" + (nLevel + 1).ToString() + "层深度3辊道当前任务号：" + GetTask(nCol, nLevel).ToString();
            //string new_task = "  新写入任务号：" + nTaskID.ToString();
            //mainFrm.logHandling.Ele_con_Event(start_status, new_task);
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

        #region 是否可放
        public int canGetUp(int channeNo)
        {
            for (int i = 0; i < channelNum.Length; i++)
            {
                if (channelNum[i] == channeNo)
                {
                    if (loadStruct[i].loadType == 0)
                    {
                        return 1;
                    }
                }
            }
            return 0;
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
                    if (error[i] == 0 || error[i] == 15)
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
    }
}
