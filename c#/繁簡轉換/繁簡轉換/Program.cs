using Microsoft.International.Converters.TraditionalChineseToSimplifiedConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址： https://hackmd.io/@SuFrank/SyPonjQA5
    有2種方法，第2種要去 NuGet 裝上 CHTCHSConv
 */

namespace 繁簡轉換
{
    internal class Program
    {
        #region 第一種方法

        [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern int LCMapString(int locale, int dwMapFlags, string lpSrcStr, int cchSrc, string lpDestStr, int cchDest);

        /// <summary>
        /// 繁轉簡 or 簡轉繁
        /// </summary>
        /// <param name="source">文字</param>
        /// <param name="direction">(S = 繁轉簡 / T = 簡轉繁 ) 預設 繁轉簡</param>
        /// <returns></returns>
        public static string ToSimplifiedOrTraditional(string source, string direction = "S")
        {
            // 固定常數
            int LocaleSystemDefault = 0x0800;
            int LcmapSimplifiedChinese = 0x02000000;
            int LcmapTraditionalChinese = 0x04000000;

            // 要傳入轉換的數 (預設轉簡體)
            int directionChinese = LcmapSimplifiedChinese;

            // 轉繁體
            if (direction == "T") {
                directionChinese = LcmapTraditionalChinese;
            }

            // 實作轉換
            string t = new String(' ', source.Length);
            LCMapString(LocaleSystemDefault, directionChinese, source, source.Length, t, source.Length);
            return t;
        }


        #endregion

        static void Main(string[] args)
        {
            string testStr = "繁體字過國說這";

            // 第一種方法
            string a = ToSimplifiedOrTraditional(testStr);
            string b = ToSimplifiedOrTraditional(a, "T");
            Console.WriteLine(a);
            Console.WriteLine(b);

            // 第二種方法
            var c = ChineseConverter.Convert(testStr, ChineseConversionDirection.TraditionalToSimplified);
            var d = ChineseConverter.Convert(c, ChineseConversionDirection.SimplifiedToTraditional);

            Console.Read();
        }
    }
}
