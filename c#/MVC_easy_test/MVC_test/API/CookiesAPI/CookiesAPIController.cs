using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web.Http;

/*
	參考網圵： https://docs.microsoft.com/zh-tw/aspnet/web-api/overview/advanced/http-cookies
*/

namespace MVC_test.API.CookiesAPI
{
    public class CookiesAPIController : ApiController
    {
		[HttpPost]
		public HttpResponseMessage SetCookies(string key, string value)
		{
			HttpResponseMessage result = new HttpResponseMessage();

			// 取到 cookies
			CookieHeaderValue cookie = Request.Headers.GetCookies(key).FirstOrDefault();
			if (cookie != null)
			{
				result.Content = new StringContent($"已經存在此cookies了， 期限至 {cookie.Expires}，值為 {cookie[key].Value}");
				// 如果要重改值 => 把舊的期限給換了，再新增一個新的
				return result;
			}
			else
			{
				// 新增 cookies
				var newCookies = new CookieHeaderValue(key, value);
				newCookies.Expires = DateTimeOffset.Now.AddDays(1);
				newCookies.Domain = Request.RequestUri.Host;
				newCookies.Path = "/";

				result.Headers.AddCookies(new CookieHeaderValue[] { newCookies });
				return result;
			}
		}
	}
}
