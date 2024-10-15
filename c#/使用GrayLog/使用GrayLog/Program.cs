using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Serilog;
using Serilog.Sinks.Graylog;

/*
    參考網址： https://www.ruyut.com/2023/09/csharp-log-graylog.html
    (1) 先去 Nuget裝上(照順序)： Serilog、Serilog.Sinks.Console、Serilog.Sinks.Graylog
 */

namespace 使用GrayLog
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string host = "localhost";
            int port = 9000;

            Log.Logger = new LoggerConfiguration()
                        .MinimumLevel.Verbose() // 設定最低顯示層級 預設: Information
                        .WriteTo.Console() // 輸出到 指令視窗
                        .WriteTo.Graylog(new GraylogSinkOptions() // 輸出到 Graylog
                        {
                            HostnameOrAddress = host,
                            Port = port,
                        })
                        .CreateLogger();


            // 訊息範例
            Log.Information("資訊");
            Log.Warning("警告");
            Log.Error("錯誤1");
            Log.Error(new Exception("例外"), "錯誤2");

            // 可以去 http://localhost:9000/search 查看
            // 備註： 目前用他的網址進去 是連不上的，不確定是我哪邊有問題 (應該是我沒有 GrayLog 的 Server)
            Console.WriteLine($"可以用 網頁 進入 http://{host}:{port}/search 查看Log紀錄");

            Log.CloseAndFlush(); // 程式結束時關閉和釋放 log 檔案

            Console.ReadLine();
        }
    }
}
