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
    public partial class FormDeviceStatus : Form
    {
        public ConnectPool dbConn;
        public frmMain mainfrm;
        public FormDeviceStatus( frmMain mainfrm)
        {
            this.mainfrm = mainfrm;
            this.dbConn = mainfrm.dbConn;
            InitializeComponent();
            
        }



        private void FormDeviceStatus_Load(object sender, EventArgs e)
        {
            if (dbConn == null)
                return;
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                try
                {
                    string strSQL = "select device_id,use_status from td_stack_dic order by device_id";
                    DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        switch (int.Parse(row["device_id"].ToString()))
                        {
                            case 1001:
                                if (int.Parse(row["use_status"].ToString()) == 1)
                                    rbAvailabel1.Checked = true;
                                else
                                    rbStop1.Checked = true;
                                break;
                            case 1002:
                                if (int.Parse(row["use_status"].ToString()) == 1)
                                    rbAvailabel2.Checked = true;
                                else
                                    rbStop2.Checked = true;
                                break;
                            case 1003:
                                if (int.Parse(row["use_status"].ToString()) == 1)
                                    rbAvailabel3.Checked = true;
                                else
                                    rbStop3.Checked = true;
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
                
        }



        private void btChange1_Click(object sender, EventArgs e)
        {
            if (dbConn == null)
                return;
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                string strSQL;
                try
                {
                    if (rbAvailabel1.Checked)
                        strSQL = "update td_stack_dic set use_status=1 where device_id=1001";
                    else
                        strSQL = "update td_stack_dic set use_status=2 where device_id=1001";

                    if (DataBase.MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL) != 0)
                    {
                        MessageBox.Show("状态修改成功");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
                
        }

        private void btChange2_Click(object sender, EventArgs e)
        {
            if (dbConn == null)
                return;
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                string strSQL;
                try
                {
                    if (rbAvailabel2.Checked)
                        strSQL = "update td_stack_dic set use_status=1 where device_id=1002";
                    else
                        strSQL = "update td_stack_dic set use_status=2 where device_id=1002";
                    if (DataBase.MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL) != 0)
                    {
                        MessageBox.Show("状态修改成功");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
                
        }

        private void btChange3_Click(object sender, EventArgs e)
        {
            if (dbConn == null)
                return;
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                string strSQL;
                try
                {
                    if (rbAvailabel3.Checked)
                        strSQL = "update td_stack_dic set use_status=1 where device_id=1003";
                    else
                        strSQL = "update td_stack_dic set use_status=2 where device_id=1003";
                    if (DataBase.MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL) != 0)
                    {
                        MessageBox.Show("状态修改成功");
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
