using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_easy_test_2.Controllers
{
    public class PartialViewController : Controller
    {
        // GET: PartialView
        public ActionResult Index()
        {
            return View();
        }

		// 實作另一種PartialView ， 可以在裡面先處理
		// 不過 視同 一般的一樣， 要有個 和 Action 相同名稱的 cshtml 檔 在這個 Controller 下
		public ActionResult PartialTest()
		{
			ViewBag.Test = "Test";

			// 這邊不是 return View() 喔
			return PartialView();

			/*
				呼叫方法：(一定要用 @{} 包起來)
						@{ 
							Html.RenderAction("PartialTest", "PartialView");
						}
			*/
		}
	}
}