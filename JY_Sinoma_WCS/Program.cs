using JY_Sinoma_WCS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace JY_Sinoma_WCS
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Boolean createdNew; //返回是否赋予了使用线程的互斥体初始所属权
            System.Threading.Mutex instance = new System.Threading.Mutex(true, "JY_Sinoma_WCS", out createdNew); //同步基元变量
            if (createdNew) //赋予了线程初始所属权，也就是首次使用互斥体
            {
                Application.Run(new FrmDlog());
                instance.ReleaseMutex();
            }
            else
            {
                MessageBox.Show("已经启动了一个系统！", "系统提示", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }
        }
    }
}
