using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

/*
    參考網址： https://dotblogs.com.tw/MemoryRecall/2023/06/17/161856
    因為權限的關係，這邊只使用  HKEY_CURRENT_USER  這個來操作
    平常有這 5個 (就是你用 regedit一進入看到的5個資料夾)
    HKEY_CLASSES_ROOT
    HKEY_CURRENT_USER
    HKEY_LOCAL_MACHINE
    HKEY_USERS
    HKEY_CURRENT_CONFIG
 */

namespace 對RegEdit增刪修讀
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 建立實體 (使用的是 HKEY_CURRENT_USER 所以是 Registry.CurrentUser)
            // 而裡面的 SOFTWARE\TEST 就是路徑 (沒有應該會自己重建)，這裡建立的是 機碼
            RegistryKey RegKey = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\TEST", true);

            string key = "Test1";

            // 寫入 (沒有會自己新增)
            RegKey.SetValue(key, "A");

            // 讀取
            string v = RegKey.GetValue(key)?.ToString();
            Console.WriteLine(v);

            // 再次修改
            RegKey.SetValue(key, "B");
            v = RegKey.GetValue(key)?.ToString();
            Console.WriteLine(v);

            // 在這機碼底下再建1個機碼
            RegKey.CreateSubKey(@"KEY1", true);

            Console.WriteLine("----------------------------------");

            // 取所有的子機碼名稱 (應該會抓到 上面多建的 KEY1)
            List<string> allSubKey = RegKey.GetSubKeyNames().ToList();
            foreach (var a in allSubKey)
            {
                Console.WriteLine(a);
            }

            Console.WriteLine("----------------------------------");

            // 取所有的登錄值碼名稱 (我們上面建立的就是這種)
            List<string> allSubKeyValue = RegKey.GetValueNames().ToList();
            foreach (var a in allSubKeyValue)
            {
                Console.WriteLine(a);
            }

            // 刪除登錄值碼
            RegKey.DeleteValue(key);

            // 刪除機碼
            RegKey.DeleteSubKey("KEY1");  //只要輸入KEY1 是因為我們在其底下建立了，所以不用加前面的

            // 若底下有子機碼要層層刪除，就用此
            // RegKey.DeleteSubKeyTree(key);

            Console.ReadLine();
        }
    }
}
