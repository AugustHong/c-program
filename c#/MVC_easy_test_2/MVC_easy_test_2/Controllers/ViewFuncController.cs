using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/*
	ViewFunc： 就是在 View 中可以使用 C# 的函式

	步驟1： 建立 App_Code 資料夾 (名字要一模一樣)
	步驟2： 在 App_Code 資料夾 新增 ViewFunc.cshtml 的檔案 (名字也一樣)
	步驟3： 實作
*/

namespace MVC_easy_test_2.Controllers
{
    public class ViewFuncController : Controller
    {
        // GET: ViewFunc
        public ActionResult Index()
        {
            return View();
        }
    }
}