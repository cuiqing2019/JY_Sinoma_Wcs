using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Management;


namespace rootnamespace
{
    class SaoMiaoYingJian
    {
        public static string GetCpuId()
        {
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection moc = mc.GetInstances();
            string strCpuID = null;
            foreach (ManagementObject mo in moc)
            {
                strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                break;
            }
            return strCpuID;
        }
        //取得设备硬盘的卷标号  
        public static string GetDiskVolumeSerialNumber()
        {
            ManagementClass mc =
                 new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObject disk =
                 new ManagementObject("win32_logicaldisk.deviceid=\"c:\"");
            disk.Get();
            return disk.GetPropertyValue("VolumeSerialNumber").ToString();
        }


        /* 生成序列号 */
        public static string CreatSerialNumber_AES()
        {
            string SerialNumber = GetCpuId() + GetDiskVolumeSerialNumber();
            return SerialNumber;
        }
        public static string CreatSerialNumber_MD5()
        {
            string SerialNumber = GetCpuId() + GetDiskVolumeSerialNumber();
            return SerialNumber;

        }
    }
}
