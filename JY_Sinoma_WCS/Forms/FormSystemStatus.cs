using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace JY_Sinoma_WCS
{
    public partial class FormSystemStatus : Form
    {
        private SystemStatus systemStatus;
        public int index;
        public FormSystemStatus(SystemStatus systemStatus,int index)
        {
            InitializeComponent();
            this.systemStatus = systemStatus;
            this.index = index-1;
        }

        private void FormSystemStatus_Load(object sender, EventArgs e)
        {
            this.Text = (index==0?1:2).ToString() + "层系统状态";
            if (systemStatus.statusStruct[index].systemRun == 1)
                checkSystemRun.Checked = true;

            if (systemStatus.statusStruct[index].warning == 1)
                checkWarning.Checked = true;

            if (systemStatus.statusStruct[index].auto == 1)
            {
                checkAuto.Checked = true;
                bt_Auto.Text = "手动";
            }
            else
                bt_Auto.Text = "自动";

            if (systemStatus.statusStruct[index].StopCarbinet == 1)
                checkStopCabinet.Checked = true;

            if (systemStatus.statusStruct[index].StopSpot == 1)
                checkStopSpot.Checked = true;
        }

        #region 关闭
        private void bt_OK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 故障总清
        private void bt_ClearAll_Click(object sender, EventArgs e)
        {
            systemStatus.WriteFaultClearCmd(index);
        }
        #endregion

        #region 手自动切换
        private void bt_Auto_Click(object sender, EventArgs e)
        {

            if (systemStatus.statusStruct[index].auto == 1)
                systemStatus.WriteWorkModelCmd(index, 0);
            else
                systemStatus.WriteWorkModelCmd(index, 1);
        }
        #endregion

        #region 初始化
        private void bt_initSystem1_Click(object sender, EventArgs e)
        {
            systemStatus.WriteDataClearCmd(index);
        }
        #endregion

        #region 工作模式切换 已作废20200612
        //private void btnModifyTaskMode_Click(object sender, EventArgs e)
        //{
        //    int nTaskMode = cmbTaskMode.SelectedIndex;
        //    if (nTaskMode > 0)
        //        systemStatus.WriteTaskModelCmd(index, nTaskMode);
        //    else
        //        MessageBox.Show("请选择任务模式");
        //}
        #endregion

    }
}
