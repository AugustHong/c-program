using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;

namespace MVC_easy_test_2.Helper
{
	/// <summary>
	/// 取得使用者資料(這裡只取Name而已，以後可以再自行調整)
	/// </summary>
	public class WebSiteHelper
	{
		public static string CurrentUserName
		{
			get
			{
				//取得當前登入者
				var httpContext = HttpContext.Current;
				var identity = httpContext.User.Identity as FormsIdentity;

				if (identity == null)
				{
					return string.Empty;
				}
				else
				{
					//取得名字
					var name = identity.Name;
					return name;
				}
			}
		}
	}
}