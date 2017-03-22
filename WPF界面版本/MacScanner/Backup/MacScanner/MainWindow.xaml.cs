using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
namespace MacScanner
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(int msg)
        {
            InitializeComponent();
            showIPInfo();
            if (msg == 0)
            {
                StartWithDB();
            }
            else if(msg == 1)
            {
                StartWithARPList();
            }
            //Start();
            //showIPInfo();            
            //string IP="未知",segment="未知";
            //IP = getlocalIP();
            //segment = getlocalLAN();
            //textBlockIP.Text += IP;
        }

        private void StartWithARPList()
        {
            //throw new NotImplementedException();
            getARPList();
        }

        private void StartWithDB()
        {
            //throw new NotImplementedException();
        }

        //private static int tag = 0;

     

       
        public void getARPList()
        {
            List<string[]> arpInfo =getLocalInfo.GetIPInfo();
            string[] ipName = ;
            string[] macName = ;
            //MessageBox.Show("ARPList");
            //listBoxWhite.ItemsSource = arpInfo;
            //gridWhite.ItemsSource = arpInfo;
            ////实例化process对象  
            //System.Diagnostics.Process p = new System.Diagnostics.Process();
            ////要执行的程序名称，cmd  
            //p.StartInfo.FileName = "cmd.exe";
            //p.StartInfo.UseShellExecute = false;
            ////可能接受来自调用程序的输入信息  
            //p.StartInfo.RedirectStandardInput = true;
            ////由调用程序获取输出信息  
            //p.StartInfo.RedirectStandardOutput = true;
            ////不显示程序窗口  
            //p.StartInfo.CreateNoWindow = true;
            //p.Start();//启动程序  
            //          //向CMD窗口发送输入信息：  
            //p.StandardInput.WriteLine("arp -a");
            ////不过不要忘记加上Exit，不然程序会异常退出  
            //p.StandardInput.WriteLine("exit");
            ////获取CMD窗口的输出信息：  
            //string sOutput = p.StandardOutput.ReadToEnd();
            //string[] sOutputs = sOutput.Split('\n');
            //int n = 0;
            //foreach (string i in sOutputs)
            //{
            //    n++;
            //    txtConsole.Text += n.ToString() + " " + i + "\r\n";
            //}
            ////txtConsole.Text += sOutput;
            ////使用正则再过滤一下，获得mac地址  
            //String mac = "";
            //// 定义一个Regex对象实例，存储mac的正则  
            //Regex r = new Regex("([a-z0-9]{2}-){5}[a-z0-9]{2}");
            //MatchCollection mc = r.Matches(sOutput);
            ////在输入字符串中找到所有匹配  
            //for (int i = 0; i < mc.Count; i++)
            //{
            //    mac = mc[i].Value; //将匹配的字符串添在字符串数组中  
            //}
            //wwed8z
            //txtConsole.Text += "本机的Mac地址是" + mac;
        }
            
           
        

        public void showIPInfo()
        {

            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface adapter in nics)
            {
                bool Pd1 = (adapter.NetworkInterfaceType == NetworkInterfaceType.Ethernet); //判断是否是以太网连接
                if (Pd1)
                {
                    textBlockIP.Text += getLocalInfo.getLocalIP(adapter);
                    textBlockLAN.Text += getLocalInfo.getLocalSegment(adapter);
                }
            }
        }
        //public string getlocalIP()
        //{
        //    string IP;
        //    string hostname = Dns.GetHostName();
        //    IPHostEntry localhost = Dns.GetHostEntry(hostname);
        //    IPAddress localaddr = localhost.AddressList[2];
            
        //    IP = localaddr.ToString();
        //    return IP;
        //}
        //public string getlocalLAN()
        //{
        //    string segment;
            
        //    return segment;
        //}
    }
}
