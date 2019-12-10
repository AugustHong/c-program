using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Nut;

/*
    參考網圵： https://blog.darkthread.net/blog/number-to-text-lib/
    
    步驟1： 去 NuGet 裝上 Nut
    步驟2： using Nut;
*/

namespace 數字轉英文
{
    class Program
    {
        static void Main(string[] args)
        {
            int test = 1234567;

            // 預設是英文 ，也可以選別的語言
            Console.WriteLine("英文 ： " + test.ToText());
            Console.WriteLine("法文 ： " + test.ToText(Nut.Language.French));
            Console.WriteLine("西班牙文 ： " + test.ToText(Nut.Language.Spanish));
            Console.WriteLine("土耳其文 ： " + test.ToText(Nut.Language.Turkish));

            Console.WriteLine("------------------------------------------------------------------");

            // 如果是 decimal 的話 預設是貨幣型式 => 所以要再多給參數 ，但基本上用美元即可
            decimal test2 = 12345.344m;
            Console.WriteLine("有小數點的英文 ： " + test2.ToText(Nut.Currency.USD, "en"));


            Console.Read();
        }
    }
}
