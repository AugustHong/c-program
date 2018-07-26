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
    public class BanController : Controller
    {
        private my_ecEntities db = new my_ecEntities();

        // GET: Ban
        public ActionResult Index(string q_name)
        {
            //要讓定cookies是管理者來可以看
            //if cookies == 管理者id

            if(Request.Cookies["MyCook"]["type"] == "管理者"){

                IEnumerable<Ban> ban_list = null;

                if (q_name == null) { ban_list = db.Ban.ToList(); }
                else {
                    IEnumerable<User> u = db.User.Where(ur => ur.name == q_name);
                    foreach (var ur in u) { ban_list = db.Ban.Where(b => b.user_id == ur.user_id); }
                }


                List<String> ban_name = new List<String>();

                foreach (var b in ban_list)
                {
                    string name;
                    name = db.User.Find(b.user_id).name;
                    ban_name.Add(name);
                }

                ViewBag.ban_name = ban_name;

                return View(ban_list);
            }else{
                return RedirectToAction("Login", "User");
            }

        }


        // GET: Ban/Create
        public ActionResult Create(int? id)
        {
            //if cookies == 管理者id  來執行
            if (Request.Cookies["MyCook"]["type"] == "管理者")
            {
                if (db.User.Find(id) != null)
                {
                    ViewBag.ban_id = id;
                    return View();
                }

                return RedirectToAction("../User/Index");
            }
            else { return RedirectToAction("Login", "User"); }
            
        }

        // POST: Ban/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "user_id,ban_reason,ban_end_data")] Ban ban)
        {
            if (ModelState.IsValid)
            {
                db.Ban.Add(ban);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(ban);
        }

        // GET: Ban/Edit/5
        public ActionResult Edit(int? id)
        {
            //要cookies是使用者
            if (Request.Cookies["MyCook"]["type"] == "管理者")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Ban ban = db.Ban.Find(id);
                if (ban == null)
                {
                    return HttpNotFound();
                }
                return View(ban);
            }
            else { return RedirectToAction("Login", "User"); }
        }

        // POST: Ban/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "user_id,ban_reason,ban_end_data")] Ban ban)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ban).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ban);
        }

        // GET: Ban/Delete/5
        public ActionResult Delete(int? id)
        {
            //要cookies是使用者
            if (Request.Cookies["MyCook"]["type"] == "管理者")
            {
                if (id == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
                }
                Ban ban = db.Ban.Find(id);
                if (ban == null)
                {
                    return HttpNotFound();
                }
                return View(ban);
            }
            else { return RedirectToAction("Login", "User"); }
        }

        // POST: Ban/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ban ban = db.Ban.Find(id);
            db.Ban.Remove(ban);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
