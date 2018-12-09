using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Http;
using System.Web.Helpers;

using AutoMapper;
using AutoMapper.Data;

using MVC_test.Controllers;
using System.Web.SessionState;  //要引用這一隻

using System.Globalization;

namespace MVC_test
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            //api一定要放在這個位置，不然會錯誤
            //要先去NuGet裝上Microsoft.AspNet.WebApi.WebHost 這個
            GlobalConfiguration.Configure(WebApiConfig.Register);

            RouteConfig.RegisterRoutes(RouteTable.Routes);


            //先裝AutoMapper.Data
            //設定初使設定（讓以後都可以用）
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<MemberController.Member, MemberController.VIP>().ReverseMap();  //加入reverseMap是讓他可以互轉（不然就是寫2行）
                cfg.AddDataReaderMapping();
            });


            //設定API只傳json
            //GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
        }

        //Web API開啟Session用的
        protected void Application_PostAuthorizeRequest()
        {
            HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        }
    }
}
