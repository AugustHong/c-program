using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

/*
    --取得電池狀態
    參考網址： https://dotblogs.com.tw/larrynung/2009/09/29/10817
    參考/右鍵/加入參考/System.WIndows.Forms

    -- CPU溫度、型號 (有可能主機版的廠商沒有實作，就會取不到)
    參考網址： https://dotblogs.com.tw/chou/2009/06/21/8927
    參考/右鍵/加入參考/System.Management
 */

namespace 取得電池_CPU溫度和型號
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 取得電池狀態
            /*
             BatteryChargeStatus	表示目前充電狀態。回傳值為BatteryChargeStatus型態的列舉值，其值可為High(高電量)、Low(低電量)、Critical(極低電量)、Charging(充電中)、NoSystemBattery(沒電池)、與Unknown(狀態不詳)。
            BatteryFullLifetime	表示充滿電力可使用多久時間(-1為不詳)。
            BatteryLifePercent	表示電力剩餘多少百分比(255為不詳)。
            BatteryLifeRemaining	表示剩餘電力可使用多久時間(-1為不詳)。
            PowerLineStatus	表示電源狀態。回傳值為PowerLineStatus型態的列舉值，其值可為Online(充電狀態)、Offline(電池模式)、與Unknow(狀態不詳)。
             */
            PowerStatus ps = SystemInformation.PowerStatus;
            Console.WriteLine($"充電狀態： {ps.BatteryChargeStatus}");
            Console.WriteLine($"充滿電力可使用多久時間： {ps.BatteryFullLifetime}");
            Console.WriteLine($"電力剩餘多少百分比： {ps.BatteryLifePercent}");
            Console.WriteLine($"剩餘電力可使用多久時間： {ps.BatteryLifeRemaining}");
            Console.WriteLine($"電源狀態： {ps.PowerLineStatus}");

            Console.WriteLine("---------------------------------------------------------------------");

            // 取得 CPU溫度、型號 (要使用 WMI 來查詢，但主機板實作廠商不一定有支援)
            // 且 執行時，一定要用 系統管理員來執行 (否則就是存取被拒)

            int i = 1;
            double CPUtprt;
            // 使用 WMI查詢
            ManagementObjectSearcher mos = new ManagementObjectSearcher(@"root\WMI", "Select * From MSAcpi_ThermalZoneTemperature");

            foreach (ManagementObject mo in mos.Get())
            {
                // 取得溫度
                CPUtprt = Convert.ToDouble(Convert.ToDouble(mo.GetPropertyValue("CurrentTemperature").ToString()) - 2732) / 10;
                Console.WriteLine($"第 {i} 個 CPU的溫度為： {CPUtprt}  °C");
                i++;
            }

            // 再來一樣使用 WMI查詢型號
            /*
                ManagementObjectSearcher 類別 : 根據指定的查詢擷取管理物件的集合
                透過查詢語法 SELECT * FROM Win32_Processor 取得所有 Win32_Processor 類別資料
                可參考 http://msdn.microsoft.com/en-us/library/aa394373(VS.85).aspx
                其中 CPU 型號為 ProcessorId 參數值
             */
            // 使用 WMI查詢
            try
            {
                mos = new ManagementObjectSearcher("Select * From Win32_Processor");

                foreach (ManagementObject mo in mos.Get())
                {
                    // 找 Key 值是 ProcessorId 的
                    Console.WriteLine($"CPU的型號： {mo["ProcessorId"]}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }


            Console.ReadLine();
        }
    }
}
