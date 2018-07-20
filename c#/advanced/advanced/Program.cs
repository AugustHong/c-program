using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advanced
{
    public enum Color { Red, Green, Blue }  //不用加;  和一定要放在class上方
    //沒給值自動會red=0 green=1 blue=2，且型態沒設定預設是int

    //自訂型態
    public enum Range : long { Max = 2147483648L, Min = 255L };

    public enum CarOptions
    {
        // The flag for SunRoof is 0001.
        SunRoof = 0x01,
        // The flag for Spoiler is 0010.
        Spoiler = 0x02,
        // The flag for FogLights is 0100.
        FogLights = 0x04,
        // The flag for TintedWindows is 1000.
        TintedWindows = 0x08,
    }

    class Program
    {
        static void Main(string[] args){

            /* 下方是建立2維陣列，但是用此宣告的長度都要相同，以下3個陣列的長度都是3*/
            string[,] a = { { "hello", "are", "you" }, {"this", "is", "book"}, {"very", "so", "much" } };
            /* string[, ,] b 這個是3維陣列 */

            /* 正常會用以下方法來做多維陣列*/
            int[,] array = new int[4, 2];   /* 此代表int[4,2] */
            int[,] array2D = new int[,] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 } };
            int[,] array2Da = new int[4, 2] { { 1, 2 }, { 3, 4 }, { 5, 6 }, { 7, 8 } };
            int[,,] array3D = new int[,,] { { { 1, 2, 3 }, { 4, 5, 6 } },{ { 7, 8, 9 }, { 10, 11, 12 } } };

            foreach(var a1 in a)  /* foreach是把每一個都跑過，所以不用2次foreach來做 */
            {
                Console.Write(a1 + " ");
            }

            Console.WriteLine("");

            /* 這是平常的for => 要用2層 */
            for(int i = 0; i <= a.GetUpperBound(0); i++)  /* getupperbound(0)的0是指 3  因為 a[3,3]；如果今天是a[4,2]，那getupperbound(0)得到的值就是4*/
            {
                for(int j = 0; j <= a.GetUpperBound(1); j++) { Console.Write(a[i, j] + " " ); }
            }

            Console.WriteLine("");

            string b = "this is a book and how are you doing";
            string[] words = b.Split(' ');  /* split裡面的參數是char型態，所以不能用"" 要用 ' '來括 */
            foreach(var word in words) { Console.Write(word + " "); }

            Console.WriteLine("");

            /*比較好閱讀的寫法 */
            Console.Write("this is {0} and {1}", array2D[0, 0], array2D[3, 1]);

            Console.WriteLine("\n");

            /* 日期 */
            DateTime date1 = new DateTime(2008, 6, 1, 7, 47, 0);
            DateTime dateOnly = date1.Date; //只有日期沒有時間

            Console.WriteLine(date1.ToString());
            Console.WriteLine(date1);  //可以不用加ToString()和上面結果相同   
            Console.WriteLine(dateOnly.ToString("d"));
            Console.WriteLine(dateOnly.ToString("g")); //用24小時的時鐘表示法
            Console.WriteLine(dateOnly.ToString("MM/dd/yyyy HH:mm"));
            Console.WriteLine(date1.ToString("yyyy-MM-dd HH:mm"));

            Console.WriteLine("");

            //string轉日期
            string[] dateStrings = {"2008-05-01T07:34:42-5:00", "2008-05-01 7:34:42Z", "Thu, 01 May 2008 07:34:42 GMT"};
            foreach (string dateString in dateStrings)
            {
                DateTime convertedDate = DateTime.Parse(dateString);
                Console.WriteLine(convertedDate);
            }

            Console.WriteLine("");

            //日期轉時間戳
            DateTime DateStart = new DateTime(1970, 1, 1, 8, 0, 0);
            long timestamp = Convert.ToInt32((DateTime.Now - DateStart).TotalSeconds);
            Console.WriteLine(timestamp);

            //時間戳轉日期
            DateTime dtStart = TimeZone.CurrentTimeZone.ToLocalTime(new DateTime(1970, 1, 1));
            long lTime = long.Parse(timestamp + "0000000");
            TimeSpan toNow = new TimeSpan(lTime);
            Console.WriteLine(dtStart.Add(toNow));


            Console.WriteLine("");


            //switch
            switch (array2D[0,0])
            {
                case 1:
                    Console.WriteLine("Case 1");
                    break;
                case 2:
                    Console.WriteLine("Case 2");
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }

            switch (array2D[3, 0])
            {
                case 1:
                    Console.WriteLine("Case 1");
                    break;
                case 2:
                    Console.WriteLine("Case 2");
                    break;
                default:
                    Console.WriteLine("Default case");
                    break;
            }

            Console.WriteLine("");

            //enum用法（列舉）
            Color c = (Color)(new Random()).Next(0, 3);   //enum型別要轉換（從數字轉成enum => int->enum）
                                                          //亂數0-2之間的數
            switch (c)
            {
                case Color.Red:
                    Console.WriteLine("The color is red");
                    break;
                case Color.Green:
                    Console.WriteLine("The color is green");
                    break;
                case Color.Blue:
                    Console.WriteLine("The color is blue");
                    break;
                default:
                    Console.WriteLine("The color is unknown.");
                    break;
            }

                                        
            long x = (long)Range.Max;  //要轉換型態，因為這次的設定是long型態（從enum轉成long）
            long y = (long)Range.Min;
            Console.WriteLine("Max = {0}", x);
            Console.WriteLine("Min = {0}", y);

            //Range.Max->enum型別  255L->long型別  （用gettype()來看型別）
            Console.WriteLine(Range.Max.GetType());
            Console.WriteLine(x.GetType());


            CarOptions options = CarOptions.SunRoof | CarOptions.FogLights;  // |是or運算
            Console.WriteLine(options); // The integer value of 0101 is 5.
            Console.WriteLine((int)options); // The integer value of 0101 is 5.

            Console.WriteLine("");

            //亂數
            Random rand = new Random();
            Random rand1 = new Random();

            int[] z = new int[101];
            for(int k = 0; k <= 100; k++) { z[k] = k; }

            Console.WriteLine(rand.Next(0, 101));  //0-100的整數
            Console.WriteLine(rand1.NextDouble()); //傳回0-1之間的浮點數
            Console.WriteLine(rand.Next(0, 101) + rand1.NextDouble());  //用加法 就可以完成0-101之間的浮點數了

            //如果有byte[]陣列，則可用 rand.NextByte() => 用隨機的byte數填byte陣列
            //ex:  byte[100] by;
            //     rand.NextBytes(by);

            Console.WriteLine("");


            int[] w = { 5, 7, 3, 6, 9, 40, 58, 88, 12 };  
            //類似basic中所提的where是個可以判斷的東西，但odrer是排序，所以打j=>j就是全部都拿，且排
            foreach(int r in w.OrderBy(j => j)) { Console.Write($"{r} "); }
            //用where或者order的結果如果想要變成變數，那型態要注意喔！看以下
            //用List<int> primes = new List<int>();來存結果喔  記得如果是string的話2邊的int要改成string

            Console.WriteLine("");
            Console.WriteLine("___________________________________________________________________________");

            //linq寫法（其實就和上方陣列的那種寫法差不多） numbers.where(j+1=>j);
            int[] numbers = { 5, 4, 3, 2, 19, 9 };
            var result = from score in numbers
                         select score + 1;

            var result2 = from score in numbers
                          where score <= 8
                          select score;

            //以下2個結果會是相同的
            foreach (var re in result){
                Console.WriteLine(re);
            }
            Console.WriteLine("__________________________________________________________________");

            foreach(var re in numbers.Where(j => true)) {  //因為後面要是  =>判斷式   ，所以要全選就給他true
                Console.WriteLine(re + 1);
            }

            Console.WriteLine("");

            foreach (var re in result2){
                Console.WriteLine(re);
            }


            Console.Read();
        }
    }
}
