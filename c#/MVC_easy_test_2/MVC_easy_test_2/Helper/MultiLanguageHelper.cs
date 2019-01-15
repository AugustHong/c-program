using MVC_easy_test_2.MultiLanguage;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace MVC_easy_test_2.Helper
{
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
				string result = Resource.ResourceManager.GetString(key, UseCultureInfoDictionary[languekey]);

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