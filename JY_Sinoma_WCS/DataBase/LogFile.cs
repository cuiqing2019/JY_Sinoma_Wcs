using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace DataBase
{
    public class LogFile
    {
        private FileStream fs;
        private string lastStr = "  ";
        public LogFile(string fileName)
        {
            string dirStr = DateTime.Now.ToString("yyyyMMdd") + "Log";
            if (!Directory.Exists(dirStr))
                (new DirectoryInfo(dirStr)).Create();
            fs = new FileStream(dirStr + "\\" + fileName + ".txt", FileMode.Append, FileAccess.Write);
        }
        private object lkObject = new object();
        public void Write(string text)
        {
            lock (lkObject)
            {
                if (lastStr != text)
                {
                    byte[] b = Encoding.Default.GetBytes(DateTime.Now.ToString("HH:mm:ss") + "\t" + text + "\r\n");
                    fs.Write(b, 0, b.Length);
                    fs.Flush();
                    lastStr = text;
                }
            }
        }
    }
}

