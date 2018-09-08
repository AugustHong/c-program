using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_test.Controllers
{
    public class AuthorController : Controller
    {
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(Models.ViewModel.AuthorViewModel formData)
        {
            //在這裡額外加寫需要的驗證，有錯就加入ModelState

            //有寫key值的(ilevel）會顯示在自已的下面
            if(formData.ILevel <= 0) { ModelState.AddModelError("ILevel", "ILevel必需大於0"); }

            //沒寫key值的，會在最上面
            if(string.IsNullOrEmpty(formData.AuthorPhote) && string.IsNullOrEmpty(formData.AuthorName)){
                ModelState.AddModelError(string.Empty, "photo and name not 同時空白");
            } 


            //後台的基本檢查
            if (ModelState.IsValid) {
                //通過驗證，開始執行insert record
            }

            return View();
        }
    }
}
