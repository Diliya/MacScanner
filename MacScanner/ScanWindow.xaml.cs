using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MacScanner
{
    /// <summary>
    /// ScanWindow.xaml 的交互逻辑
    /// </summary>
    public partial class ScanWindow : Window
    {
        public ScanWindow()
        {
            InitializeComponent();
            InitInfo();
        }
        
        private void InitInfo()
        {

            try
            {
                System.IO.StreamReader file =
                    new System.IO.StreamReader(@"IPinfo.txt");
                int msg = 0;
                Window main = new MainWindow(msg);
                main.Show();
                this.Close();
            }
            catch
            {
                MessageBoxResult myResult = MessageBox.Show("是否开始扫描局域网？(点击取消将读取本地的ARP列表)", "未检测到本地数据", MessageBoxButton.OKCancel);
                if (myResult == MessageBoxResult.OK)
                {
                    Scan();
                }
                else if (myResult == MessageBoxResult.Cancel)
                {
                    int msg = 1;
                    Window main = new MainWindow(msg);
                    main.Show();
                    this.Close();
                }
            }

        }
        

        private static int numPC = 0;//扫描到的局域网主机数
        private void Scan()
        {
            //int numPC = 0;//扫描到的局域网主机数
            txtConsole.Text = "开始扫描......\r\n";
            
            //List<string[]> str = getLocalInfo.GetIPInfo();
            //foreach (string[] i in str)
            //{
            //    txtConsole.Text += i[0]+"-"+i[1]+"\r";
            //}
                txtConsole.Text += String.Format("已扫描到{0}台主机", numPC);
        }
        
        
        //private void EnumComputers()
       
    }
}


