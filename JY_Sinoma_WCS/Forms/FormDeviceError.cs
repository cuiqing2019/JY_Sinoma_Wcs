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
    public partial class FormDeviceError : Form
    {

        public DataSet ds = null;
        public DataSet dsd = null;
        private frmMain mainFrm;
        public ConnectPool dbConn;//定义数据库连接

        public FormDeviceError(frmMain mainFrm, string de)
        {
            ThreadExceptionDialog.CheckForIllegalCrossThreadCalls = false;
            this.mainFrm = mainFrm;
            dbConn = mainFrm.dbConn;
            InitializeComponent();
        }

        private void FormDeviceError_Load(object sender, EventArgs e)
        {

            listView1.View = View.Details;
            InitLV();
            GetError();
            dtpStart.Value = DateTime.Now.AddDays(-1);
        }

        private void InitLV()
        {
            listView1.Columns.Add("序号", (int)(listView1.Width * 0.05), HorizontalAlignment.Center);
            listView1.Columns.Add("任务号", (int)(listView1.Width * 0.1), HorizontalAlignment.Center);
            listView1.Columns.Add("设备名", (int)(listView1.Width * 0.2), HorizontalAlignment.Center);
            listView1.Columns.Add("出入库类型", (int)(listView1.Width * 0.1), HorizontalAlignment.Center);
            listView1.Columns.Add("当前层", (int)(listView1.Width * 0.08), HorizontalAlignment.Center);
            listView1.Columns.Add("报警信息", (int)(listView1.Width * 0.2), HorizontalAlignment.Center);
            listView1.Columns.Add("巷道", (int)(listView1.Width * 0.05), HorizontalAlignment.Center);
            listView1.Columns.Add("报警时间", (int)(listView1.Width * 0.3), HorizontalAlignment.Center);
            listView1.Columns.Add("起始地址", (int)(listView1.Width * 0.3), HorizontalAlignment.Center);
            listView1.Columns.Add("目的地址", (int)(listView1.Width * 0.2), HorizontalAlignment.Center);
            listView1.Columns.Add("托盘号", (int)(listView1.Width * 0.2), HorizontalAlignment.Center);
            comboBox1.SelectedIndex = 0;
        }

        private void GetError()
        {
           
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                int i = 0;
                string sql = "";
                string countsql = "";
                try
                {
                    if (conn == null)
                        return;
                    if (comboBox1.Text == "全部")
                    {
                        sql = "select t.task_id,t.device_name,t.level_no,t.error_desc,t.row_no,t.create_time ,d.task_type, d.box_barcode,d.from_unit,d.to_unit from tb_plt_error_record t left JOIN tb_plt_task_d d on  d.task_id=t.task_id  where t.task_id not in (0)  and t.error_desc<> '空闲' and STR_TO_DATE(substr(date_format(t.create_time,'%Y-%m-%d'),1,10),'%Y-%m-%d') = STR_TO_DATE('" + dtpStart.Text.ToString() + "','%Y-%m-%d') group by t.task_id,t.device_name,t.level_no,t.error_desc,t.row_no,t.create_time ,d.task_type, d.box_barcode,d.from_unit,d.to_unit order by t.create_time desc ";
                        countsql =  "select count(1) from tb_plt_error_record t left JOIN tb_plt_task_d d on  d.task_id=t.task_id  where t.task_id not in (0)  and t.error_desc<> '空闲' and STR_TO_DATE(substr(date_format(t.create_time,'%Y-%m-%d'),1,10),'%Y-%m-%d') = STR_TO_DATE('" + dtpStart.Text.ToString() + "','%Y-%m-%d')  ";

                    }
                    if (comboBox1.Text == "堆垛机")
                    {
                        sql = "select t.task_id,t.device_name,t.level_no,t.error_desc,t.row_no,t.create_time ,d.task_type, d.box_barcode,d.from_unit,d.to_unit from tb_plt_error_record t left JOIN tb_plt_task_d d on  d.task_id=t.task_id  where t.device_type = 'STACK' and t.task_id not in (0)  and t.error_desc<> '空闲' and str_to_date(substr(date_format(t.create_time,'%Y-%m-%d'),1,10),'%Y-%m-%d') = str_to_date('" + dtpStart.Text.ToString() + "','%Y-%m-%d') group by t.task_id,t.device_name,t.level_no,t.error_desc,t.row_no,t.create_time ,d.task_type, d.box_barcode,d.from_unit,d.to_unit order by t.create_time desc";
                        countsql = "select count(1) from tb_plt_error_record t left JOIN tb_plt_task_d d on  d.task_id=t.task_id  where t.device_type = 'STACK' and t.task_id not in (0)  and t.error_desc<> '空闲' and str_to_date(substr(date_format(t.create_time,'%Y-%m-%d'),1,10),'%Y-%m-%d') = str_to_date('" + dtpStart.Text.ToString() + "','%Y-%m-%d')";
                            }
                    if (comboBox1.Text == "轨道")
                    {
                        sql = " select t.task_id,t.device_name,t.level_no,t.error_desc,t.row_no,t.create_time ,d.task_type, d.box_barcode,d.from_unit,d.to_unit from tb_plt_error_record t left JOIN tb_plt_task_d d on  d.task_id=t.task_id  where t.device_type <> 'STACK' and t.task_id not in (0)  and t.error_desc<> '空闲' and str_to_date(substr(date_format(t.create_time,'%Y-%m-%d'),1,10),'%Y-%m-%d') = str_to_date('" + dtpStart.Text.ToString() + "','%Y-%m-%d') group by t.task_id,t.device_name,t.level_no,t.error_desc,t.row_no,t.create_time ,d.task_type, d.box_barcode,d.from_unit,d.to_unit order by t.create_time desc ";
                        countsql = " select count(1) from tb_plt_error_record t left JOIN tb_plt_task_d d on  d.task_id=t.task_id  where t.device_type <> 'STACK' and t.task_id not in (0)  and t.error_desc<> '空闲' and str_to_date(substr(date_format(t.create_time,'%Y-%m-%d'),1,10),'%Y-%m-%d') = str_to_date('" + dtpStart.Text.ToString() + "','%Y-%m-%d') ";

                    }
                    if (textBox1.Text != "")
                    {
                        sql = "select t.task_id,t.device_name,t.level_no,t.error_desc,t.row_no,t.create_time ,d.task_type, d.box_barcode,d.from_unit,d.to_unit from tb_plt_error_record t left JOIN tb_plt_task_d d on  d.task_id=t.task_id  where t.task_id='" + textBox1.Text + "' and  t.task_id not in (0) and str_to_date(substr(date_format(t.create_time,'%Y-%m-%d'),1,10),'%Y-%m-%d') = str_to_date('" + dtpStart.Text.ToString() + "','%Y-%m-%d') group by t.task_id,t.device_name,t.level_no,t.error_desc,t.row_no,t.create_time ,d.task_type, d.box_barcode,d.from_unit,d.to_unit order by t.create_time desc ";
                        countsql = "select count(1) from tb_plt_error_record t left JOIN tb_plt_task_d d on  d.task_id=t.task_id  where    t.task_id='" + textBox1.Text + "' and t.task_id not in (0) and str_to_date(substr(date_format(t.create_time,'%Y-%m-%d'),1,10),'%Y-%m-%d') = str_to_date('" + dtpStart.Text.ToString() + "','%Y-%m-%d')";
                    }

                    ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, sql);
                    string count = DataBase.MySqlHelper.ExecuteScalar(conn, CommandType.Text, countsql).ToString();
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        string[] items = new string[listView1.Columns.Count];
                        i++;
                        items[0] = i.ToString();
                        items[1] = row["task_id"].ToString();
                        items[2] = row["device_name"].ToString();
                        items[3] = row["task_type"].ToString() == "1" ? "入库" : "出库";
                        if (row["task_type"].ToString() == "1")
                            items[3] = "入库";
                        else if (row["task_type"].ToString() == "2")
                            items[3] = "出库";
                        else if (row["task_type"].ToString() == "3")
                            items[3] = "空托盘入库";
                        else if (row["task_type"].ToString() == "4")
                            items[3] = "退库";
                        else if (row["task_type"].ToString() == "5")
                            items[3] = "异常回库";
                        items[4] = row["level_no"].ToString();
                        items[5] = row["error_desc"].ToString();
                        items[6] = row["row_no"].ToString();
                        items[7] = row["create_time"].ToString();
                        items[8] = row["from_unit"].ToString();
                        items[9] = row["to_unit"].ToString();
                        items[10] = row["box_barcode"].ToString();
                        listView1.Items.Add(new ListViewItem(items));
                    }
                    tbErrorCount.Text = count;

                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }

        }


        private void btSelect_Click(object sender, EventArgs e)
        {
            listView1.Items.Clear();
            GetError();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
        public void ExportExcel(string fileName, System.Windows.Forms.ListView listView, int titleRowCount)
        {
            string saveFileName = "立库报警";
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

        private void button1_Click(object sender, EventArgs e)
        {
            if (listView1.Items.Count > 0)
            {
                ExportExcel("立库报警", listView1, 0);
            }
            else
                MessageBox.Show("没有可导出的数据！");
        }
    }

}

