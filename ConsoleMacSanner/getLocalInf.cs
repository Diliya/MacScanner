using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.NetworkInformation;
using System.Diagnostics;

namespace ConsoleMacSanner
{
    class getLocalInf
    {
        //通过调用arp命令读取结果来获取arp列表
        private static string GetARPResult()
        {
            Process p = null;
            string output = string.Empty;
            try
            {
                p = Process.Start(new ProcessStartInfo("arp", "-a")
                {
                    CreateNoWindow = true,
                    UseShellExecute = false,
                    RedirectStandardOutput = true
                });
                output = p.StandardOutput.ReadToEnd();
            }
            catch (Exception ex)
            {
                throw new Exception("IPInfo: Error Retrieving 'arp -a' Results", ex);
            }
            finally
            {
                if (p != null)
                {
                    p.Close();
                }
            }
            return output;
        }
        public static List<string[]> GetIPInfo()
        {
            var list = new List<string[]>();
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                bool Pd1 = (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet); //判断是否是以太网连接，过滤非以太网接口的arp列表
                if (Pd1)
                {
                    try
                    {
                        
                        foreach (var arp in GetARPResult().Split(new char[] { '\n', '\r' }))
                        {
                            if (!string.IsNullOrEmpty(arp))
                            {
                                var pieces = (from piece in arp.Split(new char[] { ' ', '\t' })
                                              where !string.IsNullOrEmpty(piece)
                                              select piece).ToArray();

                                if (pieces.Length == 3 && getSegment(pieces[0]) == getLocalSegment(adapter))
                                {
                                    //pieces[1]Mac  
                                    //pieces[0]IP  
                                    list.Add(new string[2] { pieces[1], pieces[0] });
                                }
                            }
                        }
                        
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("IPInfo: Error Parsing 'arp -a' results", ex);
                    }
                }
            }
            return list;
        }
        
        
        public static string getSegment(string addr)
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            string IPSegment = "";
            foreach (NetworkInterface adapter in nics)
            {
                bool Pd1 = (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet); //判断是否是以太网连接
                if (Pd1)
                {
                    IPInterfaceProperties ip = adapter.GetIPProperties();
                    string subnet = ip.UnicastAddresses[1].IPv4Mask.ToString();
                    string[] spSub = subnet.Split('.');
                    string binarySub = IPtoBinary(subnet);
                    string binaryOne = binarySub.Replace("0", "");
                    int num = binaryOne.Length;//子网掩码长度
                    string[] spAddr = addr.Split('.');
                    int[] netCode = new int[4];
                    for (int i = 0; i < 4; i++)
                    {
                        netCode[i] = Convert.ToInt32(spAddr[i]) & Convert.ToInt32(spSub[i]);
                    }
                    
                    IPSegment = netCode[0] + "." + netCode[1] + "." + netCode[2] + "." + netCode[3] + "/" + num.ToString();                    
                    
                }
            }
            return IPSegment;
        }

        internal static string getLocalSegment(NetworkInterface adapter)
        {
            IPInterfaceProperties ip = adapter.GetIPProperties();
            string subnet = ip.UnicastAddresses[1].IPv4Mask.ToString();
            
            //string binarySub = subnet.Replace(".","");
            //string binaryOne = binarySub.Replace("0", "");
            string[] spSub = subnet.Split('.');
            //int num = binaryOne.Length;
            string binarySub = IPtoBinary(subnet);
            string binaryOne = binarySub.Replace("0", "");
            int num = binaryOne.Length;//子网掩码长度
            string IPAddr = ip.UnicastAddresses[1].Address.ToString();
            string[] spIP = IPAddr.Split('.');
            //string binaryIP = IPtoBinary(IPAddr);
            int[] netCode = new int[4];
            for(int i=0;i<4;i++)
            {
                netCode[i] = Convert.ToInt32(spSub[i]) & Convert.ToInt32(spIP[i]);
            }
           
            string IPSegment = "";
            IPSegment = netCode[0] + "." + netCode[1] + "." + netCode[2] + "." + netCode[3] + "/" + num.ToString();
            //IPSegment = num.ToString();
            //string IPSegment = Dns.GetHostEntry(Dns.GetHostName()).AddressList[2].ToString().Substring(0, 3);
            return IPSegment;
        }

        internal static string getLocalIP(NetworkInterface adapter)
        {

            IPInterfaceProperties ip = adapter.GetIPProperties();
            string IPAddr = ip.UnicastAddresses[1].Address.ToString();
            return IPAddr;
        }
        /*IP地址转换为整数
         * 原理：IP地址每段可以看成是8位无符号整数即0-255，把每段拆分成一个二进制形式组合起来，然后把这个二进制数转变成一个无符号的32位整数。
         * 举例：一个ip地址为10.0.3.193
         * 每段数字 相对应的二进制数
         * 10 00001010
         * 0 00000000
         * 3 00000011
         * 193 11000001
         * 组合起来即为：00001010 00000000 00000011 11000001，转换为10进制就是：167773121，即该IP地址转换后的数字就是它了。
         */
        public static long IpToInt(string ip)
        {
            char[] separator = new char[] { '.' };
            string[] items = ip.Split(separator);
            return long.Parse(items[0]) << 24
                    | long.Parse(items[1]) << 16
                    | long.Parse(items[2]) << 8
                    | long.Parse(items[3]);
        }
        /*整数转换为IP地址
         *原理：把这个整数转换成一个32位二进制数。从左到右，每8位进行一下分割，得到4段8位的二进制数，把这些二进制数转换成整数然后加上”.”，就是这个ip地址了。
         *举例，整数:167773121
         *二进制表示形式为：00001010 00000000 00000011 11000001
         * 分割成四段：00001010,00001010,00000011,11000001，分别转换为整数后加上“.”就得到了10.0.3.193。
         */
        public static string IntToIp(long ipInt)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append((ipInt >> 24) & 0xFF).Append(".");
            sb.Append((ipInt >> 16) & 0xFF).Append(".");
            sb.Append((ipInt >> 8) & 0xFF).Append(".");
            sb.Append(ipInt & 0xFF); 
            return sb.ToString();
        }
        /*将网络地址转换成去除点号的二进制表示形式
         * 
         */
         public static string IPtoBinary(string code)
        {
            string[] part = code.Split('.');
            string splice="";
            foreach(string i in part)
            {
                splice += Convert.ToString(Convert.ToInt32(i),2);
            }
            return splice;
        }


         internal static List<string[]> GetIPInfo(string str)
         {
             //throw new NotImplementedException();
             List<string[]> localRecInf = new List<string[]>();
             //localInf.Add(str.Split(new char[] { '\n', '\r' }));
             //localInf.Add(str.Split('\n'));
             foreach (var arp in str.Split(new char[] { '\n', '\r' }))
             {
                 if (!string.IsNullOrEmpty(arp))
                 {
                     var pieces = (from piece in arp.Split(new char[] { ' ', '\t' })
                                   where !string.IsNullOrEmpty(piece)
                                   select piece).ToArray();

                     if (pieces.Length == 2)
                     {
                         //pieces[1]Mac  
                         //pieces[0]IP  
                         localRecInf.Add(new string[2] { pieces[1], pieces[0] });
                     }
                 }
             }
             return localRecInf;
         }
    }
}
