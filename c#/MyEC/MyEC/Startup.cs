using Microsoft.Owin;
using Owin;

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
