using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

// 這邊下面要實作 json 轉成 class , 故去 NuGet 裝上 Newtonsoft.Json
using Newtonsoft.Json;

/*
    參考網圵： https://blog.yowko.com/string-create-instance/   (第一部份)

    參考網圵： https://blog.csdn.net/huoliya12/article/details/78873123 (第二部份)
*/

namespace UseStringToCreateNewClassInstance
{
    public class A
    {
        public string B { get; set; }
        public int C { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 得到 Class 中文實體名稱
            A a = new A { B = "bb", C = 0 };
            string typename = typeof(A).AssemblyQualifiedName;
            Console.WriteLine(typename);

            // 用 剛才得到的長長的實體名稱， 得到 Type 型別 (用於  IResopoty<T> 的 T的位置)
            Type dataType = Type.GetType(typename);

            // 用 剛才得到的長長的實體名稱 ， 去建立 新的 Class (可用於泛形相關應用) => 會得到全新的 Class (但目前未知如何轉成強型別)，故用 dynamic來做
            // 最好用 Interface 或是 父類別  來接
            dynamic parameterInstance = Activator.CreateInstance(dataType);
            parameterInstance.B = "ccc";
            Console.WriteLine(parameterInstance is A);


            // 實作 ： 利用傳入 JSON 格式， 將資料解開後 轉成 物件 (值都在)
            List<A> testA = new List<A> { new A { B = "測試", C = 10 }, new A { B = "測試2", C = 20 } };
            string TypeName = typeof(List<A>).AssemblyQualifiedName;
            Console.WriteLine(TypeName);

            string jsonstr = JsonConvert.SerializeObject(testA);   // 化成 JSON 文字
            Console.WriteLine(jsonstr);

            // 解開 並化成實體
            Type DataType = Type.GetType(TypeName);
            var obj = JsonConvert.DeserializeObject(jsonstr, DataType);
            Console.WriteLine(obj is List<A>);

            // 未知如何變成強型別 (照理來說要用個 Interface 或者是 繼承的父類別  來做 => 但很難就是了)
            // 正常寫法
            foreach (var item in (List<A>)obj)
            {
                Console.WriteLine($"B = {item.B}, C = {item.C}");
            }

            // 解法： 用 dynamic 來接
            // 最好用 Interface 或是 父類別  來接
            dynamic o2 = obj;
            foreach (var item in o2)
            {
                Console.WriteLine($"B = {item.B}, C = {item.C}");
            }


            // ---------------------------------------------------------------------------------------

            // 找某個 namespace 下的 所有 class 
            List<Type> classes = Assembly.Load("UseStringToCreateNewClassInstance").GetTypes().Where(t => !t.Name.Contains("<>")).ToList();
            foreach (var item in classes)
            {
                Console.WriteLine(item.Name);
            }

            Console.ReadLine();
        }
    }
}
