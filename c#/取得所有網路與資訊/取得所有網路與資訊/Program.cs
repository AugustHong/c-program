using NativeWifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址： https://dotblogs.com.tw/Artgogo/2016/04/08/164739
               https://dotblogs.com.tw/law1009/2012/01/20/66799

    去 參考/右鍵/加入參考/組件/System.Net
    去 NuGet 裝上 managedwifi
 */

namespace 取得所有網路與資訊
{
    internal class Program
    {
        public static long bytesRX = 0;      //儲存bytesReceived
        public static long bytesTX = 0;      //儲存bytesSent
        public static NetworkInterface currentNetWorkI;

        static void Main(string[] args)
        {
            // 取得當前連線的網路介面
            WlanClient wlanClient = new WlanClient();
            string currentLan = wlanClient.Interfaces[0].InterfaceName;
            Console.WriteLine($"當前連接網路類型 = {currentLan}");
            Console.WriteLine("---------------------------------------------------------------");

            //取得所有網路介面類別(封裝本機網路資料)
            List<NetworkInterface> nics = NetworkInterface.GetAllNetworkInterfaces().ToList();
            foreach (NetworkInterface adapter in nics)
            {
                // 如果 類型 和 當前網路連接類型相同，存起來要算速度
                if (adapter.Name == currentLan)
                {
                    currentNetWorkI = adapter;
                }

                string name = adapter.Name;                                             //網路介面名稱
                string description = adapter.Description;                               //網路介面描述
                PhysicalAddress mac = adapter.GetPhysicalAddress();                     //取得Mac Address
                NetworkInterfaceType netWorkInterfaceType = adapter.NetworkInterfaceType;  //介面類型
                IPInterfaceStatistics ipStatistics = adapter.GetIPStatistics();  // IP資料統計
                long br = ipStatistics.BytesReceived;  //接收Bytes
                long bs = ipStatistics.BytesSent;      //傳送Bytes

                // 取得 IPInterfaceProperties(可提供網路介面相關資訊) 的 IP資訊
                var ipInfo = GetNetIpInfo(adapter);
                string ipv6 = ipInfo.Item1;
                string ipv4 = ipInfo.Item2;
                string ipv4Netmask = ipInfo.Item3;
                
                Console.WriteLine($"名稱 = {name}, 描述 = {description}, MAC = {mac}, IPV6 = {ipv6}, IPV4 = {ipv4},  IPV4子網路遮罩 = {ipv4Netmask}, 介面類型 = {netWorkInterfaceType}, 己接收Bytes = {br}, 己傳送Bytes = {bs}");
            }

            Console.WriteLine("-----------------------------------------------------------");
            Console.WriteLine("請輸入任意鍵，進行算網路速度");

            string start = Console.ReadLine();
            if (!string.IsNullOrEmpty(start))
            {
                // 設定計時器，每秒跑1次
                System.Timers.Timer spdTimer = new System.Timers.Timer(1000);
                spdTimer.Elapsed += new System.Timers.ElapsedEventHandler(countNetSpeed);  // 執行 算網速的動作
                spdTimer.Enabled = true;
                spdTimer.AutoReset = true;
                spdTimer.Start();
            }

            Console.Read();
        }

        /// <summary>
        /// 取得 網路 (IPV6, IPV4, IPV4子網路遮罩)
        /// </summary>
        /// <param name="net"></param>
        /// <returns></returns>
        public static (string, string, string) GetNetIpInfo(NetworkInterface net)
        {
            //取得IPInterfaceProperties(可提供網路介面相關資訊)
            IPInterfaceProperties ipProperties = net.GetIPProperties();
            string ipv4 = "";
            string ipv4Netmask = "";

            string ipv6 = "";

            int addressCount = ipProperties.UnicastAddresses.Count;
            if (addressCount > 0)
            {
                // 有2個的，第1個是 ipv6 ， 第2個是 ipv4
                if (addressCount >= 2)
                {
                    ipv6 = ipProperties.UnicastAddresses[0].Address.ToString();        //取得IP

                    ipv4 = ipProperties.UnicastAddresses[1].Address.ToString();        //取得IP
                    ipv4Netmask = ipProperties.UnicastAddresses[1].IPv4Mask.ToString();  //取得遮罩
                }
                else
                {
                    // 就都當他是 ipv4
                    ipv4 = ipProperties.UnicastAddresses[0].Address.ToString();        //取得IP
                    ipv4Netmask = ipProperties.UnicastAddresses[0].IPv4Mask.ToString();  //取得遮罩
                }
            }

            return (ipv6, ipv4, ipv4Netmask);
        }

        /// <summary>
        /// 算網速
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="e"></param>
        public static void countNetSpeed(object obj, System.Timers.ElapsedEventArgs e)  //為上傳與下載速率的方法
        {
            IPInterfaceStatistics ipStatistics = currentNetWorkI.GetIPStatistics();

            // 減掉上一次的總量，就是這次的傳輸量
            double download = (double)(ipStatistics.BytesReceived - bytesRX) / 1024;
            int upload = (int)(ipStatistics.BytesSent - bytesTX) / 1024;

            bytesRX = ipStatistics.BytesReceived;
            bytesTX = ipStatistics.BytesSent;
            Console.Clear();
            Console.WriteLine("網路介面卡名稱：" + currentNetWorkI.Name);
            Console.WriteLine("當前網路連接介面類型：" + currentNetWorkI.NetworkInterfaceType);
            Console.WriteLine("IPV6：" + GetNetIpInfo(currentNetWorkI).Item1);
            Console.WriteLine("IPV4：" + GetNetIpInfo(currentNetWorkI).Item2);
            Console.WriteLine("下載速度：" + download + " KB/s");
            Console.WriteLine("上傳速度：" + upload + " KB/s");
            Console.WriteLine("已接收bytes：" + bytesRX);
            Console.WriteLine("已傳送bytes：" + bytesTX);
        }
    }
}
