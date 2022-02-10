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
    public partial class FormSubTask : Form
    {
        private frmMain mainFrm;
        private string strTask;
        public ConnectPool dbConn;//定义数据库连接
        public FormSubTask(frmMain mainFrm,string strTask)
        {
            this.mainFrm = mainFrm;
            this.strTask = strTask;
            dbConn = mainFrm.dbConn;
            InitializeComponent();
        }

        private void FrmSubTask_Load(object sender, EventArgs e)
        {
            lvTask.Columns.Add("任务号", (int)(lvTask.Width * 0.1), HorizontalAlignment.Center);
            lvTask.Columns.Add("主任务类型", (int)(lvTask.Width * 0.15), HorizontalAlignment.Center);
            lvTask.Columns.Add("子任务类型", (int)(lvTask.Width * 0.15), HorizontalAlignment.Center);
            lvTask.Columns.Add("步骤", (int)(lvTask.Width * 0.05), HorizontalAlignment.Center);
            lvTask.Columns.Add("起始地址", (int)(lvTask.Width * 0.15), HorizontalAlignment.Center);
            lvTask.Columns.Add("目的地址", (int)(lvTask.Width * 0.15), HorizontalAlignment.Center);
            lvTask.Columns.Add("设备类型", (int)(lvTask.Width * 0.1), HorizontalAlignment.Center);
            lvTask.Columns.Add("巷道", (int)(lvTask.Width * 0.05), HorizontalAlignment.Center);
            lvTask.Columns.Add("状态", (int)(lvTask.Width * 0.10), HorizontalAlignment.Center);
            RefreshListView();
        }

        public void RefreshListView()
        {
            lvTask.Items.Clear();
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                int i = 0;
                string strSQL = "select task_id ,task_type,step,from_unit,to_unit,device_type,ROW_NUM,status from TB_PLT_TASK_D  where task_id=" + strTask + " order by step";
                try
                {
                    DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                    lvTask.BeginUpdate();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string[] items = new string[lvTask.Columns.Count];
                        items[0] = row["task_id"].ToString();
                        items[1] = mainFrm.DecodeMTaskType(int.Parse(row["task_type"].ToString()));
                        items[2] = mainFrm.DecodeMTaskType(int.Parse(row["task_type"].ToString()));
                        items[3] = row["step"].ToString();
                        items[4] = row["from_unit"].ToString();
                        items[5] = row["to_unit"].ToString();
                        items[6] = mainFrm.DecodeDTaskType(row["device_type"].ToString());
                        items[7] = row["ROW_NUM"].ToString();
                        items[8] = mainFrm.DecodeDTaskStatus(int.Parse(row["status"].ToString()));
                        lvTask.Items.Add(new ListViewItem(items));
                        if (i % 2 != 0)
                            lvTask.Items[i].BackColor = Color.FromArgb(229, 255, 229);
                        i++;
                    }
                    lvTask.EndUpdate();
                }
                catch (Exception)
                {
                    return;
                }
            }
        }


        private void btOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void tsmiChange_Click(object sender, EventArgs e)
        {
            if (lvTask.SelectedIndices == null || lvTask.SelectedIndices.Count <= 0)
            {
                MessageBox.Show("请选中一行订单数据！");
                return;
            }
            FormSubTaskOperate frmSubTaskOperation = new FormSubTaskOperate(this);
            frmSubTaskOperation.strDTaskID = lvTask.SelectedItems[0].SubItems[0].Text.ToString();
            frmSubTaskOperation.strMTaskType  = lvTask.SelectedItems[0].SubItems[1].Text.ToString();
            frmSubTaskOperation.strDTaskType = lvTask.SelectedItems[0].SubItems[2].Text.ToString();
            frmSubTaskOperation.strStep = lvTask.SelectedItems[0].SubItems[3].Text.ToString();
            frmSubTaskOperation.strFromUnit = lvTask.SelectedItems[0].SubItems[4].Text.ToString();
            frmSubTaskOperation.strToUnit = lvTask.SelectedItems[0].SubItems[5].Text.ToString();
            frmSubTaskOperation.strDevice = lvTask.SelectedItems[0].SubItems[6].Text.ToString();
            frmSubTaskOperation.strChannel = lvTask.SelectedItems[0].SubItems[7].Text.ToString();
            frmSubTaskOperation.strStatus = lvTask.SelectedItems[0].SubItems[8].Text.ToString();
            frmSubTaskOperation.ShowDialog();
        }
    }
}
