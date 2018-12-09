using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_test.Controllers
{
    public class UploadDirectoryController : Controller
    {
        // GET: UploadDirectory
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }


        //上傳上來的處理(用IEnumerable<HttpPostedFileBase>是因為上傳資料夾其實就是上傳好幾個檔案）
        [HttpPost]
        public bool Index(IEnumerable<HttpPostedFileBase> directory)
        {
            ///這邊是可以寫在Service中的

            try
            {
                //Server主路徑
                string serverPath = Path.Combine(Server.MapPath("~/"));

                //跑過所有資料
                foreach (var file in directory)
                {
                    //他file.FileName會是路徑 例如： ABC/CC/1.txt 這種的，所以先用 / 把它所有的資料都拿出來
                    string[] fileTotalName = file.FileName.Split('/');

                    //真正要儲存的路徑資料（用迴圈跑出來。例如： 第一次是 ABC 第二次是 ABC/CC)
                    string midDirectoryPath = string.Empty;

                    //最後一筆才是真正的檔案，但要先把他路徑中的檔案一一建立起來（不用含最後一個，因為最後一個不是資料夾）
                    for(var i = 0; i < fileTotalName.Count() - 1; i ++)
                    {
                        //先串接資料
                        midDirectoryPath += fileTotalName[i];

                        //如果資料夾未存在，則建立
                        if (!Directory.Exists(midDirectoryPath)) { Directory.CreateDirectory(midDirectoryPath); }

                        //要往下走所以要加上 "/"
                        midDirectoryPath += "/";
                    }

                    //檔案名 1.txt
                    var fileName = Path.GetFileName(file.FileName);

                    //把前面的資料夾總路徑和檔名串接（不用特別加/，是因為在最後一步的時候有加了）
                    var path = midDirectoryPath + fileName;

                    //儲存
                    file.SaveAs(path);
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}