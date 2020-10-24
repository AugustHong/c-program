using Aspose.Pdf;
using Microsoft.Office.Interop.Word;
using Spire.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xceed.Words.NET;

/*
    參考網圵： https://blog.aspose.com/2019/11/24/convert-pdf-to-word-doc-docx-in-csharp-vb-net/
    1. 去 NuGet 裝上 Aspose.Pdf

    其他相關的 PDF 轉 Word：
    PDF Focus .NET: https://sautinsoft.com/products/pdf-focus/index.php         Nuget 找不到
    Aspose.PDF: https://products.aspose.com/pdf/net                             當前使用
    Gembox: https://www.gemboxsoftware.com/document                             免費版只能20個段落
    Spire.PDF: https://www.e-iceblue.com/Introduce/pdf-for-net-introduce.html   應該跟 Aspose.PDF 一樣會產生多餘文字
    PdfBox： https://www.c-sharpcorner.com/UploadFile/07c1e7/convert-pdf-to-word-using-C-Sharp/ (去NuGet 裝上 PdfBox 和 DOCX)
 */

namespace PDF轉Word
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = System.IO.Directory.GetCurrentDirectory() + "\\";
            string pdfPath = rootPath + "source.pdf";

            string docPathForSpirePDF = rootPath + "outputDOC_Spire.doc";
            string docxPathForSpirePDF = rootPath + "outputDOCX_Spire.docx";

            // 使用 Spire.PDF
            // 優點： 較快、FreeSpire.PDF 不會有 多餘的文字
            // 缺點： 不知為何 空1格 會被處理的有點怪(不只這個，也很多格式轉出來很怪) + 免費版只能 10 頁
            PdfDocument doc = new PdfDocument();
            doc.LoadFromFile(pdfPath);
            doc.SaveToFile(docPathForSpirePDF, FileFormat.DOC);

            PdfDocument doc2 = new PdfDocument();
            doc2.LoadFromFile(pdfPath);
            doc2.SaveToFile(docxPathForSpirePDF, FileFormat.DOCX);

            // --------------------------------------------------------------------------------------------

            string docPathForAspose= rootPath + "outputDOC_Aspose.doc";
            string docxPathForAspose = rootPath + "outputDOCX_Aspose.docx";
            // Aspose.PDF
            // 優點： 處理的較完善(不會像 FreeSpire.PDF 空1個 有點問題)
            // 缺點： 處理 doc 會較慢、會出現多餘文字、只能 5 頁
            // 且產生後 最上面會有 Evaluation Only. Created with Aspose.PDF. Copyright 2002-2020 Aspose Pty Ltd. 文字出現
            Aspose.Pdf.Document pdfDocument = new Aspose.Pdf.Document(pdfPath);
            pdfDocument.Save(docPathForAspose, SaveFormat.Doc);

            Aspose.Pdf.Document pdfDocument2 = new Aspose.Pdf.Document(pdfPath);
            pdfDocument2.Save(docxPathForAspose, SaveFormat.DocX);

            // 其他設定
            /*
             Document pdfDocument = new Document(pdfPath);            

            // Save using save options
            // Create DocSaveOptions object
            DocSaveOptions saveOptions = new DocSaveOptions();

            // Set the recognition mode as Flow
            saveOptions.Mode = DocSaveOptions.RecognitionMode.Flow;

            // Set the Horizontal proximity as 2.5
            saveOptions.RelativeHorizontalProximity = 2.5f;

            // Enable the value to recognize bullets during conversion process
            saveOptions.RecognizeBullets = true;

            // Save the resultant DOC file
            pdfDocument.Save(rootPath + "saveOptionsOutput_out.doc", saveOptions);
             */

            // 因為有這個缺點(多餘文字) => 可以用 WordHelper 去 取代文字 把他拿掉
            string makeStr = "Evaluation Only. Created with Aspose.PDF. Copyright 2002-2020 Aspose Pty Ltd.";
            WordHelper wordDOC = new WordHelper();
            wordDOC.Open(docPathForAspose);
            wordDOC.Replace(makeStr, string.Empty);
            wordDOC.Save();

            WordHelper wordDOCX = new WordHelper();
            wordDOCX.Open(docxPathForAspose);
            wordDOCX.Replace(makeStr, string.Empty);
            wordDOCX.Save();

            // ---------------------------------------------------------------------------------------

            // PdfBox
            // 優點： 速度快，沒限制
            // 缺點： 拿直接拿格式來貼 => 所以 我按換頁(Ctrl+Enter) 他還是視同 \r\n => 出來的結果 變成在同一頁
            org.apache.pdfbox.pdmodel.PDDocument docPDFBox = org.apache.pdfbox.pdmodel.PDDocument.load(pdfPath);
            org.apache.pdfbox.util.PDFTextStripper textStrip = new org.apache.pdfbox.util.PDFTextStripper();
            string strPDFText = textStrip.getText(docPDFBox);
            docPDFBox.close();

            // 寫入
            //string docPathForpdfBox = rootPath + "outputDOC_pdfBox.doc"; // doc 的再自己用 docx 轉
            string docxPathForpdfBox = rootPath + "outputDOCX_pdfBox.docx";

            if (File.Exists(docxPathForpdfBox))
            {
                File.Delete(docxPathForpdfBox);
            }

            var word = DocX.Create(docxPathForpdfBox);
            word.InsertParagraph(strPDFText);
            word.Save();

            Console.WriteLine("執行完畢");
            Console.ReadLine();
        }
    }

    #region Word 取代文字 (可參考我 github 的 Word取代文字)

    public class WordHelper
    {
        public Microsoft.Office.Interop.Word._Document _word;
        public Microsoft.Office.Interop.Word._Application WordApp;

        // 固定參數
        public Object oMissing = System.Reflection.Missing.Value;

        public WordHelper()
        {

        }

        /// <summary>
		///  初使化
		/// </summary>
		/// <param name="dir">目錄路徑(最後要加上 \ 喔)</param>
		/// <param name="fileName">檔名(要加上副檔名)</param>
		public WordHelper(string dir, string fileName, bool visible = true)
        {
            // 初使化 Application
            this.InitWordApp(visible);

            if (!Directory.Exists(dir))
            {
                //建立檔案所在目錄
                Directory.CreateDirectory(dir);
            }

            string filePath = dir + fileName;

            if (!File.Exists(filePath))
            {
                // 如果不存在就建立一個新的
                this.CreateEmpty(dir, fileName);
            }
            else
            {
                // 開啟他
                this.Open(filePath);
            }
        }

        /// <summary>
		///  初使化 Application
		/// </summary>
		public void InitWordApp(bool visible)
        {
            WordApp = new Application();
            WordApp.Visible = visible;  // 啟不啟動 word 程式
        }

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

        /// <summary>
		///  讓游標移到當前的位置 (最後面的位置)
		/// </summary>
		public void MoveToCurrentSelect()
        {
            //移動游標文件末尾 (因為 新增文字 沒動到游標 => 故位置不對)
            object count = this._word.Paragraphs.Count;
            object WdLine = Microsoft.Office.Interop.Word.WdUnits.wdParagraph;
            WordApp.Selection.MoveDown(ref WdLine, ref count, ref oMissing);//移動焦點
        }

        /// <summary>
        ///  建立空的檔案
        /// </summary>
        /// <param name="dir">目錄路徑(最後要加上 \ 喔)</param>
        /// <param name="fileName">檔名(要加上副檔名)</param>
        public bool CreateEmpty(string dir, string fileName)
        {
            try
            {
                if (!Directory.Exists(dir))
                {
                    //建立檔案所在目錄
                    Directory.CreateDirectory(dir);
                }

                //建立Word文件(Microsoft.Office.Interop.Word)
                WordApp = new Microsoft.Office.Interop.Word.Application();
                Microsoft.Office.Interop.Word._Document WordDoc = WordApp.Documents.Add(ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                //儲存
                object filename = dir + fileName;
                WordDoc.SaveAs(ref filename, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing,
                    ref oMissing, ref oMissing, ref oMissing, ref oMissing, ref oMissing);

                this._word = WordDoc;

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
		///  寫入文字 (會自動斷行喔) => 不用在後面再寫 NewLine()
		/// </summary>
		/// <param name="Text">呈現的文字</param>
		/// <param name="Font_Bold">文字粗體長度</param>
		/// <param name="Font_Color">文字顏色(預設 黑色)(輸入 WdColor 裡的屬性， 例如 ： WdColor.wdColorBlack)</param>
		/// <param name="Font_Italic">文字斜體度</param>
		/// <param name="Font_Size">字型大小(預設 12)(輸入： 12f)</param>
		/// <param name="Font_Name">字型(預設 新細明體)(輸入： 新細明體)</param>
		/// <param name="haveNewLine">是否要先斷行再 寫入</param>
		/// <param name="endHaveNewLine">結束後是否要斷行(預設是 true)</param>
		/// <returns></returns>
		public bool WriteText(string Text = "", int Font_Bold = 0, WdColor Font_Color = WdColor.wdColorBlack, int Font_Italic = 0, float Font_Size = 12f, string Font_Name = "新細明體", bool haveNewLine = false, bool endHaveNewLine = true)
        {
            try
            {
                // 移動 游標
                this.MoveToCurrentSelect();

                // 插入段落
                if (haveNewLine)
                {
                    WordApp.Selection.TypeParagraph();
                }

                // 文字
                WordApp.Selection.Text = Text;
                WordApp.Selection.Font.Bold = Font_Bold;
                WordApp.Selection.Font.Color = Font_Color;
                WordApp.Selection.Font.Italic = Font_Italic;
                WordApp.Selection.Font.Size = Font_Size;
                WordApp.Selection.Range.Font.Name = Font_Name;

                this.MoveToCurrentSelect();

                // 如果沒斷行，就直接寫 WriteText的話，本身會被覆蓋
                if (endHaveNewLine)
                {
                    WordApp.Selection.TypeParagraph();//插入段落
                }


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
		///  關閉
		/// </summary>
		/// <returns></returns>
		public bool Close()
        {
            try
            {
                if (this._word != null)
                {
                    this._word.Close(ref oMissing, ref oMissing, ref oMissing);
                }

                if (this.WordApp != null)
                {
                    this.WordApp.Quit(ref oMissing, ref oMissing, ref oMissing);
                }

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
		///  先儲存再關閉
		/// </summary>
		/// <returns></returns>
		public bool SaveAndClose()
        {
            try
            {
                //儲存
                this._word.Save();
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
    }

    #endregion
}
