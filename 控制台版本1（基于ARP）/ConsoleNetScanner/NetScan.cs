using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ConsoleNetScanner
{
    class NetScan
    {
        /// <summary>
        /// 通过ARP命令获取本地ARP列表
        /// </summary>
        /// <returns></returns>
        internal static List<NetCollection> GetARPList()
        {
            //throw new NotImplementedException();
            string strARP = Common.ReadCmd("arp","-a");
            
            List <NetCollection> listNet = Common.StrToList(strARP);
            return listNet;
        }

        /// <summary>
        /// 将本地记录的每一条数据与检测的数据进行比对，发现IP相同但Mac不同的主机或者检测到本地记录没有的将存入listDiff
        /// </summary>
        /// <param name="listLocal"></param>
        /// <param name="listScan"></param>
        /// <returns></returns>
        internal static List<NetCollection> Cmp(List<NetCollection> listLocal, List<NetCollection> listScan)
        {
            //throw new NotImplementedException();
            List<NetCollection> listDiff = new List<NetCollection>();
            foreach (var s in listScan)
            {
                bool b = false;
                foreach (var l in listLocal)
                {
                    
                    if (s.IP == l.IP)
                    {
                        
                        if (s.Mac != l.Mac)
                        {
                            s.DetecMac = l.Mac;
                            listDiff.Add(s);
                            
                        }
                        b = true;
                    }                    
                }
                if (!b)
                {                    
                    listDiff.Add(s);
                }
                
            }
            return listDiff;
        }
    }
}
