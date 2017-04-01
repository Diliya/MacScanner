using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConsoleNetScanner
{
    class LocalRecord
    {
        /// <summary>
        /// 判断本地记录文件是否存在
        /// </summary>
        /// <returns></returns>
        internal static bool GetLocalRecord()
        {
            //throw new NotImplementedException();
            try
            {
                StreamReader file = new StreamReader(@"IPInfo.txt");
                file.Close();
                return true;
            }
            catch
            {
                return false;
            }
            finally
            {
                
            }
        }
        /// <summary>
        /// 从本地记录导入数据，转换成List
        /// </summary>
        /// <returns></returns>
        internal static List<NetCollection> ImportFromRecord()
        {
            //throw new NotImplementedException();
            
                StreamReader file = new StreamReader(@"IPInfo.txt");
                string strText = file.ReadToEnd();
                file.Close();
                
                List<NetCollection> listNet = Common.StrToList(strText);
                //System.Console.WriteLine("成功读取到本地记录");
                return listNet;
            
        }
        /// <summary>
        /// 将存储IP和Mac的List转成字符串
        /// </summary>
        /// <param name="listLocal"></param>
        internal static void ListExport(List<NetCollection> listLocal)
        {
            //throw new NotImplementedException();
            
            try
            {
                string str = string.Empty;
                FileStream fs = new FileStream(@"IPInfo.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                foreach (var i in listLocal)
                {
                    str += i.IP + " " + i.Mac + "\r\n";
                }
                Common.StrToFile(str,fs);
            }
            catch
            { 
            
            }
        }
    }
}
