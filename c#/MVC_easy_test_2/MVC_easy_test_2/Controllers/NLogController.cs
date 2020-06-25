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

		/*
                如果要讓 NLog 寫檔的路徑是用 Web.Config來設定的話，請依照以下步驟：
                1. 先去 Web.Config 加入你要的變數
                    <add key="LogPath" value="C:\TEST.Logs" />
                2. 在你的進入點，即 你宣告 logger 這一隻的 時候，要順便告訴他我要載入的東西
                    var l = ConfigurationManager.AppSettings["LogPath"];
                    string logPath = l == null ? string.Empty : l.ToString();
                    NLog.MappedDiagnosticsLogicalContext.Set("LogPath", logPath);   
                    // 注意： .Set( 這個是你到時候要在 NLog.config中設的變數名稱, 你的Web.Config的值)
                3. 在 NLog.Config 中使用
                    <target name="File" xsi:type="File" fileName="${mdlc:item=LogPath}\Error.log" layout="${Layout}"
              encoding="utf-8" maxArchiveFiles="30" archiveNumbering="Sequence"
              archiveAboveSize="1048576" archiveFileName="${LogTxtDir}/${logger}.log{#######}" />

                    // 注意： ${mdlc:item=你在第二步設的變數名稱} => 這段就是用你得到的值
                    // 所以這時候 ${mdlc:item=LogPath} 就會是 C:\TEST.Logs 了
        */
	}
}