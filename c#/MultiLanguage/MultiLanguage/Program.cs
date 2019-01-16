using MultiLanguage.MultiLanguages;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static MultiLanguage.MultiLanguageHelper;

/*
參考網址：https://blog.gss.com.tw/index.php/2017/04/02/asp_net_mvc_i18n_1/
參考網址：https://ithelp.ithome.com.tw/questions/10190017 之下方的內容

	--------------------------------------------------------------------------------------------------------------------------

創建資源檔：
1.先建好統一管理的資料夾（MultiLanguages）
2.對資料夾按右鍵/加入/新增項目         選擇  一般/文字檔（.txt的）
3.輸入Resource.resx（檔名要一同輸入喔）    Resource可以改成自己想要的
4.在Resource.resx裡面寫完你的東西後，看上方 有個 "存取修餘詞" 改成public（一定要！！！）

5.Resource.resx是主檔（中文字）  皆下來就是繼續新增語系 
6.照前面的方式新增  檔名為：  你主檔所輸入的（Resource） . 語系（英文：en-US、日文：ja-JP、韓文：ko-KR）
例如：Resource.en-US.resx

*/


/*
在MVC中有幾個要注意的：
1. 在Global.asax中加入

		protected void Application_PostMapRequestHandler(Object sender, EventArgs e)  {
			try {
				var language = (string)HttpContext.Current.Request.RequestContext.RouteData.Values["lang"] ?? "zh-TW";
				Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
			} catch (Exception) { }
		}

2. 在View中（最好是在 Layout、或者其他全部View都會套用的）  中加入

@{
	//這邊主要是取到當前語系，因為如果有語系了 那 url = 語系/Controller/Action
	//所以超聯結就寫 <a href="@lang/Controller/Action" >聯結</a>
	var culture = System.Globalization.CultureInfo.CurrentCulture;
	var lang = culture.Name == "zh-TW" ? string.Empty : "/" + culture.Name;
}

中間的部份會有切換語系的按鈕或下拉式選單

下方會有個切換語系的js如下：
<script>
	//切換語系
	function ChangeLang(langValue){
		window.location.href = "/" + langValue;
	}
</script>

3.最好寫一隻Helper來再度處理（可以使用下面的）

4.在App_Start/RouteConfig.cs  中加上 （記得要放在 name:"Default" 那隻程式 的 上面喔）

			//因為多國語系，所以路徑改為  語系/Controller/Action
			//預設沒有語系時的路徑（因為語系如果是空的=>預設中文）
			routes.MapRoute(
					name: "Localized",
					url: "{lang}/{controller}/{action}/{id}",
					constraints: new { lang = @"(\w{2})|(\w{2}-\w{2})" },
					defaults: new { controller = "Captcha", action = "Index", id = UrlParameter.Optional }
			);

 */

namespace MultiLanguage
{
	class Program
	{
		static void Main(string[] args)
		{
			//取預設值方式
			var Test_defalut = MultiLanguageHelper.GetResourceValue("測試");//output:測試 ，因為我預設是zh-TW

			//使用工具方式+enum
			var Test_zhTW = MultiLanguageHelper.GetResourceValue("測試", LangugeEnum.zh_tw);//output:測試
			var Test_enUS = MultiLanguageHelper. GetResourceValue("測試", LangugeEnum.en_US);//output:TEST 
			var Test_jaJP = MultiLanguageHelper.GetResourceValue("測試", LangugeEnum.ja_JP);//output:テストする
			var Test_koKR = MultiLanguageHelper.GetResourceValue("測試", LangugeEnum.ko_KR);//output:시험

			//使用string key方式，可以用在cookie取值帶參數
			var Test2_zhTW = MultiLanguageHelper.GetResourceValue("測試", "zh-TW");//output:測試
			var Test2_enUS = MultiLanguageHelper.GetResourceValue("測試", "en-US");//output:TEST 
			var Test2_jaJP = MultiLanguageHelper.GetResourceValue("測試", "ja-JP");//output:テストする
			var Test2_koKR = MultiLanguageHelper.GetResourceValue("測試", "ko-KR");//output:시험


			Console.WriteLine(Test_defalut);
			Console.WriteLine(Test_zhTW);
			Console.WriteLine(Test_enUS);
			Console.WriteLine(Test_jaJP);
			Console.WriteLine(Test_koKR);
			Console.WriteLine(Test2_zhTW);
			Console.WriteLine(Test2_enUS);
			Console.WriteLine(Test2_jaJP);
			Console.WriteLine(Test2_koKR);

			//日文、韓文出現????是正常的，因為我沒有相關的語言

			Console.Read();
		}
	}

	/// <summary>
	/// 進行相關多語言的執行
	/// </summary>
	public class MultiLanguageHelper
	{
		const string
		    zh_tw = "zh-TW",
		    en_US = "en-US",
			ja_JP = "ja-JP",
			ko_KR = "ko-KR";

		//設定字典類型
		readonly static Dictionary<string, CultureInfo> UseCultureInfoDictionary = new Dictionary<string, CultureInfo>()
	  {
		{ zh_tw, CultureInfo.GetCultureInfo(zh_tw) },
		{ en_US, CultureInfo.GetCultureInfo(en_US) },
		{ ja_JP, CultureInfo.GetCultureInfo(ja_JP) },
		{ ko_KR, CultureInfo.GetCultureInfo(ko_KR) }
	  };

		//設定 列舉 類型
		public enum LangugeEnum
		{
			zh_tw = 1,
			en_US = 2,
			ja_JP = 3,
			ko_KR = 4
		}

		//用列舉取得資料，並轉成該語系
		public static string GetResourceValue(string key, LangugeEnum langugeenum)
		{
			var languekey = string.Empty;

			switch (langugeenum)
			{
				case LangugeEnum.zh_tw:
					languekey = zh_tw;
					break;
				case LangugeEnum.en_US:
					languekey = en_US;
					break;
				case LangugeEnum.ja_JP:
					languekey = ja_JP;
					break;
				case LangugeEnum.ko_KR:
					languekey = ko_KR;
					break;
				default:
					return Resource.ResourceManager.GetString(key);
			}

			//取得文字（第一個參數：文字  第二個參數：語系）
			string result =  Resource.ResourceManager.GetString(key, UseCultureInfoDictionary[languekey]);

			return string.IsNullOrEmpty(result) ? key : result;
		}

		//使用輸入語系的
		public static string GetResourceValue(string key, string langugekey)
		{
			//取得文字（第一個參數：文字  第二個參數：語系）
			string result = Resource.ResourceManager.GetString(key, UseCultureInfoDictionary[langugekey]);

			return string.IsNullOrEmpty(result) ? key : result;
		}

		//使用預設的
		public static string GetResourceValue(string key)
		{
			//預設的
			string result = Resource.ResourceManager.GetString(key);

			return string.IsNullOrEmpty(result) ? key : result;
		}
	}
}
