using Microsoft.SqlServer.Server;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址： https://dotblogs.com.tw/alexwang/2016/09/21/234628
    參考/右鍵/加入參考/組件/System.Windows.Forms
 */

namespace 強迫使用系統管理員開啟程式
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 得到當前登入的WIndow角色
            WindowsIdentity currentWI = WindowsIdentity.GetCurrent();
            Console.WriteLine($"當前登入Window角色-登入者名稱 ： {currentWI.Name}");
            Console.WriteLine($"當前登入Window角色-設定標籤 ： {currentWI.Label}");
            Console.WriteLine($"當前登入Window角色-擁有者SID ： {currentWI.Owner}");
            Console.WriteLine($"當前登入Window角色-模擬層級 ： {currentWI.ImpersonationLevel}");
            Console.WriteLine($"當前登入Window角色-使用者SID ： {currentWI.User}");
            Console.WriteLine($"當前登入Window角色-AccessToken ： {currentWI.AccessToken}");
            Console.WriteLine($"當前登入Window角色-驗證類型 ： {currentWI.AuthenticationType}");
            Console.WriteLine($"當前登入Window角色-是否匿名帳戶 ： {currentWI.IsAnonymous}");
            Console.WriteLine($"當前登入Window角色-是否由Window驗證 ： {currentWI.IsAuthenticated}");
            Console.WriteLine($"當前登入Window角色-是否為Guest帳戶 ： {currentWI.IsGuest}");
            Console.WriteLine($"當前登入Window角色-是否System帳戶 ： {currentWI.IsSystem}");

            WindowsPrincipal currentWP = new WindowsPrincipal(currentWI);

            // 驗證是否是系統管理員
            if (!currentWP.IsInRole(WindowsBuiltInRole.Administrator))
            {
                Console.WriteLine("強制使用系統管理員");

                var processInfo = new ProcessStartInfo();
                // 強制使用 系統管理員身份執行
                processInfo.UseShellExecute = true;
                processInfo.FileName = System.Windows.Forms.Application.ExecutablePath;
                processInfo.Verb = "runas";

                try
                {
                    Process.Start(processInfo);
                    // 這下面可以不用加，是因為我想看變成2個視窗才加的
                    Console.Read();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"請使用系統管理員身份執行。 錯誤訊息： {ex.Message}");
                    Console.Read();
                }
            }
            else
            {
                // 寫入相關程式
                Console.WriteLine("己確認成功是系統管理員身份");
                Console.Read();
            }
        }
    }
}
