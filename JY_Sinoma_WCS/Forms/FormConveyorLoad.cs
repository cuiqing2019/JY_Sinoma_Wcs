using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DataBase;
using System.Threading;

namespace JY_Sinoma_WCS
{
    public partial class FormConveyorLoad : Form
    {
        private ConveyorLoad conveyor;

        SystemStatus systemstatus;
        public ConnectPool dbConn;
        public int index;
        public FormConveyorLoad(ConveyorLoad electric, int nIndex, SystemStatus systemStatus)
        {
            InitializeComponent();
            this.conveyor = electric;
            this.systemstatus = systemStatus;
            dbConn = electric.mainFrm.dbConn;
            this.Text = conveyor.conveyorName[nIndex];
            this.index = nIndex;
            if (electric.isBindToPLC)
            {
                int nTask = 0;
                lbLoadStatus.Text = "设备装载状态：";
                if (electric.loadStruct[nIndex].loadType == 1)
                    lbLoadStatus.Text += "吨桶；";
                else if (electric.loadStruct[nIndex].loadType == 2)
                    lbLoadStatus.Text += "圆桶；";
                else if (electric.loadStruct[nIndex].loadType == 3)
                    lbLoadStatus.Text += "整摞空托盘；";
                else if (electric.loadStruct[nIndex].loadType == 4)
                    lbLoadStatus.Text += "单个空托盘；";
                else
                    lbLoadStatus.Text += "无货；";
                lbLoadStatus.Text += electric.mainFrm.deviceStatusDic.getDesc(electric.deviceType[nIndex],electric.error[nIndex].ToString());
                lbLoadStatus.Text += "；任务号：" + electric.loadStruct[nIndex].taskID.ToString();
                lbLoadStatus.Text += "；起始地址：" + electric.loadStruct[nIndex].from.ToString();
                lbLoadStatus.Text += "；目的地址：" + electric.loadStruct[nIndex].to.ToString();
                lbLoadStatus.Text += "," + systemStatus.GetAuto(conveyor.levelNum[nIndex]) + "；";
                nTask = electric.loadStruct[nIndex].taskID;
                lbLoadStatus.Text += "，箱号：" + DataBaseInterface.SelectBoxCode(nTask.ToString());

            }
            else
            {
                //lbDeviceStatus.Text = "设备状态：未连接";
                lbLoadStatus.Text = "设备状态：未连接";

            }
        }

        #region 关闭
        private void btOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 清错
        private void btReset_Click(object sender, EventArgs e)
        {
            conveyor.WriteSingleAction(index, 1);
        }
        #endregion

        #region 初始化
        private void btinit_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("确定要初始化吗，请确保初始化后任务序列不会发生问题", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.No)
                return;
            else if (dialogResult == DialogResult.Yes)
            {
                DataBaseInterface.InsertInitLog(conveyor.conveyorName[index], conveyor.loadStruct[index].taskID, conveyor.loadStruct[index].from, conveyor.loadStruct[index].to);
                conveyor.WriteSingleAction(index, 2);
            }
        }
        #endregion

        #region 自动
        private void btAuto_Click(object sender, EventArgs e)
        {
            systemstatus.WriteWorkModelCmd(conveyor.systemStatusID[index] - 1, 1);
        }
        #endregion

        #region 手动
        private void btmanul_Click(object sender, EventArgs e)
        {
            systemstatus.WriteWorkModelCmd(conveyor.systemStatusID[index] - 1, 0);
        }

        #endregion

        private void FormConveyor_Load(object sender, EventArgs e)
        {
            CmbControl.SelectedIndex = 0;
            CmbControl.Items.Add(conveyor.mainFrm.ControlCmd(conveyor.deviceType[index]));
        }

        private void btn_Eorror_check_Click(object sender, EventArgs e)
        {
            //FrmConverError frmConveyorError = new FrmConverError(stationLoadDepth1.error[0]);
            //frmConveyorError.ShowDialog();
        }

        private void tbWriteId_KeyPress(object sender, KeyPressEventArgs e)
        {
            char result = e.KeyChar;
            if (char.IsDigit(result) || result == 8)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }

        #region 执行点动
        private void BtnControl_Click(object sender, EventArgs e)
        {
            if (systemstatus.GetAuto(conveyor.levelNum[index]) == "自动")
            {
                MessageBox.Show("点动命令只能在手动状态下进行");
            }
            else
            {
                switch (CmbControl.SelectedIndex)
                {
                    case 1:
                        conveyor.WriteSingleAction(index, 10);
                        break;
                    case 2:
                        conveyor.WriteSingleAction(index, 11);
                        break;
                    case 3:
                        conveyor.WriteSingleAction(index, 12);
                        break;
                    case 4:
                        conveyor.WriteSingleAction(index, 13);
                        break;
                    case 5:
                        conveyor.WriteSingleAction(index, 14);
                        break;
                    case 6:
                        conveyor.WriteSingleAction(index, 15);
                        break;
                    case 7:
                        conveyor.WriteSingleAction(index, 16);
                        break;
                    case 8:
                        conveyor.WriteSingleAction(index, 17);
                        break;
                    case 9:
                        conveyor.WriteSingleAction(index, 18);
                        break;
                    case 0:
                        MessageBox.Show("请选择点动命令");
                        break;
                    default:
                        break;
                }
            }
        }
        #endregion

        #region 停止点动
        private void button1_Click(object sender, EventArgs e)
        {
            conveyor.WriteSingleAction(index, 0);
        }
        #endregion


    }
}
