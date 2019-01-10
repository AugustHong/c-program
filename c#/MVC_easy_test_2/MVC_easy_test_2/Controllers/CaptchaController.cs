using MVC_easy_test_2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using CaptchaLib; //使用驗證碼（去NuGet裝上 MvcCaptchaLib） => View Controller Model都要加東西
using CaptchaMvc.HtmlHelpers;  //使用驗證碼（去NuGet裝上CaptchaMvc）=> 只要View Controller 加即可

/*
	參考網址：https://github.com/leon737/MvcCaptchaLib  較不主流

	較為主流的是NuGet中的CaptchaMvc
*/

namespace MVC_easy_test_2.Controllers
{
    public class CaptchaController : Controller
    {
		public ActionResult Index()
		{
			return View();
		}

		[HttpPost]
		[ValidateAntiForgeryToken]
		public ActionResult Index(CheckModel formData)
		{
			//CaptchaMvc的由這個判斷（如果失敗會出現這個錯誤訊息）
			this.IsCaptchaValid("Captcha is not valid");

			//MvcCaptchaLib的直接由ModelState.IsValid判斷即可
			TempData["Message"] = ModelState.IsValid ? "成功" : "失敗";

			return View();
		}


		//取得驗證碼（由View呼叫） => MvcCaptchaLib要多一個function
		public ActionResult GetCaptcha()
		{
			//取得驗證碼東西
			return this.Captcha();

			//可以製作半客製化的（比較可惜的是只能4碼而已）
			//第一個參數是 CaptchImage類型（裡面可以用 new CaptchaImage{ 屬性 = 值} 來設定）
			//CaptchaImage內容請參照 https://github.com/leon737/MvcCaptchaLib/blob/master/CaptchaLib/CaptchaImage.cs
			//第二個參數： 畫質
			//第三個參數 ：寬度   第四個參數：高度
			//return this.Captcha(new CaptchaImage(), 50, 300, 100);
		}
	}
}