using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console使用Args
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Count() > 0)
            {
                for (var i = 0; i < args.Count(); i++)
                {
                    Console.WriteLine($"第 {i + 1} 個參數 = {args[i]}");
                }
            }


            Console.WriteLine("------------------------------");
            Console.WriteLine("結束程式");
        }

        /*
            執行步驟：
            1. 打開 cmd
            2. 指到 exe 的位址。例如： cd C:\Users\green\Desktop\Console使用Args\Console使用Args\bin\Debug
            3. 使用 exe 並帶入參數。(各參數間用 1個 半形空白隔開)例如： 
            Console使用Args.exe 111 333 555 999 888 jjj aaa ccc kkk lll ddd yyy zzz YOU 你好 大家好

            結果如下：
            第 1 個參數 = 111
            第 2 個參數 = 333
            第 3 個參數 = 555
            第 4 個參數 = 999
            第 5 個參數 = 888
            第 6 個參數 = jjj
            第 7 個參數 = aaa
            第 8 個參數 = ccc
            第 9 個參數 = kkk
            第 10 個參數 = lll
            第 11 個參數 = ddd
            第 12 個參數 = yyy
            第 13 個參數 = zzz
            第 14 個參數 = YOU
            第 15 個參數 = 你好
            第 16 個參數 = 大家好
            ------------------------------
            結束程式

         */
    }
}
