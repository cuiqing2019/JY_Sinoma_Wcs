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
    public partial class FrmLocationEdit : Form
    {
        private FrmWLocation mainFrm;
        private FormLocationView vMainFrm;
        public ConnectPool dbConn;//定义数据库连接
        public string strLocation;
        public string strIsFreeze;
        public string strAvailStatus;
        public FrmLocationEdit(FrmWLocation mainFrm, string strLocation, string strAvailStatus)
        {
            this.mainFrm = mainFrm;
            this.strLocation = strLocation;
            this.strAvailStatus = strAvailStatus;
            dbConn = mainFrm.dbConn;
            InitializeComponent();
        }
        public FrmLocationEdit(FormLocationView mainFrm, string strLocation, string strAvailStatus)
        {
            this.vMainFrm = mainFrm;
            this.strLocation = strLocation;
            this.strAvailStatus = strAvailStatus;
            dbConn = vMainFrm.mainFrm.dbConn;
            InitializeComponent();
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("确认要修改库位【" + strLocation + "】的库位信息？", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                using (MySqlConnection conn = dbConn.GetConnectFromPool())
                {
                    if (conn == null)
                        return;
                    if (cmbAvailStatus.SelectedIndex < 1)
                        return;
                    string strSQL = "update td_plt_location_dic set use_status='" + (cmbAvailStatus.SelectedIndex-1).ToString() + "'where location_id='" + strLocation + "'";
                    try
                    {
                        if (DataBase.MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL) > 0)
                        {
                            MessageBox.Show("修改数据成功！");
                            if (mainFrm != null)
                                mainFrm.RefreshListViewAll();
                            if (vMainFrm != null)
                                vMainFrm.ShowLoactionLabel(vMainFrm.cmSelectLocationLevel.SelectedIndex + 1);
                            this.Close();
                        }
                        else
                            MessageBox.Show("修改数据失败！");
                    }
                    catch (Exception)
                    {
                       return;
                    }
                }
                    
            }
        }


        private void FrmLocationEdit_Load(object sender, EventArgs e)
        {
            tbLocation.Text = strLocation;
            cmbAvailStatus.Text = strAvailStatus;
        }

        private void btnExc_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
