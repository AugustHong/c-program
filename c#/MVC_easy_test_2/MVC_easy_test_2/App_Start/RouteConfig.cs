using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC_easy_test_2
{
	public class RouteConfig
	{
		public static void RegisterRoutes(RouteCollection routes)
		{
			routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

			//因為多國語系，所以路徑改為  語系/Controller/Action
			//預設沒有語系時的路徑（因為語系如果是空的=>預設中文）
			routes.MapRoute(
					name: "Localized",
					url: "{lang}/{controller}/{action}/{id}",
					constraints: new { lang = @"(\w{2})|(\w{2}-\w{2})" },
					defaults: new { controller = "Captcha", action = "Index", id = UrlParameter.Optional }
			);

			
			routes.MapRoute(
				name: "Default",
				url: "{controller}/{action}/{id}",
				defaults: new { controller = "Captcha", action = "Index", id = UrlParameter.Optional }
			);
		}
	}
}
