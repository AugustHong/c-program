using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using MVC_test.Models;
using MVC_test.Models.ViewModel;

namespace MVC_test.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public string Index(string name, string btnSubmit)
        {
            //也可以用 var name = Request.QueryString["name"];
            //var submit = Request.QueryString["btnSubmit"];
            //來取得get 和 post的值
            return "Hello World  "+ name + "  you press " + btnSubmit + " " + DateTime.Now;
        }

        [HttpGet]
        public string Index2()
        {
            string html = @"<h1>hello</h1>
                            <h2>hello</h2>
                            <h3>hello</h3>
                            <form mathod='get' action='../../home/index'>
                                <input type='text' name='name'>
                                <input type='submit' name='btnSubmit' value='submit'>
                            </form>";
            return html;
        }


        public ActionResult Tax()
        {
            return View();
        }


        [HttpPost]
        [ValidateInput(false)] //讓全部表單資料階可以輸入html，預設是不行輸入
        public ActionResult Tax(int? price)
        {
            int total = 0;
            ViewBag.price = price;

            try{
                ViewBag.tax = Order.GetTax(price, out total);
                ViewBag.total = total;

            }catch(Exception e){
                ViewBag.message = e.Message;
            }          

            return View();
        }

        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(ArticleViewModel article)
        {
            return View();
        }
    }
}