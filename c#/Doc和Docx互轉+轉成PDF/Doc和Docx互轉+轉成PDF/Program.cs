using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    去 Nuget 裝上 Microsoft.Office.Interop.Word
 */

namespace Doc和Docx互轉_轉成PDF
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = System.IO.Directory.GetCurrentDirectory() + "\\";
            string sourcePath = rootPath + "source.docx";
            string docPath = rootPath + "output.doc";
            string docxPath = rootPath + "output.docx";
            string pdfPath = rootPath + "output.pdf";

            ConvertToDoc(sourcePath, docPath);
            ConvertToDocx(sourcePath, docxPath);
            ConvertToPdf(sourcePath, pdfPath);

            // 如果需要砍掉 word process 可參考我的 WriteWord 裡的內容

            Console.WriteLine("執行成功");
        }

        /// <summary>
        ///  轉成 Docx
        /// </summary>
        /// <returns></returns>
        static bool ConvertToDocx(string sourcePath, string goalPath)
        {
            Console.WriteLine($"轉備將 {sourcePath} 轉成 {goalPath}");

            // 如果存在了 就先刪掉
            if (File.Exists(goalPath))
            {
                File.Delete(goalPath);
            }

            try
            {
                Application wordApp = new Application() { Visible = false };
                // 這裡路徑要給明確，不然他吃不到
                Document doc = wordApp.Documents.Open(sourcePath);
                doc.SaveAs2(goalPath, WdSaveFormat.wdFormatDocumentDefault);
                Console.WriteLine("轉成 DOCX 成功");
                doc.Close();
                wordApp.Quit();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"轉成 DOCX 失敗， 原因 = {ex.Message}");
                return false;
            }
        }

        /// <summary>
        ///  轉成 Doc
        /// </summary>
        /// <returns></returns>
        static bool ConvertToDoc(string sourcePath, string goalPath)
        {
            Console.WriteLine($"轉備將 {sourcePath} 轉成 {goalPath}");

            // 如果存在了 就先刪掉
            if (File.Exists(goalPath))
            {
                File.Delete(goalPath);
            }

            try
            {
                Application wordApp = new Application() { Visible = false };
                // 這裡路徑要給明確，不然他吃不到
                Document doc = wordApp.Documents.Open(sourcePath);
                doc.SaveAs2(goalPath, WdSaveFormat.wdFormatDocument97);
                Console.WriteLine("轉成 DOC 成功");
                doc.Close();
                wordApp.Quit();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"轉成 DOC 失敗， 原因 = {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// 轉成 Pdf
        /// </summary>
        /// <returns></returns>
        static bool ConvertToPdf(string sourcePath, string goalPath)
        {
            Console.WriteLine($"轉備將 {sourcePath} 轉成 {goalPath}");

            // 如果存在了 就先刪掉
            if (File.Exists(goalPath))
            {
                File.Delete(goalPath);
            }

            try
            {
                Application wordApp = new Application() { Visible = false };
                // 這裡路徑要給明確，不然他吃不到
                Document doc = wordApp.Documents.Open(sourcePath);
                doc.SaveAs2(goalPath, WdSaveFormat.wdFormatPDF);
                Console.WriteLine("轉成 PDF 成功");
                doc.Close();
                wordApp.Quit();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"轉成 PDF 失敗， 原因 = {ex.Message}");
                return false;
            }
        }
    }
}
