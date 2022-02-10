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
    public partial class FormTray : Form
    {
        private Tray conveyor;

        SystemStatus systemstatus;
        // public ConnectPool dbConn;
        public int index;
        public FormTray(Tray electric, int nIndex, SystemStatus systemStatus)
        {
            InitializeComponent();
            this.conveyor = electric;
            this.systemstatus = systemStatus;
            //dbConn = electric.mainFrm.dbConn;
            this.Text = conveyor.nDeviceName[nIndex];
            this.index = nIndex;
            if (electric.isBindToPLC)
            {
                int nTask = 0;
                lbDeviceStatus.Text = "设备返回状态：";


                if (electric.returnStruct[nIndex].status == 10)

                    lbDeviceStatus.Text += "允许下发" + "任务；";
                else
                    lbDeviceStatus.Text += "不允许下发" + "任务；";
                lbDeviceStatus.Text += electric.mainFrm.deviceStatusDic.getDesc("DP", electric.error[nIndex].ToString());
                lbDeviceStatus.Text += "；任务号：" + electric.returnStruct[nIndex].taskID.ToString();
                lbDeviceStatus.Text += "；任务类型：" + electric.returnStruct[nIndex].taskType.ToString();
                lbDeviceStatus.Text += "；起始地址：" + electric.returnStruct[nIndex].from.ToString();
                lbDeviceStatus.Text += "；目的地址：" + electric.returnStruct[nIndex].to.ToString();
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
                lbLoadStatus.Text += "；载货类型：" + electric.loadStruct[nIndex].loadType.ToString();
                lbLoadStatus.Text += "," + systemStatus.GetAuto(conveyor.levelNum[nIndex]) + "；";
                nTask = electric.loadStruct[nIndex].taskID;
                //else(electric.returnPermit == 0&&)

            }
            else
            {
                lbLoadStatus.Text = "设备状态：未连接";

            }
            if (conveyor.playTray)
                tbStop.Text = "停止放托盘";
            else
                tbStop.Text = "开始放托盘";
        }


        private void btOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btReset_Click(object sender, EventArgs e)
        {
            conveyor.WriteSingleAction(index, 1);
        }

        private void btinit_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("确定要初始化吗，请确保初始化后任务序列不会发生问题", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
            if (dialogResult == DialogResult.No)
                return;
            else if (dialogResult == DialogResult.Yes)
            {
                DataBaseInterface.InsertInitLog(conveyor.nDeviceName[index], conveyor.loadStruct[index].taskID, conveyor.loadStruct[index].from, conveyor.loadStruct[index].to);
                conveyor.WriteSingleAction(index, 2);
            }
        }

        private void btWriteId_Click(object sender, EventArgs e)
        {
            if (!conveyor.mainFrm.Stackers[1001].getUpTask())
                conveyor.WriteCmd(index, conveyor.taskid + 1, cmbLoadType.SelectedIndex, 1001, 1007, cmbTaskType.SelectedIndex);
            else
                MessageBox.Show("一巷道堆垛机有放货站台放货任务，请等待放货任务完成后再次进行尝试，如堆垛机放货站台放货任务较多，请将堆垛机任务状态修改为停用后等待任务完成后下发");
        } 

        private void btAuto_Click(object sender, EventArgs e)
        {
            systemstatus.WriteWorkModelCmd(conveyor.systemStatusID[index] - 1, 1);
        }

        private void btmanul_Click(object sender, EventArgs e)
        {
            systemstatus.WriteWorkModelCmd(conveyor.systemStatusID[index]-1,0);
        }

        private void FormConveyor_Load(object sender, EventArgs e)
        {
            CmbControl.SelectedIndex = 0;
            cmbTaskType.SelectedIndex = 0;
            cmbLoadType.SelectedIndex = 0;

            status();
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
            conveyor.WriteSingleAction(index, 0);
        }
       

        private void stop_Click(object sender, EventArgs e)
        {
            if (conveyor.playTray)
            {
                tbStop.Text = "开始放托盘";
                conveyor.playTray = false;
            }
            else
            {
                tbStop.Text = "停止放托盘";
                conveyor.playTray = true;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (conveyor.empetTask)
            {
              //  button2.Text = "出空托盘";
                DataBaseInterface.Updateempty(1);
                conveyor.empetTask = false;
            }
            else
            {
               // button2.Text = "停止出空托盘";
                DataBaseInterface.Updateempty(0);
                conveyor.empetTask = true;
            }
            status();


        }
        public void status() {
            string status = DataBaseInterface.Updateemptystatus();
            if (status == "1")
            {
                button2.Text = "开始下任务";
            }
            else
            {
                button2.Text = "停止下任务";
            }

            if (conveyor.empetTask)
                button2.Text = "停止下任务";
            else
                button2.Text = "开始下任务";
        }
       
    }
}
