using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;   //方法1（先去NuGet裝上Newtonsoft.Json）

using System.Web.Script.Serialization;  //方法2（參考/右鍵/加入參考 => System.Web.Extensions）

/*
	本次講述如何轉成JSON字串，和把JSON字串轉成Class

	總共有2種方法：（各自要using的不同）
	1.基本型
	2.使用下方的Helper來做的

	參考網址：（方法1） https://dotblogs.com.tw/berrynote/2016/08/18/200338
*/

namespace JsonString
{
	//測試用的
	public class Test
	{
		public Test()
		{
			nameList = new List<string>();
		}

		public int id { get; set; }
		public List<string> nameList { get; set; }
	}

	class Program
	{
		static void Main(string[] args)
		{
			//先製作假資料
			List<Test> t = new List<Test>();

			Test t1 = new Test { id = 1, nameList = { "aaa", "bbb", "ccc" } };
			Test t2 = new Test { id = 2, nameList = { "AAA", "BBB", "CCC" } };
			t.Add(t1);
			t.Add(t2);

			#region 方法1

			//轉成JSON字串 =>  JsonConvert.SerializeObject(要轉的物件);
			string json = JsonConvert.SerializeObject(t);

			Console.WriteLine(json);

			//解JSON字串 =>  JsonConvert.DeserializeObject<要轉出的類型>(json字串);
			List<Test> rtn = JsonConvert.DeserializeObject<List<Test>>(json);

			foreach(var r in rtn) { Console.WriteLine($"id = {r.id}  value = {r.nameList[0]} , {r.nameList[1]} , {r.nameList[2]}"); }
			#endregion

			#region 方法2
			//轉成JSON字串  => 可直接用 new JavaScriptSerializer().Serialize(t);即可
			json = JsonStringHelper.AsJsonList(t);

			Console.WriteLine(json);

			//解JSON字串  => 可直接用 new JavaScriptSerializer().Deserialize<List<T>>(tt); 即可
			rtn = JsonStringHelper.AsObjectList<Test>(json);

			foreach (var r in rtn) { Console.WriteLine($"id = {r.id}  value = {r.nameList[0]} , {r.nameList[1]} , {r.nameList[2]}"); }
			#endregion

			//------------------------------------------------------------------------------------------------------------------------------------------------------


			/*
				MVC中Controller和View互傳JSON

				參考網圵： https://blog.csdn.net/a84934532/article/details/53121444

				基本上都照著網圵的即可，比較特別的是Controller給View中的 "要用個input去接"
				=> 用 <input type="text" style="display:none;" id="jsonData" value="@ViewBag.JsonString" />
				而不行直接用 var s = "@ViewBag.JsonString" ，用這種得到的s 中 " 這個符號會變成 &qot; 而且去不掉

				，所以用input去接  然後用 var s = $('#jsonData').val();
				, 再來用 var json = eval('(' + s + ')'); 即可得到	
			
			*/

			// ----------------------------------------------------------------------------------------------

			/*
				如果是 JObject 物件要轉為 Model 的話： (如下)
				JObject jo = new JObject;
				A a = jo.ToObject<A>();
			*/

			Console.Read();
		}
	}

	//方法2要用到的Helper（當然也可以不用，直接拿裡面的方法來用即可）
	public static class JsonStringHelper
	{
		public static string AsJsonList<T>(List<T> tt)
		{
			return new JavaScriptSerializer().Serialize(tt);
		}
		public static string AsJson<T>(T t)
		{
			return new JavaScriptSerializer().Serialize(t);
		}
		public static List<T> AsObjectList<T>(string tt)
		{
			return new JavaScriptSerializer().Deserialize<List<T>>(tt);
		}
		public static T AsObject<T>(string t)
		{
			return new JavaScriptSerializer().Deserialize<T>(t);
		}
	}
}
