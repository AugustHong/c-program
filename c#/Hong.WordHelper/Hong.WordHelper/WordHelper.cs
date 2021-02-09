using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Web;

using System.Drawing;
using System.Drawing.Imaging;

using Microsoft.Office.Interop.Word;

namespace Hong.WordHelper
{
    public class WordHelper
    {
        public Microsoft.Office.Interop.Word._Document _word;
        public Microsoft.Office.Interop.Word._Application WordApp;

        // 固定參數
        public Object oMissing = System.Reflection.Missing.Value;

        /// <summary>
        ///  初使化
        /// </summary>
        /// <param name="dir">目錄路徑(最後要加上 \ 喔)</param>
        /// <param name="fileName">檔名(要加上副檔名)</param>
        public WordHelper(string dir, string fileName, bool visible = false)
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
        ///  儲存
        /// </summary>
        /// <returns></returns>
        public bool Save(bool disposet = true)
        {
            try
            {
                if (disposet == false)
                {
                    _word.Save();
                }
                else
                {
                    this.Save(false);
                    KillWinword();
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

        /// <summary>
        /// 關閉Word程序
        /// </summary>
        public static void KillWinword()
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
        ///  編輯 頁首 頁尾
        /// </summary>
        /// <param name="headerText">頁首文字</param>
        /// <param name="footerText">頁尾文字</param>
        /// <returns></returns>
        public bool WriteHeaderFooter(string headerText, string footerText)
        {
            try
            {
                ////新增頁首方法一：
                //WordApp.ActiveWindow.View.Type = WdViewType.wdOutlineView;
                //WordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekPrimaryHeader;
                //WordApp.ActiveWindow.ActivePane.Selection.InsertAfter( headerText);//頁首內容

                ////新增頁首方法二：

                // 固定寫法
                if (WordApp.ActiveWindow.ActivePane.View.Type == WdViewType.wdNormalView ||
                    WordApp.ActiveWindow.ActivePane.View.Type == WdViewType.wdOutlineView)
                {
                    WordApp.ActiveWindow.ActivePane.View.Type = WdViewType.wdPrintView;
                }

                // 設定頁首
                WordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekCurrentPageHeader;
                WordApp.Selection.HeaderFooter.LinkToPrevious = false;
                WordApp.Selection.HeaderFooter.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                WordApp.Selection.HeaderFooter.Range.Text = headerText;

                // 設定頁尾
                WordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekCurrentPageFooter;
                WordApp.Selection.HeaderFooter.LinkToPrevious = false;
                WordApp.Selection.HeaderFooter.Range.ParagraphFormat.Alignment = WdParagraphAlignment.wdAlignParagraphCenter;
                WordApp.ActiveWindow.ActivePane.Selection.InsertAfter(footerText);

                //跳出頁首頁尾設定
                WordApp.ActiveWindow.View.SeekView = WdSeekView.wdSeekMainDocument;

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
        ///  游標移動位置
        /// </summary>
        /// <param name="paragraphsCount">第幾個段落</param>
        /// <param name="wdLine">第幾行</param>
        public void Move(int paragraphsCount, int wdLine)
        {
            object count = paragraphsCount;
            object WdLine = wdLine;
            WordApp.Selection.MoveDown(ref WdLine, ref count, ref oMissing);//移動焦點
        }


        /// <summary>
        ///  設定當前段落 樣式
        /// </summary>
        /// <param name="Alignment">對齊方式(預設 居左)(輸入 Microsoft.Office.Interop.Word.WdParagraphAlignment 裡的屬性， 例如：WdParagraphAlignment.wdAlignParagraphLeft )</param>
        /// <param name="LineSpacing">行間距(預設 15)(輸入： 15f )</param>
        /// <param name="settingText">是否設定文字樣式(預設為 false) => 只設定 間距+對齊方式</param>
        /// <param name="Font_Bold">文字粗體長度</param>
        /// <param name="Font_Color">文字顏色(預設 黑色)(輸入 WdColor 裡的屬性， 例如 ： WdColor.wdColorBlack)</param>
        /// <param name="Font_Italic">文字斜體度</param>
        /// <param name="Font_Name">字型(預設 新細明體)(輸入： 新細明體)</param>
        /// <param name="Font_Size">字型大小(預設 12)(輸入： 12f)</param>
        /// <returns></returns>
        public bool SettingCurrentParagraphFormat(WdParagraphAlignment Alignment = WdParagraphAlignment.wdAlignParagraphLeft, float LineSpacing = 15f, bool settingText = false, int Font_Bold = 0, WdColor Font_Color = WdColor.wdColorBlack, int Font_Italic = 0, float Font_Size = 12f, string Font_Name = "新細明體")
        {
            try
            {
                // 移動 游標
                this.MoveToCurrentSelect();

                // 對齊方式
                WordApp.Selection.ParagraphFormat.Alignment = Alignment;

                // 間距
                WordApp.Selection.ParagraphFormat.LineSpacing = 15f;

                // 設定段落
                Microsoft.Office.Interop.Word.Paragraph para = WordApp.Selection.Paragraphs.Last;

                if (settingText)
                {
                    // 段落內的文字樣式
                    para.Range.Font.Bold = Font_Bold;
                    para.Range.Font.Color = Font_Color;
                    para.Range.Font.Italic = Font_Italic;
                    para.Range.Font.Size = Font_Size;
                    para.Range.Font.Name = Font_Name;

                    // 其他操作
                    // para.Range.Copy();  //copy
                    // para.Range.Delete();  //delete
                    // para.Range.Paste();    // 貼上
                    // para.Range.PasteExcelTable();  //貼上 excel 表格
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
        ///  斷行
        /// </summary>
        public void NewLine()
        {
            // 用這個的會自動斷行
            this.WriteText(Text: "\n");

            // 或者用下面的寫法：
            // WordApp.Selection.TypeParagraph();
        }

        /// <summary>
        ///  加入新的一頁(換頁)
        /// </summary>
        public void NewPage()
        {
            WordApp.Selection.InsertNewPage();
        }

        /// <summary>
        ///  複製至剪貼簿
        /// </summary>
        public void Copy(int timespan = 1000)
        {
            // 複製
            WordApp.ActiveDocument.Select();
            WordApp.Selection.Copy();

            // 要等幾秒，不然很容易 後面的 複製時，沒複製到
            System.Threading.Thread.Sleep(timespan);
        }

        /// <summary>
        ///  貼上至文件
        /// </summary>
        public void Paste()
        {
            // 先移至最後面
            MoveToCurrentSelect();
            WordApp.Selection.Paste();
        }

        /// <summary>
        ///  加入 超連結
        /// </summary>
        /// <param name="link">連結(http://xxx)</param>
        /// <param name="text">顯示文字</param>
        /// <param name="tip">滑到上面的提示文字</param>
        /// <param name="Font_Bold">文字粗體長度</param>
        /// <param name="Font_Color">文字顏色(預設 藍色)(輸入 WdColor 裡的屬性， 例如 ： WdColor.wdColorBlack)</param>
        /// <param name="Font_Italic">文字斜體度</param>
        /// <param name="Font_Size">字型大小(預設 12)(輸入： 12f)</param>
        /// <param name="Font_Name">字型(預設 新細明體)(輸入： 新細明體)</param>
        /// <param name="haveNewLine">需要先斷行再放超連結嗎？</param>
        /// <param name="endHaveNewLine">結束後是否要斷行</param>
        /// <returns></returns>
        public bool WriteHyperlink(string link, string text, string tip = "", int Font_Bold = 0, WdColor Font_Color = WdColor.wdColorBlue, int Font_Italic = 0, float Font_Size = 12f, string Font_Name = "新細明體", bool haveNewLine = false, bool endHaveNewLine = false)
        {
            try
            {
                // 移到游標
                this.MoveToCurrentSelect();

                if (haveNewLine)
                {
                    WordApp.Selection.TypeParagraph();//插入段落
                }

                //插入Hyperlink
                object linkAddr = link;
                object displayText = text;
                object screenTip = tip;
                object Anchor = this._word.Application.Selection.Range;  // 定在目前游標上的位置 (但是因為上面的 WriteText 不會動到游標 => 不會依照順序)
                Hyperlink hyperLink = this._word.Application.ActiveDocument.Hyperlinks.Add(Anchor, ref linkAddr, ref oMissing, ref screenTip, ref displayText, ref oMissing);

                // 設定樣式
                hyperLink.Range.Font.Size = Font_Size;
                hyperLink.Range.Font.Bold = Font_Bold;
                hyperLink.Range.Font.Italic = Font_Italic;
                hyperLink.Range.Font.Name = Font_Name;
                hyperLink.Range.Font.Color = Font_Color;

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
        ///  插入圖片
        /// </summary>
        /// <param name="picPath">圖片路徑</param>
        /// <param name="title">滑上的標題</param>
        /// <param name="height">高(不帶入，就依原本的大小)</param>
        /// <param name="width">寬(不帶入，就依原本的大小)</param>
        /// <param name="haveNewLine">需要先斷行再放超連結嗎？</param>
        /// <param name="endHaveNewLine">結束後是否要斷行</param>
        /// <returns></returns>
        public bool AddPicture(string picPath, string title = "", int height = -1, int width = -1, bool haveNewLine = false, bool endHaveNewLine = false)
        {
            try
            {
                // 移到游標到當前位置
                this.MoveToCurrentSelect();

                if (haveNewLine)
                {
                    WordApp.Selection.TypeParagraph();//插入段落
                }

                // 插入圖片
                object LinkToFile = false;
                object SaveWithDocument = true;
                object Anchor = this._word.Application.Selection.Range;
                InlineShape pic = this._word.Application.ActiveDocument.InlineShapes.AddPicture(picPath, ref LinkToFile, ref SaveWithDocument, ref Anchor);

                pic.Title = title;

                if (height >= 0) { pic.Height = height; }
                if (width >= 0) { pic.Width = width; }

                this.MoveToCurrentSelect();

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
        ///  新建一個表格
        /// </summary>
        /// <param name="row">row</param>
        /// <param name="col">col</param>
        /// <param name="InsideLineStyle">內部框線(輸入 WdLineStyle 屬性裡的值)(例如： WdLineStyle.wdLineStyleSingle)(預設： 單線) </param>
        /// <param name="OutsideLineStyle">外部框線(輸入 WdLineStyle 屬性裡的值)(例如： WdLineStyle.wdLineStyleSingle)(預設： 單線) </param>
        /// <param name="haveNewLine">需要先斷行再放超連結嗎？</param>
        /// <param name="endHaveNewLine">結束後是否要斷行</param>
        /// <returns></returns>
        public bool CreateNewTable(int row, int col, WdLineStyle InsideLineStyle = WdLineStyle.wdLineStyleSingle, WdLineStyle OutsideLineStyle = WdLineStyle.wdLineStyleSingle, bool haveNewLine = false, bool endHaveNewLine = false)
        {
            try
            {
                // 移到游標到當前位置
                this.MoveToCurrentSelect();

                if (haveNewLine)
                {
                    WordApp.Selection.TypeParagraph();//插入段落
                }

                // 當前游標的位置
                Range range = this._word.Application.Selection.Range;

                // 建立一個 Table
                Table table = this._word.Application.ActiveDocument.Tables.Add(range, row, col, ref oMissing, ref oMissing);

                table.Borders.InsideLineStyle = InsideLineStyle;    //設定內部框線
                table.Borders.OutsideLineStyle = OutsideLineStyle;  //設定外部框線


                this.MoveToCurrentSelect();

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
        ///  設定 表格欄位
        /// </summary>
        /// <param name="tableIndex">此Word 中第幾個 Table (從 1 開始)</param>
        /// <param name="rowIndex">第幾 row (從 1 開始) </param>
        /// <param name="colIndex">第幾 col (從 1 開始) </param>
        /// <param name="height">長 (輸入： 30f)</param>
        /// <param name="width">寬 (輸入 ： 20f)</param>
        /// <returns></returns>
        public bool SettingTableColumns(int tableIndex, int rowIndex, int colIndex, float height, float width)
        {
            try
            {
                // 得到 Table
                Table table = this._word.Application.ActiveDocument.Tables[tableIndex];
                if (table != null)
                {
                    rowIndex = rowIndex > table.Rows.Count ? (table.Rows.Count) : (rowIndex <= 0) ? 1 : rowIndex;
                    colIndex = colIndex > table.Columns.Count ? (table.Columns.Count) : (colIndex <= 0) ? 1 : colIndex;

                    // Row 只能改 高 ； Col 只能改 寬
                    table.Rows[rowIndex].Height = height;
                    table.Columns[colIndex].Width = width;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        /// <summary>
        ///  把全部 表格欄位 設定相同
        /// </summary>
        /// <param name="tableIndex">此Word 中第幾個 Table (從 1 開始)</param>
        /// <param name="height">長 (輸入： 30f)</param>
        /// <param name="width">寬 (輸入 ： 20f)</param>
        /// <returns></returns>
        public bool SettingTableAllColumns(int tableIndex, float height, float width)
        {
            try
            {
                Table table = this._word.Application.ActiveDocument.Tables[tableIndex];
                if (table != null)
                {
                    for (var i = 1; i <= table.Rows.Count; i++)
                    {
                        for (var j = 1; j <= table.Columns.Count; j++)
                        {
                            this.SettingTableColumns(tableIndex, i, j, height, width);
                        }
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        /// <summary>
        ///  合併儲存格
        /// </summary>
        /// <param name="tableIndex">此Word 中第幾個 Table (從 1 開始)</param>
        /// <param name="row1">開始 row</param>
        /// <param name="row2">開始 col</param>
        /// <param name="col1">結束 row</param>
        /// <param name="col2">結束 col</param>
        /// <returns></returns>
        public bool MergeTableColumns(int tableIndex, int row1, int col1, int row2, int col2)
        {
            try
            {
                // 得到 Table
                Table table = this._word.Application.ActiveDocument.Tables[tableIndex];
                if (table != null)
                {
                    row1 = row1 > table.Rows.Count ? (table.Rows.Count) : (row1 <= 0) ? 1 : row1;
                    row2 = row2 > table.Rows.Count ? (table.Rows.Count) : (row2 <= 0) ? 1 : row2;
                    col1 = col1 > table.Columns.Count ? (table.Columns.Count) : (col1 <= 0) ? 1 : col1;
                    col2 = col2 > table.Columns.Count ? (table.Columns.Count) : (col2 <= 0) ? 1 : col2;

                    if (row1 > row2)
                    {
                        int tmp = row1;
                        row1 = row2;
                        row2 = tmp;
                    }

                    if (col1 > col2)
                    {
                        int tmp = col1;
                        col1 = col2;
                        col2 = tmp;
                    }

                    // 合併
                    table.Cell(row1, col1).Merge(table.Cell(row2, col2));

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        /// <summary>
        ///  填入表格內容
        /// </summary>
        /// <param name="tableIndex">此Word 中第幾個 Table (從 1 開始)</param>
        /// <param name="row">row</param>
        /// <param name="col">col</param>
        /// <param name="Alignment">對齊方式(預設 居左)(輸入 Microsoft.Office.Interop.Word.WdParagraphAlignment 裡的屬性， 例如：WdParagraphAlignment.wdAlignParagraphLeft )</param>
        /// <param name="Text">呈現的文字</param>
        /// <param name="Font_Bold">文字粗體長度</param>
        /// <param name="Font_Color">文字顏色(預設 黑色)(輸入 WdColor 裡的屬性， 例如 ： WdColor.wdColorBlack)</param>
        /// <param name="Font_Italic">文字斜體度</param>
        /// <param name="Font_Size">字型大小(預設 12)(輸入： 12f)</param>
        /// <param name="Font_Name">字型(預設 新細明體)(輸入： 新細明體)</param>
        /// <returns></returns>
        public bool WriteTableColumns(int tableIndex, int row, int col, WdParagraphAlignment Alignment = WdParagraphAlignment.wdAlignParagraphLeft, string Text = "", int Font_Bold = 0, WdColor Font_Color = WdColor.wdColorBlack, int Font_Italic = 0, float Font_Size = 12f, string Font_Name = "新細明體")
        {
            try
            {
                // 得到 Table
                Table table = this._word.Application.ActiveDocument.Tables[tableIndex];
                if (table != null)
                {
                    row = row > table.Rows.Count ? (table.Rows.Count) : (row <= 0) ? 1 : row;
                    col = col > table.Columns.Count ? (table.Columns.Count) : (col <= 0) ? 1 : col;

                    table.Cell(row, col).Range.Text = Text;  //設定文字

                    // 對齊方式
                    table.Cell(row, col).Range.ParagraphFormat.Alignment = Alignment;

                    table.Cell(row, col).Range.Font.Size = Font_Size;//設定字體大小
                    table.Cell(row, col).Range.Font.Name = Font_Name;//設定字型
                    table.Cell(row, col).Range.Font.Bold = Font_Bold;
                    table.Cell(row, col).Range.Font.Italic = Font_Italic;
                    table.Cell(row, col).Range.Font.Color = Font_Color;

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e.StackTrace);
                return false;
            }
        }

        /// <summary>
        /// 設定 Table 的全部 Colnums 全是  單線黑框
        /// </summary>
        /// <param name="tableIndex"></param>
        public void SettingTableAllColnumsBorder(int tableIndex)
        {
            // 得到 Table
            Table table = this._word.Application.ActiveDocument.Tables[tableIndex];
            if (table != null)
            {
                // 之後可自己換掉，這邊只是寫個寫法而已
                table.Borders.OutsideLineStyle = WdLineStyle.wdLineStyleSingle;
                table.Borders.OutsideLineWidth = WdLineWidth.wdLineWidth025pt;
                table.Borders.OutsideColor = WdColor.wdColorBlack;
                table.Borders.OutsideColorIndex = WdColorIndex.wdBlack;
                table.Borders.InsideLineStyle = WdLineStyle.wdLineStyleSingle;
                table.Borders.InsideLineWidth = WdLineWidth.wdLineWidth025pt;
                table.Borders.InsideColor = WdColor.wdColorBlack;
                table.Borders.InsideColorIndex = WdColorIndex.wdBlack;
            }
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
        /// 更新目錄
        /// </summary>
        /// <param name="goalPath"></param>
        public void UpdateDir(string goalPath = "")
        {
            int count = _word.TablesOfContents.Count;
            for (int i = 0; i < count; i++)
            {
                // 更新全部
                _word.TablesOfContents[i + 1].Update();
                // 更新頁數
                //_word.TablesOfContents[i + 1].UpdatePageNumbers();
            }

            // 如果是空的 => 自己存檔
            if (string.IsNullOrWhiteSpace(goalPath))
            {
                Save();
            }
            else
            {
                if (goalPath.Split('.').LastOrDefault().ToUpper() == "DOC")
                {
                    // 存 doc
                    _word.SaveAs2(goalPath, WdSaveFormat.wdFormatDocument97);
                }
                else
                {
                    // 存 docx
                    _word.SaveAs2(goalPath, WdSaveFormat.wdFormatDocumentDefault);
                }
            }
        }

        /// <summary>
        ///  轉成 Html
        /// </summary>
        /// <returns></returns>
        public bool ConvertToHtml(string goalPath)
        {
            // 如果存在了 就先刪掉
            if (File.Exists(goalPath))
            {
                File.Delete(goalPath);
            }

            try
            {
                _word.SaveAs2(goalPath, WdSaveFormat.wdFormatHTML);
                Console.WriteLine("轉成 HTML 成功");
                _word.Close();
                WordApp.Quit();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"轉成 HTML 失敗， 原因 = {ex.Message}");
                return false;
            }
        }

        /// <summary>
        ///  轉成 Html
        /// </summary>
        /// <returns></returns>
        public static bool ConvertToHtml(string sourcePath, string goalPath)
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

        /// <summary>
        ///  轉成 Docx
        /// </summary>
        /// <returns></returns>
        public static bool ConvertToDocx(string sourcePath, string goalPath)
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
        public static bool ConvertToDoc(string sourcePath, string goalPath)
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
        public static bool ConvertToPdf(string sourcePath, string goalPath)
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

        /// <summary>
        ///  Word 轉成 Image (預設給 png) => 之後自己實作可自行調整
        /// </summary>
        /// <param name="sourceFilePath">來源路徑(整完路徑)</param>
        /// <param name="goalDir">目標資料夾路徑(最後面不要加上 \\)</param>
        /// <param name="goalFileName">檔名(可輸入 {{page}} 代表頁數； 可輸入 {{index}} 代表序號) (例： 輸入 File_{{page}}_{{index}} => 則第1頁出來的會是 File_1_0)</param>
        /// <returns></returns>
        public static bool WordConvertToImage(string sourceFilePath, string goalDir, string goalFileName)
        {
            try
            {
                var msWordApp = new Microsoft.Office.Interop.Word.Application();
                msWordApp.Visible = false;

                Microsoft.Office.Interop.Word.Document doc = msWordApp.Documents.Open(sourceFilePath);

                foreach (Microsoft.Office.Interop.Word.Window window in doc.Windows)
                {
                    foreach (Microsoft.Office.Interop.Word.Pane pane in window.Panes)
                    {
                        for (var i = 1; i <= pane.Pages.Count; i++)
                        {
                            var page = pane.Pages[i];
                            object bits = page.EnhMetaFileBits;//Returns a Object that represents a picture 【Read-only】
                                                               //以下Try Catch區段為將圖片的背景填滿為白色，不然輸出的圖片背景會是透明的
                            try
                            {
                                using (var ms = new MemoryStream((byte[])bits))
                                {
                                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms);

                                    using (var backGroundWritePNG = new Bitmap(image.Width, image.Height))
                                    {
                                        //設定圖片的解析度
                                        backGroundWritePNG.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                                        using (Graphics graphis = Graphics.FromImage(backGroundWritePNG))
                                        {
                                            graphis.Clear(Color.White);
                                            graphis.DrawImageUnscaled(image, 0, 0);
                                        }

                                        string name = goalFileName.Replace("{{page}}", i.ToString()).Replace("{{index}}", (i - 1).ToString()) + ".png";
                                        string outPutFilePath = goalDir + "\\" + name;

                                        if (File.Exists(outPutFilePath))
                                        {
                                            File.Delete(outPutFilePath);
                                        }

                                        backGroundWritePNG.Save(outPutFilePath, ImageFormat.Png);//輸出圖片
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }
                    }
                }

                //關閉Word，釋放資源
                doc.Close(Type.Missing, Type.Missing, Type.Missing);
                msWordApp.Quit(Type.Missing, Type.Missing, Type.Missing);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(msWordApp);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 將 Word 第一頁 轉成 Image
        /// </summary>
        /// <param name="sourceFilePath"></param>
        /// <param name="goalFilePath"></param>
        /// <returns></returns>
        public static bool WordFirstPageToImage(string sourceFilePath, string outPutFilePath)
        {
            try
            {
                var msWordApp = new Microsoft.Office.Interop.Word.Application();
                msWordApp.Visible = false;

                Microsoft.Office.Interop.Word.Document doc = msWordApp.Documents.Open(sourceFilePath);

                foreach (Microsoft.Office.Interop.Word.Window window in doc.Windows)
                {
                    foreach (Microsoft.Office.Interop.Word.Pane pane in window.Panes)
                    {
                        var page = pane.Pages[1];
                        object bits = page.EnhMetaFileBits;//Returns a Object that represents a picture 【Read-only】
                                                           //以下Try Catch區段為將圖片的背景填滿為白色，不然輸出的圖片背景會是透明的
                        try
                        {
                            using (var ms = new MemoryStream((byte[])bits))
                            {
                                System.Drawing.Image image = System.Drawing.Image.FromStream(ms);

                                using (var backGroundWritePNG = new Bitmap(image.Width, image.Height))
                                {
                                    //設定圖片的解析度
                                    backGroundWritePNG.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                                    using (Graphics graphis = Graphics.FromImage(backGroundWritePNG))
                                    {
                                        graphis.Clear(Color.White);
                                        graphis.DrawImageUnscaled(image, 0, 0);
                                    }

                                    if (File.Exists(outPutFilePath))
                                    {
                                        File.Delete(outPutFilePath);
                                    }

                                    backGroundWritePNG.Save(outPutFilePath, ImageFormat.Png);//輸出圖片
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.ToString());
                        }
                    }
                }

                //關閉Word，釋放資源
                doc.Close(Type.Missing, Type.Missing, Type.Missing);
                msWordApp.Quit(Type.Missing, Type.Missing, Type.Missing);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(msWordApp);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
