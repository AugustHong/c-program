using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    參考網圵： https://www.itread01.com/article/1431663819.html
              https://www.itdaan.com/tw/fb77df293e8ea561227d5de9aa951ca7
    1. 去 NuGet 裝上 iTextSharp
    2. 能轉得很像 但缺點： 
                (a)頁首 頁尾 都放在一起 
                (b)空行 沒有出現 (所以變成 全部擠在一起，很難看)
                (c)有些怪字 + 圖片 出不來 (沒辦法，說好是轉文字檔了嘛~)
                (d)好像有長度上限(因為我有個378頁的，但他 只有出現至 330頁的 上半部)
 */

namespace PDF轉文字檔
{
    class Program
    {
        static void Main(string[] args)
        {
            // 先讀取出檔案
            string rootPath = System.IO.Directory.GetCurrentDirectory() + "\\";
            string sourcePath = rootPath + "Input";
            DirectoryInfo sourceDir = new DirectoryInfo(sourcePath);

            List<string> useFile = new List<string> { ".PDF" };
            List<string> sourceFileNameList = sourceDir.EnumerateFiles().ToList().Where(w => useFile.Where(u => w.FullName.ToUpper().Contains(u)).Count() > 0).Select(w => w.Name).ToList();

            Console.WriteLine($"已找出符合的項目 {sourceFileNameList.Count()} 筆");

            sourcePath = sourcePath + "\\";
            // 目標路徑
            string goalPath = rootPath + "Output\\";

            foreach (var fileName in sourceFileNameList)
            {
                // 取出名稱
                List<string> tmp = fileName.Split('.').ToList();
                string subName = tmp.LastOrDefault();  // 副檔名
                tmp.Remove(subName);
                string name = string.Join(".", tmp);   // 名稱

                string sourceFilePath = sourcePath + fileName;
                string goalFilePath = goalPath + name + ".txt";

                //開始轉
                Console.WriteLine($"開始轉換 {fileName}");
                PDFParseToTxtHelper.ParsePdfToTxt(sourceFilePath, goalFilePath);
            }

            Console.WriteLine("-----------------------------------------------------------------");
            Console.WriteLine("執行完畢");
            Console.ReadLine();
        }
    }

    public static class PDFParseToTxtHelper
    {
        /// <summary>
        ///  PDF 轉成 文字檔
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="goalFilePath"></param>
        /// <returns></returns>
        public static bool ParsePdfToTxt(string sourceFilePath, string goalFilePath)
        {
            if (!File.Exists(sourceFilePath))
            {
                return false;
            }

            try
            {
                if (File.Exists(goalFilePath))
                {
                    File.Delete(goalFilePath);
                }

                PdfReader reader = new PdfReader(sourceFilePath);
                StreamWriter output = new StreamWriter(new FileStream(goalFilePath, FileMode.Create));
                int pageCount = reader.NumberOfPages;

                for (int pg = 1; pg <= pageCount; pg++)
                {
                    ITextExtractionStrategy strategy = new SimpleTextExtractionStrategy();
                    string text = string.Empty;

                    try
                    {
                        text = PdfTextExtractor.GetTextFromPage(reader, pg, strategy);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"第{pg}頁 讀入發生問題： {ex.Message}");
                    }

                    output.WriteLine(text);

                    // 做 頁 和 頁 之間的分行
                    output.WriteLine($"\n--------第{pg}頁----------\n");
                }

                output.Flush();
                output.Close();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
