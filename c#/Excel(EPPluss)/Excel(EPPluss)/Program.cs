using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 來 using
using OfficeOpenXml;
using OfficeOpenXml.Style;
using Spire.Xls;

// 寫檔的
using System.IO;

// 接 Controller傳入的
using System.Web;

/*
    1. 先去 NuGet 裝上
     (a) DocumentFormat.OpenXml
     (b) EPPlus
     (c) Spire.Xls (轉 ODS用的 => 但要認證，否則會多一頁訊息頁 => 看你要不要用)
*/

namespace Excel_EPPluss_
{
    class Program
    {
        static void Main(string[] args)
        {
        }


        /// <summary>
        ///  讀實體檔，並寫成實體檔
        /// </summary>
        public static void Write()
        {
            string filename = "test.xlsx";

            // 防弱掃的
            filename.Replace("..", string.Empty);
            filename.Replace("//", string.Empty);
            filename.Replace("\\", string.Empty);

            string path = @"" + filename;

            // 這是要傳回 byte[] 時才要用的
            MemoryStream memory = new MemoryStream();

            // 先把檔案讀出來，接下來就要寫了
            // 開啟出來的 都是用 "另存新檔" 來存喔！ 所以不用擔心會直接改到原本的
            using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                // 看是要哪種認證，基本上 NonCommercial 就行了
                // If you are a commercial business and have
                // purchased commercial licenses use the static property
                // LicenseContext of the ExcelPackage class:
                ExcelPackage.LicenseContext = LicenseContext.Commercial;

                // If you use EPPlus in a noncommercial context
                // according to the Polyform Noncommercial license:
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                // 固定寫法 (讀取頁面)
                using (ExcelPackage excel = new ExcelPackage(fs))
                {
                    // 取得Sheet1 (都是從1開始，不是0喔)
                    // 第2次自己做實測 卻又從 0 開始 
                    // 下次自己先測一下，會是從哪開始 
                    ExcelWorksheet sheet = excel.Workbook.Worksheets[1]; 

                    // 也可再改變 sheet 的名稱
                    sheet.Name = "第一張表";

                    // 最強招(複製) => 等同於只要做一頁，後面都複製即可
                    excel.Workbook.Worksheets.Copy(sheet.Name, "測試");  // 把 sheet.Name 這一個 複製到 一個新的 ，且新的 Name 叫做 測試
                    ExcelWorksheet newSheet = excel.Workbook.Worksheets[2];   // 剛新增完(所以 2 就是剛才新增的資料了)

                    // excel.Workbook.Worksheets 是個 List 集合，所以也可以用Find Delete ……等;
                    ExcelWorksheet testSheet = excel.Workbook.Worksheets.Where(e => e.Name.Contains("測試")).FirstOrDefault();
                    if (testSheet != null)
                    {
                        excel.Workbook.Worksheets.Delete(testSheet);
                    }                   

                    sheet.InsertRow(3, 5);   //從第3行加5行
                    sheet.InsertColumn(2, 3);  // 從第2 col 加 3個 col

                    // Cells[row, col] => Cells[1, 1] = 儲存格的 A1； Cells[1, 2] = 儲存格的 B1
                    // 都是從 1 開始數喔
                    int i = 1;
                    sheet.Cells[(i), 1].Value = "第一項";
                    sheet.Cells[(i), 2].Value = "第二項";
                    sheet.Cells[(i), 3].Value = "第三項";
                    sheet.Cells[(i), 4].Value = "第四項";
                    sheet.Cells[(i), 5].Value = "第五項";
                    sheet.Cells[(i), 6].Value = "第六項";
                    sheet.Cells[(i), 7].Value = "第七項";

                    // 傳回 byte[] 用的
                    //excel.SaveAs(memory);

                    // 寫入實體檔
                    FileInfo file = new FileInfo(@"new.xlsx");
                    excel.SaveAs(file);
                }
            }
        }


        /// <summary>
        ///  接收 Controller 傳入的檔案 + 回傳 byte[]
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public static byte[] Write(HttpPostedFileBase file, string fileType = "excel")
        {
            // 回傳要用的
            MemoryStream memory = new MemoryStream();

            try
            {

                // 看是要哪種認證，基本上 NonCommercial 就行了
                // If you are a commercial business and have
                // purchased commercial licenses use the static property
                // LicenseContext of the ExcelPackage class:
                ExcelPackage.LicenseContext = LicenseContext.Commercial;

                // If you use EPPlus in a noncommercial context
                // according to the Polyform Noncommercial license:
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

                // 讀檔並組出 資料
                using (ExcelPackage excel = new ExcelPackage(file.InputStream))
                {
                    // 取得Sheet1 (都是從1開始，不是0喔)
                    // 第2次自己做實測 卻又從 0 開始 
                    // 下次自己先測一下，會是從哪開始
                    ExcelWorksheet sheet = excel.Workbook.Worksheets[1];

                    // 開始讀檔
                    int startRowIndex = sheet.Dimension.Start.Row; // 起始列(從1開始 => 如果有標題列，那會是標題列那)
                    int endRowIndex = sheet.Dimension.End.Row; // 結束列

                    // 自已設定的(看你要從第幾個col 讀到第幾個 col  => 一樣從 1 開始)
                    int startColumn = 1;
                    int endColumn = 7;

                    // 如果 本身有標題的話
                    startRowIndex += 1;

                    // 先確認 開始不能小於 結束
                    if (startRowIndex <= endRowIndex)
                    {
                        // 開始組出來
                        for (int currentRow = startRowIndex; currentRow <= endRowIndex; currentRow++)
                        {
                            // 抓出當前的資料範圍
                            ExcelRange range = sheet.Cells[currentRow, startColumn, currentRow, endColumn];

                            // 全部儲存格是完全空白時則跳過
                            if (range.Any(c => !string.IsNullOrEmpty(c.Text)) == false)
                            {
                                // 略過此列
                                continue;
                            }

                            // 接收資料
                            List<string> data = new List<string>();
                            data.Add(sheet.Cells[currentRow, 1].Text.Trim());
                            data.Add(sheet.Cells[currentRow, 2].Text.Trim());
                            data.Add(sheet.Cells[currentRow, 3].Text.Trim());
                            data.Add(sheet.Cells[currentRow, 4].Text.Trim());
                        }
                    }

                    // 上面是取資料--------------------------------------下面就再寫一次資料

                    // 實務上不會這麼做，但我只是把筆記都寫起來才這麼寫的
                    // 我要覆蓋掉原本的再寫一次檔案

                    // 設定 文字置中
                    var _center = ExcelHorizontalAlignment.Center;

                    // 來做點特別的 ( 合併儲存格) => 詳細看下面
                    // 合併儲存格中 (一樣是用 Cells ，只是 是 從哪一個到 另一個 儲存格中間都會併)
                    //[1, 1] 開始 到 [1, 12] 全合併 => 後面的 1, 1 就放一開始的 1, 1 即可 ， 最後面就是值
                    HeadStyle(sheet.Cells[1, 1, 1, 12], _center, 12, sheet, 1, 1, string.Empty);
                    HeadStyle(sheet.Cells[2, 1, 2, 12], _center, 12, sheet, 2, 1, "測試");

                    // 因為上面 1 和 2 是 主標題列 => 所以接下來寫的要從第3行
                    // 這裡是 副標題列
                    int i = 3;
                    sheet.Cells[(i), 1].Value = "第一項";
                    sheet.Cells[(i), 2].Value = "第二項";
                    sheet.Cells[(i), 3].Value = "第三項";
                    sheet.Cells[(i), 4].Value = "第四項";
                    sheet.Cells[(i), 5].Value = "第五項";
                    sheet.Cells[(i), 6].Value = "第六項";
                    sheet.Cells[(i), 7].Value = "第七項";

                    // 內容
                    i+= 1;
                    sheet.Cells[(i), 1].Value = 1;
                    sheet.Cells[(i), 2].Value = 2;
                    sheet.Cells[(i), 3].Value = 3;
                    sheet.Cells[(i), 4].Value = 4;
                    sheet.Cells[(i), 5].Value = 5;
                    sheet.Cells[(i), 6].Value = 6;
                    sheet.Cells[(i), 7].Value = 7;

                    // 存進 memory中
                    excel.SaveAs(memory);

                    // 如檔案是 ods 時
                    if (fileType == "ods")
                    {
                        Workbook workbook = new Workbook();
                        workbook.LoadFromStream(memory);
                        workbook.SaveToStream(memory, FileFormat.ODS);
                    }

                    if (fileType == "pdf")
                    {
                        Workbook workbook = new Workbook();
                        workbook.LoadFromStream(memory);
                        workbook.SaveToStream(memory, Spire.Xls.FileFormat.PDF);
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception();
            }

            return memory.ToArray();
        }


        /*
            用上面的方法 ， 得到回傳的 byte[] 之後就是給檔案ConectType和檔名
            
            Controller 長這樣：

            public ActionResult Write(HttpPostedFileBase file, string fileType = "excel"){
                bool result = false;
                string contentType = string.Empty;

                byte[] content = Write(file, fileType);
                string filename = "測試";
                switch (fileType)
                {
                    // 在這邊給 副檔名 和 contentType 
                    case "excel": contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet"; filename += ".xlsx"; result = true; break;
                    case "ods": contentType = "application/vnd.oasis.opendocument.spreadsheet"; filename += ".ods"; result = true; break;
                    case "pdf": contentType = "application/pdf"; filename += ".pdf"; result = true; break;
                    default: break;
                }

                if (result)
                {
                    return this.File(content, contentType, filename);
                }
                else
                {
                    return this.RedirectToAction("Index");
                }
             }
        */


        /// <summary>
        /// 設定標題列的style (做儲存格合併 + 設定儲存格格式)
        /// </summary>
        /// <param name="excelRange">內容範圍</param>
        /// <param name="align">對齊方式</param>
        /// <param name="size">字型大小</param>
        /// <param name="worksheet">ExcelWorksheet物件</param>
        /// <param name="x">X軸</param>
        /// <param name="y">Y軸</param>
        /// <param name="data">要寫入儲存格中的資料</param>
        private static void HeadStyle(ExcelRange excelRange, ExcelHorizontalAlignment align, float size, ExcelWorksheet worksheet, int x, int y, object data)
        {
            excelRange.Merge = true;
            excelRange.Style.HorizontalAlignment = align;
            excelRange.Style.Font.Size = size;
            excelRange.Style.Font.Name = "標楷體";
            worksheet.Cells[x, y].Value = data;
        }

        /// <summary>
        /// 資料Style (做儲存格合併 + 設定儲存格格式)
        /// </summary>
        /// <param name="excelRange">內容範圍</param>
        /// <param name="align">對齊方式</param>
        /// <param name="size">字型大小</param>
        /// <param name="worksheet">ExcelWorksheet物件</param>
        /// <param name="x">X軸</param>
        /// <param name="y">Y軸</param>
        /// <param name="data">要寫入儲存格中的資料</param>
        private static void TableStyle(ExcelRange excelRange, ExcelHorizontalAlignment align, float size, ExcelWorksheet worksheet, int x, int y, object data)
        {
            excelRange.Merge = true;
            excelRange.Style.HorizontalAlignment = align;
            excelRange.Style.VerticalAlignment = ExcelVerticalAlignment.Center;
            excelRange.Style.Font.Size = size;
            excelRange.Style.Font.Name = "標楷體";

            excelRange.Style.Border.Top.Style = ExcelBorderStyle.Medium;
            excelRange.Style.Border.Bottom.Style = ExcelBorderStyle.Medium;
            excelRange.Style.Border.Left.Style = ExcelBorderStyle.Medium;
            excelRange.Style.Border.Right.Style = ExcelBorderStyle.Medium;
            worksheet.Cells[x, y].Value = data;
        }
    }
}
