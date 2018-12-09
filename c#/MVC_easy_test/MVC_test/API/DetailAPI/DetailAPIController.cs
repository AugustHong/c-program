using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;


/*
    要做API之前，要先去NuGet裝Microsoft.AspNet.WebApi.Core才能引用System.Web.Http喔
    再來就是在App_Start加入WebApiConfig的檔案，裡面如同RouteConfig一樣是設定路徑的
    再去NuGet裝Microsoft.AspNet.WebApi.WebHost
    最後再Global.asax.cs加入 GlobalConfiguration.Configure(WebApiConfig.Register); 這一行
    加入API用 加入>新增項目>WebAPI來新增
*/

namespace MVC_test.API.DetailAPI
{
    //加入下方這一行，會讓路徑改變
    //沒加前路徑： localhost:xxxx/api/DetailAPI
    //加入後路徑： localhost:xxxx/api
    //當然這邊的名稱也都是可以自行改的
    [System.Web.Http.RoutePrefix("api")]
    public class DetailAPIController : ApiController
    {

        //有設定路徑的寫法
        //路徑為： localhost:xxxx/api/GetFile/?fileName=
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("GetFile/")]
        public IHttpActionResult GetFile(string fileName)
        {
            //
            //以下是將字串變成檔案下載給使用者
            //

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            string resultStr = "this is your file";

            response.Content = new StringContent(resultStr);
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = HttpUtility.UrlPathEncode(fileName);
            //response.Content.Headers.ContentLength = fileStream.Length; //告知瀏覽器下載長度
            return ResponseMessage(response);
        }


        //有設定路徑的寫法
        //路徑為： localhost:xxxx/api/GetNowFile/?fileName=
        [System.Web.Http.HttpGet]
        [System.Web.Http.Route("GetNowFile/")]
        public IHttpActionResult GetNowFile(string fileName)
        {
            //
            //將Server現有的檔案下載給使用者
            //

            var response = new HttpResponseMessage(HttpStatusCode.OK);

            //要傳回文字型式
            response.Content = new StringContent("文字、或者html標籤……等相關文字組成");

            //傳回檔案型式
            string path = HttpContext.Current.Server.MapPath(@"~/" + fileName);     //檔案路徑

            //讀取檔案內容
            StreamReader sr = new StreamReader(path);
            response.Content = new StreamContent(sr.BaseStream);

            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");
            response.Content.Headers.ContentDisposition.FileName = HttpUtility.UrlPathEncode(fileName);
            //response.Content.Headers.ContentLength = fileStream.Length; //告知瀏覽器下載長度

            return ResponseMessage(response);
        }

        //Post的寫法（最主要的差別就是要在路徑上寫好參數名稱，並且加上一個?再用{}包起來）
        //路徑為： localhost:xxxx/api/PostResult
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route("PostResult/{text?}")]
        public string PostResult(string text)
        {
            return text;

            //如果今天回傳的是一個Model，那接收到後轉成字串即為JSON字串
        }
    }
}