using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

/*

	權限控管：

	參考網圵： http://kevintsengtw.blogspot.com/2013/11/aspnet-mvc.html

	步驟：
	1. 先去寫Controller(主要是LoginProcess，次要是Login、Logout)。再來就是 權限的Attribute的設定
	2. 去產生頁面
	3.在Web.config的 System.Web 區段裡加入 Authentication 的設定(初使登入頁的設定)
	4.為了要取得登入者資料，去Global.asax新增Application_AuthenticateRequest方法
	5.寫一隻Helper取得使用者名稱(要用在第6點)
	6.在_Layout.cshtml中 加入 依照使用者是否有登入

	如果要做客製化的權限控管，就新增一個Attribute且去繼承AuthorizeAttribute即可在裡面自己寫喔

*/

namespace MVC_easy_test_2.Controllers
{
	[Authorize]   //加入這個就是權限的Attribute(指的是要有登入才能進來)
	 //不要再加上[AllowAnonymous]，同時這2個屬性在一起會變成都可以隨便進入
	public class AuthorizeController : Controller
	{
		[Authorize(Roles = "1")]   //設定可以進入的權限(寫在這裡的會比上方[AllowAnonymous] 還強，所以是依照這邊的權限設定)
		public string Index()
		{
			return "這是只有權限1可以進來的";
		}

		[Authorize(Roles = "2")]   //設定可以進入的權限(寫在這裡的會比上方[AllowAnonymous] 還強，所以是依照這邊的權限設定)
		public string Index2()
		{
			return "這是只有權限2可以進來的";
		}

		[Authorize(Roles = "2,3")]   //設定可以進入的權限(寫在這裡的會比上方[AllowAnonymous] 還強，所以是依照這邊的權限設定)
		public string Index3()
		{
			return "這是只有權限2或3可以進來的";
		}

		[Authorize]
		public ActionResult ListPage()
		{
			return View();
		}

		/* 這邊 才是主要的*/

		//登入的頁面
		[AllowAnonymous]   //所有人都可以進入(不用登入)
		public ActionResult Login()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		[AllowAnonymous]
		//登入頁面(Post進來的資料) =>照理來說是輸入帳密，且偵錯。但這裡是測試的，所以就直接輸入權限即可
		public ActionResult Login(string roles)
		{
			//進行登入
			LoginProcess(roles, roles, false);

			return RedirectToAction("ListPage");
		}

		/// <summary>
		/// 這邊是在做登入後的儲存記錄(要撈取登入者是誰，還有驗證機制都是靠這邊的)
		///  => 下面的參數自己決定(最重要的是roles的部份，這決定了他可不可以進入)
		/// </summary>
		/// <param name="name"></param>
		/// <param name="roles"></param>
		/// <param name="isRemeber"></param>
		private void LoginProcess(string name, string roles, bool isRemeber)
		{
			//這邊基本上都是照寫(但裡面的值有些可以自訂)
			var ticket = new FormsAuthenticationTicket(
			    version: 1,
			    //要呈現的名字(可隨便設)
			    name: name,
			    //基本上拿今天
			    issueDate: DateTime.Now,
			    //期限
			    expiration: DateTime.Now.AddMinutes(50),
			    //是否要記住
			    isPersistent: isRemeber,
			    //他的權限("1" 或 "1,2" 這種的型式)
			    userData: roles,
			    //照寫
			    cookiePath: FormsAuthentication.FormsCookiePath
			    );

			//以下也都是照寫即可
			string encryptedTicket = FormsAuthentication.Encrypt(ticket);
			HttpCookie cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
			Response.Cookies.Add(cookie);
		}

		//登出
		[Authorize]
		public ActionResult Logout()
		{
			//先執行登出
			FormsAuthentication.SignOut();

			//清除所有的 session
			Session.RemoveAll();

			//建立一個同名的 Cookie 來覆蓋原本的 Cookie
			HttpCookie cookie1 = new HttpCookie(FormsAuthentication.FormsCookieName, "");
			cookie1.Expires = DateTime.Now.AddYears(-1);
			Response.Cookies.Add(cookie1);

			//建立 ASP.NET 的 Session Cookie 同樣是為了覆蓋
			HttpCookie cookie2 = new HttpCookie("ASP.NET_SessionId", "");
			cookie2.Expires = DateTime.Now.AddYears(-1);
			Response.Cookies.Add(cookie2);

			//回到Login頁面
			return RedirectToAction("Login");
		}
	}
}