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
using PLC;

namespace JY_Sinoma_WCS
{
    public class Conveyor : OPCServer
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
        /// 是否放行
        /// </summary>
        public bool running = true;
        /// <summary>
        /// 辊道图标
        /// </summary>
        public Label[] lb;
        /// <summary>
        /// 是否已初始化OPCServer
        /// </summary>
        public bool isBindToPLC = false; 
        /// <summary>
        /// 辊道名称
        /// </summary>
        public string[] conveyorName;
        /// <summary>
        /// 未知
        /// </summary>
        public int nDownLoadTask = 0;
        /// <summary>
        /// 入库扫码巷道
        /// </summary>
        public int[] channelNum;
        /// <summary>
        /// 入库扫码层
        /// </summary>
        public int[] levelNum;
        /// <summary>
        /// 入库扫码排
        /// </summary>
        public int[] rowNum;
        /// <summary>
        /// 入库扫码器编号
        /// </summary>
        public string[] scanner_id;
        /// <summary>
        /// 辊道对应出入库任务地址
        /// </summary>
        public int taskAddress = 0;
        /// <summary>
        /// load块数据结构体
        /// </summary>
        public struct LoadStruct
        {
            public int taskID;            //任务号
            public int taskType;          //任务类型
            public int from;              //起始地址
            public int to;                //目的地址
            public int loadType;              //货物类型 0-无信息 1-吨桶 2-圆桶 3-整摞空托盘 4-单个空托盘
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
            public int loadType;            //货物类型 0-无信息 1-吨桶 2-圆桶 3-整摞空托盘 4-单个空托盘
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
        /// <summary>
        /// 设备编号
        /// </summary>
        public int[] nDeviceID;
        public int taskid;
        public string[] deviceType;
        #endregion
 
        #region 构造函数
        public Conveyor(frmMain mainFrm, DataTable bt)
        {
            this.mainFrm = mainFrm;
            dbConn = mainFrm.dbConn;
            systemstatus = mainFrm.systemStatus;
            nCount = bt.Rows.Count;
            error = new int[bt.Rows.Count];
            lastError = new int[bt.Rows.Count];
            errorDB = new String[bt.Rows.Count];
            errorHandle = new int[bt.Rows.Count];
            errorClientHandle = new int[bt.Rows.Count];
            controlDB = new String[bt.Rows.Count];
            controlHandle = new int[bt.Rows.Count];
            conveyorName = new string[bt.Rows.Count];
            channelNum = new int[nCount];
            levelNum = new int[nCount];
            rowNum = new int[nCount];
            scanner_id=new string [nCount];
            lb = new Label[bt.Rows.Count];
            deviceType = new string[bt.Rows.Count];
            nDeviceID = new int[bt.Rows.Count];
        }
        #endregion

        #region 连接PLC
        /// <summary>
        /// 连接PLC
        /// </summary>
        /// <returns></returns>
        public virtual bool BindToPLC()
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

        #region 刷新辊道状态
        /// <summary>
        /// 刷新辊道状态
        /// </summary>
        public virtual void RefreshStatus()
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
                        int[] value = new int[5];
                        object[] readValues = new object[loadDB.Length];
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
                            loadStruct[i].taskType =int.Parse(value[1].ToString());
                            loadStruct[i].from =int.Parse(value[2].ToString());
                            loadStruct[i].to =  int.Parse(value[3].ToString());
                            loadStruct[i].loadType =  int.Parse(value[4].ToString());
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Conveyor类：RefreshStatus方法" + ex.Message);
                    }
                }
                Thread.Sleep(200);
            
        }
        #endregion 

        #region 图片双击事件
        public virtual void lb_DoubleClick(object sender, EventArgs e){ }
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

        

        #region 辊道下任务
        /// <summary>
        /// 辊道下任务
        /// </summary>
        public virtual void WriteCmd(int index, int nTaskID, int nTaskType,int FromLev, int ToLev, int loadType)
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
            writeValues[0] += (ToLev ).ToString() + "|";
            writeValues[0] += (loadType).ToString();
            writeValues[0] += "}";
            if (!SyncWrite(writeValues, handle)) 
            {
                mainFrm.DeviceDisConnection();
                mainFrm.SetClosing(true);
            }
        }
        #endregion

        #region 显示出库站台图片
        /// <summary>
        /// 刷新辊道图片
        /// </summary>
        public virtual void DisplayStatus()
        {
            try
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
                                lb[i].BackColor = Color.Green;
                            else
                                lb[i].BackColor = Color.LightGreen;
                        }
                        else
                        {
                            lb[i].BackColor = Color.Red;
                            if (lastError[i] != error[i])
                            {
                                MessageBox.Show(taskid.ToString());
                               // mainFrm.speech.speech("辊道编号" + this.conveyorName[i].ToString() + mainFrm.ConveyorError(deviceType[i],error[i]));
                                if (error[i] != 15)
                                    DataBaseInterface.DeviceErrorMessage(conveyorName[i], levelNum[i], rowNum[i], deviceType[i], error[i], mainFrm.deviceStatusDic.getDesc(deviceType[i],error[i].ToString()), 0);
                            }
                        }
                        lastError[i] = error[i];
                    }
                }
            }
            catch (Exception )
            { }
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
    
        #region 辊道写入控制命令
        /// <summary>
        /// 辊道写入控制命令
        /// </summary>
        /// <param name="nCol"></param> 列
        /// <param name="nLevel"></param> 层
        /// <param name="nAction"></param> 控制代码  1 清错；2 初始化
        public virtual void WriteSingleAction(int nIndex, int nAction)
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


    }
}
