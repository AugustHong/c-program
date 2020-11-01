using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System;
using OpenQA.Selenium.Support.Extensions;

/*
    參考網圵： https://medium.com/@xinhe998/selenium-web-automation-testing-dc76d7d52b2b
    
    步驟：
    1. 新增專案/單元測試專案(.Net FrameWork)
    2. 去 NuGet 裝上 MSTest.TestAdapter 、 MSTest.TestFramework 、 
                    Selenium.RC 、 Selenium.WebDriver 、 Selenium.WebDriver.ChromeDriver 、 Selenium.Support
    3. 寫程式

    Selenium提供了多種方式來查找元素(FindElement)，包括：
    By ID
    By Name
    By ClassName
    By TagName
    By LinkText & PartialLinkText
    By XPath

    而除了 FindElement 外，Selenium WebDriver 也提供了許多 Command，例如：ElementIsPresent、Click等等。

    4. 點開【測試】> Test Explorer 就可以查看所有我們撰寫的測試方法。
    5. 然後點選【全部執行】可以進行建置，並掃描存在的測試方法然後執行。
    6. 如果想要個別測試， 就點擊要測試的那個。 再按 "執行"
    7. 如果要下中斷點 請按 "執行" 旁邊有個小三角形 (找到 "偵錯" ；如果想全部偵錯 ，點 "xxx所有進行偵錯")
 */

namespace Selenium
{
    [TestClass]
    public class BasicTest
    {
        // 指定網站
        private static string baseURL = "https://www.google.com.tw";
        private static IWebDriver driver;

        // 初使化 (順便連去 你所指定的網站)
        [ClassInitialize()]
        public static void SetupTest(TestContext context)
        {
            driver = new ChromeDriver();
            driver.Manage().Window.Maximize();  // 設定視窗最大化
            driver.Navigate().GoToUrl(baseURL);
        }

        // 關閉並離開
        [ClassCleanup()]
        public static void TestCleanup()
        {
            try
            {
                // 如果想保留畫面，就註解掉
                //driver.Quit();
            }
            catch (Exception)
            {
                // Ignore errors if unable to close the browser
            }
        }

        // 主測試邏輯
        [TestMethod]
        public void TestBasic()
        {
            // 輸入值
            IWebElement input = driver.FindElement(By.CssSelector("input.gLFyf"));
            input.SendKeys("你好啊！");
            Assert.AreEqual<string>("你好啊！", input.Text); 

            // 找到 連結 且 文字是 "隱私權" 的連結 再按下去
            driver.FindElement(By.LinkText("隱私權")).Click();

            // 找 是否有 連結(文字是 "下載 PDF") 的物件 存在在此頁面中
            bool isPresent = driver.ElementIsPresent(By.LinkText("下載 PDF"));

            // 確是 要是 true
            Assert.IsTrue(isPresent);

            isPresent = driver.ElementIsPresent(By.LinkText("下載 PDFFFF"));
            Assert.IsTrue(isPresent);   // 這邊應該要錯
        }

        [TestMethod]
        public void TestBasic2()
        {
            // 執行 js (如果放到太上面，下面的都不能跑了，因為沒人按掉 alert)
            // 而且 裡面不能用 " or \" 他是讀失敗的 (所以裡面要用 2次 ' 的話用 \')
            // 因 google 上沒 jquery 所以用 js 選擇器
            driver.ExecuteJavaScript("document.querySelectorAll('input.gLFyf')[0].value = 'bbb';");

            // 如果要用 jquery 的話 載入進去
            //string loadJS = "document.body.innerHTML += '<script src=\"https://ajax.googleapis.com/ajax/libs/jquery/3.3.1/jquery.min.js" + "\"></script>';";
            //driver.ExecuteJavaScript(loadJS);

            // 目前實測 載入成功了 ， 但還是失敗了
            //driver.ExecuteJavaScript("$('input.gLFyf')[0].value = 'aaa';");

            driver.ExecuteJavaScript("console.log('aaa');");
            driver.ExecuteJavaScript("var a = '123';alert('hello' + a);");
        }
    }
}
