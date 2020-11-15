using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    參考網圵： https://dotblogs.com.tw/kiwifruit0612/2009/08/17/10101
    1. 去 NuGet 裝上 iTextSharp
 */

namespace PDF加入浮水印
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = Directory.GetCurrentDirectory() + "\\";

            string inputPath = rootPath + "input.pdf";
            string outputPath = rootPath + "output.pdf";
            string watermarkPath = rootPath + "watermark.png";
            string outputPath2 = rootPath + "output2.pdf";

            // 這邊的 y (高度) 是從底下 為 0 開始算的 (特別) ； x 一樣是 最左 為 0
            PdfHelper.AddWaterMarkByPic(inputPath, outputPath, watermarkPath, 355, 395);
            PdfHelper.AddWaterMarkByText(inputPath, outputPath2, "這是浮水印的文字！！！", 200, 10);
            Console.WriteLine("執行完成");
            Console.Read();
        }
    }

    public static class PdfHelper
    {
        /// <summary>
        ///  加入 浮水印 (圖片)
        /// </summary>
        /// <param name="inputPath">輸入路徑(完整路徑)</param>
        /// <param name="outputPath">輸出路徑(完整路徑)</param>
        /// <param name="watermarkPath">浮水印圖片(完整路徑)</param>
        /// <param name="x">x 位置</param>
        /// <param name="y">y 位置</param>
        /// <returns></returns>
        public static bool AddWaterMarkByPic(string inputPath, string outputPath, string watermarkPath, int x, int y)
        {
            try
            {
                // 讀取檔案
                PdfReader pdfReader = new PdfReader(inputPath);
                int numberOfPages = pdfReader.NumberOfPages;

                if (File.Exists(outputPath))
                {
                    File.Delete(outputPath);
                }

                // 開新檔案
                FileStream outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, outputStream);
                PdfContentByte waterMarkContent;

                // 讀取浮水印圖片檔
                string watermarkimagepath = watermarkPath;
                iTextSharp.text.Image image = iTextSharp.text.Image.GetInstance(watermarkimagepath);

                // 設定位置
                image.SetAbsolutePosition(x, y);  // 設定絕對位置
                // 設定 固定 大小 image.ScaleToFit(fitWidth, fitHeight)
                // 設定 相對 位置 image.ScalePercent(x, y);
                // image.ScalePercent(20f);  //縮放比例
                // image.RotationDegrees = 10; //旋轉角度

                for (int i = 1; i <= numberOfPages; i++)
                {
                    waterMarkContent = pdfStamper.GetOverContent(i);

                    // 加入圖片
                    waterMarkContent.AddImage(image);
                }

                pdfStamper.Close();
                pdfReader.Close();
                outputStream.Close();

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        /// 加入浮水印 (文字) => 文字大小 那些就先用預設 (之後再改)
        /// </summary>
        /// <param name="inputPath">輸入路徑(完整路徑)</param>
        /// <param name="outputPath">輸出路徑(完整路徑)</param>
        /// <param name="text">浮水印文字</param>
        /// <param name="x">x 位置</param>
        /// <param name="y">y 位置</param>
        /// <returns></returns>
        public static bool AddWaterMarkByText(string inputPath, string outputPath, string text, int x, int y)
        {
            try
            {
                // 讀取檔案
                PdfReader pdfReader = new PdfReader(inputPath);
                int numberOfPages = pdfReader.NumberOfPages;

                if (File.Exists(outputPath))
                {
                    File.Delete(outputPath);
                }

                // 開新檔案
                FileStream outputStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write);
                PdfStamper pdfStamper = new PdfStamper(pdfReader, outputStream);

                // 設定文字
                // 後面的 GetFont(字型, 字體大小, 字顏色) => 之後可自己設定
                // 字體樣式 (用這個就行， 其他的不知道為啥都用不起來)
                string windir = Environment.GetEnvironmentVariable("windir");
                BaseFont baseFont = BaseFont.CreateFont(windir + "\\Fonts\\mingliu.ttc,0", BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                
                Chunk ctitle = new Chunk(text, new Font(baseFont, 20, Font.NORMAL, new BaseColor(0, 0, 0)));
                // 也可以這樣寫，但你的 字體名稱要有 (不然他跑不出來，不然就用上面的寫法)
                // Chunk ctitle = new Chunk(text,FontFactory.GetFont("Futura", 12f, new BaseColor(0, 0, 0)));  
                Phrase ptitle = new Phrase(ctitle);
                PdfGState pdfgstate = new PdfGState
                {
                    FillOpacity = 0.5f,    //背景
                    StrokeOpacity = 0.5f   //文字
                };

                for (int i = 1; i <= numberOfPages; i++)
                {
                    PdfContentByte waterMarkContent = pdfStamper.GetOverContent(i);

                    // 加入文字的 浮水印
                    // 參考網圵： https://ithelp.ithome.com.tw/articles/10190232  (產生出來的 PDF 錯誤)
                    //           https://www.tpisoftware.com/tpu/articleDetails/1101 (產出來的 PDF 沒有加入到文字)

                    ColumnText.ShowTextAligned(waterMarkContent, Element.ALIGN_LEFT, ptitle, x, y, 0);

                    //設定浮水印透明度
                    waterMarkContent.SetGState(pdfgstate);
                }

                pdfStamper.FormFlattening = true;  // 多加這個
                pdfStamper.Close();
                pdfReader.Close();
                outputStream.Close();

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
