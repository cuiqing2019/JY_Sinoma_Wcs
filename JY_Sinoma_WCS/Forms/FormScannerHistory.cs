using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataBase;
using Sinoma_WCS;
using MySql.Data.MySqlClient;

namespace JY_Sinoma_WCS
{
    public partial class FormScannerHistory : Form
    {
        public DataSet ds = null;
        private frmMain mainFrm;
        public ConnectPool dbConn;//定义数据库连接
        public string strselect = "";
        public FormScannerHistory(frmMain mainFrm)
        {
            ThreadExceptionDialog.CheckForIllegalCrossThreadCalls = false;//允许直接访问线程之间的控件
            this.mainFrm = mainFrm;
            dbConn = mainFrm.dbConn;
            InitializeComponent();
        }
        #region 在combox中显示扫描器
        public void ShowScannerName()
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                try
                {
                    strselect = "select DEVICE_ID,DEVICE_NAME from TD_PLT_SCAN_DIC order by scanner_id";
                    ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strselect);
                    DataTable dt = ds.Tables[0];
                    // cmbScanner.BeginUpdate();
                    DataRow dr1 = dt.NewRow();
                    dr1["DEVICE_NAME"] = "--选择所有--";
                    dt.Rows.InsertAt(dr1, 0);
                    cmbScanner.DataSource = dt;
                    cmbScanner.DisplayMember = "DEVICE_NAME";
                    cmbScanner.ValueMember = "DEVICE_ID";
                    //cmbScanner.EndUpdate();
                    cmbStatus.SelectedIndex = 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
               
        }
        #endregion
        private void FormScannerHistory_Load(object sender, EventArgs e)
        {
            dtpStart.Value = DateTime.Now.AddDays(-1);
            ShowScannerName();
            InitLV();
        }

        private void btnSelect_Click(object sender, EventArgs e)
        {
            RefreshListView();
        }

        #region 初始化listView
        /// <summary>
        /// 初始化listView
        /// </summary>
        private void InitLV()
        {
            lvContainer.Columns.Add("扫描器ID", (int)(lvContainer.Width * 0.1), HorizontalAlignment.Center);
            lvContainer.Columns.Add("扫描器名", (int)(lvContainer.Width * 0.25), HorizontalAlignment.Center);
            lvContainer.Columns.Add("托盘条码", (int)(lvContainer.Width * 0.15), HorizontalAlignment.Center);
            lvContainer.Columns.Add("扫描时间", (int)(lvContainer.Width * 0.25), HorizontalAlignment.Center);
            lvContainer.Columns.Add("完成状态", (int)(lvContainer.Width * 0.1), HorizontalAlignment.Center);
            this.ContextMenuStrip = this.cmsChangeStatus;
            RefreshListView();
            //tbContainer.Text = "ZX0";
        }
        #endregion

        #region 刷新ListView
        public void RefreshListView()
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                string device_id = cmbScanner.SelectedValue.ToString();
                string barcode = txtBarcode.Text;
                string strSQL = "";
                int i = 0;
                strSQL = "select a.SCANNER_ID,b.DEVICE_NAME,BOX_BARCODE,SCAN_TIME,STATUS from TB_PLT_SCAN_RECORD a,TD_PLT_SCAN_DIC b where a.SCANNER_ID=b.DEVICE_ID ";
                if (barcode != "")
                    strSQL += " and a.BOX_BARCODE like'%" + barcode + "%'";
                if (cmbStatus.SelectedIndex.ToString() != "0")
                    strSQL += " and a.STATUS=" + (cmbStatus.SelectedIndex - 1) + "";
                if (cmbScanner.SelectedIndex > 0)
                {
                    strSQL += " and a.SCANNER_ID=" + cmbScanner.SelectedIndex + "";
                }
                strSQL += " and  a.SCAN_TIME> str_to_date('" + dtpStart.Text.ToString() + "','%Y-%m-%d %H:%i:%s') order by a.scan_time desc ";

                try
                {
                    DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                    i = UpdateListview(i, ds);
                }
                catch (Exception)
                {
                    throw;
                }
            }
                
        }

        private int UpdateListview(int i, DataSet ds)
        {
            lvContainer.BeginUpdate();
            lvContainer.Items.Clear();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string[] items = new string[lvContainer.Columns.Count];
                items[0] = row["SCANNER_ID"].ToString();
                items[1] = row["DEVICE_NAME"].ToString();
                items[2] = row["BOX_BARCODE"].ToString();
                items[3] = row["SCAN_TIME"].ToString();
                items[4] = int.Parse(row["STATUS"].ToString()) == 0 ? "未处理" : (int.Parse(row["status"].ToString()) == 1 ? "已处理" : (int.Parse(row["status"].ToString()) == 2 ? "未处理空托盘" : "手动处理"));

                lvContainer.Items.Add(new ListViewItem(items));
                if (i % 2 != 0)
                    lvContainer.Items[i].BackColor = Color.FromArgb(229, 255, 229);
                i++;
            }
            lvContainer.EndUpdate();
            return i;
        }
        #endregion

        private void tspmiChangeStatus_Click(object sender, EventArgs e)
        {
            string strSQL = "";
            if (lvContainer.SelectedIndices == null || lvContainer.SelectedIndices.Count <= 0)
            {
                MessageBox.Show("请选中一行扫描数据！");
                return;
            }
            if (lvContainer.SelectedItems[0].SubItems[4].Text.ToString() == "已处理" || lvContainer.SelectedItems[0].SubItems[4].Text.ToString() == "手动处理")
            {
                MessageBox.Show("该任务已被处理！");
                return;
            }

            if (MessageBox.Show("确认要将" + lvContainer.SelectedItems[0].SubItems[3].Text.ToString() + "扫到的条码：" + lvContainer.SelectedItems[0].SubItems[2].Text.ToString() + "手动处理？", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                using (MySqlConnection conn = dbConn.GetConnectFromPool())
                {
                    if (conn == null)
                    {
                        MessageBox.Show("无法建立数据库连接！");
                        return;
                    }
                    try
                    {
                        strSQL = "update TB_PLT_SCAN_RECORD set STATUS=3 where BOX_BARCODE='" + lvContainer.SelectedItems[0].SubItems[2].Text.ToString() + "' and SCAN_TIME=str_to_date('" + lvContainer.SelectedItems[0].SubItems[3].Text.ToString() + "','%Y-%m-%d %H:%i:%s')";
                        DataBase.MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL);
                        RefreshListView();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("修改数据库失败！" + ex.Message);
                        return;
                    }
                } 
            }
        }

        private void 修改为未处理ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string strSQL = "";
            if (lvContainer.SelectedIndices == null || lvContainer.SelectedIndices.Count <= 0)
            {
                MessageBox.Show("请选中一行扫描数据！");
                return;
            }
            if (lvContainer.SelectedItems[0].SubItems[4].Text.ToString() == "未处理" )
            {
                MessageBox.Show("该任务未被处理！");
                return;
            }

            if (MessageBox.Show("确认要将" + lvContainer.SelectedItems[0].SubItems[3].Text.ToString() + "扫到的条码：" + lvContainer.SelectedItems[0].SubItems[2].Text.ToString() + "修改为未处理？", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                using (MySqlConnection conn = dbConn.GetConnectFromPool())
                {
                    if (conn == null)
                    {
                        MessageBox.Show("无法建立数据库连接！");
                        return;
                    }
                    try
                    {
                        strSQL = "update TB_PLT_SCAN_RECORD set STATUS=0 where BOX_BARCODE='" + lvContainer.SelectedItems[0].SubItems[2].Text.ToString() + "' and SCAN_TIME=str_to_date('" + lvContainer.SelectedItems[0].SubItems[3].Text.ToString() + "','%Y-%m-%d %H:%i:%s')";
                        DataBase.MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL);
                        RefreshListView();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("修改数据库失败！" + ex.Message);
                        return;
                    }
                }
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (cmbScanner.SelectedIndex == 0 || comboBox1.SelectedIndex <1)
                MessageBox.Show("请选择扫码器编号与托盘型号！");
            FormNewScannReadCode f = new FormNewScannReadCode(this.mainFrm, comboBox1.SelectedIndex == 2 ? 3 : 1, cmbScanner.SelectedIndex.ToString());
            f.Show();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                try
                {
                    if (cmbScanner.SelectedIndex == 0)
                    {
                        DataBaseInterface.ClearHaveReadRFID(conn,"1");
                        DataBaseInterface.ClearHaveReadRFID(conn,"2");
                        DataBaseInterface.ClearHaveReadRFID(conn,"3");

                    }
                    else if (cmbScanner.SelectedIndex > 0 && cmbScanner.SelectedIndex < 4)
                    {
                        DataBaseInterface.ClearHaveReadRFID(conn,cmbScanner.SelectedIndex.ToString());
                    }
                    RefreshListView();
                    MessageBox.Show("处理完成！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
              
        }


    }
}