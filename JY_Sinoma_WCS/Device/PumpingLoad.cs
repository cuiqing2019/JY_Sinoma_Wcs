
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
using PLC;
using MySql.Data.MySqlClient;

namespace JY_Sinoma_WCS
{
    /// <summary>
    /// 等待区与抽液处类
    /// </summary>
    public class Pumping : OPCServer
    {
        #region 自定义变量
        /// <summary>
        /// 允许放桶DB
        /// </summary>
        public string[] permitDB;
        /// <summary>
        /// 允许放桶块句柄
        /// </summary>
        public int[] permitHandle;
        /// <summary>
        /// 取液完成DB
        /// </summary>
        public string[] completionDB;
        /// <summary>
        /// 取液完成块句柄
        /// </summary>
        public int[] completionHandle;
        /// <summary>
        /// 放货完成DB
        /// </summary>
        public string[] deliveryDB;
        /// <summary>
        /// 放货完成块句柄
        /// </summary>
        public int[] deliveryHandle;
        /// <summary>
        /// 取货完成DB
        /// </summary>
        public string[] acquisitionDB;
        /// <summary>
        /// 取货完成块句柄
        /// </summary>
        public int[] acquisitionHandle;
        /// <summary>
        /// 取液处状态结构体
        /// </summary>
        public struct LoadStruct
        {
            /// <summary>
            /// 允许放桶
            /// </summary>
            public int permit;
            /// <summary>
            /// 取液完成
            /// </summary>
            public int completion;
            /// <summary>
            /// 放货完成
            /// </summary>
            public int delivery;
            /// <summary>
            /// 取货完成
            /// </summary>
            public int acquisition;
        }
        /// <summary>
        /// 取液处状态
        /// </summary>
        public LoadStruct[] loadStruct;
        /// <summary>
        /// 取液处名称
        /// </summary>
        public string[] loadName;
        /// <summary>
        /// 是否已初始化OPCServer
        /// </summary>
        public bool isBindToPLC = false;
        /// <summary>
        /// 是否正在关闭
        /// </summary>
        public bool closing = false;
        /// <summary>
        /// 主界面
        /// </summary>
        public frmMain mainFrm;
        public ConnectPool dbConn;
        #endregion
        public Thread PumpingThread;
        /// <summary>
        ///抽液出可用状态
        /// </summary>
        public static bool PumpingStaus = false;
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainFrm"></param>
        /// <param name="bt"></param>
        public Pumping(frmMain mainFrm, DataTable bt)
        {
            this.mainFrm = mainFrm;
            this.dbConn = mainFrm.dbConn;
            int count = 2;//取液处数量
            permitDB = new string[count];
            completionDB = new string[count];
            deliveryDB = new string[count];
            acquisitionDB = new string[count];
            permitHandle = new int[count];
            completionHandle = new int[count];
            deliveryHandle = new int[count];
            acquisitionHandle = new int[count];
            loadName = new string[count];
            loadStruct = new LoadStruct[count];
            int waitCount = bt.Rows.Count - count;
            int i = 0;//抽液处编号
            foreach (DataRow dr in bt.Rows)
            {
                if (int.Parse(dr["DEVICE_TYPE"].ToString()) == 1)
                {
                    permitDB[i] = dr["PERMIT_DB"].ToString();
                    completionDB[i] = dr["COMPLETION_DB"].ToString();
                    deliveryDB[i] = dr["DELIVERY_DB"].ToString();
                    acquisitionDB[i] = dr["ACQUISITION_DB"].ToString();
                    loadName[i] = dr["DEVICE_ID"].ToString();
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
        public bool BindToPLC()
        {
            if (!SyncAddGroup()) return false;
            if (!AsyncAddGroup()) return false;
            int client = 1;

            OpcRcw.Da.OPCITEMDEF[] permitItems = new OPCITEMDEF[permitDB.Length];
            for (int i = 0; i < permitDB.Length; i++)
            {
                permitItems[i].szAccessPath = "";
                permitItems[i].bActive = 1;
                permitItems[i].hClient = client;
                permitItems[i].dwBlobSize = 1;
                permitItems[i].pBlob = IntPtr.Zero;
                permitItems[i].vtRequestedDataType = (int)VarEnum.VT_I2;
                permitItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}", permitDB[i]);
                client++;
            }
            if (!SyncAddItems(permitItems, permitHandle)) return false;

            OpcRcw.Da.OPCITEMDEF[] completionItems = new OPCITEMDEF[completionDB.Length];
            for (int i = 0; i < completionDB.Length; i++)
            {
                completionItems[i].szAccessPath = "";
                completionItems[i].bActive = 1;
                completionItems[i].hClient = client;
                completionItems[i].dwBlobSize = 1;
                completionItems[i].pBlob = IntPtr.Zero;
                completionItems[i].vtRequestedDataType = (int)VarEnum.VT_I2;
                completionItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}", completionDB[i]);
                client++;
            }
            if (!SyncAddItems(completionItems, completionHandle)) return false;

            OpcRcw.Da.OPCITEMDEF[] deliveryItems = new OPCITEMDEF[deliveryDB.Length];
            for (int i = 0; i < deliveryDB.Length; i++)
            {
                deliveryItems[i].szAccessPath = "";
                deliveryItems[i].bActive = 1;
                deliveryItems[i].hClient = client;
                deliveryItems[i].dwBlobSize = 1;
                deliveryItems[i].pBlob = IntPtr.Zero;
                deliveryItems[i].vtRequestedDataType = (int)VarEnum.VT_I2;
                deliveryItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}", deliveryDB[i]);
                client++;
            }
            if (!SyncAddItems(deliveryItems, deliveryHandle)) return false;

            OpcRcw.Da.OPCITEMDEF[] acquisitionItems = new OPCITEMDEF[acquisitionDB.Length];
            for (int i = 0; i < acquisitionDB.Length; i++)
            {
                acquisitionItems[i].szAccessPath = "";
                acquisitionItems[i].bActive = 1;
                acquisitionItems[i].hClient = client;
                acquisitionItems[i].dwBlobSize = 1;
                acquisitionItems[i].pBlob = IntPtr.Zero;
                acquisitionItems[i].vtRequestedDataType = (int)VarEnum.VT_I2;
                acquisitionItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}", acquisitionDB[i]);
                client++;
            }
            if (!SyncAddItems(acquisitionItems, acquisitionHandle)) return false;
            //开始接收订阅数据项的事件
            SetState(true);
            isBindToPLC = true;
            return isBindToPLC;
        }
        #endregion

        #region 刷新PLC状态
        /// <summary>
        /// 刷新PLC状态
        /// </summary>
        public void RefreshStatus()
        {
          
                if (!isBindToPLC)
                {
                    //if (!BindToPLC())
                    //{
                    //    mainFrm.DeviceDisConnection();
                    //    mainFrm.SetClosing(true);
                    //}
                }
                try
                {
                    #region 允许放桶
                    object[] readValues = new object[permitDB.Length];
                    int[] value = new int[5];
                    readValues = new object[permitDB.Length];
                    if (!SyncRead(readValues, permitHandle))
                    {
                        mainFrm.DeviceDisConnection();
                        mainFrm.SetClosing(true);
                        return;
                    }
                    for (int i = 0; i < permitDB.Length; i++)
                    {
                        try
                        {
                            loadStruct[i].permit = int.Parse(readValues[i].ToString());
                        if (loadStruct[0].permit==1 || loadStruct[1].permit == 1)
                        {
                            mainFrm. PumpingStaus = true;
                        }
                        else
                        {
                            mainFrm.PumpingStaus = false;
                        }
                       
                    
                      }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    #endregion
                    #region 取液完成
                    readValues = new object[completionDB.Length];
                    if (!SyncRead(readValues, completionHandle))
                    {
                        mainFrm.DeviceDisConnection();
                        mainFrm.SetClosing(true);
                        return;
                    }
                    for (int i = 0; i < completionDB.Length; i++)
                    {
                        try
                        {
                            loadStruct[i].completion = int.Parse(readValues[i].ToString());
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    #endregion
                    #region 放货完成
                    readValues = new object[deliveryDB.Length];
                    if (!SyncRead(readValues, deliveryHandle))
                    {
                        mainFrm.DeviceDisConnection();
                        mainFrm.SetClosing(true);
                        return;
                    }
                    for (int i = 0; i < deliveryDB.Length; i++)
                    {
                        try
                        {
                            loadStruct[i].delivery = int.Parse(readValues[i].ToString());
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    #endregion
                    #region 取货完成
                    readValues = new object[acquisitionDB.Length];
                    if (!SyncRead(readValues, acquisitionHandle))
                    {
                        mainFrm.DeviceDisConnection();
                        mainFrm.SetClosing(true);
                        return;
                    }
                    for (int i = 0; i < acquisitionDB.Length; i++)
                    {
                        try
                        {
                            loadStruct[i].acquisition = int.Parse(readValues[i].ToString());
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message);
                        }
                    }
                    #endregion
                    
                  
                }
                catch (Exception ex)
                {
                    MessageBox.Show("ConveyorLoad类，RefreshStatus方法：" + ex.Message);

                }
            
        }
        #endregion

        #region 断开PLC
        /// <summary>
        /// 断开PLC
        /// </summary>
        public void DisConnectPLC()
        {
            if (isBindToPLC)
            {
                //PumpingThread.Abort();
                DisConnect();
                isBindToPLC = false;
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

        #region 根据地址返回抽液处状态
        public LoadStruct? PointStatus(string unit)
        {
            RefreshStatus();
            for (int i = 0; i < 2; i++)
            {
                if (loadName[i] == unit)
                    return loadStruct[i];
            }
            return null;
        }

        #endregion

        #region 根据条件返回取液处编号
        /// <summary>
        /// 根据条件返回取液处编号
        /// </summary>
        /// <param name="status">1-允许放桶 2-取液完成 3-放货完成 4-取货完成</param>
        /// <returns>存在该状态的取液处返回该取液处编号，否则返回null</returns>
        public string PointStatus(MySqlConnection conn, int status,out string boxCode)
        {
            boxCode = null;
            RefreshStatus();
            for (int i = 0; i < loadName.Length; i++)
            { 
                switch (status)
                {
                    case 1: if (loadStruct[i].permit == 1)
                        {                       
                          return loadName[i];
                        }

                        break;
                    case 2: if (loadStruct[i].completion == 1) 
                    {
                        boxCode = DataBaseInterface.GetPumpingBoxcode(conn,loadName[i]);
                        return loadName[i];
                    }
                    break;
                    default: return null;
                }
            }
            return null;
        }
        #endregion

        #region 抽液处箱号
        /// <summary>
        /// 抽液处箱号
        /// </summary>
        /// <param name="boxCode">箱号</param>
        /// <param name="point">设备名称/地址</param>
        /// <returns>抽液处箱号</returns>
        public string PointBoxCodeSelect(MySqlConnection conn,string point)
        {
            RefreshStatus();
            for (int i = 0; i < 2; i++)
            {
                if (loadName[i] == point)
                    return DataBaseInterface.GetPumpingBoxcode(conn,loadName[i]);
            }
            return null;
        }
        #endregion
      
        #region 抽液处箱号更新
        /// <summary>
        /// 抽液处箱号更新
        /// </summary>
        /// <param name="boxCode">箱号</param>
        /// <param name="point">设备名称/地址</param>
        /// <returns>是否更新成功</returns>
        public bool PointBoxCodeUpdate(string point, string boxCode)
        {
            try
            {
                RefreshStatus();
                for (int i = 0; i < 2; i++)
                {
                    if (loadName[i] == point)
                    {
                        if (DataBaseInterface.UpdatePumpingBoxcode(point, boxCode) != 1)
                        {
                            MessageBox.Show("更新抽液处箱号失败！");
                            return false;
                        }
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show("抽液处箱号更新失败：" + ex.Message);
                return false;
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
        public void WriteSingleAction(int nIndex, int nAction,int taskStep)
        {
            int[] handle = new int[1];
            if (taskStep == 6)
                handle[0] = acquisitionHandle[nIndex];
            else if (taskStep == 12)
                handle[0] = deliveryHandle[nIndex];
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
