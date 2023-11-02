using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

/*
    想做到類似輸入文字，就產出圖片
 */

namespace 呼叫Goole圖片並下載
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string url = "https://www.google.com.tw/search?q={queryKey}&hl=zh-TW&tbm=isch&source=hp&biw=1280&bih=643&sclient=img";
            string queryKey = "";
            string html = "";

            Console.Write("請輸入要查詢的圖片篩選文字：");
            queryKey = Console.ReadLine();

            url = url.Replace("{queryKey}", queryKey);

            // 取得 網頁內容
            using (var client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                html = client.DownloadString(url);
            }

            // 開始解析網頁，取出裡面的圖片網址
            List<string> imgUrlList = new List<string>();
            if (!string.IsNullOrEmpty(html))
            {
                string fKey = "****^^****";
                string lKey = "****vv****";
                html = html.Replace("src=\"", fKey).Replace("\" />", lKey).Replace("\"/>", lKey);

                // 開始切割字串
                List<string> fS = html.Split(fKey);

                // 己看過，要從第3個開始
                for (int i = 2; i < fS.Count; i++)
                {
                    string tmp = fS[i];
                    List<string> lS = tmp.Split(lKey);

                    // 抓第1個就是網址
                    string imgUrl = lS[0];
                    imgUrlList.Add(imgUrl);

                    // 抓前10筆就行
                    if (imgUrlList.Count >= 10)
                    {
                        break;
                    }
                }

                // 下載圖檔
                string appPath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase;
                int index = 0;
                foreach (string imgUrl in imgUrlList)
                {
                    using (var client = new WebClient())
                    {
                        //創建臨時文件目錄下的存儲必應圖片的絕對路徑
                        var filePath = appPath + queryKey + (index).ToString() + ".png";
                        //將圖片下載到這個路徑下
                        client.DownloadFile(imgUrl, filePath);
                        Console.WriteLine($"下載圖片 {index} ， 檔名： {filePath}");
                        index++;
                    }
                }
            }

            Console.WriteLine("==============================己下載完成===========================");
            Console.Read();
        }
    }
}
