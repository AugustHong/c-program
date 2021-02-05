using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 快速排序法_QuickSort_
{
    class Program
    {
        static void Main(string[] args)
        {
            List<int> data = new List<int> { 89, 34, 23, 78, 67, 100, 66, 29, 79, 55, 78, 88, 92, 96, 96, 23, 11, 0, 5, -3 };
            QuickSort(data, 0, data.Count - 1);

            List<string> output = data.Select(x => x.ToString()).ToList();
            Console.WriteLine(string.Join(", ", output));

            Console.ReadLine();
        }

        /// <summary>
        /// 快速排序法
        /// 先設1基準點
        /// 左邊找 比 基準 大的數
        /// 右邊找 比 基準 小的數
        /// 如果2個都找到 => 交換 => 直到 left = right
        /// 把當前的那個值 和 基準點交換 => 會切出 小於的一組 + 大於的一組
        /// 每組個再跑一次 quickSort 即可
        /// </summary>
        /// <param name="source"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        static void QuickSort(List<int> source, int left, int right)
        {
            // 如果左邊大於右邊，就跳出function
            if (left >= right) { return; }

            int i = left;  // 左邊 pointer
            int j = right;   // 右邊 pointer
            int key = source[left];  // 基準點

            // 直到 i = j
            while (i != j)
            {
                // 從右邊開始找，找比基準點小的值
                while ((source[j] > key) && (i < j))
                {
                    j--;
                }

                // 從左邊開始找，找比基準點大的值
                while ((source[i] <= key) && (i < j))
                {
                    i++;
                }

                // 當左右代理人沒有相遇時，互換值
                if (i < j)
                {
                    int tmp = source[i];
                    source[i] = source[j];
                    source[j] = tmp;
                }
            }

            // 將基準點 開遇到的這個點 互換值
            source[left] = source[i];
            source[i] = key;

            QuickSort(source, left, i - 1);   //繼續處理較小部分的子循環
            QuickSort(source, i + 1, right);  //繼續處理較大部分的子循環
        }
    }
}
