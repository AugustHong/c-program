using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium;
using System.Runtime.InteropServices;

/*
	參考網址： https://www.gushiciku.cn/pl/ggP3/zh-tw
      先去 NuGet 裝上  HtmlAgilityPack (解析網頁內容用) 、 Selenium.WebDriver、 Selenium.Chrome.WebDriver
      、Selenium.WebDriver.ChromeDriver

      去 參考/右鍵/加入參考
      System.Security
 */

namespace 抓取動態網頁內容
{
      class Program
	{
            static void Main(string[] args)
		{
                  CrawlingWeb c = new CrawlingWeb();
                  c.crawlingWebFunc();
                  Console.Read();
            }         
      }

      public class CrawlingWeb
	{
            #region 異常  退出chromedriver


            [DllImport("user32.dll", EntryPoint = "FindWindow")]
            private extern static IntPtr FindWindow(string lpClassName, string lpWindowName);


            [DllImport("user32.dll", EntryPoint = "SendMessage")]
            public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);


            public const int SW_HIDE = 0;
            public const int SW_SHOW = 5;


            [DllImport("user32.dll", EntryPoint = "ShowWindow")]
            public static extern int ShowWindow(IntPtr hwnd, int nCmdShow);


            /// <summary>
            /// 獲取視窗控制代碼
            /// </summary>
            /// <returns></returns>
            public IntPtr GetWindowHandle()
            {
                  string name = (Environment.CurrentDirectory + "\\chromedriver.exe");
                  IntPtr hwd = FindWindow(null, name);
                  return hwd;
            }


            /// <summary>
            /// 關閉chromedriver視窗
            /// </summary>
            public void CloseWindow()
            {
                  try
                  {
                        IntPtr hwd = GetWindowHandle();
                        SendMessage(hwd, 0x10, 0, 0);
                  }
                  catch { }
            }


            /// <summary>
            /// 退出chromedriver
            /// </summary>
            /// <param name="driver"></param>
            public void CloseChromeDriver(IWebDriver driver)
            {
                  try
                  {
                        driver.Quit();
                        driver.Dispose();
                  }
                  catch { }
                  CloseWindow();
            }


            #endregion 異常  退出chromedriver

            public void crawlingWebFunc()
            {
                  Console.WriteLine("開始嘗試...");

                  // 目標網址
                  List<string> surls = new List<string>()
                  {
                        "https://www.google.com.tw",
                        "https://www.yahoo.com.tw"
                  };

                  ChromeDriverService service = ChromeDriverService.CreateDefaultService(System.Environment.CurrentDirectory);
                  service.HideCommandPromptWindow = true;

                  ChromeOptions options = new ChromeOptions();
                  options.AddArguments("--test-type", "--ignore-certificate-errors");
                  options.AddArgument("enable-automation");
                  //  options.AddArgument("headless");
                  //  options.AddArguments("--proxy-server=http://user:password@yourProxyServer.com:8080");

                  // 開始讀取
                  using (IWebDriver driver = new OpenQA.Selenium.Chrome.ChromeDriver(service, options, TimeSpan.FromSeconds(120)))
                  {
                        // 初始網頁 (也可不設)
                        //driver.Url = "https://www.google.com.tw";
                        //Thread.Sleep(200);
                        try
                        {
                              int a = 1;
                              foreach (var itemsurls in surls)
                              {
                                    Console.WriteLine("\r\n第" + a.ToString() + "個網址");
                                    driver.Navigate().GoToUrl(itemsurls);


                                    // 這邊還可以執行 JS 語法，他會真的開出網頁來執行 => 所以可以寫腳本
                                    //滑鼠放上去的內容因為頁面自帶只能顯示一個的原因 沒辦法做到全部顯示 然後在下載 只能是其他方式下載
                                    //  var elements = document.getElementsByClassName('hover-container');
                                    //  Array.prototype.forEach.call(elements, function(element) {
                                    //  element.style.display = "block";
                                    //   console.log(element);
                                    //  });


                                    //   IJavaScriptExecutor js = (IJavaScriptExecutor)driver;


                                    //    var sss = js.ExecuteScript(" var elements = document.getElementsByClassName('hover-container');  Array.prototype.forEach.call(elements, function(element) {  console.log(element); element.setAttribute(\"class\", \"測試title\");  element.style.display = \"block\";  console.log(element); });");

                                    Thread.Sleep(500);
                                    string result = driver.PageSource;
                                    a++;
                              }

                              CloseChromeDriver(driver);
                        }
                        catch (Exception ex)
                        {
                              CloseChromeDriver(driver);
                              Console.WriteLine(ex.Message);
                        }
                  }
            }
      }
}
