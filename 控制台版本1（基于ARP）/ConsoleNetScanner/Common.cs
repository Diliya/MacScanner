/////////////////////////////////////////////////////////////////////////
///公用类
/////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace ConsoleNetScanner
{
    class Common
    {
        /// <summary>
        /// 将字符串转换成List
        /// </summary>
        /// <param name="strText"></param>
        /// <returns></returns>
        internal static List<NetCollection> StrToList(string strText)
        {
            //throw new NotImplementedException();
            List<NetCollection> listNet = new List<NetCollection>();
            foreach (var arp in strText.Split(new char[] { '\n', '\r' }))
            {
                if (!string.IsNullOrEmpty(arp))
                {
                    var pieces = (from piece in arp.Split(new char[] { ' ', '\t' })
                                  where !string.IsNullOrEmpty(piece)
                                  select piece).ToArray();

                    if (pieces.Length == 2|| (pieces.Length ==3 && 
                        (pieces[2] == "动态" || pieces[2] == "静态")))
                    {
                        //pieces[1]Mac  
                        //pieces[0]IP 
                        listNet.Add(new NetCollection { IP = pieces[0], Mac = pieces[1] });
                    }
                }
            }
            return listNet;
        }
        /// <summary>
        /// 通过调用控制台执行命令并获取控制台输出的结果
        /// </summary>
        /// <param name="p"></param>
        /// <param name="c"></param>
        /// <returns></returns>
        internal static string ReadCmd(string p,string c)
        {
            //throw new NotImplementedException();
            Process ps = null;
            string output = string.Empty;
            try
            {
                ps = Process.Start(new ProcessStartInfo(p, c)
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                });
                output = ps.StandardOutput.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception("IPInfo: Error Retrieving 'arp -a' Results", ex);
            }
            finally
            {
                if (ps != null)
                {
                    ps.Close();
                }
            }
            return output;            
        }
        /// <summary>
        /// 将字符串写入文件
        /// </summary>
        /// <param name="s"></param>
        /// <param name="fs"></param>
        internal static void StrToFile(string s, System.IO.FileStream fs)
        {
            //throw new NotImplementedException();
            try
            {
                fs.Position = fs.Length;
                Encoding enc = Encoding.UTF8;
                byte[] str = enc.GetBytes(s);
                fs.Write(str, 0, str.Length);
                fs.Close();
            }
            catch
            {

            }
        }
    }
}
