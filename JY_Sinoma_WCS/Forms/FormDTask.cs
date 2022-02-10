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
    public partial class FormDTask : Form
    {
        public DataSet ds = null;
        private frmMain mainFrm;
        public ConnectPool dbConn;//定义数据库连接
        public string strselect = "";
        List<KeyValuePair<int, string>> listItem1 = new List<KeyValuePair<int, string>>();
        public FormDTask(frmMain mainFrm)
        {
            ThreadExceptionDialog.CheckForIllegalCrossThreadCalls = false;//允许直接访问线程之间的控件
            this.mainFrm = mainFrm;
            dbConn = mainFrm.dbConn;
            InitializeComponent();
        }

        public void showTaskType()
        {
          
            listItem1.Clear();
            listItem1.Add(new KeyValuePair<int, string>(0, "--选择全部--"));
            listItem1.Add(new KeyValuePair<int, string>(1, "入库"));
            listItem1.Add(new KeyValuePair<int, string>(2, "出库"));
            listItem1.Add(new KeyValuePair<int, string>(3, "空托入库"));
            listItem1.Add(new KeyValuePair<int, string>(4, "退库"));
            listItem1.Add(new KeyValuePair<int, string>(5, "异常回库"));
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
            lvContainer.Columns.Add("主任务类型", (int)(lvContainer.Width * 0.1), HorizontalAlignment.Center);
            lvContainer.Columns.Add("子任务类型", (int)(lvContainer.Width * 0.1), HorizontalAlignment.Center);
            lvContainer.Columns.Add("生成时间", (int)(lvContainer.Width * 0.15), HorizontalAlignment.Center);
            lvContainer.Columns.Add("执行时间", (int)(lvContainer.Width * 0.15), HorizontalAlignment.Center);
            lvContainer.Columns.Add("完成时间", (int)(lvContainer.Width * 0.15), HorizontalAlignment.Center);
            lvContainer.Columns.Add("起始地址", (int)(lvContainer.Width * 0.09), HorizontalAlignment.Center);
            lvContainer.Columns.Add("目的地址", (int)(lvContainer.Width * 0.09), HorizontalAlignment.Center);
            lvContainer.Columns.Add("步骤", (int)(lvContainer.Width * 0.07), HorizontalAlignment.Center);
            lvContainer.Columns.Add("巷道", (int)(lvContainer.Width * 0.07), HorizontalAlignment.Center);
            lvContainer.Columns.Add("托盘条码", (int)(lvContainer.Width * 0.1), HorizontalAlignment.Center);
            lvContainer.Columns.Add("任务状态", (int)(lvContainer.Width * 0.1), HorizontalAlignment.Center);
           
           
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
                int taskType = int.Parse(cmbTaskType.SelectedValue.ToString());//0--选择全部--；1-入库2-出库 3-空托盘入库 4-退库5-异常回库564422
                string strSQL = "";

                strSQL = "select t.* from tb_plt_task_d t  where 1=1";
                if (taskType > 0)
                    strSQL += " and task_type=" + taskType + "";
                if (barcode != "")
                    strSQL += " and box_barcode like'%" + barcode + "%'";
                if (txtTaskID.Text.Trim().Length != 0)
                {
                    strSQL += " and task_id ='" + txtTaskID.Text.Trim().ToString() + "'";
                }
                strSQL += " and create_time>=str_to_date('" + dtpStart.Text.ToString() + "','%Y-%m-%d %H:%i:%s') and create_time<= str_to_date('" + dtpEnd.Text.ToString() + "','%Y-%m-%d %H:%i:%s')  order by create_time desc";

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
                items[2] = this.mainFrm.DecodeMTaskType(int.Parse(row["TASK_TYPE"].ToString()));
                items[3] = row["CREATE_TIME"].ToString();
                items[4] = row["BEGIN_TIME"].ToString();
                items[5] = row["FINISH_TIME"].ToString();
                items[6] = row["FROM_UNIT"].ToString();
                items[7] = row["TO_UNIT"].ToString();
                items[8] = row["STEP"].ToString();
                items[9] = row["ROW_NUM"].ToString();
                items[10] = row["BOX_BARCODE"].ToString();
                items[11] = row["STATUS"].ToString();
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
