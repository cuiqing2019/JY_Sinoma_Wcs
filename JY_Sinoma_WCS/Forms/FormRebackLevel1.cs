using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Data.SqlClient;
using DataBase;
using Sinoma_WCS;
using MySql.Data.MySqlClient;

namespace JY_Sinoma_WCS
{
    public partial class FormRebackLevel1 : Form
    {
        frmMain mainFrm;
        public int nLevel;
        public FormRebackLevel1(frmMain mainFrm,int nLevel)
        {
            this.mainFrm = mainFrm;
            InitializeComponent();
            this.nLevel = nLevel;
            lvTask_init();
        }

        #region 初始化lvTask
        public void lvTask_init()
        {
            lvTask.Columns.Add("序号", (int)(lvTask.Width * 0.02), HorizontalAlignment.Center);
            lvTask.Columns.Add("任务号", (int)(lvTask.Width * 0.1), HorizontalAlignment.Center);
            lvTask.Columns.Add("任务类型", (int)(lvTask.Width * 0.05), HorizontalAlignment.Center);
            lvTask.Columns.Add("起始地址", (int)(lvTask.Width * 0.1), HorizontalAlignment.Center);
            lvTask.Columns.Add("目的地址", (int)(lvTask.Width * 0.1), HorizontalAlignment.Center);
            lvTask.Columns.Add("托盘条码", (int)(lvTask.Width * 0.2), HorizontalAlignment.Center);
            lvTask.Columns.Add("任务状态", (int)(lvTask.Width * 0.1), HorizontalAlignment.Center);
            lvTask.Columns.Add("废物批次", (int)(lvTask.Width * 0.2), HorizontalAlignment.Center);
            lvTask.Columns.Add("废物SKU", (int)(lvTask.Width * 0.2), HorizontalAlignment.Center);
            lvTask.Columns.Add("创建时间", (int)(lvTask.Width * 0.2), HorizontalAlignment.Center);
            lvTask.Columns.Add("废物包装", (int)(lvTask.Width * 0.1), HorizontalAlignment.Center);
            lvTask.Columns.Add("任务层", (int)(lvTask.Width * 0.05), HorizontalAlignment.Center);
            lvTask.Columns.Add("接运单明显ID", (int)(lvTask.Width * 0.2), HorizontalAlignment.Center);
            lvTask.Columns.Add("废物重量", (int)(lvTask.Width * 0.2), HorizontalAlignment.Center);
        }
        #endregion

        private string goodsKindsInt(string subItem)
        {
            switch (subItem)
            {
                case "1": return "吨桶";
                case "2": return "圆桶";
                default:
                    return "未知";
            };
        }

        private int DecodeGoodsKind(string strKind)
        {
            switch (strKind)
            {
                case "吨桶": return 1;
                case "圆桶": return 2;
                default:
                    return 0;
            };
        }

        #region 页面加载
        private void FormRebackLevel1_Load(object sender, EventArgs e)
        {
            lvTask.Items.Clear();
            int i = 0;
            DataSet ds = DataBaseInterface.SelectRebackTaskM();
            foreach (DataRow item in ds.Tables[0].Rows)
            {
                i++;
                string[] task = new string[lvTask.Columns.Count];
                task[0] = i.ToString();
                task[1] = item["task_id"].ToString();
                task[2] ="异常回库";
                task[3] = item["from_unit"].ToString();
                task[4] = item["to_unit"].ToString();
                task[5] = item["box_barcode"].ToString();
                task[6] = DecodeTaskStatus(item["task_status"].ToString());
                task[7] = item["batch_no"].ToString();
                task[8] = item["sku"].ToString();
                task[9] = item["create_time"].ToString();
                task[10] = goodsKindsInt(item["goods_kind"].ToString());
                task[11] = item["task_level"].ToString();
                task[12] = item["batch_id"].ToString();
                task[13] = item["goods_weight"].ToString();
                ListViewItem lv = new ListViewItem(task);
                lvTask.Items.Add(lv);
            }
            cmbInPort.SelectedIndex = 0;
        }

        private string DecodeTaskStatus(string strStatus)
        {
            switch(strStatus)
            {
                case "0":
                    return "新生成";
                case "1":
                    return "执行中";
                case "2":
                    return "已完成";
                default:
                    return "未知状态";
            }
        }

        #endregion

        private void lvTask_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (lvTask.SelectedIndices != null && lvTask.SelectedIndices.Count > 0)
            {
                tbTaskId.Text= lvTask.SelectedItems[0].SubItems[1].Text.ToString();
                tbBoxBarcode.Text = lvTask.SelectedItems[0].SubItems[5].Text.ToString();
                tbBatchId.Text= lvTask.SelectedItems[0].SubItems[12].Text.ToString();
                tbSku.Text= lvTask.SelectedItems[0].SubItems[8].Text.ToString();
                tbBatchNo.Text= lvTask.SelectedItems[0].SubItems[7].Text.ToString();
                tbGoodsWeight.Text= lvTask.SelectedItems[0].SubItems[13].Text.ToString();
                cbWasteKinds.SelectedText= lvTask.SelectedItems[0].SubItems[10].Text.ToString();
            }
            else
                MessageBox.Show("请选中一行数据！");
        }

        private void btnGetTask_Click(object sender, EventArgs e)
        {
            if(tbGoodName.Text.Length==0)
            {
                MessageBox.Show("请填写废物名称");
                return;
            }
            if(tbHazardArea.Text.Length==0)
            {
                MessageBox.Show("请填写入库危险分区");
                return;
            }
            if(cmbInPort.SelectedIndex==0)
            {
                MessageBox.Show("请选择异常回库口");
                return;
            }
            else
            {
                string str = cbWasteKinds.SelectedText;
                using (MySqlConnection conn = mainFrm.dbConn.GetConnectFromPool())
                {
                    int strr = cbWasteKinds.SelectedIndex;
                    //验证入库口是否为异常回库模式
                    DataSet ds = DataBaseInterface.SelectInPort(conn,cmbInPort.SelectedIndex);
                    if (ds.Tables[0].Rows[0]["use_status"].ToString() == "2")
                    {
                        MessageBox.Show(cmbInPort.SelectedIndex + "号入库口为停用状态，不可选为异常回库口");
                        return;
                    }
                    if (ds.Tables[0].Rows[0]["task_type"].ToString() != "5")
                    {
                        MessageBox.Show("请先将" + cmbInPort.SelectedIndex + "号入库口切换到异常回库模式");
                        return;
                    }
                    string spError = string.Empty;
                    int nReaturn = DataBaseInterface.CreatInboundTaskM(conn,tbBoxBarcode.Text.Trim(), 5, tbBatchNo.Text.Trim(), tbBatchId.Text.Trim(),cbWasteKinds.SelectedIndex+1, tbSku.Text.Trim(), double.Parse(tbGoodsWeight.Text.Trim()), 1, int.Parse(tbTaskId.Text.Trim()), 0, tbGoodName.Text.Trim(), tbHazardArea.Text.Trim(), cmbInPort.SelectedIndex, out spError);
                    if (nReaturn == 1)
                    {
                        MessageBox.Show("异常回库任务生成成功");
                    }
                    else
                    {
                        MessageBox.Show("异常回库任务生成失败");
                    }
                }
                   
            }

        }
    }
}
