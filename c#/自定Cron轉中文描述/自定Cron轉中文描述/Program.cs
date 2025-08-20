using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 自定Cron轉中文描述
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
                Cron 規則： 基本5個值 (現在也有第6個值，第1個是秒，後面全部順延)
                0 0 0 0 0
                第1位： 分
                第2位： 時
                第3位： 日
                第4位： 月
                第5位： 週幾 
             */
            // * 代表 不限定。
            // */5 代表 每5。 放在分鐘 就是每5分鐘跑一次。 放小時 就是每5小時跑一次……以此類推
            // 1-5 代表 1到5。 放週次就是 星期一至五； 放日 就是 1號到5號……以此類推
            // 1,5 代表 1和5。 放週次就是 星期一和五； 放日 就是 1號和5號……以此類推
            // 例如： */5 * 10 6 *   => 代表 6月10號 每5分鐘跑一次
            CronTranslator c = new CronTranslator();
            List<string> inputList = new List<string>
            {
                "*/10 * * * *",
                "20 9 * * *",
                "5-10 20 * * *",
                "0 0 6-10 * *",
                "0 0 9 1,3,5 *",
                "10 20 15 * 1,5"
            };

            foreach (string input in inputList)
            {
                string output = c.ToChineseDescription(input);
                Console.WriteLine($"{input}   代表：  {output}");
            }

            Console.ReadLine();
        }
    }

    public class CronTranslator
    {
        /// <summary>
        /// 將 Cron轉成文字
        /// </summary>
        /// <param name="cron">Cron值</param>
        /// <param name="maxLen">最大幾碼位元</param>
        /// <param name="autoInsert">若不滿5個值，是否自動補值</param>
        /// <returns></returns>
        public string ToChineseDescription(string cron, int maxLen = 5, bool autoInsert = true)
        {
            string result = string.Empty;

            // 處理字串
            List<string> cronList = cron.Split(' ').ToList();

            // 若後面不到5個值，自動補 *
            if (autoInsert == true)
            {
                for (int i = cronList.Count + 1; i <= maxLen; i++)
                {
                    cronList.Add("*");
                }
            }

            // 超過 最大長度 才做事
            if (cronList.Count >= maxLen)
            {
                List<string> outputStrList = new List<string>();

                if (maxLen == 6)
                {
                    string second = cronList[0];
                    string minute = cronList[1];
                    string hour = cronList[2];
                    string day = cronList[3];
                    string month = cronList[4];
                    string weekday = cronList[5];

                    outputStrList.Add(TranslateWeekday(weekday));
                    outputStrList.Add(TranslateOther(month, "月"));
                    outputStrList.Add(TranslateOther(day, "日"));
                    outputStrList.Add(TranslateOther(hour, "時"));
                    outputStrList.Add(TranslateOther(minute, "分"));
                    outputStrList.Add(TranslateOther(second, "秒"));
                }
                else
                {
                    string minute = cronList[0];
                    string hour = cronList[1];
                    string day = cronList[2];
                    string month = cronList[3];
                    string weekday = cronList[4];

                    outputStrList.Add(TranslateWeekday(weekday));
                    outputStrList.Add(TranslateOther(month, "月"));
                    outputStrList.Add(TranslateOther(day, "日"));
                    outputStrList.Add(TranslateOther(hour, "時"));
                    outputStrList.Add(TranslateOther(minute, "分"));
                }

                List<string> rList = outputStrList.Where(x => !string.IsNullOrEmpty(x)).ToList();
                if (rList.Count > 0)
                {
                    result = string.Join(" ", rList);
                }
            }


            return result;
        }

        private string GetWeekdayStr(string weekday)
        {
            List<string> weekdayList = new List<string> { "", "週一", "週二", "週三", "週四", "週五", "週六", "週日" };
            string result = string.Empty;

            int.TryParse(weekday, out int tmp);
            if (tmp >= 1 && tmp <= 7)
            {
                result = weekdayList[tmp];
            }

            return result;
        }

        /// <summary>
        /// 轉換週
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        private string TranslateWeekday(string part)
        {
            string result = string.Empty;

            if (part == "*") { result = ""; }
            else
            {
                // 算是否有 , 
                List<string> tmp1 = part.Split(',').ToList();
                if (tmp1.Count > 1)
                {
                    foreach (var t in tmp1)
                    {
                        result += GetWeekdayStr(t) + "、";
                    }

                    result = result.TrimEnd('、');
                }
                else
                {
                    // 算是否有 - 
                    List<string> tmp2 = part.Split('-').ToList();
                    if (tmp2.Count > 1)
                    {
                        result = GetWeekdayStr(tmp2[0]) + "至" + GetWeekdayStr(tmp2[1]);
                    }
                    else
                    {
                        // 原值輸出
                        result = GetWeekdayStr(part);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// 轉換其他類型
        /// </summary>
        /// <param name="part"></param>
        /// <returns></returns>
        private string TranslateOther(string part, string desc)
        {
            string result = string.Empty;

            if (part == "*") { result = ""; }
            else
            {
                // 算是否有 , 
                List<string> tmp1 = part.Split(',').ToList();
                if (tmp1.Count > 1)
                {
                    foreach (var t in tmp1)
                    {
                        result += t.ToString().PadLeft(2, '0') + "、";
                    }

                    result = result.TrimEnd('、');
                    result += desc;
                }
                else
                {
                    // 算是否有 - 
                    List<string> tmp2 = part.Split('-').ToList();
                    if (tmp2.Count > 1)
                    {
                        result = tmp2[0].ToString().PadLeft(2, '0') + "至" + tmp2[1].ToString().PadLeft(2, '0') + desc;
                    }
                    else
                    {
                        // 算是否有 / 
                        List<string> tmp3 = part.Split('/').ToList();
                        if (tmp3.Count > 1)
                        {
                            result = "每" + tmp3[1] + desc;
                        }
                        else
                        {
                            // 原值輸出
                            result = part.PadLeft(2, '0') + desc;
                        }
                    }
                }
            }

            return result;
        }
    }
}
