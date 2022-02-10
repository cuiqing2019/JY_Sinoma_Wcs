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
    public partial class FrmChangeLocation : Form
    {
        private frmMain mainFrm;
        public ConnectPool dbConn;//定义数据库连接
        public string strLocation;//原库位
        public string strPalletBarcode;//托盘条码
        public string strBatch;
        public string strGoodsCode;//商品编号
        public string strLocateType;//货格类型
        string formType;//窗体类型

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
        public FrmChangeLocation(frmMain mainFrm, string strLocation, string strPalletBarcode, string strBatch, string strGoodsCode, string strLocateType,string formType)
        {
            this.mainFrm = mainFrm;
            this.strLocation = strLocation;
            this.strPalletBarcode = strPalletBarcode;
            this.strBatch = strBatch;
            this.strGoodsCode = strGoodsCode;
            this.strLocateType = strLocateType;
            this.formType = formType;
            dbConn = mainFrm.dbConn;
            InitializeComponent();
        }

        private void FrmChangeLocation_Load(object sender, EventArgs e)
        {
            tbContainerNo.Text = strPalletBarcode;
            tbStartLocation.Text = strLocation;
            tbBatch.Text = strBatch;
            txtGoodsCode.Text = strGoodsCode;
            txtLoacteType.Text = strLocateType;
            if (formType == "Waiting")
            {
                txtLoacteType.Enabled = false;
                txtGoodsCode.Enabled = false;
                tbBatch.Enabled = false;
            }
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            if (tbEndLocation.Text.Trim() != "")
            {
                int nReturn=LocationIsOK(tbEndLocation.Text.Trim());//确认调整库位是否可用
                if (nReturn == 1)
                {
                    try
                    {
                        string rs;
                        if (DataBaseInterface.ChangeLocation(tbStartLocation.Text, tbEndLocation.Text,formType,out rs) > 0)
                        {
                            MessageBox.Show("修改数据成功！");

                            this.Close();
                        }
                        else
                        {
                            MessageBox.Show("修改数据失败！"+rs);
                        }
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
                else
                    MessageBox.Show("该货位不可用，请核对库位状态！");
            }
            else
                MessageBox.Show("调整库位不能为空，请输入库位！");
            this.Close();
        }

        private void RefreshListView(string strLocation)
        {
            throw new NotImplementedException();
        }

        #region 验证调整库位的可用性
        public int LocationIsOK(string strLocation)
        {
            if (dbConn == null)
                return 0;
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return 0;
                try
                {
                    if (formType == "Waiting")
                    {
                        string strSQL = "select t.* from TD_AGV_WAREHOUSE_LOCATION  t where t.location_no='" + strLocation + "'";
                        DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            MessageBox.Show("库位输入有误，请重新输入");
                            return 0;
                        }
                        if (ds.Tables[0].Rows[0]["USE_STATUS"].ToString() != "0" || ds.Tables[0].Rows[0]["UNIT_STATUS"].ToString() != "0")
                            return 0;
                        else
                            return 1;
                    }
                    else if (formType == "MainFrm")
                    {
                        string strSQL = "select t.* from TD_PLT_LOCATION_DIC t where location_id='" + strLocation + "'";
                        DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                        if (ds.Tables[0].Rows.Count == 0)
                        {
                            MessageBox.Show("库位输入有误，请重新输入");
                            return 0;
                        }
                        if (ds.Tables[0].Rows[0]["USE_STATUS"].ToString() != "0" || ds.Tables[0].Rows[0]["UNIT_STATUS"].ToString() != "0" || DecodeStoreType(ds.Tables[0].Rows[0]["GOODS_KINDS"].ToString()) != strLocateType)
                            return 0;
                        else
                            return 1;
                    }
                    return 0;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return 0;
                }
            }
                
        }
        #endregion


        private void btnExc_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private string DecodeStoreType(string strType)
        {
            switch (strType)
            {
                case "0":
                    return "空货位";
                case "1":
                    return "吨桶";
                case "2":
                    return "圆桶";
                case "3":
                    return "空托盘组";
                default:
                    return "未知";
            }
        }
    }
}
