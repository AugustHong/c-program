using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc; //有這個才能繼承Attribute

//資料庫要用的
using System.Data.SqlClient;
using Dapper;

using MVC_test.Models.ViewModel;
using MVC_test.Controllers;
using System.Web.Routing;

namespace MVC_test.Infrastrueture.Filter
{
    /// <summary>
    /// 取得所有種類的下拉式清單
    /// </summary>
    public class CategoryFilter : ActionFilterAttribute
    {
        //覆寫其ActionFilterAttribute的函式（這函式是action一開始執行後執行的）
        //ActionExecutedContext就是只在執行這action（controller裡的），所有的相關資料集合
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            //把東西載入到OnActionExecuted（一定要寫的）
            base.OnActionExecuted(filterContext);

            //取得所有種類（照理來說是要寫成service，但是懶得寫所以寫在這裡）
            string connString = System.Configuration.ConfigurationManager.ConnectionStrings["dbContext"].ToString();

            var sql = "select * from Categories order by ID";

            //用using會讓裡面的conn連線用完會自動斷線（即用完就結束）
            using (var conn = new SqlConnection(connString))
            {
                var data = conn.Query<CategoryViewModel>(sql);  //Dapper用的 

                // 放入ViewData/ViewBag
                filterContext.Controller.ViewBag.CategoryList = data;
            }
        }

        //執行前執行的
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            //判別是否是哪一個Controller
            bool isCategoryController = filterContext.Controller is CategoryController;

            //判斷這個Action是否有這個Attribute（後面的CategoryFilter是Attribute名）
            bool isAttribute = filterContext.ActionDescriptor.IsDefined(typeof(CategoryFilter), true);

            //跳轉頁面（可以做判斷，如果沒登入就進去Login畫面）
            //其中Redirect是下面的函式
            //filterContext.Result = Redirect("Account", "LogIn");


            base.OnActionExecuting(filterContext);
        }

        //做跳轉頁面用
        private RedirectToRouteResult Redirect(string controller, string action)
        {
            return new RedirectToRouteResult(new RouteValueDictionary {
                                                                    { "Controller", controller },
                                                                    { "Action", action }}
            );
        }
    }
}