using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Xml;

/*
	得到 XML 和 回傳 XML
	1. 在 WebApiConfig.cs 中加入 config.Formatters.XmlFormatter.UseXmlSerializer = true; 這行
*/

namespace MVC_test.API.XmlAPI
{
    public class XmlAPIController : ApiController
    {
		/// <summary>
		///  得到 xml (記得用postman的話，先去 body 輸入 xml 文字(且要選 xml) ；再來 去head 選 contentType = application/xml ；
		///  而 還有 Accept = application/xml 即可)
		/// </summary>
		/// <param name="request">得到的資料</param>
		/// <returns></returns>
		[HttpPost]
		public IHttpActionResult HandleXml(HttpRequestMessage request)
		{
			// 解析成 xml 字串
			var doc = new XmlDocument();
			doc.XmlResolver = null;
			doc.Load(request.Content.ReadAsStreamAsync().Result);
			string xmlS = doc.DocumentElement.OuterXml.ToString();

			// 詳細 xml 字串 轉成 class 會在  struct_and_file 這個裡面解說

			// 回覆xml 字串
			string result = "<message>這是回覆的xml格式</message>";
			XmlDocument returnDoc = new XmlDocument();
			returnDoc.XmlResolver = null;
			returnDoc.LoadXml(result);

			var response = new HttpResponseMessage(HttpStatusCode.OK);
			response.Content = new StringContent(returnDoc.InnerXml);

			return this.ResponseMessage(response);
		}
	}
}
