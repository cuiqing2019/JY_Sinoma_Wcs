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
    public partial class FormPortSet : Form
    {
        private frmMain mainFrm;
        public ConnectPool dbConn;
        public FormPortSet(frmMain mainFrm)
        {
            InitializeComponent();
            this.mainFrm = mainFrm;
            dbConn = mainFrm.dbConn;
        }

        public void RefreshListViewAll()
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                try
                {
                    lvPort.BeginUpdate();
                    lvPort.Items.Clear();
                    int i = 0;
                    string strSQL = "select port_id,port_desc,use_status,task_type,level_num from td_inport_dic order by id";
                    DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string[] items = new string[lvPort.Columns.Count];
                        items[0] = row["port_id"].ToString();
                        items[1] = row["port_desc"].ToString();
                        items[2] = row["use_status"].ToString() == "1" ? "启用" : "停用";
                        items[3] = DecodeTaskType(row["task_type"].ToString());
                        items[4] = row["level_num"].ToString();

                        lvPort.Items.Add(new ListViewItem(items));
                        if (i % 2 != 0)
                            lvPort.Items[i].BackColor = Color.FromArgb(229, 255, 229);
                        i++;
                    }
                    lvPort.EndUpdate();

                }
                catch(Exception ex)
                {
                    MessageBox.Show("获取出入库口设置信息失败：" + ex.Message);
                }
            }

        }
        private string DecodeTaskType(string strType)
        {
            switch(strType)
            {
                case "1":
                    return "入库";
                case "2":
                    return "出库";
                case "3":
                    return "空托入库";
                case "4":
                    return "退库";
                case "5":
                    return "异常回库";
                default:
                    return "未知任务类型";
            }

        }
        private void FormPortSet_Load(object sender, EventArgs e)
        {
            lvPort.Columns.Add("编号", (int)(lvPort.Width * 0.2), HorizontalAlignment.Center);
            lvPort.Columns.Add("出入库口描述", (int)(lvPort.Width * 0.2), HorizontalAlignment.Center);
            lvPort.Columns.Add("状态", (int)(lvPort.Width * 0.2), HorizontalAlignment.Center);
            lvPort.Columns.Add("任务类型", (int)(lvPort.Width * 0.2), HorizontalAlignment.Center);
            lvPort.Columns.Add("所在层", (int)(lvPort.Width * 0.2), HorizontalAlignment.Center);
            this.ContextMenuStrip = this.cmsStatus;
            RefreshListViewAll();
        }
        #region 修改任务类型
        private void tsmiChangeTaskType_Click(object sender, EventArgs e)
        {
            if(lvPort.SelectedItems[0].SubItems[2].Text.ToString()=="启用")
            {
                FormTaskType frmTaskType = new FormTaskType(mainFrm, lvPort.SelectedItems[0].SubItems[0].Text.ToString(), lvPort.SelectedItems[0].SubItems[1].Text.ToString(), lvPort.SelectedItems[0].SubItems[4].Text.ToString(), lvPort.SelectedItems[0].SubItems[3].Text.ToString());
                frmTaskType.ShowDialog();
                RefreshListViewAll();
            }
            else
            {
                MessageBox.Show("该出入库口为停用状态，请先启用在进行任务类型修改");
            }
         
        }
        #endregion


        #region 修改可用性
        private void tsmiChangeUnitStatus_Click(object sender, EventArgs e)
        {
            string strSQL = string.Empty;
            if (lvPort.SelectedIndices != null && lvPort.SelectedIndices.Count > 0)
            {
                using (MySqlConnection conn = dbConn.GetConnectFromPool())
                {
                    if (conn == null)
                        return;
                    try
                    {
                        int nPort = int.Parse(lvPort.SelectedItems[0].SubItems[0].Text.ToString());
                        string strStatus = lvPort.SelectedItems[0].SubItems[2].Text.ToString() == "启用" ? "1" : "2";
                        if (nPort == 5)
                        {
                            if (strStatus == "1")
                            {
                                MessageBox.Show("出库口不可禁用");
                                return;
                            }
                            else
                            {
                                strSQL = "update td_inport_dic set use_status=1 where port_id=5";
                                if (DataBase.MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL) != 0)
                                {
                                    MessageBox.Show("状态修改成功");
                                }
                            }

                        }
                        if (nPort == 1)
                        {
                            if (strStatus == "2")
                            {
                                DialogResult dialogResult = MessageBox.Show("若启用一号入库口，2-4号入库口将停用，确认要启用一号入库口？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                if (dialogResult == DialogResult.No)
                                    return;
                                else
                                {
                                    strSQL = "UPDATE TD_INPORT_DIC SET USE_STATUS=1 WHERE PORT_ID=1";
                                    if (DataBase.MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL) != 0)
                                    {
                                        strSQL = "UPDATE TD_INPORT_DIC SET USE_STATUS=2 WHERE PORT_ID IN(2,3,4)";
                                        if (DataBase.MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL) != 0)
                                            MessageBox.Show("状态修改成功");
                                    }

                                }
                            }
                            else
                            {
                                DialogResult dialogResult = MessageBox.Show("确定要停用一号入库口？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                                if (dialogResult == DialogResult.No)
                                    return;
                                else
                                {
                                    strSQL = "UPDATE TD_INPORT_DIC SET USE_STATUS=2 WHERE PORT_ID=1";
                                    if (DataBase.MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL) != 0)
                                    {
                                        MessageBox.Show("状态修改成功");
                                    }
                                }

                            }

                        }
                        if (nPort == 2 || nPort == 3 || nPort == 4)
                        {
                            if (strStatus == "2")//改启用
                            {
                                strSQL = "select count(1) from td_inport_dic where port_id=1 and use_status=1";
                                DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                                int isUse = int.Parse(ds.Tables[0].Rows[0]["count(1)"].ToString());
                                if (isUse > 0)
                                {
                                    MessageBox.Show("请先停用一号入库口！");
                                    return;
                                }
                                else
                                {
                                    strSQL = "update td_inport_dic set use_status=1 where port_id=" + nPort;
                                    if (DataBase.MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL) != 0)
                                    {
                                        MessageBox.Show("状态修改成功");
                                    }
                                }
                            }
                            else
                            {
                                strSQL = "select count(1) from td_inport_dic where port_id <5 and port_id<>" + nPort + " and use_status=1";
                                DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                                int isUse = int.Parse(ds.Tables[0].Rows[0]["count(1)"].ToString());
                                if (isUse > 0)
                                {
                                    strSQL = "update td_inport_dic set use_status=2 where port_id=" + nPort;
                                    if (DataBase.MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL) != 0)
                                    {
                                        MessageBox.Show("状态修改成功");
                                    }
                                }
                                else
                                {
                                    MessageBox.Show("请至少保留一个可用的入库口");
                                    return;
                                }
                            }

                        }
                        RefreshListViewAll();

                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("修改出入库口状态失败：" + ex.Message);
                    }
                }
            }
            else
                MessageBox.Show("请选中一行数据！");
        }
    }
    #endregion

}
