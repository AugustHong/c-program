using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Configuration;

// 轉全形用的
using Microsoft.VisualBasic;
using System.IO;

namespace Hong.StringHelper
{
	public static class StringHelper
	{
		#region  因為 Substring 都會爆錯 => 所以加上 try catch ，就不用再寫那麼多行了
		/// <summary>
		///  同 原本的 Substring ， 但是有加上 try catch => 會回傳 空
		/// </summary>
		/// <param name="source"></param>
		/// <param name="startIndex"></param>
		/// <param name="length"></param>
		/// <returns></returns>
		public static string SubString2(this string source, int startIndex, int length)
		{
			try
			{
				// 錯誤判斷，不要讓他 猛進到 try catch 脫慢速度
				if (string.IsNullOrEmpty(source)) { return string.Empty; }
				int len = source.Length;
				if (startIndex >= len) { return string.Empty; }
				if ((startIndex + length) > len) { return source.SubString2(startIndex); }

				source = string.IsNullOrEmpty(source) ? string.Empty : source;
				string result = source.Substring(startIndex, length);
				return result;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " --> " + ex.Message);

				return source.SubString2(startIndex);
			}
		}

		/// <summary>
		///  同 原本的 Substring ， 但是有加上 try catch => 會回傳 空
		/// </summary>
		/// <param name="source"></param>
		/// <param name="startIndex"></param>
		/// <returns></returns>
		public static string SubString2(this string source, int startIndex)
		{
			try
			{
				// 錯誤判斷，不要讓他 猛進到 try catch 脫慢速度
				if (string.IsNullOrEmpty(source)) { return string.Empty; }
				if (startIndex >= source.Length) { return string.Empty; }

				source = string.IsNullOrEmpty(source) ? string.Empty : source;
				string result = source.Substring(startIndex);
				return result;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " --> " + ex.Message);
				return string.Empty;
			}
		}

		#endregion

		#region  重複字串 

		//回傳重複的 "字元" 所組成的字串
		public static string Repeat(this char source, int repeatNum = 2)
		{
			repeatNum = repeatNum < 1 ? 1 : repeatNum;
			return new string(source, repeatNum);
		}

		//回傳重複的 "字串" 所組成的字串
		public static string Repeat(this string source, int repeatNum = 2)
		{
			if (string.IsNullOrEmpty(source)) { return string.Empty; }
			if (repeatNum <= 1)
			{
				return source;
			}

			var builder = new StringBuilder(repeatNum * source.Length);
			for (int i = 0; i < repeatNum; i++)
			{
				builder.Append(source);
			}
			return builder.ToString();
		}

		#endregion

		/// <summary>
		///  維持固定的長度回傳
		///  isRightAddSpace = 如果不足長度是否是向右補空白 (預設是 true, 如果是 false 就是像左補空白)
		/// </summary>
		/// <param name="source"></param>
		/// <param name="startIndex"></param>
		/// <returns></returns>
		public static string GetFixedLength(this string source, int stringLen, bool isRightAddSpace = true)
		{
			// 長度 
			stringLen = stringLen <= 0 ? 0 : stringLen;

			// String.Format("{0, -3}", ) => 不足3，向右補空白
			// String.Format("{0, 3}", ) => 不足3，向左補空白
			string addSpace = isRightAddSpace ? "-" : "";
			return string.Format("{0, " + addSpace + stringLen.ToString() + "}", source.SubString2(0, stringLen));
		}

		/// <summary>
		///  同上用法，只是會把  "半形空白" 全換成 "全形空白"
		///  所以裡面的內容本來就有空白的要注意
		/// </summary>
		/// <param name="source"></param>
		/// <param name="stringLen"></param>
		/// <param name="isRightAddSpace"></param>
		/// <returns></returns>
		public static string GetFixedLengthByWide(this string source, int stringLen, bool isRightAddSpace = true)
		{
			return source.GetFixedLength(stringLen, isRightAddSpace).StrConv();
		}

		#region 字串比大小，寫的比較直覺

		/// <summary>
		///  字串 source 是否 比 matchStr 小
		/// </summary>
		/// <param name="source"></param>
		/// <param name="matchStr"></param>
		/// <returns></returns>
		public static bool SmallTo(this string source, string matchStr)
		{
			/*
			  string.Compare(A, B) => 會得出數字
			  < 0 => A < B
			  = 0 => A = B
			  > 0 => A > B
			 */
			return string.Compare(source, matchStr) < 0;
		}

		/// <summary>
		///  字串 source 是否 比 matchStr 大
		/// </summary>
		/// <param name="source"></param>
		/// <param name="matchStr"></param>
		/// <returns></returns>
		public static bool LargeTo(this string source, string matchStr)
		{
			return string.Compare(source, matchStr) > 0;
		}

		#endregion

		#region 字串分割 + Trim

		/// <summary>
		/// 用正規化去 切割字串
		/// </summary>
		/// <param name="source"></param>
		/// <param name="splitStr"></param>
		/// <returns></returns>
		public static List<string> RegexSplit(this string source, string splitStr)
		{
			List<string> result = new List<string>();

			// 如果是 空的，就直接回傳 空字串
			if (string.IsNullOrEmpty(source))
			{
				return new List<string> { "" };
			}

			// 如果切割字串是 null 回傳整個
			if (splitStr == null)
			{
				result.Add(source);
				return result;
			}

			return Regex.Split(source, "\r\n", RegexOptions.IgnoreCase).ToList();
		}

		/// <summary>
		/// 上面的正規化 如果用 . 來切 會變成要輸入 \. (有點像 js 會遇到的狀況)
		/// 解法： 自已寫
		/// </summary>
		/// <param name="source"></param>
		/// <param name="splitStr"></param>
		/// <returns></returns>
		public static List<string> Split(this string source, string splitStr)
		{
			List<string> result = new List<string>();

			string tmpSource = source;

			// 如果是 空的，就直接回傳 空字串
			if (string.IsNullOrEmpty(source))
			{
				return new List<string> { "" };
			}

			// 如果切割字串是 null 回傳整個
			if (splitStr == null)
			{
				result.Add(source);
				return result;
			}

			int len = tmpSource.Length;

			// 如果 切割字串是 空字串，就每個字母來切
			if (splitStr == string.Empty)
			{
				for (var i = 0; i < len; i++)
				{
					string tmp = source.Substring(i, 1);
					result.Add(tmp);
				}
				return result;
			}

			// 其餘照著切
			int splitStrLen = splitStr.Length;

			// 判斷是否有進去
			bool haveI = false;

			// 位置
			int pos = tmpSource.IndexOf(splitStr);

			// 直到結束
			while (pos >= 0)
			{
				haveI = true;
				string tmp = string.Empty;

				if (pos == 0)
				{
					tmp = string.Empty;
				}
				else
				{
					tmp = tmpSource.Substring(0, pos);
				}

				result.Add(tmp);

				// 算出要延後幾位
				int diff = pos + splitStrLen;

				// 切割 (讓剩下的繼續跑)
				tmpSource = tmpSource.Substring(diff);

				// 重算位置
				pos = tmpSource.IndexOf(splitStr);

				// 如果 最後一次砍完剩下 空字串 => 要 push 進去
				if (string.IsNullOrEmpty(tmpSource))
				{
					result.Add(string.Empty);
				}
				else
				{
					if (pos < 0)
					{
						result.Add(tmpSource);
					}
				}
			}

			// 如果一開始就查不到 => 直接回傳 自己
			if (haveI == false)
			{
				result.Add(source);
			}

			return result;
		}

		/// <summary>
		/// Trim
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string Trim2(this string source)
		{
			string result = string.Empty;

			if (!string.IsNullOrEmpty(source))
			{
				try
				{
					result = source.Trim();
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.Message);
					return string.Empty;
				}
			}

			return result;
		}

		/// <summary>
		/// TrimStart
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string TrimStart2(this string source)
		{
			string result = string.Empty;

			if (!string.IsNullOrEmpty(source))
			{
				try
				{
					result = source.TrimStart();
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine(ex.Message);
					return string.Empty;
				}
			}

			return result;
		}

		#endregion

		#region 相關 TryParse 平常要寫2行 => 把他精簡掉

		/// <summary>
		///  把用 try parse 寫成2行的部份 轉成一行
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static int IntTryParse(this char source)
		{
			int result = 0;

			try
			{
				result = Convert.ToInt32(source);
				return result;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(ex.Message);
				return 0;
			}
		}

		/// <summary>
		///  把用 try parse 寫成2行的部份 轉成一行
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static int IntTryParse(this string source)
		{
			int result = 0;
			int.TryParse(source, out result);
			return result;
		}

		/// <summary>
		///  把用 try parse 寫成2行的部份 轉成一行
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static decimal DecimalTryParse(this string source)
		{
			decimal result = 0;
			decimal.TryParse(source, out result);
			return result;
		}

		/// <summary>
		/// 轉成 Decimal ， 後面可以輸入有幾個小數位
		/// 例如： source = "12345", floatNum = 2 => 結果 = 123.45
		/// </summary>
		/// <param name="source"></param>
		/// <param name="floatNum"></param>
		/// <returns></returns>
		public static decimal DecimalTryParse(string source, int floatNum = 0)
		{
			decimal result = 0;
			if (string.IsNullOrEmpty(source)) { source = "0"; }
			floatNum = floatNum <= 0 ? 0 : floatNum;

			string before = string.Empty;
			string after = string.Empty;
			string haveFloat = (floatNum <= 0) ? "" : ".";  // 是否要出現小數點 (如果 float是 <= 0 的話)

			if (floatNum >= source.Length)
			{
				before = "0";

				int diff = floatNum - source.Length;
				for (var i = 1; i <= diff; i++)
				{
					after += "0";
				}
				after += source;
			}
			else
			{
				int diff = source.Length - floatNum;
				before = source.SubString2(0, diff);
				after = source.SubString2(diff);
			}

			source = $"{before}{haveFloat}{after}";

			decimal.TryParse(source, out result);
			return result;
		}

		/// <summary>
		///  把用 try parse 寫成2行的部份 轉成一行
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static double DoubleTryParse(this string source)
		{
			double result = 0;
			double.TryParse(source, out result);
			return result;
		}

		/// <summary>
		///  這個 字串 是不是 數字型 (能用下面的 驗證全數字就行)
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static bool IsNumeric(string source)
		{
			if (string.IsNullOrEmpty(source)) { return false; }

			try
			{
				var i = Convert.ToDecimal(source);
				return true;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " --> " + ex.Message);

				try
				{
					var i = Convert.ToDouble(source);
					return true;
				}
				catch (Exception e)
				{
					System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " --> " + e.Message);

					/*
					    原本在這邊有寫 ToInt32 => 但拿掉
					    原因： 連 Double 和 Decimal 都不行了 ，那 Int 絕對就不可能
					    不要再浪費時間 做 Convert (花的時間很久)
					 */

					return false;
				}
			}
		}

		/// <summary>
		///  無條件捨去至 小數點第幾位
		/// </summary>
		/// <param name="source"></param>
		/// <param name="floatNum"></param>
		/// <returns></returns>
		public static decimal RoundDown(string source, int floatNum = 0)
		{
			decimal result = 0;
			if (string.IsNullOrEmpty(source)) { source = "0"; }
			floatNum = floatNum <= 0 ? 0 : floatNum;

			if (source.Contains("."))
			{
				// 只取到第一個，後面不管
				List<string> data = source.Split('.').ToList();
				string item = data[0];

				if (floatNum > 0)
				{
					// 先幫他補0 (免得其實他的長度不夠)
					item += ".";
					string item1 = data[1];
					int len = item1.Length;
					for (var i = len; i < floatNum; i++)
					{
						item1 += "0";
					}

					// 再用 Substring
					item += item1.SubString2(0, floatNum);
				}

				result = DecimalTryParse(item);
			}
			else
			{
				if (floatNum > 0)
				{
					source += ".";
					for (var i = 0; i < floatNum; i++)
					{
						source += "0";
					}
				}

				result = DecimalTryParse(source);
			}

			return result;
		}

		#endregion

		#region 相關 數字靠右前補0 + 數字靠左後補0 + 小數格式

		/// <summary>
		///  像是 "0   " => "0000"
		///  數字靠右補空白
		///  特例： 如果是 "1234" ， len 給2 => 結果 "34"
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string NumericStringNumberMoveRight(string source, int len)
		{
			string result = string.Empty;
			len = len < 0 ? 0 : len;

			if (!string.IsNullOrEmpty(source))
			{
				result = source.Replace(" ", string.Empty);

				// 必要是全數字
				if (IsAllNumeric(result))
				{
					int diff = len - result.Length;
					if (diff <= 0)
					{
						// 拿後面的碼
						return result.SubString2(result.Length - len, len);
					}
					else
					{
						string space = string.Empty;
						for (var i = 0; i < diff; i++)
						{
							space += "0";
						}

						return space + result;
					}
				}
			}

			return result;
		}

		/// <summary>
		///  像是 "0   " => "0000"
		///  數字靠右補空白
		///  特例： 如果是 "1234" ， len 給2 => 結果 "34"
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string NumericStringNumberMoveRight(int source, int len)
		{
			string item = source.ToString();
			return NumericStringNumberMoveRight(item, len);
		}

		/// <summary>
		///  像是 "   0" => "0000"
		///  數字靠左補空白 (在小數點後的數用的)
		///  特例： 如果是 "1234" ， len 給2 => 結果 "12"
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string NumericStringNumberMoveLeft(string source, int len)
		{
			string result = string.Empty;
			len = len < 0 ? 0 : len;

			if (!string.IsNullOrEmpty(source))
			{
				result = source.Replace(" ", string.Empty);

				// 必要是全數字
				if (IsAllNumeric(result))
				{
					int diff = len - result.Length;
					if (diff <= 0)
					{
						// 拿前面的碼
						return result.SubString2(0, len);
					}
					else
					{
						string space = string.Empty;
						for (var i = 0; i < diff; i++)
						{
							space += "0";
						}

						return result + space;
					}
				}
			}

			return result;
		}

		/// <summary>
		///  像是 "   0" => "0000"
		///  數字靠左補空白 (在小數點後的數用的)
		///  特例： 如果是 "1234" ， len 給2 => 結果 "12"
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static string NumericStringNumberMoveLeft(int source, int len)
		{
			string item = source.ToString();
			return NumericStringNumberMoveLeft(item, len);
		}

		/// <summary>
		///  將 deciaml 轉換為 格式 
		///  例： YYY PIC 9(3)V9
		///  給的值是 22
		///  出來 的要是 0220
		///  給的值是 12.33
		///  出來的要是 012.3
		/// </summary>
		/// <param name="source"></param>
		/// <param name="numLen"></param>
		/// <param name="floatLen"></param>
		/// <returns></returns>
		public static string NumericStringDecimalFormat(string source, int numLen, int floatLen)
		{
			string result = string.Empty;
			numLen = numLen < 0 ? 0 : numLen;
			floatLen = floatLen < 0 ? 0 : floatLen;

			if (!string.IsNullOrEmpty(source))
			{
				// 因為要直接拿小數點，所以給他加個 . 就絕對不會爆錯了
				source += ".0";
				List<string> datas = source.Split('.').ToList();

				// 數字靠右 前補0，小數靠左 後補0
				result = NumericStringNumberMoveRight(datas[0], numLen) +
				    "." +
				    NumericStringNumberMoveLeft(datas[1], floatLen);
			}

			return result;
		}

		/// <summary>
		///  將 deciaml 轉換為 格式 
		///  例： YYY PIC 9(3)V9
		///  給的值是 22
		///  出來 的要是 0220
		///  給的值是 12.33
		///  出來的要是 012.3
		/// </summary>
		/// <param name="source"></param>
		/// <param name="numLen"></param>
		/// <param name="floatLen"></param>
		/// <returns></returns>
		public static string NumericStringDecimalFormat(decimal source, int numLen, int floatLen)
		{
			string item = source.ToString();
			return NumericStringDecimalFormat(item, numLen, floatLen);
		}

		#endregion

		#region 相關驗證(是否全數字、是否全英文、是否全英數字、是否全是中文) => 用 StringHelper.xxx() 的方式來用

		/// <summary>
		///  是否是全數字
		/// </summary>
		public static bool IsAllNumeric(string source)
		{
			bool result = false;

			try
			{
				if (string.IsNullOrEmpty(source)) { return false; }
				source = source.Trim();
				result = Regex.IsMatch(source, @"^[0-9]+$");
				return result;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " --> " + ex.Message);
				return false;
			}
		}

		/// <summary>
		///  是否是全英文
		/// </summary>
		public static bool IsAllEnglish(string source)
		{
			bool result = false;

			try
			{
				if (string.IsNullOrEmpty(source)) { return false; }
				source = source.Trim();
				result = Regex.IsMatch(source, @"^[a-zA-Z]+$");
				return result;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " --> " + ex.Message);
				return false;
			}
		}

		/// <summary>
		///  是否是全英文或數字
		/// </summary>
		public static bool IsAllEnglishOrNumeric(string source)
		{
			bool result = false;

			try
			{
				if (string.IsNullOrEmpty(source)) { return false; }
				source = source.Trim();
				result = Regex.IsMatch(source, @"^[a-zA-Z0-9]+$");
				return result;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + " --> " + ex.Message);
				return false;
			}
		}

		/// <summary>
		///  是否 包含 中文 (符號不算中文 => 用 ASCII > 126 來判斷)
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static bool IsContainChinese(string source)
		{
			bool result = false;

			if (!string.IsNullOrEmpty(source))
			{
				foreach (var currentChar in source)
				{
					// 這邊不管符號的 => 所以查 ASCII碼時發現 126 是底
					// => 所以如果 ASCII > 126 就都視為中文
					int currentCharNum = currentChar.IntTryParse();
					if ((currentCharNum <= 0) || (currentCharNum > 126))
					{
						result = true;
					}
				}
			}

			return result;
		}

		/// <summary>
		///  是否 全部 中文 (符號不算中文 => 用 ASCII > 126 來判斷)
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static bool IsAllChinese(string source)
		{
			bool result = true;

			if (!string.IsNullOrEmpty(source))
			{
				foreach (var currentChar in source)
				{
					// 這邊不管符號的 => 所以查 ASCII碼時發現 126 是底
					// => 所以如果 ASCII > 126 就都視為中文
					int currentCharNum = currentChar.IntTryParse();
					if ((currentCharNum > 0) && (currentCharNum <= 126))
					{
						result = false;
					}
				}

				return result;
			}
			else
			{
				return false;
			}
		}



		#endregion

		#region JSON 處理

		/// <summary>
		///  轉成 JSON 字串
		/// </summary>
		/// <param name="obj"></param>
		/// <returns></returns>
		public static string ToJSON(object obj)
		{
			string error = string.Empty;

			// 這邊可以寫成一個 Helper
			try
			{
				error = JsonConvert.SerializeObject(obj);
			}
			catch (Exception e)
			{
				System.Diagnostics.Debug.WriteLine(e.Message);
			}

			return error;
		}

		/// <summary>
		///  JSON 格式化
		/// </summary>
		/// <param name="str"></param>
		/// <returns></returns>
		static string FormatJson(string str)
		{
			try{
				//格式化json字串
				JsonSerializer serializer = new JsonSerializer();
				TextReader tr = new StringReader(str);
				JsonTextReader jtr = new JsonTextReader(tr);
				object obj = serializer.Deserialize(jtr);
				if (obj != null)
				{
					StringWriter textWriter = new StringWriter();
					JsonTextWriter jsonWriter = new JsonTextWriter(textWriter)
					{
						Formatting = Formatting.Indented,
						Indentation = 4,
						IndentChar = ' '
					};
					serializer.Serialize(jsonWriter, obj);
					return textWriter.ToString();
				}
				else
				{
					return str;
				}
			}
			catch (Exception ex){
				return str;
			}
		}

		#endregion

		#region 得到字串長度

		/// <summary>
		/// 算出 字串去掉 特定 char 後 的長度
		/// (預設是 空白 => 即預設是去掉 空白後的長度)
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static int RemoveSpecificCharLength(string source, char filter = ' ')
		{
			int result = 0;
			if (!string.IsNullOrEmpty(source))
			{
				for (var i = 0; i < source.Length; i++)
				{
					char c = source[i];
					if (c != filter)
					{
						result++;
					}
				}
			}

			return result;
		}

		/// <summary>
		/// 算出 字串去掉 特定 string 後 的長度
		/// (預設是 空白 => 即預設是去掉 空白後的長度)
		/// </summary>
		/// <param name="source"></param>
		/// <returns></returns>
		public static int RemoveSpecificCharLength(string source, string filter = " ")
		{
			int result = 0;
			if (!string.IsNullOrEmpty(source))
			{
				return source.Replace(filter, string.Empty).Length;
			}

			return result;
		}

		#endregion

		#region 轉全形

		/// <summary>
		/// 數字半形轉全形
		/// 欄位
		///Hiragana 32 
		///將字串中的片假名字元轉換為平假名字元。 僅適用於日本地區設定。 這個成員相當於 Visual Basic 常數 vbHiragana。
		///Katakana 16 
		///將字串中的平假名字元轉換為片假名字元。 僅適用於日本地區設定。 這個成員相當於 Visual Basic 常數 vbKatakana。
		///LinguisticCasing 1024 
		///將字串大小寫從檔案系統規則轉換為語言規則。 這個成員相當於 Visual Basic 常數 vbLinguisticCasing。
		///Lowercase 2 
		///將字串轉換為小寫字元。 這個成員相當於 Visual Basic 常數 vbLowerCase。
		///Narrow 8 
		///將字串中的寬(雙位元組) 字元轉換為窄(單一位元組) 字元。 適用於亞洲地區設定。 這個成員相當於 Visual Basic 常數 vbNarrow。
		///None 0 
		///沒有執行任何轉換。
		///ProperCase 3 
		///將字串中每個字的第一個字母轉換為大寫。 這個成員相當於 Visual Basic 常數 vbProperCase。
		///SimplifiedChinese 256 
		///將字串轉換為簡體中文字元。 這個成員相當於 Visual Basic 常數 vbSimplifiedChinese。
		///TraditionalChinese 512 
		///將字串轉換為繁體中文字元。 這個成員相當於 Visual Basic 常數 vbTraditionalChinese。
		///Uppercase 1 
		///將字串轉換為大寫字元。 這個成員相當於 Visual Basic 常數 vbUpperCase。
		///Wide 4 
		///將字串中的窄(單一位元組) 字元轉換為寬(雙位元組) 字元。 適用於亞洲地區設定。 這個成員相當於 Visual Basic 常數 vbWide。 轉換可能會使用正規化格式 C，即使輸入字元已經是全形也一樣。 例如，字串 "は゛" (這已經是全形) 會正規化為 "ば"。 請參閱 Unicode 正規化格式。
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public static string StrConv(this string input, int type = 4)
		{
			/*
				要用前 去 參考/加入參考/組件/Microsoft.VisualBasic 裝上
				再 using Microsoft.VisualBasic
			 */

			string result = string.Empty;

			if (!string.IsNullOrEmpty(input))
			{
				result = Strings.StrConv(input, (VbStrConv)type);
			}

			return result;
		}

		#endregion
	
		#region 得到 config 的資料

		/// <summary>
        ///  取到 AppSetting
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetAppSetting(string key)
        {
            string result = string.Empty;

            try
            {
                var o = ConfigurationManager.AppSettings[key];
                result = o == null ? string.Empty : o.ToString();
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }

        /// <summary>
        ///  取到 ConnectionStrings
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetConnectionStrings(string key)
        {
            string result = string.Empty;

            try
            {
                var o = ConfigurationManager.ConnectionStrings[key];
                result = o == null ? string.Empty : o.ToString();
                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }

		#endregion
	}
}
