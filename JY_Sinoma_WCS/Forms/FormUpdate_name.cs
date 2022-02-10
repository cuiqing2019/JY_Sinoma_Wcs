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
    public partial class FormUpdate_name : Form
    {
        public ConnectPool dbConn;//定义数据库连接
        public string sql = "";
        string names;
        string goodsname;
        string character;
        string color;
        public FormUpdate_name(string names,string goodsname,string character,string color)
        {
            
            
            this.names= names;
            this.goodsname = goodsname;
            this.character = character;
            this.color= color;

            InitializeComponent();
        }

        private void FormUpdate_name_Load(object sender, EventArgs e)
        {
            textBox1.Text = this.names;
            textBox2.Text = this.goodsname;
            textBox3.Text = this.character;
            textBox4.Text = this.color;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dbConn = new ConnectPool(DataBase.MySqlHelper.LocalConnectionString, 100, 50, 60);
            if (dbConn == null)
                return;
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                try
                {

                 //   sql = "update  td_plt_location_dic set batch_id='"+textBox1.Text+ "',batch_no='"+textBox2.Text+ "',goods_sku ='" + textBox3.Text+"',goods_name='"+textBox4.Text+"'where batch_id='" + names+"'";
                    sql = "update  td_plt_location_dic set BATCH_NO='" + textBox1.Text + "' ,BATCH_ID='" + textBox2.Text + "' ,GOODS_SKU='" + textBox3.Text + "',GOODS_NAME='" + textBox4.Text + "' where BATCH_ID='" + goodsname + "'";
                    if (DataBase.MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, sql) != 0)
                    {
                        MessageBox.Show("状态修改成功");
                        this.Close();
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
