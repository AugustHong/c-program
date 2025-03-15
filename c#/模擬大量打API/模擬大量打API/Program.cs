using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace 模擬大量打API
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            string url = @"你的網址";
            string jwt = "有些要JWT";
            int runTimes = 1000; // 一次執行次數量
            List<Task> taskList = new List<Task>();

            using (var httpClient = new HttpClient())
            {
                // 在這裡設定全域 Header（如果所有請求都需要相同的 Header）
                // 有需要才要加
                httpClient.DefaultRequestHeaders.Add("JWT", $"{jwt}");

                for (int i = 0; i < runTimes; i++)
                {
                    DateTime now = DateTime.Now;

                    // 傳入的資料
                    var jsonBody = new
                    {
                        A = "11",
                        B = 20,
                        C = false,
                        D = now
                    };

                    taskList.Add(SendRequestAsync(httpClient, url, i, jsonBody));
                }

                await Task.WhenAll(taskList); // 等待所有完成
            }

            Console.WriteLine("已完成！");
        }

        // 呼叫API動作
        static async Task SendRequestAsync(HttpClient httpClient, string url, int seq, object jsonBody)
        {
            try
            {
                string json = JsonConvert.SerializeObject(jsonBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                Console.WriteLine($"=======================Request(序號 {seq})=========================");
                Console.WriteLine(json);
                Console.WriteLine($"=======================Request(序號 {seq})=========================");

                var response = await httpClient.PostAsync(url, content);
                Console.WriteLine($"=======================Response(序號 {seq})========================");
                Console.WriteLine($"Response StatusCode: {response.StatusCode}");
                string responseJson = await response.Content.ReadAsStringAsync();
                Console.WriteLine($"Response Result ： {responseJson}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"發生錯誤: {ex.Message}");
            }

            Console.WriteLine($"=======================Response(序號 {seq})========================");
        }
    }
}
