using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2日期日數差_不用現有函式_
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
                題目： 計算 2日期 天數差 (不用現有的函式)
                例： 2002/10/1 和 2002/10/2 差 1 天
             */
            Console.WriteLine(DateDiff(new DateTime(2002, 10, 1), new DateTime(2002, 10, 1)));
            Console.WriteLine(DateDiff(new DateTime(2002, 10, 1), new DateTime(2002, 10, 2)));
            Console.WriteLine(DateDiff(new DateTime(2002, 10, 31), new DateTime(2002, 11, 1)));
            Console.WriteLine(DateDiff(new DateTime(2002, 10, 1), new DateTime(2003, 10, 1)));

            Console.Read();
        }

        /// <summary>
        ///  計算 2 日期差
        /// </summary>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        static int DateDiff(DateTime start, DateTime end)
        {
            // 去掉 Time 的部份
            string startDate = start.ToString("yyyyMMdd");
            string endDate = end.ToString("yyyyMMdd");

            // 先確定 start <= end
            if (start.CompareTo(end) > 0)
            {
                string tmp = startDate;
                startDate = endDate;
                endDate = tmp;
            }

            // 切出值
            int sYYYY = Convert.ToInt32(startDate.Substring(0, 4));
            int sMM = Convert.ToInt32(startDate.Substring(4, 2));
            int sDD = Convert.ToInt32(startDate.Substring(6, 2));

            int eYYYY = Convert.ToInt32(endDate.Substring(0, 4));
            int eMM = Convert.ToInt32(endDate.Substring(4, 2));
            int eDD = Convert.ToInt32(endDate.Substring(6, 2));

            // 如果 年月 全相同 (直接用 日期相減)
            if ((sYYYY == eYYYY) && (sMM == eMM))
            {
                return eDD - sDD;
            }
            else
            {
                // 先計算 當月 有幾天 - 開始日期 + 1 (要加1 是因為 10/31 日的話 10月有31 天 ， 31-31 = 0 )，但其實是1天
                int header = GetDayNumByMonth(sYYYY, sMM) - sDD + 1;

                // 結束日期天數 (要 減1喔)
                int footer = eDD - 1;

                // 中間值
                int body = 0;

                // 因為 算完本月了 要加1
                sMM++;
                if (sMM == 13)
                {
                    sYYYY++;
                    sMM = 1;
                }

                // 跑過 直至 結束日當前年月
                bool f = true;   // 只有第一次 從 sMM 開始 ，其餘從 1 開始算
                for (var y = sYYYY; y <= eYYYY; y++)
                {
                    int beginM = f ? sMM : 1;
                    if (f) { f = false; }
                    for (var m = beginM; m < 13; m++)
                    {
                        if ($"{y}{m}" == $"{eYYYY}{eMM}") { break; }
                        body += GetDayNumByMonth(y, m);
                    }
                }

                // 回傳
                return header + body + footer;
            }
        }

        /// <summary>
        ///  計算 一個月有幾天
        /// </summary>
        /// <param name="year"></param>
        /// <param name="month"></param>
        /// <returns></returns>
        static int GetDayNumByMonth(int year, int month)
        {
            int result = 0;
            if (month >= 1 && month <= 12)
            {
                List<int> days = new List<int> { 0, 31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31 };
                result = days[month];

                // 如果是 閏年 且 是2月要多加1
                if (month == 2)
                {
                    if (IsLeapYear(year))
                    {
                        result++;
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///  是否是 閏年
        /// </summary>
        /// <param name="year"></param>
        /// <returns></returns>
        static bool IsLeapYear(int year)
        {
            if ((year % 4 == 0) && (year % 100 != 0))
            {
                return true;
            }
            else
            {
                if (year % 400 == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
