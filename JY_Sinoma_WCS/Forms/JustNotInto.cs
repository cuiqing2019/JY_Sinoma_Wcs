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

namespace JY_Sinoma_WCS
{
    public partial class JustNotInto : Form
    {
        public ConnectPool dbConn;//定义数据库连接
        public string sql = "";
        public JustNotInto()
        {
            InitializeComponent();
        }

        #region 修改堆垛机状态
        private void button1_Click(object sender, EventArgs e)
        {
            if (dbConn == null)
                return;
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                try
                {
                    if (radio1.Checked == true)
                        sql = "update td_stack_dic t set t.task_status=1 where t.device_id=1001 ";
                    else
                        sql = "update td_stack_dic t set t.task_status=2 where t.device_id=1001 ";
                    if (DataBase.MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql) > 0)
                        MessageBox.Show("1号堆垛机状态修改成功！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }   
            GetInto();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (dbConn == null)
                return;
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                try
                {
                    if (radio2.Checked == true)
                    {
                        sql = "update td_stack_dic t set t.task_status=1 where t.device_id=1002";
                    }
                    else
                    {
                        sql = "update td_stack_dic t set t.task_status=2 where t.device_id=1002";
                    }
                    if (DataBase.MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql) > 0)
                        MessageBox.Show("2号堆垛机状态修改成功！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
            GetInto();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (dbConn == null)
                return;
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                try
                {
                    if (radio3.Checked == true)
                    {
                        sql = "update td_stack_dic t set t.task_status=1 where t.device_id=1003";
                    }
                    else
                    {
                        sql = "update td_stack_dic t set t.task_status=2 where t.device_id=1003";
                    }
                    if (DataBase.MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql) > 0)
                        MessageBox.Show("3号堆垛机状态修改成功！");
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
               
            GetInto();
        }

        #endregion
        #region 获取堆垛机状态
        public void GetInto()//堆垛机状态
        {
            if (dbConn == null)
                return;
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                try
                {
                    sql = "select t.device_id, t.task_status from td_stack_dic t ";
                    DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        switch (int.Parse(row["device_id"].ToString()))
                        {
                            case 1001:
                                if (int.Parse(row["task_status"].ToString()) == 1)
                                    radio1.Checked = true;
                                else
                                    radio11.Checked = true;
                                break;
                            case 1002:
                                if (int.Parse(row["task_status"].ToString()) == 1)
                                    radio2.Checked = true;
                                else
                                    radio22.Checked = true;
                                break;
                            case 1003:
                                if (int.Parse(row["task_status"].ToString()) == 1)
                                    radio3.Checked = true;
                                else
                                    radio33.Checked = true;
                                break;
                            default:
                                break;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
               
        #endregion
        }

        private void JustNotInto_Load(object sender, EventArgs e)
        {

            GetInto();
        }
    }
}
