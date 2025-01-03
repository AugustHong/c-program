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

    public class ProgramInfo
    {
        public string Name { get; set; }

        public string Version { get; set; }

        public string InstallDateStr { get; set; }

        public string InstallLocation { get; set; }

        public string Publish { get; set; }

        public int? EstimatedSize { get; set; }
    }

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

            // 延續上面的做法，做更進階 (這個比較優)，基本上跟上面的一樣
            /*
                備註：
                (1) 有些抓到的還是很不準，沒有到我想要的像 控制台/程式集 那且的清楚
                (2) 目前用 Get-StartApps 才真的抓到完全的。 像這裡的 我的 小烏龜 就一直沒出現，但我有安裝
             */
            List<ProgramInfo> totalInstallPrograms = GetTotalInstalledPrograms();
            Console.WriteLine("名稱,發行商,安裝日期,版本號,檔案大小,安裝位置");
            foreach (ProgramInfo info in totalInstallPrograms)
            {
                string EstimatedSize = info.EstimatedSize == null ? string.Empty : info.EstimatedSize.ToString();
                Console.WriteLine($"{info.Name},{info.Publish},{info.InstallDateStr},{info.Version},{EstimatedSize} KB,{info.InstallLocation}");
            }

        }

        /// <summary>
        ///  取得 全部已安裝程式清單
        /// </summary>
        /// <returns></returns>
        static List<ProgramInfo> GetTotalInstalledPrograms()
        {
            string registryKey = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall";
            List<ProgramInfo> localMachineList = GetInstalledPrograms(Registry.LocalMachine.OpenSubKey(registryKey));
            List<ProgramInfo> currentUser = GetInstalledPrograms(Registry.CurrentUser.OpenSubKey(registryKey));

            List<ProgramInfo> result = new List<ProgramInfo>();
            result.AddRange(localMachineList);
            result.AddRange(currentUser);

            return result;
        }

        /// <summary>
        /// 取得 安裝清單
        /// </summary>
        /// <param name="registryKey"></param>
        /// <returns></returns>
        static List<ProgramInfo> GetInstalledPrograms(RegistryKey registryKey)
        {
            List<ProgramInfo> result = new List<ProgramInfo>();
            if (registryKey != null)
            {
                foreach (string subKey in registryKey.GetSubKeyNames())
                {
                    using (RegistryKey key = registryKey.OpenSubKey(subKey))
                    {
                        if (key != null)
                        {
                            string displayName = key.GetValue("DisplayName") as string;
                            string displayVersion = key.GetValue("DisplayVersion") as string;
                            string publisher = key.GetValue("Publisher") as string;
                            string installDate = key.GetValue("InstallDate") as string;
                            string installLocation = key.GetValue("InstallLocation") as string;
                            int? estimatedSize = (int?)key.GetValue("EstimatedSize");

                            if (!string.IsNullOrWhiteSpace(displayName))
                            {
                                result.Add(new ProgramInfo
                                {
                                    Name = displayName,
                                    Version = displayVersion,
                                    Publish = publisher,
                                    InstallDateStr = installDate,
                                    InstallLocation = installLocation,
                                    EstimatedSize = estimatedSize
                                });
                            }
                        }
                    }
                }

            }

            return result;
        }
    }
}
