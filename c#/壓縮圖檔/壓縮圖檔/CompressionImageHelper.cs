using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;

/*
    參考網址： (參考/右鍵/加入參考/System.Drawing)
    https://blog.csdn.net/weixin_42953003/article/details/119751529
    https://www.mlplus.net/2020/04/04/csharpimagecompress/
 */

namespace 壓縮圖檔
{
    public class CompressionImageHelper
    {
        /// <summary>
        /// 取得加密碼
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private static ImageCodecInfo GetEncoder(ImageFormat format)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            foreach (ImageCodecInfo codec in codecs)
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            return null;
        }

        /// <summary>
        /// 壓縮圖檔
        /// </summary>
        /// <param name="stream">串流值</param>
        /// <param name="quality">品質(100就是原圖)</param>
        /// <returns></returns>
        private static byte[] CompressionImage(Stream fileStream, long quality)
        {
            using (Image img = Image.FromStream(fileStream))
            {
                using (Bitmap bitmap = new Bitmap(img))
                {
                    ImageCodecInfo CodecInfo = GetEncoder(img.RawFormat);
                    Encoder myEncoder = Encoder.Quality;
                    EncoderParameters myEncoderParameters = new EncoderParameters(1);
                    EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, quality);
                    myEncoderParameters.Param[0] = myEncoderParameter;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        bitmap.Save(ms, CodecInfo, myEncoderParameters);
                        myEncoderParameters?.Dispose();
                        myEncoderParameter?.Dispose();
                        return ms.ToArray();
                    }
                }
            }
        }

        /// <summary>
        /// 壓縮圖檔
        /// </summary>
        /// <param name="imagePath">來源圖檔</param>
        /// <param name="quality">品質(100就是原圖)</param>
        /// <returns></returns>
        private static byte[] CompressionImage(string imagePath, long quality)
        {
            using (FileStream fileStream = new FileStream(imagePath, FileMode.Open))
            {
                return CompressionImage(fileStream, quality);
            }
        }

        /// <summary>
        /// 用 Stream 寫檔，並回傳檔案大小
        /// </summary>
        /// <param name="fileStream">資料流</param>
        /// <param name="targetFullFile"></param>
        private static long SaveImg(byte[] imageByte, string targetFullFile)
        {
            using (MemoryStream ms = new MemoryStream(imageByte))
            {
                using (Image image = Image.FromStream(ms))
                {
                    image.Save(targetFullFile);
                    return ms.Length;
                }
            }
        }

        /// <summary>
        /// 取得 新目標圖檔路徑
        /// </summary>
        /// <param name="imagePath">來源圖檔</param>
        /// <param name="targetfolder">壓縮後目標路徑</param>
        /// <returns></returns>
        public static string GetOutputFile(string imagePath, string targetfolder)
        {
            // 檢查
            if (!File.Exists(imagePath))
            {
                throw new FileNotFoundException();
            }

            // 若無目標資料夾，新增它
            if (!Directory.Exists(targetfolder))
            {
                Directory.CreateDirectory(targetfolder);
            }

            // 取得檔案 (要組出 回覆檔用的)
            FileInfo fileInfo = new FileInfo(imagePath);
            string fileName = fileInfo.Name.Replace(fileInfo.Extension, "");
            string fileFullName = Path.Combine($"{targetfolder}", $"{fileName}{fileInfo.Extension}");

            return fileFullName;
        }

        /// <summary>
        /// 開始壓縮 (用品質壓縮)
        /// </summary>
        /// <param name="imagePath">來源圖檔</param>
        /// <param name="targetfolder">壓縮後目標路徑</param>
        /// <param name="quality">壓縮品質</param>
        /// <exception cref="FileNotFoundException"></exception>
        public static void CompressionImageByQuality(string imagePath, string targetfolder, long quality = 100)
        {
            // 實作，並儲存
            string fileFullName = GetOutputFile(imagePath, targetfolder);
            byte[] imageByte = CompressionImage(imagePath, quality);
            SaveImg(imageByte, fileFullName);
        }

        /// <summary>
        ///  開始壓縮 (壓到 特定KB 以下)
        /// </summary>
        /// <param name="imagePath">來源圖檔</param>
        /// <param name="targetfolder">壓縮後目標路徑</param>
        /// <param name="kbSize">最大KB數</param>
        /// <param name="nearRangKbSize">偏差值(因為不可能剛好)</param>
        public static bool CompressionImageByMaxFileSize(string imagePath, string targetfolder, long kbSize = 30, long nearRangKbSize = 10)
        {
            string fileFullName = GetOutputFile(imagePath, targetfolder);

            long maxKbSize = kbSize * 1024;  // 最大
            long minKbSize = (kbSize - nearRangKbSize) * 1024; // 最小 (最大 - 偏差值)

            if (minKbSize > maxKbSize)
            {
                minKbSize = maxKbSize;
            }

            // 取到原檔案大小
            long srcOldLength = 0;
            using (Image image = Image.FromFile(imagePath))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Save(ms, image.RawFormat);
                    srcOldLength = ms.Length;
                }
            }

            // 如果原本的大小就符合了，就直接回傳 (回壓縮品質 = 100 的就是原值)
            if (srcOldLength < maxKbSize)
            {
                byte[] imageByte = CompressionImage(imagePath, 100);
                SaveImg(imageByte, fileFullName);
                return true;
            }
            else
            {
                long startQ = 0;
                long endQ = 100;  //品質最高是100
                long q = (startQ + endQ) / 2;  //一開始抓 50

                bool result = false;
                while (true)
                {
                    // 得到處理後的 KB 數
                    byte[] afterImageByte = CompressionImage(imagePath, q);
                    SaveImg(afterImageByte, fileFullName);
                    long afterKbSize;

                    using (Image image = Image.FromFile(fileFullName))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            image.Save(ms, image.RawFormat);
                            afterKbSize = ms.Length;
                        }
                    }

                    // 若處理後的 介於區間內，回傳
                    if (afterKbSize <= maxKbSize && afterKbSize >= minKbSize)
                    {
                        result = true;
                        break;
                    }
                    else
                    {
                        // 若 處理後的 < min，繼續升
                        if (afterKbSize < minKbSize)
                        {
                            startQ = q;
                        }
                        else
                        {
                            endQ = q;
                        }
                    }

                    // 若 開始 > 結束，直接回傳此結果
                    if (startQ > endQ)
                    {
                        break;
                    }

                    // 再次取到新值
                    long newQ = (startQ + endQ) / 2;

                    // 若結果相同，直接回傳
                    if (q == newQ)
                    {
                        break;
                    }

                    q = newQ;
                }

                return result;
            }
        }

        /// <summary>
        /// 開始壓縮 (壓到 指定長寬比下)
        /// </summary>
        /// <param name="imagePath">來源圖檔</param>
        /// <param name="targetfolder">壓縮後目標路徑</param>
        /// <param name="targetHeight">目標長</param>
        /// <param name="targetWidth">目標寬</param>
        public static void CompressionImageByPicSize(string imagePath, string targetfolder, int targetHeight, int targetWidth)
        {
            string fileFullName = GetOutputFile(imagePath, targetfolder);

            using (Image image = Image.FromFile(imagePath))
            {
                int w = 0, h = 0;

                // 按比例
                int sourceWidth = image.Width;
                int sourceHeight = image.Height;

                if (sourceHeight > targetHeight || sourceWidth > targetWidth)
                {
                    if ((sourceWidth * targetHeight) > (sourceHeight * targetWidth))
                    {
                        w = targetWidth;
                        h = (targetWidth * sourceHeight) / sourceWidth;
                    }
                    else
                    {
                        h = targetHeight;
                        w = (sourceWidth * targetHeight) / sourceHeight;
                    }
                }
                else
                {
                    w = sourceWidth;
                    h = sourceHeight;
                }

                int tw = (targetWidth - w) / 2;
                int th = (targetHeight - h) / 2;

                Bitmap b = new Bitmap(targetWidth, targetHeight);
                using (Graphics g = Graphics.FromImage(b))
                {
                    g.Clear(Color.Transparent);
                    // 設定質量
                    g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                    g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                    g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                    g.DrawImage(image, new Rectangle(tw,th, w, h), 0, 0, sourceWidth, sourceHeight, GraphicsUnit.Pixel);
                }

                Image i = b;
                i.Save(fileFullName);
            }
        }
    }
}
