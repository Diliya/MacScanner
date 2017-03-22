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
using System.Data;
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


        string[] ipName = { "" };
        string[] macName = { "" };
        List<string[]> arpInfo = getLocalInfo.GetIPInfo();
        public void getARPList()
        {
            
            //将存储arp列表的list转换成DataTable，然后让datagrid绑定DataTable
            DataTable dtArp = new DataTable();
            //int num = 0;
            //string[] strIP = new string[arpInfo.Count()];
            ////string[] strMac = new string[arpInfo.Count()];
            //foreach (string[] i in arpInfo)
            //{

            //    for (int num = 0; num < i.Length;num++ )
            //    {
                    dtArp.Columns.Add("序号");
                    dtArp.Columns.Add("IP地址");
                    dtArp.Columns.Add("Mac地址");
                    dtArp.Columns.Add("说明");
            //    }
            //}
                    int num = 1;
            foreach (string[] i in arpInfo)
            {
                DataRow dr = dtArp.NewRow();
                dr["序号"] = num.ToString();
                dr["IP地址"] = i[1];
                dr["Mac地址"] = i[0];
                dr["说明"] = "";
                num++;
                dtArp.Rows.Add(dr);
                
            }
            //gridWhite.Columns[1].IsReadOnly = true;
            //gridWhite.Columns[2].IsReadOnly = true;
            gridWhite.ItemsSource = dtArp.DefaultView;
            
            
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
                    textBlockNum.Text += arpInfo.Count() +"台主机";
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
