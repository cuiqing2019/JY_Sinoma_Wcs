//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Windows.Forms;
//using System.IO.Ports;
//using System.Threading;
//using DataBase;
//using System.Configuration;
//using System.Data.OracleClient;
//using System.Diagnostics;
//using Sinoma_WCS;

//namespace Sinoma_WCS
//{
//    public class AGVTaskAssign
//    {
//        #region 自定义变量
//        private frmMain mainFrm;
//        public ConnectPool dbConn;//定义数据库连接
//        private Thread AGVThread;//出库线程
//        public string rs = "";
//        /// <summary>
//        /// 工作模式 1-自动 0-手动
//        /// </summary>
//        public int worktType = 0;
//        #endregion

//        #region 构造函数
//        public AGVTaskAssign(frmMain mainFrm)
//        {
//            ThreadExceptionDialog.CheckForIllegalCrossThreadCalls = false;//允许直接访问线程之间的控件
//            this.mainFrm = mainFrm;
//            dbConn = mainFrm.dbConn;

//            #region 定义出库货位分配线程
//            AGVThread = new Thread(new ThreadStart(AGVAssigning));
//            AGVThread.IsBackground = true;
//            if (!AGVThread.IsAlive)
//                AGVThread.Start();
//            #endregion
//        }
//        #endregion


//        #region AGV主线程
//        public void AGVAssigning()
//        {
//            while (true)
//            {
//                if (mainFrm.bPicking)//开始分拣
//                {
//                    string boxCode;
//                    string toUnit = string.Empty;
//                    string fromUnit = string.Empty;
//                    string rsRet = string.Empty;
//                    MySqlConnection conn = dbConn.GetConnectFromPool();
//                    //if (conn == null)
//                    {
//                        Thread.Sleep(1000);
//                        continue;
//                    }
//                    try
//                    {
//                        if (worktType == 1)
//                        {
//                            #region 生成空吨桶入横梁区或等待区任务
//                            if (mainFrm.pumping.PointStatus(2, out boxCode) != null && mainFrm.agv.auto)
//                            {
//                                toUnit = DataBaseInterface.SelectAGVLocation(0, out boxCode);
//                                fromUnit = mainFrm.pumping.PointStatus(2, out boxCode);
//                                if (toUnit != null)
//                                {
//                                    DataBaseInterface.AGVTaskCreate(boxCode, 1, fromUnit, toUnit, out rsRet);
//                                }
//                            }
//                            #endregion
//                        }
//                    }
//                    catch (Exception)
//                    {
//                        dbConn.DisposeConnectToPool(conn);
//                        Thread.Sleep(1000);
//                        continue;
//                    }
//                    dbConn.DisposeConnectToPool(conn);
//                }
//                else
//                {
//                    Thread.Sleep(1000);
//                    continue;
//                }
//                Thread.Sleep(1000);
//            }
//        }
//        #endregion


//    }
//}
