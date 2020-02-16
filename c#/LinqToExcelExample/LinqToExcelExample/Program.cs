using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    參考網圵： https://blog.xuite.net/f8789/DCLoveEP/64271656-LINQ+-+%E5%AF%A6%E4%BD%9C+LinqToExcel
    參考網圵： https://dotblogs.com.tw/wasichris/2015/07/18/151903

    去 NuGet 裝上 LinqToExcel (_64版 也要裝)

    發生「'Microsoft.ACE.OLEDB.12.0' 提供者並未登錄於本機電腦上」錯誤訊息 ， 請至 我的 ImportExcel 或者是 ExportExcel 其中一個有解決方法(裝那個東西)
    https://devmanna.blogspot.com/2017/03/sql-server-excel-microsoftaceoledb120.html 這個東西
*/

namespace LinqToExcelExample
{
    public class A
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 取得當前執行的目錄
            string path = System.IO.Directory.GetCurrentDirectory() + "\\";
            string fileName = "Test.xlsx";

            string filePath = path + fileName;

            // 載入檔案
            var excel = new ExcelQueryFactory(filePath);

            // 得到檔案路徑
            Console.WriteLine(excel.FileName);

            // 得到所有的 Sheet 名稱
            var sheetNameList = excel.GetWorksheetNames();

            // 得到指定Sheet => 裡面的是 excel 中的 sheet 名
            // 可以拿上面的用 var sheet1 = excel.Worksheet(sheetNameList[0]);
            var sheet1 = excel.Worksheet("使用者");

            // 一、直接不轉成 Class 直接輸出
            foreach (var item in sheet1)
            {
                // 這裡的 "姓名"  和  "年齡"  都是 excel 中的 第一行 的名稱 (如果是英文的就輸入英文)
                string name = item["姓名"];
                int age = Convert.ToInt32(item["年齡"]);
                Console.WriteLine($"姓名 = {name} , 年齡 = {age} ");
            }

            Console.WriteLine("-------------------------------------------------");

            // 二、 如果今天的 欄位名稱 和 Excel 表頭的名稱相同 => 那就不用做Mapping，直接轉
            // 但我這邊是測試用的， 所以我的 Class 屬性 是 英文； 但我的 Excel 是 中文的

            //var sqlExcelData_2 = excel.Worksheet<A>("使用者");
            //foreach (var item in sqlExcelData_2)
            //{
            //    string name = item.Name;
            //    int age = Convert.ToInt32(item.Age);
            //    Console.WriteLine($"姓名 = {name} , 年齡 = {age} ");
            //}


            Console.WriteLine("--------------------------------------------------");

            // 三、很不幸的 Class 的屬性名  和 Excel 的表頭名是不同的話 => 要做 Mapping

            // Mapping 的部份 (指定用法)
            excel.AddMapping<A>(p => p.Name, "姓名");
            excel.AddMapping<A>(p => p.Age, "年齡");

            // 上面的部份 因為 最後一個參數都是寫死的 => 不好變動 => 可以用以下方法取代
            // 得到 這個 sheet 的 所有 column 名稱 (就是表頭欄啦)
            // 然後把上面 "姓名" 和 "年齡" => sheet1ColumnNameList[0] 和 sheet1ColumnNameList[1] 即可 (雖然 p.Name 這還是得要寫死，不過如果今天 Age 改成 Age1 ，程式也不用改)
            var sheet1ColumnNameList = excel.GetColumnNames("使用者");

            // 執行，而且這個還可以下條件
            var ExcelData_3 = excel.Worksheet<A>("使用者").Where(e => e.Age >= 20);
            Console.WriteLine("找年齡大於等於20歲的人");
            foreach (var item in ExcelData_3)
            {
                string name = item.Name;
                int age = Convert.ToInt32(item.Age);
                Console.WriteLine($"姓名 = {name} , 年齡 = {age} ");
            }

            Console.WriteLine("-----------------------------------------------");

            // 四、指定範圍內的資料
            excel.AddMapping<A>(p => p.Name, "姓名");
            excel.AddMapping<A>(p => p.Age, "年齡");

            // 只取 A1 到 B5 的資料 (要使用 WorksheetRange 不是 Worksheet 喔)
            // 你選的範圍 一定要包含 標頭喔 => 不然會取到 null
            var ExcelData_4 = excel.WorksheetRange<A>("A1", "B5", "使用者");
            Console.WriteLine("只找 A1 到 B5 的資料");
            foreach (var item in ExcelData_4)
            {
                string name = item.Name;
                int age = Convert.ToInt32(item.Age);
                Console.WriteLine($"姓名 = {name} , 年齡 = {age} ");
            }


            Console.Read();
        }
    }
}
