using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_test.Controllers
{
	public class GetControllerNameController : Controller
	{
		// GET: GetControllerName
		public ActionResult Index()
		{

			//在Controller中取得Area、Controller、Action的名稱
			ViewBag.AreaName = ControllerContext.RouteData.DataTokens["area"];   //若沒使用則為null
			ViewBag.ControllerName = ControllerContext.RouteData.Values["controller"];
			ViewBag.ActionName = ControllerContext.RouteData.Values["action"];

			return View();
		}
	}
}