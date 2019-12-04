using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

// 引用
using System.Drawing;
using System.Drawing.Imaging;
using Spire.Pdf;

/*
    參考網圵： https://www.itread01.com/article/1517971950.html
    1. 去 Nuget 裝上 Spire.Pdf
    2. 去 加入參考 System.Drawing
*/

namespace PdfToImage
{
    class Program
    {
        static void Main(string[] args)
        {
            string path = System.IO.Directory.GetCurrentDirectory() + "\\";
            PdfToImageHelper.PdfToImage(path + "a.pdf", ImageFormat.Png, path + "normal\\" + "test.png");
            Console.WriteLine("第一個產生完成");
            PdfToImageHelper.PdfToImage(path + "a.pdf", 2, ImageFormat.Png, path + "normal\\" + "page.png");
            Console.WriteLine("第二個產生完成");

            PdfToImageHelper.PdfToTiff(path + "a.pdf", path + "tiff\\" + "test.tiff");
            Console.WriteLine("第三個產生完成");
            PdfToImageHelper.PdfToTiff(path + "a.pdf", path + "tiff\\" + "page.tiff", 2);
            Console.WriteLine("第四個產生完成");

            // 開啟圖片 (後面接的是完整路徑)
            //System.Diagnostics.Process.Start("convertToBmp.bmp");
            Console.Read();
        }
    }


    public class PdfToImageHelper
    {
        /// <summary>
        ///  將目標全部轉成 Image (檔名不設，因為可能會多頁)
        /// </summary>
        /// <param name="sourcePath">pdf路徑(要含副檔名)</param>
        /// <param name="format">ImageFormat格式 (例如： png => ImageFormat格式.Png) </param>
        /// <param name="goalPath">目標路徑(含副檔名)</param>
        public static bool PdfToImage(string sourcePath, ImageFormat format, string goalPath)
        {
            try
            {
                //初始化一個PdfDocument類例項,並載入PDF文件
                PdfDocument doc = new PdfDocument();
                doc.LoadFromFile(sourcePath);

                string[] p = goalPath.Split('.');
                string first = string.Empty;
                for (var i = 0; i < p.Length - 1; i++)
                {
                    first += p[i];
                }
                string second = p.Last();

                //遍歷PDF每一頁
                for (int i = 0; i < doc.Pages.Count; i++)
                {
                    //將PDF頁轉換成Bitmap圖形
                    Image bmp = doc.SaveAsImage(i);

                    //將Bitmap圖形儲存為Png格式的圖片
                    string fileName = $"{first}_{i}.{second}";
                    bmp.Save(fileName, format);
                }

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        ///  將目標某一面 轉成 圖片
        /// </summary>
        /// <param name="sourcePath">pdf路徑(要含副檔名)</param>
        /// <param name="page">頁數(從1開始)</param>
        /// <param name="format">ImageFormat格式 (例如： png => ImageFormat格式.Png) </param>
        /// <param name="goalPath">目標路徑(含副檔名)</param>
        public static bool PdfToImage(string sourcePath, int page, ImageFormat format, string goalPath)
        {
            try
            {
                //初始化一個PdfDocument類例項,並載入PDF文件
                PdfDocument doc = new PdfDocument();
                doc.LoadFromFile(sourcePath);

                page = page - 1;

                if (page >= 0 && page < doc.Pages.Count)
                {
                    //將PDF頁轉換成Bitmap圖形
                    Image bmp = doc.SaveAsImage(page);

                    //將Bitmap圖形儲存為Png格式的圖片
                    string fileName = $"{goalPath}";
                    bmp.Save(fileName, format);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        /// <summary>
        ///  將 目標 回傳成 Image[]
        /// </summary>
        /// <param name="sourcePath">pdf完整路徑(含副檔名)</param>
        /// <returns></returns>
        public static List<Image> SaveAsImage(string sourcePath)
        {
            List<Image> result = new List<Image>();

            try
            {
                PdfDocument document = new PdfDocument();
                document.LoadFromFile(sourcePath);

                Image[] images = new Image[document.Pages.Count];
                for (int i = 0; i < document.Pages.Count; i++)
                {
                    images[i] = document.SaveAsImage(i);
                }

                result = images.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }

        /// <summary>
        ///  將 目標 回傳成 Image[]
        /// </summary>
        /// <param name="document">PDF 物件</param>
        /// <returns></returns>
        public static List<Image> SaveAsImage(PdfDocument document)
        {
            List<Image> result = new List<Image>();

            try
            {
                Image[] images = new Image[document.Pages.Count];
                for (int i = 0; i < document.Pages.Count; i++)
                {
                    images[i] = document.SaveAsImage(i);
                }

                result = images.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return result;
        }


        /// <summary>
        ///  PDF 轉成 Tiff 檔 (不能用一般上面的轉)
        /// </summary>
        /// <param name="sourcePath">PDF完整路徑(含副檔名)</param>
        /// <param name="goalPath">Tiff完整路徑(含副檔名)</param>
        /// <returns></returns>
        public static bool PdfToTiff(string sourcePath, string goalPath)
        {
            try
            {
                //建立一個PdfDocument類物件，並載入PDF文件
                PdfDocument document = new PdfDocument();
                document.LoadFromFile(sourcePath);

                // 先把 PDF 轉成 Image[]
                List<Image> images = SaveAsImage(document);

                // 轉換成 Tiff
                JoinTiffImages(images, goalPath);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }


        /// <summary>
        ///  PDF 轉成 Tiff 檔 (不能用一般上面的轉)
        /// </summary>
        /// <param name="sourcePath">PDF完整路徑(含副檔名)</param>
        /// <param name="goalPath">Tiff完整路徑(含副檔名)</param>
        /// <param name="page">頁數(從1開始)</param>
        /// <returns></returns>
        public static bool PdfToTiff(string sourcePath, string goalPath, int page)
        {
            try
            {
                //建立一個PdfDocument類物件，並載入PDF文件
                PdfDocument document = new PdfDocument();
                document.LoadFromFile(sourcePath);

                // 先把 PDF 轉成 Image[]
                List<Image> images = SaveAsImage(document);

                // 要丟進去的
                List<Image> image = new List<Image>();
                page = page - 1;
                if (page >= 0 && page < images.Count())
                {
                    image.Add(images[page]);

                    // 轉換成 Tiff
                    JoinTiffImages(image, goalPath);

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        ///  轉換成 Tiff (只有全轉，沒有挑頁數的)
        /// </summary>
        /// <param name="images">Image[]</param>
        /// <param name="outFile">輸入檔案(含副檔名)</param>
        public static void JoinTiffImages(List<Image> images, string outFile)
        {
            if (images.Count() > 0)
            {
                EncoderValue compressEncoder = EncoderValue.CompressionLZW;

                Encoder enc = Encoder.SaveFlag;
                EncoderParameters ep = new EncoderParameters(2);
                ep.Param[0] = new EncoderParameter(enc, (long)EncoderValue.MultiFrame);
                ep.Param[1] = new EncoderParameter(Encoder.Compression, (long)compressEncoder);
                Image pages = images[0];
                int frame = 0;

                ImageCodecInfo info = GetEncoderInfo("image/tiff");
                foreach (Image img in images)
                {
                    if (frame == 0)
                    {
                        pages = img;
                        pages.Save(outFile, info, ep);
                    }
                    else
                    {
                        ep.Param[0] = new EncoderParameter(enc, (long)EncoderValue.FrameDimensionPage);
                        pages.SaveAdd(img, ep);
                    }
                    if (frame == images.Count() - 1)
                    {
                        ep.Param[0] = new EncoderParameter(enc, (long)EncoderValue.Flush);
                        pages.SaveAdd(ep);
                    }

                    frame++;
                }
            }
        }

        /// <summary>
        ///  轉碼 (上一隻會呼叫到)
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            ImageCodecInfo[] encoders = ImageCodecInfo.GetImageEncoders();
            for (int j = 0; j < encoders.Length; j++)
            {
                if (encoders[j].MimeType == mimeType)
                    return encoders[j];
            }
            throw new Exception(mimeType + " mime type not found in ImageCodecInfo");
        }
    }
}
