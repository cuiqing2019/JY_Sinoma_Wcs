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
    public partial class FormConveyorNoLoad : Form
    {
        private ConveyorNoLoad conveyor;

        SystemStatus systemstatus;
        public ConnectPool dbConn;
        public int index;
        public FormConveyorNoLoad(ConveyorNoLoad electric, int nIndex, SystemStatus systemStatus)
        {
            InitializeComponent();
            this.conveyor = electric;
            this.systemstatus = systemStatus;
            dbConn = electric.mainFrm.dbConn;
            this.Text = conveyor.conveyorName[nIndex];
            this.index = nIndex;
            if (electric.isBindToPLC)
                lbLoadStatus.Text += electric.mainFrm.deviceStatusDic.getDesc(electric.deviceType[nIndex], electric.error[nIndex].ToString());
            else
                lbLoadStatus.Text = "设备状态：未连接";
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
                conveyor.WriteSingleAction(index, 2);
            }
        }

        private void btAuto_Click(object sender, EventArgs e)
        {
            systemstatus.WriteWorkModelCmd(conveyor.systemStatusID[index]-1,0);
        }

        private void btmanul_Click(object sender, EventArgs e)
        {
            systemstatus.WriteWorkModelCmd(conveyor.systemStatusID[index]-1,0);
        }

        private void FormConveyor_Load(object sender, EventArgs e)
        { 
            CmbControl.Items.Add("--选择点动命令--");
            //添加手动命令
            switch (conveyor.deviceType[index])
            {
                    //如果是叠盘机
                case "DP":
                    CmbControl.Items.Add("输送电机正转");
                    CmbControl.Items.Add("输送电机反转");
                    CmbControl.Items.Add("托盘提升上升");
                    CmbControl.Items.Add("托盘提升下降");
                    CmbControl.Items.Add("伸叉气缸伸");
                    CmbControl.Items.Add("伸叉气缸缩");
                    CmbControl.Items.Add("阻挡上升");
                    CmbControl.Items.Add("阻挡下降");
                    CmbControl.Items.Add("清空叠盘机");
                    CmbControl.SelectedIndex = 0;
                    break;
                    //如果是链条机
                case "SS":
                    CmbControl.Items.Add("电机正转");
                    CmbControl.Items.Add("电机反转");
                    CmbControl.SelectedIndex = 0;
                    break;
                    //如果是顶升移栽
                case "YZ":
                    CmbControl.Items.Add("输送电机正转");
                    CmbControl.Items.Add("输送电机反转");
                    CmbControl.Items.Add("移载机构正转");
                    CmbControl.Items.Add("移载机构反转");
                    CmbControl.Items.Add("顶升电机上升");
                    CmbControl.Items.Add("顶升电机下降");
                    CmbControl.SelectedIndex = 0;
                    break;
                default: break;
            }
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

        private void button1_Click(object sender, EventArgs e)
        {
            conveyor.WriteSingleAction(index, 0);
        }

    }
}
