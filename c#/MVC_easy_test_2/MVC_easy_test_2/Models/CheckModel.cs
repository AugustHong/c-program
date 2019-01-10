using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

using CaptchaLib;  //使用驗證碼（去NuGet裝上 MvcCaptchaLib）

namespace MVC_easy_test_2.Models
{
	public class CheckModel
	{
		/// <summary>
		/// 帳號
		/// </summary>
		[Required(ErrorMessage = "必填")]
		[Display(Name = "帳號")]
		public string Account { get; set; }


		/// <summary>
		/// 密碼
		/// </summary>
		[Required(ErrorMessage = "必填")]
		[Display(Name = "密碼")]
		public string Passwd { get; set; }


		/// <summary>
		/// 驗證碼(MvcCaptchaLib的)
		/// </summary>
		[Required(ErrorMessage = "必填")]
		[Display(Name = "驗證碼")]
		[ValidateCaptcha]   //加入驗證
		public string Captcha { get; set; }

		/// <summary>
		/// 驗證碼(CaptchaMvc的) =>完全由前端+Controller作業（故不用加上[Required]），其實連此
		/// 欄位都可以拿掉了（因為前端根本沒出現此欄位，只有用他的DisplayName而已）
		/// </summary>
		[Display(Name = "驗證碼")]
		public string Captcha2 { get; set; }
	}
}