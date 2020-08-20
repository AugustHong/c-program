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

        public static Process process = new Process();

        static void Main(string[] args)
        {
            string command = "help";
            string response = RunCmd(command);
            
            List<string> commandList = new List<string>{"dir", "help"};
            response = RunManyCmd(commandList);

            Console.Read();
        }

        /// <summary>
        /// 執行 cmd 指令
        /// 一定要一定 把一個process 結束掉
        /// 不然一直開著 也取不到回覆(超級怪)
        /// 解法： 寫個 可以一次跑多個指令的，然後把之前單元測試可以過的 cmd 組起來 只能這樣了
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        static string RunCmd(string command, int sleepTime = 0)
        {
            string result = string.Empty;

            if (!string.IsNullOrEmpty(command))
            {
                try
                {
                    process = new Process();

                    // 相關設定 (基本上就照抄即可)
                    process.StartInfo.UseShellExecute = false;   //是否使用作業系統shell啟動 
                    process.StartInfo.CreateNoWindow = true;   //是否在新視窗中啟動該程序的值 (不顯示程式視窗)
                    process.StartInfo.RedirectStandardInput = true;  // 接受來自呼叫程式的輸入資訊 
                    process.StartInfo.RedirectStandardOutput = true;  // 由呼叫程式獲取輸出資訊
                    process.StartInfo.RedirectStandardError = true;  //重定向標準錯誤輸出

                    process.StartInfo.FileName = "cmd.exe";   // 要執行的程式名

                    process.Start();                         // 啟動程式

                    //说明：不管命令是否成功均执行exit命令，否则当调用ReadToEnd()方法时，会处于假死状态
                    Console.WriteLine($"執行 {command}");

                    command = command.Trim().TrimEnd('&') + "&exit";

                    process.StandardInput.WriteLine(command); //向cmd視窗傳送輸入資訊
                    process.StandardInput.AutoFlush = true;  // 自動刷新
                    

                    // 用 git 的等幾秒
                    if (sleepTime > 0)
                    {
                        System.Threading.Thread.Sleep(sleepTime);
                    }

                    // 回傳結果
                    StreamReader Response = process.StandardOutput;
                    string responseMessage = Response.ReadToEnd();

                    // response 拿 第5行開始的資料
                    List<string> responseMessageList = Regex.Split(responseMessage, "\r\n", RegexOptions.IgnoreCase).ToList();
                    for (var j = 4; j < responseMessageList.Count(); j++)
                    {
                        string item = responseMessageList[j];
                        if (!string.IsNullOrEmpty(item))
                        {
                            result += item + "\r\n";
                        }
                    }

                    //錯誤
                    StreamReader Error = process.StandardError;
                    string errorMessage = Error.ReadToEnd();

                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        result += errorMessage;
                    }

                    Console.WriteLine(result);

                    process.Close();

                    return result;
                }
                catch (Win32Exception e)
                {
                    Console.WriteLine(e.Message);
                    process.Close();
                }
            }

            return result;
        }

        /// <summary>
        /// 一次 run 多個指令
        /// 為解決上面的問題，只好每個單元測試過可以 再新增讓下一行可以延用之前的路徑
        /// </summary>
        /// <param name="commandList"></param>
        /// <param name="sleepTime"></param>
        /// <returns></returns>
        static string RunManyCmd(List<string> commandList, int sleepTime = 0)
        {
            string result = string.Empty;

            if (commandList.Count() > 0)
            {
                try
                {
                    List<string> runCommandList = new List<string>();
                    runCommandList.AddRange(commandList);

                    int filter = 2;  // 因為是 前2行 + 你執行的指令數 *2 (每個指令前一行都有個空白行)

                    // 因為要讀到回覆值， 所以最後一段加上 exit
                    runCommandList.Add("exit");

                    process = new Process();

                    // 相關設定 (基本上就照抄即可)
                    process.StartInfo.UseShellExecute = false;   //是否使用作業系統shell啟動 
                    process.StartInfo.CreateNoWindow = true;   //是否在新視窗中啟動該程序的值 (不顯示程式視窗)
                    process.StartInfo.RedirectStandardInput = true;  // 接受來自呼叫程式的輸入資訊 
                    process.StartInfo.RedirectStandardOutput = true;  // 由呼叫程式獲取輸出資訊
                    process.StartInfo.RedirectStandardError = true;  //重定向標準錯誤輸出

                    process.StartInfo.FileName = "cmd.exe";   // 要執行的程式名

                    process.Start();                         // 啟動程式


                    foreach (var command in runCommandList)
                    {
                        if (!string.IsNullOrEmpty(command))
                        {
                            Console.WriteLine($"執行 {command}");

                            process.StandardInput.WriteLine(command); //向cmd視窗傳送輸入資訊
                            process.StandardInput.AutoFlush = true;  // 自動刷新

                            filter += 2;
                        }
                    }

                    // 用 git 的等幾秒
                    if (sleepTime > 0)
                    {
                        System.Threading.Thread.Sleep(sleepTime);
                    }

                    // 回傳結果
                    StreamReader Response = process.StandardOutput;
                    string responseMessage = Response.ReadToEnd();
                    if (!string.IsNullOrEmpty(responseMessage))
                    {
                        result += responseMessage;
                    }

                    //錯誤
                    StreamReader Error = process.StandardError;
                    string errorMessage = Error.ReadToEnd();

                    if (!string.IsNullOrEmpty(errorMessage))
                    {
                        result += errorMessage;
                    }

                    Console.WriteLine(result);

                    process.Close();

                    return result;
                }
                catch (Win32Exception e)
                {
                    Console.WriteLine(e.Message);
                    process.Close();
                }
            }

            return result;
        }
    }
}
