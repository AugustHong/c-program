using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;   //要加入這個才能寫檔讀檔
using System.Xml;
using System.Xml.Serialization;
using System.Text.RegularExpressions;

namespace struct_and_file
{
	//跟enum一樣要寫在外面
	public struct Book
	{
		public decimal price;
		public string title;
		public string author;
	}

	public struct CoOrds
	{
		public int x, y;

		public CoOrds(int p1, int p2)  //跟struct同名的函式，是用來一開始可以設值的
		{
			x = p1;
			y = p2;
		}
	}

	// 測試 XML的
	public class A
	{
		public string B { get; set; }

		public string C { get; set; }

		public D D { get; set; }
	}

	public class D
	{
		public string E { get; set; }
	}

	class Program
	{
		static void Main(string[] args)
		{

			CoOrds coords1 = new CoOrds();
			CoOrds coords2 = new CoOrds(10, 10);

			Console.Write("CoOrds 1: ");
			Console.WriteLine("x = {0}, y = {1}", coords1.x, coords1.y);  //沒給值的預設為0

			Console.Write("CoOrds 2: ");
			Console.WriteLine("x = {0}, y = {1}", coords2.x, coords2.y);  //有先給值的

			//如一開始沒給預設，就如下方自己設
			Book book1 = new Book();
			book1.author = "me";
			book1.title = "who is me";
			book1.price = 3000;

			Console.WriteLine("author: {0}  title:{1}  price:{2}", book1.author, book1.title, book1.price);

			//結構陣列
			Book[] books = new Book[10];

			Console.WriteLine("--------------------------------------------------------------------------");

			// 寫檔（不是複寫，所以會蓋掉之前的）   建立檔案串流（@ 可取消跳脫字元 escape sequence）
			StreamWriter sw = new StreamWriter(@"hello.txt");  //直接寫的話預設路徑是在bin/Debug裡
			sw.WriteLine("write something");
			sw.WriteLine("hello");
			sw.Close();


			//讀檔  建立檔案串流（@ 可取消跳脫字元 escape sequence）
			StreamReader sr = new StreamReader(@"hello.txt");
			while (!sr.EndOfStream)
			{               // 每次讀取一行，直到檔尾
				string line = sr.ReadLine();            // 讀取文字到 line 變數
				Console.WriteLine(line);
			}
			sr.Close();

			//讀檔，一次讀取全部至結尾
			string tmp = string.Empty;
			StreamReader sr2 = new StreamReader("hello.txt", System.Text.Encoding.UTF8);
			try
			{
				//一次讀取至結尾
				tmp = sr.ReadToEnd();
			}
			catch
			{
				tmp = "";
			}
			sr2.Close();


			//用file來建立檔案
			Console.Write("please input file name: ");
			string file_name = Console.ReadLine();

			bool b = File.Exists(file_name); // 判定檔案是否存在

			if (b)
			{
				string text = File.ReadAllText(file_name);  // 讀取檔案內所有文字
				Console.WriteLine(text);
			}
			else {
				FileStream output = File.Create(file_name);             // 建立檔案（output是要開檔來寫用的）

				StreamWriter sc = new StreamWriter(output);            //同樣寫檔，不過要用output才不會爆錯
				sc.WriteLine("this is your first file");
				sc.WriteLine("you can try to write you note");
				sc.Close();
			}

			//寫檔，且可複寫（append）
			StreamWriter sa = new StreamWriter(@"hello.txt", true);  //後面加個true是允許append
			sa.WriteLine("this is append text");
			sa.Close();

			//看過以上的之後，參考這一篇 https://dotblogs.com.tw/harry/2016/10/14/181017
			//經過實證，使用84行的  或者是57行的   。都有機會出現檔案正在使用中（因為還在建立檔案，等待傳回FileStream中）
			//故使用以下方法會更為好 （且寫檔的，自己會去判斷檔案是否存在 => 不在，會自動建立）

			using (StreamWriter sw2 = new StreamWriter(@"hello.txt", true))
			{
				sw.WriteLine("寫檔成功");
			}

			//File.Delete(路徑);   是刪除檔案
			//File.Move(原路徑, 新路徑)  是移動檔案


			//取到當前的資料夾位置
			Directory.GetCurrentDirectory();

			//取到資料夾內的所有檔案名稱
			string[] allFileName = Directory.GetFileSystemEntries("你所指定的目錄");


			//-----------------------------------------------------------------------------------------------------------------------------------------------------
			//------------------------------------------------------------------------------------------------------------------------------------------------------
			Console.WriteLine();

			//解析xml
			string xmlstring = "<aaa>hello</aaa>";
			XmlDocument doc = new XmlDocument();
			doc.XmlResolver = null;       //要設定(不然有可以會錯)
			doc.LoadXml(xmlstring);    //xmlstring 是傳入 XML 格式的 string

			//後面的那個?是確定有值才會給
			string xmlData = doc.GetElementsByTagName("aaa")[0]?.InnerText;
			string xmlData2 = doc.GetElementsByTagName("bbb")[0]?.InnerText;
			Console.WriteLine(xmlData);
			Console.WriteLine(xmlData2);   //因為沒有 => 空字串

			// 將 XML 格式轉成 Class
			try
			{
				A result = new A();
				string xml = "<A><B>bbb</B><C>ccc</C><D><E>eee</E></D></A>";
				// 記住： 上面字串的 A 和 class A 是要全部相同， D 的部份也是 => 不然會轉錯
				// 就是 不屬於 string int 這種的 自已寫的 Class 名稱 要和 xml 字串的 名稱相同
				XmlSerializer serializer = new XmlSerializer(typeof(A));
				using (StringReader reader = new StringReader(xml))
				{
					result = (A)serializer.Deserialize(reader);
				}
				Console.WriteLine($"B = {result.B} , C = {result.C}, E = {result.D.E}");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}

			/*
				第二種 將 XML 格式轉成 Class ：

				// T 是泛形 (你傳進來的)
				XmlDocument doc = new XmlDocument();
				doc.XmlResolver = null;
				doc.LoadXml(xmlString);
				XmlNodeReader reader = new XmlNodeReader(doc.DocumentElement);
				XmlSerializer ser = new XmlSerializer(typeof(T));
				T deserialize = ser.Deserialize(reader) as T;
				return deserialize;
			*/

			/*
				PS：如果你的 Class 裡面有 List 型別： 有以下2種方式可以解決轉換

				例： 
				public class A{
					public List<B> B {get;set;}
				}

				public class B{
					public string text {get;set;}
				}

				string xmlString = "<A><B><text>111</text></B><B><text>222</text></B></A>";

				(1) ： 在 List<B> 上加上 屬性 => 如下：

				public class A{	
					[XmlElement("B")] 
					public List<B> B {get;set;}
				}

				(2) ： 一個一個讀出來，再給值：
				XmlDocument doc = new XmlDocument();
				doc.XmlResolver = null;
				doc.LoadXml(xmlString);

				XmlNodeList source = doc.SelectNodes("A/B");

				A a = new A();
				a.B = new List<B>();

				foreach (XmlNode item in source)
				{
      				try
            		{
                		B b = new B();
                		B.text = item.SelectNodes("text")[0]?.InnerText;

               			a.B.add(b);
            		}
             		catch (Exception ex)
             		{
                    	System.Diagnostics.Debug.WriteLine(ex.Message);
                    	continue;
               		}
				}

			*/

			// -----------------------------------------------------------------------------------------------------------

			// 用字串分割字串
			// 平常只能用 char 來分割 => 這次用字串來分割
			string testStr = "aaaCCDaaaBBaaaKKKaaaGGGaaa";
			List<string> afterSplitStr = Regex.Split(testStr, "aaa", RegexOptions.IgnoreCase).ToList();

			foreach (var af in afterSplitStr)
			{
				Console.WriteLine(af + "  ");
			}

			Console.Read();
		}
	}
}
