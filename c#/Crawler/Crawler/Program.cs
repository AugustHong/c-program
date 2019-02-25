using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

/*
	c#爬蟲的寫法

	參考網圵： https://sites.google.com/site/willsnote/Home/%E5%A6%82%E4%BD%95%E6%A7%8B%E9%80%A0%E4%B8%80%E5%80%8Bc%E8%AA%9E%E8%A8%80%E7%9A%84%E7%88%AC%E8%9F%B2%E7%A8%8B%E5%BC%8F
	參考網圵2： https://dotblogs.com.tw/v6610688/2013/11/02/parsing_html_by_html_agility_parser_on_windows_phone_8
*/

namespace Crawler
{
	class Program
	{
		static void Main(string[] args)
		{
			string result = "";
			byte[] bResult;

			/*  下載Html的部份 */

			string goalUrl = "https://news.google.com/?hl=zh-TW&gl=TW&ceid=TW:zh-Hant";
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(goalUrl);
			WebResponse response = request.GetResponse();
			Stream stream = response.GetResponseStream();

			//開始下載作業
			//二進位文件的內容類型聲明不以“text/”開頭(所以先判斷是二進位文件，還是有字串文件)
			if (!response.ContentType.ToLower().StartsWith("text/"))
			{
				//儲存成二進制
				bResult = SaveBinaryFile(response, "result.html");
			}
			else
			{
				//存成字串文件
				result = SaveFile(stream, "result.html");
			}

			Console.WriteLine("===================================================================");


			/*  解析Html的部份   => 這裡目前無法使用，根本沒寫要裝什麼才能使用 ParseHTML類別 */

			//建立類型
			//ParseHTML parse = new ParseHTML();

			////加入要解析的html文檔內容
			//parse.Source = "Hello World";

			////解析
			//while (!parse.Eof())
			//{
			//	char ch = parse.Parse();

			//	//如果回傳0代表是讀到html標籤
			//	if (ch == 0)
			//	{
			//		//讀取標籤
			//		HTMLTag tag = parse.GetTag();

			//		Attribute href = tag["HREF"];
			//		string link = href.Value;
			//	}
			//}

			Console.WriteLine("==========================================================================");

			/* 使用 HtmlAgilityPack 處理 爬網(先去Nuget裝上 HtmlAgilityPack ，並引用)*/

			HtmlDocument document = new HtmlDocument();
			document.LoadHtml("<ul><li>aaa</li><li id='b'>bbb</li></ul><ul><li>ccc</li></ul>");

			//後面那邊有點像正規表達式的寫法(但仍看網圵2寫的) => 找到所有的ul
			var ulElement = document.DocumentNode.SelectNodes("//ul");
			foreach (var item in ulElement)
			{
				//抓出 li 的部份
				var liElements = item.SelectNodes("li");

				foreach (var li in liElements)
				{
					//取出id和其值
					string id = li.Id;
					string value = li.InnerHtml;
					Console.WriteLine("id:{0}, content:{1}", "id", value);
				}
			}

			
			Console.WriteLine();

			//利用上面取到的資料來做事吧
			HtmlDocument document2 = new HtmlDocument();
			document2.LoadHtml(result);
			var hrefElement = document2.DocumentNode.SelectNodes("//a");

			foreach(var href in hrefElement)
			{
				//取得Attributes的內容，可以下中斷點去詳細查看。
				//注意：一定要判斷 == null ，因為不一定他會有這個屬性
				string hrefUrl = href.Attributes["href"] == null ? "" : href.Attributes["href"].Value;
				string hrefContent = href.InnerHtml;
				Console.WriteLine($"Url = {hrefUrl}       ,   Conten = {hrefContent} ");
			}

			Console.Read();
		}


		/// <summary>
		/// 將得到的html存成二進制文件
		/// </summary>
		/// <param name="response">得到的Response</param>
		/// <param name="fileName">完整檔案路徑</param>
		private static byte[] SaveBinaryFile(WebResponse response, string filePath)
		{
			byte[] buffer = new byte[1024];

			Stream outStream = File.Create(filePath);
			Stream inStream = response.GetResponseStream();

			int l;
			do
			{
				l = inStream.Read(buffer, 0,buffer.Length);

				if (l > 0) { outStream.Write(buffer, 0, l); }
								
			} while (l > 0);

			return buffer;
		}

		/// <summary>
		/// 存成字串文件
		/// </summary>
		/// <param name="stream">得到的Stream</param>
		/// <param name="filePath">完整檔案路徑</param>
		private static string SaveFile(Stream stream, string filePath)
		{
			string buffer = "";
			string line = "";

			//讀取
			StreamReader reader = new StreamReader(stream);
			while ((line = reader.ReadLine()) != null){buffer += line + "\r\n"; Console.WriteLine(line); }

			Console.WriteLine("要寫檔了");

			//寫檔
			using (StreamWriter sw = new StreamWriter(@"" + filePath, false))
			{
				sw.WriteLine(buffer);
			}

			Console.WriteLine("寫檔完畢");

			return buffer;
		}
	}
}
