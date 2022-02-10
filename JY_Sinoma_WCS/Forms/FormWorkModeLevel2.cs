using DataBase;
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
    public partial class FormWorkModeLevel2 : Form
    {
        public frmMain mainFrm;
        public int nIndex = 1;
        public FormWorkModeLevel2(frmMain mainFrm)
        {
            InitializeComponent();
            this.mainFrm = mainFrm;
        }

        private void btnChangeTaskType_Click(object sender, EventArgs e)
        {
            if(mainFrm.taskType[nIndex]!=0)
            {
                if(!mainFrm.stopTaskCreate[nIndex]&&cmbTaskType.SelectedIndex>0 &&DecodeTaskType(cmbTaskType.SelectedIndex)!=mainFrm.taskType[nIndex])
                {
                    MessageBox.Show("请先停止当前工作模式！");
                    return;
                }
            }
            if(cmbTaskType.SelectedIndex==1)
            {
                if(DataBaseInterface.SelectOtherTaskCount("2",2)>0)
                {
                    MessageBox.Show("存在正你在执行的其他任务，请等待执行完成后切换状态！");
                    return;
                }
                mainFrm.taskType[nIndex] = cmbTaskType.SelectedIndex;
                mainFrm.systemStatus.WriteTaskModelCmd(nIndex, 2);
                mainFrm.btnWorkModeLevel2.Text = "二楼出入库";
                mainFrm.stopTaskCreate[nIndex]= false;
                lbTaskType.Text = "当前任务模式：二楼出入库";
                
            }
            else if(cmbTaskType.SelectedIndex==2)
            {
                if (DataBaseInterface.SelectOtherTaskCount("3", 2) > 0)
                {
                    MessageBox.Show("存在正你在执行的其他任务，请等待执行完成后切换状态！");
                    return;
                }
                mainFrm.taskType[nIndex] = cmbTaskType.SelectedIndex;
                mainFrm.systemStatus.WriteTaskModelCmd(nIndex, 3);
                mainFrm.btnWorkModeLevel2.Text = "二楼空托入库";
                mainFrm.stopTaskCreate[nIndex] = false;
                lbTaskType.Text = "当前任务模式：二楼空托入库";
                mainFrm.goodsKind[1] = 3;
                mainFrm.goodsSku[1] = "000000";
                mainFrm.batchNo[1] = "000000";
                mainFrm.batchid[1] = "000000";
                mainFrm.goodsName[1] = "空托盘组";
                mainFrm.dealWay[1] = 1;
                mainFrm.hazardArea[1] = "A";
        
            }
            else if(cmbTaskType.SelectedIndex==3)
            {
                mainFrm.stopTaskCreate[nIndex - 1] = true;
                if (DataBaseInterface.SelectOtherTaskCount("5", 2) > 0|| DataBaseInterface.SelectOtherTaskCount("5", 1) > 0)
                {
                    MessageBox.Show("存在正你在执行的其他任务，请等待执行完成后切换状态！");
                    return;
                }
                mainFrm.taskType[nIndex] = 4;
                mainFrm.systemStatus.WriteTaskModelCmd(nIndex, 5);
                mainFrm.btnWorkModeLevel2.Text = "二楼异常回库";
                mainFrm.taskType[nIndex-1] = 4;
                mainFrm.systemStatus.WriteTaskModelCmd(nIndex-1, 5);
                mainFrm.workModeBotton.Text = "一楼异常回库";
                mainFrm.stopTaskCreate[nIndex] = false;
                mainFrm.stopTaskCreate[nIndex - 1] = false;
                lbTaskType.Text = "当前任务模式：二楼异常回库";
            }
            else
            {
                mainFrm.stopTaskCreate[nIndex] = true;
                mainFrm.btnWorkModeLevel2.Text = "二楼工作模式";
                lbTaskType.Text = "当前任务模式：无";
            }
            if(mainFrm.stopTaskCreate[nIndex])
            {
                btnStop.Text = "开始生成任务";
            }
            else
            {
                btnStop.Text = "停止生成任务";
            }
        }

        public int DecodeTaskType(int nNum)
        {
            switch(nNum)
            {
                case 1:
                    return 1;
                case 2:
                    return 3;
                case 3:
                    return 4;
                default:
                    return 0;
            }
        }

        private void FormWorkModeLevel2_Load(object sender, EventArgs e)
        {
            if (mainFrm.taskType[nIndex] == 1)
            {
                lbTaskType.Text = "当前任务模式：出库";
                cmbTaskType.SelectedIndex = 1;
            }
            else if (mainFrm.taskType[nIndex] == 2)
            {
                lbTaskType.Text = "当前任务模式：空托入库";
                cmbTaskType.SelectedIndex = 2;
            }
            else if (mainFrm.taskType[nIndex] == 4)
            {
                lbTaskType.Text = "当前任务模式：异常回库";
                cmbTaskType.SelectedIndex = 3;
            } 
            else if(mainFrm.taskType[nIndex]==0)
            {
                lbTaskType.Text = "当前任务模式：空闲模式";
                cmbTaskType.SelectedIndex = 0;
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            if (mainFrm.stopTaskCreate[nIndex])
            {
                btnStop.Text = "停止生成任务";
                mainFrm.stopTaskCreate[nIndex] = false;
              
            }
            else
            {
                btnStop.Text = "开始生成任务";
                mainFrm.stopTaskCreate[nIndex] = true;
              //  mainFrm.systemStatus.WriteTaskModelCmd(nIndex, 0);
            }
          
        }
    }
}
