using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generic
{
    class Program
    {
        static void Main(string[] args)
        {


            List<A> a = new List<A>();
            List<B> b = new List<B>();


            //指定放法（用接收的名稱:值）
            GetInfo<A>(a, b: "this is b", a: "this is a");
            GetInfo<B>(b, b: "ha ha ha");

            //c# 的 wait
            System.Threading.Thread.Sleep(1000);


            for (var i = 0; i <= 3; i++)
            {
                Console.WriteLine($"a value: {a[i].a.ToString()}  b value : {b[i].b.ToString()}");
            }

            Console.Read();

        }

        //輸入什麼類型，就回傳什麼類型（List<T> result不用加ref，會帶回去）
        // where T : class  是代表這個T要是class
        public static void GetInfo<T>(List<T> result, string a = "", string b="") where T : class
        {
            for(var i = 0; i <= 3; i++)
            {
                //如果result 傳入的類型是 AppAreaInfo
                if (result is List<A>)
                {
                    A data = new A
                    {
                        a = i
                    };

                    //轉型成T才能夠add
                    result.Add(data as T);
                }


                //如果result 傳入的類型是 AirAreaInformation
                if (result is List<B>)
                {
                    B data = new B
                    {
                        b = i * 2
                    };

                    //轉型成T才能夠add
                    result.Add(data as T);
                }

            }

            Console.WriteLine($"a = {a} b = {b}");
        }
    }

    public class A
    {
        public int a;
    }

    public class B
    {
        public int b;
    }
}
