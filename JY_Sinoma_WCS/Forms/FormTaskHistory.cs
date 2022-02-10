using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using DataBase;
using Sinoma_WCS;
using MySql.Data.MySqlClient;

namespace JY_Sinoma_WCS
{
    public partial class FormTaskHistory : Form
    {
        public DataSet ds = null;
        private frmMain mainFrm;
        public ConnectPool dbConn;//定义数据库连接
        public string strselect = "";
        List<KeyValuePair<int, string>> listItem1 = new List<KeyValuePair<int, string>>();
        public FormTaskHistory(frmMain mainFrm)
        {
            ThreadExceptionDialog.CheckForIllegalCrossThreadCalls = false;//允许直接访问线程之间的控件
            this.mainFrm = mainFrm;
            dbConn = mainFrm.dbConn;
            InitializeComponent();
        }

        public void showTaskType()
        {
            listItem1.Clear();
            listItem1.Add(new KeyValuePair<int, string>(0, "--选择全部--"));
            listItem1.Add(new KeyValuePair<int, string>(1, "入库"));
            listItem1.Add(new KeyValuePair<int, string>(2, "出库"));
            listItem1.Add(new KeyValuePair<int, string>(3, "空托入库"));
            listItem1.Add(new KeyValuePair<int, string>(4, "退库"));
            listItem1.Add(new KeyValuePair<int, string>(5, "异常回库"));
            cmbTaskType.DataSource = listItem1;
            cmbTaskType.DisplayMember = "value";
            cmbTaskType.ValueMember = "key";
            // cmbTaskType.SelectedItem = 0;
        }

        private void FormTaskHistory_Load(object sender, EventArgs e)
        {
            dtpStart.Value = DateTime.Now.AddDays(-1);
            showTaskType();
            InitLV();
            comboBox1.SelectedIndex = 0;

        }

        #region 初始化listView
        /// <summary>
        /// 初始化listView
        /// </summary>
        private void InitLV()
        {
            lvContainer.Columns.Add("任务号", (int)(lvContainer.Width * 0.10), HorizontalAlignment.Center);
            lvContainer.Columns.Add("任务类型", (int)(lvContainer.Width * 0.08), HorizontalAlignment.Center);
            lvContainer.Columns.Add("层", (int)(lvContainer.Width * 0.03), HorizontalAlignment.Center);
            lvContainer.Columns.Add("托盘条码", (int)(lvContainer.Width * 0.1), HorizontalAlignment.Center);
            lvContainer.Columns.Add("重量", (int)(lvContainer.Width * 0.06), HorizontalAlignment.Center);
            lvContainer.Columns.Add("状态", (int)(lvContainer.Width * 0.07), HorizontalAlignment.Center);
            lvContainer.Columns.Add("任务接收时间", (int)(lvContainer.Width * 0.15), HorizontalAlignment.Center);
            lvContainer.Columns.Add("任务执行时间", (int)(lvContainer.Width * 0.15), HorizontalAlignment.Center);
            lvContainer.Columns.Add("废料包装", (int)(lvContainer.Width * 0.1), HorizontalAlignment.Center);
            lvContainer.Columns.Add("废物性状", (int)(lvContainer.Width * 0.1), HorizontalAlignment.Center);
            lvContainer.Columns.Add("废品单位", (int)(lvContainer.Width * 0.1), HorizontalAlignment.Center);
            lvContainer.Columns.Add("废物名称", (int)(lvContainer.Width * 0.1), HorizontalAlignment.Center);


            //this.ContextMenuStrip = this.cmsTask;
            RefreshListView();
            //tbContainer.Text = "ZX0";
        }
        #endregion

        private void btnQuery_Click(object sender, EventArgs e)
        {
            RefreshListView();
        }

        #region 刷新ListView
        public void RefreshListView()
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                string barcode = txtBarcode.Text.ToString().Trim();
                int taskType = int.Parse(cmbTaskType.SelectedValue.ToString());//0--选择全部--；1入库；2出库；3空托入库；4退库；5异常回库；
                string strSQL = "";

                strSQL = "select * from tb_plt_task_m where 1=1";
                if (taskType > 0)
                    strSQL += " and TASK_TYPE=" + taskType + "";
                if (comboBox1.SelectedIndex == 0)
                    strSQL += " and TASK_STATUS in(0,1,2,3,4)";
                if (comboBox1.SelectedIndex ==1)
                    strSQL += " and TASK_STATUS=0";
                if (comboBox1.SelectedIndex==2)
                    strSQL += " and TASK_STATUS=1";
                if (comboBox1.SelectedIndex ==3)
                    strSQL += " and TASK_STATUS=2";
                if (barcode != "")
                    strSQL += " and BOX_BARCODE like'%" + barcode + "%'";
                if (textname.Text != "")
                {
                    strSQL += " and batch_id like'%" + textname.Text + "%'";
                }
                  
                if (txtTaskId.Text.Trim().Length != 0)
                {
                    strSQL += " and TASK_ID ='" + txtTaskId.Text.Trim().ToString() + "'";
                }
                strSQL += " and CREATE_TIME>= str_to_date('" + dtpStart.Text.ToString() + "','%Y-%m-%d %H:%i:%s') and CREATE_TIME<= str_to_date('" + dtpEnd.Text.ToString() + "','%Y-%m-%d %H:%i:%s')  GROUP BY BOX_BARCODE  order by CREATE_TIME desc";

                int i = 0;
                try
                {
                    DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                    i = UpdateListview(i, ds);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                   
                }
            }
        }
        private int UpdateListview(int i, DataSet ds)
        {
            int count = 0;
            lvContainer.BeginUpdate();
            lvContainer.Items.Clear();
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                string[] items = new string[lvContainer.Columns.Count];
                items[0] = row["TASK_ID"].ToString();
                items[1] = this.mainFrm.DecodeMTaskType(int.Parse(row["TASK_TYPE"].ToString()));
                items[2] = row["TASK_LEVEL"].ToString();

                items[3] = row["BOX_BARCODE"].ToString();
                items[4] = row["GOODS_WEIGHT"].ToString();
                items[5] = this.mainFrm.DecodeMTaskStatus(int.Parse(row["TASK_STATUS"].ToString()));
                items[6] = row["CREATE_TIME"].ToString();
                items[7] = row["BEGIN_TIME"].ToString();
                items[8] = GoodsKinds(row["GOODS_KIND"].ToString());
                items[9] = row["SKU"].ToString();
                items[10] = row["BATCH_NO"].ToString();
                items[11] = row["BATCH_ID"].ToString();

                lvContainer.Items.Add(new ListViewItem(items));
                if (i % 2 != 0)
                    lvContainer.Items[i].BackColor = Color.FromArgb(229, 255, 229);
                i++;
                count++;
            }
            lvContainer.EndUpdate();
            txtTaskCount.Text = count.ToString();
            return i;

        }
        #endregion


        public string GoodsKinds(string goodsKinds)
        {
            if (goodsKinds == "1")
                return "吨桶";
            else if (goodsKinds == "2")
                return "圆桶";
            else if (goodsKinds == "3")
                return "空托盘";
            return "未知";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (lvContainer.Items.Count > 0)
            {
                ExportExcel("任务导出", lvContainer, 0);
            }
            else
                MessageBox.Show("没有可导出的数据！");
        }
        public void ExportExcel(string fileName, System.Windows.Forms.ListView listView, int titleRowCount)
        {
            string saveFileName = "出入库任务";
            //bool fileSaved = false;
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "xls";
            saveDialog.Filter = "Excel文件|*.xlsx";
            saveDialog.FileName = fileName;
            saveDialog.ShowDialog();
            saveFileName = saveDialog.FileName;
            if (saveFileName.IndexOf(":") < 0) return; //被点了取消 
            Microsoft.Office.Interop.Excel.Application xlApp;
            try
            {
                xlApp = new Microsoft.Office.Interop.Excel.Application();
            }
            catch (Exception)
            {
                MessageBox.Show("无法创建Excel对象，可能您的机子未安装Excel");
                return;
            }
            finally
            {
            }
            Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
            Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
            Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1
            //写Title
            //if (titleRowCount != 0)
            //  MergeCells(worksheet, 1, 1, titleRowCount, listView.Columns.Count, listView.Tag.ToString());
            //写入列标题
            for (int i = 0; i <= listView.Columns.Count - 1; i++)
            {
                worksheet.Cells[titleRowCount + 1, i + 1] = listView.Columns[i].Text;
            }
            //写入数值
            for (int r = 0; r <= listView.Items.Count - 1; r++)
            {
                for (int i = 0; i <= listView.Columns.Count - 1; i++)
                {
                    worksheet.Cells[r + titleRowCount + 2, i + 1] = listView.Items[r].SubItems[i].Text;
                }
                System.Windows.Forms.Application.DoEvents();
            }
            worksheet.Columns.EntireColumn.AutoFit();//列宽自适应
            //if (Microsoft.Office.Interop.cmbxType.Text != "Notification")
            //{
            // Excel.Range rg = worksheet.get_Range(worksheet.Cells[2, 2], worksheet.Cells[ds.Tables[0].Rows.Count + 1, 2]);
            // rg.NumberFormat = "00000000";
            //}
            if (saveFileName != "")
            {
                try
                {
                    workbook.Saved = true;
                    workbook.SaveCopyAs(saveFileName);
                    //fileSaved = true;
                }
                catch (Exception ex)
                {
                    //fileSaved = false;
                    MessageBox.Show("导出文件时出错,文件可能正被打开！n" + ex.Message);
                }
            }
            //else
            //{
            // fileSaved = false;
            //}
            xlApp.Quit();
            GC.Collect();//强行销毁 
            // if (fileSaved && System.IO.File.Exists(saveFileName)) System.Diagnostics.Process.Start(saveFileName); //打开EXCEL
            MessageBox.Show(fileName + "导出到Excel成功", "提示", MessageBoxButtons.OK);
        }
        /// <summary>
        /// DataTable导出到Excel
        /// </summary>
        /// <param name="fileName">默认的文件名</param>
        /// <param name="dataTable">数据源,一个DataTable数据表</param>
        /// <param name="titleRowCount">标题占据的行数，为0则表示无标题</param>
        public void ExportExcel(string fileName, System.Data.DataTable dataTable, int titleRowCount)
        {
            string saveFileName = "";
            //bool fileSaved = false;
            SaveFileDialog saveDialog = new SaveFileDialog();
            saveDialog.DefaultExt = "xls";
            saveDialog.Filter = "Excel文件|*.xls";
            saveDialog.FileName = fileName;
            saveDialog.ShowDialog();
            saveFileName = saveDialog.FileName;
            if (saveFileName.IndexOf(":") < 0) return; //被点了取消 
            Microsoft.Office.Interop.Excel.Application xlApp;
            try
            {
                xlApp = new Microsoft.Office.Interop.Excel.Application();
                Microsoft.Office.Interop.Excel.Workbooks workbooks = xlApp.Workbooks;
                Microsoft.Office.Interop.Excel.Workbook workbook = workbooks.Add(Microsoft.Office.Interop.Excel.XlWBATemplate.xlWBATWorksheet);
                Microsoft.Office.Interop.Excel.Worksheet worksheet = (Microsoft.Office.Interop.Excel.Worksheet)workbook.Worksheets[1];//取得sheet1
                //写Title
                if (titleRowCount != 0)
                    MergeCells(worksheet, 1, 1, titleRowCount, dataTable.Columns.Count, dataTable.TableName);
                //写入列标题
                for (int i = 0; i <= dataTable.Columns.Count - 1; i++)
                {
                    worksheet.Cells[titleRowCount + 1, i + 1] = dataTable.Columns[i].ColumnName;
                }
                //写入数值
                for (int r = 0; r <= dataTable.Rows.Count - 1; r++)
                {
                    for (int i = 0; i <= dataTable.Columns.Count - 1; i++)
                    {
                        worksheet.Cells[r + titleRowCount + 2, i + 1] = dataTable.Rows[r][i].ToString();
                    }
                    System.Windows.Forms.Application.DoEvents();
                }
                worksheet.Columns.EntireColumn.AutoFit();//列宽自适应
                //if (Microsoft.Office.Interop.cmbxType.Text != "Notification")
                //{
                // Excel.Range rg = worksheet.get_Range(worksheet.Cells[2, 2], worksheet.Cells[ds.Tables[0].Rows.Count + 1, 2]);
                // rg.NumberFormat = "00000000";
                //}
                if (saveFileName != "")
                {
                    try
                    {
                        workbook.Saved = true;
                        workbook.SaveCopyAs(saveFileName);
                        //fileSaved = true;
                    }
                    catch (Exception ex)
                    {
                        //fileSaved = false;
                        MessageBox.Show("导出文件时出错,文件可能正被打开！n" + ex.Message);
                    }
                }
                //else
                //{
                // fileSaved = false;
                //}
                xlApp.Quit();
                GC.Collect();//强行销毁 
                // if (fileSaved && System.IO.File.Exists(saveFileName)) System.Diagnostics.Process.Start(saveFileName); //打开EXCEL
                MessageBox.Show(fileName + "导出到Excel成功", "提示", MessageBoxButtons.OK);
            }
            catch (Exception)
            {
                MessageBox.Show("无法创建Excel对象，可能您的机子未安装Excel");
                return;
            }
            finally
            {
            }

        }
        /// <summary> 
        /// 合并单元格，并赋值，对指定WorkSheet操作 
        /// </summary> 
        /// <param name="sheetIndex">WorkSheet索引</param> 
        /// <param name="beginRowIndex">开始行索引</param> 
        /// <param name="beginColumnIndex">开始列索引</param> 
        /// <param name="endRowIndex">结束行索引</param> 
        /// <param name="endColumnIndex">结束列索引</param> 
        /// <param name="text">合并后Range的值</param> 
        public void MergeCells(Microsoft.Office.Interop.Excel.Worksheet workSheet, int beginRowIndex, int beginColumnIndex, int endRowIndex, int endColumnIndex, string text)
        {
            Microsoft.Office.Interop.Excel.Range range = workSheet.get_Range(workSheet.Cells[beginRowIndex, beginColumnIndex], workSheet.Cells[endRowIndex, endColumnIndex]);
            range.ClearContents(); //先把Range内容清除，合并才不会出错 
            range.MergeCells = true;
            range.Value2 = text;
            range.HorizontalAlignment = Microsoft.Office.Interop.Excel.XlHAlign.xlHAlignCenter;
            range.VerticalAlignment = Microsoft.Office.Interop.Excel.XlVAlign.xlVAlignCenter;

        }

    }
}
