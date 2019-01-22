using System;
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

namespace MVC_test.API.BaseAPI
{
	public class BaseAPIController : ApiController
	{
		//如果是使用Get這個的話，那路徑就不一樣了（預設上是不用加Action名，但如果仍沒反應那就得輸入localhost:xxxx/api/BaseAPI/Get?text= 這種了）
		//路徑為 localhost:xxxx/api/BaseAPI?text=
		//有沒有發現，不用加上Action的名字而是直接接參數了（也可以自己創一個api的就是這些基本mathod（新增專案>asp.net>選擇web api即可玩玩看）
		//或者進入 https://github.com/hitler11319/c-program/blob/master/c%23/WebAPI_test/WebAPI_test/Controllers/EventLogApiController.cs 看即可
		public string Get(string text)
		{
			return text;
		}


		//如同上面，也要Post的寫法（預設上是不用加Action名，但如果仍沒反應那就得輸入localhost:xxxx/api/BaseAPI/Post 這種了）
		//路徑為 localhost:xxxx/api/BaseAPI
		//Post的記得要寫[FromBody]，如果像其他的有在上面路徑上做的就不用
		public string Post([FromBody]string text)
		{
			return text;
		}


		//回傳值+狀態
		[HttpGet]
		[Route("Return/")]
		public HttpResponseMessage Return()
		{
			try
			{
				return Request.CreateResponse(HttpStatusCode.OK, "ok");
			}catch(Exception ex)
			{
				return Request.CreateResponse(HttpStatusCode.BadRequest, ex.Message.ToString());

				//http : 500的狀態
				//return Request.CreateResponse(HttpStatusCode.InternalServerError, ex.Message.ToString());
			}
		}
	}
}