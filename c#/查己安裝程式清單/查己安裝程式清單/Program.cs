using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址： https://blog.csdn.net/soft2buy/article/details/5795261
    基本上就是去讀取 Regedit 的內容即可， 也可參考我有一篇  對Regedit增刪修查 的部份
 */

namespace 查己安裝程式清單
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string key = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";

            // 去查出資料
            using (RegistryKey  rk = Registry.LocalMachine.OpenSubKey(key))
            {
                // 列出所有的 Key資料
                foreach (string skName in rk.GetSubKeyNames()) 
                {
                    // 再用 Key 去查出詳細資料
                    using (RegistryKey sk = rk.OpenSubKey(skName)) 
                    {
                        string displayName = sk.GetValue("DisplayName") == null ? "" : sk.GetValue("DisplayName").ToString();
                        string installLocation = sk.GetValue("InstallLocation") == null ? "" : sk.GetValue("InstallLocation").ToString();

                        if (!string.IsNullOrEmpty(displayName))
                        {
                            Console.WriteLine($"程式名稱 =  {displayName}  ,  路徑 = {installLocation}");
                        }
                    }
                }
            }

            Console.ReadLine();
        }
    }
}
