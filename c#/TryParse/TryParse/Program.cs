using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TryParse
{
	class Program
	{
		static void Main(string[] args)
		{
			/*
				在轉型前先做這個判斷，可以幫你轉型也可得出是否可以轉	
				參考網址：https://docs.microsoft.com/zh-tw/dotnet/csharp/programming-guide/strings/how-to-determine-whether-a-string-represents-a-numeric-value
			 */

			//Int的部份
			int i = 0;
			string text = "108";
			//轉換，第二個參數是如果可以的話，會自動幫你轉型
			bool result = int.TryParse(text, out i);
			Console.WriteLine($"字串為 108  可以轉成 Int 嗎 = {result} ，如果可以轉其值為 {i}");

			//long的部份
			long l = 0;
			text = "1287543";
			bool result1 = long.TryParse(text, out l);
			Console.WriteLine($"字串為 1287543  可以轉成 long 嗎 = {result1} ，如果可以轉其值為 {l}");

			//byte的部份
			byte b = 0;
			text = "255";
			bool result2 = byte.TryParse(text, out b);
			Console.WriteLine($"字串為 255  可以轉成 byte 嗎 = {result2} ，如果可以轉其值為 {b}");

			//decimal 的部份
			decimal d = 0;
			text = "27.3";
			bool result3 = decimal.TryParse(text, out d);
			Console.WriteLine($"字串為 27.3  可以轉成 decimal 嗎 = {result3} ，如果可以轉其值為 {d}");

			//decimal 的部份
			double dou = 0;
			text = "27.3";
			bool result4 = double.TryParse(text, out dou);
			Console.WriteLine($"字串為 27.3  可以轉成 double 嗎 = {result4} ，如果可以轉其值為 {dou}");

			//bool的部份
			bool bo = false;
			text = "true";
			bool result5 = bool.TryParse(text, out bo);
			Console.WriteLine($"字串為 true  可以轉成 bool 嗎 = {result5} ，如果可以轉其值為 {bo}");

			//datetime的部份
			DateTime date = new DateTime();
			text = "2018/12/20";
			bool result6 = DateTime.TryParse(text, out date);
			Console.WriteLine($"字串為 2018/12/20  可以轉成 DateTime 嗎 = {result6} ，如果可以轉其值為 {date}");

			//short的部份
			short s = 0;
			text = "2";
			bool result7 = short.TryParse(text, out s);
			Console.WriteLine($"字串為 2  可以轉成 short 嗎 = {result7} ，如果可以轉其值為 {s}");

			//char的部份
			char c = new char();
			text = "a";
			bool result8 = char.TryParse(text, out c);
			Console.WriteLine($"字串為 a  可以轉成 char 嗎 = {result8} ，如果可以轉其值為 {c}");

			//float的部份
			float f = 0;
			text = "3.1415925235";
			bool result9 = float.TryParse(text, out f);
			Console.WriteLine($"字串為 3.1415925235  可以轉成 float 嗎 = {result9} ，如果可以轉其值為 {f}");

			//其他一樣可以轉的類型有
			uint u = 0;
			uint.TryParse(text, out u);

			ulong ul = 0;
			ulong.TryParse(text, out ul);

			ushort us = 0;
			ushort.TryParse(text, out us);

			sbyte sb = 0;
			sbyte.TryParse(text, out sb);

			Console.Read();
		}
	}
}
