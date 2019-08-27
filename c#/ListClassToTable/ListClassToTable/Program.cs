using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ListClassToTable
{

    //得到DisplayName的擴充方法
    public static class ExtensionMethods
    {
        public static string GetDisplayName(this PropertyInfo prop)
        {
            if (prop.CustomAttributes == null || prop.CustomAttributes.Count() == 0)
                return prop.Name;

            var displayNameAttribute = prop.CustomAttributes.Where(x => x.AttributeType == typeof(DisplayNameAttribute)).FirstOrDefault();

            if (displayNameAttribute == null || displayNameAttribute.ConstructorArguments == null || displayNameAttribute.ConstructorArguments.Count == 0)
                return prop.Name;

            return displayNameAttribute.ConstructorArguments[0].Value.ToString() ?? prop.Name;
        }
    }

    //測試的class
    public class A
    {
        [DisplayName("測試")]
        public string B { get; set; }
        [DisplayName("C的DisplayName")]
        public string C { get; set; }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            List<A> data = new List<A>
            {
                new A {B = "aaa", C = "bbb"},
                new A {B = "ccc", C = "ddd"}
            };

            Console.WriteLine(GetTable<A>(data));

            Console.ReadLine();

        }

        /// <summary>
        /// 使用泛型作法
        /// 將得到的ViewModel或者 其他 相關 Class 列成表格的模式
        /// 也可以實作成html的table，就可以將VM直接變成表格了
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">來源資料</param>
        /// <returns></returns>
        public static string GetTable<T>(List<T> source)
        {
            string result = "";

            if (source.Count() > 0)
            {
                //單純只是為了得到屬性和屬性的長度
                T t = source[0];

                //總屬性欄位數
                int len = t.GetType().GetProperties().Length;

                //全部屬性的名稱
                List<string> attributeName = t.GetType().GetProperties().Where(x => true).Select(x => x.Name).ToList();
                //全部屬性其所對應的DisplayName
                List<string> displayNameList = attributeName.Select(n => t.GetType().GetProperty(n).GetDisplayName()).ToList();


                //列出總長度(可視情況拿掉)
                result += "屬性長度 = " + len.ToString() + "\n\n";

                //表頭head列出來
                foreach (var displayname in displayNameList)
                {
                    result += displayname + "            ";
                }
                result += "\n";

                //要將資料秀出來
                for (var i = 0; i < source.Count(); i++)
                {
                    //這邊是為了要取到自己的值，所以每一個都才要去GetType()
                    Type ty = source[i].GetType();

                    //將資料都塞進去
                    foreach (var name in attributeName)
                    {
                        //得到自己的資料
                        result += "屬性: " + name + " 值= " + ty.GetProperty(name).GetValue(source[i]).ToString() + " ";
                    }

                    result += "\n";
                }
            }

            //可視情況變成html文字型態也可
            return result;
        }


        /// <summary>
        /// 將傳入的List Class轉成 HTML的表格
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">來源資料</param>
        /// <param name="tableID">HTML表格的ID</param>
        /// <param name="tableClass">HTML表格的Class (例如： table tt)</param>
        /// <param name="ignoreList">不要顯示的欄位名稱(拿其Class的屬性名稱)</param>
        /// <returns></returns>
        public string GetHtmlTable<T>(List<T> source, string tableID, string tableClass, List<string> ignoreList)
        {
            string result = $"<table id='{tableID}' class='{tableClass}'>";

            if(source.Count() > 0)
            {
                //單純只是為了得到屬性和屬性的長度
                T t = source[0];

                //總屬性欄位數
                int len = t.GetType().GetProperties().Length;

                //全部屬性的名稱(ignore的就不要出現了)
                List<string> attributeName = t.GetType().GetProperties().Where(x => ignoreList.Where(i => i == x.Name).Count() == 0).Select(x => x.Name).ToList();
                //全部屬性其所對應的DisplayName
                List<string> displayNameList = attributeName.Select(n => t.GetType().GetProperty(n).GetDisplayName()).ToList();

                //加入表頭
                #region 加入表頭
                result += "<tr>";
                foreach(var displayName in displayNameList)
                {
                    result += $"<th> {displayName} </th>";
                }
                result += "</tr>";
                #endregion

                //加入值
                #region 加入值
                //要將資料秀出來
                for (var i = 0; i < source.Count(); i++)
                {
                    result += "<tr>";

                    //這邊是為了要取到自己的值，所以每一個都才要去GetType()
                    Type ty = source[i].GetType();

                    //將資料都塞進去
                    foreach (var name in attributeName)
                    {
                        //得到自己的資料
                        result += $"<td> {ty.GetProperty(name).GetValue(source[i]).ToString()} </td>";
                    }

                    result += "</tr>";
                }
                #endregion
            }

            result += "</table>";

            return result;
        }
    }
}
