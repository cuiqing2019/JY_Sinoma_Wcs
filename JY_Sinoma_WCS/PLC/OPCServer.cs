using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.ComponentModel;
using System.Windows.Forms;
using System.Data;
using OpcRcw.Da;
using OpcRcw.Comn;

namespace PLC
{
    /// <summary>
    /// OPCServer类
    /// </summary>
    public class OPCServer : IOPCDataCallback
    {
        #region 通用变量
        /// <summary>
        /// OPCServer连接对象
        /// </summary>
        private OPCServerConnection opcConn;
        /// <summary>
        /// OPCServer对象
        /// </summary>
        private IOPCServer serverObj;
        /// <summary>
        /// 是本类的连接true 还是传入的连接false
        /// </summary>
        private bool isNative = false;
        /// <summary>
        /// OPCServer返回文本的语言，0x407为英语实测返回的德语，0x804为中文，实测返回的是英语
        /// </summary>
        private const int LOCALE_ID = 0x804;
        /// <summary>
        /// the fastest rate at which data changes may be sent to OnDataChange for items in this group
        /// </summary>
        private Int32 dwRequestedUpdateRate = 500;
        /// <summary>
        /// 死区百分比，在此不设死区
        /// </summary>
        private float deadband = 0;
        /// <summary>
        /// 时差用于调整时区，在此不对时区进行调整
        /// </summary>
        private int timeBias = 0;
        /// <summary>
        /// 设备代码
        /// </summary>
        public int deviceId;
        /// <summary>
        /// 设备名称
        /// </summary>
        public string deviceName;
        #endregion

        #region 同步通讯变量
        /// <summary>
        /// 同步组对象，创建组对象时产生，添加Item时用
        /// </summary>
        private Object syncGroupObj = null;
        /// <summary>
        /// 同步组句柄，创建组对象时产生，释放内存时用
        /// </summary>
        private int syncGroupHandle = 0;
        /// <summary>
        /// 同步是否已添加组
        /// </summary>
        private bool syncIsAddGroup = false;
        /// <summary>
        /// 同步读写接口对象，创建组对象时产生，执行同步读写时用
        /// </summary>
        private IOPCSyncIO2 syncIOPCIO2Obj = null;
        /// <summary>
        /// 同步是否已添加项
        /// </summary>
        private bool syncIsAddItems = false;
        #endregion

        #region 异步通讯变量
        /// <summary>
        /// 异步组对象，创建组对象时产生，添加Item时用
        /// </summary>
        private Object asyncGroupObj = null;
        /// <summary>
        /// 异步组句柄，创建组对象时产生，释放内存时用
        /// </summary>
        private int asyncGroupHandle = 0;
        /// <summary>
        /// 异步是否已添加组
        /// </summary>
        private bool asyncIsAddGroup = false;
        /// <summary>
        /// 异步读写接口对象，创建组对象时产生，执行同步读写时用
        /// </summary>
        private IOPCAsyncIO2 asyncIOPCIO2Obj = null;
        /// <summary>
        /// 异步是否已添加项
        /// </summary>
        private bool asyncIsAddItems = false;
        /// <summary>
        /// 通信组状态接口对象，用于异步通信状态设置
        /// </summary>
        private IOPCGroupStateMgt iGroupStateMgtObj = null;
        private Int32 dwCookie = 0;
        private IConnectionPointContainer pIConnectionPointContainer = null;
        private IConnectionPoint pIConnectionPoint = null;
        #endregion

        #region 构造函数
        public OPCServer()
        {
            opcConn = new OPCServerConnection();
            serverObj = opcConn.serverObj;
            isNative = true;
        }
        public OPCServer(OPCServerConnection opcConn)
        {
            this.opcConn = opcConn;
            if (!opcConn.isConnected)
                opcConn.Connect();
            serverObj = opcConn.serverObj;
            isNative = false;
        }
        #endregion

        #region 同步通讯相关函数
        /// <summary>
        /// 添加同步组,失败返回false，成功返回true
        /// </summary>
        public bool SyncAddGroup()
        {
            
                //返回的实际更新速率值单位ms
                Int32 pRevUpdaterate;
                //客户端的组句柄
                Int32 hClientGroup = opcConn.groupNum;
                //在生成组对象的时候组的异步通信是否被激活，0不被激活
                int bActive = 0;
                string groupName = "group" + opcConn.groupNum.ToString();
                opcConn.groupNum++;
                GCHandle hTimeBias, hDeadband;
                hTimeBias = GCHandle.Alloc(timeBias, GCHandleType.Pinned);
                hDeadband = GCHandle.Alloc(deadband, GCHandleType.Pinned);
                Guid iidRequiredInterface = typeof(IOPCItemMgt).GUID;
                if (!opcConn.isConnected)
                {
                    if (!opcConn.Connect()) return false; //如果还没有没有建立连接，先建立连接
                }
                try
                {   //返回值类型为void
                    serverObj.AddGroup(groupName, bActive,
                             dwRequestedUpdateRate, hClientGroup,
                             hTimeBias.AddrOfPinnedObject(), hDeadband.AddrOfPinnedObject(),
                             LOCALE_ID, out syncGroupHandle,
                             out pRevUpdaterate, ref iidRequiredInterface, out syncGroupObj);
                    syncIOPCIO2Obj = (IOPCSyncIO2)syncGroupObj;
                    syncIsAddGroup = true;
                }
                catch (System.Exception error)
                {
                    syncIsAddGroup = false;
                    MessageBox.Show(deviceName + "创建同步组对象时出错:" + error.Message, "建组出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (hDeadband.IsAllocated)
                        hDeadband.Free();
                    if (hTimeBias.IsAllocated)
                        hTimeBias.Free();
                }
                return syncIsAddGroup;
        }

        /// </summary>
        /// <param name="items">添加同步读写数据项，Items为读写对象数组</param>
        /// <returns>添加Items是否执行成功</returns>
        public bool SyncAddItems(OPCITEMDEF[] items, int[] itemHandle)
        {
            
                string errText = string.Empty;
                IntPtr pResults = IntPtr.Zero;
                IntPtr pErrors = IntPtr.Zero;
                if (!syncIsAddGroup)
                {
                    if (!SyncAddGroup())
                        return false;  //如果还没有没有添加组，先添加组
                }
                try
                {
                    ((IOPCItemMgt)syncGroupObj).AddItems(items.Length, items, out  pResults, out pErrors);
                    int[] errors = new int[items.Length];
                    Marshal.Copy(pErrors, errors, 0, items.Length);
                    IntPtr pos = pResults;
                    OPCITEMRESULT result;
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (errors[i] == 0)
                        {
                            result = (OPCITEMRESULT)Marshal.PtrToStructure(pos, typeof(OPCITEMRESULT));
                            itemHandle[i] = result.hServer;
                            pos = new IntPtr(pos.ToInt32() + Marshal.SizeOf(typeof(OPCITEMRESULT)));
                            syncIsAddItems = true;
                        }
                        else
                        {
                            syncIsAddItems = false;
                            string pstrError;
                            serverObj.GetErrorString(errors[i], LOCALE_ID, out pstrError);
                            Console.WriteLine(items[i].szItemID);
                            //MessageBox.Show(deviceName + "添加同步" + items[i].szItemID + "对象时出错" + pstrError, "添加Item对象出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }
                }
                catch (System.Exception ex) // catch for add item  
                {
                    syncIsAddItems = false;
                    MessageBox.Show(deviceName + "添加同步Item对象时出错" + ex.Message, "添加Item对象出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Free the memory  
                    if (pResults != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(pResults);
                        pResults = IntPtr.Zero;
                    }
                    if (pErrors != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(pErrors);
                        pErrors = IntPtr.Zero;
                    }
                }
                return syncIsAddItems;
        }
        /// <summary>
        /// 同步写方法
        /// </summary>
        public bool SyncWrite(object[] values, int[] itemHandle) //由编程人员保证，所写数据和添加Item的数据说明相对应
        {
         
                IntPtr pErrors = IntPtr.Zero;
                bool isWrited = false;
                try
                {
                    if (values.Length != itemHandle.Length)
                    {
                        MessageBox.Show(deviceName + "同步写入数据的个数与添加Item的数据说明长度不一致", "写数据出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    syncIOPCIO2Obj.Write(values.Length, itemHandle, values, out pErrors);//四个参数
                    int[] errors = new int[values.Length];
                    Marshal.Copy(pErrors, errors, 0, values.Length);
                    for (int i = 0; i < values.Length; i++)
                    {
                        if (errors[i] != 0)  //写数据不成功
                        {
                            string pstrError;   //需不需要释放？
                            serverObj.GetErrorString(errors[i], LOCALE_ID, out pstrError);
                            MessageBox.Show(deviceName + "同步写入第" + i.ToString() + "个数据时出错:" + pstrError, "写数据出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            isWrited = false;
                            break;
                        }
                        else
                        {
                            isWrited = true;
                        }
                    }
                }
                catch (System.Exception error)
                {
                    isWrited = false;
                    MessageBox.Show(deviceName + "同步写数据时出错:" + error.Message, "写数据出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (pErrors != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(pErrors);
                        pErrors = IntPtr.Zero;
                    }
                }
                return isWrited;
        }
        public bool SyncRead(object[] values, int[] itemHandle)
        {
           
                IntPtr pItemValues = IntPtr.Zero;
                IntPtr pos = IntPtr.Zero;
                IntPtr pErrors = IntPtr.Zero;
                bool isRead = false;
                try
                {
                    if (values.Length != itemHandle.Length)
                    {
                        MessageBox.Show("同步读需要读出数据的个数与添加Item的数据说明长度不一致", "读数据出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    syncIOPCIO2Obj.Read(OPCDATASOURCE.OPC_DS_DEVICE, itemHandle.Length, itemHandle, out pItemValues, out pErrors);
                    int[] errors = new int[itemHandle.Length];
                    Marshal.Copy(pErrors, errors, 0, itemHandle.Length);
                    OPCITEMSTATE pItemState = new OPCITEMSTATE();
                    for (int i = 0; i < itemHandle.Length; i++)
                    {
                        if (errors[i] == 0)
                        {
                            pos = new IntPtr(pItemValues.ToInt32() + i * Marshal.SizeOf(typeof(OPCITEMSTATE)));
                            pItemState = (OPCITEMSTATE)Marshal.PtrToStructure(pos, typeof(OPCITEMSTATE));
                            values[i] = pItemState.vDataValue.ToString();   //pItemState中还包含质量和时间等信息，目前只使用了读取的数据值
                            Marshal.DestroyStructure(pos, typeof(OPCITEMSTATE));
                            isRead = true;
                        }
                        else
                        {
                            string pstrError;   //需不需要释放？
                            serverObj.GetErrorString(errors[i], LOCALE_ID, out pstrError);
                            MessageBox.Show("同步读第" + i.ToString() + "个数据时出错:" + pstrError, "读数据出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            isRead = false;
                            break;
                        }
                    }
                }
                catch (System.Exception error)
                {
                    isRead = false;
                    MessageBox.Show("同步读数据时出错:" + error.Message, "读数据出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (pItemValues != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(pItemValues);
                        pItemValues = IntPtr.Zero;
                    }
                 
                    if (pErrors != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(pErrors);
                        pErrors = IntPtr.Zero;
                    }
                }
                return isRead;
            
        }
        #endregion

        #region 异步通信相关函数
        /// <summary>
        /// 添加异步组,失败返回false，成功返回true
        /// </summary>
        public bool AsyncAddGroup()
        {
           
                //返回的实际更新速率值单位ms
                Int32 pRevUpdaterate;
                //客户端的组句柄
                Int32 hClientGroup = opcConn.groupNum;
                //在生成组对象的时候组的异步通信是否被激活，0不被激活
                int bActive = 0;
                string groupName = "group" + opcConn.groupNum.ToString();
                opcConn.groupNum++;
                GCHandle hTimeBias, hDeadband;
                hTimeBias = GCHandle.Alloc(timeBias, GCHandleType.Pinned);
                hDeadband = GCHandle.Alloc(deadband, GCHandleType.Pinned);
                Guid iidRequiredInterface = typeof(IOPCItemMgt).GUID;
                if (!opcConn.isConnected)
                {
                    if (!opcConn.Connect()) return false; //如果还没有没有建立连接，先建立连接
                }
                try
                {   //返回值类型为void
                    serverObj.AddGroup(groupName, bActive,
                             dwRequestedUpdateRate, hClientGroup,
                             hTimeBias.AddrOfPinnedObject(), hDeadband.AddrOfPinnedObject(),
                             LOCALE_ID, out asyncGroupHandle,
                             out pRevUpdaterate, ref iidRequiredInterface, out asyncGroupObj);
                    asyncIOPCIO2Obj = (IOPCAsyncIO2)asyncGroupObj;

                    iGroupStateMgtObj = (IOPCGroupStateMgt)asyncGroupObj;
                    pIConnectionPointContainer = (IConnectionPointContainer)asyncGroupObj;
                    Guid iid = typeof(IOPCDataCallback).GUID;
                    pIConnectionPointContainer.FindConnectionPoint(ref iid, out pIConnectionPoint);
                    pIConnectionPoint.Advise(this, out dwCookie);

                    asyncIsAddGroup = true;
                }
                catch (System.Exception error)
                {
                    asyncIsAddGroup = false;
                    MessageBox.Show(deviceName + "创建异步组对象时出错:" + error.Message, "建组出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    if (hDeadband.IsAllocated)
                        hDeadband.Free();
                    if (hTimeBias.IsAllocated)
                        hTimeBias.Free();
                }
                return asyncIsAddGroup;
            
        }
        /// </summary>
        /// <param name="items">添加异步读写数据项，Items为读写对象数组</param>
        /// <returns>添加Items是否执行成功</returns>
        public bool AsyncAddItems(OPCITEMDEF[] items, int[] itemHandle)
        {
            
                string errText = string.Empty;
                IntPtr pResults = IntPtr.Zero;
                IntPtr pErrors = IntPtr.Zero;
                if (!asyncIsAddGroup)
                {
                    if (!AsyncAddGroup())
                        return false;  //如果还没有没有添加组，先添加组
                }
                try
                {
                    ((IOPCItemMgt)asyncGroupObj).AddItems(items.Length, items, out  pResults, out pErrors);
                    int[] errors = new int[items.Length];
                    Marshal.Copy(pErrors, errors, 0, items.Length);
                    IntPtr pos = pResults;
                    OPCITEMRESULT result;
                    for (int i = 0; i < items.Length; i++)
                    {
                        if (errors[i] == 0)
                        {
                            result = (OPCITEMRESULT)Marshal.PtrToStructure(pos, typeof(OPCITEMRESULT));
                            itemHandle[i] = result.hServer;
                            pos = new IntPtr(pos.ToInt32() + Marshal.SizeOf(typeof(OPCITEMRESULT)));
                            asyncIsAddItems = true;
                        }
                        else
                        {
                            asyncIsAddItems = false;
                            string pstrError;
                            serverObj.GetErrorString(errors[i], LOCALE_ID, out pstrError);
                            Console.WriteLine(items[i].szItemID);
                            //MessageBox.Show(deviceName + "添加异步" + items[i].szItemID + "对象时出错" + pstrError, "添加Item对象出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }
                    }
                }
                catch (System.Exception ex) // catch for add item  
                {
                    asyncIsAddItems = false;
                    MessageBox.Show(deviceName + "添加异步Item对象时出错" + ex.Message, "添加Item对象出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                finally
                {
                    // Free the memory  
                    if (pResults != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(pResults);
                        pResults = IntPtr.Zero;
                    }
                    if (pErrors != IntPtr.Zero)
                    {
                        Marshal.FreeCoTaskMem(pErrors);
                        pErrors = IntPtr.Zero;
                    }
                }
                return asyncIsAddItems;
            
        }
        /// <summary>
        /// 设置异步更新状态，使之触发或关闭OnDataChange事件函数
        /// </summary>
        /// <param name="group"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public void SetState(bool value)
        {
            
                IntPtr pRequestedUpdateRate = IntPtr.Zero;
                int nRevUpdateRate;
                IntPtr hClientGroup = IntPtr.Zero;
                IntPtr pTimeBias = IntPtr.Zero;
                IntPtr pDeadband = IntPtr.Zero;
                IntPtr pLCID = IntPtr.Zero;
                int nActive = 0;

                // activates or deactivates group according to value status  
                GCHandle hActive = GCHandle.Alloc(nActive, GCHandleType.Pinned);
                if (value != true)
                    hActive.Target = 0;
                else
                    hActive.Target = 1;
                try
                {
                    iGroupStateMgtObj.SetState(pRequestedUpdateRate, out nRevUpdateRate, hActive.AddrOfPinnedObject(), pTimeBias, pDeadband, pLCID, hClientGroup);
                }
                finally
                {
                    hActive.Free();
                }
            
        }

        /// <summary>
        /// 异步读方法
        /// </summary>
        private void AsyncRead(int[] itemHandle)
        {
           
                int nCancelid;
                IntPtr pErrors = IntPtr.Zero;
                if (asyncIOPCIO2Obj != null)
                {
                    try
                    {
                        asyncIOPCIO2Obj.Read(itemHandle.Length, itemHandle, itemHandle.Length, out nCancelid, out pErrors);
                        int[] errors = new int[itemHandle.Length];
                        Marshal.Copy(pErrors, errors, 0, itemHandle.Length);
                    }
                    catch (System.Exception error)
                    {
                        MessageBox.Show(deviceName + "异步读出错" + error.Message, "异步读出错", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            
        }

        public void AsyncWrite(object[] values, int[] itemHandle)
        {
            IntPtr pErrors = IntPtr.Zero;
            //bool isWrited = false;
            int nCancelid;
            try
            {
                if (values.Length == itemHandle.Length)
                {

                    asyncIOPCIO2Obj.Write(values.Length, itemHandle, values, values.Length, out nCancelid, out pErrors);//四个参数
                    int[] errors = new int[values.Length];
                    Marshal.Copy(pErrors, errors, 0, values.Length);

                    for (int i = 0; i < values.Length; i++)
                    {
                        if (errors[i] != 0)  //写数据不成功
                        {
                            String pstrError;   //需不需要释放？
                            serverObj.GetErrorString(errors[i], LOCALE_ID, out pstrError);
                            MessageBox.Show(deviceName + "-" + deviceId + "asynchronous write  the " + i.ToString() + "data is error." + pstrError,
                                "write data is error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            // isWrited = false;
                            break;
                        }
                        else
                        {
                            // isWrited = true;
                        }
                    }
                }
                else
                {
                    MessageBox.Show(deviceName + "The number of asynchronous object is not consistent with the length of data description added with Item",
                          "write data is error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (System.Exception error)
            {
                // isWrited = false;
                //MessageBox.Show(deviceName + "asynchronous write data is error :" + error.Message,
                //    "write data is error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                if (pErrors != IntPtr.Zero)
                {
                    Marshal.FreeCoTaskMem(pErrors);
                    pErrors = IntPtr.Zero;
                }
            }
            return;
        }
        /// <summary>
        /// 异步读订阅数据改变回调函数
        /// </summary>
        public virtual void OnDataChange(Int32 dwTransid, Int32 hGroup, Int32 hrMasterquality, Int32 hrMastererror, Int32 dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
        { }
        /// <summary>
        /// 异步读完成回调函数
        /// </summary>
        public virtual void OnReadComplete(Int32 dwTransid, Int32 hGroup, Int32 hrMasterquality, Int32 hrMastererror, Int32 dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
        { }
        /// <summary>
        /// 异步读写取消完成回调函数
        /// </summary>
        public virtual void OnCancelComplete(int dwTransid, int hGroup)
        { }
        /// <summary>
        /// 异步写完成回调函数
        /// </summary>
        public virtual void OnWriteComplete(int dwTransid, int hGroup, int hrMastererr, int dwCount, int[] pClienthandles, int[] pErrors)
        { }
        #endregion

        ~OPCServer()
        {
            DisConnect();
        }

        #region 断开连接函数
        /// <summary>
        /// 断开连接函数
        /// </summary>
        public void DisConnect()
        {
            try
            {
                syncIsAddGroup = false;
                syncIsAddItems = false;
                if (syncIOPCIO2Obj != null)
                {
                    Marshal.ReleaseComObject(syncIOPCIO2Obj);
                    syncIOPCIO2Obj = null;
                }
                if (syncGroupHandle != 0)
                {
                    serverObj.RemoveGroup(syncGroupHandle, 0);
                    syncGroupHandle = 0;
                }
                if (syncGroupObj != null)
                {
                    Marshal.ReleaseComObject(syncGroupObj);
                    syncGroupObj = null;
                }
                asyncIsAddGroup = false;
                asyncIsAddItems = false;
                if (asyncIOPCIO2Obj != null)
                {
                    Marshal.ReleaseComObject(asyncIOPCIO2Obj);
                    asyncIOPCIO2Obj = null;
                }
                if (asyncGroupHandle != 0)
                {
                    serverObj.RemoveGroup(asyncGroupHandle, 0);
                    asyncGroupHandle = 0;
                }
                if (asyncGroupObj != null)
                {
                    Marshal.ReleaseComObject(asyncGroupObj);
                    asyncGroupObj = null;
                }
                if (isNative)
                {
                    opcConn.DisConnect();
                }
            }
            catch (System.Exception error)
            {
                MessageBox.Show(error.Message.ToString().Trim());
            }
        }
        #endregion

        #region 目前系统中没有使用的方法
        /// <summary>
        /// 判断通讯质量，目前没有使用
        /// </summary>
        /// <param name="wQuality"></param>
        /// <returns></returns>
        private string GetQuality(long wQuality)
        {
            string strQuality = "";
            switch (wQuality)
            {
                //case Qualities.OPC_QUALITY_GOOD:
                //    strQuality = "Good";
                //    break;
                //case Qualities.OPC_QUALITY_BAD:
                //    strQuality = "Bad";
                //    break;
                //case Qualities.OPC_QUALITY_CONFIG_ERROR:
                //    strQuality = "BadConfigurationError";
                //    break;
                //case Qualities.OPC_QUALITY_NOT_CONNECTED:
                //    strQuality = "BadNotConnected";
                //    break;
                //case Qualities.OPC_QUALITY_DEVICE_FAILURE:
                //    strQuality = "BadDeviceFailure";
                //    break;
                //case Qualities.OPC_QUALITY_SENSOR_FAILURE:
                //    strQuality = "BadSensorFailure";
                //    break;
                //case Qualities.OPC_QUALITY_COMM_FAILURE:
                //    strQuality = "BadCommFailure";
                //    break;
                //case Qualities.OPC_QUALITY_OUT_OF_SERVICE:
                //    strQuality = "BadOutOfService";
                //    break;
                //case Qualities.OPC_QUALITY_WAITING_FOR_INITIAL_DATA:
                //    strQuality = "BadWaitingForInitialData";
                //    break;
                //case Qualities.OPC_QUALITY_EGU_EXCEEDED:
                //    strQuality = "UncertainEGUExceeded";
                //    break;
                //case Qualities.OPC_QUALITY_SUB_NORMAL:
                //    strQuality = "UncertainSubNormal";
                //    break;
                default:
                    strQuality = "Not handled";
                    break;
            }

            return strQuality;
        }
        /// <summary>
        /// 对OPCServer读出数据时间标签的计量形式转换，目前没有使用。
        /// </summary>
        /// <param name="ft"></param>
        /// <returns></returns>
        private DateTime ToDateTime(OpcRcw.Da.FILETIME ft)
        {
            long highbuf = (long)ft.dwHighDateTime;
            long buffer = (highbuf << 32) + ft.dwLowDateTime + 8 * 36000000000L;
            return DateTime.FromFileTimeUtc(buffer);
        }
        #endregion
    }
}
