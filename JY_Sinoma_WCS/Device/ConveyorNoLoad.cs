
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpcRcw.Da;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Diagnostics;
using DataBase;
using PLC;

namespace JY_Sinoma_WCS
{
    public class ConveyorNoLoad : Conveyor
    {
        public Thread NoLoadConveyorThread;
        public int[] systemStatusID;
        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainFrm"></param>
        /// <param name="bt"></param>
        public ConveyorNoLoad(frmMain mainFrm, DataTable bt)
            : base(mainFrm, bt)
        {
            int i= 0;
            systemStatusID = new int[nCount];
            foreach (DataRow row in bt.Rows)
            {
                errorDB [i]=row["error_db"].ToString ();
                controlDB [i]=row["control_db"].ToString ();
                nInitX = int.Parse("0"+row["x"].ToString());
                nInitY = int.Parse("0" + row["y"].ToString());
                conveyorName[i] = row["device_tag"].ToString();
                levelNum[i] = int.Parse(row["level_no"].ToString());
                rowNum[i] = int.Parse(row["row_no"].ToString());
                deviceType[i] = row["device_mold"].ToString();
                systemStatusID[i] = int.Parse(row["system_status_id"].ToString());
                //定义图片
                lb[i] = new Label();
                lb[i].Size = new Size( int.Parse(row["length"].ToString()),int.Parse(row["width"].ToString()));
                lb[i].Location = new Point(int.Parse(row["x"].ToString()), int.Parse(row["y"].ToString()));
                lb[i].BackColor = Color.Gray;
                lb[i].Font = new Font("宋体", 10F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(134)));
                lb[i].TextAlign = ContentAlignment.MiddleCenter;
                lb[i].DoubleClick += new System.EventHandler(this.lb_DoubleClick);
                lb[i].Tag = i.ToString();
                if (levelNum[i] == 1)
                    mainFrm.PLevel1.Controls.Add(lb[i]);
                else
                    mainFrm.pLevel21.Controls.Add(lb[i]);
                lb[i].BringToFront();
                i++;
            }
        }
        #endregion 

        #region 连接PLC
        /// <summary>
        /// 连接PLC
        /// </summary>
        /// <returns></returns>
        public override bool BindToPLC()
        {
            if (!SyncAddGroup()) return false;
            if (!AsyncAddGroup()) return false;
            int client = 1;
           
           OpcRcw.Da.OPCITEMDEF[] errorItems=new OPCITEMDEF [errorDB .Length ];
           for (int i = 0; i < errorDB.Length; i++)
           {
               errorItems[i].szAccessPath = "";
               errorItems[i].bActive = 1;
               errorItems[i].hClient = client;
               errorItems[i].dwBlobSize = 1;
               errorItems[i].pBlob = IntPtr.Zero;
               errorItems[i].vtRequestedDataType = (int)VarEnum.VT_I2;
               errorItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}",errorDB [i]);
               errorClientHandle[i] = client;
               client++;
           }
           if (!AsyncAddItems(errorItems, errorHandle)) return false;
          
           OpcRcw.Da.OPCITEMDEF[] controlItems = new OPCITEMDEF[controlDB.Length];
           for (int i = 0; i < controlDB.Length; i++)
           {
               controlItems[i].szAccessPath = "";
               controlItems[i].bActive = 1;
               controlItems[i].hClient = client;
               controlItems[i].dwBlobSize = 1;
               controlItems[i].pBlob = IntPtr.Zero;
               controlItems[i].vtRequestedDataType = (int)VarEnum.VT_I2;
               controlItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}",controlDB[i]);
               client++;
           }
           if (!SyncAddItems(controlItems, controlHandle)) return false;

           //开始接收订阅数据项的事件
           SetState(true);
           isBindToPLC = true;


           return isBindToPLC;
        }
        #endregion 

        #region 刷新辊道状态
        /// <summary>
        /// 刷新辊道状态
        /// </summary>
        public override void RefreshStatus()
        {
            while (!closing)
            {
                if (!isBindToPLC)
                {
                    if (!BindToPLC())
                    {
                        mainFrm.DeviceDisConnection();
                        mainFrm.SetClosing(true);
                    }
                }
                Thread.Sleep(200);
            }
        }
        #endregion

        #region 断开PLC
        /// <summary>
        /// 断开PLC
        /// </summary>
        public override void DisConnectPLC()
        {
            if (isBindToPLC)
            {
                //NoLoadConveyorThread.Abort();
                DisConnect();
                isBindToPLC = false;
            }
        }
        #endregion 

        #region 标签的双击事件
        public override void lb_DoubleClick(object sender, EventArgs e)
        {
            string strTag = Convert.ToString(((Label)sender).Tag);
            int Index = int.Parse(strTag);
            FormConveyorNoLoad frmConveyor = new FormConveyorNoLoad(this, Index, mainFrm.systemStatus);
            frmConveyor.ShowDialog();
        }
        #endregion  
        
        #region 显示出库站台图片
        /// <summary>
        /// 刷新辊道图片
        /// </summary>
        public override void DisplayStatus()
        {
            try
            {
                for (int i = 0; i < this.nCount; i++)
                {
                    if (!isBindToPLC)
                        lb[i].BackColor = Color.DeepSkyBlue;
                    else
                    {
                        if (error[i] == 0)
                        {
                            if (lb[i].BackColor == Color.Red)
                                DataBaseInterface.DeviceErrorMessage(conveyorName[i], levelNum[i], 0, deviceType[i], error[i], mainFrm.deviceStatusDic.getDesc(deviceType[i], error[i].ToString()), 0);
                            if (systemstatus.GetAuto(levelNum[i]) == "自动")
                            {
    
                                    lb[i].BackColor = Color.Green;
                            }
                            else
                                lb[i].BackColor = Color.LightGreen ;
                        }
                        else 
                        {
                            lb[i].BackColor = Color.Red;
                            if (lastError[i] != error[i])
                            {
                              //  mainFrm.speech.speech("辊道编号" + this.conveyorName[i].ToString() + mainFrm.ConveyorError(deviceType[i], error[i]));
                                if (error[i] != 15)
                                    DataBaseInterface.DeviceErrorMessage(conveyorName[i], levelNum[i], 0, deviceType[i], error[i], mainFrm.deviceStatusDic.getDesc(deviceType[i], error[i].ToString()), 0);
                            }
                        }
                        lastError[i] = error[i];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion
    }
}
