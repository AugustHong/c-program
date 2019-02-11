using NLog;
using System;
using System.Collections.Generic;
using System.Globalization;     //要記得引用（多國語系所需要的）
using System.Linq;
using System.Threading;         //要記得引用（多國語系所需要的）
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace MVC_easy_test_2
{
	public class MvcApplication : System.Web.HttpApplication
	{
		protected void Application_Start()
		{
			AreaRegistration.RegisterAllAreas();
			RouteConfig.RegisterRoutes(RouteTable.Routes);


			//寫Web上的排程(一定要在Application_Start這隻Function中)
			//啟用多執行緒(TaskLoop是下方的函式)
			ThreadStart tsTask = new ThreadStart(TaskLoop);
			Thread MyTask = new Thread(tsTask);
			MyTask.Start();
		}


		/// <summary>
		/// 執行排程內的Loop
		/// </summary>
		static void TaskLoop()
		{
			while (true)
			{
				// 執行的動作都放這(如果有好幾個就寫好幾個吧)，這邊只先寫一個即可
				ToDo();

				// 等待幾秒後繼續執行排程(這裡是寫500秒)
				System.Threading.Thread.Sleep(TimeSpan.FromSeconds(500));
			}
		}

		//執行的動作，這邊只是示範所以就只寫log即可
		static void ToDo()
		{
			//這邊是做測試用所以才加的
			Logger logger = LogManager.GetCurrentClassLogger();

			logger.Info("排程正在執行中……");
	}


		//多國語系要用的
		protected void Application_PostMapRequestHandler(Object sender, EventArgs e)
		{
			try
			{
				var language = (string)HttpContext.Current.Request.RequestContext.RouteData.Values["lang"] ?? "zh-TW";
				Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentUICulture = new CultureInfo(language);
			}
			catch (Exception) { }
		}
	}
}
