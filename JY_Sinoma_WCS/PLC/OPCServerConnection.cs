using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using OpcRcw.Da;
using OpcRcw.Comn;

namespace PLC
{
    /// <summary>
    /// OPCServer连接类
    /// </summary>
    public class OPCServerConnection
    {
        #region 自定义变量
        /// <summary>
        /// OPCServer对象
        /// </summary>
        public IOPCServer serverObj;
        /// <summary>
        /// 是否已连接
        /// </summary>
        public bool isConnected = false;
        /// <summary>
        /// 已建立组的数量，用于向连接中添加组时构造组名
        /// </summary>
        public int groupNum = 1;
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数，此时建立OPC连接
        /// </summary>
        public OPCServerConnection()
        {
            Connect();
        }
        #endregion

        #region 建立和OPCServer的连接
        /// <summary>
        /// 建立和OPCServer的连接
        /// </summary>
        public bool Connect() //返回值true为成功，false为失败
        {
            lock (this)
            {
                if (isConnected) return true;
                try
                {
#if _WGTEST_
                    Type svrComponenttyp = Type.GetTypeFromProgID("KEPware.KEPServerEx.V4", "localhost");
#else
                    Type svrComponenttyp = Type.GetTypeFromProgID("OPC.SimaticNet", "localhost");
#endif
                    serverObj = (IOPCServer)Activator.CreateInstance(svrComponenttyp);
                    isConnected = true;
                }
                catch (System.Exception error)
                {
                    isConnected = false;
                    MessageBox.Show(string.Format("建立OPCServer连接失败:-{0}", error.Message),
                        "连接失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            return isConnected;
        }
        #endregion

        #region 断开和OPCServer的连接
        /// <summary>
        /// 断开和OPCServer的连接
        /// </summary>
        public void DisConnect()
        {
            try
            {
                isConnected = false;
                if (serverObj != null)
                {
                    Marshal.ReleaseComObject(serverObj);
                    serverObj = null;
                }
            }
            catch (System.Exception error)
            {
                MessageBox.Show(error.Message.ToString().Trim());
            }
        }
        #endregion
    }
}
