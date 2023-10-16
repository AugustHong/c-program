using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址： https://blog.imfing.com/2015/09/csharp-mosaic/ 、 https://www.twblogs.net/a/5b8253d12b71772b883031a0
 */

namespace 圖片馬賽克
{
    internal class Program
    {
        static void Main(string[] args)
        {
            toMosaic("input.png", 20, "output.png", System.Drawing.Imaging.ImageFormat.Png);
            Console.WriteLine("執行完成");
            Console.Read();
        }

        /// <summary>
        /// 馬賽克
        /// </summary>
        /// <param name="ImgFile">來源檔案(完整路徑)</param>
        /// <param name="effectWidth">影響範圍</param>
        /// <param name="saveImgFile">儲存檔名(完整路徑)</param>
        /// <param name="saveFormat">儲存格式</param>
        /// <returns></returns>
        public static string toMosaic(string ImgFile, int effectWidth, string saveImgFile, System.Drawing.Imaging.ImageFormat saveFormat)
        {
            string result = "";
            try
            {
                using (Bitmap img = new Bitmap(Image.FromFile(ImgFile)))
                {
                    // 差異最多的就是以照一定範圍取樣 之後直接去下一個範圍
                    for (int h = 0; h < img.Height; h += effectWidth)
                    {
                        for (int w = 0; w < img.Width; w += effectWidth)
                        {
                            int avgR = 0, avgG = 0, avgB = 0;
                            int count = 0;

                            // 跑過的所有範圍
                            List<(int, int)> allRange = new List<(int, int)>();

                            // 先跑過所有範圍(要算平圴的顏色)
                            for (int x = w; (x < w + effectWidth && x < img.Width); x++)
                            {
                                for (int y = h; (y < h + effectWidth && y < img.Height); y++)
                                {
                                    Color pix = img.GetPixel(x, y);
                                    avgR += pix.R;
                                    avgG += pix.G;
                                    avgB += pix.B;
                                    count++;

                                    // 加入進去，下面就不用再重跑2重迴圈(會快一點點)
                                    allRange.Add((x, y));
                                }
                            }

                            // 計算範圍平均
                            avgR = avgR / count;
                            avgG = avgG / count;
                            avgB = avgB / count;
                            Color newColor = Color.FromArgb(avgR, avgG, avgB);

                            // 所有範圍內都設定此值
                            //for (int x = w; (x < w + effectWidth && x < img.Width); x++)
                            //{
                            //    for (int y = h; (y < h + effectWidth && y < img.Height); y++)
                            //    { 
                            //        img.SetPixel(x, y, newColor);
                            //    }
                            //}
                            foreach (var range in allRange)
                            {
                                img.SetPixel(range.Item1, range.Item2, newColor);
                            }
                        }
                    }

                    img.Save(saveImgFile, saveFormat);
                }
            }
            catch (Exception ex)
            {
                result = ex.Message;
            }

            return result;
        }
    }
}
