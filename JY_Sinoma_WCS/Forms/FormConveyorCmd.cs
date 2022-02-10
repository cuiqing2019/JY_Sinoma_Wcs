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
using JY_Sinoma_WCS;
using MySql.Data.MySqlClient;

namespace JY_Sinoma_WCS
{
    public partial class FormConveyorCmd : Form
    {
        private FirstFloorInConveyorScannerCmd firstConveyorScannerCmd = null;
        private FirstFloorInConveyorCmd firstConveyorCmd = null;
        private SecondFloorInConveyorCmd secondConveyor = null;
        private OutConveyorCmd outConveyor = null;
        private RGV rgv = null;
        

        SystemStatus systemstatus;
        // public ConnectPool dbConn;
        public int index;

        #region FirstFloorInConveyorScannerCmd
        public FormConveyorCmd(FirstFloorInConveyorScannerCmd electric, int nIndex, SystemStatus systemStatus)
        {
            InitializeComponent();
            btnDel.Enabled = true;
            this.firstConveyorScannerCmd = electric;
            this.systemstatus = systemStatus;
            //dbConn = electric.mainFrm.dbConn;
            this.Text = firstConveyorScannerCmd.conveyorName[nIndex];
            this.index = nIndex;
            if (electric.isBindToPLC)
            {
                int nTask = 0;
                lbDeviceStatus.Text = "设备返回状态：";
                btn_Eorror_check.Text = "入库扫码退回";

                if (electric.returnStruct[nIndex].status == 10)

                    lbDeviceStatus.Text += "允许下发" + "任务；";
                else
                    lbDeviceStatus.Text += "不允许下发" + "任务；";
                lbDeviceStatus.Text += electric.mainFrm.deviceStatusDic.getDesc(electric.deviceType[nIndex], electric.error[nIndex].ToString());
                lbDeviceStatus.Text += "；任务号：" + electric.returnStruct[nIndex].taskID.ToString();
                lbDeviceStatus.Text += "；任务类型：" + electric.returnStruct[nIndex].taskType.ToString();
                lbDeviceStatus.Text += "；起始地址：" + electric.returnStruct[nIndex].from.ToString();
                lbDeviceStatus.Text += "；目的地址：" + electric.returnStruct[nIndex].to.ToString();

                lbDeviceStatus.Text += "，下发任务箱号：" + DataBaseInterface.SelectBoxCode(electric.returnStruct[nIndex].taskID.ToString());

                lbLoadStatus.Text = "设备装载状态：";
                if (electric.loadStruct[nIndex].loadType == 1)
                    lbLoadStatus.Text += "货箱；";
                else if (electric.loadStruct[nIndex].loadType == 2)
                    lbLoadStatus.Text += "单个空托盘；";
                else if (electric.loadStruct[nIndex].loadType == 3)
                    lbLoadStatus.Text += "整摞空托盘；";
                else if (electric.loadStruct[nIndex].loadType == 4)
                    lbLoadStatus.Text += "单个空托盘；";
                else
                    lbLoadStatus.Text += "无货；";
                lbLoadStatus.Text += "任务号：" + electric.loadStruct[nIndex].taskID.ToString();
                lbLoadStatus.Text += "；起始地址：" + electric.loadStruct[nIndex].from.ToString();
                lbLoadStatus.Text += "；目的地址：" + electric.loadStruct[nIndex].to.ToString();
                lbLoadStatus.Text += "；任务类型：" + (electric.taskStruct[nIndex].taskType == 1 ? "入库" : electric.taskStruct[nIndex].taskType == 2 ? "出库" : electric.taskStruct[nIndex].taskType == 3 ? "空托入库" : electric.taskStruct[nIndex].taskType == 4 ? "退库" : "异常回库");
                lbLoadStatus.Text += "," + systemStatus.GetAuto(firstConveyorScannerCmd.levelNum[nIndex]) + "；";
                lbLoadStatus.Text += "，承载任务箱号：" + DataBaseInterface.SelectBoxCode(electric.loadStruct[nIndex].taskID.ToString());
                nTask = electric.loadStruct[nIndex].taskID;
                //else(electric.returnPermit == 0&&)
               // this.btn_Eorror_check.Enabled = false;
            }
            else
            {
                lbLoadStatus.Text = "设备状态：未连接";

            }
        }
        #endregion

        #region FirstFloorInConveyorCmd
        public FormConveyorCmd(FirstFloorInConveyorCmd electric, int nIndex, SystemStatus systemStatus)
        {
            InitializeComponent();
            this.firstConveyorCmd = electric;
            this.systemstatus = systemStatus;
            //dbConn = electric.mainFrm.dbConn;
            this.Text = firstConveyorCmd.conveyorName[nIndex];
            this.index = nIndex;
            if (electric.isBindToPLC)
            {
                int nTask = 0;
                lbDeviceStatus.Text = "设备返回状态：";
                btn_Eorror_check.Text = "入库扫码退回";

                if (electric.returnStruct[nIndex].status == 10)

                    lbDeviceStatus.Text += "允许下发" + "任务；";
                else
                    lbDeviceStatus.Text += "不允许下发" + "任务；";
                lbDeviceStatus.Text += electric.mainFrm.deviceStatusDic.getDesc(electric.deviceType[nIndex], electric.error[nIndex].ToString());
                lbDeviceStatus.Text += "；任务号：" + electric.returnStruct[nIndex].taskID.ToString();
                lbDeviceStatus.Text += "；任务类型：" + electric.returnStruct[nIndex].taskType.ToString();
                lbDeviceStatus.Text += "；起始地址：" + electric.returnStruct[nIndex].from.ToString();
                lbDeviceStatus.Text += "；目的地址：" + electric.returnStruct[nIndex].to.ToString();

                lbDeviceStatus.Text += "，下发任务箱号：" + DataBaseInterface.SelectBoxCode(electric.returnStruct[nIndex].taskID.ToString());

                lbLoadStatus.Text = "设备装载状态：";
                if (electric.loadStruct[nIndex].loadType == 1)
                    lbLoadStatus.Text += "货箱；";
                else if (electric.loadStruct[nIndex].loadType == 2)
                    lbLoadStatus.Text += "单个空托盘；";
                else if (electric.loadStruct[nIndex].loadType == 3)
                    lbLoadStatus.Text += "整摞空托盘；";
                else if (electric.loadStruct[nIndex].loadType == 4)
                    lbLoadStatus.Text += "单个空托盘；";
                else
                    lbLoadStatus.Text += "无货；";
                lbLoadStatus.Text += "任务号：" + electric.loadStruct[nIndex].taskID.ToString();
                lbLoadStatus.Text += "；起始地址：" + electric.loadStruct[nIndex].from.ToString();
                lbLoadStatus.Text += "；目的地址：" + electric.loadStruct[nIndex].to.ToString();
                lbLoadStatus.Text += "；任务类型：" + (electric.taskStruct[nIndex].taskType == 1 ? "入库" : electric.taskStruct[nIndex].taskType == 2 ? "出库" : electric.taskStruct[nIndex].taskType == 3 ? "空托入库" : electric.taskStruct[nIndex].taskType == 4 ? "退库" : "异常回库");
                lbLoadStatus.Text += "," + systemStatus.GetAuto(firstConveyorCmd.levelNum[nIndex]) + "；";
                lbLoadStatus.Text += "，承载任务箱号：" + DataBaseInterface.SelectBoxCode(electric.loadStruct[nIndex].taskID.ToString());
                nTask = electric.loadStruct[nIndex].taskID;
                //else(electric.returnPermit == 0&&)
                this.btn_Eorror_check.Enabled = false;
            }
            else
            {
                lbLoadStatus.Text = "设备状态：未连接";

            }
        }
        #endregion

        #region SecondFloorInConveyorCmd
        public FormConveyorCmd(SecondFloorInConveyorCmd electric, int nIndex, SystemStatus systemStatus)
        {
            InitializeComponent();
            this.secondConveyor = electric;
            this.systemstatus = systemStatus;
            //dbConn = electric.mainFrm.dbConn;
            this.Text = secondConveyor.conveyorName[nIndex];
            this.index = nIndex;
            if (electric.isBindToPLC)
            {
                int nTask = 0;
                lbDeviceStatus.Text = "设备返回状态：";

                btn_Eorror_check.Enabled = false;
                if (electric.returnStruct[nIndex].status == 10)

                    lbDeviceStatus.Text += "允许下发" + "任务；";
                else
                    lbDeviceStatus.Text += "不允许下发" + "任务；";
                lbDeviceStatus.Text += electric.mainFrm.deviceStatusDic.getDesc(electric.deviceType[nIndex], electric.error[nIndex].ToString());
                lbDeviceStatus.Text += "；任务号：" + electric.returnStruct[nIndex].taskID.ToString();
                lbDeviceStatus.Text += "；任务类型：" + electric.returnStruct[nIndex].taskType.ToString();
                lbDeviceStatus.Text += "；起始地址：" + electric.returnStruct[nIndex].from.ToString();
                lbDeviceStatus.Text += "；目的地址：" + electric.returnStruct[nIndex].to.ToString();

                lbDeviceStatus.Text += "，下发任务箱号：" + DataBaseInterface.SelectBoxCode(electric.returnStruct[nIndex].taskID.ToString());

                lbLoadStatus.Text = "设备装载状态：";
                if (electric.loadStruct[nIndex].loadType == 1)
                    lbLoadStatus.Text += "货箱；";
                else if (electric.loadStruct[nIndex].loadType == 2)
                    lbLoadStatus.Text += "单个空托盘；";
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
                lbLoadStatus.Text += "," + systemStatus.GetAuto(secondConveyor.levelNum[nIndex]) + "；";
                lbLoadStatus.Text += "，承载任务箱号：" + DataBaseInterface.SelectBoxCode(electric.loadStruct[nIndex].taskID.ToString());
                nTask = electric.loadStruct[nIndex].taskID;
                //else(electric.returnPermit == 0&&)
                this.btn_Eorror_check.Enabled = false;
            }
            else
            {
                lbLoadStatus.Text = "设备状态：未连接";

            }
        }
        #endregion

        #region OutConveyorCmd
        public FormConveyorCmd(OutConveyorCmd electric, int nIndex, SystemStatus systemStatus)
        {
            InitializeComponent();
            this.outConveyor = electric;
            this.systemstatus = systemStatus;
            //dbConn = electric.mainFrm.dbConn;
            this.Text = outConveyor.conveyorName[nIndex];
            this.index = nIndex;
            if (electric.isBindToPLC)
            {
                int nTask = 0;
                lbDeviceStatus.Text = "设备返回状态：";


                if (electric.returnStruct[nIndex].status == 10)

                    lbDeviceStatus.Text += "允许下发" + "任务；";
                else
                    lbDeviceStatus.Text += "不允许下发" + "任务；";
                lbDeviceStatus.Text += electric.mainFrm.deviceStatusDic.getDesc(electric.deviceType[nIndex], electric.error[nIndex].ToString());
                lbDeviceStatus.Text += "；任务号：" + electric.returnStruct[nIndex].taskID.ToString();
                lbDeviceStatus.Text += "；任务类型：" + electric.returnStruct[nIndex].taskType.ToString();
                lbDeviceStatus.Text += "；起始地址：" + electric.returnStruct[nIndex].from.ToString();
                lbDeviceStatus.Text += "；目的地址：" + electric.returnStruct[nIndex].to.ToString();

                lbDeviceStatus.Text += "，下发任务箱号：" + DataBaseInterface.SelectBoxCode(electric.returnStruct[nIndex].taskID.ToString());

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
                lbLoadStatus.Text += "," + systemStatus.GetAuto(outConveyor.levelNum[nIndex]) + "；";
                lbLoadStatus.Text += "，承载任务箱号：" + DataBaseInterface.SelectBoxCode(electric.loadStruct[nIndex].taskID.ToString());
                nTask = electric.loadStruct[nIndex].taskID;
                //else(electric.returnPermit == 0&&)
                this.btn_Eorror_check.Enabled = true;

            }
            else
            {
                lbLoadStatus.Text = "设备状态：未连接";

            }
        }
        #endregion

        #region RGV
        public FormConveyorCmd(RGV electric, int nIndex, SystemStatus systemStatus)
        {
            InitializeComponent();
            this.rgv = electric;
            this.systemstatus = systemStatus;
            //dbConn = electric.mainFrm.dbConn;
            this.Text = rgv.conveyorName[nIndex];
            this.index = nIndex;
            if (electric.isBindToPLC)
            {
                int nTask = 0;
                lbDeviceStatus.Text = "设备返回状态：";


                if (electric.returnStruct[nIndex].status == 10 || electric.returnStruct[nIndex].status == 11)

                    lbDeviceStatus.Text += "允许下发任务；";
                else
                    lbDeviceStatus.Text += "不允许下发任务；";
                if (nIndex == 0)
                    lbDeviceStatus.Text += electric.mainFrm.deviceStatusDic.getDesc(electric.deviceType[nIndex], electric.error[nIndex].ToString());
                lbDeviceStatus.Text += "；任务号：" + electric.returnStruct[nIndex].taskID.ToString();
                lbDeviceStatus.Text += "；任务类型：" + electric.returnStruct[nIndex].taskType.ToString();
                lbDeviceStatus.Text += "；起始地址：" + electric.returnStruct[nIndex].from.ToString();
                lbDeviceStatus.Text += "；目的地址：" + electric.returnStruct[nIndex].to.ToString();

                lbDeviceStatus.Text += "，下发任务箱号：" + DataBaseInterface.SelectBoxCode(electric.returnStruct[nIndex].taskID.ToString());

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
                lbLoadStatus.Text += "," + systemStatus.GetAuto(rgv.levelNum[nIndex]) + "；";
                lbLoadStatus.Text += "，承载任务箱号：" + DataBaseInterface.SelectBoxCode(electric.loadStruct[nIndex].taskID.ToString());
                nTask = electric.loadStruct[nIndex].taskID;
                //else(electric.returnPermit == 0&&)
               // this.btn_Eorror_check.Enabled = true;
            }
            else
            {
                lbLoadStatus.Text = "设备状态：未连接";

            }
        }
        #endregion

        #region 关闭
        private void btOK_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        #endregion

        #region 清错
        private void btReset_Click(object sender, EventArgs e)
        {
            if (firstConveyorScannerCmd != null)
            {
                firstConveyorScannerCmd.WriteSingleAction(index, 1);
                firstConveyorScannerCmd.nBarcodeError[index] = 0;
            }
            if (firstConveyorCmd != null)
            {
                firstConveyorCmd.WriteSingleAction(index, 1);
                firstConveyorCmd.nBarcodeError[index] = 0;
            }
            if (secondConveyor != null)
            {
                secondConveyor.WriteSingleAction(index, 1);
                secondConveyor.nBarcodeError[index] = 0;
            }
            if (outConveyor != null)
            {
                outConveyor.WriteSingleAction(index, 1);
                outConveyor.nBarcodeError[index] = 0;
            }
            if (rgv != null)
            {
                rgv.WriteSingleAction(index, 1);
                rgv.nBarcodeError[index] = 0;
            }
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
                if (firstConveyorScannerCmd != null)
                {
                    DataBaseInterface.InsertInitLog(firstConveyorScannerCmd.conveyorName[index], firstConveyorScannerCmd.loadStruct[index].taskID, firstConveyorScannerCmd.loadStruct[index].from, firstConveyorScannerCmd.loadStruct[index].to);
                    firstConveyorScannerCmd.WriteSingleAction(index, 2);
                    firstConveyorScannerCmd.nBarcodeError[index] = 0;
                }
                if (firstConveyorCmd != null)
                {
                    DataBaseInterface.InsertInitLog(firstConveyorCmd.conveyorName[index], firstConveyorCmd.loadStruct[index].taskID, firstConveyorCmd.loadStruct[index].from, firstConveyorCmd.loadStruct[index].to);
                    firstConveyorCmd.WriteSingleAction(index, 2);
                    firstConveyorCmd.nBarcodeError[index] = 0;
                }
                if (secondConveyor != null)
                {
                    DataBaseInterface.InsertInitLog(secondConveyor.conveyorName[index], secondConveyor.loadStruct[index].taskID, secondConveyor.loadStruct[index].from, secondConveyor.loadStruct[index].to);
                    secondConveyor.WriteSingleAction(index, 2);
                    secondConveyor.nBarcodeError[index] = 0;
                }
                if (outConveyor != null)
                {
                    DataBaseInterface.InsertInitLog(outConveyor.conveyorName[index], outConveyor.loadStruct[index].taskID, outConveyor.loadStruct[index].from, outConveyor.loadStruct[index].to);
                    outConveyor.WriteSingleAction(index, 2);
                    outConveyor.nBarcodeError[index] = 0;
                }
                if (rgv != null)
                {
                    DataBaseInterface.InsertInitLog(rgv.conveyorName[index], rgv.loadStruct[index].taskID, rgv.loadStruct[index].from, rgv.loadStruct[index].to);
                    rgv.WriteSingleAction(index, 2);
                    rgv.nBarcodeError[index] = 0;
                }

            }
        }
        #endregion

        #region 写任务
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
            if (firstConveyorScannerCmd != null)
            {
                firstConveyorScannerCmd.WriteCmd(index, int.Parse(this.tbWriteId.Text.ToString()), cmbLoadType.SelectedIndex, int.Parse(this.txtFrom.Text.ToString()), int.Parse(this.txtTo.Text.ToString()), cmbTaskType.SelectedIndex);
            }
            if (firstConveyorCmd != null)
            {
                firstConveyorCmd.WriteCmd(index, int.Parse(this.tbWriteId.Text.ToString()), cmbLoadType.SelectedIndex, int.Parse(this.txtFrom.Text.ToString()), int.Parse(this.txtTo.Text.ToString()), cmbTaskType.SelectedIndex);
            }
            if (secondConveyor != null)
            {
                secondConveyor.WriteCmd(index, int.Parse(this.tbWriteId.Text.ToString()), cmbLoadType.SelectedIndex, int.Parse(this.txtFrom.Text.ToString()), int.Parse(this.txtTo.Text.ToString()), cmbTaskType.SelectedIndex);
            }
            if (outConveyor != null)
            {
                outConveyor.WriteCmd(index, int.Parse(this.tbWriteId.Text.ToString()), cmbLoadType.SelectedIndex, int.Parse(this.txtFrom.Text.ToString()), int.Parse(this.txtTo.Text.ToString()), cmbTaskType.SelectedIndex);
            }
            if (rgv != null)
            {
                rgv.WriteCmd(index, int.Parse(this.tbWriteId.Text.ToString()), cmbLoadType.SelectedIndex, int.Parse(this.txtFrom.Text.ToString()), int.Parse(this.txtTo.Text.ToString()), cmbTaskType.SelectedIndex);
            }

        }
        #endregion

        #region 自动
        private void btAuto_Click(object sender, EventArgs e)
        {
            if (outConveyor != null)
            {
                systemstatus.WriteWorkModelCmd(outConveyor.systemStatusID[index] - 1, 1);
            }
            if (firstConveyorScannerCmd != null)
            {
                systemstatus.WriteWorkModelCmd(firstConveyorScannerCmd.systemStatusID[index] - 1, 1);
            }
            if (firstConveyorCmd != null)
            {
                systemstatus.WriteWorkModelCmd(firstConveyorCmd.systemStatusID[index] - 1, 1);
            }
            if (secondConveyor != null)
            {
                systemstatus.WriteWorkModelCmd(secondConveyor.systemStatusID[index] - 1, 1);
            }
            if (rgv != null)
            {
                systemstatus.WriteWorkModelCmd(rgv.systemStatusID[index] - 1, 1);
            }
        }
        #endregion

        #region 手动
        private void btmanul_Click(object sender, EventArgs e)
        {
            if (outConveyor != null)
            {
                systemstatus.WriteWorkModelCmd(outConveyor.systemStatusID[index] - 1, 0);
            }
            if (firstConveyorScannerCmd != null)
            {
                systemstatus.WriteWorkModelCmd(firstConveyorScannerCmd.systemStatusID[index] - 1, 0);
            }
            if (firstConveyorCmd != null)
            {
                systemstatus.WriteWorkModelCmd(firstConveyorCmd.systemStatusID[index] - 1, 0);
            }
            if (secondConveyor != null)
            {
                systemstatus.WriteWorkModelCmd(secondConveyor.systemStatusID[index] - 1, 0);
            }
            if (rgv != null)
            {
                systemstatus.WriteWorkModelCmd(rgv.systemStatusID[index] - 1, 0);
            }
        }
        #endregion

        #region 页面加载
        private void FormConveyor_Load(object sender, EventArgs e)
        {
            if (outConveyor != null)
            {
                CmbControl.Items.Add("--选择点动命令--");
                foreach (var item in outConveyor.mainFrm.ControlCmd(outConveyor.deviceType[index]))
                {
                    CmbControl.Items.Add(item);
                }
                CmbControl.SelectedIndex = 0;
                if (outConveyor.nBarcodeError[index] != 2)
                {
                    btn_Eorror_check.Enabled = false;
                    btn_Eorror_check.ForeColor = Color.Black;
                }
                else
                {
                    btn_Eorror_check.Enabled = true;
                    btn_Eorror_check.ForeColor = Color.Red;
                }
                cmbTaskType.SelectedIndex = 0;
                cmbLoadType.SelectedIndex = 0;
            }
            if (firstConveyorScannerCmd != null)
            {

                CmbControl.Items.Add("--选择点动命令--");
                foreach (var item in firstConveyorScannerCmd.mainFrm.ControlCmd(firstConveyorScannerCmd.deviceType[index]))
                {
                    CmbControl.Items.Add(item);
                }
                CmbControl.SelectedIndex = 0;
                cmbTaskType.SelectedIndex = 0;
                cmbLoadType.SelectedIndex = 0;
            }

            if (firstConveyorCmd != null)
            {

                CmbControl.Items.Add("--选择点动命令--");
                foreach (var item in firstConveyorCmd.mainFrm.ControlCmd(firstConveyorCmd.deviceType[index]))
                {
                    CmbControl.Items.Add(item);
                }
                CmbControl.SelectedIndex = 0;
                cmbTaskType.SelectedIndex = 0;
                cmbLoadType.SelectedIndex = 0;
            }
            if (secondConveyor != null)
            {

                CmbControl.Items.Add("--选择点动命令--");
                foreach (var item in secondConveyor.mainFrm.ControlCmd(secondConveyor.deviceType[index]))
                {
                    CmbControl.Items.Add(item);
                }
                CmbControl.SelectedIndex = 0;

                cmbTaskType.SelectedIndex = 0;
                cmbLoadType.SelectedIndex = 0;
            }
            if (rgv != null)
            {

                CmbControl.Items.Add("--选择点动命令--");
                foreach (var item in rgv.mainFrm.ControlCmd(rgv.deviceType[index]))
                {
                    CmbControl.Items.Add(item);
                }
                CmbControl.SelectedIndex = 0;
                if (rgv.nBarcodeError[index] != 2)
                {
                    btn_Eorror_check.Enabled = false;
                    btn_Eorror_check.ForeColor = Color.Black;
                }
                else
                {
                    btn_Eorror_check.Enabled = true;
                    btn_Eorror_check.ForeColor = Color.Red;
                }
                cmbTaskType.SelectedIndex = 0;
                cmbLoadType.SelectedIndex = 0;
            }

        }
        #endregion



        private void btn_Eorror_check_Click(object sender, EventArgs e)
        {
            string rs;
            if (firstConveyorScannerCmd != null)
            {
                using (MySqlConnection conn = firstConveyorScannerCmd.dbConn.GetConnectFromPool())
                {

                    if (firstConveyorScannerCmd.mainFrm.taskType[0] == 3)//若为退库，则根据操作员的判断
                    {
                        DialogResult dialogResult = MessageBox.Show("请确定当前出库废料是否为需要出立库的废料", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (dialogResult == DialogResult.No)
                        {
                            string taskId = DataBaseInterface.ErrorRebackTaskCreate(conn,DataBaseInterface.GetCurrentBarCode(conn, firstConveyorScannerCmd.scanner_id[index]), firstConveyorScannerCmd.loadStruct[index].taskID, 1, 1, out rs);
                            MessageBox.Show("请先将托盘取下，待退库完成之后统一异常回库！");
                        }
                        else
                            MessageBox.Show("请在主界面手动将出立库任务报完成！");
                    }
                }
              
            }
            if (outConveyor != null) //一楼吨桶出库
            {
                using (MySqlConnection conn = outConveyor.dbConn.GetConnectFromPool())
                {
                    if (systemstatus.GetAuto(outConveyor.systemStatusID[index]) == "手动")
                    {
                        DialogResult dialogResult = MessageBox.Show("请确定当前出库废料是否为需要出立库的废料", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (dialogResult == DialogResult.No)
                        {
                            string taskId = DataBaseInterface.ErrorRebackTaskCreate(conn,DataBaseInterface.GetCurrentBarCode(conn, outConveyor.scanner_id[index]), outConveyor.loadStruct[index].taskID, 1, 1, out rs);
                            MessageBox.Show("请先将托盘取下，去一楼入库口进行异常回库！");
                        }
                        else
                            MessageBox.Show("请在主界面手动将出立库任务报完成！");
                    }
                }
                  
            }
            if (rgv != null)
            {
                using (MySqlConnection conn = rgv.dbConn.GetConnectFromPool())
                {
                    if (systemstatus.GetAuto(rgv.systemStatusID[index]) == "手动")
                    {
                        DialogResult dialogResult = MessageBox.Show("请确定当前出库废料是否为需要出立库的废料", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                        if (dialogResult == DialogResult.No)
                        {
                            string taskId = DataBaseInterface.ErrorRebackTaskCreate(conn, DataBaseInterface.GetCurrentBarCode(conn, rgv.scanner_id[index]), rgv.loadStruct[index].taskID, 2, 1, out rs);
                            MessageBox.Show("请联系兰剑工作人员处理问题！");
                        }
                        else
                            rgv.isSuccess = true;
                    }
                }
                   
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


        #region 执行点动命令
        private void BtnControl_Click(object sender, EventArgs e)
        {
            if (outConveyor != null)
            {
                if (systemstatus.GetAuto(outConveyor.systemStatusID[index]) == "自动")
                {
                    MessageBox.Show("点动命令只能在手动状态下进行");
                }
                else
                {
                    if (CmbControl.SelectedIndex > 0)
                        outConveyor.WriteSingleAction(index, 10 + CmbControl.SelectedIndex - 1);
                }
            }
            if (firstConveyorScannerCmd != null)
            {
                if (systemstatus.GetAuto(firstConveyorScannerCmd.systemStatusID[index]) == "自动")
                {
                    MessageBox.Show("点动命令只能在手动状态下进行");
                }
                else
                {
                    if (CmbControl.SelectedIndex > 0)
                        firstConveyorScannerCmd.WriteSingleAction(index, 10 + CmbControl.SelectedIndex - 1);
                }
            }
            if (firstConveyorCmd != null)
            {
                if (systemstatus.GetAuto(firstConveyorCmd.systemStatusID[index]) == "自动")
                {
                    MessageBox.Show("点动命令只能在手动状态下进行");
                }
                else
                {
                    if (CmbControl.SelectedIndex > 0)
                        firstConveyorCmd.WriteSingleAction(index, 10 + CmbControl.SelectedIndex - 1);
                }
            }
            if (secondConveyor != null)
            {
                if (systemstatus.GetAuto(secondConveyor.systemStatusID[index]) == "自动")
                {
                    MessageBox.Show("点动命令只能在手动状态下进行");
                }
                else
                {
                    if (CmbControl.SelectedIndex > 0)
                        secondConveyor.WriteSingleAction(index, 10 + CmbControl.SelectedIndex - 1);
                }
            }
            if (rgv != null)
            {
                if (systemstatus.GetAuto(rgv.systemStatusID[index]) == "自动")
                {
                    MessageBox.Show("点动命令只能在手动状态下进行");
                }
                else
                {
                    if (CmbControl.SelectedIndex > 0)
                        rgv.WriteSingleAction(index, 10 + CmbControl.SelectedIndex - 1);
                }
            }

        }
        #endregion

        #region 停止点动命令
        private void button1_Click(object sender, EventArgs e)
        {
            if (outConveyor != null)
            {
                outConveyor.WriteSingleAction(index, 0);
            }
            if (firstConveyorScannerCmd != null)
            {
                firstConveyorScannerCmd.WriteSingleAction(index, 0);
            }
            if (firstConveyorCmd != null)
            {
                firstConveyorCmd.WriteSingleAction(index, 0);
            }
            if (secondConveyor != null)
            {
                secondConveyor.WriteSingleAction(index, 0);
            }
            if (rgv != null)
            {
                rgv.WriteSingleAction(index, 0);
            }

        }
        #endregion

        #region 取消入库
        private void btnDel_Click(object sender, EventArgs e)
        {
            string rs;//取消入库
            if (firstConveyorScannerCmd != null)
            {
                DialogResult dialogResult = MessageBox.Show("确定要取消该入库口该托盘的入库任务？", "系统提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);
                if (dialogResult == DialogResult.No)
                    return;
                else
                {
                    string strReturn = DataBaseInterface.TaskCancel(firstConveyorScannerCmd.loadStruct[index].taskID,out rs);
                    if(strReturn=="1")
                    {
                        MessageBox.Show(rs);
                    }
                    else
                    {
                        MessageBox.Show(rs + "，请联系兰剑工作人员查找失败原因！");
                    }
                }
            }

        }
        #endregion
    }
}
