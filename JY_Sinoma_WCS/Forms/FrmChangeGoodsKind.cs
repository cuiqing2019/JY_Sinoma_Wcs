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
    public partial class FrmChangeGoodsKind : Form
    {
        private FrmWLocation mainFrm;
        private FormLocationView vMainFrm;
        public ConnectPool dbConn;//定义数据库连接
        public string strLocation;
        public string strGoodsKind;


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
        public FrmChangeGoodsKind(FrmWLocation mainFrm, string strLocation, string strGoodsKind)
        {
            this.mainFrm = mainFrm;
            this.strLocation = strLocation;
            this.strGoodsKind = strGoodsKind;
            dbConn = mainFrm.dbConn;
            InitializeComponent();
        }
        public FrmChangeGoodsKind(FormLocationView vMainFrm, string strLocation, string strGoodsKind)
        {
            this.vMainFrm = vMainFrm;
            this.strLocation = strLocation;
            this.strGoodsKind = strGoodsKind;
            dbConn = mainFrm.dbConn;
            InitializeComponent();
        }

        private void FrmChangeGoodsKind_Load(object sender, EventArgs e)
        {
            cmbGoodsKindOld.SelectedText = strGoodsKind;
            cmbGoodsKindNew.SelectedIndex = 0;

        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("确认要修改库位【" + strLocation + "】的库位类型？", "", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                using (MySqlConnection conn = dbConn.GetConnectFromPool())
                {
                    if (conn == null)
                        return;
                    if (cmbGoodsKindNew.SelectedIndex < 1)
                        return;
                    string strSQL = "update td_plt_location_dic set goods_kinds=" + cmbGoodsKindNew.SelectedIndex + "where location_id='" + strLocation + "'";
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

      

     


        private void btnExc_Click(object sender, EventArgs e)
        {
            this.Close();
        }

     
    }
}
