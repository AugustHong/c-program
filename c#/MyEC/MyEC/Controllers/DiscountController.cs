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
    public class DiscountController : Controller
    {
        private my_ecEntities db = new my_ecEntities();

        // GET: Discount
        public ActionResult Index()
        {
            //先確定cookies是廠商的
            if (Request.Cookies["MyCook"]["type"] == "廠商")
            {
                int i = Int32.Parse(Request.Cookies["MyCook"]["id"]);

                ViewBag.v_id = i;

                //照理來說只會有一筆，因為vender_id是pk
                return View(db.Discount.Where(d => d.vender_id == i).ToList());
            }
            else { return RedirectToAction("Login", "User"); }
        }


        // POST: Discount/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "vender_id,discount_percent,status")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                db.Discount.Add(discount);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(discount);
        }

        // POST: Discount/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "vender_id,discount_percent,status")] Discount discount)
        {
            if (ModelState.IsValid)
            {
                db.Entry(discount).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(discount);
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
