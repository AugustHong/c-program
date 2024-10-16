﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1;
using Microsoft.SqlServer.Types;  //要在NuGet裝喔（名稱就是：Microsoft.SqlServer.Types）
using System.Data.Entity.Spatial;
using System.IO;
using System.Windows.Forms;

namespace WebApplication1.Controllers
{
	//練習中（裡面的只for POLYGON類型而已） => 可以去看 Hong系列中的，有實用版的。這裡是原版
	#region TGOS一系列流程（轉格式=>存檔=>使用）
	/// <summary>
	/// tgos引用資料庫至呼叫至結束的一個process（底下分別為以下流程）
	/// </summary>
	public class TgosKMLProcess
	{
		private static string _kmlFrontTag = "<?xml version='1.0' encoding='UTF-8'?>" +
							     "<kml xmlns = 'http:////www.opengis.net//kml//2.2' ><Document>" +
								    "<Style id='WraDefaultStyle'>" +
									  "<IconStyle id='TpcIconStyle'><color>a1ff00ff</color><scale>0.5</scale></IconStyle>" +
									  "<LableStyle id='EpaLableStyle'><color>ffff0000</color><scale>1.5</scale></LableStyle>" +
									  "<LineStyle id='EpaLineStyle'><color>ffff0000</color><width>3</width></LineStyle>" +
									  "<PolyStyle id='EpaPolyStyle'><color>7f3fe4e4</color><colorMode>random</colorMode></PolyStyle>" +
								    "</Style><Placemark><styleUrl>#WraDefaultStyle</styleUrl>";

		private static string _kmlBehindTag = "</Placemark></Document></kml> ";

		private static string _rootHttpPath = "http:////drone.shengsen.com.tw//";


		/// <summary>
		/// 傳入DbGeometry把它轉成一個給ToKML()用的格式 => 會轉成  類型（點or多邊） 座標
		/// </summary>
		/// <param name="graph">DbGeometry格式的圖型</param>
		/// <returns>string[]格式  有2個值  第一個是類型，第二個是座標資料</returns>
		private static string[] ToKMLFormat(DbGeometry graph)
		{
			string[] result = graph.AsText().Replace("))", "").Replace(", ", "*").Replace(" ", ",").Replace("*", " ").Replace(",((", "*").Split('*');
			return result;
		}

		/// <summary>
		/// 把輸入的資料轉成kml字串（現只有 多邊形  和  點  =>  所以只要做這2個的判斷即可）
		/// </summary>
		/// <param name="type">類型：現只支援 點 和 多邊型</param>
		/// <param name="coordinate">座標資料用串</param>
		/// <param name="name">圖層的名字（或活動名，地區名 皆可）</param>
		/// <returns>傳回一個kml檔樣式的string</returns>
		private static string ToKML(string type, string coordinate, string name)
		{
			string result = string.Empty;
			string nameTag = "<name>" + name + "</name>";

			switch (type)
			{
				//多邊形
				case "POLYGON":
					result = _kmlFrontTag + nameTag + "<Polygon><outerBoundaryIs><LinearRing><coordinates>" + coordinate + "</coordinates></LinearRing></outerBoundaryIs></Polygon>" + _kmlBehindTag;
					break;

				//點
				case "POINT":
					result = _kmlFrontTag + nameTag + "<Point><coordinates>" + coordinate + "</<coordinates></Point>" + _kmlBehindTag;
					break;

				default:
					throw new Exception("您的類型不是此Class能接受的");
			}

			return result;
		}

		/// <summary>
		/// 將剛才上一個mathod產出來的kml檔案字串存成xml檔
		/// </summary>
		/// <param name="kmlTag">上一個mathod產生出來的kml檔案字串</param>
		/// <param name="filePath">檔案路徑，例如：hello.kml（只限kml，但沒做防呆）</param>
		/// <returns>http的基層路徑 + 檔案路徑 = 新url用來呼叫的</returns>
		private static string SaveToKMLFile(string kmlTag, string filePath)
		{
			if (filePath == string.Empty || filePath == null) { return string.Empty; }

			bool fileIsExit = File.Exists(filePath); // 判定檔案是否存在

			//如果存在就先刪掉（因為我們這個是讀一次用的，所以不要久留）
			if (fileIsExit) { File.Delete(filePath); }

			//寫檔
			FileStream output = File.Create(filePath);             // 建立檔案（output是要開檔來寫用的）

			StreamWriter sc = new StreamWriter(output);            //同樣寫檔，不過要用output才不會爆錯
			sc.Write(kmlTag);
			sc.Close();

			return _rootHttpPath + filePath;
		}

		/// <summary>
		/// 把上面3個private的寫成一個流程，一個步驟就可以完成
		/// </summary>
		/// <param name="graph">DbGeometry類型的圖</param>
		/// <returns>http的基層路徑 + 檔案路徑 = 新url用來呼叫的</returns>
		public static string Process(DbGeometry graph, string filePath)
		{
			//第一步
			string[] kmlFormat = ToKMLFormat(graph);

			//第二步
			string kmlTag = ToKML(kmlFormat[0], kmlFormat[1], "FirstKMLlayer");

			//第三步（先讓它選路徑，再做SaveToKMLFile）
			string url = SaveToKMLFile(kmlTag, filePath);

			return url;
		}

	}
	#endregion

	public class geo_test2Controller : Controller
	{
		private geoEntities db = new geoEntities();

		// GET: geo_test2
		public ActionResult Index()
		{
			//geo_test2 re = db.geo_test2.Find(7);
			//string x = TgosKMLProcess.Process(re.graph, "C:\\Users\\偉德\\Desktop\\hello.kml");
			return View(db.geo_test2.ToList());
		}

		// GET: geo_test2/Details/5
		public ActionResult Details(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			geo_test2 geo_test2 = db.geo_test2.Find(id);
			if (geo_test2 == null)
			{
				return HttpNotFound();
			}
			return View(geo_test2);
		}

		// GET: geo_test2/Create
		public ActionResult Create()
		{
			return View();
		}

		// POST: geo_test2/Create
		// 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
		// 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Create([Bind(Include = "id,graph")] geo_test2 geo_test2)
		{
			if (ModelState.IsValid)
			{
				db.geo_test2.Add(geo_test2);
				db.SaveChanges();
				return RedirectToAction("Index");
			}

			return View(geo_test2);
		}

		// GET: geo_test2/Edit/5
		public ActionResult Edit(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			geo_test2 geo_test2 = db.geo_test2.Find(id);
			if (geo_test2 == null)
			{
				return HttpNotFound();
			}
			return View(geo_test2);
		}

		// POST: geo_test2/Edit/5
		// 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
		// 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Edit([Bind(Include = "id,graph")] geo_test2 geo_test2)
		{
			if (ModelState.IsValid)
			{
				db.Entry(geo_test2).State = EntityState.Modified;
				db.SaveChanges();
				return RedirectToAction("Index");
			}
			return View(geo_test2);
		}

		// GET: geo_test2/Delete/5
		public ActionResult Delete(int? id)
		{
			if (id == null)
			{
				return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
			}
			geo_test2 geo_test2 = db.geo_test2.Find(id);
			if (geo_test2 == null)
			{
				return HttpNotFound();
			}
			return View(geo_test2);
		}

		// POST: geo_test2/Delete/5
		[HttpPost, ActionName("Delete")]
		[ValidateAntiForgeryToken]
		public ActionResult DeleteConfirmed(int id)
		{
			geo_test2 geo_test2 = db.geo_test2.Find(id);
			db.geo_test2.Remove(geo_test2);
			db.SaveChanges();
			return RedirectToAction("Index");
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				db.Dispose();
			}
			base.Dispose(disposing);
		}

		//取得是否重疊，如果有重疊會列出其交集區域
		public string GetData()
		{
			string ret = string.Empty;

			List<geo_test2> data = db.geo_test2.ToList();

			for (var i = 0; i < data.Count(); i++)
			{
				for (var j = i + 1; j < data.Count(); j++)
				{
					//Disjoint是否不相交
					if (data[i].graph.Disjoint(data[j].graph) == false)
					{
						//算中心點
						string centerPoint = data[i].graph.Centroid.AsText();

						//Intersection算出交集區域
						string area = data[i].graph.Intersection(data[j].graph).AsText();
						ret += $"id為{data[i].id.ToString()} 和 id為{data[j].id.ToString()}有重疊，相交的區域為{area}" + "<br>";
					}
					else
					{
						ret += $"id為{data[i].id.ToString()} 和 id為{data[j].id.ToString()}沒有重疊" + "<br>";
					}
				}
				ret += "<br>";
			}


			return ret;
		}

		//取得其範圍
		public string GetData2()
		{
			string ret = string.Empty;

			foreach (var item in db.geo_test2)
			{
				ret += $"id為{item.id.ToString()}其座標為{item.graph.AsText()}" + "<br>";
			}

			return ret;
		}

		//將string型別轉換成dbgeometry型別（只for POLYGON的），下面有更好的
		public void StringConvertToDbGeometry()
		{
			//今天是點的話（一個括號即可） => string command = "POINT(120 25)";
			string command = "POLYGON((120 25, 122 25, 122 23, 120 23, 120 25))";
			DbGeometry dg = System.Data.Entity.Spatial.DbGeometry.PolygonFromText(command, 4326);
		}

		/// <summary>
		/// 看此資料是何地圖類型，並且轉出資料（上面函式的進化版）
		/// </summary>
		/// <param name="userInputArea">資料樣式為 121 24, 121 25, 122 25, 122 24, 121 24</param>
		/// <param name="areaType">回傳出來的地圖類型(例如：POLYGON)</param>
		/// <param name="isAddType">是否要加上類型(預設為true)，為false就代表他所輸入的是完整的地圖座標格式</param>
		/// <returns></returns>
		public DbGeometry GetAreaTypeAndConvertToGeometry(string userInputArea, ref string areaType, bool isAddType = true)
		{
			int srid = 4326;

			try
			{
				//點
				DbGeometry result = isAddType ? DbGeometry.PointFromText("POINT(" + userInputArea + ")", srid) : DbGeometry.PointFromText(userInputArea, srid);
				areaType = "POINT";
				return result;
			}
			catch
			{
				try
				{
					//多邊型
					//注意：多邊型一定要比線段還前面判斷(因為 他不會依照封閉區間就把它當作多邊型，
					//故如果拿多邊型的座標資料給線段  他會直接當作成功而回傳線段的類型)
					DbGeometry result = isAddType ? DbGeometry.PolygonFromText("POLYGON((" + userInputArea + "))", srid) : DbGeometry.PolygonFromText(userInputArea, srid);
					areaType = "POLYGON";
					return result;
				}
				catch
				{
					try
					{
						//線
						DbGeometry result = isAddType ? DbGeometry.LineFromText("LINESTRING(" + userInputArea + ")", srid) : DbGeometry.LineFromText(userInputArea, srid);
						areaType = "LINESTRING";
						return result;
					}
					catch
					{
						try
						{
							//多重多邊型（不建議 isAddType為true）
							//一樣多邊型都要比線段前面判斷
							DbGeometry result = isAddType ? DbGeometry.MultiPolygonFromText("MULTIPOLYGON(((" + userInputArea + ")))", srid) : DbGeometry.MultiPolygonFromText(userInputArea, srid);
							areaType = "MULTIPOLYGON";
							return result;
						}
						catch
						{
							try
							{
								//多重線段(不建議isAddType為true)
								DbGeometry result = isAddType ? DbGeometry.MultiLineFromText("MULTILINESTRING((" + userInputArea + "))", srid) : DbGeometry.MultiLineFromText(userInputArea, srid);
								areaType = "MULTILINESTRING";
								return result;
							}
							catch
							{
								try
								{
									//組合圖形（不建議 isAddType為true）
									DbGeometry result = isAddType ? DbGeometry.GeometryCollectionFromText("GEOMETRYCOLLECTION(" + userInputArea + ")", srid) : DbGeometry.GeometryCollectionFromText(userInputArea, srid);
									areaType = "GEOMETRYCOLLECTION";
									return result;
								}
								catch
								{
									throw new Exception("GetAreaType: 地圖座標資料轉換錯誤");
								}
							}
						}
					}
				}
			}
		}


		/// <summary>
		/// 將 SqlGeometry 轉成 地圖座標資料
		/// </summary>
		/// <param name="input"></param>
		/// <returns></returns>
		public string SqlGeometryToString(SqlGeometry input)
		{
			string strGraph = new string(input.STAsText().Value);
			return strGraph;    //出來的樣式是 POLYGON((121 24,121 25,122 25,122 24,121 24))
		}


		/// <summary>
		/// 判斷x是否完全包含在y 中  => x 包含於 y
		/// </summary>
		/// <param name="x">資料</param>
		/// <param name="y">資料</param>
		/// <returns></returns>
		public bool IsAllContain(DbGeometry x, DbGeometry y)
		{
			// x.Difference(y) => X - X交集Y (故如果 x 完全包含於 y 中 = x - x = 空的）
			DbGeometry diff = x.Difference(y);

			//DbGeometry中的ProviderValue值 會呈現 POLYGON(())…等這種資料
			//而如果他是空的就會是 GEOMETRYCOLLECTION EMPTY
			string resultStr = diff.ProviderValue.ToString();

			//因為如果是空的 ProviderValue出來的值會是 GEOMETRYCOLLECTION EMPTY
			//故可以由此判斷
			return resultStr.Contains("EMPTY");
		}
	}
}
