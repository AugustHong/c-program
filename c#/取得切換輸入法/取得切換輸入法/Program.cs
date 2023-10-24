using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
    參考網址： https://vocus.cc/article/63c56fe0fd897800014b251a
    參考/右鍵/加入參考/組件/System.Windows.Forms;
 */

namespace 取得切換輸入法
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
                注意： 目前實測 切換輸入法 實際都沒切成功，還是原本的。(有可能都是 繁中輸入所以切不了？)，目前還未解
                        但值確實是改了 (在猜有可能是他一定要切換到語系)
             */

            // 取得當前輸入法
            InputLanguage currentLanguage = InputLanguage.CurrentInputLanguage;
            Console.WriteLine($"當前輸入法 = {currentLanguage.LayoutName},  輸入法語系 = {currentLanguage.Culture.Name}");

            // 取得預設輸入法
            InputLanguage defaultLanguage = InputLanguage.DefaultInputLanguage;
            Console.WriteLine($"預設輸入法 = {defaultLanguage.LayoutName},  輸入法語系 = {defaultLanguage.Culture.Name}");

            // 設定輸入法 by 語系
            InputLanguage newLanguage = InputLanguage.FromCulture(new CultureInfo("zh-TW"));
            InputLanguage.CurrentInputLanguage = newLanguage;

            // 如果要指定的話 那就去下面 找所有可用的輸入法 ，再用 name 去篩選出來後再指定 (這邊就改為預設輸入法)
            Console.WriteLine("切換為 預設輸入法 …");
            InputLanguage.CurrentInputLanguage = defaultLanguage;
            Console.WriteLine($"當前輸入法 = {InputLanguage.CurrentInputLanguage.LayoutName},  輸入法語系 = {InputLanguage.CurrentInputLanguage.Culture.Name}");

            Console.WriteLine("");
            Console.WriteLine("--------------------------可用輸入法-------------------------");
            InputLanguage assignLanguage = defaultLanguage;
            // 取得所有可用的輸入法
            foreach (InputLanguage inputLanguage in InputLanguage.InstalledInputLanguages)
            {
                Console.WriteLine($"輸入法名稱 = {inputLanguage.LayoutName}, 輸入法語系 = {inputLanguage.Culture.Name}");

                if (inputLanguage.LayoutName.Contains("嘸"))
                {
                    assignLanguage = inputLanguage;
                }
            }

            // 改成指定的輸入法
            InputLanguage.CurrentInputLanguage = assignLanguage;

            Console.Read();
        }
    }
}
