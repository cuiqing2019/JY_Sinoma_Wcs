using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PLC;
using OpcRcw.Da;
using System.Data;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;
using System.Diagnostics;
using DataBase;
using System.Collections;
using MySql.Data.MySqlClient;
using System.Net.Sockets;
using System.Net;

namespace JY_Sinoma_WCS
{
    public class ReadBoxCode
    {

        #region 自定义变量
        frmMain mainFrm;
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
        string strIp;
        int nPort;
        string startPoint = Encoding.ASCII.GetString(new byte[1] { 0x02 });
        string EndPoint = Encoding.ASCII.GetString(new byte[1] { 0x03 });
        private Thread spThread;
        /// <summary>
        /// socket的客户端
        /// </summary>
        public Socket ClientSocket;  
        /// <summary>
        /// 接收字节数组
        /// </summary>
        public Byte[] returnBytes = new Byte[1024];
        /// <summary>
        /// 前次多读出的部分条码
        /// </summary>
        private string lastBarcode = string.Empty;
        #endregion

        #region 构造函数
        public ReadBoxCode(string readerName, string readerIP, frmMain mainFrm, int level,int port)
        {
            this.level = level;
            this.mainFrm = mainFrm;
            this.readerName = readerName;
            this.mainFrm = mainFrm;
            this.readerName = readerName;
            #region 获取本机IP

            //本机地址信息
            System.Net.IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(Dns.GetHostName());
            IPEndPoint IEP = null;
            //循环判断获取IP4地址
            for (int icount = 0; icount < ipEntry.AddressList.Length; icount++)
            {
                if (ipEntry.AddressList[icount].AddressFamily == AddressFamily.InterNetwork)
                {
                    IEP = new IPEndPoint(ipEntry.AddressList[icount], port);
                    break;
                }
            }
            #endregion
            if (IEP != null)
            {
                strIp = IEP.Address.ToString();
                nPort = port;
                spThread = new Thread(new ThreadStart(Scanning));
                spThread.IsBackground = true;
                if (!spThread.IsAlive)
                    spThread.Start();
            }
            //Connt();
        }
        #endregion 

        #region 读取条码器信息并保存
        /// <summary>
        /// 读数据
        /// </summary>
        public void Scanning()
        {
            string barCodes;
            string scannerNo;//扫描器编号
            string shortBarCode;
            string tempBarCodes;
         
          
            Int64 lastTime;

            while (true)
            {
                #region 连接Socket
                if (ClientSocket != null && ClientSocket.Connected) //连接测试，已确认已保持连接
                {
                    try
                    {
                        bool blockingState = ClientSocket.Blocking;
                        byte[] tmp = new byte[1];
                        tmp[0] = 0x02;
                        ClientSocket.Blocking = false;
                        ClientSocket.Send(tmp, 1, 0);
                        ClientSocket.Blocking = blockingState;
                        ClientSocket.SendTimeout = 500;
                        ClientSocket.ReceiveTimeout = 500;
                    }
                    catch (Exception)
                    {
                        ClientSocket = null;
                        Thread.Sleep(1000);
                        continue;
                    }
                }
                else
                {
                    Connect();
                    Thread.Sleep(1000);
                }
                #endregion 

                if (ClientSocket != null && ClientSocket.Connected)
                {
                    barCodes = lastBarcode;
                    int ncount = 0;
                    while (!(barCodes.Contains(startPoint) && barCodes.Contains(EndPoint)))  //1S收一次
                    {

                        if (ClientSocket.Available > 0)
                        {
                            Array.Clear(returnBytes, 0, returnBytes.Length);
                            ClientSocket.Receive(returnBytes);
                            tempBarCodes = Encoding.ASCII.GetString(returnBytes);
                            barCodes += tempBarCodes;
                            int nZero = barCodes.IndexOf("\0");
                            if (nZero >= 0)
                                barCodes = barCodes.Substring(0, nZero);
                            if (barCodes.Length == 0) break;
                            if (!(barCodes.Contains(startPoint) && barCodes.Contains(EndPoint)))
                                Thread.Sleep(200);
                        }
                        ncount++;
                        if (ncount > 5)
                        {
                            break;
                        }
                    }

                    if (barCodes.Length > 0)
                    {
                        //处理barcodes的尾部存在不完整的条码
                        int startPos = barCodes.LastIndexOf(startPoint);
                        int stopPos = barCodes.LastIndexOf(EndPoint);
                        if (startPos > stopPos)  //barcodes的尾部存在不完整的条码
                            lastBarcode = barCodes.Substring(startPos);
                        else
                            lastBarcode = string.Empty;
                    }
                    if (barCodes.Length > 0)//最短为2位编号+“NoRead”+起始符+结束符，9位
                    {
                        string[] codes = barCodes.Split(EndPoint.ToCharArray()[0]); //一次可能读出多个条码，  为条码结束符
                        foreach (string bar in codes)
                        {
                            if (bar.Length < 8) continue;
                            int start = bar.IndexOf(startPoint); //查找起始符的位置
                            int end = bar.Length - 1;//查找终止符的位置
                            scannerNo = bar.Substring(start + 1, 2);//获取扫描器编号
                            if (readerName.PadLeft(2, '0') != scannerNo)
                            {
                                barCodes = string.Empty;
                                break;
                            }
                            if (bar.Contains("NoRead")) //没有读出条码
                                shortBarCode = "NoRead";
                            else
                            {
                                if (start >= 0)
                                    shortBarCode = bar.Substring(start + 3, end - start - 2);
                                else
                                    shortBarCode = " ";
                            }
                            if (shortBarCode.Length > 0 || (shortBarCode.Contains("NoRead")))
                            {
                                string rs = string.Empty;
                                DataBaseInterface.SaveCurrentBarcode(shortBarCode, level, int.Parse(readerName), 1, out rs);
                            }
                        }
                    }
                }
                Thread.Sleep(300);
            }
            //mainFrm.ChangeTreadNum(-1);
        }
        #endregion 

     

        #region 连接socket
        /// <summary>
        /// 连接socket
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            if (ClientSocket != null && ClientSocket.Connected) return true;
            if (ClientSocket != null) ClientSocket = null;
            try
            {
                ClientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                ClientSocket.Connect(new IPEndPoint(IPAddress.Parse(strIp), nPort));
                Console.WriteLine("已经连接服务端！");
            }
            catch (Exception ex)
            {
                return false;
            }
            bool blockingState = ClientSocket.Blocking;
            byte[] tmp = new byte[1];
            tmp[0] = 0x02;
            ClientSocket.Blocking = false;
            ClientSocket.Send(tmp, 1, 0);
            ClientSocket.Blocking = blockingState;
            ClientSocket.SendTimeout = 500;
            ClientSocket.ReceiveTimeout = 500;
            return ClientSocket.Connected;
        }
        #endregion

    }
}
