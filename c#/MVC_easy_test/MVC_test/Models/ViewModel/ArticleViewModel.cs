using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MVC_test.Models.ViewModel
{
    public class ArticleViewModel
    {
        [DisplayName("會員id")]
        public int ID { get; set; }

        [DisplayName("標題")]
        public string Title { get; set; }

        [AllowHtml]  //只予許這個可以輸入html
        [DisplayName("內容")]
        public string Content { get; set; }
    }
}