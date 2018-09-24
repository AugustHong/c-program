using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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



            Console.Read();
        }
    }
}
