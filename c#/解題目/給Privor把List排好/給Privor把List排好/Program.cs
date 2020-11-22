using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 給Privor把List排好
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
                題目： 給1個 privor x 變數， 和 1個 陣列 lst
                分為3個階段， < privor 、 = privor 、 > privor (不用排序)
                例： x = 10, lst = [9, 10, 10, 12, 14, 10, 3, 5]
                果： [9, 3, 5, 10, 10, 10, 12, 14]
                     => 9, 3, 5 是第一階段 (< privor，但不用排序)
                     => 10, 10, 10 是第二階段 (= privor)
                     => 12, 14 是第三階段 (> privor，但不用排序)
             */

            int privor = 10;
            //List<int> lst = new List<int> { 9, 10, 10, 12, 14, 10, 3, 5 };
            // count = 11
            MainAction(new List<int> { 9, 10, 10, 12, 14, 10, 3, 5 }, privor);

            // best case 全是 < privor
            //List<int> lst = new List<int> { 9, 8, 7, 3, 2, 1, 3, 5 };
            // count = 7
            MainAction(new List<int> { 9, 8, 7, 3, 2, 1, 3, 5 }, privor);

            // worse case 全是 >= privor
            //List<int> lst = new List<int> { 19, 10, 10, 12, 14, 10, 13, 15 };
            // count = 14
            MainAction(new List<int> { 19, 10, 10, 12, 14, 10, 13, 15 }, privor);

            // 邊界 case 
            //List<int> lst = new List<int> { 10, 11, 23, 43, 22, 11, 35, 9 };
            // count = 13
            MainAction(new List<int> { 10, 11, 23, 43, 22, 11, 35, 9 }, privor);

            // 邊界 case 
            //List<int> lst = new List<int> { 3, 6, 3, 5, 7, 9, 1, 10 };
            // count = 7
            MainAction(new List<int> { 3, 6, 3, 5, 7, 9, 1, 10 }, privor);

            // => 總結： n  ~ 2n (複雜度)
            Console.Read();

            // 註： 沒測過很多，可以在某個特定情況會錯
            // 註： 這個題目其實用 3 個陣列去存 更快 => 但為了增加難度，所以不這樣做
        }

        /// <summary>
        ///  主排序邏輯
        /// </summary>
        /// <param name="start"></param>
        /// <param name="lst"></param>
        /// <param name="privor"></param>
        /// <param name="count"></param>
        static void Sort(ref int start, List<int> lst, int privor, ref int count)
        {
            int end = lst.Count() - 1;
            int index = start;

            while (!(start == end))
            {
                int r = lst[index];
                count++;

                if (r < privor)
                {
                    // 往下跑
                    start++;
                    index++;
                }
                else
                {
                    // 交換
                    int ttttt = lst[index];
                    lst[index] = lst[end];
                    lst[end] = ttttt;

                    end--;
                }
            }
        }

        /// <summary>
        ///  主邏輯
        /// </summary>
        /// <param name="lst"></param>
        static void MainAction(List<int> lst, int privor)
        {
            int count = 0;

            string source = string.Join(", ", lst.Select(x => x.ToString()).ToList());

            // 第一次排序 (把 >= privor 的丟到後面)
            int start = 0;
            Sort(ref start, lst, privor, ref count);

            // 第二次排序 (把 >= privor 往後算， 先把 privor++ ，這樣 就會照著排序了)
            Sort(ref start, lst, (privor + 1), ref count);

            string result = string.Join(", ", lst.Select(x => x.ToString()).ToList());
            Console.WriteLine($"source = [{source}], result = [{result}], Count = {count}");
        }
    }
}
