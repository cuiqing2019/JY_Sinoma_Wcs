using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.Threading;
using DataBase;
using System.Configuration;
using System.Diagnostics;
using Sinoma_WCS;
using MySql.Data.MySqlClient;
using Device;

namespace JY_Sinoma_WCS
{
    public class OutAssign
    {
        #region 自定义变量
        private frmMain mainFrm;
        public ConnectPool dbConn;//定义数据库连接
        private Thread outboundThread;//出库线程
        public string rs = "";
        public string lastRs = string.Empty;
        #endregion

        #region 构造函数
        public OutAssign(frmMain mainFrm)
        {
            ThreadExceptionDialog.CheckForIllegalCrossThreadCalls = false;//允许直接访问线程之间的控件
            this.mainFrm = mainFrm;
            dbConn = mainFrm.dbConn;

            #region 定义出库货位分配线程
            outboundThread = new Thread(new ThreadStart(OutboundAssigning));
            outboundThread.IsBackground = true;
            if (!outboundThread.IsAlive)
                outboundThread.Start();
            #endregion
        }
        #endregion


        #region 出库/退库主线程
        public void OutboundAssigning()
        {
            while (true)
            {
                int cc = diepanCount.PICKStatusStructS[0].PickStopSpot;
                Thread.Sleep(100);
                if (mainFrm.bPicking)//开始分拣
                {
                    using (MySqlConnection conn = dbConn.GetConnectFromPool())
                    {
                        if (conn == null)
                        {
                            Thread.Sleep(1000);
                            continue;
                        }
                        try
                        {
                            
                            if (mainFrm.taskType[1] == 1 && !mainFrm.stopTaskCreate[1])
                            {
                                if(mainFrm.emptyPalletType==2)
                                {
                                    string strSQL = "select t.* from tb_outstockplan t where t.tasktype = 0 and t.status = 0";
                                    try
                                    {
                                        DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                                        if(ds!=null)
                                        {
                                            foreach (DataRow row in ds.Tables[0].Rows)
                                            {

                                                lock (DataBaseInterface.obLock)
                                                {
                                                    string strSql = "select t.GOODS_KINDS FROM td_plt_location_dic t where t.BOX_BARCODE = '"+ row["outplanno"].ToString() + "'";
                                                    DataSet ds1 = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSql);
                                                    if (ds1.Tables[0].Rows[0]["GOODS_KINDS"].ToString() == "1" && mainFrm.outConveyorCmd.OutBoundBoxCode() != null)
                                                            continue;
                                                    int nRte = DataBaseInterface.CreateOutBoundTask(conn, row["outplanno"].ToString(),"", 2, mainFrm.dealWay[1], out rs);
                                                    if (nRte != 1) //返回货位值
                                                    {
                                                        if (nRte < 0)
                                                        {
                                                            if (lastRs != rs)
                                                            {
                                                                MessageBox.Show("出库任务生成错误" + rs);
                                                                lastRs = rs;
                                                            }
                                                        }

                                                    }

                                                }
                                            }
                                        }
                                    }
                                    catch (Exception)
                                    {
                                        Thread.Sleep(1000);
                                        continue;
                                    }
                                }
                                else if(mainFrm.emptyPalletType==1 )
                                {
                                    string strSQL = "select t.* from tb_outstockplan t where t.tasktype = 0 and t.status = 0";
                                   

                                    try
                                    {
                                        DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                                     
                                        if (ds!=null)
                                        {
                                            foreach (DataRow row in ds.Tables[0].Rows)
                                            {
                                                string strsql = "select empty_status from td_empty_status t where id=2";
                                                DataSet ds2 = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strsql);
                                                int counts = int.Parse(ds2.Tables[0].Rows[0]["empty_status"].ToString());
                                                int nGoodsKinds = DataBaseInterface.GetGoodsKind(conn, row["outplanno"].ToString());
                                                if ((nGoodsKinds == 2 || nGoodsKinds==4) && counts <7)//圆桶需要计算数量
                                                {
                                                    lock (DataBaseInterface.obLock)
                                                    {
                                                        int nReturn = DataBaseInterface.CreateOutBoundTask(conn, row["outplanno"].ToString(),"", 2, mainFrm.dealWay[1], out rs);
                                                        if (nReturn != 1) //返回货位值
                                                        {
                                                            if (nReturn < 0)
                                                            {
                                                                if (lastRs != rs)
                                                                {
                                                                    MessageBox.Show("出库任务生成错误" + rs);
                                                                    lastRs = rs;
                                                                }
                                                            }
                                                        }
                                                        else
                                                        {
                                                           mainFrm.outTaskNum = counts + 1;
                                                            strSQL = "update td_empty_status set empty_status='"+ mainFrm.outTaskNum + "' where id=2";
                                                            if (DataBase.MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL) <= 0)
                                                            {
                                                                MessageBox.Show("叠盘计数错误");
                                                            }  
                                                        }
                                                        if (cc == 8)
                                                        {
                                                            mainFrm.isEmptyPalletIn = true;
                                                            continue;
                                                        }


                                                    }
                                                }
                                            
                                                else //吨桶不计算数量
                                                {
                                                    lock (DataBaseInterface.obLock)
                                                    {
                                                        if (mainFrm.outConveyorCmd.OutBoundBoxCode() != null)
                                                            continue;
                                                        int nReturn = DataBaseInterface.CreateOutBoundTask(conn, row["outplanno"].ToString(),"", 2, mainFrm.dealWay[1], out rs);
                                                        if (nReturn != 1) //返回货位值
                                                        {
                                                            if (nReturn < 0)
                                                            {
                                                                if (lastRs != rs)
                                                                {
                                                                    MessageBox.Show("出库任务生成错误" + rs);
                                                                    lastRs = rs;
                                                                }
                                                            }
                                                        }
                                                    }
                                                }

                                            }
                                              if (cc == 8)
                                            {
                                                mainFrm.isEmptyPalletIn = true;
                                                continue;
                                            }
                                        }
                                      
                                    }
                                    catch (Exception)
                                    {
                                        Thread.Sleep(1000);
                                        continue;
                                    }

                                }
                               
                            }
                            if (mainFrm.taskType[0] == 3 && !mainFrm.stopTaskCreate[0])
                            {
                                string strSQL = "select t.* from tb_outstockplan t where t.tasktype = 1 and t.status = 0";
                                try
                                {
                                    DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                                    if(ds!=null)
                                    {
                                        foreach (DataRow row in ds.Tables[0].Rows)
                                        {

                                            lock (DataBaseInterface.obLock)
                                            {
                                                int deviceId = mainFrm.inConveyorScannerCmd.GoodsStatus();
                                                if (DataBaseInterface.CreateOutBoundTask(conn, row["outplanno"].ToString(),deviceId.ToString(), 4, mainFrm.dealWay[0], out rs) != 1) //返回货位值          
                                                    if (lastRs != rs)
                                                    {
                                                        MessageBox.Show("退库任务生成错误" + rs);
                                                        lastRs = rs;
                                                    }
                                            }
                                        }
                                    }
                                  
                                }
                                catch (Exception)
                                {
                                    Thread.Sleep(1000);
                                    continue;
                                }
                            }
                        }
                        catch (Exception)
                        {
                            Thread.Sleep(1000);
                            continue;
                        }
                    }
                }
                else
                {
                    Thread.Sleep(1000);
                    continue;
                }
                Thread.Sleep(1000);
            }
        }
        #endregion
    }
}
