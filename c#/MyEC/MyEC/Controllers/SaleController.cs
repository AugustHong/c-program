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
            IQueryable<Sale> Sale_list;
            List<String> buyer_name = new List<String>();  //購買者名稱
            List<String> product_name = new List<String>(); //產品名稱
            List<String> vender_name = new List<String>(); //廠商名稱


            int i = Int32.Parse(Request.Cookies["MyCook"]["id"]);

            if (i != 0)
            {
                switch (Request.Cookies["MyCook"]["type"])
                {
                    case "廠商":
                        //如果是廠商
                        //先找出所以是此廠商的商品，再把一個個串上去
                        //IEnumerable<Product> p = db.Product.Where(pro => pro.vendor_id == i);

                        //foreach (var pd in p)
                        //{
                        //    Sale_list.Concat(db.Sale.Where(s => s.product_id == pd.product_id));
                        //}

                        Sale_list = from pd in db.Product
                                     join sa in db.Sale on pd.product_id equals sa.product_id
                                     where pd.vendor_id == i
                                     orderby sa.sale_date descending  //遞減排序（盡量不要再用.wher()這種，這個可以用於簡單的，但是要排序或者其他都會有問題）
                                     select sa;


                        ViewBag.u_type = true;  //true代表廠商

                        break;

                    default:
                        //如果是顧客（管理者也是一個顧客）
                        Sale_list = from sa in db.Sale
                                    where sa.buyer_id == i
                                    orderby sa.sale_date descending
                                    select sa;

                        ViewBag.u_type = false;  //false代表顧客

                        break;
                }

                //年的篩選
                if (year != null)
                {
                    int y = Int32.Parse(year);
                    Sale_list = Sale_list.Where(s => s.sale_date.Year == y);
                }


                //把購買者名稱和產品名稱寫成list
                if (Sale_list.Count() >= 1)
                {

                    foreach (var s in Sale_list)
                    {
                        buyer_name.Add(db.User.Find(s.buyer_id).name);
                        product_name.Add(db.Product.Find(s.product_id).pruduct_name);
                        vender_name.Add(db.User.Find(db.Product.Find(s.product_id).vendor_id).name);
                    }
                }
                
                ViewBag.b_name = buyer_name;
                ViewBag.p_name = product_name;
                ViewBag.v_name = vender_name;

                return View(Sale_list);
            }
            else { return RedirectToAction("Login", "User"); }
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
                //新增sale
                db.Sale.Add(sale);
                db.SaveChanges();

                //要把product 的amount修改一下
                Product p = db.Product.Find(sale.product_id);
                p.amount = p.amount - sale.buy_amount;
                db.Entry(p).State = EntityState.Modified;
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

            if (Request.Cookies["MyCook"]["type"] == "廠商" && db.User.Find(db.Product.Find(db.Sale.Find(id).product_id).vendor_id).user_id == Int32.Parse(Request.Cookies["MyCook"]["id"]))
            {
                Sale sale = db.Sale.Find(id);
                if (sale == null)
                {
                    return HttpNotFound();
                }

                ViewBag.b_n = db.User.Find(sale.buyer_id).name;
                ViewBag.p_n = db.Product.Find(sale.product_id).pruduct_name;

                return View(sale);
            }
            else { return RedirectToAction("Login", "User"); }
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
            if (Request.Cookies["MyCook"]["type"] == "廠商")
            {
                decimal[] month_r = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
                int q_year = Int32.Parse(year);

                int j = Int32.Parse(Request.Cookies["MyCook"]["id"]);

                IQueryable<Sale> s_list;

                s_list = from pd in db.Product
                            join sa in db.Sale on pd.product_id equals sa.product_id
                            where pd.vendor_id == j
                            select sa;

                if (year != null)
                {
                    //where裡面的查詢不允許用s.sale_date.toString("yyyy-MM-dd")，也不準用int32.parse這些
                    s_list = s_list.Where(s => s.sale_date.Year == q_year);

                    int[] m = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11, 12 };

                    if (s_list.Count() != 0)
                    {
                        for (var i = 0; i <= 11; i++)
                        {
                            int mm = m[i];
                            var s_l = s_list.Where(s => s.sale_date.Month == mm);

                            //相加
                            if (s_l.Count() != 0) {
                                foreach(var s in s_l){month_r[i] += s.sale_price;}
                            }
                        }
                    }

                    ViewBag.month_r = month_r;
                    ViewBag.year = year;

                }
                else { return HttpNotFound(); }


                return View();
            }
            else { return RedirectToAction("Login", "User"); }
        }
    }
}
