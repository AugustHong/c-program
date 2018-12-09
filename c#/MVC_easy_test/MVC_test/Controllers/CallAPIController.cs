using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;

namespace MVC_test.Controllers
{
    public class CallAPIController : Controller
    {
        /// <summary>
        /// 取得Get的API資料
        /// </summary>
        /// <returns></returns>
        public string CallGetAPI()
        {
            //先來設定路徑
            string _baseAddress = "http://localhost:57410/";  //記得要有http://
            string _apiPath = "api/EasyAPI/Test";
            string parm = "?text=hello";   //即Get後面的參數

            //串接起來成路徑
            string strConcatUrl = string.Concat(_baseAddress, _apiPath, parm);

            //開始執行呼叫API和取得
            using (var client = new HttpClient())
            {
                //呼叫API
                client.BaseAddress = new Uri(strConcatUrl);

                //取值
                //出來的是"\"DAA0181000004\""  （會多出 \" \"的東西）
                string result = client.GetStringAsync(strConcatUrl).Result;
                return result.Substring(1, result.Length - 2);
            }


            #region 接收Model型別
            //string strConcatUrl = "http://localhost:57410/api/EasyAPI/Test?text=hello";
            //using (var client = new HttpClient())
            //{
            //    client.BaseAddress = new Uri(strConcatUrl);
            //    //HTTP GET
            //    var responseTask = client.GetStringAsync(strConcatUrl);
            //    responseTask.Wait();

            //    var result = responseTask.Result;
            //    if (result.IsSuccessStatusCode)
            //    {
            //將資料轉型
            //        var readTask = result.Content.ReadAsAsync<Model>();
            //        readTask.Wait();

            //用.Result取到剛才轉型後的資料
            //        students = readTask.Result;
            //    }
            //    else //web api sent error response 
            //    {
            //        //log response status here..

            //        students = Enumerable.Empty<Model>();

            //        ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
            //    }
            #endregion
        }

        /// <summary>
        /// Post給API並取值
        /// </summary>
        /// <returns></returns>
        public string CallPostAPI()
        {
            string rtnResult = string.Empty;

            string strConcatUrl = "http://localhost:57410/api/DetailAPI/PostResult";

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(strConcatUrl);
                //HTTP GET
                var responseTask = client.PostAsJsonAsync(strConcatUrl, "hello");
                responseTask.Wait();

                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsAsync<string>();
                    readTask.Wait();

                    rtnResult = readTask.Result;
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Server error. Please contact administrator.");
                }
            }

                return rtnResult;
        }
    }
}