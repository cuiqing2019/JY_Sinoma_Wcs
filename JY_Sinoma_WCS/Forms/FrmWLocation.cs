using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataBase;
using MySql.Data.MySqlClient;
using JY_Sinoma_WCS.Forms;

namespace JY_Sinoma_WCS
{
    public partial class FrmWLocation : Form
    {
        public frmMain mainFrm;
        public ConnectPool dbConn;//定义数据库连接
        string locationStatus;
        public FrmWLocation(frmMain mainFrm, string locationStatus)
       
        {
            this.locationStatus = locationStatus;
            InitializeComponent();
            this.mainFrm = mainFrm;
            dbConn = mainFrm.dbConn;
        }
        private void FrmWLocation_Load(object sender, EventArgs e)
        {
            if (locationStatus == "MainFrm")
            {
                lvLocation.Columns.Add("库位编码", (int)(lvLocation.Width * 0.1), HorizontalAlignment.Center);
                lvLocation.Columns.Add("库位类型", (int)(lvLocation.Width * 0.05), HorizontalAlignment.Center);
                lvLocation.Columns.Add("可用状态", (int)(lvLocation.Width * 0.05), HorizontalAlignment.Center);
                lvLocation.Columns.Add("实时状态", (int)(lvLocation.Width * 0.1), HorizontalAlignment.Center);
                lvLocation.Columns.Add("巷道", (int)(lvLocation.Width * 0.05), HorizontalAlignment.Center);
                lvLocation.Columns.Add("排", (int)(lvLocation.Width * 0.05), HorizontalAlignment.Center);
                lvLocation.Columns.Add("列", (int)(lvLocation.Width * 0.05), HorizontalAlignment.Center);
                lvLocation.Columns.Add("层", (int)(lvLocation.Width * 0.05), HorizontalAlignment.Center);
                lvLocation.Columns.Add("托盘条码", (int)(lvLocation.Width * 0.1), HorizontalAlignment.Center);
                lvLocation.Columns.Add("废品单位", (int)(lvLocation.Width * 0.1), HorizontalAlignment.Center);
                lvLocation.Columns.Add("废物名称", (int)(lvLocation.Width * 0.1), HorizontalAlignment.Center);
                lvLocation.Columns.Add("废物性状", (int)(lvLocation.Width * 0.1), HorizontalAlignment.Center);
                lvLocation.Columns.Add("入库时间", (int)(lvLocation.Width * 0.1), HorizontalAlignment.Center);
                lvLocation.Columns.Add("废物颜色", (int)(lvLocation.Width * 0.1), HorizontalAlignment.Center);
                lvLocation.Columns.Add("危险分区", (int)(lvLocation.Width * 0.1), HorizontalAlignment.Center);
                this.ContextMenuStrip = this.cmsStatus;
                aGVToolStripMenuItem.Enabled = false;
                tsmiChangeGoodsKind.Enabled = true;
                tsmiChangeHazardArea.Enabled = true;
                RefreshListViewAll(); 
            }
            if (locationStatus == "Waiting")
            {
                lvLocation.Columns.Add("库位编码", (int)(lvLocation.Width * 0.15), HorizontalAlignment.Center);
                lvLocation.Columns.Add("可用状态", (int)(lvLocation.Width * 0.05), HorizontalAlignment.Center);
                lvLocation.Columns.Add("实时状态", (int)(lvLocation.Width * 0.15), HorizontalAlignment.Center);
                lvLocation.Columns.Add("排", (int)(lvLocation.Width * 0.15), HorizontalAlignment.Center);
                lvLocation.Columns.Add("列", (int)(lvLocation.Width * 0.15), HorizontalAlignment.Center);
                lvLocation.Columns.Add("层", (int)(lvLocation.Width * 0.15), HorizontalAlignment.Center);
                lvLocation.Columns.Add("托盘条码", (int)(lvLocation.Width * 0.2), HorizontalAlignment.Center);
                this.ContextMenuStrip = this.cmsStatus;
                this.立库ToolStripMenuItem.Enabled = false;
                tsmiChangeGoodsKind.Enabled = false;
                tsmiChangeHazardArea.Enabled = false;
                btnLocatinView.Enabled=false;
                RefreshListViewAll();
            }
            comboBox1.SelectedIndex = 0;
            HWweight();
        }
        public void RefreshListView()
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                try
                {

                    #region 立库库存
                    if (locationStatus == "MainFrm")
                    {
                        lvLocation.BeginUpdate();
                        lvLocation.Items.Clear();
                        int i = 0;
                        string strSQL;
                        strSQL = "select * from TD_PLT_LOCATION_DIC t where 1 = 1  ";
                        if (tbLocation.Text.Trim().Length != 0)
                        {
                            strSQL += " and location_id ='" + tbLocation.Text.Trim().ToString() + "' ";
                        }
                        if (txtpallet.Text.Trim().Length != 0)
                        {
                            strSQL += " and t.box_barcode ='" + txtpallet.Text.Trim().ToString() + "' ";
                        }
                        if (checkBox1.Checked)
                        {
                            strSQL += " and (t.use_status > 0 or t.unit_status > 0 )  ";
                        }
                        if (txtBatchNo.Text.Trim().Length != 0)
                        {
                            strSQL += " and t.batch_id like'%" + txtBatchNo.Text.Trim() + "%'";
                        }
                        if (comboBox1.Text == "有货")
                        {
                            strSQL += " and t.UNIT_STATUS='1'";
                        }
                        if (comboBox1.Text == "无货")
                        {
                            strSQL += " and t.UNIT_STATUS='0'";
                        }
                            strSQL += "order by t.row_no,t.level_no,t.bay_no";
                            DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                string[] items = new string[lvLocation.Columns.Count];
                                items[0] = row["location_id"].ToString();
                                items[1] = DecodeStoreType(row["GOODS_KINDS"].ToString());
                                items[2] = DecodeUseStatus(row["USE_STATUS"].ToString());
                                items[3] = DecodeUnitStatus(row["UNIT_STATUS"].ToString());
                                items[4] = row["CHANNEL_NO"].ToString();
                                items[5] = row["ROW_NO"].ToString();
                                items[6] = row["BAY_NO"].ToString();
                                items[7] = row["LEVEL_NO"].ToString();
                                items[8] = row["BOX_BARCODE"].ToString();
                                items[9] = row["BATCH_NO"].ToString(); ;
                                items[10] = row["BATCH_ID"].ToString(); ;
                                items[11] = row["GOODS_SKU"].ToString();
                                items[12] = row["INBOUND_TIME"].ToString();
                                items[13] = row["goods_name"].ToString();
                                items[14] = row["hazard_area"].ToString();
                                lvLocation.Items.Add(new ListViewItem(items));
                                if (i % 2 != 0)
                                    lvLocation.Items[i].BackColor = Color.FromArgb(229, 255, 229);
                                i++;
                                tb_count_storage_location_null.Text = i.ToString();
                            }
                            lvLocation.EndUpdate();
                        }
                        #endregion
                        #region 缓存区库存
                        if (locationStatus == "Waiting")
                        {
                            lvLocation.BeginUpdate();
                            lvLocation.Items.Clear();
                            int i = 0;
                            string strSQL;
                            strSQL = "select * from TD_AGV_WAREHOUSE_LOCATION t where 1 = 1 ";
                            if (tbLocation.Text.Trim().Length != 0)
                            {
                                strSQL += " and location_no ='" + tbLocation.Text.Trim().ToString() + "' ";
                            }
                            if (txtpallet.Text.Trim().Length != 0)
                            {
                                strSQL += " and box_code ='" + txtpallet.Text.Trim().ToString() + "' ";
                            }
                            if (checkBox1.Checked)
                            {
                                strSQL += "and (t.use_status > 0 or t.UNIT_STATUS > 0)";
                            }
                            strSQL += "order by t.row_no,t.level_no,t.column_no";
                            DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                            foreach (DataRow row in ds.Tables[0].Rows)
                            {
                                string[] items = new string[lvLocation.Columns.Count];

                                items[0] = row["LOCATION_NO"].ToString();
                                items[1] = DecodeUseStatus(row["USE_STATUS"].ToString());
                                items[2] = DecodeUnitStatus(row["UNIT_STATUS"].ToString());
                                items[3] = row["ROW_NO"].ToString();
                                items[4] = row["COLUMN_NO"].ToString();
                                items[5] = row["LEVEL_NO"].ToString();
                                items[6] = row["BOX_CODE"].ToString();
                                lvLocation.Items.Add(new ListViewItem(items));
                                if (i % 2 != 0)
                                    lvLocation.Items[i].BackColor = Color.FromArgb(229, 255, 229);
                                i++;
                                tb_count_storage_location_null.Text = i.ToString();
                            }
                            lvLocation.EndUpdate();
                        }
                        #endregion
                    
                }
                catch (Exception)
                {
                    throw;
                }
            }
               
        }

        public void RefreshListViewAll()
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                try
                {
                    GetStatus();
                    #region 立库库存
                    if (locationStatus == "MainFrm")
                    {
                        lvLocation.BeginUpdate();
                        lvLocation.Items.Clear();
                        int i = 0;
                        string strSQL;
                        strSQL = "select * from TD_PLT_LOCATION_DIC t where 1 = 1  order by t.row_no,t.level_no,t.bay_no";

                        DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string[] items = new string[lvLocation.Columns.Count];
                            items[0] = row["location_id"].ToString();
                            items[1] = DecodeStoreType(row["GOODS_KINDS"].ToString());
                            items[2] = DecodeUseStatus(row["USE_STATUS"].ToString());
                            items[3] = DecodeUnitStatus(row["UNIT_STATUS"].ToString());
                            items[4] = row["CHANNEL_NO"].ToString();
                            items[5] = row["ROW_NO"].ToString();
                            items[6] = row["BAY_NO"].ToString();
                            items[7] = row["LEVEL_NO"].ToString();
                            items[8] = row["BOX_BARCODE"].ToString(); ;
                            items[9] = row["BATCH_NO"].ToString(); ;
                            items[10] = row["BATCH_ID"].ToString(); ;
                            items[11] = row["GOODS_SKU"].ToString();
                            items[12] = row["INBOUND_TIME"].ToString();
                            items[13] = row["goods_name"].ToString();
                            items[14] = row["hazard_area"].ToString();
                            lvLocation.Items.Add(new ListViewItem(items));
                            if (i % 2 != 0)
                                lvLocation.Items[i].BackColor = Color.FromArgb(229, 255, 229);
                            i++;
                            tb_count_storage_location_null.Text = i.ToString();
                        }
                        lvLocation.EndUpdate();
                    }
                    #endregion
                    #region 缓存区库存
                    if (locationStatus == "Waiting")
                    {

                        lvLocation.BeginUpdate();
                        lvLocation.Items.Clear();
                        int i = 0;
                        string strSQL;
                        strSQL = "select * from TD_AGV_WAREHOUSE_LOCATION t where 1 = 1 order by t.row_no,t.level_no,t.column_no";

                        DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            string[] items = new string[lvLocation.Columns.Count];


                            items[0] = row["LOCATION_NO"].ToString();
                            items[1] = DecodeUseStatus(row["USE_STATUS"].ToString());
                            items[2] = DecodeUnitStatus(row["UNIT_STATUS"].ToString());
                            items[3] = row["ROW_NO"].ToString();
                            items[4] = row["COLUMN_NO"].ToString();
                            items[5] = row["LEVEL_NO"].ToString();
                            items[6] = row["BOX_CODE"].ToString();

                            lvLocation.Items.Add(new ListViewItem(items));
                            if (i % 2 != 0)
                                lvLocation.Items[i].BackColor = Color.FromArgb(229, 255, 229);
                            i++;
                            tb_count_storage_location_null.Text = i.ToString();
                        }
                        lvLocation.EndUpdate();
                    }
                    #endregion
                }
                catch (Exception)
                {
                   
                    throw;
                }
            }
                
        }
        private string DecodeUseStatus(string strUseStatus)
        {
            switch (strUseStatus)
            {
                case "0":
                    return "可用";
                case "1":
                    return "不可出";
                case "2":
                    return "不可入";
                case "3":
                    return "不可出入";
                default:
                    return "未知";
              
            }
        }
        private string DecodeStoreType(string strType)
        {
            switch (strType)
            {
                case "0":
                    return "空货位";
                case "1":
                    return "吨桶";
                case "2":
                    return "圆桶";
                case "3":
                    return "空托盘组";
                default:
                    return "铁桶";
            }
        }
        private string DecodeUnitStatus(string strType)
        {
            switch (strType)
            {
                case "0":
                    return "无货";
                case "1":
                    return "有货";
                case "2":
                    return "入库占用";
                case "3":
                    return "出库占用";
                default:
                    return "未知";
            }
        }
        private void btRequery_Click(object sender, EventArgs e)
        {
            RefreshListView();
        }



        private void GetStatus() //查询可用库位，和已禁库位
        {
            if (locationStatus == "MainFrm")
            {
                using (MySqlConnection conn = dbConn.GetConnectFromPool())
                {
                    if (conn == null)
                        return;
                    try
                    {
                        String sql = "select count(*) from TD_PLT_LOCATION_DIC t where t.unit_status = 0 and t.use_status = 0";
                        DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
                        label7.Text = ds.Tables[0].Rows[0]["count(*)"].ToString();

                        sql = "select count(*) from TD_PLT_LOCATION_DIC t where t.use_status<>0";
                        ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
                        label8.Text = ds.Tables[0].Rows[0]["count(*)"].ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                    
            }
            if (locationStatus == "Waiting")
            {
                using (MySqlConnection conn = dbConn.GetConnectFromPool())
                {
                    if (conn == null)
                        return;
                    try
                    {
                        String sql = "select count(*) from TD_AGV_WAREHOUSE_LOCATION t where t.unit_status = 0 and t.use_status = 0";
                        DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
                        label7.Text = ds.Tables[0].Rows[0]["count(*)"].ToString();

                        sql = "select count(*) from TD_AGV_WAREHOUSE_LOCATION t where t.use_status<>0";
                        ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
                        label8.Text = ds.Tables[0].Rows[0]["count(*)"].ToString();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                    
            }
        }

 
        private void tsmiChangeLocation_Click(object sender, EventArgs e)
        {
            if (lvLocation.SelectedIndices != null && lvLocation.SelectedIndices.Count > 0)
            {
                if (lvLocation.SelectedItems[0].SubItems[3].Text.ToString() != "有货" )
                    MessageBox.Show("该货位没有可调整的周转箱！");
                else
                {
                    if (locationStatus == "MainFrm")
                    {
                        FrmChangeLocation frmChangeLocation = new FrmChangeLocation(this.mainFrm, lvLocation.SelectedItems[0].SubItems[0].Text.ToString(), lvLocation.SelectedItems[0].SubItems[8].Text.ToString(), lvLocation.SelectedItems[0].SubItems[9].Text.ToString(), lvLocation.SelectedItems[0].SubItems[10].Text.ToString(), lvLocation.SelectedItems[0].SubItems[1].Text.ToString(), locationStatus);
                        frmChangeLocation.Show();
                    }
                    else if (locationStatus == "Waiting")
                    {
                        FrmChangeLocation frmChangeLocation = new FrmChangeLocation(this.mainFrm, lvLocation.SelectedItems[0].SubItems[0].Text.ToString(), lvLocation.SelectedItems[0].SubItems[6].Text.ToString(), "","","", locationStatus);
                        frmChangeLocation.Show();
                    }
                }
            }
            else
                MessageBox.Show("请选中一行订单数据！");
        }


        private void tsmiChangeUnitStatus_Click(object sender, EventArgs e)
        {
            if (lvLocation.SelectedIndices != null && lvLocation.SelectedIndices.Count > 0)
            {
                FrmLocationEdit frmLocationEdit = new FrmLocationEdit(this, lvLocation.SelectedItems[0].SubItems[0].Text.ToString(), lvLocation.SelectedItems[0].SubItems[2].Text.ToString());
                frmLocationEdit.Show();
            }
            else
                MessageBox.Show("请选中一行订单数据！");
        }

        /// <summary>
        /// 按接运单号全部出库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 全部ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string rs;
            if(mainFrm.taskType[1]==1)
            {
                if (lvLocation.SelectedItems.Count < 1)
                    MessageBox.Show("请先选中一行数据！");
                if (lvLocation.SelectedItems[0].SubItems[3].Text != "有货")
                    MessageBox.Show("当前库位无货！");
                using (MySqlConnection con = mainFrm.dbConn.GetConnectFromPool())
                {
                    if (con == null)
                    {
                        MessageBox.Show("数据库连接不足！请稍后再试！");
                        return;
                    }
                    try
                    {
                        if (mainFrm.outConveyorCmd.OutBoundBoxCode() != null)
                        {
                            MessageBox.Show("一楼吨桶放货站台有货，无可用放货站台！");
                            return;
                        }
                        string sql = "select t.* from TD_PLT_LOCATION_DIC t where t.batch_id = '" + lvLocation.SelectedItems[0].SubItems[9].Text + "' and t.unit_status = 1 and (t.use_status = 0 or t.use_status = 2)";
                        DataSet ds = DataBase.MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (DataBaseInterface.CreateOutBoundTask(con, row["box_barcode"].ToString(), "", 2, 1, out rs) != 1)
                                MessageBox.Show("生成失败！");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                RefreshListView();
            }
            else
            {
                MessageBox.Show("请先将二楼任务模式切换到出库");
            }
           
        }

        public void HWweight() {

            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                try
                {
                    String sql = "select  sum(goods_weight) as kg from  td_plt_location_dic where goods_sku<>000000 and unit_status=1";
                    DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
                    label12.Text = ds.Tables[0].Rows[0]["kg"].ToString()+"吨";

                 
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 仅相同废料ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string rs;
            if (lvLocation.SelectedItems.Count < 1)
                MessageBox.Show("请先选中一行数据！");
            if (lvLocation.SelectedItems[0].SubItems[3].Text != "有货")
                MessageBox.Show("当前库位无货！");
            using (MySqlConnection con = mainFrm.dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    MessageBox.Show("数据库连接不足！请稍后再试！");
                    return;
                }
                try
                {
                    if (mainFrm.outConveyorCmd.OutBoundBoxCode() != null)
                    {
                        MessageBox.Show("一楼吨桶放货站台有货，无可用放货站台！");
                        return;
                    }
                    string sql = "select t.* from TD_PLT_LOCATION_DIC t where t.batch_no = '" + lvLocation.SelectedItems[0].SubItems[9].Text + "' and t.unit_status = 1 and (t.use_status = 0 or t.use_status = 2) and t.goods_sku = '" + lvLocation.SelectedItems[0].SubItems[10].Text + "'";
                    DataSet ds = DataBase.MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (DataBaseInterface.CreateOutBoundTask(con,row["box_barcode"].ToString(),"", 2, 1, out rs) != 1)
                            MessageBox.Show("生成失败！");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            RefreshListView();
        }
        /// <summary>
        /// 出库-仅单个
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 仅单个ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string rs;
            if(mainFrm.taskType[1]==1)
            {
                if (lvLocation.SelectedItems.Count < 1)
                    MessageBox.Show("请先选中一行数据！");
                if (lvLocation.SelectedItems[0].SubItems[3].Text != "有货")
                    MessageBox.Show("当前库位无货！");
                using (MySqlConnection con = mainFrm.dbConn.GetConnectFromPool())
                {
                    if (con == null)
                    {
                        MessageBox.Show("数据库连接不足！请稍后再试！");
                        return;
                    }
                    try
                    {
                        string sql = "select t.* from TD_PLT_LOCATION_DIC t where t.location_id = '" + lvLocation.SelectedItems[0].SubItems[0].Text + "'";
                        DataSet ds = DataBase.MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (row["use_status"].ToString() == "3" || row["use_status"].ToString() == "1")
                            {
                                MessageBox.Show("该库位禁止出库！");
                                continue;
                            }
                            if (mainFrm.outConveyorCmd.OutBoundBoxCode() != null)
                            {
                                MessageBox.Show("一楼吨桶放货站台有货，无可用放货站台！");
                                continue;
                            }
                            DataBaseInterface.Inserttask(row["box_barcode"].ToString());
                            //if (
                            // DataBaseInterface.CreateOutBoundTask(con, row["box_barcode"].ToString(),"", 2, 1, out rs) != 1)
                            //MessageBox.Show("生成失败！");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                RefreshListView();
            }
            else
            {
                MessageBox.Show("请先将二楼任务模式切换到出库");
            }
           
        }
        /// <summary>
        /// 退库-相同废料
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 相同废料ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string rs;
            if(mainFrm.taskType[0]==3)
            {
                if (lvLocation.SelectedItems.Count < 1)
                    MessageBox.Show("请先选中一行数据！");
                if (lvLocation.SelectedItems[0].SubItems[3].Text != "有货")
                    MessageBox.Show("当前库位无货！");
                using (MySqlConnection con = mainFrm.dbConn.GetConnectFromPool())
                {
                    if (con == null)
                    {
                        MessageBox.Show("数据库连接不足！请稍后再试！");
                        return;
                    }
                    try
                    {
                        string sql = "select t.* from TD_PLT_LOCATION_DIC t where t.batch_no = '" + lvLocation.SelectedItems[0].SubItems[9].Text + "' and t.unit_status = 1 and (t.use_status = 0 or t.use_status = 2) and t.goods_sku = '" + lvLocation.SelectedItems[0].SubItems[10].Text + "'";
                        DataSet ds = DataBase.MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (DataBaseInterface.CreateOutBoundTask(con, row["box_barcode"].ToString(), mainFrm.inConveyorScannerCmd.GoodsStatus().ToString(), 4, 1, out rs) != 1)
                                MessageBox.Show("生成失败！");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                RefreshListView();
            }
            else
            {
                MessageBox.Show("请先将一楼的任务模式改为退库");
            }
        }
        /// <summary>
        /// 退库-全部
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 全部ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string rs;
            if(mainFrm.taskType[0]==3)
            {
                if (lvLocation.SelectedItems.Count < 1)
                    MessageBox.Show("请先选中一行数据！");
                if (lvLocation.SelectedItems[0].SubItems[3].Text != "有货")
                    MessageBox.Show("当前库位无货！");
                using (MySqlConnection con = mainFrm.dbConn.GetConnectFromPool())
                {
                    if (con == null)
                    {
                        MessageBox.Show("数据库连接不足！请稍后再试！");
                        return;
                    }
                    try
                    {
                        string sql = "select t.* from TD_PLT_LOCATION_DIC t where t.batch_no = '" + lvLocation.SelectedItems[0].SubItems[9].Text + "' and t.unit_status = 1 and (t.use_status = 0 or t.use_status = 2)";
                        DataSet ds = DataBase.MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (DataBaseInterface.CreateOutBoundTask(con, row["box_barcode"].ToString(), mainFrm.inConveyorScannerCmd.GoodsStatus().ToString(), 4, 1, out rs) != 1)
                                MessageBox.Show("生成失败！");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
                RefreshListView();
            }
            else
            {
                MessageBox.Show("请先将一楼的任务模式切换到退库");
            }
            
        }
        /// <summary>
        /// 退库仅单个
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 仅单个ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string rs;
            if(mainFrm.taskType[0]==3)
            {
                if (lvLocation.SelectedItems.Count < 1)
                    MessageBox.Show("请先选中一行数据！");
                if (lvLocation.SelectedItems[0].SubItems[3].Text != "有货")
                    MessageBox.Show("当前库位无货！");
                using (MySqlConnection con = mainFrm.dbConn.GetConnectFromPool())
                {
                    if (con == null)
                    {
                        MessageBox.Show("数据库连接不足！请稍后再试！");
                        return;
                    }
                    try
                    {
                        string sql = "select t.* from TD_PLT_LOCATION_DIC t where t.location_id = '" + lvLocation.SelectedItems[0].SubItems[0].Text + "'";
                        DataSet ds = DataBase.MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (row["use_status"].ToString() == "3" || row["use_status"].ToString() == "1")
                            {
                                MessageBox.Show("该库位禁止出库！");
                                continue;
                            }
                            if (DataBaseInterface.CreateOutBoundTask(con, row["box_barcode"].ToString(), mainFrm.inConveyorScannerCmd.GoodsStatus().ToString(), 4, 1, out rs) != 1)
                                MessageBox.Show("生成失败！");
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }

                RefreshListView();
            }
            else
            {
                MessageBox.Show("请先将一楼的任务模式切换到退库");
            }
           
        }

        /// <summary>
        /// AGV单任务出库目的地1003
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 目的1003ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvLocation.SelectedItems.Count < 1)
                MessageBox.Show("请先选中一行数据！");
            if (lvLocation.SelectedItems[0].SubItems[2].Text != "有货")
                MessageBox.Show("当前库位无货！");
            try
            {
                string rs;
                using (MySqlConnection conn = dbConn.GetConnectFromPool())
                {
                    if (DataBaseInterface.AGVTaskCreate(conn,lvLocation.SelectedItems[0].SubItems[6].Text, 2, lvLocation.SelectedItems[0].SubItems[0].Text, "1003", "1", out rs) != 1)
                        MessageBox.Show("生成任务失败：" + rs);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            RefreshListView();
        }
        /// <summary>
        /// AGV单任务出库目的地1004
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 等待处1004ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvLocation.SelectedItems.Count < 1)
                MessageBox.Show("请先选中一行数据！");
            if (lvLocation.SelectedItems[0].SubItems[2].Text != "有货")
                MessageBox.Show("当前库位无货！");
            using (MySqlConnection con = mainFrm.dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    MessageBox.Show("数据库连接不足！请稍后再试！");
                    return;
                }
                try
                {
                    string rs;
                    using (MySqlConnection conn = dbConn.GetConnectFromPool())
                    {
                        if (DataBaseInterface.AGVTaskCreate(conn,lvLocation.SelectedItems[0].SubItems[6].Text, 2, lvLocation.SelectedItems[0].SubItems[6].Text, "1004", "1", out rs) != 1)
                            MessageBox.Show("生成任务失败：" + rs);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
                
            RefreshListView();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            FormLocationView f = new FormLocationView(mainFrm);
            f.ShowDialog();
        }

        private void tsmiChangeGoodsKind_Click(object sender, EventArgs e)
        {
            if (lvLocation.SelectedIndices != null && lvLocation.SelectedIndices.Count > 0)
            {
                FrmChangeGoodsKind frmChangeGoodsKind = new FrmChangeGoodsKind (this, lvLocation.SelectedItems[0].SubItems[0].Text.ToString(), lvLocation.SelectedItems[0].SubItems[1].Text.ToString());
                frmChangeGoodsKind.Show();
            }
            else
                MessageBox.Show("请选中一行订单数据！");
        }

        private void tsmiChangeHazardArea_Click(object sender, EventArgs e)
        {
            if (lvLocation.SelectedIndices != null && lvLocation.SelectedIndices.Count > 0)
            {
                FrmChangeHazardArea frmChangeHazardArea = new FrmChangeHazardArea(this, lvLocation.SelectedItems[0].SubItems[0].Text.ToString(), lvLocation.SelectedItems[0].SubItems[13].Text.ToString());
                frmChangeHazardArea.Show();
            }
            else
                MessageBox.Show("请选中一行订单数据！");
        }

        private void 仅单个ToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            if (lvLocation.SelectedItems.Count < 1)
                MessageBox.Show("请先选中一行数据！");
            if (lvLocation.SelectedItems[0].SubItems[2].Text != "有货")
                MessageBox.Show("当前库位无货！");
            try
            {
                DataBaseInterface.UpdateAgvLocation(lvLocation.SelectedItems[0].SubItems[0].Text);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
     
            RefreshListView();

        }

        private void 所有ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (lvLocation.SelectedItems.Count < 1)
                MessageBox.Show("请先选中一行数据！");
            if (lvLocation.SelectedItems[0].SubItems[2].Text != "有货")
                MessageBox.Show("当前库位无货！");
            try
            {
                DataBaseInterface.UpdateAgvLocation();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            RefreshListView();
        }

        private void 修改废物名称ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FormUpdate_name FUname = new FormUpdate_name(lvLocation.SelectedItems[0].SubItems[9].Text, lvLocation.SelectedItems[0].SubItems[10].Text, lvLocation.SelectedItems[0].SubItems[11].Text, lvLocation.SelectedItems[0].SubItems[13].Text);
            FUname.Show();

        }
    }
}

