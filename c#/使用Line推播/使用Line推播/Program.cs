using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

/*
	參考網址： https://qinoo.blogspot.com/2019/08/line-notify-c.html 、 https://alanlabtw.blogspot.com/2018/08/line-notify.html
      申請Line推播功能： https://notify-bot.line.me/my/
	去 加入參考 System.Web
 */

namespace 使用Line推播
{
	public class Program
	{
		static void Main(string[] args)
		{
			string token = "";  //換成你申請的token
			string message = string.Empty;
			string exitMsg = "EXIT LINE BOT";


			while (message.ToUpper() != exitMsg)
			{
				Console.WriteLine($"請輸入發送訊息，如果要離開請輸入 {exitMsg} 即可離開：");
				message = Console.ReadLine();

				if (!string.IsNullOrWhiteSpace(message) && message.ToUpper() != exitMsg)
				{
					Console.WriteLine("您輸入的是： " + message);
					var result = SendLineMessage(token, message);
					Console.WriteLine($"回傳結果： {result.Item1}  回傳訊息：  {result.Item2}");
					Console.WriteLine("===========================================================================");
				}
			}
			
		}

		// 送出 Line 訊息
		public static (int, string) SendLineMessage(string token, string message, int timeout = 30000)
		{
			int responseCode = 0;
			string responseMsg = string.Empty;

			//使用line通知
			string line_notify_url = @"https://notify-api.line.me/api/notify";
			message = @"message=" + System.Web.HttpUtility.UrlEncode(message);
			byte[] byteArray = Encoding.UTF8.GetBytes(message);
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(line_notify_url);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			request.Headers.Add("Authorization", "Bearer " + token);
			request.Timeout = timeout;

			// Get the request stream.  
			Stream dataStream = request.GetRequestStream();
			// Write the data to the request stream.  
			dataStream.Write(byteArray, 0, byteArray.Length);
			// Close the Stream object.  
			dataStream.Close();

			try
			{
				HttpWebResponse response = (HttpWebResponse)request.GetResponse();
				responseCode = (int)response.StatusCode;
				responseMsg = (String)new StreamReader(response.GetResponseStream()).ReadToEnd();
			}
			catch (Exception ex)
			{
				responseCode = 9999;
				responseMsg = ex.Message;
			}

			return (responseCode, responseMsg);
		}
	}
}
