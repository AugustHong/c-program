using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    1.先去 NuGet 裝上 Microsoft.Office.Interop.Excel
 */

using Microsoft.Office.Interop.Excel;

// 要使用 剪貼簿功能
// 請去 加入參考/組件/System.Windows.Forms
using System.Windows.Forms;
using System.IO;

namespace Excel基本操作_取代文字__用Microsoft.Office.Interop.Excel_
{
    class Program
    {
        // 要使用剪貼簿功能要加的
        [STAThread]
        static void Main(string[] args)
        {
            string rootPath = System.IO.Directory.GetCurrentDirectory() + "\\";
            string fileName = "result.xlsx";

            string filePath = rootPath + fileName;

            // 建立一個空的(只有一個Sheet的Excel)
            CreateEmptyExcel(filePath);

            // 開啟 Excel
            Microsoft.Office.Interop.Excel._Application resultExcelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook resultBook = resultExcelApp.Workbooks.Open(filePath);
            Microsoft.Office.Interop.Excel.Worksheet resultCurrentSheet = resultBook.Worksheets[1];  // 從 1 開始
            string SheetName = resultCurrentSheet.Name;

            // 共有幾個 sheet
            int sourceSheetCount = resultBook.Worksheets.Count;

            // 複製 Sheet
            // 複製 (要先設為 Active)
            resultCurrentSheet.Activate();
            resultCurrentSheet.Cells.Copy();
            // 等個幾秒
            System.Threading.Thread.Sleep(1000);

            // 做複製完後，會變動到 Excel (所以關閉的時候，要設定不要管 變動)
            //resultBook.Close(false);
            //resultExcelApp.Quit();


            // 貼上的寫法 (這邊就拿原本的實作，這個 Program 不可執行，僅作教學)
            resultCurrentSheet.Paste();
            //resultCurrentSheet.PasteSpecial();
            // 貼上完要記得清空 剪貼簿
            Clipboard.Clear();

            // 新增一個 Sheet
            /*
                參數1：此sheet放在誰之前 (如不要， 用 Type.Missing)
                參數2：此sheet放在誰之後 (就設 當前的sheet之後)
                參數3：要 新增幾個
                參數4：就直接給 Type.Missing
             */
            resultBook.Sheets.Add(Type.Missing, resultCurrentSheet, 1, Type.Missing);

            // 取代文字
            resultCurrentSheet.Cells.Replace($"aaa", "123");

            // 取 Cell值 (沒試過，但應該是這樣)
            string t = (string)resultCurrentSheet.Range["A1"].Value;
            resultCurrentSheet.Range["B2"].Value = "123";

            // 關閉
            resultBook.Close();
            resultExcelApp.Quit();
        }

        /// <summary>
        ///  建立 新的空Excel
        /// </summary>
        /// <param name="filePath"></param>
        static void CreateEmptyExcel(string filePath)
        {
            // 如果檔案存在先刪掉
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            // 建立 空的 Excel(會預設有1個sheet)
            _Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Workbook book = excelApp.Workbooks.Add();  // 直接新增

            // 副檔名
            string subName = filePath.Split('.').Last().ToUpper();
            if (subName == "XLS")
            {
                // 要存成 xls 
                book.SaveAs(filePath, XlFileFormat.xlWorkbookNormal);
            }
            else
            {
                // 預設是 xlsx
                book.SaveAs(filePath, XlFileFormat.xlWorkbookDefault);
            }

            book.Close();
            excelApp.Quit();
        }
    }
}
