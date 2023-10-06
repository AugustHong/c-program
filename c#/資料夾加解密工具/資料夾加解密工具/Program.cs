using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址： https://www.itread01.com/article/1457332726.html
 */

namespace 資料夾加解密工具
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 後面加上 這個 就會變安全鎖(但用程式一樣可以讀取)
            string eKey = ".{2559a1f2-21d7-11d4-bdaf-00c04f60b9f0}";

            string quit = "";
            while (quit.ToUpper() != "EXIT"){
                Console.Write("請選擇動作  1 = 離開程式  2 = 加密資料夾  3 = 解密資料夾   ： ");
                string action = Console.ReadLine();
                if (action == "1") { quit = "EXIT"; }
                else
                {
                    if (action != "2" && action != "3")
                    {
                        Console.WriteLine("請輸入上列的數字選項(例如： 1 or 2 ...)");
                    }
                    else
                    {
                        Console.Write("請輸入要 加/解密 的資料夾路徑 (絕對路徑) ： ");
                        string inputPath = Console.ReadLine();
                        string dInputPath = inputPath + eKey; //因為 加密後的會多這1段，但外面在選的時候不知道
                        if (!Directory.Exists(inputPath) && !Directory.Exists(dInputPath))
                        {
                            Console.WriteLine("所輸入的路徑查無此資料夾");
                        }
                        else
                        {
                            Console.Write("請輸入要 密碼 (沒有就空白) ： ");
                            string passowrd = Console.ReadLine();

                            if (action == "2")
                            {
                                // 加密
                                string msg = DirEncryption(inputPath, eKey, passowrd);
                                if (!string.IsNullOrEmpty(msg))
                                {
                                    Console.WriteLine(msg);
                                }
                                else
                                {
                                    Console.WriteLine("加密完成");
                                }
                            }
                            else
                            {
                                // 解密
                                string msg = DirDecrypt(dInputPath, passowrd);
                                if (!string.IsNullOrEmpty(msg))
                                {
                                    Console.WriteLine(msg);
                                }
                                else
                                {
                                    Console.WriteLine("解密完成");
                                }
                            }
                        }

                    }

                    Console.WriteLine("============================================================");
                }
            }

        }

        /// <summary>
        /// 資料夾加密
        /// </summary>
        /// <param name="absPath">絕對路徑</param>
        /// <param name="password">密碼</param>
        /// <returns></returns>
        static string DirEncryption(string absPath, string eKey, string password = "")
        {
            string result = "";

            try
            {
                DirectoryInfo d = new DirectoryInfo(absPath);

                // 如果有輸入密碼，要寫入密碼本
                if (!string.IsNullOrEmpty(password))
                {
                    // 寫檔
                    string fileName = "資料夾加解密工具.psd.txt";
                    using (StreamWriter sw = new StreamWriter(absPath + "\\" + fileName))
                    {
                        sw.WriteLine(password);
                    }
                }

                // 將資料夾加密
                d.MoveTo(absPath + eKey);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }

        /// <summary>
        /// 資料夾解密
        /// </summary>
        /// <param name="absPath">絕對路徑</param>
        /// <param name="password">密碼</param>
        /// <returns></returns>
        static string DirDecrypt(string absPath, string password = "")
        {
            string result = "";

            try
            {
                DirectoryInfo d = new DirectoryInfo(absPath);
                string passwordFileName = "資料夾加解密工具.psd.txt";
                bool havePassword = d.GetFiles().Where(x => x.Name == passwordFileName).Count() > 0;
                string readPassword = "";

                // 如果有密碼的要多密碼判斷
                if (havePassword)
                {
                    // 去讀取出密碼
                    using (StreamReader sr = new StreamReader(absPath + "\\" + passwordFileName))
                    {
                        readPassword = sr.ReadToEnd();
                    }

                    if (password != readPassword.Trim())
                    {
                        throw new Exception("密碼錯誤！！");
                    }
                    else
                    {
                        File.Delete(absPath + "\\" + passwordFileName);
                    }
                }

                string path = absPath.Substring(0, absPath.LastIndexOf("."));
                d.MoveTo(path);
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }
    }
}
