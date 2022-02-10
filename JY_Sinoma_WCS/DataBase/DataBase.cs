using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.IO;
using MySql.Data.MySqlClient;

namespace DataBase
{
    /// <summary>
    /// 数据操作类
    /// </summary>
    public static class DataBaseInterface
    {
        public static ConnectPool dbConn;//定义数据库连接
        public static object obLock = new object();
        private static FileStream fs = new FileStream("TaskCmd.log", FileMode.Append, FileAccess.Write);
        public static void InsertTaskLog(MySqlConnection dbConn, string txt)
        {
            string strLine = "";
            strLine += DateTime.Now.ToLongTimeString() + "\r\n";
            strLine += txt + "\r\n";
            byte[] aa = Encoding.Default.GetBytes(strLine);
            fs.Write(aa, 0, aa.Length);
            fs.Flush();
        }

        #region 生成入库主任务
        /// <summary>
        /// 生成入库主任务
        /// </summary>
        /// <param name="boxCode">箱号</param>
        /// <param name="taskType">任务类型 1-入库 2-出库 3-空托盘入库 </param>
        /// <param name="batchNo">托盘所属批次</param>
        /// <param name="goodsKinds">货物类型 0-空托盘 1-吨桶 2-圆桶</param>
        /// <param name="goodsSku">货物sku 空托盘默认sku为固定值</param>
        /// <param name="goodsWeight">货物重量</param>
        /// <param name="level">入库所在层</param>
        /// <param name="rs_ret">存储过程报错详细信息</param>
        /// <returns>存储过程运行结果 1-成功 其他失败</returns>
        public static int CreatInboundTaskM(MySqlConnection con, string boxCode, int taskType, string batchNo, string batchId, int goodsKinds, string goodsSku, double goodsWeight, int level, int taskIdM, int dealWay, string goodsName, string hazardArea, int nScannerNo, out string rs_ret)
        {
            //using (MySqlConnection con = dbConn.GetConnectFromPool())
            //{
            if (con == null)
            {
                rs_ret = "数据库连接已用完！";
                return -1;
            }
            try
            {
                rs_ret = string.Empty;
                MySqlParameter[] createInboundTaskMParameter = new MySqlParameter[]
                {
                    new MySqlParameter("ls_box_code",MySqlDbType.VarChar,20),
                    new MySqlParameter("li_task_type",MySqlDbType.Int64),
                    new MySqlParameter("ls_batch_no",MySqlDbType.VarChar,30),
                    new MySqlParameter("ls_batch_id",MySqlDbType.VarChar,30),
                    new MySqlParameter("li_goods_kinds",MySqlDbType.Int64),
                    new MySqlParameter("ls_sku",MySqlDbType.VarChar,30),
                    new MySqlParameter("li_goods_weight",MySqlDbType.Double),
                    new MySqlParameter("li_level",MySqlDbType.Int64),
                    new MySqlParameter("li_task_id_m",MySqlDbType.Int64),
                    new MySqlParameter("li_dealway",MySqlDbType.Int64),
                    new MySqlParameter("ls_goods_name",MySqlDbType.VarChar,30),
                    new MySqlParameter("ls_hazard_area",MySqlDbType.VarChar,2),
                    new MySqlParameter("li_scanner_id",MySqlDbType.Int64),
                    new MySqlParameter("ri_ret",MySqlDbType.Int64),
                    new MySqlParameter("rs_ret",MySqlDbType.VarChar,255),
                };
                createInboundTaskMParameter[0].Value = boxCode;
                createInboundTaskMParameter[1].Value = taskType;
                createInboundTaskMParameter[2].Value = batchNo;
                createInboundTaskMParameter[3].Value = batchId;
                createInboundTaskMParameter[4].Value = goodsKinds;
                createInboundTaskMParameter[5].Value = goodsSku;
                createInboundTaskMParameter[6].Value = goodsWeight;
                createInboundTaskMParameter[7].Value = level;
                createInboundTaskMParameter[8].Value = taskIdM;
                createInboundTaskMParameter[9].Value = dealWay;
                createInboundTaskMParameter[10].Value = goodsName;
                createInboundTaskMParameter[11].Value = hazardArea;
                createInboundTaskMParameter[12].Value = nScannerNo;
                createInboundTaskMParameter[13].Direction = ParameterDirection.Output;
                createInboundTaskMParameter[14].Direction = ParameterDirection.Output;
                MySqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "SP_InTaskToRoute", createInboundTaskMParameter);
                rs_ret = createInboundTaskMParameter[14].Value.ToString();
                return System.Convert.ToInt32(createInboundTaskMParameter[13].Value);
            }
            catch (Exception ex)
            {
                //MessageBox.Show("生成入库主任务存储过程出错：" + ex.Message);
                rs_ret = "生成入库主任务存储过程出错：" + ex.Message;
                return -1;
            }
            //}

        }
        #endregion

        #region 获取辊道任务
        public static string GetBarCode(MySqlConnection conn, int taskId)
        {
            string strBarCode = "";
            if (conn == null)
                return null;
            string strSQL = "select t.box_barcode from tb_plt_task_d t where t.task_id= " + taskId + " and t.status<2";
            try
            {
                DataSet ds = MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                if (ds != null && ds.Tables[0].Rows.Count > 0)
                {
                    strBarCode = (ds.Tables[0].Rows[0])["box_barcode"].ToString();
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("获取当前任务的：" + ex.Message);
            }
            return strBarCode;

        }
        #endregion
        #region 获取当前已扫描条码
        /// <summary>
        /// 获取当前已扫条码
        /// </summary>
        /// <param name="scanID">扫码器编号</param>
        /// <returns>已扫条码</returns>
        public static string GetCurrentBarCode(MySqlConnection con, string scanID)
        {
            if (con == null)
                return null;
            try
            {
                string sqlstr = " select * from tb_plt_scan_record t where t.scanner_id = " + scanID + " and t.status = 0";
                DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                        return null;
                    return ds.Tables[0].Rows[0]["box_barcode"].ToString();
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                MessageBox.Show("获取当前已扫条码查询语句出错：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 清除已经扫到但未处理的条码记录
        /// <summary>
        /// 清除指定扫码器已扫码但未处理的条码记录
        /// </summary>
        /// <param name="scanID">需要处理的扫码器ID</param>
        /// <returns>1-成功 0-失败</returns>
        public static int ClearHaveReadRFID(MySqlConnection con, string scanID)
        {
            if (con == null)
            {
                return 0;
            }
            try
            {
                string sql = "update tb_plt_scan_record t set t.status = 3 where t.status <> 1 and t.scanner_id = '" + scanID + "'";
                return MySqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("清除" + scanID + "扫码器已读标识语句运行出错：" + ex.Message);
                return 0;
            }

        }
        #endregion

        #region 更新已入库重量
        /// <summary>
        /// 更新已入库重量
        /// </summary>
        /// <param name="scanID">需要处理的扫码器ID</param>
        /// <returns>1-成功 0-失败</returns>
        public static int UpdateBatchWeight(MySqlConnection con, double weight, string batcheNo, string goodsSku)
        {
            //using (MySqlConnection con = dbConn.GetConnectFromPool())
            //{
            if (con == null)
            {
                return 0;
            }
            try
            {
                string sql = "update tb_shipment t set t.WASTEWEIGHT = " + weight.ToString() + " where t.VEHICLEORDERNO = '" + batcheNo + "' and t.WASTECODE = '" + goodsSku + "'";
                return MySqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
            }
            catch (Exception ex)
            {
                MessageBox.Show("更新已入库重量出错：" + ex.Message);
                return 0;
            }
            //}

        }
        #endregion

        #region 获取库位状态信息
        /// <summary>
        /// 获取库位状态信息
        /// </summary>
        /// <returns>库位表</returns>
        public static DataTable SelectLocationState()
        {
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    return null;
                }
                try
                {
                    string sql = "select t.* from td_plt_location_dic t";
                    return MySqlHelper.ExecuteDataset(con, CommandType.Text, sql).Tables[0];
                }
                catch (Exception ex)
                {
                    MessageBox.Show("查询库位状态信息出错：" + ex.Message);
                    return null;
                }
            }

        }
        #endregion
        #region 修改危险分区
        /// <summary>
        /// 修改危险分区
        /// </summary>
        public static int UpdateHazardArea(int pai, int lie1, int lie2, int ceng, string hazardArea)
        {
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    return 0;
                }
                try
                {
                    string sql = "update td_plt_location_dic t set t.hazard_area = '" + hazardArea + "' where t.row_no = " + pai.ToString() + " and t.level_no = " + ceng.ToString() + " and t.bay_no >= " + lie1.ToString() + " and t.bay_no <= " + lie2.ToString() + "";
                    return MySqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("修改危险分区：" + ex.Message);
                    return -1;
                }
            }

        }
        #endregion
        #region 获取主任务号
        public static string GetTaskM(MySqlConnection con)
        {
            string sqlstr = string.Empty;

            if (con == null)
            {
                return null;
            }
            try
            {
                sqlstr = "insert into tb_task_id(create_time,status)values(sysdate(),0)";
                if (MySqlHelper.ExecuteNonQuery(con, CommandType.Text, sqlstr) > 0)
                {
                    sqlstr = " select task_id from tb_task_id where status=0 order by create_time desc limit 1";
                    DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
                    if (ds != null)
                    {
                        sqlstr = "update tb_task_id set status=1 where task_id=" + int.Parse(ds.Tables[0].Rows[0]["task_id"].ToString());
                        if (MySqlHelper.ExecuteNonQuery(con, CommandType.Text, sqlstr) > 0)
                            return ds.Tables[0].Rows[0]["task_id"].ToString();
                        else
                            return null;
                    }
                    else
                        return null;

                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                MessageBox.Show("获取主任务号出错：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 通过主任务号查询子任务信息
        /// <summary>
        /// 通过主任务号查询子任务信息通过主任务号查询子任务信息
        /// </summary>
        /// <param name="taskid">主任务号</param>
        /// <returns>子任务行</returns>
        public static DataRow SelectTaskD(MySqlConnection con, string taskid, string deviceType)
        {
            //using (MySqlConnection con = dbConn.GetConnectFromPool())
            //{
            if (con == null)
            {
                return null;
            }
            try
            {
                string sql = "select t.*,m.goods_kind from tb_plt_task_d t,tb_plt_task_m m where t.task_id = '" + taskid + "' and t.device_type = '" + deviceType + "' and t.status<2 and t.task_id=m.task_id order by t.create_time desc";
                //string sql = "select t.* from tb_plt_task_d t where t.task_id = " + taskid + " and t.device_type = '" + deviceType + "' and t.status<2  order by t.create_time desc";
                DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                        return null;
                    else
                        return ds.Tables[0].Rows[0];
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                MessageBox.Show("通过任务号查询箱号出错：" + ex.Message);
                return null;
            }
            //}

        }
        public static DataRow SelectTaskD(MySqlConnection con)
        {
            //using (MySqlConnection con = dbConn.GetConnectFromPool())
            //{
            if (con == null)
            {
                return null;
            }
            try
            {
                string sql = "select * from tb_plt_task_d t where (t.from_unit='1001'or t.from_unit='1002') and t.device_type ='CON' and status<2 order by t.create_time";
                DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                        return null;
                    else
                        return ds.Tables[0].Rows[0];
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                MessageBox.Show("通过任务号查询箱号出错：" + ex.Message);
                return null;
            }
            //}

        }
        #endregion


        public static DataRow SelectTaskM(MySqlConnection con, string barcode)
        {
            //using (MySqlConnection con = dbConn.GetConnectFromPool())
            //{
            if (con == null)
            {
                return null;
            }
            try
            {
                string sql = "select m.* from tb_plt_task_m m where m.box_barcode = '" + barcode + "' and m.task_type = 1  and m.task_status<2  order by m.create_time desc";
                //string sql = "select t.* from tb_plt_task_d t where t.task_id = " + taskid + " and t.device_type = '" + deviceType + "' and t.status<2  order by t.create_time desc";
                DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                        return null;
                    else
                        return ds.Tables[0].Rows[0];
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                MessageBox.Show("通过任务号查询箱号出错：" + ex.Message);
                return null;
            }
            //}

        }
        public static DataRow SelectTaskM2(MySqlConnection con, string taskid)
        {
            //using (MySqlConnection con = dbConn.GetConnectFromPool())
            //{
            if (con == null)
            {
                return null;
            }
            try
            {
                string sql = "select m.* from tb_plt_task_d m where m.task_id = '" + taskid + "' and m.task_type = 3  and m.status<2 and m.step=1 order by m.create_time desc";
                //string sql = "select t.* from tb_plt_task_d t where t.task_id = " + taskid + " and t.device_type = '" + deviceType + "' and t.status<2  order by t.create_time desc";
                DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                        return null;
                    else
                        return ds.Tables[0].Rows[0];
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                MessageBox.Show("通过任务号查询箱号出错：" + ex.Message);
                return null;
            }
            //}

        }

        #region 通过主任务号查询条码
        /// <summary>
        /// 通过主任务号查询子任务信息通过主任务号查询条码
        /// </summary>
        /// <param name="taskid">主任务号</param>
        /// <returns>子任务行</returns>
        public static string SelectBoxCode(string taskid)
        {
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    return null;
                }
                try
                {
                    string sql = "select * from tb_plt_task_d t where t.task_id = " + taskid + " and t.status < 2";
                    DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count == 0)
                            return "无任务";
                        else
                            return ds.Tables[0].Rows[0]["box_barcode"].ToString();
                    }
                    else
                        return "无任务";

                }
                catch (Exception ex)
                {
                    MessageBox.Show("通过任务号查询箱号出错：" + ex.Message);
                    return null;
                }
            }

        }
        #endregion

        #region 通过主任务号货物类型
        /// <summary>
        /// 通过主任务号查询子任务信息通过主任务号查询条码
        /// </summary>
        /// <param name="taskid">主任务号</param>
        /// <returns>子任务行</returns>
        public static string SelectBoxgood(string taskid)
        {
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    return null;
                }
                try
                {
                    string sql = "select * from tb_plt_task_m t where t.task_id = " + taskid + " and t.status < 2";
                    DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count == 0)
                            return "无任务";
                        else
                            return ds.Tables[0].Rows[0]["goods_kind"].ToString();
                    }
                    else
                        return "无任务";
                }
                catch (Exception ex)
                {
                    MessageBox.Show("通过任务号查询箱号出错：" + ex.Message);
                    return null;
                }
            }

        }
        #endregion

        #region 通过主任务号查询废物重量信息
        /// <summary>
        /// 通过主任务号查询废物重量信息
        /// </summary>
        /// <param name="taskid">主任务号</param>
        /// <returns>废物重量</returns>
        public static int SelectTaskMWeight(string taskid)
        {
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    return 0;
                }
                try
                {
                    string sql = "select t.goods_weight from tb_plt_task_m t where t.task_id = " + taskid;
                    DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                    if (ds != null)
                    {
                        return Convert.ToInt32(ds.Tables[0].Rows[0]["goods_weight"].ToString());
                    }
                    else
                        return 0;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("通过任务号查询箱号出错：" + ex.Message);
                    return 0;
                }
            }

        }
        #endregion

        #region 查询接口表收到信息
        /// <summary>
        /// 查询接口表收到信息
        /// </summary>
        public static DataSet SelectComMessage()
        {
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    return null;
                }
                try
                {
                    string sql = "select * from TB_SHIPMENT t where t.VEHICLEORDERSTATE <2 order by t.DOWN_DATE,t.vehicleorderid";
                    return MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("查询接口表收到信息出错：" + ex.Message);
                    return null;
                }
            }

        }
        #endregion

        #region 查询一楼异常回库任务
        /// <summary>
        /// 查询一楼异常回库任务
        /// </summary>
        public static DataSet SelectRebackTaskM()
        {
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    return null;
                }
                try
                {
                    string sql = "select * from tb_plt_task_m t where t.task_type=5 and t.task_level=1 and t.task_status<2 order by t.create_time ";
                    return MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("查询一楼异常回库任务：" + ex.Message);
                    return null;
                }
            }

        }
        #endregion

        #region 验证入库口是否为异常回库模式
        public static DataSet SelectInPort(MySqlConnection conn, int nIndex)
        {
            //using (MySqlConnection conn = dbConn.GetConnectFromPool())
            //{
            if (conn == null)
            {
                return null;
            }
            try
            {
                string strSQL = "select * from td_inport_dic where port_id= " + nIndex;
                return MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);


            }
            catch (Exception ex)
            {
                return null;
            }

            //}
        }
        #endregion

        #region 查询正在执行入库任务的批次数量
        /// <summary>
        /// 查询接口表收到信息
        /// </summary>
        public static int SelectComMessageCount(string batchNo, string sku)
        {
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    return 0;
                }
                try
                {
                    string sql = "select t.* from tb_shipment t where t.VEHICLEORDERSTATE = 1";
                    DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count >= 1)
                        {
                            if (ds.Tables[0].Rows[0]["VEHICLEORDERNO"].ToString() == batchNo && ds.Tables[0].Rows[0]["WASTECODE"].ToString() == sku)
                                return 0;
                            return 1;
                        }
                        else
                            return 1;
                    }
                    else
                        return -1;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("查询接口表收到信息出错：" + ex.Message);
                    return -1;
                }
            }

        }
        #endregion

        #region 查询其他任务模式的任务数量
        /// <summary>
        /// 查询其他任务模式的任务数量
        /// </summary>
        public static int SelectOtherTaskCount(string taskType, int nLevel)
        {
            string strType = string.Empty;
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                switch (nLevel)
                {
                    case 1:
                        if (taskType == "1")
                            strType = "3,4,5";
                        else if (taskType == "3")
                            strType = "1,4,5";
                        else if (taskType == "4")
                            strType = "1,3,5";
                        else if (taskType == "5")
                            strType = "1,3,4";
                        break;
                    case 2:
                        if (taskType == "2")
                            strType = "3,5";
                        else if (taskType == "3")
                            strType = "2,5";
                        else if (taskType == "5")
                            strType = "2,3";
                        break;
                    default:
                        break;

                }
                try
                {
                    string sql = "select count(*) from TB_PLT_TASK_M t where t.task_type in(" + strType + ") and t.task_status < 2  and t.task_status >0 and task_level=" + nLevel;
                    return int.Parse(MySqlHelper.ExecuteDataset(con, CommandType.Text, sql).Tables[0].Rows[0]["count(*)"].ToString());

                }
                catch (Exception ex)
                {
                    MessageBox.Show("查询接口表收到信息出错：" + ex.Message);
                    return -1;
                }
            }

        }
        #endregion

        #region 查询可用库存的数量
        /// <summary>
        /// 查询可用库存的数量
        /// </summary>
        public static int SelectUseLocationNum(MySqlConnection conn, int goodsKinds)
        {
            if (conn == null)
                return -1;
            try
            {
                string sql = "select * from td_plt_location_dic t where t.use_status < 2 and t.unit_status = 0 and t.goods_kinds = " + goodsKinds.ToString();
                return MySqlHelper.ExecuteDataset(conn, CommandType.Text, sql).Tables[0].Rows.Count;

            }
            catch (Exception ex)
            {
                MessageBox.Show("查询接口表收到信息出错：" + ex.Message);
                return -1;
            }

        }
        #endregion

        #region 查询一楼可用的入库口
        public static DataSet SelectInPort(MySqlConnection conn)
        {
            DataSet ds = new DataSet();
            if (conn == null)
                return null;
            try
            {
                string sql = "select port_id,unit from td_inport_dic t where t.use_status=1 and t.task_type=1 and t.level_num=1 ";
                ds = MySqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
                return ds;

            }
            catch (Exception ex)
            {
                MessageBox.Show("查询接口表收到信息出错：" + ex.Message);
            }
            return ds;

        }
        #endregion

        #region 获取包装类型
        public static int GetGoodsKind(MySqlConnection conn, string strBoxCode)
        {
            if (conn == null)
                return 0;
            try
            {
                string strSQL = "select t.goods_kinds from td_plt_location_dic t where t.BOX_BARCODE='" + strBoxCode + "' ";
                DataSet ds = MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows[0]["goods_kinds"].ToString().Length > 0)
                        return int.Parse(ds.Tables[0].Rows[0]["goods_kinds"].ToString());
                    else
                        return 0;
                }
                else
                    return 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show("获取包装类型失败" + ex.Message);
            }
            return 0;
        }
        #endregion
        #region 修改批次状态
        /// <summary>
        /// 修改批次状态
        /// </summary>
        public static int UpdateComMessageState(string vehicleorderno, string shipmentdetailid, string wastecode, int state,string time)
        {
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    return 0;
                }
                try
                {
                    string sql = "update tb_shipment t set t.vehicleorderstate = " + state.ToString() + " where t.vehicleorderno = '" + vehicleorderno + "'and t.shipmentdetailid = '" + shipmentdetailid + "'and t.wastecode = '" + wastecode + "'and t.SHIPMENTDATE = '" + time + "'";
                    MySqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
                    sql = "select * from tb_comm_inbound where batch_no='" + vehicleorderno + "' and goods_sku='" + wastecode + "' and shipmentdetailid='" + shipmentdetailid + "' and box_code='start'";
                    DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        sql = "insert into tb_comm_inbound(batch_no,task_status,create_time,begin_time,finish_time,goods_sku,box_code,goods_weight,shipmentdetailid,dealway)values('" + vehicleorderno + "',2,sysdate(),sysdate(),sysdate(),'" + wastecode + "','start',0,'" + shipmentdetailid + "',0)";
                        return MySqlHelper.ExecuteNonQuery(con, CommandType.Text, sql); ;
                    }
                    else
                        return 0;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("修改批次状态报错：" + ex.Message);
                    return -1;
                }
            }

        }
        #endregion
        #region 修改批次状态
        /// <summary>
        /// 修改批次状态
        /// </summary>
        public static int UpdateComMessageState(string vehicleorderno, string shipmentdetailid, string wastecode, int state, int PALLETNUM, int TOTALNUM)
        {
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    return 0;
                }
                try
                {
                    string sql = "update tb_shipment t set t.vehicleorderstate = " + state.ToString() + " where t.vehicleorderno = '" + vehicleorderno + "'and t.shipmentdetailid = '" + shipmentdetailid + "'and t.wastecode = '" + wastecode + "'";
                    MySqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
                    sql = "insert into tb_comm_inbound(batch_no,task_status,create_time,begin_time,finish_time,goods_sku,box_code,goods_weight,shipmentdetailid,dealway,palletnum,totalnum)values('" + vehicleorderno + "',2,sysdate(),sysdate(),sysdate(),'" + wastecode + "','end',0,'" + shipmentdetailid + "',0," + PALLETNUM.ToString() + "," + TOTALNUM.ToString() + ")";
                    return MySqlHelper.ExecuteNonQuery(con, CommandType.Text, sql); ;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("修改批次状态报错：" + ex.Message);
                    return -1;
                }
            }

        }
        #endregion

        #region 查询当前批次已入托盘号
        public static int SelectTrayNum(string batchNo, string goodsSku)
        {
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    return 0;
                }
                try
                {
                    string sqlstr = @"select count(*) from tb_comm_inbound t where t.batch_no = '" + batchNo + "' and t.goods_sku = '" + goodsSku + "' and t.task_status <= 2";
                    DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            return 0;
                        }
                        return int.Parse(ds.Tables[0].Rows[0]["count(*)"].ToString());
                    }
                    else
                        return 0;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("获取托盘组编号出错：" + ex.Message);
                    return 0;
                }
            }

        }
        #endregion

        #region 获取横梁缓存区库位信息
        /// <summary>
        /// 获取横梁缓存区库位信息
        /// </summary>
        /// <param name="usingStatus">0-无货库位 1-有货库位</param>
        /// <returns>找到返回符合条件的库位 否则返回null</returns>
        public static string SelectAGVLocation(MySqlConnection con)
        {
            if (con == null)
            {
                return null;
            }
            try
            {
                string sql = @"select w.* from td_agv_warehouse_location w where w.use_status = 0 and w.unit_status = 0 order by w.level_no asc,w.column_no asc limit 1";
                DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count > 0)
                        return ds.Tables[0].Rows[0]["location_no"].ToString();
                    else
                        return null;
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                MessageBox.Show("通过任务号查询箱号出错：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 生成AGV小车任务
        /// <summary>
        /// 生成AGV小车任务
        /// </summary>
        /// <param name="boxCode">托盘号</param>
        /// <param name="taskType">任务类型 1-空吨桶入横梁缓存区 2-空吨桶出横梁缓存区 3-吨桶入取液处</param>
        /// <param name="fromUnit"></param>
        /// <param name="toUnit"></param>
        /// <param name="rs_ret"></param>
        /// <returns></returns>
        public static int AGVTaskCreate(MySqlConnection con, string boxCode, int taskType, string fromUnit, string toUnit, string taskM, out string rs_ret)
        {
            //using (MySqlConnection con = dbConn.GetConnectFromPool())
            //{
            if (con == null)
            {
                rs_ret = "数据库连接已用完！";
                return -1;
            }
            try
            {
                rs_ret = string.Empty;
                MySqlParameter[] createInboundTaskMParameter = new MySqlParameter[]
                {
                    new MySqlParameter("ls_box_code",MySqlDbType.VarChar,30),
                    new MySqlParameter("ls_from_unit",MySqlDbType.VarChar,20),
                    new MySqlParameter("ls_to_unit",MySqlDbType.VarChar,20),
                    new MySqlParameter("li_task_type",MySqlDbType.Int64),
                    new MySqlParameter("li_task_m",MySqlDbType.Int64),
                    new MySqlParameter("ri_ret",MySqlDbType.Int64),
                    new MySqlParameter("rs_ret",MySqlDbType.VarChar,255),
                };
                createInboundTaskMParameter[0].Value = boxCode;
                createInboundTaskMParameter[1].Value = fromUnit;
                createInboundTaskMParameter[2].Value = toUnit;
                createInboundTaskMParameter[3].Value = taskType;
                createInboundTaskMParameter[4].Value = int.Parse(taskM);
                createInboundTaskMParameter[5].Direction = ParameterDirection.Output;
                createInboundTaskMParameter[6].Direction = ParameterDirection.Output;
                MySqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "SP_AgvTaskToRoute", createInboundTaskMParameter);
                rs_ret = createInboundTaskMParameter[6].Value.ToString();
                return System.Convert.ToInt32(createInboundTaskMParameter[5].Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show("AGV生成入库主任务存储过程出错：" + ex.Message);
                rs_ret = "AGV生成入库主任务存储过程出错：" + ex.Message;
                return -1;
            }
            //}

        }
        #endregion

        #region 更新AGV小车任务状态
        /// <summary>
        /// 更新AGV小车任务状态
        /// </summary>
        /// <param name="taskID">任务号</param>
        /// <param name="taskStatus">任务状态 1-开始任务 4-允许取货 7-确认取货完成 10-允许卸货 13-作业完成</param>
        /// <param name="rs_ret"></param>
        /// <returns></returns>
        public static int AGVTaskUpdate(MySqlConnection con, int taskID, int taskStatus, out string rs_ret)
        {
            if (con == null)
            {
                rs_ret = "数据库连接已用完！";
                return -1;
            }
            try
            {
                rs_ret = string.Empty;
                MySqlParameter[] updateAgvTaskParameter = new MySqlParameter[]
                {
                new MySqlParameter("li_task_id",MySqlDbType.Int64),
                new MySqlParameter("li_task_status",MySqlDbType.Int64),
                new MySqlParameter("ri_ret",MySqlDbType.Int64),
                new MySqlParameter("rs_ret",MySqlDbType.VarChar,255),
                };
                updateAgvTaskParameter[0].Value = taskID;
                updateAgvTaskParameter[1].Value = taskStatus;
                updateAgvTaskParameter[2].Direction = ParameterDirection.Output;
                updateAgvTaskParameter[3].Direction = ParameterDirection.Output;
                MySqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "SP_UpdateAgvTaskStatus", updateAgvTaskParameter);
                rs_ret = updateAgvTaskParameter[3].Value.ToString();
                return System.Convert.ToInt32(updateAgvTaskParameter[2].Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show("AGV任务状态更新存储过程出错：" + ex.Message);
                rs_ret = "AGV任务状态更新存储过程出错：" + ex.Message;
                return -1;
            }
        }
        #endregion

        #region 更新任务状态
        /// <summary>
        /// 更新任务状态
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <param name="deviceID">设备编号</param>
        /// <param name="deviceName">设备名称</param>
        /// <param name="step">正在执行步骤</param>
        /// <param name="taskStatus">需要修改成的任务状态</param>
        /// <param name="rs_ret">存储过程运行报错</param>
        /// <returns>存储过程报错代码</returns>
        public static int TaskStatusUpdate(int taskID, int deviceID, string deviceName, int step, int taskStatus, out string rs_ret)
        {
            //li_task_id     in number, --任务号 
            //                    li_device_seq  in number, --设备编序号
            //                    ls_device_name in varchar2, --设备名称
            //                    li_step        in number, --步骤
            //                    li_task_status in number, --任务状态
            //                    ri_ret         out number, --返回错误类型
            //                    rs_ret         out varchar2);
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    rs_ret = "数据库连接已用完！";
                    return -1;
                }
                try
                {
                    rs_ret = string.Empty;
                    MySqlParameter[] updateTaskParameter = new MySqlParameter[]
                    {
                    new MySqlParameter("li_task_id",MySqlDbType.Int64),
                    new MySqlParameter("li_device_seq",MySqlDbType.Int64),
                    new MySqlParameter("ls_device_name",MySqlDbType.VarChar,20),
                    new MySqlParameter("li_step",MySqlDbType.Int64),
                    new MySqlParameter("li_task_status",MySqlDbType.Int64),
                    new MySqlParameter("ri_ret",MySqlDbType.Int64),
                    new MySqlParameter("rs_ret",MySqlDbType.VarChar,255),
                    };
                    updateTaskParameter[0].Value = taskID;
                    updateTaskParameter[1].Value = deviceID;
                    updateTaskParameter[2].Value = deviceName;
                    updateTaskParameter[3].Value = step;
                    updateTaskParameter[4].Value = taskStatus;
                    updateTaskParameter[5].Direction = ParameterDirection.Output;
                    updateTaskParameter[6].Direction = ParameterDirection.Output;
                    MySqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "SP_UpdateTaskStatus", updateTaskParameter);
                    rs_ret = updateTaskParameter[6].Value.ToString();
                    return System.Convert.ToInt32(updateTaskParameter[5].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("任务状态更新存储过程出错：" + ex.Message);
                    rs_ret = "任务状态更新存储过程出错：" + ex.Message;
                    return -1;
                }
            }

        }
        #endregion

        #region 更新任务状态
        /// <summary>
        /// 更新任务状态
        /// </summary>
        /// <param name="taskID">任务ID</param>
        /// <param name="deviceID">设备编号</param>
        /// <param name="deviceName">设备名称</param>
        /// <param name="step">正在执行步骤</param>
        /// <param name="taskStatus">需要修改成的任务状态</param>
        /// <param name="rs_ret">存储过程运行报错</param>
        /// <returns>存储过程报错代码</returns>
        public static int taskupdateyichang(int taskID, int deviceID, string deviceName, int step, int taskStatus, out string rs_ret)
        {
            //li_task_id     in number, --任务号 
            //                    li_device_seq  in number, --设备编序号
            //                    ls_device_name in varchar2, --设备名称
            //                    li_step        in number, --步骤
            //                    li_task_status in number, --任务状态
            //                    ri_ret         out number, --返回错误类型
            //                    rs_ret         out varchar2);
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    rs_ret = "数据库连接已用完！";
                    return -1;
                }
                try
                {
                    rs_ret = string.Empty;
                    MySqlParameter[] updateTaskParameter1 = new MySqlParameter[]
                    {
                    new MySqlParameter("li_task_id",MySqlDbType.Int64),
                    new MySqlParameter("li_device_seq",MySqlDbType.Int64),
                    new MySqlParameter("ls_device_name",MySqlDbType.VarChar,20),
                    new MySqlParameter("li_step",MySqlDbType.Int64),
                    new MySqlParameter("li_task_status",MySqlDbType.Int64),
                    new MySqlParameter("ri_ret",MySqlDbType.Int64),
                    new MySqlParameter("rs_ret",MySqlDbType.VarChar,255),
                    };
                    updateTaskParameter1[0].Value = taskID;
                    updateTaskParameter1[1].Value = deviceID;
                    updateTaskParameter1[2].Value = deviceName;
                    updateTaskParameter1[3].Value = step;
                    updateTaskParameter1[4].Value = taskStatus;
                    updateTaskParameter1[5].Direction = ParameterDirection.Output;
                    updateTaskParameter1[6].Direction = ParameterDirection.Output;
                    MySqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "sp_upkong", updateTaskParameter1);
                    rs_ret = updateTaskParameter1[6].Value.ToString();
                    return System.Convert.ToInt32(updateTaskParameter1[5].Value);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("任务状态更新存储过程出错：" + ex.Message);
                    rs_ret = "任务状态更新存储过程出错：" + ex.Message;
                    return -1;
                }
            }

        }
        #endregion
        #region 记录设备报警信息
        ////ls_device_name in varchar2, --设备编号
        //                       li_level       in number, --设备所在层
        //                       li_row         in number, --设备所在列
        //                       ls_device_type in number, --设备类型 CON-辊道 STACK-堆垛机 
        //                       li_error_code  in number, --设备报错代码
        //                       ls_error_des   in varchar2, --设备错误信息
        //                       li_task_id     in number, --设备错误所搭载的任务号
        //                       ri_ret         out number, --异常代码
        //                       rs_ret         out varchar2) as

        public static void DeviceErrorMessage(string deviceName, int level, int row, string deviceType, int errorCode, string errorDes, int taskId)
        {
            string rs_ret = string.Empty;
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                if (con == null)
                    return;
                try
                {
                    MySqlParameter[] insertDeviceErrorParameter = new MySqlParameter[]
                    {
                    new MySqlParameter("ls_device_name",MySqlDbType.VarChar,20),
                    new MySqlParameter("li_level",MySqlDbType.Int64),
                    new MySqlParameter("li_row",MySqlDbType.Int64),
                    new MySqlParameter("ls_device_type",MySqlDbType.VarChar,10),
                    new MySqlParameter("li_error_code",MySqlDbType.Int64),
                    new MySqlParameter("ls_error_des",MySqlDbType.VarChar,255),
                    new MySqlParameter("li_task_id",MySqlDbType.Int64),
                    new MySqlParameter("ri_ret",MySqlDbType.Int64),
                    new MySqlParameter("rs_ret",MySqlDbType.VarChar,255),
                    };
                    insertDeviceErrorParameter[0].Value = deviceName;
                    insertDeviceErrorParameter[1].Value = level;
                    insertDeviceErrorParameter[2].Value = row;
                    insertDeviceErrorParameter[3].Value = deviceType;
                    insertDeviceErrorParameter[4].Value = errorCode;
                    insertDeviceErrorParameter[5].Value = errorDes;
                    insertDeviceErrorParameter[6].Value = taskId;
                    insertDeviceErrorParameter[7].Direction = ParameterDirection.Output;
                    insertDeviceErrorParameter[8].Direction = ParameterDirection.Output;
                    MySqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "sp_devicestatuslog", insertDeviceErrorParameter);
                    rs_ret = insertDeviceErrorParameter[8].Value.ToString();
                    if (insertDeviceErrorParameter[7].Value.ToString() == "-1")
                        MessageBox.Show("记录设备存储过程数据库报错：" + rs_ret);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("记录设备存储过程数据库报错：" + ex.Message);
                    rs_ret = "记录设备存储过程数据库报错：" + ex.Message;
                }

            }


        }
        #endregion

        #region 创建出库任务
        //ls_box_code   in varchar2,
        //                       ls_message_no in varchar2,
        //                       li_task_type  in number,
        //                       ri_ret        out number,
        //                       rs_ret        out varchar2) as

        public static int CreateOutBoundTask(MySqlConnection con, string boxCode, string deviceId, int taskType, int dealWay, out string rs_ret)
        {

            if (con == null)
            {
                rs_ret = "数据库连接已用完！";
                return 0;
            }
            try
            {
                rs_ret = string.Empty;
                MySqlParameter[] insertDeviceErrorParameter = new MySqlParameter[]
                {
                new MySqlParameter("ls_box_code",MySqlDbType.VarChar,30),
                new MySqlParameter("ls_device_id",MySqlDbType.VarChar,255),
                new MySqlParameter("li_task_type",MySqlDbType.Int64),
                new MySqlParameter("li_dealway",MySqlDbType.Int64),
                new MySqlParameter("ri_ret",MySqlDbType.Int64),
                new MySqlParameter("rs_ret",MySqlDbType.VarChar,255),
                };
                insertDeviceErrorParameter[0].Value = boxCode;
                insertDeviceErrorParameter[1].Value = deviceId;
                insertDeviceErrorParameter[2].Value = taskType;
                insertDeviceErrorParameter[3].Value = dealWay;
                insertDeviceErrorParameter[4].Direction = ParameterDirection.Output;
                insertDeviceErrorParameter[5].Direction = ParameterDirection.Output;
                MySqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "SP_Out_TaskToRoute", insertDeviceErrorParameter);
                rs_ret = insertDeviceErrorParameter[5].Value.ToString();
                return int.Parse(insertDeviceErrorParameter[4].Value.ToString());
            }
            catch (Exception ex)
            {
                MessageBox.Show("生成入库主任务存储过程出错：" + ex.Message);
                rs_ret = "生成入库主任务存储过程出错：" + ex.Message;
                return 0;
            }


        }
        #endregion

        #region 查询库中的空托盘组编号
        public static string SelectTray(MySqlConnection con)
        {
            if (con == null)
            {
                return null;
            }
            try
            {
                string sqlstr = @"select t.* from td_plt_location_dic t where t.unit_status = 1 and t.goods_kinds = 3 and t.goods_sku = '000000' order by t.goods_weight";
                DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        return null;
                    }
                    return ds.Tables[0].Rows[0]["box_barcode"].ToString();
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                MessageBox.Show("获取托盘组编号出错：" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 查询库中已有托盘编号
        public static int SelectTrayCount(string boxCode)
        {
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                if (con == null)
                    return 1;
                try
                {
                    string sqlstr = @"select count(*) from ((select count(*) from td_plt_location_dic t where t.box_barcode = '') 
                      union 
                      (select count(*) from td_tray_group g,td_plt_location_dic d where g.tray_group_id = d.box_barcode and d.goods_kinds = 3 and g.box_code = ''))";
                    DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows[0]["count(*)"].ToString() == "0")
                            return 0;
                        return 1;
                    }
                    else
                        return 1;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("获取托盘组编号出错：" + ex.Message);
                    return 1;
                }
            }

        }
        #endregion

        #region 查询是否存在未完成的吨桶至抽液处任务
        public static int HavingAGVTask(MySqlConnection con, string boxCode)
        {
            //using (MySqlConnection con = dbConn.GetConnectFromPool())
            //{
            if (con == null)
            {
                return 0;
            }
            try
            {
                // string sqlstr = @"select t.* from td_agv_task t where t.box_code = '" + boxCode + "' and t.task_status < 2";
                string sqlstr = @"select t.* from td_agv_task t where  t.task_status < 2";
                DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
                if (ds != null)
                {
                    return ds.Tables[0].Rows.Count;
                }
                else
                    return 0;

            }
            catch (Exception ex)
            {
                MessageBox.Show("获取托盘组编号出错：" + ex.Message);
                return 0;
            }
            //}

        }
        #endregion

        #region 插入初始化操作日志
        /// <summary>
        /// 插入初始化操作日志
        /// </summary>
        /// <param name="row"></param>
        /// <param name="level"></param>
        /// <param name="type"></param>
        public static void InsertInitLog(string nDeviceName, int Task_id, int nFrom, int nTo)
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                try
                {
                    string strSQL = "insert into td_init_hist(DEIVCE_NAME,INIT_TIME,TASK_ID,FROM_ADDR,TO_ADDR) values('" + nDeviceName + "',sysdate(),'" + Task_id + "','" + nFrom + "'," + nTo + ")";

                    if (MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL) <= 0)
                        return;
                }
                catch
                {
                    return;
                }
            }

        }
        #endregion

        #region 判断是否是纯数字
        public static bool isPureNum(string temp)
        {
            for (int i = 0; i < temp.Length; i++)
            {
                byte tempByte = Convert.ToByte(temp[i]);
                if ((tempByte < 48) || (tempByte > 57))
                    return false;
            }
            return true;
        }
        #endregion

        #region 清除RFID已扫条码
        /// <summary>
        /// 插入初始化操作日志
        /// </summary>
        /// <param name="nScannid">扫码器编号</param>
        public static void UpdateRFIDStatus(string nScannid)
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                try
                {
                    string strSQL = "update TB_PLT_SCAN_RECORD t set t.status = 1,t.scan_desc = '外形检测退回',t.scan_time = sysdate where t.scanner_id = (select s.scanner_id from td_plt_scan_dic s where s.device_id = " + nScannid + ") and t.status = 0";

                    if (MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL) <= 0)
                        return;
                }
                catch
                {
                    return;
                }
            }

        }
        #endregion

        #region 存储RFID标识
        //ls_box_code  in varchar2, --箱号
        //                   li_task_type in number, --任务类型
        //                   li_level     in number, --扫码器所在层
        //                   li_device_id in number, --扫码器编号
        //                   ri_ret       out number,
        //                   rs_ret       out varchar2
        public static void SaveCurrentBarcode(string boxCode, int level, int scannID, int saveType, out string rs)
        {
            rs = ".";
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    return;
                }
                try
                {
                    MySqlParameter[] createInboundTaskMParameter = new MySqlParameter[]
                    {
                    new MySqlParameter("ls_box_code",MySqlDbType.VarChar,30),
                    new MySqlParameter("li_level",MySqlDbType.Int64),
                    new MySqlParameter("li_device_id",MySqlDbType.Int64),
                    new MySqlParameter("li_save_type",MySqlDbType.Int64),
                    new MySqlParameter("ri_ret",MySqlDbType.Int64),
                    new MySqlParameter("rs_ret",MySqlDbType.VarChar,255),
                    };
                    createInboundTaskMParameter[0].Value = boxCode;
                    createInboundTaskMParameter[1].Value = level;
                    createInboundTaskMParameter[2].Value = scannID;
                    createInboundTaskMParameter[3].Value = saveType;
                    createInboundTaskMParameter[4].Direction = ParameterDirection.Output;
                    createInboundTaskMParameter[5].Direction = ParameterDirection.Output;
                    MySqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "SP_SaveBarcode", createInboundTaskMParameter);
                    rs = createInboundTaskMParameter[5].Value.ToString();
                    if (createInboundTaskMParameter[4].Value.ToString() != "1")
                        MessageBox.Show(rs + boxCode);
                    else
                        rs = string.Empty;
                }
                catch (Exception ex)
                {
                    rs = ex.Message;
                    MessageBox.Show("存储托盘号存储过程出错：" + ex.Message);
                }
            }

        }


        public static void SaveCurrentBarcode(string boxCode1, string boxCode2, string boxCode3, string boxCode4, string boxCode5, string boxCode6, string boxCode7, string boxCode8, int level, int scannID, int saveType, out string rs)
        {
            //rs = ".";
            //using (MySqlConnection con = dbConn.GetConnectFromPool())
            //{
            //    if (con == null)
            //    {
            //        return;
            //    }
            //    try
            //    {
            //        MySqlParameter[] createInboundTaskMParameter = new MySqlParameter[]
            //        {
            //        new MySqlParameter("ls_boxcode_1",MySqlDbType.Varchar2,255),
            //        new MySqlParameter("ls_boxcode_2",MySqlDbType.Varchar2,255),
            //        new MySqlParameter("ls_boxcode_3",MySqlDbType.Varchar2,255),
            //        new MySqlParameter("ls_boxcode_4",MySqlDbType.Varchar2,255),
            //        new MySqlParameter("ls_boxcode_5",MySqlDbType.Varchar2,255),
            //        new MySqlParameter("ls_boxcode_6",MySqlDbType.Varchar2,255),
            //        new MySqlParameter("ls_boxcode_7",MySqlDbType.Varchar2,255),
            //        new MySqlParameter("ls_boxcode_8",MySqlDbType.Varchar2,255),
            //        new MySqlParameter("li_level",MySqlDbType.Int64),
            //        new MySqlParameter("li_device_id",MySqlDbType.Int64),
            //        new MySqlParameter("li_save_type",MySqlDbType.Int64),
            //        new MySqlParameter("ri_ret",MySqlDbType.Int64),
            //        new MySqlParameter("rs_ret",MySqlDbType.Varchar2,255),
            //        };
            //        createInboundTaskMParameter[0].Value = boxCode1;
            //        createInboundTaskMParameter[1].Value = boxCode2;
            //        createInboundTaskMParameter[2].Value = boxCode3;
            //        createInboundTaskMParameter[3].Value = boxCode4;
            //        createInboundTaskMParameter[4].Value = boxCode5;
            //        createInboundTaskMParameter[5].Value = boxCode6;
            //        createInboundTaskMParameter[6].Value = boxCode7;
            //        createInboundTaskMParameter[7].Value = boxCode8;
            //        createInboundTaskMParameter[8].Value = level;
            //        createInboundTaskMParameter[9].Value = scannID;
            //        createInboundTaskMParameter[10].Value = saveType;
            //        createInboundTaskMParameter[11].Direction = ParameterDirection.Output;
            //        createInboundTaskMParameter[12].Direction = ParameterDirection.Output;
            //        MySqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "sppg_plt_wcs.SP_SaveNullTray", createInboundTaskMParameter);
            //        rs = createInboundTaskMParameter[12].Value.ToString();
            //        if (createInboundTaskMParameter[11].Value.ToString() != "1")
            //            MessageBox.Show(rs);
            //        else
            //            rs = string.Empty;
            //    }
            //    catch (Exception ex)
            //    {
            //        rs = ex.Message;
            //        MessageBox.Show("存储托盘号存储过程出错：" + ex.Message);
            //    }
            //}
            rs = "";

        }
        #endregion

        #region 获取堆垛机任务
        /// <summary>
        /// 获取堆垛机任务
        /// </summary>
        /// <param name="channelNo">所属巷道</param>
        /// <param name="taskId">任务号</param>
        /// <returns></returns>
        public static DataRow GetTempStackerTk(int channelNo, int taskId)
        {
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    return null;
                }
                try
                {
                    string sqlstr = " select * from tb_plt_task_d t where t.row_num = " + channelNo.ToString() + " and t.task_id = " + taskId.ToString() + " and t.device_type = 'STACK'";
                    DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
                    if (ds != null)
                    {
                        if (ds.Tables[0].Rows.Count == 1)
                            return ds.Tables[0].Rows[0];
                        else
                            return null;
                    }
                    else
                        return null;


                }
                catch (Exception ex)
                {
                    MessageBox.Show("获取主任务号出错：" + ex.Message);
                    return null;
                }
            }

        }
        #endregion

        #region 获取异常回库机任务
        /// <summary>
        /// 获取异常回库机任务
        /// </summary>
        /// <param name="channelNo">所属巷道</param>
        /// <param name="taskId">任务号</param>
        /// <returns></returns>
        public static DataRow GetErrorRebackTask(MySqlConnection con, int level, string strScnnerId)
        {
            string strFromUnit = string.Empty;
            //using (MySqlConnection con = dbConn.GetConnectFromPool())
            //{
            if (con == null)
            {
                return null;
            }
            try
            {
                if (level == 1)
                {
                    if (strScnnerId == "1")
                        strFromUnit = "1005";
                    else if (strScnnerId == "2")
                        strFromUnit = "1010";
                    else if (strScnnerId == "3")
                        strFromUnit = "1014";
                    else if (strScnnerId == "4")
                        strFromUnit = "1018";
                    string sqlstr = " select t.*,a.* from TB_PLT_TASK_D t,tb_plt_task_m a where a.task_id = t.task_id and t.status = 0 and (t.step = 3 OR T.step=1) and t.task_type = 5 and t.from_unit = '" + strFromUnit + "' ";
                    DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
                    if (ds != null)
                        return ds.Tables[0].Rows[0];
                    else
                        return null;
                }
                else if (level == 2)
                {
                    string sqlstr = " select t.*,a.* from TB_PLT_TASK_D t,tb_plt_task_m a where a.task_id = t.task_id and t.status = 0 and t.step = 1 and t.task_type = 5 and t.from_unit = '1003' ";
                    DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
                    if (ds != null)
                        return ds.Tables[0].Rows[0];
                    else
                        return null;
                }
                return null;

            }
            catch (Exception ex)
            {
                MessageBox.Show("获取主任务号出错：" + ex.Message);
                return null;
            }
            //}

        }
        #endregion
        #region 清除空吨桶缓存区的空吨桶
        /// <summary>
        /// 清除空吨桶缓存区的空吨桶
        /// </summary>
        /// <param name="nScannid">扫码器编号</param>
        public static void UpdateAgvLocation(string locId)
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                try
                {
                    string strSQL = "update td_agv_warehouse_location t set t.box_code=null , t.unit_status=0 where t.location_no = '" + locId + "'";
                    if (MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL) <= 0)
                        return;
                }
                catch
                {
                    return;
                }
            }

        }
        /// <summary>
        /// 清除空吨桶缓存区的空吨桶
        /// </summary>
        /// <param name="nScannid">扫码器编号</param>
        public static void UpdateAgvLocation()
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                try
                {
                    string strSQL = "update td_agv_warehouse_location t set t.box_code=null , t.unit_status=0 ";
                    if (MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL) <= 0)
                        return;
                }
                catch
                {
                    return;
                }
            }

        }
        #endregion

        #region 获取AGV小车任务状态
        /// <summary>
        /// 获取AGV小车任务状态
        /// </summary>
        /// <param name="channelNo">所属巷道</param>
        /// <param name="taskId">任务号</param>
        /// <returns></returns>
        public static DataRow GetAgvTaskStatus(MySqlConnection con)
        {
            if (con == null)
            {
                return null;
            }
            try
            {
                string sqlstr = " select t.*,a.task_type,a.box_code,a.TASK_ID,a.task_status from  tb_agvtask t,td_agv_task a where t.taskid = a.task_id and t.taskstatus < 13 and a.task_status < 2 order by t.producetime,t.taskstatus desc,t.priority desc";
                DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                        return null;
                    return ds.Tables[0].Rows[0];
                }
                else
                    return null;

            }
            catch (Exception ex)
            {
                MessageBox.Show("获取AGV执行任务状态出错：" + ex.Message);
                return null;
            }
        }
        public static DataSet GetAgvTask(MySqlConnection con)
        {
            if (con == null)
                return null;
            try
            {
                string sqlstr = " select t.*,a.task_type,a.box_code,a.TASK_ID,a.task_status from  tb_agvtask t,td_agv_task a where t.taskid = a.task_id and t.taskstatus < 13 and a.task_status < 2 order by t.producetime,t.taskstatus desc,t.priority desc";
                DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sqlstr);
                return ds;
            }
            catch (Exception ex)
            {
                MessageBox.Show("获取AGV任务出错" + ex.Message);
                return null;
            }
        }
        #endregion

        #region 获取横梁缓存区库位状态
        /// <summary>
        /// 获取横梁缓存区库位状态
        /// </summary>
        /// <param name="locNo">库位</param>
        public static DataRow AgvLocationStatus(MySqlConnection conn, string locNo)
        {
            if (conn == null)
                return null;
            try
            {
                string strSQL = "select t.* from td_agv_warehouse_location t where t.location_no = '" + locNo + "' ";

                DataSet ds = MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                        return null;
                    return ds.Tables[0].Rows[0];
                }
                else
                    return null;

            }
            catch
            {
                return null;
            }
        }
        #endregion

        #region 获取取液处箱号
        public static string GetPumpingBoxcode(MySqlConnection conn, string device_id)
        {
            if (conn == null)
                return null;
            try
            {
                string strSQL = "select boxcode from td_plt_pumping where device_id= '" + device_id + "' and device_type= 1 and length(boxcode)>0";
                DataSet ds = MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                if (ds != null)
                {
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        // MessageBox.Show("未找到抽液处" + device_id + "对应箱号");
                        return null;
                    }
                    return ds.Tables[0].Rows[0]["boxcode"].ToString();
                }
                else
                    return null;

            }
            catch
            {
                return null;
            }
        }
        #endregion



        #region 更新取液处箱号
        /// update TB_PLT_SCAN_RECORD t set t.status = 3 where t.status <> 1 and t.scanner_id = '" + scanID + "'
        public static int UpdatePumpingBoxcode(string device_id, string boxcode)
        {
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    return 0;
                }
                try
                {
                    string sql = "update td_plt_pumping t set t.boxcode='" + boxcode + "' where device_id='" + device_id + "'";
                    return MySqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);

                }
                catch (Exception ex)
                {
                    MessageBox.Show("修改抽液处箱号失败：" + ex.Message);
                    return -1;
                }
            }

        }
        #endregion



        #region 更改库位
        //ls_box_code   in varchar2,
        //                       ls_message_no in varchar2,
        //                       li_task_type  in number,
        //                       ri_ret        out number,
        //                       rs_ret        out varchar2) as

        public static int ChangeLocation(string oldLocation, string newLocation, string locationType, out string rs_ret)
        {
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    rs_ret = "数据库连接已用完！";
                    return 0;
                }
                try
                {
                    rs_ret = string.Empty;
                    MySqlParameter[] insertDeviceErrorParameter = new MySqlParameter[]
                    {
                    new MySqlParameter("ls_old_location",MySqlDbType.VarChar,12),
                    new MySqlParameter("ls_new_location",MySqlDbType.VarChar,12),
                    new MySqlParameter("ls_location_type",MySqlDbType.VarChar,20),
                    new MySqlParameter("ri_ret",MySqlDbType.Int64),
                    new MySqlParameter("rs_ret",MySqlDbType.VarChar,255),
                    };
                    insertDeviceErrorParameter[0].Value = oldLocation;
                    insertDeviceErrorParameter[1].Value = newLocation;
                    insertDeviceErrorParameter[2].Value = locationType;
                    insertDeviceErrorParameter[3].Direction = ParameterDirection.Output;
                    insertDeviceErrorParameter[4].Direction = ParameterDirection.Output;
                    MySqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "SP_UpdateLocation", insertDeviceErrorParameter);
                    rs_ret = insertDeviceErrorParameter[4].Value.ToString();
                    return int.Parse(insertDeviceErrorParameter[3].Value.ToString());
                }
                catch (Exception ex)
                {
                    MessageBox.Show("更改库位存储过程出错：" + ex.Message);
                    rs_ret = "更改库位存储过程出错：" + ex.Message;
                    return 0;
                }
            }

        }
        #endregion

        #region 生成异常回库任务
        //ls_box_code   in varchar2,
        //                       ls_message_no in varchar2,
        //                       li_task_type  in number,
        //                       ri_ret        out number,
        //                       rs_ret        out varchar2) as

        public static string ErrorRebackTaskCreate(MySqlConnection con, string trueBoxcode, int taskIDM, int level, int reback, out string rs_ret)
        {
            //using (MySqlConnection con = dbConn.GetConnectFromPool())
            //{
            if (con == null)
            {
                rs_ret = "数据库连接已用完！";
                return null;
            }
            try
            {
                rs_ret = string.Empty;
                MySqlParameter[] insertDeviceErrorParameter = new MySqlParameter[]
                {
                    new MySqlParameter("ls_true_box_code",MySqlDbType.VarChar,30),
                    new MySqlParameter("li_task_m",MySqlDbType.Int64),
                    new MySqlParameter("li_level",MySqlDbType.Int64),
                    new MySqlParameter("li_reback",MySqlDbType.Int64),
                    new MySqlParameter("rs_ret",MySqlDbType.VarChar,255),
                    new MySqlParameter("ri_ret",MySqlDbType.Int64),
                };
                insertDeviceErrorParameter[0].Value = trueBoxcode;
                insertDeviceErrorParameter[1].Value = taskIDM;
                insertDeviceErrorParameter[2].Value = level;
                insertDeviceErrorParameter[3].Value = reback;
                insertDeviceErrorParameter[4].Direction = ParameterDirection.Output;
                insertDeviceErrorParameter[5].Direction = ParameterDirection.Output;
                MySqlHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "SP_ErrorInTaskToRoute", insertDeviceErrorParameter);
                rs_ret = insertDeviceErrorParameter[4].Value.ToString();
                if (int.Parse(insertDeviceErrorParameter[5].Value.ToString()) == -1)
                    MessageBox.Show("生成异常回库主任务存储过程报错：" + rs_ret);
                return insertDeviceErrorParameter[5].Value.ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show("生成异常回库主任务存储过程程序报错：" + ex.Message);
                rs_ret = "生成异常回库主任务存储过程程序报错：" + ex.Message;
                return "";
            }
            //}

        }
        #endregion

        #region 取消入库任务
        public static string TaskCancel(int taskId, out string rs_ret)
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                {
                    rs_ret = "未建立数据库连接";
                    return null;
                }
                try
                {
                    rs_ret = string.Empty;
                    MySqlParameter[] taskCancelParameter = new MySqlParameter[]
                   {
                    new MySqlParameter("li_task_id",MySqlDbType.Int64),
                    new MySqlParameter("ri_ret",MySqlDbType.Int64),
                    new MySqlParameter("rs_ret",MySqlDbType.VarChar,255),

                   };
                    taskCancelParameter[0].Value = taskId;
                    taskCancelParameter[1].Direction = ParameterDirection.Output;
                    taskCancelParameter[2].Direction = ParameterDirection.Output;
                    MySqlHelper.ExecuteNonQuery(conn, CommandType.StoredProcedure, "SP_TaskCancel", taskCancelParameter);
                    rs_ret = taskCancelParameter[2].Value.ToString();
                    if (int.Parse(taskCancelParameter[1].Value.ToString()) == -1)
                        MessageBox.Show("任务取消存储过程程序报错：" + rs_ret);
                    return taskCancelParameter[1].Value.ToString();
                }
                catch (Exception ex)
                {
                    rs_ret = "任务取消存储过程程序报错：" + ex.Message;
                    return "";
                }
            }
        }
        #endregion

        #region 获取设备字典
        private static object lockGetDic = new object();
        public static DataSet GetDic(MySqlConnection conn, string strFields, string strTalbeName, string strConditon, string strOrderBy)
        {
            lock (lockGetDic)
            {
                DataSet ds = new DataSet();
                try
                {
                    string strSQL = "select " + strFields + " from " + strTalbeName + " where 1=1 " + strConditon + " order by " + strOrderBy;
                    ds = MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                }
                catch (Exception ex)
                {
                    return null;
                }
                return ds;
            }

        }
        #endregion

        #region 写空托补入库口任务

        public static void InsertTaskM(MySqlConnection conn, int nTaskId, int nTaskType, string fromUnit, string toUnit, int goodsKind, int taskLevel)
        {
            if (conn == null)
                return;
            try
            {
                string strSQL = "insert into tb_plt_task_m(task_id,task_type,from_unit,to_unit,task_status,begin_time,create_time,goods_kind,task_level) values(" + nTaskId + "," + nTaskType + ",'" + fromUnit + "','" + toUnit + "',1,sysdate(),sysdate()," + goodsKind + "," + taskLevel + ")";
                MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL);
            }
            catch
            {
                return;
            }
        }
        #endregion

        #region 获取执行中出库任务数量
        public static int GetOutTaskNum()
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return -1;
                try
                {
                    string strSQL = "select count(1) from tb_plt_task_m t where t.task_type=2 and t.goods_kind=2 and t.task_status<2 and ifnull(t.is_out,0)=0";
                    DataSet ds = MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                    if (ds != null)
                    {
                        return int.Parse(ds.Tables[0].Rows[0]["count(1)"].ToString());
                    }
                    else
                        return -1;

                }
                catch (Exception)
                {
                    return -1;

                }
            }
        }
        #endregion

        #region 是否存在空托回库任务
        public static bool getEmptyPalletTask()
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return false;
                try
                {
                    string strSQL = "select count(1) from tb_plt_task_m t where t.task_type=3 and t.task_level=2 and t.task_status<2";
                    DataSet ds = MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                    if (ds != null)
                    {
                        if (int.Parse(ds.Tables[0].Rows[0]["count(1)"].ToString()) > 0)
                            return true;
                        else
                            return false;
                    }
                    else
                        return false;

                }
                catch (Exception)
                {
                    return false;
                }
            }
        }
        #endregion

        #region 修改出库任务是否经过叠盘机前输送段
        public static int UpdateTaskIsOut(int nTaskId)
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return 0;
                try
                {
                    string strSQL = "update tb_plt_task_m t set t.is_out=1 where t.task_type=2 and t.task_id=" + nTaskId;
                    int nReturn = MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL);
                    return nReturn;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
        #endregion

        #region AGV停止
        /// <summary>
        /// AGV停止
        /// </summary>
        /// <param name="nScannid">扫码器编号</param>
        public static void AGVstop(MySqlConnection conn, string status)
        {
            if (conn == null)
                return;
            try
            {
                string strSQL = "update td_system_agv_status t set t.status_id = " + status;
                if (MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL) <= 0)
                    return;
            }
            catch
            {
                return;
            }

        }
        #endregion

        #region 查询AGV设备状态
        public static DataTable AGVstatus()
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return null;
                try
                {
                    string sqlstr = @"select t.* from td_agv_status t";
                    DataSet ds = MySqlHelper.ExecuteDataset(conn, CommandType.Text, sqlstr);
                    if (ds.Tables[0].Rows.Count != 0)
                        return ds.Tables[0];
                    else
                        return null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show("获取托盘组编号出错：" + ex.Message);
                    return null;
                }
            }

        }
        #endregion
        #region 修改空托盘去入库口报完成
        public static int UpdateTaskIsOut2(int nTaskId)
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return 0;
                try
                {
                    string strSQL = "update tb_plt_task_m t set t.task_status=2 where t.task_type=6 and t.task_id=" + nTaskId;
                    int nReturn = MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL);
                    return nReturn;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
        #endregion
        #region 外形检测异常报新生成
        public static int UpdateTaskIsOut3(int nTaskId)
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return 0;
                try
                {
                    string strSQL = "update tb_plt_task_m t set t.task_status=2 where t.task_type=6 and t.task_id=" + nTaskId;
                    int nReturn = MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL);
                    return nReturn;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
        }
        #endregion

        #region 外形检测异常报新生成
        public static void Updateempty(int nTaskId)
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                try
                {
                    string strSQL = "update td_empty_status t set t.empty_status=" + nTaskId + " where t.id=1";
                    int nReturn = MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL);
                    if (nReturn == 1)
                    {
                        MessageBox.Show("修改成功");
                    }
                }
                catch (Exception ex)
                {

                }
            }
        }
        #endregion
        #region 外形检测异常报新生成
        public static string Updateemptystatus()
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return "";
                try
                {
                    string strSQL = "select empty_status from td_empty_status ";

                    DataSet ds = MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                    if (ds != null)
                    {
                        if (int.Parse(ds.Tables[0].Rows[0]["empty_status"].ToString()) == 1)
                        {
                            return ds.Tables[0].Rows[0]["empty_status"].ToString();
                        }

                        else
                            return ds.Tables[0].Rows[0]["empty_status"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    return "";
                }
                return "";
            }
        }
        #endregion




        public static int UpdateCounts()
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return 0;
                try
                {
                    string strSQL = "update td_empty_status t set t.empty_status=0 where t.id=2";
                    int nReturn = MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL);
                    if (nReturn == 1)
                    {
                        MessageBox.Show("修改成功");
                        return 1;
                    }
                }
                catch (Exception ex)
                {
                    return 0;
                }
                return 0;
            }
        }

        public static int Insertshoudong( string sku, string goodsname, string batchno, string batchid, string batchweight, string hazard, string maketime,string boxtype)
        {
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    return 0;
                }
                try
                {
                    string sql = "select * from tb_shipment t where t.WASTECODE='" + sku + "' and t.WASTENAME='" + goodsname + "' and t.VEHICLEORDERNO='" + batchno + "' and t. SHIPMENTDETAILID='" + batchid + "' and t.WASTEWEIGHT=" + batchweight + " and t.HAZARDAREA='" + hazard + "' and t.SHIPMENTDATE='" + maketime + "'";
                    MySqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
                    DataSet ds = MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                    if (ds.Tables[0].Rows.Count == 0)
                    {
                        sql = "insert into tb_shipment (WASTECODE,WASTENAME,VEHICLEORDERNO,SHIPMENTDETAILID,WASTEWEIGHT,HAZARDAREA,SHIPMENTDATE,VEHICLEORDERSTATE,boxtype)values('" + sku + "','" + goodsname + "','" + batchno + "','" + batchid + "'," + batchweight + ",'" + hazard + "','" + maketime + "','0','"+ boxtype + "');";
                        return MySqlHelper.ExecuteNonQuery(con, CommandType.Text, sql); ;
                    }
                    else
                        return 0;

                }
                catch (Exception ex)
                {
                    MessageBox.Show("修改批次状态报错：" + ex.Message);
                    return -1;
                }
            }

        }


        #region 

        public static void Inserttask(string outplannp)
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                try
                {
                    string strSQL = "insert into  tb_outstockplan (id,outplanno,status,down_date,tasktype)values(now(),'" + outplannp + "','0',now(),'0')";


                    if (MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL) <= 0)
                        return;
                }
                catch
                {
                    return;
                }
            }

        }
        #endregion
        #region 取消出库减叠盘统计数

        public static void cancleCount()
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                try
                {
                    string strsql = "select empty_status from td_empty_status t where id=2";
                    DataSet ds2 = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strsql);
                    int counts = int.Parse(ds2.Tables[0].Rows[0]["empty_status"].ToString());
                    int sum = counts - 1;
                    string strSQL = "update  td_empty_status t  set empty_status='"+ sum  +"' where id=2";

                    if (MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL) <= 0)
                        return;
                }
                catch
                {
                    return;
                }
            }

        }
        #endregion
    }

}
