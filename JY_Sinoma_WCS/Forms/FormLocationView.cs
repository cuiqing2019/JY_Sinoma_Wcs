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
    public partial class FormLocationView : Form
    {
        private int[,] fristBoundLocation = new int[6, 2] { { 68, 46 }, { 68, 90 }, { 68, 164 }, { 68, 207 }, { 68, 280 }, { 68, 324 } };
        private List<Label> labelArray = new List<Label>();
        private List<Label> clearBayLabelArray = new List<Label>();
        private List<Label> clearRowLabelArray = new List<Label>();
        Label onlyLabel;
        DataTable locationTable;
        Label rowLabel, loactionLabelX, loactionLabelY;
        Label bayLabel;
        public frmMain mainFrm;
        DataRow locationRow;
        
        public FormLocationView(frmMain mainFrm)
        {
            InitializeComponent();
            cmSelectLocationLevel.SelectedIndex = 0;
            locationTable = DataBaseInterface.SelectLocationState();
            this.mainFrm = mainFrm;
            if (locationTable != null)
                ShowLoactionLabel(cmSelectLocationLevel.SelectedIndex+1);
            else
                this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            foreach (var item in clearRowLabelArray)
            {
                this.Controls.Remove(item);
            }
            foreach (var item in clearBayLabelArray)
            {
                this.Controls.Remove(item);
            }
            this.Controls.Remove(onlyLabel);
            if (rowLabel != null)
                rowLabel.ForeColor = Color.Black;
            if (bayLabel != null)
                bayLabel.ForeColor = Color.Black;
            if (loactionLabelX != null)
                loactionLabelX.ForeColor = Color.Black;
            if (loactionLabelY != null)
                loactionLabelY.ForeColor = Color.Black;
            if (locationTable != null)
                ShowLoactionLabel(cmSelectLocationLevel.SelectedIndex + 1);
        }

        private void bay_Click(object sender, EventArgs e)
        {
            Label l = (Label)sender;
            if (loactionLabelX != null)
                loactionLabelX.ForeColor = Color.Black;
            if (loactionLabelY != null)
                loactionLabelY.ForeColor = Color.Black;
            if (bayLabel != null)
                bayLabel.ForeColor = Color.Black;
            l.ForeColor = Color.Red;
            bayLabel = l;
            foreach (var item in clearBayLabelArray)
            {
                this.Controls.Remove(item);
            }
            this.Controls.Remove(onlyLabel);
            foreach (var item in labelArray)
            {
                DataRow row = (DataRow)item.Tag;
                if (l.Tag.ToString() == row["BAY_NO"].ToString())
                {
                    Label a = new Label();
                    a.Location = new Point(item.Location.X - 3, item.Location.Y - 3);
                    a.Size = new Size(31, 31);
                    a.BackColor = Color.Red;
                    a.SendToBack();
                    clearBayLabelArray.Add(a);
                    this.Controls.Add(a);
                }
            }
        }

        private void row_Click(object sender, EventArgs e)
        {
            Label l = (Label)sender;
            if (rowLabel != null)
                rowLabel.ForeColor = Color.Black;
            if (loactionLabelX != null)
                loactionLabelX.ForeColor = Color.Black;
            if (loactionLabelY != null)
                loactionLabelY.ForeColor = Color.Black;
            l.ForeColor = Color.Red;
            rowLabel = l;
            foreach (var item in clearRowLabelArray)
            {
                this.Controls.Remove(item);
            }
            this.Controls.Remove(onlyLabel);
            foreach (var item in labelArray)
            {
                DataRow row = (DataRow)item.Tag;
                if (l.Tag.ToString() == row["ROW_NO"].ToString())
                {
                    Label a = new Label();
                    a.Location = new Point(item.Location.X - 3, item.Location.Y - 3);
                    a.Size = new Size(31, 31);
                    a.BackColor = Color.Red;
                    a.SendToBack();
                    clearRowLabelArray.Add(a);
                    this.Controls.Add(a);
                }
            }
        }

        private void location_MouseDown(object sender, MouseEventArgs e)
        {
            foreach (var item in clearRowLabelArray)
            {
                this.Controls.Remove(item);
            }
            foreach (var item in clearBayLabelArray)
            {
                this.Controls.Remove(item);
            }
            this.Controls.Remove(onlyLabel);
            if (rowLabel != null)
                rowLabel.ForeColor = Color.Black;
            if (bayLabel != null)
                bayLabel.ForeColor = Color.Black;
            if (loactionLabelX != null)
                loactionLabelX.ForeColor = Color.Black;
            if (loactionLabelY != null)
                loactionLabelY.ForeColor = Color.Black;
            foreach (var item in panel1.Controls)
            {
                if ((item as Label).Tag.ToString() == ((sender as Label).Tag as DataRow)["bay_no"].ToString())
                {
                    (item as Label).ForeColor = Color.Red;
                    loactionLabelX = (item as Label);
                }
            } foreach (var item in panel2.Controls)
            {
                if ((item as Label).Tag.ToString() == ((sender as Label).Tag as DataRow)["ROW_NO"].ToString())
                {
                    (item as Label).ForeColor = Color.Red;
                    loactionLabelY = (item as Label);
                }
            }
            onlyLabel = new Label();
            onlyLabel.Location = new Point((sender as Label).Location.X - 3, (sender as Label).Location.Y - 3);
            onlyLabel.Size = new Size(31, 31);
            onlyLabel.BackColor = Color.Red;
            onlyLabel.SendToBack();
            this.locationRow = ((sender as Label).Tag as DataRow);
            this.tbBatchID.Text = locationRow["BATCH_ID"].ToString();
            this.tbBatchNo.Text = locationRow["BATCH_NO"].ToString();
            this.tbLocationId.Text = locationRow["LOCATION_ID"].ToString();
            this.tbSku.Text = locationRow["GOODS_SKU"].ToString();
            this.tbTrayCode.Text = locationRow["BOX_BARCODE"].ToString();
            this.tbWeight.Text = locationRow["GOODS_WEIGHT"].ToString();
            this.txtGoodsName.Text = locationRow["GOODS_NAME"].ToString();
            this.Controls.Add(onlyLabel);
        }
      
        public void ShowLoactionLabel(int level)
        {
            try
            {
                locationTable = DataBaseInterface.SelectLocationState();
                if (locationTable == null)
                {
                    MessageBox.Show("获取库位失败！");
                    return;
                }
                foreach (var item in labelArray)
                {
                    this.Controls.Remove(item);
                }
                foreach (var item in clearRowLabelArray)
                {
                    this.Controls.Remove(item);
                }
                foreach (var item in clearBayLabelArray)
                {
                    this.Controls.Remove(item);
                }
                this.Controls.Remove(onlyLabel);
                foreach (DataRow row in locationTable.Rows)
                {
                    if (row["LEVEL_NO"].ToString() == level.ToString())
                    {
                        Label l = new Label();
                        l.Location = new Point(fristBoundLocation[int.Parse(row["ROW_NO"].ToString()) - 1, 0] + (int.Parse(row["BAY_NO"].ToString()) - 1) * 50, fristBoundLocation[int.Parse(row["ROW_NO"].ToString()) - 1, 1]);
                        l.Size = new Size(25, 25);
                        if (row["UNIT_STATUS"].ToString() == "1")//如果有货
                            l.BackColor = Color.Green;
                        else if (row["UNIT_STATUS"].ToString() == "0")//如果无货
                            l.BackColor = Color.LightGreen;
                        else if (row["UNIT_STATUS"].ToString() == "2")//如果入库占用
                            l.BackColor = Color.Yellow;
                        else if (row["UNIT_STATUS"].ToString() == "3")//如果出入占用
                            l.BackColor = Color.LightYellow;
                        else //如果存在不可用状态
                            l.BackColor = Color.Red;
                        l.Tag = row;
                        l.Text = row["hazard_area"].ToString();
                        l.TextAlign = ContentAlignment.MiddleCenter;
                        l.MouseDown += new MouseEventHandler(this.location_MouseDown);
                        l.ContextMenuStrip = this.cmsStatus;
                        labelArray.Add(l);
                        this.Controls.Add(l);
                    }
                }
            }
            catch (Exception)
            {

            }
           
           
        }

        /// <summary>
        /// 按接运单号全部出库
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 全部ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string rs;
            if (locationRow["UNIT_STATUS"].ToString() != "1")
                MessageBox.Show("当前库位无货！");
            using (MySqlConnection con = mainFrm.dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    MessageBox.Show("数据库未连接！");
                    return;
                }
                try
                {
                    if (mainFrm.taskType[0]==1)
                    {
                        if (mainFrm.outConveyorCmd.OutBoundBoxCode() != null)
                        {
                            MessageBox.Show("一楼吨桶放货站台有货，无放货站台！");
                            return;
                        }
                        string sql = "select t.* from TD_PLT_LOCATION_DIC t where t.batch_id = '" + locationRow["BATCH_id"].ToString() + "' and t.unit_status = 1 and (t.use_status = 0 or t.use_status = 2)";
                        DataSet ds = DataBase.MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {

                            if (DataBaseInterface.CreateOutBoundTask(con, row["box_barcode"].ToString(), "", 2, 1, out rs) != 1)
                                MessageBox.Show("生成失败！");
                        }
                    }
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                ShowLoactionLabel(cmSelectLocationLevel.SelectedIndex + 1);
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
            if (locationRow["UNIT_STATUS"].ToString() != "1")
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
                    if (mainFrm.taskType[0]==1)
                    {
                        if (mainFrm.outConveyorCmd.OutBoundBoxCode() != null)
                        {
                            MessageBox.Show("一楼吨桶放货站台有货，无放货站台！");
                            return;
                        }
                        string sql = "select t.* from TD_PLT_LOCATION_DIC t where t.batch_no = '" + locationRow["BATCH_NO"].ToString() + "' and t.unit_status = 1 and (t.use_status = 0 or t.use_status = 2) and t.goods_sku = '" + locationRow["goods_sku"].ToString() + "'";
                        DataSet ds = DataBase.MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (DataBaseInterface.CreateOutBoundTask(con, row["box_barcode"].ToString(), "", 2, 1, out rs) != 1)
                                MessageBox.Show("生成失败！");
                        }
                    }
                 
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                ShowLoactionLabel(cmSelectLocationLevel.SelectedIndex + 1);
            }
                
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 仅单个ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string rs;
            if (locationRow["UNIT_STATUS"].ToString() != "1")
                MessageBox.Show("当前库位无货！");
            using (MySqlConnection con = mainFrm.dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    MessageBox.Show("数据库未连接！");
                    return;
                }
                try
                {
                    if (mainFrm.taskType[0] == 1)
                    {
                        string sql = "select t.* from TD_PLT_LOCATION_DIC t where t.location_id = '" + locationRow["location_id"].ToString() + "'";
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
                                MessageBox.Show("一楼吨桶放货站台有货，无放货站台！");
                                continue;
                            }
                            DataBaseInterface.Inserttask(row["box_barcode"].ToString());

                        }

                    }
                  
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                ShowLoactionLabel(cmSelectLocationLevel.SelectedIndex + 1);
            }
                
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 相同废料ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string rs;
            if (locationRow["UNIT_STATUS"].ToString() != "1")
                MessageBox.Show("当前库位无货！");
            using (MySqlConnection con = mainFrm.dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    MessageBox.Show("数据库未连接！");
                    return;
                }
                try
                {
                    if (mainFrm.taskType[0]==3)
                    {
                        string sql = "select t.* from TD_PLT_LOCATION_DIC t where t.batch_no = '" + locationRow["BATCH_NO"].ToString() + "' and t.unit_status = 1 and (t.use_status = 0 or t.use_status = 2) and t.goods_sku = '" + locationRow["goods_sku"].ToString() + "'";
                        DataSet ds = DataBase.MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (DataBaseInterface.CreateOutBoundTask(con, row["box_barcode"].ToString(), mainFrm.inConveyorScannerCmd.GoodsStatus().ToString(), 4, 1, out rs) != 1)
                                MessageBox.Show("生成失败！");
                        }
                    }
                    
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                ShowLoactionLabel(cmSelectLocationLevel.SelectedIndex + 1);
            }
                
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 全部ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string rs;
            if (locationRow["UNIT_STATUS"].ToString() != "1")
                MessageBox.Show("当前库位无货！");
            using (MySqlConnection con = mainFrm.dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    MessageBox.Show("数据库未连接！");
                    return;
                }
                try
                {
                    if (mainFrm.taskType[0]==3)
                    {
                        string sql = "select t.* from TD_PLT_LOCATION_DIC t where t.batch_id = '" + locationRow["BATCH_id"].ToString() + "' and t.unit_status = 1 and (t.use_status = 0 or t.use_status = 2)";
                        DataSet ds = DataBase.MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (DataBaseInterface.CreateOutBoundTask(con, row["box_barcode"].ToString(), mainFrm.inConveyorScannerCmd.GoodsStatus().ToString(), 4, 1, out rs) != 1)
                                MessageBox.Show("生成失败！");
                        }
                    }
                
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                ShowLoactionLabel(cmSelectLocationLevel.SelectedIndex + 1);
            }
                
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void 仅单个ToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            string rs;
            if (locationRow["UNIT_STATUS"].ToString() != "1")
                MessageBox.Show("当前库位无货！");
            using (MySqlConnection con = mainFrm.dbConn.GetConnectFromPool())
            {
                if (con == null)
                {
                    MessageBox.Show("数据库未连接！");
                    return;
                }
                try
                {
                    if (mainFrm.taskType[0] == 3)
                    {
                        string sql = "select t.* from TD_PLT_LOCATION_DIC t where t.location_id = '" + locationRow["location_id"].ToString() + "'";
                        DataSet ds = DataBase.MySqlHelper.ExecuteDataset(con, CommandType.Text, sql);
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (row["use_status"].ToString() == "3" || row["use_status"].ToString() == "1")
                            {
                                MessageBox.Show("该库位禁止出库！");
                                continue;
                            }
                            if (DataBaseInterface.CreateOutBoundTask(con, row["box_barcode"].ToString(), "", 4, 1, out rs) != 1)
                                MessageBox.Show("生成失败！");
                        }
                    }
                    else
                    {
                        MessageBox.Show("当前任务类型不是退库模式！");
                    }
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                ShowLoactionLabel(cmSelectLocationLevel.SelectedIndex + 1);
            }
               
        }
        #region 修改库位危险分区
        private void btnChangeLocationArea_Click(object sender, EventArgs e)
        {
            FormChangeHazardArea f = new FormChangeHazardArea();
            f.ShowDialog();
            ShowLoactionLabel(cmSelectLocationLevel.SelectedIndex + 1);
        }
        #endregion


        private void tsmiChangeLocation_Click(object sender, EventArgs e)
        {
            if (locationRow["UNIT_STATUS"].ToString() != "1")
                MessageBox.Show("当前库位无货！");
            else
            {
                FrmChangeLocation frmChangeLocation = new FrmChangeLocation(this.mainFrm, locationRow["location_id"].ToString(), locationRow["BOX_BARCODE"].ToString(), locationRow["BATCH_NO"].ToString(), locationRow["GOODS_SKU"].ToString(), locationRow["GOODS_KINDS"].ToString() == "1" ? "吨桶" : locationRow["GOODS_KINDS"].ToString() == "2" ? "圆桶" : "空托盘", "MainFrm");
                frmChangeLocation.Show();
                ShowLoactionLabel(cmSelectLocationLevel.SelectedIndex + 1);
            }
        }


        private void tsmiChangeUnitStatus_Click(object sender, EventArgs e)
        {
            if (locationRow["UNIT_STATUS"].ToString() != "1")
                MessageBox.Show("当前库位无货！");
            FrmLocationEdit frmLocationEdit = new FrmLocationEdit(this, locationRow["location_id"].ToString(), locationRow["USE_STATUS"].ToString() == "0" ? "可用" : locationRow["USE_STATUS"].ToString() == "1" ? "可入不可出" : locationRow["USE_STATUS"].ToString() == "2" ? "可出不可入" : "禁用");
            frmLocationEdit.Show();
            ShowLoactionLabel(cmSelectLocationLevel.SelectedIndex + 1);
        }
    }
}
