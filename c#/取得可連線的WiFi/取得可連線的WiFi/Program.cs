using NativeWifi;
using SimpleWifi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static NativeWifi.Wlan;
using static NativeWifi.WlanClient;

/*
    參考網址： https://blog.csdn.net/euxnijuoh/article/details/120074623
               https://blog.csdn.net/weixin_50725219/article/details/125159069
    去 NuGet 裝上 managedwifi 和 SimpleWifi.netstandard
 */

namespace 取得可連線的WiFi
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
                若發現只能查到自己，先斷網跑一次就可以看到全部。
                但這部份很怪，連到網路就自會出現連到的網路而己。
                所以去 NuGet 裝上 SimpleWifi.netstandard 去先斷網，再去讀取資料
             */

            // 先執行斷網操作，讓下面的取到全部
            Wifi w = new Wifi();

            // 這邊一樣可以看到可連的WiFi (但一樣會連到網路就只剩自己)
            List<AccessPoint> a = w.GetAccessPoints();
            foreach (AccessPoint p in a)
            {
                Console.WriteLine($"名稱 = {p.Name}, 訊號強度 = {p.SignalStrength}");
            }

            w.Disconnect();
            Console.WriteLine("------------------------------------------------------------");
            System.Threading.Thread.Sleep(1000);  //等個1秒去刷新

            WlanClient client = new WlanClient();
            foreach (WlanInterface wlanIface in client.Interfaces)
            {
                // 取到所有可用的WiFi
                List<WlanAvailableNetwork> networks = wlanIface.GetAvailableNetworkList(0).OrderByDescending(x => (int)x.wlanSignalQuality).ToList();

                // 因為莫名會有重複的出來，所以拿去濾掉
                List<string> ssidList = new List<string>();

                // 依照 連線訊號 (強 到 弱)
                foreach (WlanAvailableNetwork network in networks)
                {
                    string SSID = Encoding.Default.GetString(network.dot11Ssid.SSID, 0, (int)network.dot11Ssid.SSIDLength);

                    // SSID = 空白 為 "隱藏的網路"
                    if (ssidList.Where(x => x == SSID).Count() <= 0)
                    {
                        if (SSID == "")
                        {
                            SSID = "隱藏的網路";
                        }

                        int wlanSignalQuality = (int)network.wlanSignalQuality;  //品質           
                        string dot11DefaultAuthAlgorithm = network.dot11DefaultAuthAlgorithm.ToString();
                        string dot11DefaultCipherAlgorithm = network.dot11DefaultCipherAlgorithm.ToString();

                        Console.WriteLine($"WIFI名稱 = {SSID}, 訊號品質 = {wlanSignalQuality}, 認證方式 = {dot11DefaultAuthAlgorithm}, 加密方式 = {dot11DefaultCipherAlgorithm}");
                        ssidList.Add(SSID);
                    }
                }
            }

            Console.WriteLine("---------------------------------------------------------");

            // 重連網路
            Console.WriteLine("請輸入要重連回的網路SSID (可參考上面的SSID)： ");
            string reNetSSID = Console.ReadLine();
            Console.WriteLine("請輸入要重連回的網路的密碼");
            string password = Console.ReadLine();

            List<AccessPoint> wifiList = w.GetAccessPoints();
            foreach (AccessPoint wifi in wifiList)
            {
                if (wifi.Name == reNetSSID)
                {
                    AuthRequest ar = new AuthRequest(wifi);
                    ar.Password = password;
                    bool success = wifi.Connect(ar);

                    if (success)
                    {
                        Console.WriteLine("重連網路成功");
                    }
                    else
                    {
                        Console.WriteLine("重連失敗，請記得再重連");
                    }
                }
            }
        }
    }
}
