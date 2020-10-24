using iTextSharp.text;
using iTextSharp.text.pdf;
using Spire.Pdf;
using Spire.Pdf.Graphics;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

/*
    參考網圵： https://www.itread01.com/article/1516265073.html
    1. 請去 NuGet 裝上 FreeSpire.PDF (裝免費版的)  => 不管是合併 還是 其他 都只能處理 10 頁
                          Spire.PDF => 每頁都會有 多餘的訊息文字
 */

/*
    參考網圵： https://dotblogs.com.tw/kiwifruit0612/2009/05/12/8386
    1. 請去 NuGet 裝上 iTextSharp
 */

// 總結： 使用 iTextSharp 全部都可達到

namespace PDF合併與分割_使用Spire.PDF_
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 套件一(Spire.PDF) => 展示用的是免費版的
            // 套件一
            string rootPath = System.IO.Directory.GetCurrentDirectory() + "\\";

            // 合併 PDF (每個檔案 10頁內 => 因為免費版的)
            // 用套件二的 沒有限制(如下面的)
            List<string> fileNameList = new List<string> { "Merge1.pdf", "Merge2.pdf", "Merge3.pdf" };
            List<string> filePathList = fileNameList.Select(x => rootPath + x).ToList();
            string outputFile = "輸出.pdf";
            string outputFilePath = rootPath + outputFile;
            PdfDocumentBase doc = Spire.Pdf.PdfDocument.MergeFiles(filePathList.ToArray());
            doc.Save(outputFilePath, FileFormat.PDF);

            // 分割 PDF (每一頁一個)
            Spire.Pdf.PdfDocument doc2 = new Spire.Pdf.PdfDocument(rootPath + "Split2.pdf");
            String pattern = "拆分-{0}.pdf";
            doc2.Split(pattern);

            // 分割 PDF (因為免費版的只能 最多處理10頁的，所以上面的 也是10頁內比較好)
            // 這邊展示 前2頁 和 後3頁 分割
            Spire.Pdf.PdfDocument pdf = new Spire.Pdf.PdfDocument();
            pdf.LoadFromFile(rootPath + "Split3.pdf");
            Console.WriteLine($"此 PDF 共有 {pdf.Pages.Count} 頁");

            Spire.Pdf.PdfDocument pdf1 = new Spire.Pdf.PdfDocument();
            PdfPageBase page;
            // 前2頁
            for (int i = 0; i < 2; i++)
            {
                page = pdf1.Pages.Add(pdf.Pages[i].Size, new PdfMargins(0));
                pdf.Pages[i].CreateTemplate().Draw(page, new PointF(0, 0));
            }
            pdf1.SaveToFile("分割1-2頁.pdf");

            Spire.Pdf.PdfDocument pdf2 = new Spire.Pdf.PdfDocument();
            for (int i = 2; i < 5; i++)
            {
                page = pdf2.Pages.Add(pdf.Pages[i].Size, new PdfMargins(0));
                pdf.Pages[i].CreateTemplate().Draw(page, new PointF(0, 0));
            }
            pdf2.SaveToFile("分割3-5頁.pdf");

            // 讀取看看 讀超過 10頁的 PDF 是否免費版會錯
            // 結論： 免費版的會爆錯
            //PdfDocument pdf3 = new PdfDocument();
            //pdf3.LoadFromFile(rootPath + "source.pdf");
            //Console.WriteLine($"此 PDF 共有 {pdf.Pages.Count} 頁");

            // 總結：如果要用 正式版的 (會多多餘文字的) 就只能 把他再轉成 Word ，然後用 Word 的取代文字換掉，再轉回PDF
            // 超級費時+費工 (但因為有太多限制了)

            // 不然免費版的限制太大了(怎麼可能只有少於10頁的)
            #endregion

            #region 套件二(iTextSharp)

            // 合併PDF
            rootPath = System.IO.Directory.GetCurrentDirectory() + "\\";

            fileNameList = new List<string> { "Merge1.pdf", "Merge2.pdf", "Merge3.pdf" };
            filePathList = fileNameList.Select(x => rootPath + x).ToList();
            outputFile = "輸出(套件二).pdf";
            outputFilePath = rootPath + outputFile;
            iTextSharpPDFHelper.MergePDFFiles(filePathList, outputFilePath);

            // 分割 PDF (每一頁一個)
            iTextSharpPDFHelper.SplitPDFFile(rootPath + "Split2.pdf", "拆分(套件二)-{{index}}.pdf");

            // 分割 PDF
            // 這邊展示 前2頁 和 後3頁 分割
            iTextSharpPDFHelper.SplitPDFFileByRange(rootPath + "Split3.pdf", rootPath + "分割(套件二)1-2頁.pdf", 1, 2);
            iTextSharpPDFHelper.SplitPDFFileByRange(rootPath + "Split3.pdf", rootPath + "分割(套件二)3-5頁.pdf", 3, 5);

            #endregion

            // 總結： 使用套件二
        }
    }

    /// <summary>
    ///  套件 (iTextSharp) 我寫的擴充
    /// </summary>
    public static class iTextSharpPDFHelper
    {
        /// <summary> 合併PDF檔(集合) => 要包含完整路徑 </summary> 
        /// <param name="fileList">欲合併PDF檔之集合(一筆以上)</param>
        /// <param name="outMergeFile">合併後的檔名(完整路徑)</param> 
        public static void MergePDFFiles(List<string> fileList, string outMergeFile)
        {
            if (File.Exists(outMergeFile))
            {
                File.Delete(outMergeFile);
            }

            iTextSharp.text.pdf.PdfReader reader;
            iTextSharp.text.Document document = new Document();
            iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(outMergeFile, FileMode.Create));
            document.Open();
            iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
            iTextSharp.text.pdf.PdfImportedPage newPage;

            foreach (var filePath in fileList)
            {
                reader = new iTextSharp.text.pdf.PdfReader(filePath);
                int iPageNum = reader.NumberOfPages;
                for (int j = 1; j <= iPageNum; j++)
                {
                    document.NewPage();
                    newPage = writer.GetImportedPage(reader, j);
                    cb.AddTemplate(newPage, 0, 0);
                }
            }
            document.Close();
        }

        /// <summary>
        ///  切割 PDF (自己擴充的)
        /// </summary>
        /// <param name="filePath">來源PDF(完整路徑)</param>
        /// <param name="goalFilePath">目標路徑(完整路徑)，可以輸入 {{page}} 代表這是第幾頁到第幾頁； {{index}} 是切割的序號(0 開始)。 </param>
        /// <param name="perPageSplit">每幾頁切一個檔案</param>
        public static void SplitPDFFile(string filePath, string goalFilePathParm, int perPageSplit = 1)
        {
            if (File.Exists(filePath) && perPageSplit > 0)
            {
                int totalCount = GetPdfTotalPage(filePath);
                perPageSplit = perPageSplit > totalCount ? totalCount : perPageSplit;  // 如果超果上限就改成用最大頁數

                int currentPage = 1;
                int index = 0;

                iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(filePath);

                while (currentPage <= totalCount)
                {
                    // 檔名
                    string fileName = goalFilePathParm.Replace("{{page}}", $"{currentPage}_{currentPage + perPageSplit - 1}").Replace("{{index}}", index.ToString());

                    if (File.Exists(fileName))
                    {
                        File.Delete(fileName);
                    }

                    // 開啟新檔
                    iTextSharp.text.Document document = new Document();
                    iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(fileName, FileMode.Create));
                    document.Open();
                    iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                    iTextSharp.text.pdf.PdfImportedPage newPage;

                    for (var i = 1; i <= perPageSplit; i++)
                    {
                        if (currentPage <= totalCount)
                        {
                            // 新增頁
                            document.NewPage();
                            newPage = writer.GetImportedPage(reader, currentPage);
                            cb.AddTemplate(newPage, 0, 0);
                        }

                        currentPage++;
                    }

                    index++;

                    // 關閉
                    document.Close();
                }
            }
        }

        /// <summary>
        ///  切割 PDF 某一Range 成 PDF
        /// </summary>
        /// <param name="filePath">來源PDF(完整路徑)</param>
        /// <param name="goalFilePath">目標檔案(完整路徑)</param>
        /// <param name="startIndex">開始頁數(從1開始，超過檔案頁數視為1)</param>
        /// <param name="endIndex">結束頁數(超過檔案頁數，自動用檔案頁數)</param>
        public static void SplitPDFFileByRange(string filePath, string goalFilePath, int startIndex = 1, int endIndex = 1)
        {
            if (File.Exists(filePath))
            {
                // 總頁數
                int totalPage = GetPdfTotalPage(filePath);

                startIndex = startIndex > totalPage ? 1 : startIndex;
                startIndex = startIndex <= 0 ? 1 : startIndex;
                endIndex = endIndex > totalPage ? totalPage : endIndex;
                endIndex = endIndex <= 0 ? totalPage : endIndex;

                // 如果 startIndex > endIndex 交換
                if (startIndex > endIndex)
                {
                    int t = startIndex;
                    startIndex = endIndex;
                    endIndex = t;
                }

                // 開始準備要寫檔
                if (File.Exists(goalFilePath))
                {
                    File.Delete(goalFilePath);
                }

                // 寫檔
                iTextSharp.text.pdf.PdfReader reader;
                iTextSharp.text.Document document = new Document();
                iTextSharp.text.pdf.PdfWriter writer = iTextSharp.text.pdf.PdfWriter.GetInstance(document, new FileStream(goalFilePath, FileMode.Create));
                document.Open();
                iTextSharp.text.pdf.PdfContentByte cb = writer.DirectContent;
                iTextSharp.text.pdf.PdfImportedPage newPage;

                reader = new iTextSharp.text.pdf.PdfReader(filePath);

                for (int j = startIndex; j <= endIndex; j++)
                {
                    document.NewPage();
                    // 不要減 1 (因為 他是從 1 開始的)
                    newPage = writer.GetImportedPage(reader, j);
                    cb.AddTemplate(newPage, 0, 0);
                }

                document.Close();
            }
        }

        /// <summary>
        ///  得到此 PDF 有幾頁
        /// </summary>
        /// <param name="filePath">來源PDF(完整路徑)</param>
        /// <returns></returns>
        public static int GetPdfTotalPage(string filePath)
        {
            int result = 0;

            if (File.Exists(filePath))
            {
                iTextSharp.text.pdf.PdfReader reader;
                reader = new iTextSharp.text.pdf.PdfReader(filePath);
                result = reader.NumberOfPages;
            }

            return result;
        }
    }
}
