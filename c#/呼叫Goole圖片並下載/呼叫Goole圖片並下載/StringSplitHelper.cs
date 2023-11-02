using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 呼叫Goole圖片並下載
{
    public static class StringSplitHelper
    {
        /// <summary>
        /// 可以參考 Hong.StringHelper (這是我之前寫的用 字串分割字串)
        /// </summary>
        /// <param name="source">要分被割的字串</param>
        /// <param name="splitStr">字來分割的字串</param>
        /// <returns></returns>
        public static List<string> Split(this string source, string splitStr)
        {
            List<string> result = new List<string>();

            string tmpSource = source;

            // 如果是 空的，就直接回傳 空字串
            if (string.IsNullOrEmpty(source))
            {
                return new List<string> { "" };
            }

            // 如果切割字串是 null 回傳整個
            if (splitStr == null)
            {
                result.Add(source);
                return result;
            }

            int len = tmpSource.Length;

            // 如果 切割字串是 空字串，就每個字母來切
            if (splitStr == string.Empty)
            {
                for (var i = 0; i < len; i++)
                {
                    string tmp = source.Substring(i, 1);
                    result.Add(tmp);
                }
                return result;
            }

            // 其餘照著切
            int splitStrLen = splitStr.Length;

            // 判斷是否有進去
            bool haveI = false;

            // 位置
            int pos = tmpSource.IndexOf(splitStr);

            // 直到結束
            while (pos >= 0)
            {
                haveI = true;
                string tmp = string.Empty;

                if (pos == 0)
                {
                    tmp = string.Empty;
                }
                else
                {
                    tmp = tmpSource.Substring(0, pos);
                }

                result.Add(tmp);

                // 算出要延後幾位
                int diff = pos + splitStrLen;

                // 切割 (讓剩下的繼續跑)
                tmpSource = tmpSource.Substring(diff);

                // 重算位置
                pos = tmpSource.IndexOf(splitStr);

                // 如果 最後一次砍完剩下 空字串 => 要 push 進去
                if (string.IsNullOrEmpty(tmpSource))
                {
                    result.Add(string.Empty);
                }
                else
                {
                    if (pos < 0)
                    {
                        result.Add(tmpSource);
                    }
                }
            }

            // 如果一開始就查不到 => 直接回傳 自己
            if (haveI == false)
            {
                result.Add(source);
            }

            return result;
        }
    }
}
