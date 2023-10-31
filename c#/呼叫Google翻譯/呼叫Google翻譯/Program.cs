using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using 測試;

/*
    參考網址： https://blog.csdn.net/losedguest/article/details/110850777
 */

namespace 呼叫Google翻譯
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string testStr = "今天是個好天氣";
            string result = GoogleTrans(testStr, "zh-TW", "en");
            Console.WriteLine($"輸入的文字： {testStr}   翻譯的文字： {result}");

            testStr = "this is a big project";
            result = GoogleTrans("this is a big project", "en", "zh-TW");
            Console.WriteLine($"輸入的文字： {testStr}   翻譯的文字： {result}");

            Console.WriteLine("===========================以上是測試文字===========================");

            string exitFlag = "0";
            string action = string.Empty;
            while (action != exitFlag)
            {
                Console.Write("請輸入執行代碼(0=結束；1=中翻英；2=英翻中)：");
                action = Console.ReadLine();

                if (action != exitFlag)
                {
                    Console.WriteLine("請輸入要翻譯的字串：");
                    string input = Console.ReadLine();
                    if (action == "2")
                    {
                        result = GoogleTrans(input, "en", "zh-TW");
                    }
                    else
                    {
                        result = GoogleTrans(input, "zh-TW", "en");
                    }
                    Console.WriteLine(result);
                }
            }
        }

        /// <summary>
        /// Google翻譯
        /// </summary>
        /// <param name="source">要翻譯的文字</param>
        /// <param name="formLan">輸入語系(zh-TW：繁中 / en：英文)</param>
        /// <param name="toLan">輸出語系(zh-TW：繁中 / en：英文)</param>
        /// <returns></returns>
        public static string GoogleTrans(string source, string formLan, string toLan)
        {
            string result = string.Empty;

            string googleUrl = "https://translate.google.com.tw/m?sl=auto&tl={toLan}&hl={formLan}&q={source}";
            string url = googleUrl.Replace("{source}", source).Replace("{toLan}", toLan).Replace("{formLan}", formLan);
            string refer = "https://translate.google.com/";

            // 用結果去切
            string startKey = "***^v^***";
            string endKey = "*****";
            string html = GetResultHtml(url, refer);
            html = html.Replace("\"result-container\">", startKey).Replace("</div>", endKey);

            // 先抓出開頭
            List<string> slist = html.Split(startKey).ToList();
            string fHtml = slist.Count > 1 ? slist[1] : slist[0];

            // 抓出結尾前
            List<string> slist2 = fHtml.Split(endKey).ToList();
            result = slist2[0];

            return result;
        }

        /// <summary>
        /// 呼叫 網址 取得資料
        /// </summary>
        /// <param name="url"></param>
        /// <param name="refer"></param>
        /// <returns></returns>
        public static string GetResultHtml(string url, string refer)
        {
            CookieContainer cc = new CookieContainer();
            string html = "";

            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = "GET";
            webRequest.CookieContainer = cc;
            webRequest.Referer = refer;
            webRequest.Timeout = 20000;
            webRequest.Headers.Add("X-Requested-With:XMLHttpRequest");
            webRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            webRequest.UserAgent = " Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.0)";

            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                using (var reader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
                {

                    html = reader.ReadToEnd();
                }
            }

            return html;
        }
    }
}
