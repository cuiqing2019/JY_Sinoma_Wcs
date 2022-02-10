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
    public partial class FormElectricsConveyorCmd : Form
    {
        private ElectricsConveyorCmd electricsConveyor;

        SystemStatus systemstatus;
        public ConnectPool dbConn;
        public int index;
        public FormElectricsConveyorCmd(ElectricsConveyorCmd electric, int nIndex, SystemStatus systemStatus)
        {
            InitializeComponent();
            index = nIndex;
            this.electricsConveyor = electric;
            this.systemstatus = systemStatus;
            dbConn = electric.mainFrm.dbConn;
            this.Text = electricsConveyor.conveyorName[nIndex];
            if (electric.isBindToPLC)
            {
                int nTask = 0;
                lbDeviceStatus.Text = "设备返回状态：";


                if (electric.returnStruct[nIndex].status == 10)

                    lbDeviceStatus.Text += "允许下发" + "任务；";
                else
                    lbDeviceStatus.Text += "不允许下发" + "任务；";

                lbDeviceStatus.Text += electric.mainFrm.deviceStatusDic.getDesc(electric.deviceType[nIndex],electric.error[nIndex].ToString());
                lbDeviceStatus.Text += "；任务号：" + electric.returnStruct[nIndex].taskID.ToString();
                lbLoadStatus.Text += "；任务类型：" + (electric.returnStruct[nIndex].taskType == 1 ? "入库" : electric.returnStruct[nIndex].taskType == 2 ? "出库" : electric.returnStruct[nIndex].taskType == 3 ? "空托入库" : electric.returnStruct[nIndex].taskType == 4 ? "退库" : "异常回库");
                lbDeviceStatus.Text += "；起始地址：" + electric.returnStruct[nIndex].from.ToString();
                lbDeviceStatus.Text += "；目的地址：" + electric.returnStruct[nIndex].to.ToString();

                lbDeviceStatus.Text += "，下发任务箱号：" + DataBaseInterface.SelectBoxCode(electric.returnStruct[nIndex].taskID.ToString());

                //lbLoadStatus.Text = "设备装载状态：";
                ////获取任务id的货物类型
                //string i = DataBaseInterface.SelectBoxgood(electric.returnStruct[nIndex].taskID.ToString());
                //lbLoadStatus.Text = "设备装载状态：";

                //if (electric.loadStruct[nIndex].loadType == 1 && int.Parse(i) == 1)
                //    lbLoadStatus.Text += "吨桶；";
                //else if (electric.loadStruct[nIndex].loadType == 1 && int.Parse(i) == 2)
                //    lbLoadStatus.Text += "圆桶；";
                //else if (electric.loadStruct[nIndex].loadType == 1 && int.Parse(i) == 3)
                //    lbLoadStatus.Text += "整摞空托盘；";
                //else if (electric.loadStruct[nIndex].loadType == 1 && int.Parse(i) == 4)
                //    lbLoadStatus.Text += "单个空托盘；";
                //else
                //    lbLoadStatus.Text += "无货；";

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

                lbLoadStatus.Text += "任务号：" + electric.loadStruct[nIndex].taskID.ToString();
                lbLoadStatus.Text += "；起始地址：" + electric.loadStruct[nIndex].from.ToString();
                lbLoadStatus.Text += "；目的地址：" + electric.loadStruct[nIndex].to.ToString();
                lbLoadStatus.Text += "；任务类型：" + (electric.returnStruct[nIndex].taskType == 1 ? "入库" : electric.returnStruct[nIndex].taskType == 2 ? "出库" : electric.returnStruct[nIndex].taskType == 3 ? "空托入库" : electric.returnStruct[nIndex].taskType == 4 ? "退库" : "异常回库");
                lbLoadStatus.Text += "," + systemStatus.GetAuto(electricsConveyor.levelNum[index]) + "；";
                lbLoadStatus.Text += "，承载任务箱号：" + DataBaseInterface.SelectBoxCode(electric.loadStruct[nIndex].taskID.ToString());
                nTask = electric.loadStruct[nIndex].taskID;
                //else(electric.returnPermit == 0&&)

            }
            else
            {
                //lbDeviceStatus.Text = "设备状态：未连接";
                lbLoadStatus.Text = "设备状态：未连接";

            }
        }


        private void btOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btReset_Click(object sender, EventArgs e)
        {
            electricsConveyor.WriteSingleAction(index, 1);
        }

        private void btinit_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("确定要初始化吗，请确保初始化后任务序列不会发生问题", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.No)
                return;
            else if (dialogResult == DialogResult.Yes)
            {
                DataBaseInterface.InsertInitLog(electricsConveyor.conveyorName[index], electricsConveyor.loadStruct[index].taskID, electricsConveyor.loadStruct[index].from, electricsConveyor.loadStruct[index].to);
                electricsConveyor.WriteSingleAction(index, 2);
            }
        }

        private void btWriteId_Click(object sender, EventArgs e)
        {

            if (!DataBaseInterface.isPureNum(tbWriteId.Text.Trim()) || tbWriteId.Text.Trim().Length == 0)
            {
                MessageBox.Show("任务号为空或任务号非数字");
                return;
            }
            if (!DataBaseInterface.isPureNum(txtFrom.Text.Trim()) || txtFrom.Text.Trim().Length == 0)
            {
                MessageBox.Show("起始地址为空或起始地址非数字");
                return;
            }
            if (!DataBaseInterface.isPureNum(txtTo.Text.Trim()) || txtTo.Text.Trim().Length == 0)
            {
                MessageBox.Show("目的地址为空或目的地址非数字");
                return;
            }
            if (cmbLoadType.SelectedIndex == 0)
            {
                MessageBox.Show("请选择托盘类型");
                return;
            }
            if (cmbTaskType.SelectedIndex == 0)
            {
                MessageBox.Show("请选择任务类型");
                return;
            }
            electricsConveyor.WriteID(index, int.Parse(this.tbWriteId.Text.ToString()), cmbTaskType.SelectedIndex, int.Parse(this.txtFrom.Text.ToString()), int.Parse(this.txtTo.Text.ToString()), cmbLoadType.SelectedIndex);
        }

        private void btAuto_Click(object sender, EventArgs e)
        {
            systemstatus.WriteWorkModelCmd(electricsConveyor.systemStatusID[index] - 1, 1);
        }

        private void btmanul_Click(object sender, EventArgs e)
        {
            systemstatus.WriteWorkModelCmd(electricsConveyor.systemStatusID[index]-1,0);
        }

        private void FormConveyor_Load(object sender, EventArgs e)
        {
            CmbControl.SelectedIndex = 0;
            CmbControl.Items.Add(electricsConveyor.mainFrm.ControlCmd(electricsConveyor.deviceType[index]));
          
            cmbTaskType.SelectedIndex = 0;
            cmbLoadType.SelectedIndex = 0;

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

        private void BtnControl_Click(object sender, EventArgs e)
        {
            if (systemstatus.GetAuto(electricsConveyor.levelNum[index]) == "自动")
            {
                MessageBox.Show("点动命令只能在手动状态下进行");
            }
            else
            {
                switch (CmbControl.SelectedIndex)
                {
                    case 1:
                        electricsConveyor.WriteSingleAction(index, 10);
                        break;
                    case 2:
                        electricsConveyor.WriteSingleAction(index, 11);
                        break;
                    case 0:
                        MessageBox.Show("请选择点动命令");
                        break;
                    default:
                        break;
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            electricsConveyor.WriteSingleAction(index, 0);
        }

    }
}
