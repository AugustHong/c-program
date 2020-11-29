using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 一個大於2的偶數為2個質數相加
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
                題目： 輸入一 > 2 的偶數，給出一組(因可以有多組) 2個質數相加
                        例： 14 = 7 + 7 or 3 + 11
                             24 = 11 + 13
             */

            // 我在這邊沒管效能(因為我數字底子不夠 T_T ，所以只能用很爛的方法)

            Console.WriteLine("請輸入一個 > 2 的偶數，這邊就不做驗證了 = =");
            double max = Convert.ToDouble(Console.ReadLine());

            Stopwatch watch = new Stopwatch();
            watch.Start();

            // 主程式
            MainAction(max);

            watch.Stop();
            Console.WriteLine($"共花了 {watch.Elapsed.TotalSeconds} 秒");

            Console.WriteLine("=========================================================");
            Console.WriteLine("結束程式");
            Console.ReadLine();
        }

        /// <summary>
        ///  主程式
        /// </summary>
        static void MainAction(double max)
        {
            // 先除一半，如果他的一半是 質數 => 就不用那麼麻煩了
            double half = max / 2;
            if (IsPrimeNumber(half))
            {
                Console.WriteLine($"{max} = {half} + {half}");
            }
            else
            {
                // 得到 小於 max / 2 的 質數 陣列
                List<double> list = new List<double>();
                FindPrimeNumber(list, (half - 1));

                double first = 0;
                double last = 0;

                // 開始往 小的數字找
                for (var i = list.Count() - 1; i >= 0; i--)
                {
                    first = list[i];
                    last = max - first;

                    // 判斷是否是質數
                    if (IsPrimeNumber(last))
                    {
                        break;
                    }
                }

                Console.WriteLine($"{max} = {first} + {last}");
            }
        }

        /// <summary>
        ///  找出 < 某個數的質列列表
        /// </summary>
        /// <param name="result"></param>
        /// <param name="max"></param>
        static void FindPrimeNumber(List<double> result, double max)
        {
            result.AddRange(new List<double> { 2, 3 });
            for (var i = 5; i <= max; i += 2)
            {
                if (IsPrimeNumber(i))
                {
                    result.Add(i);
                }
            }
        }

        /// <summary>
        ///  最笨的判斷 是否是質數
        ///  就是跑過底下 去看是可以整除
        /// </summary>
        /// <param name="num"></param>
        /// <returns></returns>
        static bool IsPrimeNumber(double num)
        {
            /*
境界1
　　在试除法中，最最土的做法，就是：
　　假设要判断 x 是否为质数，就从 2 一直尝试到 x-1。这种做法，其效率应该是最差的。如果这道题目有10分，
按照这种方式做出的代码，即便正确无误，俺也只给1分。

◇境界2
　　稍微聪明一点点的程序猿，会想：x 如果有（除了自身以外的）质因数，那肯定会小于等于 x/2，所以捏，
他们就从 2 一直尝试到 x/2 即可。
　　这一下子就少了一半的工作量哦，但依然是很笨的办法。打分的话，即便代码正确也只有2分

◇境界3
　　再稍微聪明一点的程序猿，会想了：除了2以外，所有可能的质因数都是奇数。所以，他们就先尝试 2，
然后再尝试从 3 开始一直到 x/2 的所有奇数。
　　这一下子，工作量又少了一半哦。但是，俺不得不说，依然很土。就算代码完全正确也只能得3分。

◇境界4
　　比前3种程序猿更聪明的，就会发现：其实只要从 2 一直尝试到√x，就可以了。估计有些网友想不通了，为什么只要到√x 即可？
　　简单解释一下：因数都是成对出现的。比如，100的因数有：1和100，2和50，4和25，5和20，10和10。
看出来没有？成对的因数，其中一个必然小于等于100的开平方，另一个大于等于100的开平方。至于严密的数学证明，
用小学数学知识就可以搞定，俺就不啰嗦了。

◇境界5
　　那么，如果先尝试2，然后再针对 3 到√x 的所有奇数进行试除，是不是就足够优了捏？答案显然是否定的嘛？写到这里，才刚开始热身哦。
　　一些更加聪明的程序猿，会发现一个问题：尝试从 3 到√x 的所有奇数，还是有些浪费。
比如要判断101是否质数，101的根号取整后是10，那么，按照境界4，需要尝试的奇数分别是：3，5，7，9。
但是你发现没有，对9的尝试是多余的。不能被3整除，必然不能被9整除……顺着这个思路走下去，这些程序猿就会发现：
其实，只要尝试小于√x 的质数即可。而这些质数，恰好前面已经算出来了（是不是觉得很妙？）。

　　所以，处于这种境界的程序猿，会把已经算出的质数，先保存起来，然后用于后续的试除，效率就大大提高了。
　　顺便说一下，这就是算法理论中经常提到的：以空间换时间。

◇补充说明
　　开头的4种境界，基本上是依次递进的。不过，境界5跟境界4，是平级的。在俺考察过的应聘者中，
有人想到了境界4但没有想到境界5；反之，也有人想到境界5但没想到境界4。通常，这两种境界只要能想到其中之一，
俺会给5-7分；如果两种都想到了，俺会给8-10分。
　　对于俺要招的”初级软件工程师”的岗位，能同时想到境界4和境界5，应该就可以了。
*/
            bool isPrimeNumber = true;

            if (num % 2 == 0) { return false; }

            double square = Math.Pow(num, 0.5);
            for (var i = 3; i <= square; i+= 2)
            {
                if (num % i == 0)
                {
                    isPrimeNumber = false;
                    break;
                }
            }

            return isPrimeNumber;
        }
    }
}
