using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址： https://dotblogs.com.tw/jimmyyu/2009/10/29/11351
 */

namespace 查詢事件檢視器
{
    internal class Program
    {
        static void Main(string[] args)
        {
            EventLog tLog = new EventLog();

            /*
                這邊的類型 就是對到  事件檢視器/WIndows 紀錄 裡面的項目
                -  應用程式 (Application)
                -  安全性 (Secunity)
                -  Setup (Setup)
                -  系統 (System)
                -  Forwarded Events (Forwarded Events)
             */

            //選取Application類的Log
            tLog.Log = "Application";

            List<EventLogEntry> logList = new List<EventLogEntry>();
            foreach (EventLogEntry l in tLog.Entries)
            {
                logList.Add(l);
            }

            // 逆排
            logList = logList.OrderByDescending(x => x.TimeGenerated).ToList();

            // 抓今天的就好，不然資料太多了
            foreach (EventLogEntry l in logList.Where(x => x.TimeGenerated.ToString("yyyyMMdd") == DateTime.Now.ToString("yyyyMMdd")))
            {
                string source = l.Source; //來源
                long instanceId = l.InstanceId; //資源識別碼
                string message = l.Message; // 訊息
                DateTime time = l.TimeGenerated; //日期和時間
                string userName = l.UserName; // 使用者
                string entryType = l.EntryType.ToString(); // Warning、 Error …等
                int eventId = l.EventID; //事件識別碼
                string category = l.Category; //工作類別

                Console.WriteLine($"等級 = {entryType}, 時間 = {time}, 來源 = {source}, 事件識別碼 = {eventId}, 工作類別 = {category}");
            }

            Console.ReadLine();
        }
    }
}
