using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 給1數和1陣列求是否有其3個相加
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
                題目： 給1個 K 變數 (例如： K = 49) 和一個陣列 (一定 >=3 個數，且可 負數、0、正數)
                        例： [-1, -1, 80, 22, 22, 50, 0]
                     ，求 此陣列是否有 3個數相加 等於 K 
                    例如： 以此案 -1 + 0 + 50 = 49 = K 回傳 True
             */
            int K = 49;
            List<int> source = new List<int> { -1, -1, 80, 22, 22, 50, 0 };

            // Method1
            bool result = Method1(K, source);
            Console.WriteLine(result);

            Console.WriteLine("--------------------------------------------");

            // Method2
            bool result2 = Method2(K, source);
            Console.WriteLine(result2);

            Console.WriteLine("--------------------------------------------");

            int K2 = 33;
            Console.WriteLine(Method1(K2, source));

            Console.WriteLine("--------------------------------------------");

            Console.WriteLine(Method2(K2, source));

            Console.Read();
        }

        /// <summary>
        /// 最簡單的做法
        /// </summary>
        /// <param name="K"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        static bool Method1(int K, List<int> source)
        {
            bool result = false;

            for (var i = 0; i < source.Count - 2; i++)
            {
                for (var j = (i + 1); j < source.Count - 1; j++)
                {
                    for (var k = (j + 1); k < source.Count; k++)
                    {
                        int v = source[i] + source[j] + source[k];
                        if (v == K)
                        {
                            Console.WriteLine($"{source[i]} + {source[j]} + {source[k]} = {K}");
                            result = true;
                            break;
                        }
                    }

                    if (result)
                    {
                        break;
                    }
                }

                if (result)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// 固定1位數，再用雙指標逼進
        /// </summary>
        /// <param name="K"></param>
        /// <param name="source"></param>
        /// <returns></returns>
        static bool Method2(int K, List<int> source)
        {
            bool result = false;

            if (source.Count >= 3)
            {
                // 先排序
                var sort = source.OrderBy(x => x).ToList();
                int len = sort.Count;

                for (var i = 0; i < len - 2; i++)
                {
                    // 指標
                    int start = (i + 1);
                    int end = len - 1;
                    int goal = (K - source[i]);

                    bool finish = false;

                    // 跑迴圈
                    while (!finish)
                    {
                        int iv = sort[start];
                        int jv = sort[end];

                        if (iv + jv < goal)
                        {
                            start++;
                        }
                        else
                        {
                            if (iv + jv > goal)
                            {
                                end--;
                            }
                            else
                            {
                                finish = true;
                                result = true;
                                Console.WriteLine($"{source[i]} + {iv} + {jv} = {K}");
                            }
                        }

                        if ((start == end) || (end < start))
                        {
                            finish = true;
                        }
                    }

                    if (result) { break; }
                }
            }

            return result;
        }
    }
}
