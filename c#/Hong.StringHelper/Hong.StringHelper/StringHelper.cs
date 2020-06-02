using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Hong.StringHelper
{
    public static class StringHelper
    {
        #region  因為 Substring 都會爆錯 => 所以加上 try catch ，就不用再寫那麼多行了
        /// <summary>
        ///  同 原本的 Substring ， 但是有加上 try catch => 會回傳 空
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string SubString2(this string source, int startIndex, int length)
        {
            try
            {
                // 錯誤判斷，不要讓他 猛進到 try catch 脫慢速度
                if (string.IsNullOrEmpty(source)) { return string.Empty; }
                int len = source.Length;
                if (startIndex >= len) { return string.Empty; }
                if ((startIndex + length) > len) { return source.SubString2(startIndex); }

                source = string.IsNullOrEmpty(source) ? string.Empty : source;
                string result = source.Substring(startIndex, length);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " --> " + ex.Message);

                return source.SubString2(startIndex);
            }
        }

        /// <summary>
        ///  同 原本的 Substring ， 但是有加上 try catch => 會回傳 空
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static string SubString2(this string source, int startIndex)
        {
            try
            {
                // 錯誤判斷，不要讓他 猛進到 try catch 脫慢速度
                if (string.IsNullOrEmpty(source)) { return string.Empty; }
                if (startIndex >= source.Length) { return string.Empty; }

                source = string.IsNullOrEmpty(source) ? string.Empty : source;
                string result = source.Substring(startIndex);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " --> " + ex.Message);
                return string.Empty;
            }
        }

        #endregion

        /// <summary>
        ///  維持固定的長度回傳
        ///  isRightAddSpace = 如果不足長度是否是向右補空白 (預設是 true, 如果是 false 就是像左補空白)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        public static string GetFixedLength(this string source, int stringLen, bool isRightAddSpace = true)
        {
            // 長度
            stringLen = stringLen <= 0 ? 0 : stringLen;

            // String.Format("{0, -3}", ) => 不足3，向右補空白
            // String.Format("{0, 3}", ) => 不足3，向左補空白
            string addSpace = isRightAddSpace ? "-" : "";
            return string.Format("{0, " + addSpace + stringLen.ToString() + "}", source.SubString2(0, stringLen));
        }

        #region 字串比大小，寫的比較直覺

        /// <summary>
        ///  字串 source 是否 比 matchStr 小
        /// </summary>
        /// <param name="source"></param>
        /// <param name="matchStr"></param>
        /// <returns></returns>
        public static bool SmallTo(this string source, string matchStr)
        {
            /*
              string.Compare(A, B) => 會得出數字
              < 0 => A < B
              = 0 => A = B
              > 0 => A > B
             */
            return string.Compare(source, matchStr) < 0;
        }

        /// <summary>
        ///  字串 source 是否 比 matchStr 大
        /// </summary>
        /// <param name="source"></param>
        /// <param name="matchStr"></param>
        /// <returns></returns>
        public static bool LargeTo(this string source, string matchStr)
        {
            return string.Compare(source, matchStr) > 0;
        }

        /// <summary>
        ///  把 用 字串 分割字串 寫成精簡的 (沒測過，所有可能有小問題，但主要的都正常)
        /// </summary>
        /// <param name="source"></param>
        /// <param name="splitStr"></param>
        /// <returns></returns>
        public static string[] Split(this string source, string splitStr){
            if (string.IsNullOrEmpty(source)){return new string[];}
            splitStr = string.IsNullOrEmpty(splitStr) ? string.Empty : splitStr;
            return Regex.Split(source, splitStr, RegexOptions.IgnoreCase)
        }

        #endregion

        #region 相關 TryParse 平常要寫2行 => 把他精簡掉

        /// <summary>
        ///  把用 try parse 寫成2行的部份 轉成一行
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int IntTryParse(this char source)
        {
            int result = 0;

            try
            {
                result = Convert.ToInt32(source);
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return 0;
            }
        }

        /// <summary>
        ///  把用 try parse 寫成2行的部份 轉成一行
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static int IntTryParse(this string source)
        {
            int result = 0;
            int.TryParse(source, out result);
            return result;
        }

        /// <summary>
        ///  把用 try parse 寫成2行的部份 轉成一行
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static decimal DecimalTryParse(this string source)
        {
            decimal result = 0;
            decimal.TryParse(source, out result);
            return result;
        }

        /// <summary>
        ///  把用 try parse 寫成2行的部份 轉成一行
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static double DoubleTryParse(this string source)
        {
            double result = 0;
            double.TryParse(source, out result);
            return result;
        }

        /// <summary>
        ///  這個 字串 是不是 數字型 (能用下面的 驗證全數字就行)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsNumeric(string source)
        {
            if (string.IsNullOrEmpty(source)) { return false; }

            try
            {
                var i = Convert.ToDecimal(source);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " --> " + ex.Message);

                try
                {
                    var i = Convert.ToDouble(source);
                    return true;
                }
                catch (Exception e)
                {
                    System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " --> " + e.Message);

                    /*
                        原本在這邊有寫 ToInt32 => 但拿掉
                        原因： 連 Double 和 Decimal 都不行了 ，那 Int 絕對就不可能
                        不要再浪費時間 做 Convert (花的時間很久)
                     */

                    return false;
                }
            }
        }

        #endregion

        #region 相關驗證(是否全數字、是否全英文、是否全英數字、是否全是中文) => 用 StringHelper.xxx() 的方式來用

        /// <summary>
        ///  是否是全數字
        /// </summary>
        public static bool IsAllNumeric(string source)
        {
            bool result = false;

            try
            {
                if (string.IsNullOrEmpty(source)) { return false; }
                result = Regex.IsMatch(source, @"^[0-9]+$");
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " --> " + ex.Message);
                return false;
            }
        }

        /// <summary>
        ///  是否是全英文
        /// </summary>
        public static bool IsAllEnglish(string source)
        {
            bool result = false;

            try
            {
                if (string.IsNullOrEmpty(source)) { return false; }
                result = Regex.IsMatch(source, @"^[a-zA-Z]+$");
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " --> " + ex.Message);
                return false;
            }
        }

        /// <summary>
        ///  是否是全英文或數字
        /// </summary>
        public static bool IsAllEnglishOrNumeric(string source)
        {
            bool result = false;

            try
            {
                if (string.IsNullOrEmpty(source)) { return false; }
                result = Regex.IsMatch(source, @"^[a-zA-Z0-9]+$");
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " --> " + ex.Message);
                return false;
            }
        }

        /// <summary>
        ///  是否 包含 中文 (符號不算中文 => 用 ASCII > 126 來判斷)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsContainChinese(string source)
        {
            bool result = false;

            if (!string.IsNullOrEmpty(source))
            {
                foreach (var currentChar in source)
                {
                    // 這邊不管符號的 => 所以查 ASCII碼時發現 126 是底
                    // => 所以如果 ASCII > 126 就都視為中文
                    int currentCharNum = currentChar.IntTryParse();
                    if ((currentCharNum <= 0) || (currentCharNum > 126))
                    {
                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///  是否 全部 中文 (符號不算中文 => 用 ASCII > 126 來判斷)
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool IsAllChinese(string source)
        {
            bool result = true;

            if (!string.IsNullOrEmpty(source))
            {
                foreach (var currentChar in source)
                {
                    // 這邊不管符號的 => 所以查 ASCII碼時發現 126 是底
                    // => 所以如果 ASCII > 126 就都視為中文
                    int currentCharNum = currentChar.IntTryParse();
                    if ((currentCharNum > 0) && (currentCharNum <= 126))
                    {
                        result = false;
                    }
                }

                return result;
            }
            else
            {
                return false;
            }
        }



        #endregion
    }
}
