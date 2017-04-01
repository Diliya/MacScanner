using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ConsoleNetScanner
{
    class Program
    {
        private static List<NetCollection> listLocal;
        private static List<NetCollection> listScan;
        static void Main(string[] args)
        {
            //先判断本地记录文件IPInfo.txt是否存在，存在就导入数据，不存在先获取arp列表然后存入本地记录文件
            if (!LocalRecord.GetLocalRecord())
            {
                System.Console.WriteLine("本地记录不存在,开始导入本地ARP列表");
                listLocal = NetScan.GetARPList();
                LocalRecord.ListExport(listLocal);
                Console.WriteLine("导入成功，将自动重启程序");
                Application.Restart();//自动重启程序
                
            }
            listLocal = LocalRecord.ImportFromRecord();//导入记录
            Console.WriteLine("开始扫描...");
            
           
            bool p = true;
            while(p)
            { 
                listScan = NetScan.GetARPList();
                List<NetCollection> listDiff = NetScan.Cmp(listLocal,listScan);
                if (listDiff.Count > 0)
                {
                    p = false;
                    foreach (var i in listDiff)
                    {
                        if (i.DetecMac != null)
                        {
                            Console.WriteLine("检测到主机"+i.IP+"的Mac地址与本地记录不符，检测值为"+i.DetecMac+",本地记录值为"+i.Mac);
                            LocalLog.WriteLog("检测到主机" + i.IP + "的Mac地址与本地记录不符，检测值为" + i.DetecMac + ",本地记录值为" + i.Mac);
                        }
                        else
                        {
                            //Console.WriteLine(string.Format("检测到本地未记录的主机{0}加入局域网，该陌生主机Mac地址为{1}"), i.IP, i.Mac);
                            Console.WriteLine("检测到本地未记录的主机"+i.IP+"加入局域网，该陌生主机Mac地址为"+i.Mac);
                            LocalLog.WriteLog("检测到本地未记录的主机" + i.IP + "加入局域网，该陌生主机Mac地址为" + i.Mac);
                        }
                    }
                    Console.ReadKey();
                }
            }
            

        }
    }
}
