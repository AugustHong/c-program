using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using spire = Spire.Xls;
using excel = Microsoft.Office.Interop.Excel;
using System.IO;

/*
    有2種套件的作法：

    (1) NuGet 較大
            請去 NuGet 裝上 FreeSpire.XLS

    (2) NuGet 較輕量
            請去 NuGet 裝上 Microsoft.Office.Interop.Excel
 */

namespace XLS和XLSX互轉
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = System.IO.Directory.GetCurrentDirectory() + "\\";
            string sourcePath = rootPath + "Input\\";

            string xlsFile = sourcePath + "template.xls";
            string xlsxFile = sourcePath + "template.xlsx";

            #region FreeSpire.XLS (較大)

            // 副檔名
            string subName = xlsFile.Split('.').LastOrDefault().ToUpper();

            // 目標路徑
            string goalFileName = rootPath + "Spire\\template";

            Spire.Xls.Workbook workbook = new Spire.Xls.Workbook();
            workbook.LoadFromFile(xlsFile);

            switch (subName)
            {
                // 拿 Output 裡 製製的檔案去轉檔
                case "XLS":
                    goalFileName += ".xlsx";
                    workbook.SaveToFile(goalFileName, Spire.Xls.ExcelVersion.Version2016);
                    Console.WriteLine("轉成 XLSX 成功");
                    break;
                case "XLSX":
                    goalFileName += ".xls";
                    workbook.SaveToFile(goalFileName, Spire.Xls.ExcelVersion.Version97to2003);
                    Console.WriteLine("轉成 XLS 成功");
                    break;
                default:
                    Console.WriteLine($"副檔名錯誤，你的副檔名是 .{subName.ToLower()}");
                    break;
            }

            #endregion

            #region Interop.Excel (輕量)

            // 副檔名
            subName = xlsxFile.Split('.').LastOrDefault().ToUpper();

            // 目標路徑
            goalFileName = rootPath + "Interop\\template";

            Microsoft.Office.Interop.Excel._Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook book = excelApp.Workbooks.Open(xlsxFile);

            switch (subName)
            {
                // 拿 Output 裡 製製的檔案去轉檔
                case "XLS":
                    goalFileName += ".xlsx";

                    if (File.Exists(goalFileName))
                    {
                        File.Delete(goalFileName);
                    }

                    // 轉成 xlsx 用 Default (平常預設是這個，但原檔是 .xls => 所以要特別寫，不然它會依原檔的)
                    book.SaveAs(goalFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookDefault);
                    Console.WriteLine("轉成 XLSX 成功");
                    break;
                case "XLSX":
                    goalFileName += ".xls";

                    if (File.Exists(goalFileName))
                    {
                        File.Delete(goalFileName);
                    }

                    // 轉成 xls 用 Normal
                    book.SaveAs(goalFileName, Microsoft.Office.Interop.Excel.XlFileFormat.xlWorkbookNormal);
                    Console.WriteLine("轉成 XLS 成功");
                    break;
                default:
                    Console.WriteLine($"副檔名錯誤，你的副檔名是 .{subName.ToLower()}");
                    break;
            }

            book.Close();
            excelApp.Quit();

            #endregion

            Console.WriteLine("執行完成");
        }
    }
}
