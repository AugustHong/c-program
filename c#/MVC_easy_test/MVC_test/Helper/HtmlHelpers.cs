using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using MVC_test.Models.ViewModel;

namespace MVC_test.Helper
{
    //寫htmlhelper其實就是把html語法寫成字串（也可以在前端寫，不過習慣上把這種html的語法都寫成一個htmlhelper）
    //HtmlHelper是已有的名字，所以用HtmlHelpers（記得要設成static）
    public static class HtmlHelpers
    {

        //擴充法方的應用（IHtmlString是類型，如同string一樣是個類型）
        public static IHtmlString getCategoryListBtn(this HtmlHelper helper, IEnumerable<CategoryViewModel> categoryList)
        {
            //在htmlhelper中都是用這個類型來串接字串的
            StringBuilder sb = new StringBuilder();

            if (categoryList != null)
            {
                foreach (var item in categoryList)
                {
                    sb.AppendLine("<button class='btn btn-success btn-lg' type='button'>" + item.CategoryName + "</button>");
                }
            }

            //回傳時要用create
            return MvcHtmlString.Create(sb.ToString());
        }
    }
}
