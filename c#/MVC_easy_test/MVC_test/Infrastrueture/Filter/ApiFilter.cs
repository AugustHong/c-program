using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

// for API 的 filter
using System.Web.Http.Filters;
using System.Web.Http.Controllers;
using System.Net.Http;
using System.Net;

namespace MVC_test.Infrastrueture.Filter
{
	// 這裡的 ActionFilterAttribute 是 System.Web.Http.Filters.ActionFilterAttribute ， 和 一般的不一樣
	//  其餘基本上差不多
	public class ApiFilter : ActionFilterAttribute
	{
		public string Test { get; set; }

		// 執行前執行的
		public override void OnActionExecuting(HttpActionContext actionContext)
		{
			// 導至 結果(後面的Action就不執行 => 用於驗證)
			string errorText = "{ \"message\": \"驗證失敗。\" }";
			actionContext.Response = actionContext.Request.CreateErrorResponse(HttpStatusCode.BadRequest, errorText);
		}

		/// <summary>
		///  得到 Client 端的 IP
		/// </summary>
		/// <returns></returns>
		private string GetClientIP()
		{
			try
			{
				System.Web.HttpContext context = System.Web.HttpContext.Current;

				// 判所client端是否有設定代理伺服器
				if (context.Request.ServerVariables["HTTP_VIA"] == null)
					return context.Request.ServerVariables["REMOTE_ADDR"].ToString().Replace("::1", "127.0.0.1");
				else
					return context.Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString().Replace("::1", "127.0.0.1");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
			return "0.0.0.0";
		}
	}
}