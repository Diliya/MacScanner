using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace ConsoleMacSanner
{
    class Program
    {
        static void Main(string[] args)
        {
            StartScan();
            //ReadRecord();
            //System.Console.ReadKey();
        }
        private static bool isScan = false;//加个标志判断是否需要开始扫描
        static string strText;//存储从本地记录读取的数据
        private static void StartScan()
        {
            while (isScan == false)
            {
                //存在本地记录文件则读取，无本地记录文件则开始扫描本地网段，将扫描结果写入新文件作为本地记录
                try
                {
                    getLocalRec();
                    
                }
                catch
                {
                    try
                    {
                        System.Console.WriteLine("未读取到本地记录，开始扫描获取......");
                        //File.Create(@"IPInfo.txt");
                        List<string[]> NetInf = getLocalInf.GetIPInfo();
                        WriteRecord(NetInf);
                        isScan = true;
                    }
                    catch
                    {
                        System.Console.WriteLine("创建本地记录文件失败");
                    }
                    System.Console.WriteLine("成功写入本地记录");
                    //System.Console.ReadKey();
                    //ReadRecord();
                }
                isScan = true;
            }
            //throw new NotImplementedException();
            while (isScan)
            {
                System.Console.WriteLine("实时扫描中......");
                ReadRecord();
            }
        }

        private static void getLocalRec()
        {
            //throw new NotImplementedException();
            StreamReader file =
                            new StreamReader(@"IPInfo.txt");
            strText = file.ReadToEnd();
            file.Close();
            //System.Console.WriteLine(str);
            System.Console.WriteLine("成功读取到本地记录");
        }
        
        //先读取本地记录文件
        private static void ReadRecord()
        {
            getLocalRec();
            List<string[]> LocalRecInf = getLocalInf.GetIPInfo(strText);
            List<string[]> NetInfo = getLocalInf.GetIPInfo();
            List<string[]> NewList = new List<string[]>();
            //LocalRecInf.Sort();
            //NetInfo.Sort();
            //IEnumerable<string[]> q1 =null;
            //q1 = from items in LocalRecInf orderby items[1] select items;
            //IEnumerable<string[]> q2 = null;
            //q2 = from items in NetInfo orderby items[1] select items;
            //比较检测到的arp列表是否和本地记录一致
            //while (true)
            //{
                //bool j = false;
                //foreach (string[] i in LocalRecInf)
                //    foreach (string[] t in NetInfo)
                //    {
                //        if (i[1].ToString() == t[1].ToString() && i[0].ToString() == t[0].ToString())
                //        {
                //            j = false;
                //            NewList.Remove(t);
                //        }
                //        else
                //        {
                //            j = true;
                //            break;
                //        }
                //    }
                NewList = Comp(LocalRecInf, NetInfo);
                if (NewList.Count() > 0)
                {
                    System.Console.WriteLine("检测到当前网络arp信息与本地记录不符");
                    //List<string[]> list = q1.Except<string[]>(q2).ToList();
                    //foreach (var i in list)
                    //{
                    //    NewList.Remove(i);
                    //}

                    WriteLog(NewList);
                    System.Console.WriteLine("检测到局域网主机变化，是否将新加入主机加入本地记录？(Y/N)");
                    string str = System.Console.ReadLine();
                    if (str == "Y" || str == "y")
                    {
                        WriteRecord(NewList);
                    }
                }
                else if (NewList.Count() == 0)
                {
                    System.Console.WriteLine("当前局域网主机情况和本地记录一致");
                }
                //isScan = true;
                StartScan();
            //}
            //throw new NotImplementedException();
        }

        private static List<string[]> Comp(List<string[]> LocalRecInf, List<string[]> NetInfo)
        {
            //bool j = false;
            //throw new NotImplementedException();
            foreach (string[] i in LocalRecInf)
            {
                foreach (string[] t in NetInfo)
                {
                    if (i[1].ToString() == t[1].ToString() && i[0].ToString() == t[0].ToString())
                    {
                        //j = false;
                        NetInfo.Remove(t);
                        break;
                    }
                    //else
                    //{
                    //    j = true;
                    //    //NewList.Add(i);
                    //    //break;
                    //}
                }
            }
            return NetInfo;
            //return j;
        }

        private static void WriteRecord(List<string[]> NetInf)
        {
            //throw new NotImplementedException();
            try
            {
                //FileStream fs = File.OpenWrite(@"IPInfo.txt");
                FileStream fs = new FileStream(@"IPInfo.txt", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                foreach (string[] i in NetInf)
                {
                    fs.Position = fs.Length;
                    Encoding enc = Encoding.UTF8;
                    byte[] str = enc.GetBytes(i[1] + " " + i[0] + "\r\n");
                    fs.Write(str, 0, str.Length);
                    //File.WriteAllText(@"IPInfo.txt", i[1]+ " "+i[0]+"\r\n");

                }
                fs.Close();
            }
            catch
            {
                Console.WriteLine("写入记录失败");
            }
            finally
            {

            }
        }

        private static void WriteLog(List<string[]> NewList)
        {
            //throw new NotImplementedException();

            FileStream fs = new FileStream(@"log.log", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);

            foreach (string[] i in NewList)
            {
                fs.Position = fs.Length;
                Encoding enc = Encoding.UTF8;

                byte[] str = enc.GetBytes(DateTime.Now + " " + "未知主机接入局域网" + " " + i[1] + " " + i[0] + "\r\n");
                fs.Write(str, 0, str.Length);
                System.Console.WriteLine("检测到一台未知主机" + i[1] + "(" + i[0] + "）" + "接入局域网，已写入日志");
                //fs.Close();
            }
            fs.Close();

        }

        //private static List<string[]> getNetInfo(string str)
        //{
        //    List<string[]> NetInf = new List<string[]>();
        //    if (str == "arp")
        //    {
        //        //throw new NotImplementedException();通过ARP命令获取本地以太网口ARP列表
        //        NetInf = getLocalInf.GetIPInfo();
        //    }
        //    else
        //    {
        //        NetInf = getLocalInf.GetIPInfo(str);
        //    }
        //        return NetInf;
        //}


    }
}
/*基本逻辑
 *                   ↗有，读取      ↘        ↗有新主机，提示，写入日志,询问是否将新主机加入本地记录
 * 启动→读取本地文件                  定时扫描
 *                   ↘无，扫描，写入↗        ↘无新主机，无操作
*/