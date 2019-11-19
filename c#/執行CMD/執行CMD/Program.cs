using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;  // 最重要的
using System.ComponentModel;
using System.IO;

// 參考網圵： https://www.itread01.com/content/1548903965.html

namespace 執行CMD
{
    class Program
    {
        static void Main(string[] args)
        {
            // 建立一個 Process
            Process process = new Process();
            try
            {
                // 相關設定 (基本上就照抄即可)
                process.StartInfo.UseShellExecute = false;   //是否使用作業系統shell啟動 
                process.StartInfo.CreateNoWindow = true;   //是否在新視窗中啟動該程序的值 (不顯示程式視窗)
                process.StartInfo.RedirectStandardInput = true;  // 接受來自呼叫程式的輸入資訊 
                process.StartInfo.RedirectStandardOutput = true;  // 由呼叫程式獲取輸出資訊
                process.StartInfo.RedirectStandardError = true;  //重定向標準錯誤輸出

                process.StartInfo.FileName = "cmd.exe";   // 要執行的程式名

                process.Start();                         // 啟動程式

                process.StandardInput.WriteLine("help"); //向cmd視窗傳送輸入資訊
                process.StandardInput.AutoFlush = true;  // 自動刷新

                StreamReader reader = process.StandardOutput;//獲取exe處理之後的輸出資訊
                string curLine = reader.ReadLine(); // 讀取回傳的資訊 (基本上可以去掉前4行)

                /*
                    前4行是： (其中有一行是 空欄)
                    Microsoft Windows [版本 10.0.18362.476]
                    (c) 2019 Microsoft Corporation. 著作權所有，並保留一切權利。
                    執行的路徑 ( C:> help ) 這種  --> 不該出來
                    
                 */

                int i = 1;
                while (!reader.EndOfStream)
                {
                    if (i > 4)
                    {
                        if (!string.IsNullOrEmpty(curLine))
                        {
                            Console.WriteLine(curLine);
                        }
                    }

                    curLine = reader.ReadLine();
                    i++;
                }

                reader.Close(); //close程序

                process.WaitForExit();  //等待程式執行完退出程序
                process.Close();

            }
            catch (Win32Exception e)
            {
                Console.WriteLine(e.Message);
            }


            Console.Read();
        }
    }
}
