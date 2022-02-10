using DataBase;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JY_Sinoma_WCS.Forms
{
    public partial class FormCargoCount : Form
    {
        public DataSet ds = null;
        private frmMain mainFrm;
        public ConnectPool dbConn;//定义数据库连接
        public string strselect = "";
        List<KeyValuePair<int, string>> listItem1 = new List<KeyValuePair<int, string>>();
        public FormCargoCount(frmMain mainFrm)
        {
            this.mainFrm = mainFrm;
            dbConn = mainFrm.dbConn;
            InitializeComponent();
        }
        public void showTaskType()
        {
           listItem1.Clear();
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
        private void button1_Click(object sender, EventArgs e)
        {
            lvContainer.Items.Clear();
            RefreshListView();
           // lvContainer.Clear();

        }
        #region 初始化listView
        /// <summary>
        /// 初始化listView
        /// </summary>
        private void InitLV()
        {
            lvContainer.Columns.Add("序号", (int)(lvContainer.Width * 0.1), HorizontalAlignment.Center);
            lvContainer.Columns.Add("任务类型", (int)(lvContainer.Width * 0.2), HorizontalAlignment.Center);
            lvContainer.Columns.Add("名称", (int)(lvContainer.Width * 0.3), HorizontalAlignment.Center);
            lvContainer.Columns.Add("总数", (int)(lvContainer.Width * 0.2), HorizontalAlignment.Center);
            lvContainer.Columns.Add("总吨数", (int)(lvContainer.Width * 0.2), HorizontalAlignment.Center);


            //this.ContextMenuStrip = this.cmsTask;
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
                int taskType = int.Parse(cmbTaskType.SelectedValue.ToString());//0--选择全部--；1入库；2出库；3空托入库；4退库；5异常回库；
                string strSQL = "";

                strSQL = "select task_type,batch_id,count(1),sum(goods_weight) from  tb_plt_task_m where 1=1";
                if (taskType > 0)
                    strSQL += " and TASK_TYPE=" + taskType + "";

               
                strSQL += " and CREATE_TIME>= str_to_date('" + dtpStart.Text.ToString() + "','%Y-%m-%d %H:%i:%s') and CREATE_TIME<= str_to_date('" + dtpEnd.Text.ToString() + "','%Y-%m-%d %H:%i:%s')  GROUP BY batch_id  order by CREATE_TIME desc";

                int i = 0;
                try
                {
                    DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string[] items = new string[lvContainer.Columns.Count];
                        i++;
                        items[0] = i.ToString();
                        items[1] = row["task_type"].ToString() == "1" ? "入库" : "出库";
                        if (row["task_type"].ToString() == "1")
                            items[1] = "入库";
                        else if (row["task_type"].ToString() == "2")
                            items[1] = "出库";
                        else if (row["task_type"].ToString() == "3")
                            items[1] = "空托盘入库";
                        else if (row["task_type"].ToString() == "4")
                            items[1] = "退库";
                        else if (row["task_type"].ToString() == "5")
                            items[1] = "异常回库";
                        items[2] = row["batch_id"].ToString();
                        items[3] = row["count(1)"].ToString();
                        items[4] = row["sum(goods_weight)"].ToString();
                        lvContainer.Items.Add(new ListViewItem(items));
                       
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);

                }
            }
        }
        #endregion

        private void FormCargoCount_Load(object sender, EventArgs e)
        {
            dtpStart.Value = DateTime.Now.AddDays(-1);
            showTaskType();
            InitLV();
          

        }
    }
}
