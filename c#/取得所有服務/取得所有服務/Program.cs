using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址： https://learn.microsoft.com/zh-tw/dotnet/api/system.serviceprocess.servicecontroller?view=dotnet-plat-ext-7.0
 */

namespace 取得所有服務
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 取得所有服務
            List<ServiceController> scServices = ServiceController.GetServices().ToList();

            foreach (ServiceController s in scServices)
            {
                Console.WriteLine($"服務名稱 =  {s.ServiceName}");
                Console.WriteLine($"電腦名稱 =  {s.MachineName}");
                Console.WriteLine($"服務易記名稱 =  {s.DisplayName}");
                Console.WriteLine($"可暫停繼續 =  {s.CanPauseAndContinue}");
                Console.WriteLine($"可以關閉 =  {s.CanShutdown}");
                Console.WriteLine($"可以暫停 =  {s.CanStop}");
                Console.WriteLine($"狀態 =  {s.Status}");

                // 依賴這個服務的服務
                List<ServiceController> sd = s.ServicesDependedOn.ToList();
                List<string> sdName = sd.Select(x => x.ServiceName).ToList();
                string totalSdName = string.Join(", ", sdName);
                Console.WriteLine($"依賴此服務的所有服務 =  {totalSdName}");

                Console.WriteLine("=========================================================");
            }

            // 對 服務 操作 (這邊隨便拿1個，不要執行就行)
            string firstServiceName = scServices.Count() > 0 ? scServices[0].ServiceName : "";
            if (!string.IsNullOrEmpty(firstServiceName) )
            {
                ServiceController sc = new ServiceController(firstServiceName);

                //// 暫止服務
                //sc.Pause();
                //// 服務暫停後繼續執行
                //sc.Continue();
                //// 啟動服務(不傳入參數)
                //sc.Start();
                //// 啟動服務(傳入參數)
                //List<string> argArray = new List<string> { "test arg1", "test arg2" };
                //sc.Start(argArray.ToArray());
                //// 重新整理
                //sc.Refresh();
                //// 停止此服務和其相關的服務
                //sc.Stop();
                //// 中斷服務，並釋放所有記憶體
                //sc.Close();
                //// 在服務上執行自定命令 (後面的數字不太了解怎麼對應)
                //sc.ExecuteCommand(128);
                //// 無限期等待服務狀態變成指定狀態
                //sc.WaitForStatus(ServiceControllerStatus.Running);
            }


            Console.ReadLine();
        }
    }
}
