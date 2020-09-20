using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    先去 NuGet 裝上 Microsoft.Office.Interop.Word 和 Microsoft.Office.Interop.Excel
 */

// word
using Microsoft.Office.Interop.Word;

// excel (因為 EPPLUS 不能貼上 => 所以改別隻套件來寫)
// 這隻就只為了這件事，不然平常用 EPPLUS 比較好
// 所以就不寫這隻的 做法筆記了
using excel = Microsoft.Office.Interop.Excel;

// 這種 Excel 的寫法
// 1. https://www.itread01.com/p/607028.html
// 2. https://dotblogs.com.tw/yc421206/2012/03/09/70624

namespace Word的表格複製至Excel中
{
    class Program
    {
        static void Main(string[] args)
        {
            // 先準備 word
            // 要先複製一份出去再開啟
            string rootPath = System.IO.Directory.GetCurrentDirectory() + "\\";
            string sourceFilePath = rootPath + "Input\\input.docx";
            string tmpFilePath = rootPath + "Output\\tmp.docx";

            File.Copy(sourceFilePath, tmpFilePath, true);

            // 開啟 word
            WordHelper word = new WordHelper();
            word.Open(tmpFilePath);

            // Table 數
            int tableCount = word._word.Application.ActiveDocument.Tables.Count;

            // 這邊只做範例 => 所以拿第一張表示範而已 (照理來說要用 try catch ， 但示範就不用了)
            // 把 Word 的 Copy 起來 (是從 1 開始)
            Table t = word._word.Application.ActiveDocument.Tables[1];
            if (t != null)
            {
                // 複製至剪貼簿
                t.Select();
                t.Range.Copy();

                // 開啟 Excel 的範本檔 (一樣要 try catch ,  但示範不做)
                excel._Application excelApp = new Microsoft.Office.Interop.Excel.Application();
                excel.Workbook book = excelApp.Workbooks.Open(rootPath + "Input\\tmp.xlsx");
                excel.Worksheet sheet1 = book.Sheets[1];         // 從 1 開始算
                sheet1.Name = $"範本";

                // 新增一頁 sheet
                // Add (在哪張表之前, 在哪張表之後, 新增幾張表, xxx)
                book.Sheets.Add(Type.Missing, sheet1, 1, Type.Missing);
                excel.Worksheet sheet2 = book.Worksheets[2];
                sheet2.Name = $"新增的sheet";

                // 貼上 (因為上面有 t.Range.Copy() => 所以已經在剪貼簿上了)
                sheet2.Paste();

                string excelGoalFilePath = rootPath + "Output\\result.xlsx";

                // 如果已存在先砍掉
                if (File.Exists(excelGoalFilePath))
                {
                    File.Delete(excelGoalFilePath);
                }

                // 儲存
                book.SaveAs(excelGoalFilePath);
                book.Close();
                excelApp.Quit();
            }

            // 關閉 word
            word.Close();

            // 刪掉 暫存 word 檔
            File.Delete(tmpFilePath);

            Console.WriteLine("執行完成");
            Console.ReadLine();
        }
    }

    #region Word Helper

    public class WordHelper
    {
        public Microsoft.Office.Interop.Word._Document _word;
        public Microsoft.Office.Interop.Word._Application WordApp;

        // 固定參數
        public Object oMissing = System.Reflection.Missing.Value;

        /// <summary>
        /// 關閉Word程序
        /// </summary>
        public void KillWinword()
        {
            var p = Process.GetProcessesByName("WINWORD");
            if (p.Any()) p[0].Kill();
        }

        public bool Close()
        {
            try
            {
                this._word.Close(ref oMissing, ref oMissing, ref oMissing);
                this.WordApp.Quit(ref oMissing, ref oMissing, ref oMissing);

                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// 開啟word文件
        /// </summary>
        /// <param name=”filePath”></param>
        public void Open(string filePath)
        {

            WordApp = new Application();
            object file = filePath;
            _word = WordApp.Documents.Open(
                 ref file, ref oMissing, ref oMissing,
                 ref oMissing, ref oMissing, ref oMissing,
                 ref oMissing, ref oMissing, ref oMissing,
                 ref oMissing, ref oMissing, ref oMissing,
                 ref oMissing, ref oMissing, ref oMissing, ref oMissing);
        }

        /// <summary>
        /// 替換word中的文字
        /// </summary>
        /// <param name=”strOld”>查詢的文字</param>
        /// <param name=”strNew”>替換的文字</param>
        public void Replace(string strOld, string strNew)
        {
            //替換全域性Document
            WordApp.Selection.Find.ClearFormatting();
            WordApp.Selection.Find.Replacement.ClearFormatting();
            WordApp.Selection.Find.Text = strOld;
            WordApp.Selection.Find.Replacement.Text = strNew;

            object objReplace = Microsoft.Office.Interop.Word.WdReplace.wdReplaceAll;
            WordApp.Selection.Find.Execute(ref oMissing, ref oMissing, ref oMissing,
                                       ref oMissing, ref oMissing, ref oMissing,
                                       ref oMissing, ref oMissing, ref oMissing,
                                       ref oMissing, ref objReplace, ref oMissing,
                                       ref oMissing, ref oMissing, ref oMissing);
            //替換頁尾的字
            foreach (Microsoft.Office.Interop.Word.Section wordSection in _word.Sections)
            {
                Microsoft.Office.Interop.Word.Range footerRange = wordSection.Footers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                footerRange.Find.ClearFormatting();
                footerRange.Find.Replacement.ClearFormatting();
                footerRange.Find.Text = strOld;
                footerRange.Find.Replacement.Text = strNew;
                footerRange.Find.Execute(ref oMissing, ref oMissing, ref oMissing,
                                       ref oMissing, ref oMissing, ref oMissing,
                                       ref oMissing, ref oMissing, ref oMissing,
                                       ref oMissing, ref objReplace, ref oMissing,
                                       ref oMissing, ref oMissing, ref oMissing);
            }

            //替換頁首的字
            foreach (Microsoft.Office.Interop.Word.Section section in _word.Sections)
            {
                Microsoft.Office.Interop.Word.Range headerRange = section.Headers[Microsoft.Office.Interop.Word.WdHeaderFooterIndex.wdHeaderFooterPrimary].Range;
                headerRange.Find.ClearFormatting();
                headerRange.Find.Replacement.ClearFormatting();
                headerRange.Find.Text = strOld;
                headerRange.Find.Replacement.Text = strNew;
                headerRange.Find.Execute(ref oMissing, ref oMissing, ref oMissing,
                                       ref oMissing, ref oMissing, ref oMissing,
                                       ref oMissing, ref oMissing, ref oMissing,
                                       ref oMissing, ref objReplace, ref oMissing,
                                       ref oMissing, ref oMissing, ref oMissing);
            }

            //文字框
            Microsoft.Office.Interop.Word.StoryRanges storyRanges = _word.StoryRanges;
            foreach (Microsoft.Office.Interop.Word.Range range in storyRanges)
            {
                Microsoft.Office.Interop.Word.Range rangeFlag = range;
                if (Microsoft.Office.Interop.Word.WdStoryType.wdTextFrameStory == rangeFlag.StoryType)
                {
                    while (rangeFlag != null)
                    {
                        rangeFlag.Find.ClearFormatting();
                        rangeFlag.Find.Replacement.ClearFormatting();
                        rangeFlag.Find.Text = strOld;
                        rangeFlag.Find.Replacement.Text = strNew;
                        rangeFlag.Find.Execute(ref oMissing, ref oMissing, ref oMissing,
                                               ref oMissing, ref oMissing, ref oMissing,
                                               ref oMissing, ref oMissing, ref oMissing,
                                               ref oMissing, ref objReplace, ref oMissing,
                                               ref oMissing, ref oMissing, ref oMissing);
                        rangeFlag = range.NextStoryRange;
                    }
                }
            }

        }

        /// <summary>
        /// 儲存
        /// </summary>
        public void Save(bool disposet = true)
        {
            if (disposet == false)
            {
                _word.Save();
            }
            else
            {
                this.Save(false);
                this.KillWinword();
            }
        }
    }

    #endregion

    #region 字串分割 (參考我的 github 的 Hong.StringHelper)

    public static class StringHelper
    {
        public static List<string> Split(this string source, string splitStr)
        {
            List<string> result = new List<string>();

            string tmpSource = source;

            // 如果是 空的，就直接回傳 空字串
            if (string.IsNullOrEmpty(source))
            {
                return new List<string> { "" };
            }

            // 如果切割字串是 null 回傳整個
            if (splitStr == null)
            {
                result.Add(source);
                return result;
            }

            int len = tmpSource.Length;

            // 如果 切割字串是 空字串，就每個字母來切
            if (splitStr == string.Empty)
            {
                for (var i = 0; i < len; i++)
                {
                    string tmp = source.Substring(i, 1);
                    result.Add(tmp);
                }
                return result;
            }

            // 其餘照著切
            int splitStrLen = splitStr.Length;

            // 判斷是否有進去
            bool haveI = false;

            // 位置
            int pos = tmpSource.IndexOf(splitStr);

            // 直到結束
            while (pos >= 0)
            {
                haveI = true;
                string tmp = string.Empty;

                if (pos == 0)
                {
                    tmp = string.Empty;
                }
                else
                {
                    tmp = tmpSource.Substring(0, pos);
                }

                result.Add(tmp);

                // 算出要延後幾位
                int diff = pos + splitStrLen;

                // 切割 (讓剩下的繼續跑)
                tmpSource = tmpSource.Substring(diff);

                // 重算位置
                pos = tmpSource.IndexOf(splitStr);

                // 如果 最後一次砍完剩下 空字串 => 要 push 進去
                if (string.IsNullOrEmpty(tmpSource))
                {
                    result.Add(string.Empty);
                }
                else
                {
                    if (pos < 0)
                    {
                        result.Add(tmpSource);
                    }
                }
            }

            // 如果一開始就查不到 => 直接回傳 自己
            if (haveI == false)
            {
                result.Add(source);
            }

            return result;
        }
    }

    #endregion
}
