using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址： https://www.ez2o.com/Blog/Post/csharp-Get-Process-PerformanceCounter
 */

namespace 取得當前執行程式CPU和記憶體使用量
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 先取得當前執行的所有程式 (用使用量來排序)
            List<Process> pList = Process.GetProcesses().OrderByDescending(x => x.WorkingSet64).ToList();

            foreach (var p in pList)
            {
                string pid = p.Id.ToString();
                string processName = p.ProcessName;

                // 抓出 CPU、記憶體 使用量  (與自己得到有些許落差，先留著紀錄用)
                PerformanceCounter cpuC = new PerformanceCounter("Process", "% Processor Time", processName);
                PerformanceCounter ramC = new PerformanceCounter("Process", "Working Set", processName);

                string ram = (ramC.NextValue() / 8 / 1024).ToString();  // 現在不確定這個得到的單位到底是什麼
                string cpu = Math.Round(cpuC.NextValue(), 2).ToString();

                Console.WriteLine($"PID = {pid},  PNAME = {processName}, CPU = {cpu} %, RAM = {ram} KB");
            }

            Console.ReadLine();
        }
    }
}
