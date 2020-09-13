using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Office.Interop.Word;

/*
    1. 先去 NuGet 裝上 Microsoft.Office.Interop.Word 
    2. 有這種的必須要有裝上 Office Word (且要可以修改，像我自己當時的不能改 => 所以頁首頁尾才看起來沒出現)
    參考網圵：
    1. https://codertw.com/%E7%A8%8B%E5%BC%8F%E8%AA%9E%E8%A8%80/533316/

    !! 可以和我的github另一隻 WriteWord(Microsoft.Office.Interop.Word) 做配合
    這邊就是進階版，可以把下面的 Helper 併到 上面那隻的 WordHelper裡面
*/

namespace word取代文字
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = "source.docx";

            string rootPath = System.IO.Directory.GetCurrentDirectory() + "\\";
            string sourcePath = rootPath + "Input\\" + fileName;
            string goalPath = rootPath + "Output\\" + fileName;

            // 先複製一份做備份
            System.IO.File.Copy(sourcePath, goalPath, true);

            // 開啟備份的檔
            WordHelper word = new WordHelper();
            word.Open(goalPath);
            word.Replace("大家好", "你好");
            word.Replace("111", "***");
            word.Save();

            Console.WriteLine("取代 Word 完畢");
            Console.ReadLine();
        }
    }

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
}
