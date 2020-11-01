using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Selenium
{
    /*
        網站上有寫 有 ElementIsPresent() 函式，但實際用發現沒有 => 所以只好自己寫了
     */
    public static class SeleniumTestHelper
    {
        /// <summary>
        ///  網站上有寫 有 ElementIsPresent() 函式，但實際用發現沒有 => 所以只好自己寫了
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="by"></param>
        /// <returns></returns>
        public static bool ElementIsPresent(this IWebDriver driver, By by)
        {
            return driver.FindElements(by).Count > 0;
        }

        /// <summary>
        ///  用 JQuery 的寫法 來找
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<IWebElement> Find(this IWebDriver driver, string query)
        {
            return driver.FindElements(By.CssSelector(query)).ToList();
        }

        /// <summary>
        /// 用 JQuery 的寫法 來找 (只找第1筆)
        /// </summary>
        /// <param name="driver"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IWebElement FindFirst(this IWebDriver driver, string query)
        {
            return driver.FindElement(By.CssSelector(query));
        }

        /// <summary>
        /// 用 JQuery 的寫法 來找
        /// </summary>
        /// <param name="element"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static List<IWebElement> Find(this IWebElement element, string query)
        {
            return element.FindElements(By.CssSelector(query)).ToList();
        }

        /// <summary>
        /// 用 JQuery 的寫法 來找 (只找第1筆)
        /// </summary>
        /// <param name="element"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public static IWebElement FindFirst(this IWebElement element, string query)
        {
            return element.FindElement(By.CssSelector(query));
        }

        /// <summary>
        ///  仿 JS 的 Map
        ///  $().map(function(index, ele){});
        /// </summary>
        /// <param name=""></param>
        public static void Map(this List<IWebElement> source, Action<int, IWebElement> action)
        {
            for (var i = 0; i < source.Count(); i++)
            {
                action(i, source[i]);
            }
        }

        /// <summary>
        ///  仿 JS 的 Map
        ///  $().map(function(index, ele, arr){});
        /// </summary>
        /// <param name="source"></param>
        /// <param name="action"></param>
        public static void Map(this List<IWebElement> source, Action<int, IWebElement, List<IWebElement>> action)
        {
            for (var i = 0; i < source.Count(); i++)
            {
                action(i, source[i], source);
            }
        }
    }
}
