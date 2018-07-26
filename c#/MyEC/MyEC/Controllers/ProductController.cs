using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.IO;   //要使用檔案上傳要用這個
using MyEC;

namespace MyEC.Controllers
{
    public class ProductController : Controller
    {
        private my_ecEntities db = new my_ecEntities();

        // GET: Product
        // Product/Index?query=我的產品&q_type=家電
        public ActionResult Index(string query, string q_type)
        {
            IEnumerable<Product> p_list = null;
            string vender;
            double discount_percent = 1;
            List<String> vender_name = new List<String>();
            List<double> percent = new List<double>();

            switch (query){
                case "我的產品":
                    if (Request.Cookies["MyCook"]["type"] == "廠商")
                    {
                        int id = Int32.Parse(Request.Cookies["MyCook"]["id"]);
                        p_list = db.Product.Where(p => p.vendor_id == id);

                        //做篩選
                        if (q_type != null && q_type != "全部") { p_list = p_list.Where(p => p.type == q_type); }

                        ViewBag.is_vender_goods = true;  //記錄是否是廠商在看自己的產品（要讓他有修改、打折、下架 那些選項可以選）

                        //只有一個名稱
                        var v = db.User.Find(id);
                        vender = v.name;

                        var d = db.Discount.Find(v.user_id);

                        //percent數
                        if (d != null)
                        {
                            if (d.status == "Y")
                            {
                                discount_percent = d.discount_percent;
                            }
                        }


                        //把廠商的名稱變成list（但是全部一樣，所以用for跑）
                        for (int i = 1; i <= p_list.Count(); i++)
                        {
                            vender_name.Add(vender);
                            percent.Add(discount_percent);
                        }
                    }
                    else { return RedirectToAction("Login", "User"); }
                    break;

                default:
                    p_list = db.Product.ToList();
                    if(q_type != null && q_type != "全部") { p_list = p_list.Where(p => p.type == q_type); }

                    ViewBag.is_vender_goods = false;

                    foreach (var p in p_list){
                        var v1 = db.User.Find(p.vendor_id);
                        vender = v1.name;

                        var d2 = db.Discount.Find(v1.user_id);

                        if (d2 != null){
                            if (d2.status == "Y"){
                                discount_percent = d2.discount_percent;
                            }
                        }

                        vender_name.Add(vender);
                        percent.Add(discount_percent);

                        //要還原值（不然繼續下去一定會錯的喔）
                        discount_percent = 1;
                    }

                    break;
            }

            ViewBag.vender_name = vender_name;
            ViewBag.percent = percent;

            return View(p_list);
        }

        // GET: Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Product.Find(id);

            if (product == null)
            {
                return HttpNotFound();
            }else{
                string name;
                name = db.User.Find(db.Product.Find(id).vendor_id).name;

                double percent;
                percent = db.Discount.Find(db.Product.Find(id).vendor_id).discount_percent;

                ViewBag.v_name = name;
                ViewBag.p_percent = percent;
                ViewBag.b_id = Int32.Parse(Request.Cookies["MyCook"]["id"]);
            }

            return View(product);
        }

        // GET: Product/Create
        public ActionResult Create()
        {
            //這邊會是cookies中的id（且是廠商這邊才能觸發）
            if (Request.Cookies["MyCook"]["type"] == "廠商")
            {
                int i = Int32.Parse(Request.Cookies["MyCook"]["id"]); 
                ViewBag.v_id = i;

                return View();
            }
            else { return RedirectToAction("Login", "User"); }
        }

        // POST: Product/Create
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "product_id,pruduct_name,vendor_id,type,price,pic_path,amount,push_date,description")] Product product, HttpPostedFileBase pic)
        {
            if (ModelState.IsValid && pic != null)
            {
                //檔案上傳
                var fileName = Path.GetFileName(pic.FileName);
                var path = Path.Combine(Server.MapPath("~/Pic"), fileName);
                pic.SaveAs(path);

                //拿到檔名
                product.pic_path = fileName;


                db.Product.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Product/Edit/5
        public ActionResult Edit(int? id)
        {
            //要有cookies判斷
            
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            if (Int32.Parse(Request.Cookies["MyCook"]["id"]) == db.Product.Find(id).vendor_id)
            {
                Product product = db.Product.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }
            else { return RedirectToAction("Login", "User"); }
        }

        // POST: Product/Edit/5
        // 若要免於過量張貼攻擊，請啟用想要繫結的特定屬性，如需
        // 詳細資訊，請參閱 http://go.microsoft.com/fwlink/?LinkId=317598。
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "product_id,pruduct_name,vendor_id,type,price,pic_path,amount,push_date,description")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Product/Delete/5
        public ActionResult Delete(int? id)
        {
            //要有cookies判斷
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            if (Int32.Parse(Request.Cookies["MyCook"]["id"]) == db.Product.Find(id).vendor_id)
            {
                Product product = db.Product.Find(id);
                if (product == null)
                {
                    return HttpNotFound();
                }
                return View(product);
            }
            else { return RedirectToAction("Login", "User"); }
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
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
