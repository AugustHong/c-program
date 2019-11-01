using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;  // 先引用這個

/*
    用基本的 andom rand = new Random(); 弱掃不會過，所以要用這種的
     */

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("取到 0 或 1 ： ");
            for (int x = 1; x <= 3; x++)
            {
                Console.WriteLine(Random());
            }

            Console.WriteLine(" 取到小數點， 位數 從 1 - 10 ：");
            for (int x = 1; x <= 10; x++)
            {
                Console.WriteLine(RandomFloat(x));
            }

            Console.WriteLine(" 取到 3 -5 的數");
            for (int x = 1; x <= 3; x++)
            {
                Console.WriteLine(Random(3, 6));
            }



            Console.Read();
        }

        /// <summary>
        ///  亂數 取到 0 或 1
        /// </summary>
        /// <returns></returns>
        public static int Random()
        {
            byte[] randomNumber = new byte[1];

            RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();

            // 因為 只有 1 byte => 會得到 0 - 255 號
            Gen.GetBytes(randomNumber);

            int rand = Convert.ToInt32(randomNumber[0]);

            // 只會得到 0 或 1
            return rand % 2;
        }

		/// <summary>
		///  亂數 取到 0 到 最大的數 - 1
		/// </summary>
		/// <param name="max">最大的數</param>
		/// <returns></returns>
		public static int Random(int max)
		{
			max = max <= 0 ? 1 : max;
			byte[] randomNumber = new byte[1];

			RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();

			// 因為 只有 1 byte => 會得到 0 - 255 號
			Gen.GetBytes(randomNumber);

			int rand = Convert.ToInt32(randomNumber[0]);

			// 只會得到 0 或 1
			return rand % max;
		}

		/// <summary>
		///  得到幾位數的小數(0 - 1 之間)
		/// </summary>
		/// <param name="round">小數是幾位數</param>
		/// <returns></returns>
		public static decimal RandomFloat(int round)
        {
            byte[] randomNumber = new byte[round];

            RNGCryptoServiceProvider Gen = new RNGCryptoServiceProvider();

            // 因為 只有 1 byte => 會得到 0 - 255 號
            Gen.GetBytes(randomNumber);

            // 跑迴圈去把他組成 字串
            string resultText = string.Empty;
            foreach (var number in randomNumber)
            {
                resultText += Convert.ToInt32(number).ToString();

                if (resultText.Length >= round)
                {
                    resultText = resultText.Substring(0, round);
                    break;
                }
            }

            return Convert.ToDecimal("0." + resultText);
        }

        /// <summary>
        ///  取到 start 至 end - 1 的數
        /// </summary>
        /// <param name="start">開始位數</param>
        /// <param name="end">結束位數</param>
        /// <returns></returns>
        public static int Random(int start, int end)
        {
            if (start > end)
            {
                int tmp = start;
                start = end;
                end = tmp;
            }

            return Convert.ToInt32(RandomFloat(5) * (end - start) + start);
        }
    }
}
