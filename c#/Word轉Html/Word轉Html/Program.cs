using Microsoft.Office.Interop.Word;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    參考網圵： https://stackoverflow.com/questions/2266097/convert-from-word-document-to-html
    本次採用  Microsoft.Office.Interop.Word
    1. 去 Nuget 裝上 Microsoft.Office.Interop.Word
    2. 可以參考我 github 的 doc和docx 互轉 + 轉成 PDF (是用相同的手法做的)
 */

namespace Word轉Html
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = System.IO.Directory.GetCurrentDirectory() + "\\";
            string sourcePath = rootPath + "source.docx";
            string htmlPath = rootPath + "output.html";

            ConvertToHtml(sourcePath, htmlPath);

            // 如果需要砍掉 word process 可參考我的 WriteWord 裡的內容

            Console.WriteLine("-----------------------------------------------------------------");

            // 讀取出來內容 (要用 big5 去讀，不然中文有亂碼)
            string html;
            using (StreamReader sr = new StreamReader(htmlPath, System.Text.Encoding.GetEncoding("big5")))
            {
                // 其實主 要的在 <div class=WordSection1 這個 div 裡面的
                html = sr.ReadToEnd();
            }
            Console.WriteLine(html);

            Console.WriteLine("執行成功");
            Console.ReadLine();
        }

        /// <summary>
        ///  轉成 Html
        /// </summary>
        /// <returns></returns>
        static bool ConvertToHtml(string sourcePath, string goalPath)
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
                doc.SaveAs2(goalPath, WdSaveFormat.wdFormatHTML);
                Console.WriteLine("轉成 HTML 成功");
                doc.Close();
                wordApp.Quit();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"轉成 HTML 失敗， 原因 = {ex.Message}");
                return false;
            }
        }
    }
}
