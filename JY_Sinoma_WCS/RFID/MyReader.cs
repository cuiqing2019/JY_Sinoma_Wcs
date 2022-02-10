using System;
using System.Data;
using System.Net.NetworkInformation;
using System.Windows.Forms;
using Core = Invengo.NetAPI.Core;
using IRP1 = Invengo.NetAPI.Protocol.IRP1;
using System.Threading;
using System.Collections.Generic;
using System.Text;
using OpcRcw.Da;
using System.Runtime.InteropServices;
using System.Drawing;
using System.Diagnostics;
using DataBase;
using System.Collections;

namespace JY_Sinoma_WCS
{
    public class MyReader
    {
        frmMain mainFrm;
        IRP1.Reader reader;
        IRP1.ReadTag scanMsg = new IRP1.ReadTag(IRP1.ReadTag.ReadMemoryBank.EPC_TID_UserData_Reserved_6C_ID_UserData_6B);//扫描消息
        public DataTable myDt = new DataTable();//显示扫描数据
        public object lockobj = new object();//显示数据锁定
        public string readerName;
        bool isTryReconnNet = false;
        int tryReconnNetTimeSpan;
        DateTime timeStart;
        DateTime timeNow;
        public bool isRead = true;
        public int level;
        public string rs;
        public bool isStart = false;
        public MyReader(string readerName,string readerIP,frmMain mainFrm,int level)
        {
            this.level = level;
            this.mainFrm = mainFrm;
            this.readerName = readerName;
            reader = new IRP1.Reader(readerName, "TCPIP_Client", readerIP);//网口
            myDt.Columns.Add("TID", typeof(string)); //数据类型为 文本
            IRP1.Reader.OnApiException += new Core.ApiExceptionHandle(Reader_OnApiException);
            Connt();
        }
        // 建立连接
        public void Connt()
        {
            if (reader.Connect())
            {
                //注册接收读写器消息事件
                reader.OnMessageNotificationReceived += new Invengo.NetAPI.Core.MessageNotificationReceivedHandle(reader_OnMessageNotificationReceived);
                mainFrm.SetLabelText(readerName, "已连接");
            }
            else
            {
                MessageBox.Show("扫码器"+reader.ReaderName+"连接失败");
            }
        }

        /// <summary>
        /// 接收到读写器消息触发事件
        /// </summary>
        /// <param name="reader">读写器类</param>
        /// <param name="msg">消息内容</param>
        void reader_OnMessageNotificationReceived(Invengo.NetAPI.Core.BaseReader reader, Invengo.NetAPI.Core.IMessageNotification msg)
        {
            if (msg.StatusCode != 0)
            {
                //显示错误信息
                MessageBox.Show(msg.ErrInfo);
                return;
            }
            String msgType = msg.GetMessageType();
            msgType = msgType.Substring(msgType.LastIndexOf('.') + 1);
            switch (msgType)
            {
                #region RXD_TagData
                case "RXD_TagData":
                    {
                        IRP1.RXD_TagData m = (IRP1.RXD_TagData)msg;
                        string tagType = m.ReceivedMessage.TagType;
                        displayMethod(m);
                    }
                    break;
                #endregion
                #region RXD_IOTriggerSignal_800
                case "RXD_IOTriggerSignal_800":
                    {
                        IRP1.RXD_IOTriggerSignal_800 m = (IRP1.RXD_IOTriggerSignal_800)msg;
                        if (m.ReceivedMessage.IsStart)
                        {
                            ScanStopRead();
                            MessageBox.Show("读写器I/O被触发");
                        }
                        break;
                    }
                #endregion
                default: break;
            }
        }


        private void displayMethod(IRP1.RXD_TagData msg)
        {
            lock (lockobj)
            {
                ////如果不是空托盘入库，则只读取一个托盘的托盘号
                //if (mainFrm.taskType != 2)
                //{
                //    string rs;
                //    string tid = Core.Util.ConvertByteArrayToHexString(msg.ReceivedMessage.TID);
                //    DataBaseInterface.SaveCurrentBarcode(tid, level, int.Parse(reader.ReaderName),1, out rs);
                //    ScanStopRead();
                //}
                //else//如果是空托盘入库，则需要读取8个托盘的托盘号 
                //{
                //    bool isAdd = true;
                //    string tid = Core.Util.ConvertByteArrayToHexString(msg.ReceivedMessage.TID);
                //    if (myDt.Rows.Count >= 8)
                //    {
                //        ScanStopRead();
                //        DataBaseInterface.SaveCurrentBarcode(myDt.Rows[0]["TID"].ToString(), myDt.Rows[1]["TID"].ToString(), myDt.Rows[2]["TID"].ToString(), myDt.Rows[3]["TID"].ToString(), myDt.Rows[4]["TID"].ToString(), myDt.Rows[5]["TID"].ToString(), myDt.Rows[6]["TID"].ToString(), myDt.Rows[7]["TID"].ToString(), level, int.Parse(readerName),1, out rs);

                //        if (rs == string.Empty)
                //        {
                //            myDt = new DataTable();
                //            myDt.Columns.Add("TID", typeof(string)); //数据类型为 文本
                //        }
                //        return;
                //    }
                //    foreach (DataRow dr in myDt.Rows)
                //    {
                //        if (dr["TID"] != null && dr["TID"].ToString() != "" && dr["TID"].ToString() == tid)
                //            isAdd = false;
                //    }
                //    if (isAdd)
                //    {
                //        if (DataBaseInterface.SelectTrayCount(tid) == 0)
                //        {
                //            DataRow mydr = myDt.NewRow();
                //            mydr["TID"] = tid;
                //            myDt.Rows.Add(mydr);
                //            if (reader != null && reader.IsConnected && myDt.Rows.Count >= 8)
                //            {
                //                if (!reader.Send(new IRP1.PowerOff()))//发送关功放消息
                //                    MessageBox.Show("扫码器扫码故障！");
                //            }
                //        }
                //    }
                   
                //}
                string rs;
                string tid = Core.Util.ConvertByteArrayToHexString(msg.ReceivedMessage.TID);
                DataBaseInterface.SaveCurrentBarcode(tid, level, int.Parse(reader.ReaderName), 1, out rs);
                ScanStopRead();

            }
        }

        /// <summary>
        /// 开始读标签
        /// </summary>
        public void ScanStartRead()
        {
            if (reader != null && reader.IsConnected && isRead && !isStart)
                if (!reader.Send(scanMsg))
                    MessageBox.Show("RFID开功放出错:" + readerName);
                else
                    isStart = true;
        }
        /// <summary>
        /// 停止读标签
        /// </summary>
        public void ScanStopRead()
        {
            if (reader != null && reader.IsConnected && isStart)
                if (!reader.Send(new IRP1.PowerOff()))//发送关功放消息
                    MessageBox.Show("RFID关功放出错:"+readerName);
                else
                    isStart = false;
        }

        public void UpdateRfidWorkPower(int power)
        {
            //Byte[] param = new Byte[2];
            //param[0] = (Byte)(power / 256);
            //param[1] = (Byte)(power % 256);
            //IRP1.PowerParamConfig_500 order = new IRP1.PowerParamConfig_500(0x00, param);//设置
            //if (!reader.Send(order))
            //    MessageBox.Show("RFID设置功率出错");
           
        }

        void Reader_OnApiException(Core.ErrInfo e)
        {
            MessageBox.Show(e.Ei.ErrMsg+"："+readerName);
        }
    }
}
 