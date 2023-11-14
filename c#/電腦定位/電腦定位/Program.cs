using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址： https://dotblogs.com.tw/larrynung/2011/01/13/20778
               https://learn.microsoft.com/zh-tw/dotnet/api/system.device.location.geocoordinatewatcher?view=netframework-4.8.1

    參考/右鍵/加入參考/組件/System.Device
 */

namespace 電腦定位
{
    internal class Program
    {
        static CivicAddressResolver m_addressResolver = new CivicAddressResolver();

        static void Main(string[] args)
        {
            /*
                僅供參考，目前自己是沒定到位
             */

            // 單次
            using (GeoCoordinateWatcher watcher = new GeoCoordinateWatcher())
            {
                watcher.MovementThreshold = 1.0;
                watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));

                // 取到座標資料
                GeoCoordinate location = watcher.Position.Location;
                ShowLocationDetail(location);

                if (!location.IsUnknown)
                {
                    // 取到地址資料
                    CivicAddressResolver resolver = new CivicAddressResolver();
                    CivicAddress realLocation = resolver.ResolveAddress(location);
                    ShowCivicAddressDetail(realLocation, location);
                }
            }

            // 多次讀取 (位置變更時)
            using (GeoCoordinateWatcher watch = new GeoCoordinateWatcher())
            {
                watch.Start();
                watch.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(watch_PositionChanged);
            }

            Console.Read();
        }

        /// <summary>
        /// 顯示座標訊息
        /// </summary>
        /// <param name="location"></param>
        public static void ShowLocationDetail(GeoCoordinate location)
        {
            if (location.IsUnknown)
            {
                Console.WriteLine("無法定位");
            }
            else
            {
                Console.WriteLine("Time: {0}", DateTime.Now);
                Console.WriteLine("Longitude: {0}", location.Longitude);    //經度
                Console.WriteLine("Latitude: {0}", location.Latitude);      //緯度
                Console.WriteLine("Altitude: {0}", location.Altitude);      //高度
                Console.WriteLine("Course: {0}", location.Course);          //角度
                Console.WriteLine("Speed: {0}", location.Speed);            //速度
            }
        }

        /// <summary>
        /// 顯示 地址資訊
        /// </summary>
        /// <param name="realLocation"></param>
        /// <param name="location"></param>
        public static void ShowCivicAddressDetail(CivicAddress realLocation, GeoCoordinate location)
        {
            if (location.IsUnknown)
            {
                Console.WriteLine("無法定位");
            }
            else
            {
                Console.WriteLine("Address1: {0}", realLocation.AddressLine1);          //實際地址
                Console.WriteLine("Address2: {0}", realLocation.AddressLine2);
                Console.WriteLine("Building: {0}", realLocation.Building);              //門牌號碼
                Console.WriteLine("City: {0}", realLocation.City);                      //縣市
                Console.WriteLine("CountryRegion: {0}", realLocation.CountryRegion);    //國家
                Console.WriteLine("PostalCode: {0}", realLocation.PostalCode);          //郵遞區號
                Console.WriteLine("StateProvince: {0}", realLocation.StateProvince);    //省份
                Console.WriteLine("FloorLevel: {0}", realLocation.FloorLevel);          //樓層
            }
        }

        /// <summary>
        /// 位置變更時
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public static void watch_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            GeoCoordinate location = e.Position.Location;
            ShowLocationDetail(location);

            CivicAddress realLocation = m_addressResolver.ResolveAddress(location);
            ShowCivicAddressDetail(realLocation, location);

            Console.WriteLine("========================================================");
        }
    }
}
