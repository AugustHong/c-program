using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Mvc; //有這個才能繼承Attribute

//資料庫要用的
using System.Data.SqlClient;
using Dapper;

using MVC_test.Models.ViewModel;

namespace MVC_test.Infrastrueture.Filter
{
    /// <summary>
    /// 取得所有種類的下拉式清單
    /// </summary>
    public class CategoryFilter : ActionFilterAttribute
    {
        //覆寫其ActionFilterAttribute的函式（這函式是action一開始執行前執行的）
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
    }
}