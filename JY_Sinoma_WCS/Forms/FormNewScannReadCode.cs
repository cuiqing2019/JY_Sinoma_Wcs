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
using System.Collections;


namespace JY_Sinoma_WCS
{
    public partial class FormNewScannReadCode : Form
    {
        int goodsKinds;
        frmMain mainFrm;
        string scanId;
        public FormNewScannReadCode(frmMain mainFrm,int goodsKinds,string scanId)
        {
            InitializeComponent();
            this.mainFrm = mainFrm;
            this.goodsKinds = goodsKinds;
            this.scanId = scanId;
            if (goodsKinds == 3)
                this.oneBoxCode.Enabled = false;
            else
                this.groupBoxCode.Enabled = false;

        }

        private void AddScanRead_Click(object sender, EventArgs e)
        {
            if (goodsKinds == 3)
            {
                mainFrm.ReadBarCodeFromSPs[scanId].myDt.Rows.Clear();
                int i = 0;
                foreach (string str in groupBoxCode.Lines)
                {
                    if (str != string.Empty)
                    {
                        DataRow mydr = mainFrm.ReadBarCodeFromSPs[scanId].myDt.NewRow();
                        mydr["TID"] = str;
                        mainFrm.ReadBarCodeFromSPs[scanId].myDt.Rows.Add(mydr);
                        i++;
                    }
                }
                if (i < 8)
                    MessageBox.Show("未添加完成，剩余箱号继续由RFID扫描");
                else
                {
                    string rs;
                    DataBaseInterface.SaveCurrentBarcode(mainFrm.ReadBarCodeFromSPs[scanId].myDt.Rows[0]["TID"].ToString(), mainFrm.ReadBarCodeFromSPs[scanId].myDt.Rows[1]["TID"].ToString(), mainFrm.ReadBarCodeFromSPs[scanId].myDt.Rows[2]["TID"].ToString(), mainFrm.ReadBarCodeFromSPs[scanId].myDt.Rows[3]["TID"].ToString(), mainFrm.ReadBarCodeFromSPs[scanId].myDt.Rows[4]["TID"].ToString(), mainFrm.ReadBarCodeFromSPs[scanId].myDt.Rows[5]["TID"].ToString(), mainFrm.ReadBarCodeFromSPs[scanId].myDt.Rows[6]["TID"].ToString(), mainFrm.ReadBarCodeFromSPs[scanId].myDt.Rows[7]["TID"].ToString(), ((int.Parse(scanId) > 2) ? 1 : 2), int.Parse(scanId), 2, out rs);

                    if (rs == string.Empty)
                    {
                        mainFrm.ReadBarCodeFromSPs[scanId].myDt = new DataTable();
                        mainFrm.ReadBarCodeFromSPs[scanId].myDt.Columns.Add("TID", typeof(string)); //数据类型为 文本
                    }
                    return;
                }
               
            }
            if (goodsKinds != 3)
            {
                try
                {
                    string rs;
                    DataBaseInterface.SaveCurrentBarcode(oneBoxCode.Text, ((int.Parse(scanId) > 2) ? 1 : 2), int.Parse(scanId), 2, out rs);
                    if (rs == string.Empty)
                        MessageBox.Show("添加成功！");
                    else
                        MessageBox.Show("添加失败");
                   
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
    }
}
