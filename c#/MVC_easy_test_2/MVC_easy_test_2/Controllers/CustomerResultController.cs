using MVC_easy_test_2.Infrastructure.CustomResults;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/*
	客製化Result
	步驟1：先建立自己的客製化Result (詳見 Infrastructure/CustomResults/ExportExcelResult.cs)
	步驟2：引用，並return 它
*/

namespace MVC_easy_test_2.Controllers
{
	/// <summary>
	/// 客製化Result
	/// </summary>
    public class CustomerResultController : Controller
    {
        // GET: CustomerResult
        public ActionResult Index()
        {
			//這裡要使用我們的客製化Result
			//他是一個 變成 下載 Excel 的東西 => 所以要先準備資料
			JArray jObjects = new JArray();    //一個Json 的 物件 Array
			for(int i = 0; i <= 10; i++)
			{
				JObject jo = new JObject();
				jo.Add("流水號", i);
				jObjects.Add(jo);
			}

			//轉成JSON格式(要先ToString)
			DataTable dt = JsonConvert.DeserializeObject<DataTable>(jObjects.ToString());

			//檔名
			string exportFileName = string.Concat("export", DateTime.Now.ToString("yyyyMMddHHmm"),".xlsx");

			//呼叫我們的客製化Result
			return new ExportExcelResult{
				SheetName = "Export",
				FileName = exportFileName,
				ExportData = dt
			};
		}
    }
}