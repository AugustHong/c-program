using LinqKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
	參考網圵： https://toyo0103.blogspot.com/2016/01/linqlinq.html

	步驟1 ： 去 NuGet 裝上 LinqKit 套件
*/

namespace Linq累加動態條件
{
	class Program
	{
		static void Main(string[] args)
		{
			// 如果我們有個叫 關鍵字 的 搜尋條件， 且可以用 , 分開  但只要符合一筆就要列出來
			Console.WriteLine("找尋 用 , 隔開後 是 A 或者  是C 的 資料 (完全相等，故有一筆 AC 的不該出現 )");

			Console.WriteLine("資料列表：");
			List<string> source = new List<string> { "A,B", "A", "B,D", "C,E", "A,B,C", "AC", "B", "D,E" };

			foreach(var s in source)
			{
				Console.WriteLine(s);
			}

			// 開始實作
			// 先建置 (第一步： 先建立目標， 後面的<>裡放的是你 要丟進去的物件類型， 不用再加上 List了)
			var pred = PredicateBuilder.True<string>();

			// 這是查詢的條件 (應該是 A,C 再用 , 分開   ， 這邊就比較偷懶的寫了)
			List<string> queryStr = new List<string> { "A", "C" };

			bool first = true;
			foreach (var qs in queryStr)
			{
				// 開始串 條件 (第一個要用And 喔)
				string s = qs;

				if (first)
				{
					pred = pred.And(x => x.Split(',').Contains(s));
					first = false;
				}
				else
				{
					pred = pred.Or(x => x.Split(',').Contains(s));   // 用 Or ， 也可以用 And
				}
			}

			// 得到結果
			List<string> result = source.Where(pred.Compile()).ToList();

			// 輸出結果
			Console.WriteLine("查詢結果：");

			foreach (var r in result)
			{
				Console.WriteLine(r);
			}

			Console.Read();
		}
	}
}
