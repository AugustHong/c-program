using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Threading.Tasks;

using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.IO;

namespace Hong.UploadHelper
{
    class UploadHelper
    {
        /// <summary>
        /// 提供檔案上傳功能
        /// </summary>
        /// <param name="file">上傳的檔案</param>
        /// <param name="savePath">要儲存到server的路徑（例如要放在upload底下，輸入：upload/）</param>
        /// <param name="accept">允許的副檔名格式（再做一次判斷），預設為全部皆可。例如： .zip .doc .docx</param>
        /// <param name="format">檔名格式（YYYY=今天西元年，MM=今天月，DD=今天日，EEE=今天民國年，FFFF=原上傳檔名，RRR=亂數產生編碼（為16碼英數混合，目前無法更改設定））  預設為FFFF，可自行加入編寫。例如： YYYY-MM-FFFF-RRRR0011</param>
        /// <param name="rndCount">亂碼為幾位數，預設為16碼</param>
        /// <returns></returns>
        public static string Upload(HttpPostedFileBase file, string savePath, string accept = "", string format = "FFFF", int rndCount = 16)
        {

            //副檔名（出來會是.zip這種格式）
            string extension = Path.GetExtension(file.FileName);

            //檔名（未含副檔名，因為要用於格式的串接，所以先把它分出來）
            string fileName = Path.GetFileName(file.FileName).Replace(extension, "");

            //如果副本名不是其允許的，跳出例外
            if (!string.IsNullOrEmpty(accept) && !accept.Contains(extension)) { throw new Exception("此副檔名不是可支援的"); }

            //如果使用者沒給/，自己加
            savePath = savePath.Substring(savePath.Length - 1) == "/" || savePath.Substring(savePath.Length - 2) == "\\" ? savePath : savePath + "/";


            //整個的儲存路徑（含server）
            string newSavePath = HttpContext.Current.Server.MapPath("~/" + savePath);

            //如果沒此資料夾，新建它
            if (!Directory.Exists(newSavePath)) { Directory.CreateDirectory(newSavePath); }

            //判斷位數碼是否為1-16
            if(rndCount > 16 || rndCount <= 0) { rndCount = 16; }

            //檔案的新名字（格式化）
            string newFileName = format.Replace("YYYY", DateTime.Now.Year.ToString()).
                                        Replace("MM", DateTime.Now.Month.ToString()).
                                        Replace("DD", DateTime.Now.Day.ToString()).
                                        Replace("EEE", (DateTime.Now.Year - 1911).ToString()).
                                        Replace("FFFF", fileName).
                                        Replace("RRR", bp_guid16().Substring(0, rndCount));

            //如果檔名重複，自動幫他加亂數16碼在前面（因為他可能輸入FFFF，所以會有機會重複）
            if(File.Exists(newSavePath + newFileName + extension)){newFileName = bp_guid16() + "-" + newFileName;}


            //儲存起來
            file.SaveAs(newSavePath + extension);


            //傳回儲存路徑 + 新檔名
            return newSavePath + newFileName + extension;
        }

        /// <summary>
        /// 傳回不重覆的 len 16  GUID 字串
        /// </summary>
        /// <returns>16碼英數混合的亂數</returns>
        private static string bp_guid16()
        {
            long i = 1;
            foreach (byte b in Guid.NewGuid().ToByteArray())
            {
                i *= ((int)b + 1);
            }
            return string.Format("{0:x}", i - DateTime.Now.Ticks);
        }
    }
}
