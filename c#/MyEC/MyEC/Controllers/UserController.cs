using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using MyEC;

namespace MyEC.Controllers
{
    public class UserController : Controller
    {
        private my_ecEntities db = new my_ecEntities();

        // GET: User
        // User/Index?type=顧客
        // User/Index?type=廠商
        public ActionResult Index(string type)
        {
            IEnumerable<User> user_list = null;

            switch(type){
                case "廠商":
                    user_list = db.User.Where(j => j.user_type == 1).ToList();
                    break;
                case "顧客":
                    user_list = db.User.Where(j => j.user_type == 2).ToList();
                    break;
                //列出所以項目
                default:  
                    user_list = db.User.ToList();
                    break;
            }

            List<String> is_ban = new List<String>();

            foreach(var i in user_list){
                string ban_status;
                ban_status = db.Ban.Find(i.user_id) == null ? "N" : "Y";
                is_ban.Add(ban_status);
            }

            ViewBag.is_ban = is_ban;

            return View(user_list);
        }

        // GET: User/Create
        public ActionResult Create()
        {
            //如果cookies顯示是admin，則讓viewBag.user_type = 1
            //如不是則讓viewBag.user_type = 2
            ViewBag.user_type = 1;
            return View();
        }

        // POST: User/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_id,user_type,name,account,password,phone,address,email,remark")] User user)
        {
            if (ModelState.IsValid)
            {
                db.User.Add(user);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(user);
        }

        // GET: User/Edit/5
        public ActionResult Edit(int? id)
        {
            //依據cookies的id來find 再return 回去（現先沒做）
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.User.Find(id);

            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        // POST: User/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_id,user_type,name,account,password,phone,address,email,remark")] User user)
        {
            if (ModelState.IsValid)
            {
                db.Entry(user).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(user);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        //GET  一般login 畫面
        public ActionResult Login()
        {
            return View();
        }

        //登入（用post傳帳密進來）
        [HttpPost]
        public ActionResult Login(string account, string password)
        {
            IEnumerable<Ban> ban_list = db.Ban.ToList();
            DateTime now = DateTime.Now;  //今天日期

            //先更新ban資料庫的內容
            foreach (var b in ban_list){
                int c = DateTime.Compare(now, b.ban_end_data);  //2日期比較

                //<0代表 now比end_data小  =0代表相同 >0代表now比end_data大
                if(c >= 0) {
                    //從資料庫刪除
                    db.Ban.Remove(b);
                    db.SaveChanges();
                }
            }

            IEnumerable<User> u = db.User.Where(ur => ur.account == account && ur.password == password);

            //應該只有一筆喔
            if (u.Count() == 1){

                foreach(var a in u){
                    if(db.Ban.Find(a.user_id) == null){
                        //允許登入
                    }else{
                        //還在被封鎖中
                    }
                }                
            }else{
                //回傳輸入不正確
            }



            return View();
        }

        //登出（把session清空）
        public ActionResult Loginout()
        {
            return RedirectToAction("Login");
        }
    }
}
