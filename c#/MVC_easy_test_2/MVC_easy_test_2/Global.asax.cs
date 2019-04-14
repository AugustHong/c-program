using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;     //要記得引用（多國語系所需要的）
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;         //要記得引用（多國語系所需要的）
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;

namespace MVC_easy_test_2
{
	public class MvcApplication : System.Web.HttpApplication
	{
		private static Logger logger = LogManager.GetCurrentClassLogger();

		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			RouteConfig.RegisterRoutes(RouteTable.Routes);

			#region 驗證方法二要用的
			//AntiForgeryConfig.UniqueClaimTypeIdentifier = ClaimTypes.NameIdentifier;
			#endregion

			//寫Web上的排程(一定要在Application_Start這隻Function中)
			//啟用多執行緒(TaskLoop是下方的函式)
			ThreadStart tsTask = new ThreadStart(TaskLoop);
			Thread MyTask = new Thread(tsTask);
			MyTask.Start();
		}


		/// <summary>
		/// 執行排程內的Loop
		/// </summary>
		static void TaskLoop()
		{
			while (true)
			{
				// 執行的動作都放這(如果有好幾個就寫好幾個吧)，這邊只先寫一個即可
				ToDo();

				// 等待幾秒後繼續執行排程(這裡是寫500秒)
				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(500));
			}
		}

		//執行的動作，這邊只是示範所以就只寫log即可
		static void ToDo()
		{
			//這邊是做測試用所以才加的
			Logger logger = LogManager.GetCurrentClassLogger();

			logger.Info("排程正在執行中……");
	}


		//多國語系要用的
		protected void Application_PostMapRequestHandler(Object sender, EventArgs e)
		{
			try
			{
				var language = (string)HttpContext.Current.Request.RequestContext.RouteData.Values["lang"] ?? "zh-TW";
				Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
			}
			catch (Exception) { }
		}

		//權限控管所要用的
		#region 驗證方法一用的
		protected void Application_AuthenticateRequest(Object sender, EventArgs e)
		{
			//是否有使用者
			bool hasUser = HttpContext.Current.User != null;
			//有使用者，且登入者就是使用者
			bool isAuthenticated = hasUser && HttpContext.Current.User.Identity.IsAuthenticated;
			//身份是否能被辦識
			bool isIdentity = isAuthenticated && HttpContext.Current.User.Identity is FormsIdentity;

			if (isIdentity)
			{
				//取得表單認證目前這位使用者的身份
				FormsIdentity id = (FormsIdentity)HttpContext.Current.User.Identity;

				//取得FormsAuthenticationTicket物件(在LoginProcess中設定的那個)
				FormsAuthenticationTicket ticket = id.Ticket;

				//取得 角色權限資料
				string[] roles = ticket.UserData.Split(',');

				//賦予該使用者新的身份(含角色資料)
				HttpContext.Current.User = new GenericPrincipal(id, roles);
			}
		}
		#endregion

		//依據網頁的錯誤導去不同頁面(例如：出現404時，要導去哪一頁)
		protected void Application_Error(object sender, EventArgs e)
		{
			//得到錯誤訊息
			var exception = Server.GetLastError();
			var httpException = exception as HttpException;
			logger.Error(exception);

			//得到錯誤代碼
			Response.StatusCode = httpException.GetHttpCode();

			Response.Clear();
			Server.ClearError();

			if (httpException != null)
			{
				var httpContext = HttpContext.Current;

				httpContext.RewritePath("~/Errors/", false);

				// MVC 3 running on IIS 7+
				//這邊在傳要轉去哪一頁
				//不過這只是寫個大概，目前未實作 ErrorController => 故不會生效
				//如果下次要使用時，要把View和Controller都建好
				if (HttpRuntime.UsingIntegratedPipeline)
				{
					switch (Response.StatusCode)
					{
						case 302:
							httpContext.Server.TransferRequest("~/home", true);
							break;

						case 403:
							httpContext.Server.TransferRequest("~/Error/Http403", true);
							break;

						case 404:
							httpContext.Server.TransferRequest("~/Error/HttpNotFound", true);
							break;

						default:
							httpContext.Server.TransferRequest("~/Error/Index", true);
							break;
					}
				}
				else
				{
					switch (Response.StatusCode)
					{
						case 403:
							httpContext.RewritePath(string.Format("~/Error/Http403", true));
							break;

						case 404:
							httpContext.RewritePath(string.Format("~/Error/Http404", true));
							break;

						default:
							httpContext.RewritePath(string.Format("~/Error/index", true));
							break;
					}

					IHttpHandler httpHandler = new MvcHttpHandler();
					httpHandler.ProcessRequest(httpContext);
				}
			}
		}
	}
}
