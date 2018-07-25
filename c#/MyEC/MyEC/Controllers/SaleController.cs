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
    public class SaleController : Controller
    {
        private my_ecEntities db = new my_ecEntities();

        // GET: Sale
        public ActionResult Index(string year)
        {
            //先判斷cookies是廠商還是顧客
            //再用 cookies 的 id 來列出資料
            IEnumerable<Sale> sale_list = null;
            List<String> buyer_name = new List<String>();  //購買者名稱
            List<String> product_name = new List<String>(); //產品名稱
            List<String> vender_name = new List<String>(); //廠商名稱


            int i = 1;  //一樣先預設

            //如果是廠商
            //用sale的product_id 去找 Product的product_id => 再用 Product的vender_id == user_id
            sale_list = db.Sale.Where(s => db.Product.Find(s.product_id).vendor_id == i);


            ViewBag.u_type = true;  //true代表廠商

            //如果是顧客
            //sale_list = db.Sale.Where(s => s.buyer_id == i).ToList();
            //ViewBag.u_type = false;  //false代表顧客


            //年的篩選
            if(year != null)
            {
                int y = Int32.Parse(year);
                sale_list = sale_list.Where(s => s.sale_date.Year == y);
            }

            //把購買者名稱和產品名稱寫成list
            foreach (var s in sale_list){
                buyer_name.Add(db.User.Find(s.buyer_id).name);
                product_name.Add(db.Product.Find(s.product_id).pruduct_name);
                vender_name.Add(db.User.Find(db.Product.Find(s.product_id).vendor_id).name);
            }

            ViewBag.b_name = buyer_name;
            ViewBag.p_name = product_name;
            ViewBag.v_name = vender_name;

            return View(sale_list);
        }


        // POST: Sale/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "sale_id,buyer_id,product_id,sale_date,sale_price,goods_status,buy_amount")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Sale.Add(sale);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(sale);
        }

        // GET: Sale/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Sale sale = db.Sale.Find(id);
            if (sale == null)
            {
                return HttpNotFound();
            }

            ViewBag.b_n = db.User.Find(sale.buyer_id).name;
            ViewBag.p_n = db.Product.Find(sale.product_id).pruduct_name;

            return View(sale);
        }

        // POST: Sale/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "sale_id,buyer_id,product_id,sale_date,sale_price,goods_status,buy_amount")] Sale sale)
        {
            if (ModelState.IsValid)
            {
                db.Entry(sale).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(sale);
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        public ActionResult Chart(string year)
        {
            decimal[] month_r = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            int q_year = Int32.Parse(year);

            if(year != null){
                //where裡面的查詢不允許用s.sale_date.toString("yyyy-MM-dd")，也不準用int32.parse這些
                IEnumerable<Sale> s_list = db.Sale.Where(s => s.sale_date.Year == q_year);

                int[] m = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

                if (s_list.Count() != 0){
                    for (var i = 0; i <= 11; i++){
                        IEnumerable<Sale> s_l = s_list.Where(s => s.sale_date.Month == m[i]);

                        //相加
                        if (s_l.Count() != 0){month_r[i] = s_l.Sum(s => s.sale_price);}
                    }
                }

                ViewBag.month_r = month_r;
                ViewBag.year = year;

            }else { return HttpNotFound(); }
            

            return View();
        }
    }
}
