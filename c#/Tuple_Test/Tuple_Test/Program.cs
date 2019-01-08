using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
1. 在NuGet裝上System.ValueTuple以獲得新的支援
2. c# 7.0以上才有支援此 https://docs.microsoft.com/zh-tw/dotnet/csharp/tuples#tuple-projection-initializers
3. Tuple較為嚴僅，和有些使用上的不便（當然也有好處） => 自行決定使用
*/

namespace Tuple_Test
{
	class Program
	{
		static void Main(string[] args)
		{
			//宣告方式
			Tuple<string, string> t1 = Tuple.Create("aaa", "bbb");
			Tuple<string, string> t2 = Tuple.Create(item1: "aaaa", item2: "bbbb");

			//取值
			Console.WriteLine($"item1 = {t1.Item1}   item2 = {t1.Item2}");   //沒有給會預設就是item1-item8
			Console.WriteLine($"item1 = {t2.Item1}   item2 = {t2.Item2}");

			//型態可自訂
			Tuple<int, string> t3 = Tuple.Create(20, "ccc");
			Console.WriteLine($"item1 = {t3.Item1}   item2 = {t3.Item2}");

			Console.WriteLine();

			//Tuple比較
			Tuple<string, string> t4 = Tuple.Create("aaa", "bbb");
			Tuple<string, string> t5 = t1;

			Console.WriteLine($"t1 和 t4 是相等的嗎 {(t1 == t4).ToString()}");  //False
			Console.WriteLine($"t1 和 t4 用 Equals會相同嗎？ {t1.Equals(t4).ToString()}");  //True
			Console.WriteLine($"t1 和 t5 是相等的嗎 {(t1 == t5).ToString()}"); //True

			//由上述可知，新建一個一模一樣的但是判斷起來還是False
			//故要用Equals來判斷

			Console.WriteLine();

			//函式
			var t6 = GetTuple("hello", "are", "you", "doing", "?");
			Console.WriteLine($"{t6.Item1} {t6.Item2} {t6.Item3} {t6.Item4} {t6.Item5}");

			SetTuple(Tuple.Create("x", "y", "z"));

			Console.WriteLine();

			//Tuple的修改值
			//t1.Item1 = "ggg";
			//答案：不可修改

			//所以要修改就要轉型成ValueTuple才能修改
			ValueTuple<string, string> y = t1.ToValueTuple();
			y.Item1 = "ggg";
			y.Item2 = "hhh";
			Console.WriteLine($"item1 = {y.Item1}   item2 = {y.Item2}");

			Console.Read();
		}

		//回傳Tuple的函式
		public static Tuple<string, string, string, string, string> GetTuple(string a, string b, string c, string d, string e)
		{
			return Tuple.Create(a, b, c, d, e);
		}

		//接收Tuple的函式
		public static void SetTuple(Tuple<string, string, string> data)
		{
			Console.WriteLine(data.Item1 + data.Item2 + data.Item3);
		}
	}
}
