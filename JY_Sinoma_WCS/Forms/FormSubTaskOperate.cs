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
    public partial class FormSubTaskOperate : Form
    {
        private FormSubTask mainFrm;
        public ConnectPool dbConn;//定义数据库连接
        public int nOperation;
        public string strDTaskID;
        public string strDTaskType;
        public string strMTaskID;
        public string strMTaskType;
        public string strStep;
        public string strFromUnit;
        public string strToUnit;
        public string strDevice;
        public string strChannel;
        public string strStatus;
        public FormSubTaskOperate(FormSubTask mainFrm)
        {
            this.mainFrm = mainFrm;
            dbConn = mainFrm.dbConn;
            InitializeComponent();
            tbFromUnit.Enabled = false;
            cmbStep.Enabled = false;
            tbFromUnit.Enabled = false;
            tbToUnit.Enabled = false;
            tbDevice.Enabled = false;
            tbChannel.Enabled = false;
            tbDTaskID.Text = strDTaskID;
            tbDTaskType.Text = strDTaskType;
            tbMTaskType.Text = strMTaskType;
            tbFromUnit.Text = strFromUnit;
            tbToUnit.Text = strToUnit;
        }
        #region 子任务类型识别
        /// <summary>
        /// 子任务类型识别
        /// </summary>
        /// <param name="strMsgType"></param>
        /// <param name="nType"></param>
        /// <returns></returns>
        public string EncodeDTaskType(string nType)
        {
            if (nType == "输送")
                    return "CON";
            else if (nType == "堆垛机")
                return "STACK";
            else
                return "未知";
        }
        #endregion
        private void FormSubTaskOperate_Load(object sender, EventArgs e)
        {
            tbFromUnit.Enabled = false;
            cmbStep.Enabled = false;
            tbFromUnit.Enabled = false;
            tbToUnit.Enabled = false;
            tbDevice.Enabled = false;
            tbChannel.Enabled = false;
            tbDTaskID.Text = strDTaskID;
            tbDTaskType.Text = strDTaskType;
            tbMTaskType.Text = strMTaskType;
            tbFromUnit.Text = strFromUnit;
            tbToUnit.Text = strToUnit;
            cmbStep.SelectedIndex = int.Parse(strStep) - 1;
            tbDevice.Text = strDevice;
            tbChannel.Text = strChannel;
            if (strStatus == "新生成")
                cmbStatus.SelectedIndex = 0;
            else if (strStatus == "执行中")
                cmbStatus.SelectedIndex = 1;
            else
                cmbStatus.SelectedIndex = 2;
                       
        }
        private void btUpdate_Click(object sender, EventArgs e)
        {
            int nStatus = 0;
            string rs = string.Empty;
            switch (strStatus)
            {
                case "新生成":
                    nStatus = 0;
                    break;
                case "执行中":
                    nStatus = 1;
                    break;
                case "已完成":
                    nStatus = 2;
                    break;
            }
            if (cmbStatus.SelectedIndex <= nStatus)
            {
                MessageBox.Show("请选择下一级任务状态！");
                return;
            }
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                {
                    MessageBox.Show("无法获取数据库连接！");
                    return;
                }
                try
                {
                    string strSQL = "select t.* from TB_PLT_TASK_D t where t.TASK_ID=" + strDTaskID + " and t.STEP>" + (cmbStep.SelectedIndex + 1).ToString();
                    DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        if (int.Parse(ds.Tables[0].Rows[0]["STATUS"].ToString()) > 0)
                        {
                            MessageBox.Show("下一项子任务状态不是新生成，无法修改！");
                            return;
                        }
                    }
                    if (MessageBox.Show("是否确定修改该子任务？", "提示", MessageBoxButtons.OKCancel) == DialogResult.OK)
                    {
                        string strDeviceType = this.EncodeDTaskType(strDevice);
                        nStatus = cmbStatus.SelectedIndex;
                        if (nStatus == 1 || nStatus == 2)
                        {
                            if (DataBaseInterface.TaskStatusUpdate(int.Parse(strDTaskID), 0, "手工", cmbStep.SelectedIndex + 1, cmbStatus.SelectedIndex, out rs) == 1)
                            {
                                MessageBox.Show("修改数据成功！");
                                mainFrm.RefreshListView();
                                this.Close();
                            }
                            else
                                MessageBox.Show("修改数据失败！");
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
                
        }



        private void btCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
