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
    public partial class FormUpdateCount : Form
    {
        public ConnectPool dbConn;//定义数据库连接
        public string sql = "";
        public frmMain mainFrm;
        public FormUpdateCount(frmMain mainFrm)
        {
            this.mainFrm = mainFrm;
            dbConn = mainFrm.dbConn;
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
            count();

        }
        public void count() {

            if (dbConn == null)
                return;
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                try
                {

                    sql = "select empty_status from td_empty_status  where id=2 ";

                    DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
                    textBox1.Text = ds.Tables[0].Rows[0]["empty_status"].ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }

        private void FormUpdateCount_Load(object sender, EventArgs e)
        {
            count();
        }
    }
}
