using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using NLog;   //使用NLog

/*
	NLog的使用

	參考網址：https://dotblogs.com.tw/stanley14/2017/02/15/nlog
	參考網址：http://kevintsengtw.blogspot.com/2011/10/nlog-advanced-net-logging-1.html

	使用步驟：
	1.先去NuGet裝上 NLog.Config（他會自動幫你裝上NLog，如果沒有再自己裝）
	2. 裝完後會產生 NLog.config檔，去裡面進行設定（用我這個最基本的即可）
	3.在Controller和Service中引用    加入protected Logger logger = LogManager.GetCurrentClassLogger();
	4.開始寫Log
*/

namespace MVC_easy_test_2.Controllers
{
	public class NLogController : Controller
	{

		//如上面的第3步，加入這行（可放置於BaseController中）
		protected Logger logger = LogManager.GetCurrentClassLogger();

		public void Index()
		{
			//第一種寫法
			logger.Trace("我是追蹤:Trace");
			logger.Debug("我是偵錯:Debug");
			logger.Info("我是資訊:Info");
			logger.Warn("我是警告:Warn");
			logger.Error("我是錯誤:error");
			logger.Fatal("我是致命錯誤:Fatal");

			//第二種寫法（加上Exception的）
			logger.Error(new Exception("這是個錯誤"), "我是錯誤:error");
		}
	}
}