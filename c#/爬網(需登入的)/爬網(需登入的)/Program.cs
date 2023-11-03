using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace 爬網_需登入的_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            CookieContainer cc = new CookieContainer();  // 最主要的是這個，要記住Cookies
            string html = "";
            string html2 = "";

            string url = "https://xxxx/login";

            HttpWebRequest webRequest = WebRequest.Create(url) as HttpWebRequest;
            webRequest.Method = "POST";
            webRequest.CookieContainer = cc;
            webRequest.ContentType = "application/x-www-form-urlencoded";

            // 組出 Form 表單 參數資料 (詳細請見自己要爬的網站 F12)
            var postData =  "username=" + Uri.EscapeDataString("帳號");
            postData += "&password=" + Uri.EscapeDataString("密碼");
            var data = Encoding.ASCII.GetBytes(postData);

            webRequest.ContentLength = data.Length;

            // 傳入 Data
            using (var stream = webRequest.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }

            // 取回資料
            using (var webResponse = (HttpWebResponse)webRequest.GetResponse())
            {
                using (var reader = new StreamReader(webResponse.GetResponseStream(), Encoding.UTF8))
                {

                    html = reader.ReadToEnd();
                }

                // 在裡面再繼續前往下一頁 (有些是登入完導回頁後，要切去其他面爬資料_
                string url2 = "https://xxx/queryData";
                HttpWebRequest webRequest2 = WebRequest.Create(url2) as HttpWebRequest;
                webRequest2.Method = "GET";
                webRequest2.CookieContainer = cc;

                // 繼續往下取資料
                using (var webResponse2 = (HttpWebResponse)webRequest2.GetResponse())
                {
                    using (var reader2 = new StreamReader(webResponse2.GetResponseStream(), Encoding.UTF8))
                    {

                        html2 = reader2.ReadToEnd();
                    }
                }

                // 可以再繼續取其他頁的資料
                string html3;
                string url3 = "https://xxx/queryData2";

                HttpWebRequest webRequest3 = WebRequest.Create(url3) as HttpWebRequest;
                webRequest3.Method = "GET";
                webRequest3.CookieContainer = cc;

                using (var webResponse3 = (HttpWebResponse)webRequest3.GetResponse())
                {
                    using (var reader3 = new StreamReader(webResponse3.GetResponseStream(), Encoding.UTF8))
                    {

                        html3 = reader3.ReadToEnd();
                    }
                }
            }

            Console.Read();
        }
    }
}
