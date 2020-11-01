using Microsoft.VisualStudio.TestTools.UnitTesting;
using OpenQA.Selenium;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.IE;
using System;
using OpenQA.Selenium.Support.Extensions;
using System.Linq;

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
    public class BasicTest2
    {
        // 指定網站
        private static string baseURL = "http://devexpress.github.io/testcafe/example";
        private static IWebDriver driver;

        // 初使化 (順便連去 你所指定的網站)
        [ClassInitialize()]
        public static void SetupTest(TestContext context)
        {
            driver = new ChromeDriver();
            var cookies = driver.Manage().Cookies;    // 可以得到 Cookies
            var screenHot = driver.TakeScreenshot();  // 再研究
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
            // 把我之前寫 TestCoffee 的案例拿來用一遍
            /*
                原始 TestCoffee 裡的邏輯：
                // Chrome Copy selector 定位法  (用 jquery 的這種方式)
                const supportRemoteBox = Selector("#remote-testing");

                // Chrome Copy XPath 定位法
                // const supportRemoteBox = Selector('*[id="remote-testing"]');

                // 使用 TestCafe 鏈式 Selector API
                const macOSRadio = Selector(".column")
                  .find("label")       // 找到 label 中 文字是 MacOs 其 child 的 input 項目
                  .withText("MacOS")
                  .child("input");

                // Chrome Copy selector 定位法
                // const macOSRadio = Selector('#macos');
                const interfaceSelect = Selector("#preferred-interface");

                await t
                  // 2. 直接使用 html element id，識別 input 在 Your name 輸入 ice
                  .typeText("#developer-name", "ice")
                  // 3. 透過預定義 selector supportRemoteBox，選 testing on remote devices
                  .click(supportRemoteBox)
                  // 4. 透過預定義的 selector macOSRadio 選擇 MacOS
                  .click(macOSRadio)
                  // 5.1 透過預定義的 selector interfaceSelect，按下選單彈出選項 (先按一下下拉選單)
                  .click(interfaceSelect)
                  // 5.2 使用 ＴestCafe 鏈式 Selector API，找到 interfaceSelect 下的 Both Option
                  .click(interfaceSelect.find("option").withText("Both"))
                  // 6. 按下 Submit 按鈕
                  .click("#submit-button")
                  // 7. 確認結果等於 Thank you, ice!
                  .expect(Selector("#article-header").innerText).eql("Thank you, ice!");
             */
            IWebElement supportRemoteBox = driver.FindFirst("#remote-testing");
            IWebElement macOSRadio = driver.FindFirst("input[type='radio'][value='MacOS']");
            IWebElement interfaceSelect = driver.FindFirst("#preferred-interface");

            // 為了調慢速度 可以 sleep

            driver.FindFirst("#developer-name").SendKeys("ice");
            System.Threading.Thread.Sleep(500);

            supportRemoteBox.Click();
            System.Threading.Thread.Sleep(500);

            macOSRadio.Click();
            System.Threading.Thread.Sleep(500);

            interfaceSelect.Click();
            System.Threading.Thread.Sleep(500);

            interfaceSelect.Find("option").First(o => o.Text == "Both").Click();

            // 也可以用我擴充的 Map
            interfaceSelect.Find("option").Map((index, ele) =>
            {
                if (ele.Text == "Both")
                {
                    //ele.Click();
                }
            });

            System.Threading.Thread.Sleep(500);

            // 最後頁面的文字(是 html)
            string text = driver.PageSource;
            System.Diagnostics.Debug.WriteLine(text);

            driver.FindFirst("#submit-button").Click();
            System.Threading.Thread.Sleep(500);

            // 最後頁面的文字(是 html)
            text = driver.PageSource;
            System.Diagnostics.Debug.WriteLine(text);

            string output = driver.FindFirst("#article-header").Text;
            Assert.AreEqual(output, "Thank you, ice!");
        }
    }
}
