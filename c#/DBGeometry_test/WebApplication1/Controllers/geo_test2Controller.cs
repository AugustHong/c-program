using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using WebApplication1;
using Microsoft.SqlServer.Types;  //要在NuGet裝喔（名稱就是：Microsoft.SqlServer.Types）

namespace WebApplication1.Controllers
{
    public class geo_test2Controller : Controller
    {
        private geoEntities db = new geoEntities();

        // GET: geo_test2
        public ActionResult Index()
        {
            return View(db.geo_test2.ToList());
        }

        // GET: geo_test2/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            geo_test2 geo_test2 = db.geo_test2.Find(id);
            if (geo_test2 == null)
            {
                return HttpNotFound();
            }
            return View(geo_test2);
        }

        // GET: geo_test2/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: geo_test2/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "id,graph")] geo_test2 geo_test2)
        {
            if (ModelState.IsValid)
            {
                db.geo_test2.Add(geo_test2);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(geo_test2);
        }

        // GET: geo_test2/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            geo_test2 geo_test2 = db.geo_test2.Find(id);
            if (geo_test2 == null)
            {
                return HttpNotFound();
            }
            return View(geo_test2);
        }

        // POST: geo_test2/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "id,graph")] geo_test2 geo_test2)
        {
            if (ModelState.IsValid)
            {
                db.Entry(geo_test2).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(geo_test2);
        }

        // GET: geo_test2/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            geo_test2 geo_test2 = db.geo_test2.Find(id);
            if (geo_test2 == null)
            {
                return HttpNotFound();
            }
            return View(geo_test2);
        }

        // POST: geo_test2/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            geo_test2 geo_test2 = db.geo_test2.Find(id);
            db.geo_test2.Remove(geo_test2);
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
