using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Text;   //要使用StringBuilder時要引用

namespace Dictionary_Type
{
    class Program
    {
        static void Main(string[] args){

            //字典新增有2個方法：（前面都是key，後面是value）

            //方法一：
            Dictionary<int, string> Member = new Dictionary<int, string>()
            {
                {1, "jack" },
                {2, "me" },
                {3, "merry" },
                {4, "louie" }
            };

            //方法二：
            Dictionary<int, string> Member2 = new Dictionary<int, string>();
            Member2.Add(5, "john");
            Member2.Add(6, "bob");
            Member2.Add(7, "choo");

            //列出所有資料
            foreach(var item in Member){
                Console.WriteLine($"key = {item.Key} , value = {item.Value}");
            }

            //
            //字典仍是陣列型態的一種，所以可以用where find ……這種方法
            //
            foreach(var item in Member2.Where(m => m.Key >= 6)){
                Console.WriteLine($"key = {item.Key} , value = {item.Value}");
            }

            Console.WriteLine();
            //----------------------------------------------------------------------------------------

            //StringBuilder的使用（類似字串）

            //宣告方法（三種）
            StringBuilder stringBuild1 = new StringBuilder();

            //一開始給值
            StringBuilder stringBuild2 = new StringBuilder("hello");

            //給值，並且給他長度上限
            StringBuilder stringBuild3 = new StringBuilder("hello", 20);

            //設定長度上限
            stringBuild2.Capacity = 15;

            //新增值加入進去
            stringBuild1.Append("hahaha");
            Console.WriteLine(stringBuild1);   //輸出：hahaha

            stringBuild2.Append(" world");
            Console.WriteLine(stringBuild2);   //輸出：hello world

            //新增值加入進去（並且新增的是格式化的值）
            stringBuild3.AppendFormat(" total is {0:c} dollors", 25);   
            Console.WriteLine(stringBuild3);   //輸出：hello total is $25.00  dollors

            //將值放在某個位置中
            stringBuild2.Insert(6, "Beautiful ");  
            Console.WriteLine(stringBuild2); //輸出： hello Beautiful world

            //將某個位置的取多少字元刪除（從0開始算，到第2個字元開始刪4個字元）
            stringBuild1.Remove(2, 4);
            Console.WriteLine(stringBuild1);  //輸出：ha

            //取代
            stringBuild1.Replace("h", "k");
            Console.WriteLine(stringBuild1);  //輸出：ka

            //
            //將 StringBuilder 物件轉換為字串
            //

            StringBuilder sb = new StringBuilder();
            string[] spellings = { "recieve", "receeve", "receive" };
            sb.AppendFormat("Which of the following spellings is {0}:", true);

            //加入斷行
            sb.AppendLine();

            for (int ctr = 0; ctr <= spellings.GetUpperBound(0); ctr++)
            {
                sb.AppendFormat("   {0}. {1}", ctr, spellings[ctr]);
                sb.AppendLine();
            }

            sb.AppendLine();

            //有著斷行的並非直接是字串，所以要用ToString()去把他變成字串
            Console.WriteLine(sb.ToString());

            //輸出：
            //       Which of the following spellings is True:
            //          0. recieve
            //          1. receeve
            //          2. receive


            Console.Read();
        }
    }
}
