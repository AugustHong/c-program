using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MVC_test.Models.ViewModel;

//資料庫要用的
using System.Data.SqlClient;
using Dapper;
using MVC_test.Infrastrueture.Filter;

//先去裝Dapper（在NuGet中）

namespace MVC_test.Controllers
{
    public class CategoryController : Controller
    {

        private string connString
        {
            get
            {
                //抓我們寫在web.config裡的連線字串（裡面的dbContext是當時寫連線字串中寫的，如果改了這裡也要改）
                return System.Configuration.ConfigurationManager.ConnectionStrings["dbContext"].ToString();
            }
        }



        // GET: Category
        public ActionResult Index()
        {

            var sql = "select * from Categories order by ID";

            //用using會讓裡面的conn連線用完會自動斷線（即用完就結束）
            using (var conn = new SqlConnection(connString)) {
                var data = conn.Query<CategoryViewModel>(sql);  //Dapper用的
                return View(data);
            }
        }

        // GET: Category/Details/5
        public ActionResult Details(int id)
        {
            //查到此筆資料
            var sql = "select * from Categories where ID=@Id";
            using (var conn = new SqlConnection(connString))
            {
                var record = conn.QuerySingleOrDefault<CategoryViewModel>(sql, new { Id = id });

                if (record == null)
                {
                    var url = Url.Action("Index");  //找不到資料就回index頁面
                    var jsCode = $"<script>alert('找不到記錢');location.href='{url}';</script>";

                    return Content(jsCode); //把標籤寫進網頁中（用Content()，跟Ajax一樣）
                }

                return View(record);
            }
        }

        // GET: Category/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        //create後面的參數記得要改
        public ActionResult Create(CategoryViewModel formData)
        {
            try
            {
                // 新增資料
                var sql = "insert into Categories(CategoryName) values(@CategoryName)";
                using(var conn = new SqlConnection(connString)){
                    conn.Execute(sql, formData);
                }

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Category/Edit/5
        public ActionResult Edit(int id)
        {
            //查到此筆資料
            var sql = "select * from Categories where ID=@Id";
            using(var conn = new SqlConnection(connString))
            {
                var record = conn.QuerySingleOrDefault<CategoryViewModel>(sql, new { Id = id });

                if(record == null)
                {
                    var url = Url.Action("Index");  //找不到資料就回index頁面
                    var jsCode = $"<script>alert('找不到記錄');location.href='{url}';</script>";

                    return Content(jsCode); //把標籤寫進網頁中（用Content()，跟Ajax一樣）
                }

                return View(record);
            }
        }

        // POST: Category/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, CategoryViewModel formData)
        {
            try
            {
                // 修改
                var sql = "update Categories set CategoryName = @CategoryName where ID=@Id";
                using(var conn = new SqlConnection(connString))
                {
                    var record = conn.Execute(sql, formData);
                }
                return RedirectToAction("Index");
            }
            catch
            {
                var url = Url.Action("Edit/" + id.ToString());  //找不到資料就回原本的頁面
                var jsCode = $"<script>alert('新增失敗');location.href='{url}';</script>";

                return Content(jsCode); //把標籤寫進網頁中（用Content()，跟Ajax一樣）
            }
        }

        // GET: Category/Delete/5
        public ActionResult Delete(int id)
        {
            //查到此筆資料
            var sql = "delete from Categories where id=" + id.ToString();
            using (var conn = new SqlConnection(connString))
            {
                conn.Execute(sql);

                return RedirectToAction("Index");
            }
        }


        //加入Attribute（會在執行action中，先做這一步把資料放進viewbag中）
        [CategoryFilter]
        public ActionResult CategoryList()
        {
            return View();
        }
    }
}
