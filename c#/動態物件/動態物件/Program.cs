using System;
using Microsoft.CSharp.RuntimeBinder;
using System.Reflection;
using System.Dynamic;
using System.Linq;
using System.Collections.Generic;

/*
    使用 ExpandoObject
    參考網圵： https://blog.darkthread.net/blog/expandoobject/
 */

namespace 動態物件
{

    public class A
    {
        public string Name { get; set; }

        // 較特別的用法
        public Action<string> Show { get; set; }

        public Action<string, int> Show2 { get; set; }

        /*
            還有一種是 Function (前面是參數，最後面一個類型是 回傳)
            public Function<string, bool> func1 {get;set;}
            用法可以像這樣 (IsNum 是自己寫的額外函式， 功能就是 傳入 string 回傳 bool)
            func1 = (s) => IsNum(s)
        */
        public Function<string, bool> Func1 {get;set;}
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 比較快的方法是這樣，但缺點是 不能像 dynamic .出屬性 

            var rr = new
            {
                abc = 22,
                ccc = "sss"
            };

            // 但不能用 rr.abc 叫出來 (因為視同 Object 物件， 但 Object 物件 沒有 abc 這欄位)

            // -----------------------------------------------------------

            //將boo建立成ExpandoObject
            dynamic boo = new ExpandoObject();

            //直接寫boo.Name加上新的Property
            boo.Name = "Jeffrey";
            Console.WriteLine(boo.Name);

            //ExpandoObject可轉型成IDictionary<stirng, object>
            //讓我們可以用boo[someVariable]的方式加上新的成員
            //在要動態決定物件成員名稱的場合很好用
            IDictionary<string, object> booDict =  boo as IDictionary<string, object>;

            //掛上Show方法
            // (Action<參數類別> 是 能傳至多16個參數，但回傳為void的 method)
            booDict["Show"] = (Action<string>) (m => { Console.WriteLine(m); });

            // 他是傳圵的 => 所以動了 booDict ，而 boo 也一起改變了
            boo.Show("Hello, World!");

            boo.Show2 = (Action<string, int>)((k, v) => { Console.WriteLine($"key = {k}, value = {v}"); });
            boo.Show2("id", 335);

            boo.Func1 = (s) => Test(s);
            bool t = boo.Func1("TRUE");
            Console.WriteLine((t).ToString());

            // 如果要轉型的話 => 就用 AutoMapper 吧
            Console.WriteLine("---------------------------------------------------------------------------");
            A a = new A();
            ExpandoObjectMap<A>(a, boo);
            if (a != null)
            {
                Console.WriteLine(a.Name);
                a.Show("This is A");
                a.Show2("A", 65);
                var b = a.Func1("FALSE");
                Console.WriteLine((b).ToString());
            }

            Console.Read();
        }


        //自已寫map的函式 (擴充方法)
        public static void ExpandoObjectMap<T>(T goal, ExpandoObject source)
        {
            IDictionary<string, object> Dict = source as IDictionary<string, object>;
            List<string> allKeys = Dict.Keys.ToList();

            if (allKeys.Count() > 0)
            {
                Type t = goal.GetType();

                foreach (var pInfo in t.GetProperties())
                {
                    string propertyName = pInfo.Name;

                    try
                    {
                        dynamic v = Dict[propertyName] as dynamic;

                        pInfo.SetValue(goal, v);
                    }
                    catch(Exception ex)
                    {
                        continue;
                    }
                }
            }
        }

        // 測試功能
        public static bool Test(string s){
            return s == "TRUE";
        }
    }
}
