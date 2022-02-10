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
using MySql.Data.MySqlClient;
using PLC;

namespace JY_Sinoma_WCS
{
    public class AGV:OPCServer
    {
        #region 自定义变量
        public frmMain mainFrm;
        public ConnectPool dbConn;
        public SystemStatus systemstatus;
        public int nCount = 0;
        /// <summary>
        /// 是否正在关闭
        /// </summary>
        public bool closing = false;
        /// <summary>
        /// 是否是自动执行任务
        /// </summary>
        public bool auto = true;
        /// <summary>
        /// 是否正在执行任务
        /// </summary>
        public bool running = true;
        /// <summary>
        /// 辊道图标
        /// </summary>
        public Label[] lb;
        /// <summary>
        /// 是否已连接设备
        /// </summary>
        public bool isBindToPLC = false;
        /// <summary>
        /// 查询字符串
        /// </summary>
        public string strsql = string.Empty;
        /// <summary>
        /// 数据库连接变量
        /// </summary>
        public MySqlConnection con;
        /// <summary>
        /// 任务状态更新线程
        /// </summary>
        public Thread AGVThread;
        /// <summary>
        /// 故障信息
        /// </summary>
        public string strError = string.Empty;
        /// <summary>
        /// 是否显示故障信息
        /// </summary>
        public bool showError = false;
    
        #endregion

        #region 构造函数
        public AGV(frmMain mainFrm, DataTable dt)
        {
            lb = new Label[dt.Rows.Count];
            this.mainFrm = mainFrm;
            this.dbConn = mainFrm.dbConn;
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                //定义图片
                lb[i] = new Label();
                lb[i].Size = new Size(106, 38);
                lb[i].Location = new Point(13, 30);
                lb[i].BackColor = Color.Gray;
                lb[i].Font = new Font("宋体", 14.25f, FontStyle.Bold, GraphicsUnit.Point, ((byte)(134)));
                lb[i].TextAlign = ContentAlignment.MiddleCenter;
                lb[i].Text = "AGV";
                lb[i].DoubleClick += new System.EventHandler(this.lb_DoubleClick);
                lb[i].Tag = i.ToString();
                mainFrm.pAGV.Controls.Add(lb[i]);

                AGVThread = new Thread(new ParameterizedThreadStart(AGVTask));
                AGVThread.IsBackground = true;
                // 判断该线程是否被垃圾回收
                if (!AGVThread.IsAlive)
                {
                    AGVThread.Start(i);
                }
                i++;
            }
        }
        #endregion

        #region 图片双击事件
        public void lb_DoubleClick(object sender, EventArgs e) 
        {
            FrmAGV f = new FrmAGV(this, mainFrm.pumping);
            f.Show();
        }
        #endregion 
    
        #region 显示AGV图片
        /// <summary>
        /// 刷新AGV图片
        /// </summary>
        public void DisplayStatus()
        { 
            
            try
            {
                for (int i = 0; i < this.nCount; i++)
                {
                    if (!mainFrm.AGVTaskCreate)
                        lb[i].BackColor = Color.DeepSkyBlue;
                    else
                    {
                        if (!auto)
                        {
                              lb[i].BackColor = Color.LightGreen;
                        }
                        else
                        {
                            if (!running)
                                lb[i].BackColor = Color.Green;
                            else
                                lb[i].BackColor = Color.Gold;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
        
        #region AGV任务主线程
        
        public void AGVTask(object deviceID)
        {
            while (!closing)
            {
                Thread.Sleep(100);
                if (mainFrm.bPicking)//开始分拣
                {
                    using (MySqlConnection conn = dbConn.GetConnectFromPool())
                    {
                        try
                        {
                            string rs = string.Empty;
                            DataRow dr;
                            DataRow row = DataBaseInterface.GetAgvTaskStatus(conn);
                            if(row==null)
                            {
                                running = false;
                                Thread.Sleep(200);
                                continue;
                            }
                            int taskStatus = int.Parse(row["taskstatus"].ToString());
                            if (taskStatus == 0)
                            {
                                running = false;
                                Thread.Sleep(200);
                                continue;
                            }
                            if (showError)
                            {
                                strError = string.Empty;
                            }
                            running = true;
                            if (taskStatus > 0 && taskStatus <= 3)
                            {
                                try
                                {
                                    DataBaseInterface.AGVTaskUpdate(conn,int.Parse(row["task_id"].ToString()), 1, out rs);
                                }
                                catch (Exception ex)
                                {
                                    MessageBox.Show("运行存储过程出错:" + ex.Message);
                                    throw;
                                }
                            }


                            switch (taskStatus)
                            {
                                //如果已到达取货入口点(AGV)
                                case 3:
                                    //如果是空吨桶入缓存区任务
                                    if (row["task_type"].ToString() == "1")
                                    {
                                        if (mainFrm.pumping.PointBoxCodeSelect(conn,row["TASKSTARTPOINT"].ToString()) == row["box_code"].ToString() && mainFrm.pumping.PointStatus(row["TASKSTARTPOINT"].ToString()) != null && mainFrm.pumping.PointStatus(row["TASKSTARTPOINT"].ToString()).Value.completion == 1)
                                        {
                                            if (DataBaseInterface.AGVTaskUpdate(conn,int.Parse(row["task_id"].ToString()), 4, out rs) != 1)
                                                MessageBox.Show(rs);
                                        }
                                        else
                                            if (showError)
                                            strError += "空吨桶入缓存区任务抽液处记录箱号与任务托盘号不匹配！";
                                    }
                                    //如果是空吨桶出缓存区任务
                                    if (row["task_type"].ToString() == "2" || row["task_type"].ToString() == "4")
                                    {
                                        if (DataBaseInterface.AGVTaskUpdate(conn,int.Parse(row["task_id"].ToString()), 4, out rs) != 1)
                                            MessageBox.Show(rs);
                                    }
                                    //如果是吨桶入抽液处任务
                                    if (row["task_type"].ToString() == "3")
                                    {
                                        if (mainFrm.outConveyorCmd.OutBoundBoxCode() == row["box_code"].ToString())
                                        {
                                            if (DataBaseInterface.AGVTaskUpdate(conn,int.Parse(row["task_id"].ToString()), 4, out rs) != 1)
                                                MessageBox.Show(rs);
                                        }
                                        else
                                            if (showError)
                                            strError += "吨桶入抽液处任务出库口记录箱号与任务托盘号不匹配！";
                                    }
                                    break;
                                //如果取货完成（AGV）
                                case 6:
                                    //如果是空吨桶入缓存区任务
                                    if (row["task_type"].ToString() == "1")
                                    {
                                        string boxcode = row["box_code"].ToString();
                                        mainFrm.pumping.PointBoxCodeUpdate(row["TASKSTARTPOINT"].ToString(), boxcode);
                                        if (row["TASKSTARTPOINT"].ToString() == "1002")
                                        {
                                            mainFrm.pumping.WriteSingleAction(1, 1, 6);
                                        }
                                      else  if (row["TASKSTARTPOINT"].ToString() == "1001")
                                        {
                                            mainFrm.pumping.WriteSingleAction(0, 1, 6);
                                        }
                                        if (DataBaseInterface.AGVTaskUpdate(conn,int.Parse(row["task_id"].ToString()), 7, out rs) != 1)
                                            MessageBox.Show(rs);
                                    }
                                    //如果是空吨桶出缓存区任务
                                    if (row["task_type"].ToString() == "2" || row["task_type"].ToString() == "4")
                                    {
                                        if (DataBaseInterface.AGVTaskUpdate(conn,int.Parse(row["task_id"].ToString()), 7, out rs) != 1)
                                            MessageBox.Show(rs);
                                    }
                                    //如果是吨桶入抽液处任务
                                    if (row["task_type"].ToString() == "3")
                                    {
                                        if (mainFrm.outConveyorCmd.OutBoundBoxCode() == null)
                                        {
                                          //  mainFrm.pumping.WriteSingleAction(0, 1, 6);
                                            if (DataBaseInterface.AGVTaskUpdate(conn,int.Parse(row["task_id"].ToString()), 7, out rs) != 1)
                                                MessageBox.Show(rs);
                                        }
                                        else
                                            if (showError)
                                            strError += "一楼吨桶出库口仍然有货！";
                                    }
                                    break;
                                //已到达卸货入口点(AGV)
                                case 9:
                                    //如果是空吨桶入缓存区任务
                                    if (row["task_type"].ToString() == "1")
                                    {
                                        dr = DataBaseInterface.AgvLocationStatus(conn,row["taskendpoint"].ToString());
                                        if (dr["unit_status"].ToString() == "2")
                                            if (DataBaseInterface.AGVTaskUpdate(conn,int.Parse(row["task_id"].ToString()), 10, out rs) != 1)
                                                MessageBox.Show(rs);
                                    }
                                    //如果是空吨桶出缓存区任务
                                    if (row["task_type"].ToString() == "2" || row["task_type"].ToString() == "4")
                                    {
                                        if (DataBaseInterface.AGVTaskUpdate(conn,int.Parse(row["task_id"].ToString()), 10, out rs) != 1)
                                            MessageBox.Show(rs);
                                    }
                                    //如果是吨桶入抽液处任务
                                    if (row["task_type"].ToString() == "3")
                                    {
                                        if (mainFrm.pumping.PointStatus(row["taskendpoint"].ToString()) != null)
                                        {
                                            if (mainFrm.pumping.PointStatus(row["taskendpoint"].ToString()) != null && mainFrm.pumping.PointStatus(row["taskendpoint"].ToString()).Value.permit == 1)
                                            {
                                                if (DataBaseInterface.AGVTaskUpdate(conn,int.Parse(row["task_id"].ToString()), 10, out rs) != 1)
                                                    MessageBox.Show(rs);
                                            }
                                            else
                                            {
                                                if (showError)
                                                    strError += "当前抽液处不允许放桶";
                                            }
                                        }
                                        else
                                        {
                                            if (showError)
                                                strError += "任务类型与目的地址不匹配";
                                        }
                                    }
                                    break;
                                //如果卸货完成(AGV)6
                                case 12:
                                    //如果是空吨桶入缓存区任务
                                    if (row["task_type"].ToString() == "1")
                                    {
                                        if (DataBaseInterface.AGVTaskUpdate(conn,int.Parse(row["task_id"].ToString()), 13, out rs) != 1)
                                            MessageBox.Show(rs);
                                    }
                                    //如果是空吨桶出缓存区任务
                                    if (row["task_type"].ToString() == "2" || row["task_type"].ToString() == "4")
                                    {
                                        if (DataBaseInterface.AGVTaskUpdate(conn,int.Parse(row["task_id"].ToString()), 13, out rs) != 1)
                                            MessageBox.Show(rs);
                                    }
                                    //如果是吨桶入抽液处任务
                                    if (row["task_type"].ToString() == "3")
                                    {
                                        if (mainFrm.pumping.PointStatus(row["taskendpoint"].ToString()) != null)
                                        {
                                            
                                            if (DataBaseInterface.AGVTaskUpdate(conn,int.Parse(row["task_id"].ToString()), 13, out rs) != 1)
                                                MessageBox.Show(rs);
                                          
                                            if (row["taskendpoint"].ToString()=="1002")
                                            {
                                                mainFrm.pumping.WriteSingleAction(1, 1, 12);
                                            }
                                            else if (row["taskendpoint"].ToString() == "1001")
                                            {
                                                mainFrm.pumping.WriteSingleAction(0, 1, 12);
                                            }
                                        }
                                        else
                                        {
                                            if (showError)
                                                strError += "任务类型与目的地址不匹配";
                                        }
                                    }
                                    break;
                                default: break;
                            }
                            Thread.Sleep(200);
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
                if (showError)
                {
                    if (strError == string.Empty)
                        strError = "无下发任务故障";
                    MessageBox.Show(strError);
                }
                showError = false;
                Thread.Sleep(500);
            }
        }
        #endregion
    }
}
