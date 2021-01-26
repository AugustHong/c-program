using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 自創CMD指令程式
{
    public class ReturnCmdModel
    {
        public int ReturnCode { get; set; }

        public string Text { get; set; }
    }

    class Program
    {
        /*
            可以在 cmd 打上 
            自創CMD指令程式.exe Program_A -p A參數 B參數 Program_B
            自創CMD指令程式.exe Program_A -p A參數 Program_B
            自創CMD指令程式.exe Program_A -p   B參數 Program_B
            自創CMD指令程式.exe Program_A -p Program_B
            自創CMD指令程式.exe Program_A Program_B
         */

        static void Main(string[] args)
        {
            if (args.Count() <= 0)
            {
                Console.WriteLine("沒有輸入相關系統名稱。");
                Console.WriteLine("請輸入 自創CMD指令程式.exe A   這種的語法");
            }
            else
            {
                // 得到系統名稱
                int count = args.Count();
                for (var i = 0; i < count; i++)
                {
                    string sysName = args[i];

                    // 依據何種系統名稱 做什麼
                    // 以後改成讀DB 後 用 映射的
                    #region 依據系統名稱做何事

                    ReturnCmdModel result = new ReturnCmdModel();
                    switch (sysName)
                    {
                        case "Program_A":
                            List<string> parm = GetParms((i + 1), args, 2, ref i);
                            result = A(parm[0], parm[1]);
                            break;
                        case "Program_B":
                            result.ReturnCode = 0;
                            result.Text = "成功執行完 程式 B";
                            break;
                        default:
                            result.ReturnCode = -1;
                            result.Text = "當前的系統名稱前未有你輸入的，請確定是否正常";
                            break;
                    }

                    Console.WriteLine($"-----------------------------------------{sysName}------------------------------------");
                    Console.WriteLine($"Return Code = {result.ReturnCode}");
                    Console.WriteLine($"訊息文字： {result.Text}");

                    Console.WriteLine("====================================================================");

                    // 0 ~8 是可接受的
                    if (result.ReturnCode < 0 || result.ReturnCode > 8)
                    {
                        Console.WriteLine($"ReturnCode = {result.ReturnCode}  --> 錯誤，後面的程式皆不執行");
                        break;
                    }
                    else
                    {
                        if (result.ReturnCode != 0)
                        {
                            Console.WriteLine($"ReturnCode = {result.ReturnCode} 不是 0 ，請確認訊息");
                        }
                    }

                    #endregion
                }
            }
        }

        static ReturnCmdModel A(string a, string b)
        {
            ReturnCmdModel result = new ReturnCmdModel
            {
                ReturnCode = 0,
                Text = $"你輸入的 a = {a}, b = {b}"
            };
            return result;
        }

        /// <summary>
        ///  取參數的部份
        /// </summary>
        /// <param name="index">序號</param>
        /// <param name="args">陣列</param>
        /// <param name="parmCount">要取幾個參數</param>
        /// <returns></returns>
        static List<string> GetParms(int index, string[] args, int parmCount, ref int i)
        {
            string tmp = string.Empty;

            if (index >= args.Count())
            {
                for (var j = 1; j <= parmCount; j++)
                {
                    tmp += ";";
                }
            }
            else
            {
                // -p 後面接參數
                if (args[index] == "-p" || args[index] == "-P")
                {
                    i = i + 1;

                    int c = 1;
                    while (c <= parmCount)
                    {
                        string v = args[index + c];

                        // 這邊是判斷 下一個是不是 你的系統程式 (可自由決定要不要加)
                        // 如果是系統程式 不管當前取到第幾個參數，馬上斷開
                        if (!v.Contains("Program_"))
                        {
                            tmp += $"{v};";
                            i = i + 1;
                        }
                        else
                        {
                            for (var j = c; j <= parmCount; j++)
                            {
                                tmp += ";";
                            }
                            break;
                        }

                        c++;
                    }
                }
                else
                {
                    for (var j = 1; j <= parmCount; j++)
                    {
                        tmp += ";";
                    }
                }
            }

            return tmp.Split(';').ToList();
        }
    }
}
