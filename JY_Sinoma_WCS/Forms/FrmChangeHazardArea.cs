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
    public partial class FrmChangeHazardArea : Form
    {
        private FrmWLocation mainFrm;
        private FormLocationView vMainFrm;
        public ConnectPool dbConn;//定义数据库连接
        public string strLocation;//原库位
        public string strHazardArea;//托盘条码
      

        /// <summary>
        /// 
        /// </summary>
        /// <param name="mainFrm">主界面</param>
        /// <param name="strLocation">原库位</param>
        /// <param name="strPalletBarcode">托盘条码</param>
        /// <param name="strBatch">批次号</param>
        /// <param name="strGoodsCode">商品编号</param>
        /// <param name="strLocateType">货位类型</param>
        /// <param name="formType">界面类型</param>
        public FrmChangeHazardArea(FrmWLocation mainFrm, string strLocation, string strHazardArea)
        {
            this.mainFrm = mainFrm;
            this.strLocation = strLocation;
            this.strHazardArea = strHazardArea;
            dbConn = mainFrm.dbConn;
            InitializeComponent();
        }

        public FrmChangeHazardArea(FormLocationView vMainFrm, string strLocation, string strHazardArea)
        {
            this.vMainFrm = vMainFrm;
            this.strLocation = strLocation;
            this.strHazardArea = strHazardArea;
            dbConn = mainFrm.dbConn;
            InitializeComponent();
        }

        private void FrmChangeHazardArea_Load(object sender, EventArgs e)
        {
            cmbAreaOld.SelectedText = strHazardArea;
            cmbAreaNew.SelectedIndex = 0;
        }

        private void btnOK_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("确认要修改库位【" + strLocation + "】的危险分区？", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                using (MySqlConnection conn = dbConn.GetConnectFromPool())
                {
                    if (conn == null)
                        return;
                    if (cmbAreaNew.SelectedIndex < 1)
                        return;
                    string strSQL = "update td_plt_location_dic set hazard_area='" + cmbAreaNew.SelectedText + "'where location_id='" + strLocation + "'";
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

        private void RefreshListView(string strLocation)
        {
            throw new NotImplementedException();
        }

     


        private void btnExc_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
