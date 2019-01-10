using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hong.ReadKMLHelper
{
	class ReadKMLHelper
	{
		/// <summary>
		/// 單純讀檔
		/// </summary>
		/// <param name="filePath">完整檔案路徑</param>
		/// <returns></returns>
		public static string ReadKMLFile(string filePath)
		{
			//接收kml檔的文字
			string result = string.Empty;

			try
			{
				//讀檔  建立檔案串流（@ 可取消跳脫字元 escape sequence）
				StreamReader sr = new StreamReader(@"" + filePath);

				while (!sr.EndOfStream)
				{
					// 每次讀取一行，直到檔尾
					result += sr.ReadLine();
				}

				sr.Close();
			}
			catch
			{
				string errorText = $"讀取檔案 {filePath} 失敗";
				throw new Exception(errorText);
			}

			return result;
		}


		/// <summary>
		/// 讀取基本款kml檔，傳回全部的座標點（只找座標點，不管其他標籤的影響）
		/// </summary>
		/// <param name="filePath">檔案路徑</param>
		/// <returns></returns>
		public static List<string> FileToGetAllCoordinates(string filePath)
		{
			//傳回的List<string>
			List<string> result = new List<string>();

			//讀檔
			string readContent = ReadKMLHelper.ReadKMLFile(filePath);

			//執行把文字轉成座標陣列
			result = ReadKMLHelper.StringToListCoordinates(readContent);

			return result;
		}

		/// <summary>
		/// 讀取進階版的kml（有outer和inner且有多個placemark的資料）
		/// </summary>
		/// <param name="filePath">完整檔案路徑</param>
		/// <param name="errorList">錯誤列表</param>
		/// <returns></returns>
		public static List<Placemark> FileToStringAdvanced(string filePath, ref List<string> errorList)
		{
			List<Placemark> result = new List<Placemark>();

			//讀檔
			string readContent = ReadKMLHelper.ReadKMLFile(filePath);

			//先用Placemark(kml標籤的節點)，因為圖形有可能不是只有一個Placemark節點(例如：媽祖、澎湖   ……多島嶼組成的)，將所有資料切成List（各節點資料）
			List<string> totalPlacemarkText = ReadKMLHelper.StringSpecialSplie(readContent, "Placemark");

			//確定有Placemark標籤存在
			if (totalPlacemarkText.Count() > 0)
			{
				//跑過所有的Placemark標籤內的資料
				foreach (var placemarkText in totalPlacemarkText)
				{
					//<name>、<description>在KML中不是必需要有的，故為防止後面的程式出現例外。同上述把它用標籤進行切割。各自得出其切割後的資料
					List<string> n = ReadKMLHelper.StringSpecialSplie(placemarkText, "name");
					List<string> d = ReadKMLHelper.StringSpecialSplie(placemarkText, "description");
					List<string> s = ReadKMLHelper.StringSpecialSplie(placemarkText, "styleUrl");

					//組出Placemark類型
					Placemark placemark = new Placemark
					{
						//由上述的原因，故AreaText、Description都要判斷是否有標籤。如果沒有就給空字串
						AreaTitle = n.Count() > 0 ? n[0] : "",
						Description = d.Count() > 0 ? d[0] : "",
						StyleUrl = s.Count() > 0 ? s[0] : "",

						Polygon = new List<Polygon>(),
						//所有<Point>切出來的資料
						Point = ReadKMLHelper.StringSpecialSplie(placemarkText, "Point")
					};

					#region 點座標
					if(placemark.Point.Count() > 0)
					{
						//跑過每筆資料，並且把他化成座標格式
						for(var i = 0; i < placemark.Point.Count(); i++)
						{
							try
							{
								//將得來的資料轉成座標格式
								placemark.Point[i] = ReadKMLHelper.StringToListCoordinates(placemark.Point[i])[0];
							}
							catch
							{
								errorList.Add($"檔案 {filePath} 中格式資料有誤在name為 {placemark.AreaTitle}  且 Description為 {placemark.Description} 的資料");
								continue;
							}
						}
					}
					#endregion

					#region 多邊型
					//此Placemark中的所有Polygon資料
					List<string> totalPolygonText = ReadKMLHelper.StringSpecialSplie(placemarkText, "Polygon");

					//確定此Placemark中有Polygon類型
					if (totalPolygonText.Count() > 0)
					{
						//跑過此Placemark中的所有Polygon資料
						foreach (var polygonText in totalPolygonText)
						{
							try
							{
								//組出Polygon類型
								Polygon p = new Polygon
								{
									//這邊只是得到outer和inner的資料（並非我們要的座標資料）
									OuterBoundaryIs = ReadKMLHelper.StringSpecialSplie(polygonText, "outerBoundaryIs")[0],
									InnerBoundaryIs = ReadKMLHelper.StringSpecialSplie(polygonText, "innerBoundaryIs")
								};

								//將得來的outer資料轉成座標資料
								p.OuterBoundaryIs = ReadKMLHelper.StringToListCoordinates(p.OuterBoundaryIs)[0];

								//將得來的inner資料轉成座標資料
								if (p.InnerBoundaryIs.Count() > 0)
								{
									for (var i = 0; i < p.InnerBoundaryIs.Count(); i++)
									{
										p.InnerBoundaryIs[i] = ReadKMLHelper.StringToListCoordinates(p.InnerBoundaryIs[i])[0];
									}
								}

								//加入資料
								placemark.Polygon.Add(p);
							}
							catch
							{
								errorList.Add($"檔案 {filePath} 中格式資料有誤在name為 {placemark.AreaTitle}  且 Description為 {placemark.Description} 的資料");
								continue;
							}
						}
					}
					#endregion

					//加入資料
					result.Add(placemark);
				}
			}

			return result;
		}


		/// <summary>
		/// 將包成coordinates的一大串文字變成List<string>的座標list
		/// </summary>
		/// <param name="text">輸入文字</param>
		/// <returns></returns>
		public static List<string> StringToListCoordinates(string text)
		{
			List<string> result = new List<string>();

			//先把要分割的字串整理好（把<coordinates>和</coordinates>先取代成一個東西，再用他分割）
			text = text.Replace("<coordinates>", "*").Replace("</coordinates>", "*");

			//進行分割（只要拿奇數位的資料即可）
			//因為  ....<coordinates>座標</coordinates>....<coordinates>座標</coordinates>.....
			//分割出來就是string[] = {.... , 座標 , .... , 座標, ....}  =>string[1] 和 string[3]才是要拿的
			string[] coordinates = text.Split('*');

			for (int i = 1; i < coordinates.Length; i = i + 2)
			{
				//做座標轉換並加入資料
				var c = coordinates[i].Replace(",", "*").Replace(" ", ",").Replace("*", " ");

				//計算值（因為可能在分割時前面有空白，導致資料變成 ,121 24, 121 25這種） =>且還可能會更多,在前面
				int count = 0;

				for(var j = 0; j < c.Length; j++)
				{
					//每次取一個資料
					var s = c.Substring(j, 1);

					if(s == ",")
					{
						//如果是","的話就增加count
						count++;
					}
					else
					{
						//反之，離開
						break;
					}
				}

				//擷取字串（把前面的,拿掉）
				c = c.Substring(count);

				result.Add(c);
			}

			return result;
		}


		/// <summary>
		/// 做特殊的分割（同StringToListCoordinates之大部份功能，但是是for outerBoundis、innerBoundis這種的）
		/// xml  http  這種對稱式的都可以用這個
		/// </summary>
		/// <param name="text">輸入文字</param>
		/// <param name="splitText">分割字元</param>
		/// <returns></returns>
		public static List<string> StringSpecialSplie(string text, string splitText)
		{
			List<string> result = new List<string>();

			//先把要分割的字串整理好（把<分割文字>和</分割文字>先取代成一個東西，再用他分割）
			text = text.Replace("<" + splitText + ">", "*").Replace("</" + splitText + ">", "*");

			//進行分割（只要拿奇數位的資料即可）
			//因為  ....<分割文字>資料</分割文字><分割文字>資料</分割文字>.....
			//分割出來就是string[] = {.... , 資料 , .... , 資料, ....}  =>string[1] 和 string[3]才是要拿的
			string[] datas = text.Split('*');

			for (int i = 1; i < datas.Length; i = i + 2)
			{
				//做座標轉換並加入資料
				result.Add(datas[i]);
			}

			return result;
		}
	}

	/// <summary>
	/// kml中的<Placemark>中的資料
	/// </summary>
	public class Placemark
	{

		public Placemark()
		{
			Polygon = new List<Polygon>();
		}

		/// <summary>
		/// 標題名稱（拿<name>的資料）
		/// </summary>
		public string AreaTitle { get; set; }


		/// <summary>
		/// 描述（拿<description>的資料）
		/// </summary>
		public string Description { get; set; }


		/// <summary>
		/// 此地圖的styleUrl
		/// </summary>
		public string StyleUrl { get; set; }

		/// <summary>
		/// 多個多邊型資料
		/// </summary>
		public List<Polygon> Polygon { get; set; }


		/// <summary>
		/// 多個點資料
		/// </summary>
		public List<string> Point { get; set; }
	}


	/// <summary>
	/// kml中的<Polygon>中的資料
	/// </summary>
	public class Polygon
	{
		public Polygon()
		{
			InnerBoundaryIs = new List<string>();
		}

		/// <summary>
		/// outerBoundaryIs
		/// </summary>
		public string OuterBoundaryIs { get; set; }

		/// <summary>
		/// 多個innerBoundaryIs
		/// </summary>
		public List<string> InnerBoundaryIs { get; set; }
	}
}
