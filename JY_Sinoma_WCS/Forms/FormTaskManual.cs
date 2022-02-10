using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using DataBase;
using Sinoma_WCS;
using MySql.Data.MySqlClient;

namespace JY_Sinoma_WCS
{
    public partial class FormTaskManual : Form
    {
        public DataSet ds = null;
        private frmMain mainFrm;
        public ConnectPool dbConn;//定义数据库连接
        public string strselect = "";
        List<KeyValuePair<int, string>> listItem1 = new List<KeyValuePair<int, string>>();
        public FormTaskManual(frmMain mainFrm)
        {
            ThreadExceptionDialog.CheckForIllegalCrossThreadCalls = false;//允许直接访问线程之间的控件
            this.mainFrm = mainFrm;
            dbConn = mainFrm.dbConn;
            InitializeComponent();
        }

        public void showTaskType()
        {
            listItem1.Clear();
            listItem1.Add(new KeyValuePair<int, string>(0, "--选择--"));
            listItem1.Add(new KeyValuePair<int, string>(1, "入库"));
            listItem1.Add(new KeyValuePair<int, string>(2, "出库"));
            cmbTaskType.DataSource = listItem1;
            cmbTaskType.DisplayMember = "value";
            cmbTaskType.ValueMember = "key";
            // cmbTaskType.SelectedItem = 0;
        }
        private void FormTaskHistory_Load(object sender, EventArgs e)
        {
            dtpStart.Value = DateTime.Now.AddDays(-1);
            showTaskType();
            InitLV();
        }

        #region 初始化listView
        /// <summary>
        /// 初始化listView
        /// </summary>
        private void InitLV()
        {
            lvContainer.Columns.Add("任务号", (int)(lvContainer.Width * 0.10), HorizontalAlignment.Center);
            lvContainer.Columns.Add("任务类型", (int)(lvContainer.Width * 0.08), HorizontalAlignment.Center);
            lvContainer.Columns.Add("层", (int)(lvContainer.Width * 0.03), HorizontalAlignment.Center);
            lvContainer.Columns.Add("托盘条码", (int)(lvContainer.Width * 0.15), HorizontalAlignment.Center);
            lvContainer.Columns.Add("重量", (int)(lvContainer.Width * 0.06), HorizontalAlignment.Center);
            lvContainer.Columns.Add("任务接收时间", (int)(lvContainer.Width * 0.15), HorizontalAlignment.Center);
            lvContainer.Columns.Add("任务执行时间", (int)(lvContainer.Width * 0.15), HorizontalAlignment.Center);
            lvContainer.Columns.Add("废料包装", (int)(lvContainer.Width * 0.15), HorizontalAlignment.Center);
            lvContainer.Columns.Add("废料编号", (int)(lvContainer.Width * 0.15), HorizontalAlignment.Center);
            lvContainer.Columns.Add("派车单号", (int)(lvContainer.Width * 0.15), HorizontalAlignment.Center);
            lvContainer.Columns.Add("接运单id", (int)(lvContainer.Width * 0.16), HorizontalAlignment.Center);
            lvContainer.Columns.Add("任务状态", (int)(lvContainer.Width * 0.16), HorizontalAlignment.Center);
           
           
            //this.ContextMenuStrip = this.cmsTask;
            RefreshListView();
            //tbContainer.Text = "ZX0";
        }
        #endregion

        private void btnQuery_Click(object sender, EventArgs e)
        {
            RefreshListView();
        }

        #region 刷新ListView
        public void RefreshListView()
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;

                string barcode = txtBarcode.Text.ToString().Trim();
                int taskType = int.Parse(cmbTaskType.SelectedValue.ToString());//0--选择全部--；1一楼入库；2回库入库；3整托出库；4整托补货；5整箱出库；
                string strSQL = "";
                if (cmbTaskType.SelectedIndex == 1)
                {
                    strSQL = "select t.*,d.*,d.task_status as status from TB_COMM_INBOUND t,tb_plt_task_m d where t.batch_no = d.batch_no and t.box_code = d.box_barcode and t.goods_sku = d.sku and d.task_type = 1 and t.dealway = 1";
                    if (taskType > 0)
                        strSQL += " and d.TASK_TYPE=" + taskType + "";
                    if (barcode != "")
                        strSQL += " and d.BOX_BARCODE like'%" + barcode + "%'";
                    if (txtTaskId.Text.Trim().Length != 0)
                    {
                        strSQL += " and d.TASK_ID ='" + txtTaskId.Text.Trim().ToString() + "'";
                    }
                    strSQL += " and d.CREATE_TIME>=str_to_date('" + dtpStart.Text.ToString() + "','%Y-%m-%d %H:%i:%s') and d.CREATE_TIME<= str_to_date('" + dtpEnd.Text.ToString() + "','%Y-%m-%d %H:%i:%s')  order by d.CREATE_TIME desc";

                }
                if (cmbTaskType.SelectedIndex == 2)
                {
                    strSQL = "select t.*,d.*,d.task_status as status from TB_COMM_OUTBOUND t,tb_plt_task_m d where t.batch_no = d.batch_no and t.box_code = d.box_barcode and t.goods_sku = d.sku and d.task_type = 2 and t.dealway = 1";
                    if (taskType > 0)
                        strSQL += " and d.TASK_TYPE=" + taskType + "";
                    if (barcode != "")
                        strSQL += " and d.BOX_BARCODE like'%" + barcode + "%'";
                    if (txtTaskId.Text.Trim().Length != 0)
                    {
                        strSQL += " and d.TASK_ID ='" + txtTaskId.Text.Trim().ToString() + "'";
                    }
                    strSQL += " and d.CREATE_TIME>=str_to_date('" + dtpStart.Text.ToString() + "','%Y-%m-%d %H:%i:%s') and d.CREATE_TIME<= str_to_date('" + dtpEnd.Text.ToString() + "','%Y-%m-%d %H:%i:%s')  order by d.CREATE_TIME desc";

                }
                if (cmbTaskType.SelectedIndex < 1 || cmbTaskType.SelectedIndex > 2)
                    return;
                int i = 0;
                try
                {
                    DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                    i = UpdateListview(i, ds);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    throw;
                }
            }
                
        }
        private int UpdateListview(int i, DataSet ds)
        {
            int count = 0;
            lvContainer.BeginUpdate();
            lvContainer.Items.Clear();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string[] items = new string[lvContainer.Columns.Count];
                items[0] = row["TASK_ID"].ToString();
                items[1] = this.mainFrm.DecodeMTaskType(int.Parse(row["TASK_TYPE"].ToString()));
                items[2] = row["TASK_LEVEL"].ToString();
                items[3] = row["BOX_BARCODE"].ToString();
                items[4] = row["GOODS_WEIGHT"].ToString();
                items[5] = row["CREATE_TIME"].ToString();
                items[6] = row["BEGIN_TIME"].ToString();
                items[7] = row["GOODS_KIND"].ToString() == "1"?"吨桶":row["GOODS_KIND"].ToString() == "2"?"圆桶":row["GOODS_KIND"].ToString() == "3"?"空托盘":"未知货物";
                items[8] = row["SKU"].ToString();
                items[9] = row["BATCH_NO"].ToString();
                items[10] = row["BATCH_ID"].ToString();
                items[11] = row["status"].ToString() == "1" ? "执行中" : row["GOODS_KIND"].ToString() == "2" ? "已完成" : row["GOODS_KIND"].ToString() == "3" ? "已生成异常回库" : "未知任务类型";
                
                lvContainer.Items.Add(new ListViewItem(items));
                if (i % 2 != 0)
                    lvContainer.Items[i].BackColor = Color.FromArgb(229, 255, 229);
                i++;
                count++;
            }
            lvContainer.EndUpdate();
            txtTaskCount.Text = count.ToString();
            return i;
           
        }
        #endregion

    }
}
