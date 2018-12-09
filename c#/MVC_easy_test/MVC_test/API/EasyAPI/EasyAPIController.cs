using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


/*
    要做API之前，要先去NuGet裝Microsoft.AspNet.WebApi.Core才能引用System.Web.Http喔
    再來就是在App_Start加入WebApiConfig的檔案，裡面如同RouteConfig一樣是設定路徑的
    再去NuGet裝Microsoft.AspNet.WebApi.WebHost
    最後再Global.asax.cs加入 GlobalConfiguration.Configure(WebApiConfig.Register); 這一行
    加入API用 加入>新增項目>WebAPI來新增
*/

namespace MVC_test.API.EasyAPI
{
    //一般這種沒有寫東西的，路徑為：localhost:xxxx/api/EasyAPI
    public class EasyAPIController : ApiController
    {
        //路徑為：localhost:xxxx/api/EasyAPI/Test?text=
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("Test/")]
        public string Test(string text)
        {
            return text;
        }
    }
}