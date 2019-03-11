using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

/*
	CodeTemplates簡單介紹：
	步驟1： 先把 CodeTemplates資料夾加入專案中
	步驟2： 確認 MvcControllerWithContext/Conroller.cs.t4 => 這個是產生Controller範本的t4檔
	步驟3： 確認 MvcView/ 有 Create.cs.t4、List.cs.t4……等 => 這個是產生View範本的t4檔
	步驟4： 確認其他CodeTemplates是否有(最主要這次只用上面這2個)
	步驟5： 在Controller上按右鍵/加入Scaffold項目/具有檢視、使用Entity Framework的MVC控制器
	步驟6： 選擇要使用的類型及Class => 即會依照你的範本檔而產生
	步驟7： 進行微調即可 快速產生 Controller和View

	PS： 裡面範本檔的內容皆可以自行再修正，依照目前你所遇到的情形即可
	PS： 平常沒有自訂範本檔的話，會是微軟內建的
	PS： CodeTemplates裡面的資料夾、本身CodeTemplates的資料夾名稱不要亂改。這是真的都要取成這樣，他才讀得到
*/



namespace MVC_easy_test_2.Controllers
{
    public class CodeTemplatesController : Controller
    {
        // GET: CodeTemplates
        public ActionResult Index()
        {
            return View();
        }
    }
}