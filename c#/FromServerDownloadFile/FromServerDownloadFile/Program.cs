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
            
        }
    }
}
