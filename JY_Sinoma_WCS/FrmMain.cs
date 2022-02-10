using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Threading;
using System.Speech.Synthesis;
using System.Speech;
using MySql.Data.MySqlClient;
using DataBase;
using JY_Sinoma_WCS.Device;
using JY_Sinoma_WCS.Forms;
using System.Diagnostics;
using Device;

namespace JY_Sinoma_WCS
{
    public partial class frmMain : Form
    {
        #region 全局变量
        /// <summary>
        /// db是否连接成功
        /// </summary>
        public bool dbIsConned = false;
        /// <summary>
        /// 设备是否连接成功
        /// </summary>
        public bool deviceLinkIsOK = false;
        /// 定义语音对象
        /// </summary>
        //public Speech speech;
        /// <summary>
        /// 创建缓存图片
        /// </summary>
        public Bitmap bBuffer;
        /// <summary>
        /// 定义扫码信息列表
        /// </summary>
        private DoubleBufferListView[] lvScanInfo = new DoubleBufferListView[9];
        /// <summary>
        /// 创建显示画布
        /// </summary>
        public Graphics gDisplay;
        /// <summary>
        /// 是否开始分拣
        /// </summary>
        public bool bPicking = false;
        /// <summary>
        /// 定义数据库连接
        /// </summary>
        public ConnectPool dbConn;
        /// <summary>
        /// 刷新锁
        /// </summary>
        public object[] obLockLv = new object[9];
        /// <summary>
        /// 是否关闭系统
        /// </summary>
        public bool closing = false;
        /// <summary>
        /// 语音库
        /// </summary>
        ///         /// <summary>
        /// 定义状态刷新线程
        /// </summary>
        private Thread thrAutoRun;
        /// <summary>
        /// 定义订单监控界面列表刷新线程
        /// </summary>
        private Thread thrRefreshList;
        /// <summary>
        /// 创建缓存画布
        /// </summary>
        public Graphics gBuffer;
        private string voiceLib = "Girl XiaoKun";
        /// <summary>
        /// 设备状态字典用
        /// </summary>
        public struct DeviceStatus  //设备状态字典用
        {
            /// <summary>
            /// 状态名称
            /// </summary>
            public String statusDesc;
            /// <summary>
            /// 状态性质
            /// </summary>
            public String statusKind;
        }
        /// <summary>
        /// 语速
        /// </summary>
        private int voiceRate = 2;
        /// <summary>
        /// 音量
        /// </summary>
        private int voiceVolume = 100;
        private bool voice_inited = false;//语音初始化
        private SpeechSynthesizer synth; //语音合成对象
        public bool bClosing = false;

        #region 入库扫码器信息结构体定义
        /// <summary>
        /// 入库扫码器信息结构体
        /// </summary>
        public struct InScanInfo
        {
            /// <summary>
            /// 扫码器编号
            /// </summary>
            public int EntryId;
            /// <summary>
            /// 扫码器名称
            /// </summary>
            public string ScanName;
            /// <summary>
            /// 扫码器类型
            /// </summary>
            public string ScanType;
            /// <summary>
            /// 入库层、巷道
            /// </summary>
            public int nEntryLev;
        }

        #endregion
        /// <summary>
        /// 设备状态字典
        /// </summary>
        public DeviceStatusDic deviceStatusDic = new DeviceStatusDic();
        /// <summary>
        /// 系统状态
        /// </summary>
        public SystemStatus systemStatus;

        public diepanCount diepanCount;
        /// <summary>
        /// 一楼入库扫码段任务生成，放行辊道实例
        /// </summary>
        public FirstFloorInConveyorScannerCmd inConveyorScannerCmd;
        /// <summary>
        /// 一楼入库顶升移载写任务段
        /// </summary>
        public FirstFloorInConveyorCmd inConveyorCmd;
        /// <summary>
        /// 二楼盘库口任务生成，放行辊道实例
        /// </summary>
        public SecondFloorInConveyorCmd panConveyorCmd;
        /// <summary>
        /// 转载车实例
        /// </summary>
        public RGV rgv;
        /// <summary>
        /// 堆垛机取货站台
        /// </summary>
        public ElectricsStationInConveyor inElectricsConveyor;
        /// <summary>
        /// 堆垛机放货站台
        /// </summary>
        public ElectricsConveyorCmd outElectricsConveyor;
        /// <summary>
        /// 堆垛机放货站台
        /// </summary>
        public ElectricsConveyorAll allElectricsConveyor;
        /// <summary>
        ///普通带load块辊道实例
        /// </summary>
        public ConveyorLoad conveyorLoad;
        /// <summary>
        ///带重量load块辊道实例
        /// </summary>
        public ConveyorWeightLoad conveyorWeightLoad;
        /// <summary>
        ///不带LOAD块实例
        /// </summary>
        public Conveyor conveyorNoLoad;
        /// <summary>
        ///出库线程,*lu
        /// </summary>
        public OutAssign outAssign;
        /// <summary>
        ///拆盘机
        /// </summary>
        public Tray trayAssign;
        /// <summary>
        ///AGV
        public AGV agv;
        /// <summary>
        /// 扫码器设备字典
        /// </summary>
        public Dictionary<string, MyReader> ReadBarCodeFromSPs = new Dictionary<string, MyReader>();
        /// <summary>
        /// 定义出库任务列表  
        /// </summary>
        private DoubleBufferListView LvIOTask = new DoubleBufferListView();
        /// <summary>
        /// 入库任务列表刷新锁
        /// </summary>
        private object obIOTask = new object();
        /// <summary>
        /// 定义盘库任务列表  
        /// </summary>
        private DoubleBufferListView PanTask = new DoubleBufferListView();
        /// <summary>
        /// 定义设备任务列表  
        /// </summary>
        private DoubleBufferListView LvDeviceTask = new DoubleBufferListView();
        /// <summary>
        /// 设备任务列表刷新锁
        /// </summary>
        private object obDeviceTask = new object();
        /// <summary>
        ///堆垛机设备状态字典
        /// </summary>
        public Dictionary<int, Stacker> Stackers = new Dictionary<int, Stacker>();
        /// <summary>
        /// 抽液处及等待区实例
        /// </summary>
        public Pumping pumping;
        /// <summary>
        /// 出库辊道实例
        /// </summary>-l
        public OutConveyorCmd outConveyorCmd;
        /// <summary>
        /// 是否允许界面自动跳转
        /// </summary>
        public bool bAutoChangeForm = true;
        /// <summary>
        /// 历史列表文本
        /// </summary>
        public string lastText;
        /// <summary>
        ///当前任务类型 1-出入库 2-空托盘入库 4-异常回库 3-退库
        /// </summary>
        public int[] taskType;
        /// <summary>
        /// 货物类型 1-吨桶 2-圆桶 3-空托盘组
        /// </summary>
        public int[] goodsKind;
        /// <summary>
        /// 废物派车单单批次编号
        /// </summary>
        public string[] batchNo ;
        /// <summary>
        /// 物料编码
        /// </summary>
        public string[] goodsSku ;
        /// <summary>
        /// 物料名称
        /// </summary>
        public string[] goodsName ;
        /// <summary>
        /// 废料危险等级
        /// </summary>
        public string[] hazardArea ;
        /// <summary>
        /// 时间
        /// </summary>
        public string Maketime;
        /// <summary>
        /// 废物接运单明细id
        /// </summary>
        public string[] batchid ;
        /// <summary>
        /// 批次总重
        /// </summary>
        public double batchWeight = 0;
        /// <summary>
        /// 入库方式 0-正常入库 1-人工入库
        /// </summary>
        public int[] dealWay;
        /// <summary>
        /// 是否停止任务生成
        /// </summary>
        public bool[] stopTaskCreate ; /// <summary>
                                           /// 批次预计总重
                                           /// </summary>
        public double batchWillWeight = 0;
        /// <summary>
        /// AGV任务生成
        /// </summary>
        public bool AGVTaskCreate = false;
        /// <summary>
        /// 母托盘回库模式：1 出8个任务，母托盘自动回库 2：人工取下托盘，待出库任务完成后，托盘由人工叉车放置到输送线上回库
        /// </summary>
        public int emptyPalletType = 1;
        /// <summary>
        /// 母托盘自动回库模式下，出库任务数量统计
        /// </summary>
        public int outTaskNum = 0;
        /// <summary>
        /// 是否允许切换空托入库模式
        /// </summary>
        public bool isEmptyPalletIn = false;
        /// <summary>
        /// 切换空托入库模式后，是否执行过空托入库任务
        /// </summary>
        public bool isHaveEmptyPalletTask = false;

        public bool PumpingStaus;

        #endregion

        #region 构造函数
        public frmMain()
        {
            //ThreadExceptionDialog.CheckForIllegalCrossThreadCalls = false;
            InitializeComponent();
            taskType = new int[2];
            stopTaskCreate = new bool[2];
            goodsKind = new int[2];
            goodsSku = new string[2];
            batchNo = new string[2];
            batchid = new string[2];
            goodsName = new string[2];
            dealWay = new int[2];
            hazardArea = new string[2];
            for(int i=0;i<2;i++)
            {
                taskType[i] = 0;
                goodsKind[i] = 0;
                goodsSku[i] = string.Empty;
                batchNo[i] = string.Empty;
                batchid[i] = string.Empty;
                goodsName[i] = string.Empty;
                dealWay[i] = 0;
                hazardArea[i] = string.Empty;
                stopTaskCreate[i] = true;
            }
        }
        #endregion

        #region 数据库连接按钮
        private void tsbConnectDB_Click(object sender, EventArgs e)
        {
            dbIsConned = DBConnection();
            setMenu();
          
        }
        #endregion

        #region 连接数据库，初始化设备状态字典
        private bool DBConnection()
        {
            dbConn = new ConnectPool(DataBase.MySqlHelper.LocalConnectionString, 100, 50, 60);
            if (!dbConn.StartService())
                return false;
            DataBaseInterface.dbConn = dbConn;
            try
            {
                InitDic();
                SetLabelText("DB", "已连接");
                return true;
            }
            catch (Exception)
            {
                SetLabelText("DB", "未连接");
                return false;
            }
        }
        #endregion

        #region 初始化设备状态字典
        private void InitDic()
        {
            if (dbConn == null)
                return;
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return;
                DataSet ds = new DataSet(); 
                try
                {
                    ds = DataBaseInterface.GetDic(conn, "concat_ws(',',status_type,status_id),status_desc", "td_plt_device_status_dic", "", "status_type,status_id");

                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        deviceStatusDic.Add(row.ItemArray[0].ToString(), row["status_desc"].ToString());
                    }

                }
                catch(Exception ex)
                {
                    MessageBox.Show("初始化设备字典失败："+ex.Message.ToString());
                }
            }

        }
        #endregion

        #region 菜单栏按钮属性设置
        private void setMenu()
        {
            this.tsbConnectDB.Enabled = !dbIsConned;
            this.tsbLinkDevice.Enabled = !deviceLinkIsOK && dbIsConned;
            this.tsbStart.Enabled = deviceLinkIsOK && dbIsConned && !this.bPicking;
            this.tsbStop.Enabled = deviceLinkIsOK && dbIsConned && this.bPicking;
            this.tsbDeviceSet.Enabled = dbIsConned;
            this.tsbDtaskQuery.Enabled = dbIsConned;
            this.tsbGoodsQuery.Enabled = dbIsConned;
            this.tsbScanner.Enabled = dbIsConned;
            this.toolStripButton1.Enabled = dbIsConned;
            this.tsbErrorQuery.Enabled = dbIsConned;
            this.tsb_not_input.Enabled = dbIsConned;
            this.tsbTaskInfo.Enabled = dbIsConned;
            this.toolStripButton2.Enabled = dbIsConned;
            this.tspInPortSet.Enabled = dbIsConned;
            this.tspPalletSet.Enabled = dbIsConned;
            this.tsbRebackLevel1.Enabled = dbIsConned;
            this.toolStripButton4.Enabled = dbIsConned;
            this.toolStripButton3.Enabled = dbIsConned && deviceLinkIsOK;
        }
        #endregion

        #region 连接设备按钮
        private void tsbLinkDevice_Click(object sender, EventArgs e)
        {
            Form frm = new Form();
            frm.Text = "正在连接设备，请耐心等待....";
            frm.ClientSize = new System.Drawing.Size(292, 0);
            frm.ControlBox = false;
            frm.MaximizeBox = false;
            frm.MinimizeBox = false;
            frm.FormBorderStyle = FormBorderStyle.FixedToolWindow;
            frm.StartPosition = FormStartPosition.CenterScreen;
            frm.Show();
            deviceLinkIsOK = DeviceConnection();
         
            frm.Close();
            if (!deviceLinkIsOK)
            {
                SetLabelText("PLC", "未连接");
                MessageBox.Show("设备连接失败");
                return;
            }
            else
            {
                BindDevice();
                SetLabelText("PLC", "已连接");
                setMenu();
                if(thrRefreshList==null)
                {
                    thrRefreshList = new Thread(new ThreadStart(this.RefreshListViews));
                    thrRefreshList.IsBackground = true;
                }
                if (!thrRefreshList.IsAlive) thrRefreshList.Start();
            }
        }
        #endregion

        #region 加载设备

        private bool DeviceConnection()
        {
            using (MySqlConnection conn = dbConn.GetConnectFromPool())
            {
                if (conn == null)
                    return false;
                DataSet ds = new DataSet();
                try
                {
                    #region 系统状态
                    ds = DataBaseInterface.GetDic(conn, "device_id,device_type,device_name,status_db,x,y,length,width,workmode_db,faultclear_db,dataclear_db,taskmode_db,heart_db,count_db", "td_plt_system_dic", "", "device_id");
                    systemStatus = new SystemStatus(this, ds.Tables[0]);
                    #endregion

                    #region 累加叠盘数量
                    ds = DataBaseInterface.GetDic(conn, "count_db,device_id", "td_plt_system_dic", " and device_id='8'", "device_id");
                    diepanCount = new diepanCount(this, ds.Tables[0]);
                    #endregion



                    ds = DataBaseInterface.GetDic(conn, "device_id,device_name,channel_num,use_status,write_db,return_db,x,y,width,length,task_status", "td_stack_dic", "", "device_id");
                    if (ds.Tables[0].Rows.Count > 0)
                    {
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            Stacker device = new Stacker(this, row);
                            if (Stackers.ContainsKey(device.deviceId))
                                continue;
                            Stackers.Add(device.deviceId, device);//将设备添加到设备字典中
                            device.RefreshDisplay();
                        }
                    }

                    #region 初始化RFID
                    ds = DataBaseInterface.GetDic(conn, "device_id,device_type,ip,port,level_no,device_name,scanner_id,use_status,device_desc", "td_plt_scan_dic", "", "device_id");
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        MyReader scan = new MyReader(row["device_id"].ToString(), row["ip"].ToString() + ":" + row["port"].ToString(), this, int.Parse(row["level_no"].ToString()));
                        ReadBarCodeFromSPs.Add(scan.readerName.ToString(), scan);
                    }
                    #endregion
                    //#region 数字孪生链接扫码器Scoket
                    //ReadBoxCode r1= new ReadBoxCode("01", "127.0.0.1", this, 1, 0001);
                    //ReadBoxCode r2 = new ReadBoxCode("02", "127.0.0.1", this, 1, 0002);
                    //ReadBoxCode r3 = new ReadBoxCode("03", "127.0.0.1", this, 1, 0003);
                    //ReadBoxCode r4 = new ReadBoxCode("04", "127.0.0.1", this, 1, 0004);
                    //ReadBoxCode r5 = new ReadBoxCode("05", "127.0.0.1", this, 1, 0005);
                    //ReadBoxCode r6 = new ReadBoxCode("06", "127.0.0.1", this, 2, 0006);
                    //#endregion

                    // 初始化一层入库扫码下任务辊道
                    ds = DataBaseInterface.GetDic(conn, "device_id,device_type,error_db,control_db,write_db,return_db,load_db,device_name,x,y,is_scanner,length,width,weight_db,row_no,level_no,device_mold,device_tag,system_status_id", "td_plt_conveyor_dic", " and device_type='INSCANNERCMD'", "device_id");
                    inConveyorScannerCmd = new FirstFloorInConveyorScannerCmd(this, ds.Tables[0]);

                    // 初始化一层入库顶升移载下任务辊道
                    ds = DataBaseInterface.GetDic(conn, "device_id,device_type,error_db,control_db,write_db,return_db,load_db,device_name,x,y,length,width,row_no,level_no,device_mold,device_tag,system_status_id", "td_plt_conveyor_dic", " and device_type='INCMD'", "device_id");
                    inConveyorCmd = new FirstFloorInConveyorCmd(this, ds.Tables[0]);

                    ////初始化RGV小车  
                    ds = DataBaseInterface.GetDic(conn, "device_id,device_type,error_db,control_db,write_db,return_db,load_db,device_name,x,y,is_scanner,length,width,row_no,level_no,device_mold,device_tag,system_status_id", "td_plt_conveyor_dic", "  and device_type='RGV'", "device_id");
                    rgv = new RGV(this, ds.Tables[0]);

                    //初始化堆垛机取货站台
                    ds = DataBaseInterface.GetDic(conn, "device_id, device_type, error_db, control_db, write_db, return_db, load_db, device_name, x, y, length, width,channel_no, row_no, level_no, device_mold, device_tag, system_status_id", "td_plt_conveyor_dic", " and device_type = 'STATION_IN'", "channel_no");
                    inElectricsConveyor = new ElectricsStationInConveyor(this, ds.Tables[0]);

                    // 初始化堆垛机放货站台（CKSS1002,RKSS1003）
                    ds = DataBaseInterface.GetDic(conn, "device_id, device_type, error_db, control_db, write_db, return_db, load_db, device_name, x, y, length, width, channel_no,row_no, level_no, device_mold, device_tag, system_status_id", "td_plt_conveyor_dic", " and device_type='STATION_OUT'", "DEVICE_ID");
                    outElectricsConveyor = new ElectricsConveyorCmd(this, ds.Tables[0]);

                    ///初始化二楼堆垛机取放货站台（CKSS1001）
                    ds = DataBaseInterface.GetDic(conn, "device_id, device_type, error_db, control_db, write_db, return_db, load_db, device_name, x, y, length, width,channel_no, row_no, level_no, device_mold, device_tag, system_status_id", "td_plt_conveyor_dic", " and device_type='STATION_ALL'", "DEVICE_ID");
                    allElectricsConveyor = new ElectricsConveyorAll(this, ds.Tables[0]);

                    //一楼吨桶出库
                    ds = DataBaseInterface.GetDic(conn, "device_id, device_type, error_db, control_db, write_db, return_db, load_db, device_name, x, y, length, width,channel_no, row_no, level_no,is_scanner, device_mold, device_tag, system_status_id", "td_plt_conveyor_dic", " and device_type='STATION_OUT_T'", "DEVICE_ID");
                    outConveyorCmd = new OutConveyorCmd(this, ds.Tables[0]);

                    ///初始化带load块的辊道
                    ds = DataBaseInterface.GetDic(conn,"device_id,device_type,error_db,control_db,load_db,device_name,x,y,length,width,channel_no,row_no,level_no,device_mold,device_tag,system_status_id","td_plt_conveyor_dic", " and device_type='CONVEYORLOAD' ", "device_id");
                    conveyorLoad = new ConveyorLoad(this, ds.Tables[0]);


                    //初始化不带load块的辊道和叠盘机
                    ds = DataBaseInterface.GetDic(conn, "device_id,device_type,error_db,control_db,device_name,x,y,length,width,channel_no,row_no,level_no,device_mold,device_tag,system_status_id", "td_plt_conveyor_dic", " and device_type='CONVEYOR' ", "device_id"); 
                    conveyorNoLoad = new ConveyorNoLoad(this, ds.Tables[0]);

                    //初始化放托盘辊道
                    ds = DataBaseInterface.GetDic(conn, " device_id, device_type, error_db, control_db, write_db, return_db, load_db, device_name, x, y, length, width, row_no, level_no, device_mold, device_tag, system_status_id", "td_plt_conveyor_dic", " and device_type = 'DISM'", "DEVICE_ID");
                    trayAssign = new Tray(this, ds.Tables[0]);

                    //初始化二楼出入库口下任务辊道
                    ds = DataBaseInterface.GetDic(conn, " device_id, device_type, error_db, control_db, write_db, return_db, load_db, device_name, x, y, length, width, row_no, level_no, device_mold, device_tag, system_status_id", "td_plt_conveyor_dic", " and device_type = 'RETUNCMD'", "DEVICE_ID");
                    panConveyorCmd = new SecondFloorInConveyorCmd(this, ds.Tables[0]);

                    //初始化AGV
                    ds = DataBaseInterface.GetDic(conn,"device_id,device_type,x,y,width,hight,status","td_agv_set","","device_id");
                    agv = new AGV(this, ds.Tables[0]);

                    //初始化取液处
                    ds = DataBaseInterface.GetDic(conn,"device_id,permit_db,completion_db,delivery_db,acquisition_db,device_type","td_plt_pumping",""," device_id"); 
                    pumping = new Pumping(this, ds.Tables[0]);

                    outAssign = new OutAssign(this);
                    if (systemStatus != null)
                    {
                        if (!systemStatus.BindToPLC())
                        {
                            DeviceDisConnection();
                            return false;
                        }
                        else
                        {
                            systemStatus.RefreshStatus();
                            systemStatus.DisplayStatus();
                        }
                    }
                    if (diepanCount != null)
                    {
                        if (!diepanCount.BindToPLC())
                        {
                            DeviceDisConnection();
                            return false;
                        }
                      
                    }
                    if (inConveyorScannerCmd!=null)
                    {
                        if(!inConveyorScannerCmd.BindToPLC())
                        {
                            DeviceDisConnection();
                            return false;
                        }
                        else
                        {
                            inConveyorScannerCmd.RefreshStatus();
                            inConveyorScannerCmd.DisplayStatus();
                        }
                    }

                    if (inConveyorCmd != null)
                    {
                        if (!inConveyorCmd.BindToPLC())
                        {
                            DeviceDisConnection();
                            return false;
                        }
                        else
                        {
                            inConveyorCmd.RefreshStatus();
                            inConveyorCmd.DisplayStatus();
                        }
                    }

                    if (rgv != null)
                    {
                        if (!rgv.BindToPLC())
                        {
                            DeviceDisConnection();
                            return false;
                        }
                        else
                        {
                            rgv.RefreshStatus();
                            rgv.DisplayStatus();
                        }
                    }
                    if (inElectricsConveyor != null)
                    {
                        if (!inElectricsConveyor.BindToPLC())
                        {
                            DeviceDisConnection();
                            return false;
                        }
                        else
                        {
                            inElectricsConveyor.RefreshStatus();
                            inElectricsConveyor.DisplayStatus();
                        }
                    }

                    if (outElectricsConveyor != null)
                    {
                        if (!outElectricsConveyor.BindToPLC())
                        {
                            DeviceDisConnection();
                            return false;
                        }
                        else
                        {
                            outElectricsConveyor.RefreshStatus();
                            outElectricsConveyor.DisplayStatus();
                        }
                    }

                    if (allElectricsConveyor != null)
                    {
                        if (!allElectricsConveyor.BindToPLC())
                        {
                            DeviceDisConnection();
                            return false;
                        }
                        else
                        {
                            allElectricsConveyor.RefreshStatus();
                            allElectricsConveyor.DisplayStatus();
                        }
                    }

                    if (outConveyorCmd != null)
                    {
                        if (!outConveyorCmd.BindToPLC())
                        {
                            DeviceDisConnection();
                            return false;
                        }
                        else
                        {
                            outConveyorCmd.RefreshStatus();
                            outConveyorCmd.DisplayStatus();
                        }
                    }

                    if (conveyorLoad != null)
                    {
                        if (!conveyorLoad.BindToPLC())
                        {
                            DeviceDisConnection();
                            return false;
                        }
                        else
                        {
                            conveyorLoad.RefreshStatus();
                            conveyorLoad.DisplayStatus();
                        }
                    }

                    if (conveyorNoLoad != null)
                    {
                        if (!conveyorNoLoad.BindToPLC())
                        {
                            DeviceDisConnection();
                            return false;
                        }
                        else
                            conveyorNoLoad.DisplayStatus();
                    }

                    if (trayAssign != null)
                    {
                        if (!trayAssign.BindToPLC())
                        {
                            DeviceDisConnection();
                            return false;
                        }
                        else
                        {
                            trayAssign.RefreshStatus();
                            trayAssign.DisplayStatus();
                        }
                    }

                    if (panConveyorCmd != null)
                    {
                        if (!panConveyorCmd.BindToPLC())
                        {
                            DeviceDisConnection();
                            return false;
                        }
                        else
                        {
                            panConveyorCmd.DisplayStatus();
                        }
                    }
                   
                    if (pumping != null)
                    {
                        if (!pumping.BindToPLC())
                        {
                            DeviceDisConnection();
                            return false;
                        }
                        else
                            pumping.RefreshStatus();
                    }

                    deviceLinkIsOK = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString());
                    return false;
                }
            }
                
            return true;
        }
        #endregion

        #region 语音初始化
        private void speech_init()
        {
            //speech = new Speech(2, 100);
            //speech.init_voice();
            //speech.speech();
        }
        #endregion

        #region 定义设备刷新线程
        private void BindDevice()
        {
            thrAutoRun = new Thread(new ThreadStart(this.AutoRun));
            thrAutoRun.IsBackground = true;
            if (!thrAutoRun.IsAlive)
                thrAutoRun.Start();
        }
        #endregion

        #region 更新入库批次显示
        public void UpdateInboundShow()
        {
            this.tbShipmentdetalId.Text = this.batchid[0];
            this.tbVehicleorderNo.Text = this.batchNo[0];
            this.tbWasteCode.Text = this.goodsSku[0];
            this.tbWasteName.Text = this.goodsName[0];
            this.tbWasteKinds.Text = this.goodsKind[0] == 1 ? "吨桶" : (this.goodsKind[0] == 2 ? "圆桶" : (this.goodsKind[0] == 3 ? "空托盘" : "铁桶"));
            this.tbHazardArea.Text = hazardArea[0];
        }
        #endregion

        #region 更新设备状态显示
        private void AutoRun()
        {
            try
            {
                while (!closing)
                {
                    UpdateInboundShow();
                    if (systemStatus != null)
                    {
                        systemStatus.RefreshStatus();
                        systemStatus.DisplayStatus();
                    }
                    if(inConveyorScannerCmd!=null)
                    {
                        inConveyorScannerCmd.RefreshStatus();
                        inConveyorScannerCmd.DisplayStatus();
                    }
                    if (inConveyorCmd != null)
                    {
                        inConveyorCmd.RefreshStatus();
                        inConveyorCmd.DisplayStatus();
                    }
                    if (rgv != null)
                    {
                        rgv.RefreshStatus();
                        rgv.DisplayStatus();
                    }

                    if (inElectricsConveyor != null)
                    {
                        inElectricsConveyor.RefreshStatus();
                        inElectricsConveyor.DisplayStatus();
                    }

                    if (outElectricsConveyor != null)
                    {
                        outElectricsConveyor.RefreshStatus();
                        outElectricsConveyor.DisplayStatus();
                    }
                    if (allElectricsConveyor != null)
                    {
                        allElectricsConveyor.RefreshStatus();
                        allElectricsConveyor.DisplayStatus();
                    }
                    if (outConveyorCmd != null)
                    {
                        outConveyorCmd.RefreshStatus();
                        outConveyorCmd.DisplayStatus();
                    }
                    if (conveyorLoad != null)
                    {
                        conveyorLoad.RefreshStatus();
                        conveyorLoad.DisplayStatus();
                    }
                    if (conveyorNoLoad != null)
                    {
                        conveyorNoLoad.DisplayStatus();
                    }
                    if (trayAssign != null)
                    {
                        trayAssign.RefreshStatus();
                        trayAssign.DisplayStatus();
                    }

                    if (panConveyorCmd != null)
                    {
                        panConveyorCmd.RefreshStatus();
                        panConveyorCmd.DisplayStatus();
                    }

                    if (pumping != null)
                    {
                        pumping.RefreshStatus();
                    }
                    if (agv != null)
                    {
                        agv.DisplayStatus();
                    }
                    if (Stackers != null)
                    {
                        foreach (var item in Stackers)
                        {
                            item.Value.RefreshDisplay();
                        }
                    }
                    EmptyPalletIn();
                    //gDisplay.DrawImageUnscaled(imageBackgroud, 130, 58);
                    //gDisplay.DrawImageUnscaled(imageShuttle, 144, 875);
                    Thread.Sleep(100);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }

        }
        #endregion

        #region 母托盘自动回库
        private void EmptyPalletIn()
        {
            //若为母托盘自动回库模式，且允许切换到空托盘入库模式
            if(emptyPalletType==1&&isEmptyPalletIn)
            {
                //8个出库均已完成且输送上无货
                if(DataBaseInterface.GetOutTaskNum()==0 &&panConveyorCmd.loadStruct[0].loadType==0)
                {
                    taskType[1] = 2;//修改当前任务类型为空托入库
                    systemStatus.WriteTaskModelCmd(1, 3);//写PLC空托入库模式
                    isEmptyPalletIn = false;//不允许切换空托入库模式
                    outTaskNum = 0;//出库任务累计数量设置为0
                    stopTaskCreate[1] = false;
                    btnWorkModeLevel2.Text = "二楼空托入库";
                    goodsKind[1] = 3;
                    goodsSku[1] = "000000";
                    batchNo[1] = "000000";
                    batchid[1] = "000000";
                    goodsName[1] = "空托盘组";
                    dealWay[1] = 0;
                    hazardArea[1] = "A";
                    isHaveEmptyPalletTask = false;//设置空托入库模式后，没执行过空托入库任务
                }

            }
            //若为母托盘自动回库模式，不允许切换到空托入库模式，没有执行空托入库任务，空托入库模式
            if(emptyPalletType==1&&!isEmptyPalletIn&&!isHaveEmptyPalletTask&&taskType[1]==2)
            {
                isHaveEmptyPalletTask = DataBaseInterface.getEmptyPalletTask();//检测到有二楼的空托入库任务，则为true，否则false；
            }
            //若为母托盘自动回库模式，不允许切换到空托入库模式，有执行的空托入库任务，空托入库模式
            if (emptyPalletType == 1 && !isEmptyPalletIn && isHaveEmptyPalletTask&&taskType[1]==2)
            {
                isHaveEmptyPalletTask = DataBaseInterface.getEmptyPalletTask();//检测到有二楼的空托入库任务，则为true，否则false；
                if (!isHaveEmptyPalletTask)//若没有执行中的空托入库任务，则切换二楼工作模式为出库模式
                {
                    taskType[1] = 1;
                    systemStatus.WriteTaskModelCmd(1, 2);
                    btnWorkModeLevel2.Text = "二楼出入库"; 
                }
            }
        }
        #endregion

        #region 退出按钮
        private void tsbExit_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("真的要退出程序吗？", "退出程序", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (bPicking && !closing)
                {
                    MessageBox.Show("退出系统前请先停止工作!\r\n并等待一会！");
                    return;
                }
                System.Environment.Exit(0);
            }
          
        }
        #endregion

        #region  断开设备连接
        public void DeviceDisConnection()
        {
            PLCDisConnection();
            this.SetLabelText("PLC","未连接");
            setMenu();
        }
        #endregion

        #region  断开PLC连接
        private void PLCDisConnection()
        {
            if (systemStatus != null)
                systemStatus.DisConnectPLC();

            if (inConveyorCmd != null)
                inConveyorCmd.DisConnectPLC();

            if (rgv != null)
                rgv.DisConnectPLC();

            if (inElectricsConveyor != null)
                inElectricsConveyor.DisConnectPLC();

            if (outElectricsConveyor != null)
                outElectricsConveyor.DisConnectPLC();

            if (allElectricsConveyor != null)
                allElectricsConveyor.DisConnectPLC();

            if (panConveyorCmd != null)
                panConveyorCmd.DisConnectPLC();

            if (conveyorLoad != null)
                conveyorLoad.DisConnectPLC();

            if (conveyorNoLoad != null)
                conveyorNoLoad.DisConnectPLC();

            if (outConveyorCmd != null)
                outConveyorCmd.DisConnectPLC();

            if (trayAssign != null)
                trayAssign.DisConnectPLC();

            if (pumping != null)
                pumping.DisConnectPLC();
            //foreach (KeyValuePair<int, Stacker> staker in Stackers)
            //{
            //    staker.Value.DisConnectPLC();
            //}

            if (deviceLinkIsOK)
            {
                deviceLinkIsOK = false;
            }
        }
        #endregion

        #region 开始工作按钮
        private void tsbStart_Click(object sender, EventArgs e)
        {
            if(taskType[0]==0||taskType[1]==0)
            {
                MessageBox.Show("请先设置一二楼工作模式！");
            }
            else
            {
                bPicking = true;
                setMenu();
                this.workModeBotton.Enabled = true;
            }
        }
        #endregion

        #region 停止工作按钮
        public void tsbStop_Click(object sender, EventArgs e)
        {
            bPicking = false;
            setMenu();
            this.workModeBotton.Enabled = false;
        }
        #endregion

        #region 设置closing
        public void SetClosing(bool bSet)
        {
            closing = bSet;

            if (outConveyorCmd != null)
                outConveyorCmd.closing = bSet;

            if (systemStatus != null)
                systemStatus.closing = bSet;

            if (panConveyorCmd != null)
                panConveyorCmd.closing = bSet;

            if (inConveyorCmd != null)
                inConveyorCmd.closing = bSet;

            if (pumping != null)
                pumping.closing = bSet;

            if (conveyorLoad != null)
                conveyorLoad.closing = bSet;

            if (conveyorNoLoad != null)
                conveyorNoLoad.closing = bSet;

            if (trayAssign != null)
                trayAssign.closing = bSet;

            if (inElectricsConveyor != null)
                inElectricsConveyor.closing = bSet;

            if (outElectricsConveyor != null)
                outElectricsConveyor.closing = bSet;

            if (rgv != null)
                rgv.closing = bSet;

            if (conveyorNoLoad != null)
                conveyorNoLoad.closing = bSet;
            foreach (KeyValuePair<int, Stacker> stacker in Stackers)
            {
                stacker.Value.closing = bSet;
            }
        }
        #endregion

        #region 主界面载入事件
        private void frmMain_Load(object sender, EventArgs e)
        {
            gDisplay = this.CreateGraphics();
            speech_init();
            for (int i = 0; i < obLockLv.Length; i++)
            {
                obLockLv[i] = new object();
            }
            if (emptyPalletType == 1)
                lbCurrentType.Text = "当前模式：自动回库模式";
            else if (emptyPalletType == 2)
                lbCurrentType.Text = "当前模式：人工回库模式";
            else
                lbCurrentType.Text = "当前模式：无";

            setMenu();
            lvScanInfo_create();
            lvScanInfo_init();
            CreateListView();
            InitListView();
        }
        #endregion

        #region  扫码信息列表创建
        /// <summary>
        /// 扫码信息列表创建
        /// </summary>
        public void lvScanInfo_create()
        {
            for (int i = 0; i < 6; i++)  //创建模组订单行显示列表
            {
                lvScanInfo[i] = new DoubleBufferListView();
                this.lvScanInfo[i].Font = new System.Drawing.Font("宋体", 9F);
                this.lvScanInfo[i].FullRowSelect = true;
                this.lvScanInfo[i].GridLines = true;
                this.lvScanInfo[i].Location = new System.Drawing.Point(0, 1);
                this.lvScanInfo[i].Name = "listViewTask";
                this.lvScanInfo[i].Size = new System.Drawing.Size(tabPage1.Width - 1, tabPage1.Height - 1);
                this.lvScanInfo[i].TabIndex = i;
                this.lvScanInfo[i].UseCompatibleStateImageBehavior = false;
                this.lvScanInfo[i].View = System.Windows.Forms.View.Details;
                //this.lvScanInfo[i].ContextMenuStrip = this.cmsScanInfo;
                this.tbScanInfo.TabPages[i].Controls.Add(lvScanInfo[i]);
            }
        }
        #endregion

        #region 扫码信息列表初始化
        /// <summary>
        /// 扫码信息列表初始化
        /// </summary>
        public void lvScanInfo_init()
        {
            for (int i = 0; i < 6; i++)  //创建模组订单行显示列表
            {
                lvScanInfo[i].Columns.Add("时间", (int)(lvScanInfo[i].Width * 0.15), HorizontalAlignment.Center);
                lvScanInfo[i].Columns.Add("箱号", (int)(lvScanInfo[i].Width * 0.2), HorizontalAlignment.Center);
                lvScanInfo[i].Columns.Add("说明", (int)(lvScanInfo[i].Width * 0.65), HorizontalAlignment.Center);
            }
        }
        #endregion

        #region 扫码信息列表插入数据
        /// <summary>
        /// 扫码信息列表插入数据
        /// </summary>
        public void lvScanInfo_Insert(int nIndex, string strBoxcode, string text)
        {
            lock (obLockLv[nIndex])
            {
                if (text != lastText)
                {
                    string[] items = new string[3];
                    items[0] = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    items[1] = strBoxcode;
                    items[2] = text;
                    lvScanInfo[nIndex].Items.Insert(0, new ListViewItem(items));
                    if (lvScanInfo[nIndex].Items.Count > 50)
                    {
                        for (int i = 50; i < lvScanInfo[nIndex].Items.Count; i++)
                            lvScanInfo[nIndex].Items.RemoveAt(i);
                    }
                    lastText = text;
                }
            }
        }
        #endregion

        #region 动态改变tbScanInfo页
        private delegate void ChangetbScanInfoIndexCallback(int nIndex);
        /// <summary>
        /// 动态改变tbScanInfo页
        /// </summary>
        /// <param name="text">索引</param>
        public void ChangetbScanInfoIndex(int nIndex)
        {
            if (tbScanInfo.InvokeRequired)
            {
                ChangetbScanInfoIndexCallback d = new ChangetbScanInfoIndexCallback(ChangetbScanInfoIndex);
                this.Invoke(d, new object[] { nIndex });
            }
            else
            {
                tbScanInfo.SelectedIndex = nIndex;
            }
        }
        #endregion

        #region listview创建
        private void CreateListView()
        {
            //创建出库任务列表
            this.LvIOTask.Font = new System.Drawing.Font("宋体", 9F);
            this.LvIOTask.FullRowSelect = true;
            this.LvIOTask.GridLines = true;
            this.LvIOTask.Location = new System.Drawing.Point(3, 20);
            this.LvIOTask.Name = "listViewTask";
            this.LvIOTask.Size = new System.Drawing.Size(panel1.Width-10, panel1.Height-25);
            this.LvIOTask.TabIndex = 7;
            this.LvIOTask.UseCompatibleStateImageBehavior = false;
            this.LvIOTask.View = System.Windows.Forms.View.Details;
            this.LvIOTask.ContextMenuStrip = this.cmsTaskCancel;
            this.panel1.Controls.Add(LvIOTask);
            this.LvIOTask.SelectedIndexChanged += new System.EventHandler(this.LvIOTask_SelectedIndexChanged);

            //创建设备任务列表
            this.LvDeviceTask.Font = new System.Drawing.Font("宋体", 9F);
            this.LvDeviceTask.FullRowSelect = true;
            this.LvDeviceTask.GridLines = true;
            this.LvDeviceTask.Location = new System.Drawing.Point(3, 20);
            this.LvDeviceTask.Name = "listViewTask";
            this.LvDeviceTask.Size = new System.Drawing.Size(panel2.Width - 10, panel2.Height - 25);
            this.LvDeviceTask.TabIndex = 7;
            this.LvDeviceTask.UseCompatibleStateImageBehavior = false;
            this.LvDeviceTask.View = System.Windows.Forms.View.Details;
            this.LvDeviceTask.ContextMenuStrip = this.cmsDeviceTask;
            this.panel2.Controls.Add(LvDeviceTask);
            this.LvDeviceTask.SelectedIndexChanged += new System.EventHandler(this.LvDeviceTask_SelectedIndexChanged);
        }
        #endregion

        #region listview初始化
        private void InitListView()
        {
            #region 出入库任务列表
            LvIOTask.Columns.Add("序号", (int)(LvIOTask.Width * 0.05), HorizontalAlignment.Center);
            LvIOTask.Columns.Add("任务号", (int)(LvIOTask.Width * 0.10), HorizontalAlignment.Center);
            LvIOTask.Columns.Add("任务类型", (int)(LvIOTask.Width * 0.08), HorizontalAlignment.Center);
            LvIOTask.Columns.Add("层", (int)(LvIOTask.Width * 0.05), HorizontalAlignment.Center);
            LvIOTask.Columns.Add("托盘条码", (int)(LvIOTask.Width * 0.15), HorizontalAlignment.Center);
            LvIOTask.Columns.Add("状态", (int)(LvIOTask.Width * 0.07), HorizontalAlignment.Center);
            LvIOTask.Columns.Add("任务接收时间", (int)(LvIOTask.Width * 0.16), HorizontalAlignment.Center);
            LvIOTask.Columns.Add("任务执行时间", (int)(LvIOTask.Width * 0.16), HorizontalAlignment.Center);
            LvIOTask.Columns.Add("废料编号", (int)(LvIOTask.Width * 0.1), HorizontalAlignment.Center);
            LvIOTask.Columns.Add("接运单ID", (int)(LvIOTask.Width * 0.1), HorizontalAlignment.Center);
            LvIOTask.Columns.Add("派车单编号", (int)(LvIOTask.Width * 0.1), HorizontalAlignment.Center);
            #endregion

            #region 设备任务列表
            LvDeviceTask.Columns.Add("序号", (int)(LvDeviceTask.Width * 0.05), HorizontalAlignment.Center);
            LvDeviceTask.Columns.Add("任务号", (int)(LvDeviceTask.Width * 0.1), HorizontalAlignment.Center);
            LvDeviceTask.Columns.Add("主任务类型", (int)(LvDeviceTask.Width * 0.1), HorizontalAlignment.Center);
            LvDeviceTask.Columns.Add("子任务类型", (int)(LvDeviceTask.Width * 0.1), HorizontalAlignment.Center);
            LvDeviceTask.Columns.Add("步骤", (int)(LvDeviceTask.Width * 0.05), HorizontalAlignment.Center);
            LvDeviceTask.Columns.Add("起始地址", (int)(LvDeviceTask.Width * 0.15), HorizontalAlignment.Center);
            LvDeviceTask.Columns.Add("目的地址", (int)(LvDeviceTask.Width * 0.15), HorizontalAlignment.Center);
            LvDeviceTask.Columns.Add("设备类型", (int)(LvDeviceTask.Width * 0.1), HorizontalAlignment.Center);
            LvDeviceTask.Columns.Add("任务状态", (int)(LvDeviceTask.Width * 0.1), HorizontalAlignment.Center);
            LvDeviceTask.Columns.Add("生成时间", (int)(LvDeviceTask.Width * 0.16), HorizontalAlignment.Center);
            LvDeviceTask.Columns.Add("执行时间", (int)(LvDeviceTask.Width * 0.16), HorizontalAlignment.Center);

            #endregion
            checkIOTask.Checked = true;
            ckDeviceTask.Checked = true;
        }
        #endregion

        #region 刷新列表委托
        private delegate void SetDeviceViews();
        private void SetRefreshDeviceViews()
        {
            if (LvDeviceTask.InvokeRequired)
            {
                SetDeviceViews setDeviceViews = new SetDeviceViews(SetRefreshDeviceViews);
                this.Invoke(setDeviceViews, new object[] { });
                //“System.InvalidOperationException”类型的未经处理的异常在 System.Windows.Forms.dll 中发生 

            }
            else
                RefreshDeviceViews();
        }

        private delegate void SetIOViews();
        private void SetRefreshIOViews()
        {
            if (LvIOTask.InvokeRequired)
            {
                SetIOViews setIOViews = new SetIOViews(SetRefreshIOViews);
                this.Invoke(setIOViews, new object[] { });
            }
            else
                RefreshIOViews();
        }
        #endregion

        #region 设备任务表刷新
        /// <summary>
        /// 设备任务表刷新
        /// </summary>
        /// <param name="conn"></param>
        private void RefreshDeviceViews()
        {
            lock(obDeviceTask)
            {
                if (!this.ckDeviceTask.Checked)
                    return;
                int nRowNum = 0;
                using (MySqlConnection conn = dbConn.GetConnectFromPool())
                {
                    try
                    {
                        this.LvDeviceTask.BeginUpdate();
                        string strSQL = "select  * from tb_plt_task_d t where status <2 order by create_time";
                        DataSet ds = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQL);
                        TimeSpan tsDelay = new TimeSpan();
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (LvDeviceTask.Items.Count >= (nRowNum + 1))
                            {
                                if (LvDeviceTask.Items[nRowNum].SubItems[1].Text.ToString() != row["task_id"].ToString() || LvDeviceTask.Items[nRowNum].SubItems[4].Text.ToString() != row["step"].ToString() || LvDeviceTask.Items[nRowNum].SubItems[8].Text.ToString() != this.DecodeDTaskStatus(int.Parse(row["status"].ToString())))
                                {
                                    LvDeviceTask.Items[nRowNum].SubItems[0].Text = (nRowNum + 1).ToString();
                                    LvDeviceTask.Items[nRowNum].SubItems[1].Text = row["task_id"].ToString();
                                    LvDeviceTask.Items[nRowNum].SubItems[2].Text = DecodeMTaskType(int.Parse(row["task_type"].ToString()));
                                    LvDeviceTask.Items[nRowNum].SubItems[3].Text = DecodeDTaskType(int.Parse(row["task_type"].ToString()), int.Parse(row["step"].ToString()));
                                    LvDeviceTask.Items[nRowNum].SubItems[4].Text = row["step"].ToString();
                                    LvDeviceTask.Items[nRowNum].SubItems[5].Text = row["from_unit"].ToString();
                                    LvDeviceTask.Items[nRowNum].SubItems[6].Text = row["to_unit"].ToString();
                                    LvDeviceTask.Items[nRowNum].SubItems[7].Text = this.DecodeDTaskType(row["device_type"].ToString());
                                    LvDeviceTask.Items[nRowNum].SubItems[8].Text = this.DecodeDTaskStatus(int.Parse(row["status"].ToString()));
                                    LvDeviceTask.Items[nRowNum].SubItems[9].Text = row["create_time"].ToString();
                                    LvDeviceTask.Items[nRowNum].SubItems[10].Text = row["begin_time"].ToString();
                                }
                            }
                            else
                            {
                                string[] items = new string[LvDeviceTask.Columns.Count];
                                items[0] = (nRowNum + 1).ToString();
                                items[1] = row["task_id"].ToString();
                                items[2] = this.DecodeMTaskType(int.Parse(row["task_type"].ToString()));
                                items[3] = DecodeDTaskType(int.Parse(row["task_type"].ToString()), int.Parse(row["step"].ToString()));
                                items[4] = row["step"].ToString();
                                items[5] = row["from_unit"].ToString();
                                items[6] = row["to_unit"].ToString();
                                items[7] = this.DecodeDTaskType(row["device_type"].ToString());
                                items[8] = this.DecodeDTaskStatus(int.Parse(row["status"].ToString()));
                                items[9] = row["create_time"].ToString();
                                items[10] = row["begin_time"].ToString();
                                LvDeviceTask.Items.Add(new ListViewItem(items));
                            }
                            if (nRowNum % 2 != 0)
                                this.LvDeviceTask.Items[nRowNum].BackColor = Color.FromArgb(229, 255, 229);
                            else
                                this.LvDeviceTask.Items[nRowNum].BackColor = Color.White;
                            tsDelay = DateTime.Now - Convert.ToDateTime(row["create_time"].ToString());
                            if (tsDelay.TotalMinutes > 20)
                                this.LvDeviceTask.Items[nRowNum].BackColor = Color.Red;
                            nRowNum += 1;
                        }
                        int nItemCount = LvDeviceTask.Items.Count;
                        for (int i = nItemCount - 1; i >= nRowNum; i--)
                            LvDeviceTask.Items.RemoveAt(i);
                        LvDeviceTask.EndUpdate();
                    }
                    catch (Exception)
                    {
                        return;
                    }
                }
                
            }
           
        }
        #endregion

        #region 出入库任务表刷新
        /// <summary>
        /// 出入库任务表刷新
        /// </summary>
        /// <param name="conn"></param>
        private void RefreshIOViews()
        {
            lock(obIOTask)
            {
                if (!this.checkIOTask.Checked)
                    return;
                int nRowNum = 0;
                
                using (MySqlConnection conn = dbConn.GetConnectFromPool())
                {
                    try
                    {
                        #region 出入库任务列表
                        this.LvIOTask.BeginUpdate();
                        string strSQLin = "select *  from tb_plt_task_m t where t.task_status < 2  order by t.create_time";
                        DataSet dsin = DataBase.MySqlHelper.ExecuteDataset(conn, CommandType.Text, strSQLin);
                        TimeSpan tsDelayin = new TimeSpan();
                        foreach (DataRow row in dsin.Tables[0].Rows)
                        {
                            if (LvIOTask.Items.Count >= (nRowNum + 1))
                            {
                                if (LvIOTask.Items[nRowNum].SubItems[1].Text.ToString() != row["task_id"].ToString() || LvIOTask.Items[nRowNum].SubItems[5].Text.ToString() != this.DecodeMTaskStatus(int.Parse(row["task_status"].ToString())))
                                {
                                    LvIOTask.Items[nRowNum].SubItems[0].Text = (nRowNum + 1).ToString();
                                    LvIOTask.Items[nRowNum].SubItems[1].Text = row["task_id"].ToString();
                                    LvIOTask.Items[nRowNum].SubItems[2].Text = this.DecodeMTaskType(int.Parse(row["task_type"].ToString()));
                                    LvIOTask.Items[nRowNum].SubItems[3].Text = row["task_level"].ToString();
                                    LvIOTask.Items[nRowNum].SubItems[4].Text = row["box_barcode"].ToString();
                                    LvIOTask.Items[nRowNum].SubItems[5].Text = this.DecodeMTaskStatus(int.Parse(row["task_status"].ToString()));
                                    LvIOTask.Items[nRowNum].SubItems[6].Text = row["create_time"].ToString();
                                    LvIOTask.Items[nRowNum].SubItems[7].Text = row["begin_time"].ToString();
                                    LvIOTask.Items[nRowNum].SubItems[8].Text = row["sku"].ToString();
                                    LvIOTask.Items[nRowNum].SubItems[9].Text = row["batch_id"].ToString();
                                    LvIOTask.Items[nRowNum].SubItems[10].Text = row["batch_no"].ToString();
                                }
                            }
                            else
                            {
                                string[] items = new string[LvIOTask.Columns.Count];
                                items[0] = (nRowNum + 1).ToString();
                                items[1] = row["task_id"].ToString();
                                items[2] = this.DecodeMTaskType(int.Parse(row["task_type"].ToString()));
                                items[3] = row["task_level"].ToString();
                                items[4] = row["box_barcode"].ToString();
                                items[5] = this.DecodeMTaskStatus(int.Parse(row["task_status"].ToString()));
                                items[6] = row["create_time"].ToString();
                                items[7] = row["begin_time"].ToString();
                                items[8] = row["sku"].ToString();
                                items[9] = row["batch_id"].ToString();
                                items[10] = row["batch_no"].ToString();
                                LvIOTask.Items.Add(new ListViewItem(items));
                            }
                            if (nRowNum % 2 != 0)
                                this.LvIOTask.Items[nRowNum].BackColor = Color.FromArgb(229, 255, 229);
                            else
                                this.LvIOTask.Items[nRowNum].BackColor = Color.White;
                            tsDelayin = DateTime.Now - Convert.ToDateTime(row["create_time"].ToString());
                            if (tsDelayin.TotalMinutes > 120)
                                this.LvIOTask.Items[nRowNum].BackColor = Color.Red;
                            nRowNum += 1;
                        }
                        int nItemCountin = LvIOTask.Items.Count;
                        for (int i = nItemCountin - 1; i >= nRowNum; i--)
                            LvIOTask.Items.RemoveAt(i);
                        LvIOTask.EndUpdate();
                        #endregion
                    }
                    catch (Exception)
                    {
                        return;
                    }

                }
               
            } 
        }
        #endregion

        #region 主任务类型识别
        /// <summary>
        /// 主任务类型识别
        /// </summary>
        /// <param name="strMsgType"></param>
        /// <param name="nType"></param>
        /// <returns></returns>
        public string DecodeMTaskType(int nType)
        {
            if (nType == 1)//入库
                return "入库";
            else if (nType == 2)//出库
                return "出库";
            else if (nType == 3)
                return "空托入库";
            else if (nType == 4)
                return "退库";
            else if (nType == 5)
                return "异常回库";
            else if (nType == 6)
                return "入库口补空托";
            else
                return "未知";
        }
        #endregion

        #region 主任务类型识别
        /// <summary>
        /// 主任务类型识别
        /// </summary>
        /// <param name="strMsgType"></param>
        /// <param name="nType"></param>
        /// <returns></returns>
        public string DecodeDTaskType(int nType, int step)
        {
            if (nType == 1 || nType == 3 || (nType == 5 && step == 1) || (nType == 5 && step == 3) || (nType == 5 && step == 4))
                return "入库";
            else if (nType == 2 || nType == 4 || (nType == 5 && step == 2))
                return "出库";
            return "未知类型";
        }
        #endregion

        #region 子任务类型识别
        /// <summary>
        /// 子任务类型识别
        /// </summary>
        /// <param name="strMsgType"></param>
        /// <param name="nType"></param>
        /// <returns></returns>
        public string DecodeDTaskType(string nType)
        {
            if (nType == "CON")//入库
                return "输送";
            else if (nType == "STACK")//出库
                return "堆垛机";
            else
                return "未知";
        }
        #endregion

        #region 主任务状态识别
        /// <summary>
        /// 主任务状态识别
        /// </summary>
        /// <param name="strMsgType"></param>
        /// <param name="nType"></param>
        /// <returns></returns>
        public string DecodeMTaskStatus(int nStatus)
        {
            if (nStatus == 0)//入库
                return "新任务";
            else if (nStatus == 1)//出库
                return "执行中";
            else if (nStatus == 2)
                return "已完成";
            else if (nStatus == 3)
                return "已生成异常回库";
            else
                return "未知";
        }
        #endregion

        #region 子任务状态识别
        /// <summary>
        /// 子任务状态识别
        /// </summary>
        /// <param name="strMsgType"></param>
        /// <param name="nType"></param>
        /// <returns></returns>
        public string DecodeDTaskStatus(int nStatus)
        {
            if (nStatus == 0)//入库
                return "新生成";
            else if (nStatus == 1)//出库
                return "执行中";
            else if (nStatus == 2)
                return "已完成";
            else
                return "未知";
        }
        #endregion

        #region 入库信息表与设备任务表相关联
        /// <summary>
        /// 入库信息表与设备任务表相关联
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvIOTask_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LvIOTask.SelectedIndices != null && LvIOTask.SelectedIndices.Count > 0)
            {
                string strTaskID = LvIOTask.SelectedItems[0].SubItems[1].Text.ToString();
                for (int i = 0; i < LvDeviceTask.Items.Count; i++)
                {
                    if (LvDeviceTask.Items[i].SubItems[1].Text.ToString() == strTaskID)
                    {
                        LvDeviceTask.Items[i].BackColor = Color.DarkOrange;
                        LvDeviceTask.Items[i].EnsureVisible();
                    }
                    else
                    {
                        if (i % 2 != 0)
                            LvDeviceTask.Items[i].BackColor = Color.FromArgb(229, 255, 229);
                        else
                            LvDeviceTask.Items[i].BackColor = Color.White;
                    }
                }
            }
        }
        #endregion

      

        #region 设备任务表与出入库任务表相关联
        /// <summary>
        /// 设备任务表与出入库任务表相关联  
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LvDeviceTask_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (LvDeviceTask.SelectedIndices != null && LvDeviceTask.SelectedIndices.Count > 0)
            {
                string strTaskID = LvDeviceTask.SelectedItems[0].SubItems[1].Text.ToString();
                #region 查询入库任务表
                for (int i = 0; i < LvIOTask.Items.Count; i++)
                {
                    if (LvIOTask.Items[i].SubItems[1].Text.ToString() == strTaskID)
                    {
                        LvIOTask.Items[i].BackColor = Color.DarkOrange;
                        LvIOTask.Items[i].EnsureVisible();
                    }
                    else
                    {
                        if (i % 2 != 0)
                            LvIOTask.Items[i].BackColor = Color.FromArgb(229, 255, 229);
                        else
                            LvIOTask.Items[i].BackColor = Color.White;
                    }
                }
                #endregion

            }
        }
        #endregion

       

        #region 订单监控界面刷新
        /// <summary>
        /// 订单监控界面刷新
        /// </summary>
        private void RefreshListViews()
        {
            while (true)
            {
                if (dbIsConned)
                {
                    //RefreshIOViews();
                    //RefreshDeviceViews();
                    SetRefreshDeviceViews();
                    SetRefreshIOViews();
                }
                Thread.Sleep(2000);
            }
        }
        #endregion


        #region 同一主任务的子任务查询
        private void tsmQueryTask_Click(object sender, EventArgs e)
        {
            if (LvDeviceTask.SelectedIndices == null || LvDeviceTask.SelectedIndices.Count <= 0)
            {
                MessageBox.Show("请选中一行任务数据！");
                return;
            }
            FormSubTask frmSubTask = new FormSubTask(this, LvDeviceTask.SelectedItems[0].SubItems[1].Text.ToString());
            frmSubTask.ShowDialog();
        }
        #endregion

        #region 首页按钮点击事件
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            FrmWLocation frmwlocation = new FrmWLocation(this, "MainFrm");
            frmwlocation.ShowDialog();
        }

        private void tsbDeviceSet_Click(object sender, EventArgs e)
        {
            FormRFIDStatus f = new FormRFIDStatus(this);
            f.ShowDialog();
        }

        private void tsbGoodsQuery_Click(object sender, EventArgs e)
        {
            FormTaskHistory frmgoodTask = new FormTaskHistory(this);
            frmgoodTask.ShowDialog();
        }

        private void tsbErrorQuery_Click(object sender, EventArgs e)
        {
            FormTaskManual frmerror = new FormTaskManual(this);
            frmerror.ShowDialog();
        }

        private void tsbScanner_Click(object sender, EventArgs e)
        {
            FormScannerHistory frmScanner = new FormScannerHistory(this);
            frmScanner.ShowDialog();
        }

        private void tsbDtaskQuery_Click(object sender, EventArgs e)
        {
            FormDTask frmDTask = new FormDTask(this);
            frmDTask.ShowDialog();
        }
        #endregion

        #region 关闭按钮事件
        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("真的要退出程序吗？", "退出程序", MessageBoxButtons.OKCancel) == DialogResult.OK)
            {
                if (bPicking && !closing)
                {
                    MessageBox.Show("退出系统前请先停止工作!\r\n并等待一会！");
                    return;
                }
                System.Environment.Exit(0);
            }
        }
        #endregion

        private void tsb_not_input_Click(object sender, EventArgs e)
        {
            JustNotInto jni = new JustNotInto();
            jni.dbConn = this.dbConn;
            jni.Show();
        }

        private void tsbTaskInfo_Click(object sender, EventArgs e)
        {
            FormDeviceError de = new FormDeviceError(this, "");
            de.dbConn = this.dbConn;
            de.Show();
        }



        #region 点动控制命令
        public List<string> ControlCmd(string deviceType)
        {
            if (deviceType == "SS" )
            {
                return new List<string>() { "电机正传", "电机反转" };
            }
            else if (deviceType == "YZ")
            {

                return new List<string>() { "输送电机正转", "输送电机反转", "移载机构正转", "移载机构反转", "顶升电机上升", "顶升电机下降" };
            }
            else if (deviceType == "RGV")
            {
                return new List<string>() { "电机运行正转", "电机运行反转", "向左移动", "向右移动" };
            }
            else if(deviceType=="DP")
            {
                return new List<string>() { "输送电机正转", "输送电机反转", "托盘提升上升", "托盘提升下降", "伸叉气缸伸", "伸叉气缸缩","阻挡上升","阻挡下降","清空叠盘机" };
            }
            else
            {
                return new List<string>() { "无" };
            }
        }
        #endregion

        #region 修改显示界面下端设备状态
        public void SetLabelText(string deviceName, string status)
        {
            if (this.InvokeRequired)
            {
                MethodInvoker method = new MethodInvoker(
                    delegate ()
                    {
                        switch (deviceName)
                        {
                            case "1": ScannerConnectLabel1.Text = "扫码器1：" + status; break;
                            case "2": ScannerConnectLabel2.Text = "扫码器2：" + status; break;
                            case "3": ScannerConnectLabel3.Text = "扫码器3：" + status; break;
                            case "4": ScannerConnectLabel4.Text = "扫码器4：" + status; break;
                            case "5": ScannerConnectLabel5.Text = "扫码器5：" + status; break;
                            case "6": ScannerConnectLabel6.Text = "扫码器6：" + status; break;
                            case "DB": DBConnStatusLabel.Text = "数据库：" + status; break;
                            case "PLC": PLCConnStatusLabel.Text = "PLC：" + status; break;
                         
                        }
                    });
                this.Invoke(method);
            }
            else
            {
                switch (deviceName)
                {
                    case "1": ScannerConnectLabel1.Text = "扫码器1：" + status; break;
                    case "2": ScannerConnectLabel2.Text = "扫码器2：" + status; break;
                    case "3": ScannerConnectLabel3.Text = "扫码器3：" + status; break;
                    case "4": ScannerConnectLabel4.Text = "扫码器4：" + status; break;
                    case "5": ScannerConnectLabel5.Text = "扫码器5：" + status; break;
                    case "6": ScannerConnectLabel6.Text = "扫码器6：" + status; break;
                    case "DB": DBConnStatusLabel.Text = "数据库：" + status; break;
                    case "PLC": PLCConnStatusLabel.Text = "PLC：" + status; break;
                }
            }
        }
        #endregion

        private void workModeBotton_Click(object sender, EventArgs e)
        {
            FormChangeGoods f = new FormChangeGoods(this,0);
            f.Show();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            FormDeviceStatus frmdevicestatus = new FormDeviceStatus(this);
            frmdevicestatus.ShowDialog();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            using (MySqlConnection con = dbConn.GetConnectFromPool())
            {
                try
                {
                    string sql = "select * from td_plt_location_dic t where t.unit_status = 1 and t.box_barcode is not null and t.batch_no <> '000000'";
                    int i = 0;
                    foreach (DataRow item in DataBase.MySqlHelper.ExecuteDataset(con, CommandType.Text, sql).Tables[0].Rows)
                    {
                        i++;
                        sql = "insert into tb_outstockplan(id,outplanno,status,down_date,tasktype) values('" + DateTime.Now.ToString() + i.ToString() + "','" + item["box_barcode"].ToString() + "',0,sysdate(),0)";
                        DataBase.MySqlHelper.ExecuteNonQuery(con, CommandType.Text, sql);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
               
        }


        #region 出入库口设置
        private void tspInPortSet_Click(object sender, EventArgs e)
        {
            FormPortSet frmPortSet = new FormPortSet(this);
            frmPortSet.ShowDialog();
        }
        #endregion

        #region 母托盘回库设置
        private void tspPalletSet_Click(object sender, EventArgs e)
        {
            FormEmptyPalletSet frmEmptyPalletSet = new FormEmptyPalletSet(this);
            frmEmptyPalletSet.ShowDialog();
        }
        #endregion

        #region 母托盘自动回库模式切换按钮
        private void btnAtuo_Click(object sender, EventArgs e)
        {
            emptyPalletType = 1;
            outTaskNum = 0;
            lbCurrentType.Text = "当前模式：自动回库模式";
        }

        #endregion

        #region 母托盘人工模式切换按钮
        private void btnManual_Click(object sender, EventArgs e)
        {
            emptyPalletType = 2;
            outTaskNum = 0;
            lbCurrentType.Text = "当前模式：人工回库模式";
        }
        #endregion

        #region 尾托回库
        private void btnEmptyIn_Click(object sender, EventArgs e)
        {
            if (emptyPalletType == 1)
                isEmptyPalletIn = true;
            else
                MessageBox.Show("该功能只适用于自动回库模式");
        }
        #endregion

        #region 修改重量信息
        public void ChangeWeight(double will, double weight)
        {
            try
            {
                this.txtWillWeightMix.Text = Math.Round(will * 0.97, 2).ToString();
                this.txtWillWeightMax.Text = Math.Round(will * 1.03, 2).ToString();
                this.txtInboundWeight.Text = Math.Round(weight, 2).ToString();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        #endregion

        private void btnWorkModeLevel2_Click(object sender, EventArgs e)
        {
            FormWorkModeLevel2 frmWorkModelLevel2 = new FormWorkModeLevel2(this);
            frmWorkModelLevel2.ShowDialog();
        }

        private void tsbRebackLevel1_Click(object sender, EventArgs e)
        {
            FormUpdateCount frmRebackLevel = new FormUpdateCount(this);
            frmRebackLevel.ShowDialog();
        }
        #region 任务取消
        private void tsmTaskCancel_Click(object sender, EventArgs e)
        {
            if (LvIOTask.SelectedIndices == null || LvIOTask.SelectedIndices.Count <= 0)
            {
                MessageBox.Show("请选中一行任务数据！");
                return;
            }
            if (LvIOTask.SelectedItems[0].SubItems[2].Text.ToString()=="出库")
            {
                string strLevel = LvIOTask.SelectedItems[0].SubItems[3].Text.ToString();
                string rs = string.Empty;
                string strReturn=DataBaseInterface.TaskCancel(int.Parse(LvIOTask.SelectedItems[0].SubItems[1].Text.ToString()), out rs);
                MessageBox.Show(rs);
                if(strReturn=="1")
                {
                    if(emptyPalletType==1&& strLevel=="2")//自动回库模式且为2楼出库，则出库数量减一
                    {
                        DataBaseInterface.cancleCount();
                    }
                }
            }
            else
            {
                MessageBox.Show("只有新生成的出库订单才可取消");
            }
           
        }
        #endregion

        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {

            FormRebackLevel1 fle = new FormRebackLevel1(this,1);
            fle.ShowDialog();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {
            FormCargoCount fcc = new FormCargoCount(this);
            fcc.ShowDialog();
        }
    }
}
