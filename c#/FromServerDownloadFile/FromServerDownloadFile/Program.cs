using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.IO;

namespace FromServerDownloadFile
{
    class Program
    {
        /// <summary>
        /// 這些是要寫在Server中的（即要放入Controller中的）
        /// 現在只是做個整理，所以執行一定是錯誤的
        /// HttpContext和HttpUtility和using System.Web.Http之錯誤
        /// 全屬正常現象
        /// 如要用時，再複製使用即可
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            //要傳回文字型式
            response.Content = new StringContent("文字、或者html標籤……等相關文字組成");

            //傳回檔案型式
            string path = HttpContext.Current.Server.MapPath(@"~/subQuestion.zip");     //zip路徑
            StreamReader sr = new StreamReader(path);
            response.Content = new StreamContent(sr.BaseStream);

            //固定寫法
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment");

            response.Content.Headers.ContentDisposition.FileName = HttpUtility.UrlPathEncode("檔名.txt");
			//response.Content.Headers.ContentLength = fileStream.Length; //告知瀏覽器下載長度


			/*
			    特別注意：（其中那路徑是可以改的）

			    如果是在Controller中，取得Server當前位置用：
				  var path = Path.Combine(Server.MapPath("~/subQuestion.zip");

			    如不在Controller中，取得Server當前位直用：
				  string path = HttpContext.Current.Server.MapPath(@"~/subQuestion.zip");
			*/

			//-----------------------------------------------------------------------------------------------------------------------------------------

			/*
				另一種寫法：

			public ActionResult GetFile(string fileName){
				//取得檔案路徑
				string realPath = Server.MapPath("~/download/" + fileName);
				//下載檔案( return File(路徑, 檔案格式, 檔名+副檔名) )
				return File(realPath, "xml", "aaa.xml");
				//return File(realPath, "doc", "aaa.doc");

				File()還有一種型式 => File(byte[], "類型", "檔名+副檔名");
				例如：
				byte[] content = 讀完檔的二進制資料
				return File(content, "image/jepg", "a.jpg");
			}
			
			*/

		}
	}
}
