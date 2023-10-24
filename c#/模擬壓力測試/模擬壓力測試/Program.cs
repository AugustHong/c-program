using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

/*
    參考網址： https://blog.51cto.com/u_3044148/3351380
    基本上就是 使用呼叫 網址 的方式 + 多執行緖去打 的結果
 */

namespace 模擬壓力測試
{
    public class RunResult
    {
        // Y = 成功； N = 失敗
        public string succeed { get; set; }

        // 開始執行時間
        public DateTime startTime { get; set; }

        // 結束執行時間
        public DateTime endTime { get; set; }

        // 執行時間多久 (秒)
        public int timeDiffSeconds { get; set; }
    }

    class Program
    {
        public static List<RunResult> resultList = new List<RunResult>();

        static void Main(string[] args)
        {
            
            int connectionCount = 100; // 設定要跑幾次
            string url = "https://blog.51cto.com/u_3044148/3351380"; //要跑的網址
            string method = "GET";  //Request Method  (GET、POST…等)
            string param = "";  //  name=ABC&code=124  這種的
            int timeout = 120 * 1000; // 毫秒(1000 = 1秒)

            var requestThread = new Thread(() => StartRequest(connectionCount, url, method, param, timeout)) { IsBackground = true };
            requestThread.Start();

            Console.WriteLine("壓力測試執行完成！");
            Console.ReadLine();
        }

        /// <summary>
        /// 開始執行
        /// </summary>
        /// <param name="connectionCount">跑的次數</param>
        public static void StartRequest(int connectionCount, string url, string method, string param, int timeout)
        {
            for (var i = 1; i <= connectionCount; i++)
            {
                ThreadPool.QueueUserWorkItem(u => SendRequest(url, method, param, timeout));
            }
        }

        /// <summary>
        /// 執行動作
        /// </summary>
        public static void SendRequest(string url, string method, string param, int timeout)
        {
            DateTime startTime = DateTime.Now;

            try
            {
                string responseContent = string.Empty;

                // 先整理 參數
                if (method == "GET" && !string.IsNullOrEmpty(param))
                {
                    url += "?" + param;
                }

                // 呼叫準備
                HttpWebRequest req = (HttpWebRequest)HttpWebRequest.Create(url);
                req.Method = method;
                req.Timeout = timeout;
                req.KeepAlive = true;

                if (method != "GET")
                {
                    byte[] bs = Encoding.ASCII.GetBytes(param);
                    req.ContentType = "application/x-www-form-urlencoded";
                    req.ContentLength = bs.Length;
                    using (Stream reqStream = req.GetRequestStream())
                    {
                        reqStream.Write(bs, 0, bs.Length);
                    }
                }

                // 取得結果
                HttpWebResponse response = (HttpWebResponse)req.GetResponse();
                using (Stream resStream = response.GetResponseStream())
                {
                    using (StreamReader reader = new StreamReader(resStream, Encoding.UTF8))
                    {
                        responseContent = reader.ReadToEnd().ToString();
                    }
                }

                CountResult(startTime, DateTime.Now, "Y");
            }
            catch (Exception)
            {
                CountResult(startTime, DateTime.Now, "N");
            }
        }

        /// <summary>
        /// 計算結果
        /// </summary>
        /// <param name="seq"></param>
        /// <param name="startTIme"></param>
        /// <param name="endTime"></param>
        public static void CountResult(DateTime startTIme, DateTime endTime, string succeed)
        {
            int diffSecond = (int)(endTime.Subtract(startTIme).TotalSeconds);
            resultList.Add(new RunResult
            {
                startTime = startTIme,
                endTime = endTime,
                succeed = succeed,
                timeDiffSeconds = diffSecond
            });

            int seq = (resultList.Count);

            Console.WriteLine($"序號 = {seq} 執行結果 = {succeed} 執行秒數 = {diffSecond} 秒");
        }

    }
}
