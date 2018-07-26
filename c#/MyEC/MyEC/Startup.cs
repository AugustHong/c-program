using Microsoft.Owin;
using Owin;
using System.Web;   //要用cookies要加這個喔
using System;

[assembly: OwinStartupAttribute(typeof(MyEC.Startup))]
namespace MyEC
{
    public partial class Startup
    {

        public void Configuration(IAppBuilder app)
        {

            ConfigureAuth(app);
        }
    }
}
