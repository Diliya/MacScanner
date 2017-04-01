using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConsoleNetScanner
{
    class LocalLog
    {
        /// <summary>
        /// 写入日志
        /// </summary>
        /// <param name="p"></param>
        internal static void WriteLog(string p)
        {
            //throw new NotImplementedException();
            try
            {
                
                FileStream fs = new FileStream(@"EventLog.log", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                p = DateTime.Now + " " + p+ "\r\n";
                Common.StrToFile(p, fs);
            }
            catch
            {

            }
        }
    }
}
