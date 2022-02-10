
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Windows.Forms;
using System.Drawing;
using System.Threading;
using System.Runtime.InteropServices;
using OpcRcw.Da;
using PLC;
using MySql.Data.MySqlClient;
using DataBase;
using JY_Sinoma_WCS;

namespace Device
{
    public class diepanCount : OPCServer
    {
        #region 自定义变量
        public frmMain mainFrm;
        public ConnectPool dbConn;//定义数据库连接
        public static int pickCount = 1;
        private string[] PickStatusDB = new string[pickCount];         //拣选台急停的DB块地址
        public string[] PickSdeviceType = new string[pickCount];

        private int[] PickstatusHandle = new int[pickCount];
        private int[] PickClientHandle = new int[pickCount];
        public Thread SystemThread;
        Thread DCSTread;

        public struct PICKStatusStruct//load块读取数据
        {

            public int PickStopSpot;       //拣选台急停（现场）
        }
        public static PICKStatusStruct[] PICKStatusStructS = new PICKStatusStruct[pickCount];

        private bool isBindToPLC = false;  //是否已初始化OPCServer
        #endregion

        #region 构造函数
        public diepanCount(frmMain mainFrm, DataTable bt)
        {
            int i;
            this.mainFrm = mainFrm;
            PickSdeviceType = new string[bt.Rows.Count];
            i = 0;
            foreach (DataRow row in bt.Rows)
            {
                PickSdeviceType[i] = row["device_id"].ToString();
                PickStatusDB[i] = row["count_db"].ToString();
                i++;
            }

        }
        #endregion
      

        public bool BindToPLC()
        {
            if (!AsyncAddGroup()) return false;
            int client = 1;

            OpcRcw.Da.OPCITEMDEF[] PickstatusItems = new OPCITEMDEF[1];
            for (int i = 0; i <= 0; i++)
            {
                PickstatusItems[i].szAccessPath = "";
                PickstatusItems[i].bActive = 1;
                PickstatusItems[i].hClient = client;
                PickstatusItems[i].dwBlobSize = 1;
                PickstatusItems[i].pBlob = IntPtr.Zero;
                PickstatusItems[i].vtRequestedDataType = (int)VarEnum.VT_I2;
                PickstatusItems[i].szItemID = string.Format("S7:[S7 connection_1]{0}", PickStatusDB[i]); //计数DB
                PickClientHandle[i] = client;
                client++;
            }

            if (!AsyncAddItems(PickstatusItems, PickClientHandle)) return false;

            //开始接收订阅数据项的事件
            SetState(true);
            isBindToPLC = true;


            return isBindToPLC;

        }

   

        /// <summary>
        /// 非命令设备订阅返回的信息
        /// </summary>
        public override void OnDataChange(int dwTransid, int hGroup, int hrMasterquality, int hrMastererror, int dwCount, int[] phClientItems, object[] pvValues, short[] pwQualities, OpcRcw.Da.FILETIME[] pftTimeStamps, int[] pErrors)
        {
           
            for (int i = 0; i < phClientItems.Length; i++)
            {

                for (int j = 0; j < PickClientHandle.Length; j++)
                {


                    if (phClientItems[i] == PickClientHandle[j])
                    {
                        PICKStatusStructS[0].PickStopSpot = int.Parse(pvValues[i].ToString());

                    }
                }
            }


                }

      


        public void DisConnectPLC()
        {
            if (isBindToPLC)
            {
              
                DisConnect();
                isBindToPLC = false;
            }
        }

    }
}