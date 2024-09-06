using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Hangfire.SqlServer;

/*
    參考網址： https://docs.hangfire.io/en/latest/background-processing/processing-jobs-in-console-app.html
                https://dotblogs.com.tw/yc421206/2019/12/25/desktop_app_async_task_via_hangfire#Jobs
    備註： 這個有 Web 也有 Console。 自已習慣用Console，所以使用此展示。
    (但用 Console 一直找不到如果叫用 DashBoard 起來)

    請先用 NuGet 裝上 Hangfire.Core 和 Hangfire.SqlServer
 */

namespace 使用Hangfire設定排程
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 給定連線字串 (Hangfire 會寫入 DB中)
            string connectionString = "Server=.;Database=Test;User Id=sa;Password=密碼";
            GlobalConfiguration.Configuration.UseSqlServerStorage(connectionString)
                                            .UseColouredConsoleLogProvider();
            
            using (var server = new BackgroundJobServer())
            {
                // fire and got:立即執行一次
                BackgroundJob.Enqueue(() => Console.WriteLine("立即執行一次"));

                // delay: 立即延遲多久後只會執行1次
                BackgroundJob.Schedule(() => Console.WriteLine("延遲1天執行1次"), TimeSpan.FromDays(1));  // 啟動後的隔天
                BackgroundJob.Schedule(() => Console.WriteLine("延遲1分鐘執行1次"), TimeSpan.FromMinutes(1)); // 啟動後的下1分鐘

                // recurring: 設定cron敘述，重複執行多次
                /*
                    //每15秒 "0/15 * * * * ?"
                    //每分鐘 "0 * * * * ?"
                    //每小時 "0 0 * * * ?"
                    //每日凌晨0點 "0 0 0 * * ?"
                 */
                RecurringJob.AddOrUpdate(() => Console.WriteLine("每分鐘跑1次"), "0 * * * * ?");

                // continue: 在某個job執行完後接續執行
                var id = BackgroundJob.Enqueue(() => Console.WriteLine("第1個任務"));
                BackgroundJob.ContinueWith(id, () => Console.WriteLine("第2個任務"));


                Console.ReadKey();
            }
        }
    }
}
