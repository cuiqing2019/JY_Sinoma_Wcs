using DataBase;
using JY_Sinoma_WCS;
using Microsoft.Win32;
using MySql.Data.MySqlClient;
using ResgisterSystem;
using rootnamespace;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JY_Sinoma_WCS
{
    public partial class FrmDlog : Form
    {
        /// <summary>
        /// 获取后台系统时间
        /// </summary>
        public static string sysdatetime;
        public static int returns;
        public FrmDlog()
        {
            InitializeComponent();
         
        }
        private ConnectPool dbConn;
        string systime;

      


        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                string zhuce = AES.DesDecrypt(jihuoma.Text);//激活码解析
                systime = sysdate();
                string jiqima = zhuce.Substring(0, 24);
                string shijima = zhuce.Substring(24, 17);
                DateTime historyTime = Convert.ToDateTime(shijima);
                DateTime xitongTime = Convert.ToDateTime(systime);
                TimeSpan ts = historyTime - xitongTime;
                int time = int.Parse(ts.Days.ToString());
                if (time <= 0)
                {
                    MessageBox.Show("使用权限已到期，请联系管理员！");
                }
                else
                {
                    returns = 1;
                    MessageBox.Show("剩余" + time + "天!");
                    frmMain fm = new frmMain();
                    fm.Show();
                    this.Hide();
                }

                dbConn = new ConnectPool(DataBase.MySqlHelper.LocalConnectionString, 100, 50, 60);
                using (MySqlConnection conn = dbConn.GetConnectFromPool())
                {

                    if (conn == null)
                    {
                        MessageBox.Show("注入时间失败，请联系管理员！");
                    }
                    string strSQL = "update td_users  set create_time='" + shijima + "' where id=1 ";
                    string strSQL1 = "select user_zucema  from td_users ";
                    DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                    DataSet ds2 = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL1);
                    if (ds2.Tables[0].Rows[0]["user_zucema"].ToString()== "")
                    {
                         strSQL = "update td_users  set user_zucema='"+ jihuoma.Text+"' where id=1 ";
                        DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                    }
                    



                }
            }
            catch (Exception EX)
            {

                jihuoma.Clear();
                MessageBox.Show("注入时间失败，请联系管理员！");
            }
           


        }
        public string GetRegistDataSource(string name)
        {
            string registData;
            try
            {
                RegistryKey hkml = Registry.CurrentUser;
                RegistryKey software = hkml.OpenSubKey("Software", true);
                RegistryKey aimdir = software.OpenSubKey("System.Runtime.InteropServices", true);
                RegistryKey mytest = aimdir.OpenSubKey("System.Runtime.InteropServices.InAttribute.Root", true);
                registData = mytest.GetValue(name).ToString();
                return registData;
            }
            catch (Exception ex)
            {
                ex.ToString();
                return "";
            }
          
        }
  

        private void FrmDlog_Load(object sender, EventArgs e)
        {
            textBox1.Text = SaoMiaoYingJian.CreatSerialNumber_AES();//.CreatSerialNumber();
         
            sysdate();
            zucema();

         }
        public string sysdate() {
            dbConn = new ConnectPool(DataBase.MySqlHelper.LocalConnectionString, 100, 50, 60);
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {

                if (conn == null)
                {
                    MessageBox.Show("连接数据库失败，请联系管理员！");
                }
                   
                try
                {
                    string strSQL = "select DATE_FORMAT(NOW(),'%Y/%m/%d %H:%i:%s') as sysdate from dual ";
                    DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                    string   nReturn = ds.Tables[0].Rows[0]["sysdate"].ToString();
                    return nReturn;
                }
                catch (Exception)
                {
                    return null;
                }
            }
        }
        public void colostime()
        {
            dbConn = new ConnectPool(DataBase.MySqlHelper.LocalConnectionString, 100, 50, 60);
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                try
                {
                    string sql = "select create_time from td_users";
                    DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
                    string nReturn = ds.Tables[0].Rows[0]["create_time"].ToString();
                    string strSQL = "select DATE_FORMAT(NOW(),'%Y/%m/%d %H:%i:%s') as sysdate from dual ";
                    ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                    string nReturn2 = ds.Tables[0].Rows[0]["sysdate"].ToString();
                    DateTime historyTime = Convert.ToDateTime(nReturn);
                    DateTime xitongTime = Convert.ToDateTime(nReturn2);
                    TimeSpan ts = historyTime - xitongTime;
                    int time = int.Parse(ts.Days.ToString());
                    if (time <= 0)
                    {
                        string sqlc = "update td_users  set user_zucema=0 where id=1";
                        DataSet ds1 = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, sqlc);
                        System.Environment.Exit(0);
                    }

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            colostime();
        }
        public void zucema() {
            dbConn = new ConnectPool(DataBase.MySqlHelper.LocalConnectionString, 100, 50, 60);
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {

                if (conn == null)
                {
                    MessageBox.Show("连接数据库失败，请联系管理员！");
                }

                try
                {
                    string strSQL = "select user_zucema  from td_users ";
                    DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                     jihuoma.Text = ds.Tables[0].Rows[0]["user_zucema"].ToString();
                  
                }
                catch (Exception)
                {
                    MessageBox.Show("获取注册码失败");
                }
            }
        }
    }
}
