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
    public partial class FormTaskType : Form
    {
        private frmMain mainFrm;
        public ConnectPool dbConn;
        public string strPortId;
        public string strPortDesc;
        public string strLevel;
        public string strTaskType;
        public FormTaskType(frmMain mainFrm,string strPortId, string strPortDesc, string strLevel, string strTaskType)
        {
            InitializeComponent();
            this.mainFrm = mainFrm;
            dbConn = mainFrm.dbConn;
            this.strPortId = strPortId;
            this.strPortDesc = strPortDesc;
            this.strLevel = strLevel;
            this.strTaskType = strTaskType;
        }

        private void btnOk_Click(object sender, EventArgs e)
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                mainFrm.stopTaskCreate[int.Parse(strLevel)-1] = true;//先停止工作
                string strSQL = string.Empty;
                if (strLevel == "1")
                {
                    if (cmbTaskType.SelectedIndex == 2)
                    {
                        MessageBox.Show("一楼不能设置出库任务模式");
                        return;
                    }

                    strSQL = "select count(1) from tb_plt_task_m t where t.task_type<>"+cmbTaskType.SelectedIndex+" and t.task_type<>2 and t.task_type<>6 and t.task_status<2";
                    DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                    int nCount = int.Parse(ds.Tables[0].Rows[0]["count(1)"].ToString());
                    if(nCount>0)
                    {
                        MessageBox.Show("存在正在执行的其他类型任务，请等待任务执行完后切换状态！");
                        return;
                    }
                    strSQL = "update td_inport_dic t set t.task_type="+cmbTaskType.SelectedIndex+" where port_id="+int.Parse(strPortId);
                    if (DataBase.MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL) != 0)
                    {
                        mainFrm.stopTaskCreate[int.Parse(strLevel) - 1] = false;
                        mainFrm.systemStatus.WriteTaskModelCmd(0, cmbTaskType.SelectedIndex);
                        if (cmbTaskType.SelectedIndex == 1)
                            mainFrm.taskType[0] = 1;
                        else if (cmbTaskType.SelectedIndex == 3)
                            mainFrm.taskType[0] = 2;
                        else if (cmbTaskType.SelectedIndex == 4)
                            mainFrm.taskType[0] = 3;
                        else if (cmbTaskType.SelectedIndex == 5)
                            mainFrm.taskType[0] = 4;
                        MessageBox.Show("状态修改成功");
                    }
                }
                else
                {
                    if (cmbTaskType.SelectedIndex == 1 || cmbTaskType.SelectedIndex == 4)
                    {
                        MessageBox.Show("二楼不可设置入库或者退库任务模式");
                        return;
                    }
                    strSQL = "select count(1) from tb_plt_task_m t where t.task_type not in(1,4,"+cmbTaskType.SelectedIndex+") and t.task_status<2";
                    DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                    int nCount = int.Parse(ds.Tables[0].Rows[0]["count(1)"].ToString());
                    if (nCount > 0)
                    {
                        MessageBox.Show("存在正在执行的其他类型任务，请等待任务执行完后切换状态！");
                        return;
                    }
                    strSQL = "update td_inport_dic t set t.task_type=" + cmbTaskType.SelectedIndex + " where port_id=" + int.Parse(strPortId);
                    if (DataBase.MySqlHelper.ExecuteNonQuery(conn, CommandType.Text, strSQL) != 0)
                    {
                        mainFrm.stopTaskCreate[int.Parse(strLevel) - 1] = false;
                        mainFrm.systemStatus.WriteTaskModelCmd(1, cmbTaskType.SelectedIndex);
                        if (cmbTaskType.SelectedIndex == 2)
                            mainFrm.taskType[1] = 1;
                        else if (cmbTaskType.SelectedIndex == 3)
                            mainFrm.taskType[1] = 2;
                        else if (cmbTaskType.SelectedIndex == 5)
                            mainFrm.taskType[1] = 4;
                        MessageBox.Show("状态修改成功");
                    }
                }
            } 
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void FormTaskType_Load(object sender, EventArgs e)
        {
            txtId.Text = strPortId;
            txtDesc.Text = strPortDesc;
            txtLevel.Text = strLevel;
            cmbTaskType.SelectedText = strTaskType;
        }

     
    }
}
