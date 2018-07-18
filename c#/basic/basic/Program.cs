using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace basic
{
    class Program
    {
        static void Main(string[] args){

            var x = "null";  /* var是一個宣態型別， 可以當作任何（未給值時），當給值時即是那個type不可改 */

            int a = 10;
            int b = 20;

            double c = 1.0;
            double d = 3.0;

            decimal f = 1.0M;
            decimal g = 3.0M;

            string h = "Hello";
            string he = "325";
            string[] i = { "this", "is", "a", "book" };

            Console.WriteLine(x);
            Console.WriteLine(a);
            Console.WriteLine($"a = {a}");
            Console.WriteLine(a + b);
            Console.WriteLine($"a+b= {a + b}");
            Console.WriteLine(a.ToString() + b.ToString());
            Console.WriteLine(c / d);
            Console.WriteLine(f / g);

            Console.WriteLine("");

            Console.WriteLine(he);
            Console.WriteLine($"string to in : {Int32.Parse(he) }");
            Console.WriteLine(h);
            Console.WriteLine($"h to Upper: {h.ToUpper()}");
            Console.WriteLine($"h to Lower: {h.ToLower()}");
            string k = h.Replace("H", "y");
            Console.WriteLine($"h to replace 'h'-> 'y': {k}");
            Console.WriteLine($"h to replace 'l'-> 'x': {h.Replace("l", "x")}");
            Console.WriteLine($"h length: {h.Length}");

            Console.WriteLine("");

            foreach (string j in i)
            {
                Console.Write(j + " ");
            }

            Console.WriteLine("");

            string w = String.Join(" ", i);
            Console.WriteLine(w);

            Console.WriteLine($"wold = { String.Join(" ", i) }");

            string[] addr = new string[20];  /* 不先給值的宣告，如要結值就用for輸進去，為法用add()新增 */
            /*addr.GetLowerBound 是取得陣列最小的index值 addr.GetUpperBound 是取得陣列最大的index值，即等同於 addr.length -1*/

            List<string> primes = new List<string>();  /* 別種宣告方法 可用primes.add()新增資料*/

            Console.WriteLine("");

            int[] m = { 10, 20, 30, 40, 50 };

            foreach(var n in m)
            {
                Console.WriteLine(n);
            }

            int r = m[2] >= 30 ? 5 : 20;  /*特別用法，?前是判斷式，如果true就是5, false就是20 */
            Console.WriteLine(r);

            int s = m[2] >= 50 ? 5 : 20;
            Console.WriteLine(s);

            Console.WriteLine("");

            string t;
            foreach(char j in h)  /*把一個字串每個拆開來，記得不是string而是char  理所當然也可以用h[0]取到'H'*/
            {
                t = Convert.ToInt32(j) >= 97 ? "小寫" : "大寫";  /*Convert.ToChar(Num);是數值轉char  Convert.ToInt32(); 是char轉數值*/
                Console.WriteLine(t);
            }

            Console.WriteLine("");

            foreach(var yy in m.Where(u => u >= 25))
            {
                Console.WriteLine(yy);
            }
            /*是陣列型態可以用where，來做篩選。類例sql 的where 上面的 u 就是 foreach陣列元素 ，他會把陣列元素都跑過一遍 */

            Console.WriteLine("");

            foreach (var yy in Data(m))
            {
                if(yy != 0) { Console.WriteLine(yy); }    
            }

            Console.WriteLine("");

            Boolean z = true;
            Console.WriteLine(z);
            Console.WriteLine(z ? false : true);  /* 可直接就在這邊運算了*/
            Console.WriteLine($"boolean to int: {Convert.ToInt32(z)}");
            Console.WriteLine($"boolean to string: {z.ToString()}");

            Console.WriteLine("");


            Console.Write("please input value:");
            string p = Console.ReadLine();

            Console.WriteLine(p);

            Console.Read();
        }

        /* 跟上方where做同樣的事，不過這次用函式來寫 */
        public static int[] Data(int[] x)
        {
            int[] ii = new int[x.Length -1];
            int count = 0;

            foreach(var d in x)
            {
                if(d >= 25) { ii[count] = d; count += 1; }
            }

            return ii;  /*因長度較篩選後的長，所以會有預設值0，上方顯示的時後要做if */
        }

    }
}
