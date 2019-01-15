using System;
using System.Collections.Generic;
using System.Globalization;     //要記得引用（多國語系所需要的）
using System.Linq;
using System.Threading;         //要記得引用（多國語系所需要的）
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC_easy_test_2
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			RouteConfig.RegisterRoutes(RouteTable.Routes);
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
	}
}
