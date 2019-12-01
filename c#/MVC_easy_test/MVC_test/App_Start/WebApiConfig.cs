using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace MVC_test
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
			// 加入這行來讓可以取得和回傳 xml 格式
			config.Formatters.XmlFormatter.UseXmlSerializer = true;

			config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}