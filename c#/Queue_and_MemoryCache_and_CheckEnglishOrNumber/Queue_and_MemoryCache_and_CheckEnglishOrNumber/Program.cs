using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Runtime.Caching;  //要使用快取（先去 參考/加入參考/組建/架構  中  安裝）
using System.IO;

namespace Queue_and_MemoryCache_and_CheckEnglishOrNumber
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
				Queue
			*/

			//Queue為一個先進先出的類似陣列的型別
			Queue<string> q = new Queue<string>();

			//放入值（且一定放至最尾端）
			q.Enqueue("a");
			q.Enqueue("b");

			//取出第一個值，並且自動從Queue刪除
			Console.WriteLine(q.Dequeue());         //a，而a就會被刪除，所以下面的答案會是b

			//取出第一個值，但不會刪除它
			Console.WriteLine(q.Peek());             //b

			//如果當前數目 小於 Queue的容量的 90%，則將容量改為 目前的數目
			q.TrimExcess();

			//移除所有物件
			q.Clear();

			Console.WriteLine();
			Console.WriteLine("-----------------------------------------------------------------------------------------------------------");


			/*
				判斷是否只是英數字（即1byte內的，例如中文字一次佔2byte）
			 */

			Console.WriteLine($"00ab00dll3kodij      是    純ascii中的字元嗎?  {CheckEnglishOrNumberHelper.IsEnglishOrNumber("00ab00dll3kodij").ToString()}");     //True
			Console.WriteLine($"哈ab00dll3kodij      是    純ascii中的字元嗎?  {CheckEnglishOrNumberHelper.IsEnglishOrNumber("哈ab00dll3kodij").ToString()}");    //False


			Console.WriteLine();
			Console.WriteLine("------------------------------------------------------------------------------------------------------------");


			/*
				MemoryCache  快取記憶體
			 */

			MemoryCacheHelper m = new MemoryCacheHelper();

			string a = "aaaaa";
			string b = "bbbbb";

			//把A加入裡面
			m.Set("A", a);

			//看是否有在裡面
			Console.WriteLine($"aaaaa   => A物件  有在  快取中嗎 {m.Contains("A")}");   //True
			Console.WriteLine($"bbbbb  => B物件  有在  快取中嗎 {m.Contains("B")}");   //False

			//取出物件
			Console.WriteLine($"從快取中取出 key = A 的物件，其value = {m.Get("A")}");

			Console.WriteLine();
			Console.WriteLine("------------------------------------------------------------------------------------------------------------");

			/*
				MemoryStream的使用
				參考網圵： https://docs.microsoft.com/zh-tw/dotnet/api/system.io.memorystream?view=netframework-4.7.2
			*/

			int count;
			byte[] byteArray;
			char[] charArray;

			//要做Unicode轉碼的
			UnicodeEncoding uniEncoding = new UnicodeEncoding();

			// 要進行存進MemoryStream中的資料(要轉成Bytes)，Path.GetInvalidPathChars()是取得路徑
			byte[] firstString = uniEncoding.GetBytes("Invalid file path characters are: ");
			byte[] secondString = uniEncoding.GetBytes(Path.GetInvalidPathChars());

			//這是取得路徑上的無效字元(所以出來會是一大堆亂碼)
			Console.WriteLine(Path.GetInvalidPathChars());

			//開始使用MemoryStream，後面參數的100是位元素容量 即類似 Byte[100]
			using (MemoryStream memStream = new MemoryStream(100))
			{
				// 第一種的寫法
				memStream.Write(firstString, 0, firstString.Length);

				// 第二種寫法(慢慢把byte讀進去)
				count = 0;
				while (count < secondString.Length)
				{
					memStream.WriteByte(secondString[count++]);
				}

				// Capacity是容量，Length是目前的長度，Position是目前的位置(類似Seek)
				Console.WriteLine("Capacity = {0}, Length = {1}, Position = {2}\n",memStream.Capacity.ToString(),memStream.Length.ToString(),memStream.Position.ToString());

				// 將Seek設置在初使上
				memStream.Seek(0, SeekOrigin.Begin);

				// 從MemoryStream中讀取20 Byte至宣告的
				byteArray = new byte[memStream.Length];
				count = memStream.Read(byteArray, 0, 20);

				// 讀取資料，用一個一個Byte去讀取
				while (count < memStream.Length)
				{
					byteArray[count++] =Convert.ToByte(memStream.ReadByte());
				}

				//以下2行是將Byte[]用UniCode解碼成Char[]
				//GetChars(byteArray, 0, count, charArray, 0); 這段是把byteArray變數中的 第0位數 到 count數 的byte內容 從 charArray
				//的第0位數開始放	
				charArray = new char[uniEncoding.GetCharCount(byteArray, 0, count)];
				uniEncoding.GetDecoder().GetChars(byteArray, 0, count, charArray, 0);

				Console.WriteLine(charArray);
			}


			Console.Read();
		}
	}

	/// <summary>
	/// 用來判斷是否是英數字 或者 ascii中的文字（即1byte的）
	/// </summary>
	public  static class CheckEnglishOrNumberHelper
	{
		/// <summary>
		/// 用來判斷是否是英數字 或者 ascii中的文字（即1byte的），例如：中文就是2byte的
		/// </summary>
		/// <param name="words">要比較的單字</param>
		/// <returns></returns>
		public static bool IsEnglishOrNumber(string words)
		{
			string TmmP;
			for (int i = 0; i < words.Length; i++)
			{
				TmmP = words.Substring(i, 1);
				byte[] sarr = System.Text.Encoding.GetEncoding("gb2312").GetBytes(TmmP);

				//如果byte的長度 > 1 的，就代表它不是ascii中的文字（1byte）
				if (sarr.Length > 1)
				{
					return false;
				}
			}
			return true;
		}

	}


	/// <summary>
	/// 快取記憶體的Helper
	/// </summary>
	public class MemoryCacheHelper
	{
		//宣告一個快取物件
		private ObjectCache cache = MemoryCache.Default;

		/// <summary>
		/// 設定快取物件
		/// </summary>
		/// <param name="key">key</param>
		/// <param name="value">value（是個物件類型）</param>
		public void Set(string key, object value)
		{
			cache.Set(key, value, new CacheItemPolicy { AbsoluteExpiration = ObjectCache.InfiniteAbsoluteExpiration });
		}

		/// <summary>
		/// 取得快取
		/// </summary>
		/// <param name="key">key</param>
		/// <returns></returns>
		public object Get(string key)
		{
			return cache.Get(key);
		}

		/// <summary>
		/// 看此類型是否在快取中
		/// </summary>
		/// <param name="key">key</param>
		/// <returns></returns>
		public bool Contains(string key)
		{
			return cache.Contains(key);
		}
	}
}
