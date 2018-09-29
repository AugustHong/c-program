using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

using AutoMapper;
using AutoMapper.Data;

using MVC_test.Controllers;

namespace MVC_test
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);


            //先裝AutoMapper.Data
            //設定初使設定（讓以後都可以用）
            Mapper.Initialize(cfg =>
            {
                cfg.CreateMap<MemberController.Member, MemberController.VIP>().ReverseMap();  //加入reverseMap是讓他可以互轉（不然就是寫2行）
                cfg.AddDataReaderMapping();
            });
        }
    }
}
