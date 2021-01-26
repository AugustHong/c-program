using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Hong.DatetimeHelper
{
    public static class DatetimeHelper
    {
        /// <summary>
        ///  驗證 是否 結束 > 開始 
        ///  PS： 如果有 null 的會自動帶成預設
        /// </summary>
        /// <param name="StartDatetime"></param>
        /// <param name="EndDateTime"></param>
        /// <returns></returns>
        public static (DateTime, DateTime) VaildDateTimeRange(DateTime? StartDatetime, DateTime? EndDateTime)
        {
            DateTime s = new DateTime(1970, 1, 1);
            DateTime e = new DateTime(2999, 12, 31);

            if (StartDatetime != null) { s = (DateTime)StartDatetime; }
            if (EndDateTime != null) { e = (DateTime)EndDateTime; }

            if (s > e)
            {
                DateTime tmp = s;
                s = e;
                e = tmp;
            }

            return (s, e);
        }

        /// <summary>
        ///  2 DateTime 相差 (A - B)
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <param name="diffType">差異類型(D=天；H=小時；M=分鐘；S=秒；MS=毫秒)，預設 小時</param>
        /// <returns></returns>
        public static double DateTimeDiff(this DateTime A, DateTime B, string diffType)
        {
            TimeSpan diff = A - B;

            // 要四捨五入至小數點第8位
            diffType = string.IsNullOrWhiteSpace(diffType) ? string.Empty : diffType.ToUpper();
            double result = 0;
            switch (diffType)
            {
                case "D":
                    result = diff.TotalDays;
                    break;
                case "H":
                    result = diff.TotalHours;
                    break;
                case "M":
                    result = diff.TotalMinutes;
                    break;
                case "S":
                    result = diff.TotalSeconds;
                    break;
                case "MS":
                    result = diff.TotalMilliseconds;
                    break;
                default:
                    result = diff.TotalHours;
                    break;
            }

            return Math.Round(result, 8);
        }

        /// <summary>
        ///  這日期 是星期幾 (1 = 星期一； 7=星期日)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static (int, string) DateOfWeek(this DateTime source)
        {
            int result = (int)source.DayOfWeek;
            result = result == 0 ? 7 : result;  // 星期日 原先會回傳是 0

            string s = string.Empty;
            switch (result)
            {
                case 1: s = "一"; break;
                case 2: s = "二"; break;
                case 3: s = "三"; break;
                case 4: s = "四"; break;
                case 5: s = "五"; break;
                case 6: s = "六"; break;
                case 7: s = "日"; break;
                default:
                    break;
            }

            return (result, s);
        }

        /// <summary>
        ///  算 幾年幾月 共有幾天
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int DateInMonth(this DateTime source)
        {
            // 平常的用法就是這樣
            return DateTime.DaysInMonth(source.Year, source.Month);
        }

        /// <summary>
        /// 得到 當年月 之 起迄日期
        /// </summary>
        /// <param name="yyyy"></param>
        /// <param name="mm"></param>
        /// <returns></returns>
        public static (DateTime, DateTime) MonthStartAndEnd(int yyyy, int mm)
        {
            DateTime startDatetime = new DateTime(yyyy, mm, 1);
            DateTime endDatetime = new DateTime(yyyy, mm, 28);  // 到時候要取代掉

            // 得到當年月 最後一天
            int dateInMonth = endDatetime.DateInMonth();
            endDatetime = ($"{yyyy}-{mm}-{dateInMonth}").ToDateTime();

            return (startDatetime, endDatetime);
        }

        /// <summary>
        ///  轉成 日期
        /// </summary>
        /// <param name="source">yyyy/MM/dd or yyyy-MM-dd</param>
        /// <returns></returns>
        public static DateTime ToDateTime(this string source)
        {
            // 字串錯直接讓他爆錯(所以不判斷)
            return Convert.ToDateTime(source);
        }

        /// <summary>
        ///  民國 轉成 日期
        /// </summary>
        /// <param name="source">eee/MM/dd or eee-MM-dd</param>
        /// <returns></returns>
        public static DateTime ROCtoDateTime(this string source)
        {
            char splitChar = ' ';
            string splitStr = string.Empty;
            if (source.Contains("/"))
            {
                splitChar = '/';
                splitStr = "/";
            }
            else
            {
                if (source.Contains("-"))
                {
                    splitChar = '-';
                    splitStr = "-";
                }
            }

            List<string> tmp = source.Split(splitChar).ToList();

            // 格式錯直接讓他爆錯 (所以不判斷)
            int eee = Convert.ToInt32(tmp[0]);
            int yyyy = eee + 1911;

            return ($"{yyyy}{splitStr}{tmp[1]}{splitStr}{tmp[2]}").ToDateTime();
        }

        /// <summary>
        /// 是否為日期格式
        /// </summary>
        /// <param name="source">yyyyMMdd</param>
        /// <returns></returns>
        public static bool IsDate(string source)
        {
            bool result = false;

            try
            {
                if (string.IsNullOrEmpty(source)) { return false; }
                result = Regex.IsMatch(source, @"^[\0-9]{4}[/0-1]{1}[0-9]{1}[0-3]{1}[0-9]{1}");
                if (result)
                {
                    DateTime.Parse(source.Insert(4, "-").Insert(7, "-"));
                }
                return result;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " --> " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 是否為 民國日期格式
        /// </summary>
        /// <param name="source">eeeMMdd</param>
        /// <returns></returns>
        public static bool IsROCDate(string source)
        {
            bool result = false;

            try
            {
                if (string.IsNullOrEmpty(source)) { return false; }

                // 取前3碼 相加 (有 try ，所以如果不符合數字格式自然會錯)
                int yyyy = Convert.ToInt32(source.Substring(0, 3)) + 1911;
                source = yyyy.ToString() + source.Substring(3);

                result = Regex.IsMatch(source, @"^[\0-9]{4}[/0-1]{1}[0-9]{1}[0-3]{1}[0-9]{1}");
                if (result)
                {
                    DateTime.Parse(source.Insert(4, "-").Insert(7, "-"));
                }
                return result;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " --> " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 是否為時間格式
        /// </summary>
        /// <param name="source">yyyyMMddHHmmss</param>
        /// <returns></returns>
        public static bool IsDateTime(string source)
        {
            bool result = false;

            try
            {
                if (string.IsNullOrEmpty(source)) { return false; }
                result = Regex.IsMatch(source, @"^[\0-9]{4}[/0-1]{1}[0-9]{1}[0-3]{1}[0-9]{1}[0-2]{1}[0-9]{1}[0-5]{1}[0-9]{1}[0-5]{1}[0-9]{1}");
                if (result)
                {
                    DateTime.Parse(source.Insert(4, "-").Insert(7, "-").Insert(10, " ").Insert(13, ":").Insert(16, ":"));
                }
                return result;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " --> " + ex.Message);
                return false;
            }
        }

        /// <summary>
        ///  是否為 民國年 的時間格式
        /// </summary>
        /// <param name="source">eeeMMddHHmmss</param>
        /// <returns></returns>
        public static bool IsROCDateTime(string source)
        {
            bool result = false;

            try
            {
                if (string.IsNullOrEmpty(source)) { return false; }

                // 取前3碼 相加 (有 try ，所以如果不符合數字格式自然會錯)
                int yyyy = Convert.ToInt32(source.Substring(0, 3)) + 1911;
                source = yyyy.ToString() + source.Substring(3);

                result = Regex.IsMatch(source, @"^[\0-9]{4}[/0-1]{1}[0-9]{1}[0-3]{1}[0-9]{1}[0-2]{1}[0-9]{1}[0-5]{1}[0-9]{1}[0-5]{1}[0-9]{1}");
                if (result)
                {
                    DateTime.Parse(source.Insert(4, "-").Insert(7, "-").Insert(10, " ").Insert(13, ":").Insert(16, ":"));
                }
                return result;
            }
            catch (System.Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " --> " + ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 是否 為 時間格式
        /// </summary>
        /// <param name="source">HHmmss</param>
        /// <returns></returns>
        public static bool IsTime(string source)
        {
            return IsDateTime("20200101" + source);
        }
    }
}
