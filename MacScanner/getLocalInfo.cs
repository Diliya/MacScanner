using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Windows.Controls;

namespace MacScanner
{
    class getLocalInfo
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
        
        static void Show()
        {
            //本地计算机上的网络接口的对象,我的电脑里面以太网网络连接有两个虚拟机的接口和一个本地接口
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                bool Pd1 = (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet); //判断是否是以太网连接
                if (Pd1)
                {
                    Console.WriteLine("网络适配器名称：" + adapter.Name);
                    Console.WriteLine("网络适配器标识符：" + adapter.Id);
                    Console.WriteLine("适配器连接状态：" + adapter.OperationalStatus.ToString());

                    IPInterfaceProperties ip = adapter.GetIPProperties();     //IP配置信息
                    if (ip.UnicastAddresses.Count > 0)
                    {
                        Console.WriteLine("IP地址:" + ip.UnicastAddresses[0].Address.ToString());
                        Console.WriteLine("子网掩码:" + ip.UnicastAddresses[0].IPv4Mask.ToString());
                    }
                    if (ip.GatewayAddresses.Count > 0)
                    {
                        Console.WriteLine("默认网关:" + ip.GatewayAddresses[0].Address.ToString());   //默认网关
                    }
                    int DnsCount = ip.DnsAddresses.Count;
                    Console.WriteLine("DNS服务器地址：");   //默认网关
                    if (DnsCount > 0)
                    {
                        //其中第一个为首选DNS，第二个为备用的，余下的为所有DNS为DNS备用，按使用顺序排列
                        for (int i = 0; i < DnsCount; i++)
                        {
                            Console.WriteLine("              " + ip.DnsAddresses[i].ToString());
                        }
                    }
                    Console.WriteLine("网络接口速度：" + (adapter.Speed / 1000000).ToString("0.0") + "Mbps");
                    Console.WriteLine("接口描述：" + adapter.Description);
                    Console.WriteLine("适配器的媒体访问控制 (MAC) 地址:" + adapter.GetPhysicalAddress().ToString());
                    Console.WriteLine("该接口是否只接收数据包：" + adapter.IsReceiveOnly.ToString());
                    Console.WriteLine("该接口收到的字节数：" + adapter.GetIPv4Statistics().BytesReceived.ToString());
                    Console.WriteLine("该接口发送的字节数：" + adapter.GetIPv4Statistics().BytesSent.ToString());
                    Console.WriteLine("该接口丢弃的传入数据包数：" + adapter.GetIPv4Statistics().IncomingPacketsDiscarded.ToString());
                    Console.WriteLine("该接口丢弃的传出数据包数：" + adapter.GetIPv4Statistics().OutgoingPacketsDiscarded.ToString());
                    Console.WriteLine("该接口有错误的传入数据包数：" + adapter.GetIPv4Statistics().IncomingPacketsWithErrors.ToString());
                    Console.WriteLine("该接口有错误的传出数据包数：" + adapter.GetIPv4Statistics().OutgoingPacketsWithErrors.ToString());
                    Console.WriteLine("该接口协议未知的数据包数：" + adapter.GetIPv4Statistics().IncomingUnknownProtocolPackets.ToString());
                    Console.WriteLine("---------------------------------------------------------------------\n");
                }

            }
            Console.ReadLine();
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
    }
}

