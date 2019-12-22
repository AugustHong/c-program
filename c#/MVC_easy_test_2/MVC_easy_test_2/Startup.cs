using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Microsoft.Owin;
using Microsoft.Owin.Security.Cookies;
using Microsoft.Owin.Security.Google;
using Owin;

using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;

/* 這是身份驗證方法二所需要的，這邊因為仍是實作方法一，故會註解*/

#region 驗證方法二要用的，因實作方法一，故先註解
[assembly: OwinStartupAttribute(typeof(MVC_easy_test_2.Startup))]
namespace MVC_easy_test_2
{
	public class Startup
	{
		public void Configuration(IAppBuilder app)
		{
			ConfigureAuth(app);
		}

		// 如需設定驗證的詳細資訊，請瀏覽 http://go.microsoft.com/fwlink/?LinkId=301864
		public void ConfigureAuth(IAppBuilder app)
		{

			#region 基本型(用這個即可)
			//var cookieAuthOptions = new CookieAuthenticationOptions
			//{
			//	AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
			//	CookieHttpOnly = true,
			//	ExpireTimeSpan = TimeSpan.FromMinutes(60),
			//	SlidingExpiration = true,
			//	CookieSecure = CookieSecureOption.SameAsRequest,
			//	LoginPath = new PathString("/Authorize/Login")    //預設路徑
			//};
			//app.UseCookieAuthentication(cookieAuthOptions);
			#endregion

			#region 配合 Mircosoft的Account所使用的
			// 設定資料庫內容、使用者管理員和登入管理員，以針對每個要求使用單一執行個體
			//app.CreatePerOwinContext(ApplicationDbContext.Create);
			//app.CreatePerOwinContext<ApplicationUserManager>(ApplicationUserManager.Create);
			//app.CreatePerOwinContext<ApplicationSignInManager>(ApplicationSignInManager.Create);

			//// 讓應用程式使用 Cookie 儲存已登入使用者的資訊
			//// 並使用 Cookie 暫時儲存使用者利用協力廠商登入提供者登入的相關資訊；
			//// 在 Cookie 中設定簽章
			//app.UseCookieAuthentication(new CookieAuthenticationOptions
			//{
			//	AuthenticationType = DefaultAuthenticationTypes.ApplicationCookie,
			//	LoginPath = new PathString("/Authorize/Login"),   //預設登入路徑
			//	Provider = new CookieAuthenticationProvider
			//	{
			//		// 讓應用程式在使用者登入時驗證安全性戳記。
			//		// 這是您變更密碼或將外部登入新增至帳戶時所使用的安全性功能。  
			//		OnValidateIdentity = SecurityStampValidator.OnValidateIdentity<ApplicationUserManager, ApplicationUser>(
			//		validateInterval: TimeSpan.FromMinutes(30),
			//		regenerateIdentity: (manager, user) => user.GenerateUserIdentityAsync(manager))
			//	}
			//});
			//app.UseExternalSignInCookie(DefaultAuthenticationTypes.ExternalCookie);

			//// 讓應用程式在雙因素驗證程序中驗證第二個因素時暫時儲存使用者資訊。
			//app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

			//// 讓應用程式記住第二個登入驗證因素 (例如電話或電子郵件)。
			//// 核取此選項之後，將會在用來登入的裝置上記住登入程序期間的第二個驗證步驟。
			//// 這類似於登入時的 RememberMe 選項。
			//app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);

			/*
			// 註銷下列各行以啟用利用協力廠商登入提供者登入
			//app.UseMicrosoftAccountAuthentication(
			//    clientId: "",
			//    clientSecret: "");

			//app.UseTwitterAuthentication(
			//   consumerKey: "",
			//   consumerSecret: "");

			//app.UseFacebookAuthentication(
			//   appId: "209215126257908",
			//   appSecret: "80c8dfd767ffc3ac2944e04481d28d7e");

			//app.UseGoogleAuthentication(new GoogleOAuth2AuthenticationOptions()
			//{
			//    ClientId = "",
			//    ClientSecret = ""
			//});
			*/
			#endregion
		}
	}
}
#endregion