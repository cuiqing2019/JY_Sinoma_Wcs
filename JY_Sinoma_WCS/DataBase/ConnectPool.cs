using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Threading;
using System.Diagnostics;
using MySql.Data.MySqlClient;

namespace DataBase
{
    public class ConnectPool
    {
        #region 自定义变量

        private object obAll = new object();
        private string strConnect;//连接字符串
        #endregion

        #region 构造函数
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="mainFrm"></param> 继承体
        /// <param name="strConnect"></param>连接字符串
        /// <param name="nMaxConnectNum"></param>最大连接数
        /// <param name="nMinConnectNum"></param>最小连接数
        /// <param name="nLifetime"></param>连接建立最长存在周期(s)
        public ConnectPool(string strConnect, int nMaxConnectNum, int nMinConnectNum, int nLifetime)
        {
            this.strConnect = strConnect + ";Pooling=true;Min Pool Size="+nMinConnectNum.ToString()+";Max Pool Size="+ nMaxConnectNum .ToString()+ ";Connection Lifetime="+ nLifetime .ToString()+ ";Connection Timeout=1";          
        }
        #endregion

        #region 按最小连接数建立数据库连接
        /// <summary>
        /// 按最小连接数建立数据库连接
        /// </summary>
        /// <returns>是否成功</returns> 
        public bool StartService()
        {
            using (MySqlConnection conn = new MySqlConnection(strConnect))
            {
                try
                {
                    conn.Open();
                }
                catch(Exception ex)
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region 获取可用连接，在已建立连接无结果时，创建新的连接
        /// <summary>
        /// 获取可用连接，在已建立连接无结果时，创建新的连接
        /// </summary>
        /// <returns></returns>
        public MySqlConnection GetConnectFromPool()
        {
            lock (obAll)
            {
                MySqlConnection conn = new MySqlConnection(strConnect);
                try
                {
                    conn.Open();
                }
                catch
                {
                    return null;
                }
                return conn;
            }
        }
        #endregion

    }
}
