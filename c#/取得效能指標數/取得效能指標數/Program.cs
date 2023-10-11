using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
    參考網址： https://blog.darkthread.net/blog/performancecounter-c-sample/

    備註：如果不知道分類、計數器及執行個體名稱，可以參考 效能監視器 的新増計數器介面
 */

namespace 取得效能指標數
{
    internal class Program
    {
        /*
           new PerformanceCounter("物件", "計數器", "例項");
           上面的 資料可以開啟  效能監視器 就知道在指什麼了，然後要新增就看  效能監視器  的分類
         */

        // CPU
        static PerformanceCounter cpu = new PerformanceCounter("Processor", "% Processor Time", "_Total");
        // 記憶體
        static PerformanceCounter memory = new PerformanceCounter("Memory", "% Committed Bytes in Use");
        // 磁碟
        static PerformanceCounter disk = new PerformanceCounter("LogicalDisk", "% Disk Time", "_Total");
        // 檔案讀取速度
        static PerformanceCounter fileRead = new PerformanceCounter("System", "File Read Bytes/sec");
        // 標案寫入速度
        static PerformanceCounter fileWrite = new PerformanceCounter("System", "File Write Bytes/sec");

        // 除了 CPU 和 記憶體 外，其他不確定是不是對的。但 效能監視器 上的分類又太多也不知道實際是不是我要的

        static void Main(string[] args)
        {
            int i = 0;
            while (i < 10)
            {
                Console.WriteLine("CPU: {0:n1}%", cpu.NextValue());
                Console.WriteLine("Memory: {0:n1}%", memory.NextValue());
                Console.WriteLine("DISK: {0:n1}%", disk.NextValue());
                Console.WriteLine("File Read: {0}  Bytes/sec", fileRead.NextValue());
                Console.WriteLine("File Write: {0}  Bytes/sec", fileWrite.NextValue());
                Console.WriteLine("--------------------------------------------------------------------");
                Thread.Sleep(1000);
                i++;
            }
            Console.Read();
        }
    }
}
