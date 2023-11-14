using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AForge.Imaging;

/*
    參考網址： https://blog.51cto.com/u_15316096/5826621
    (1) NuGet 裝上 AForge.Imaging
    (2) 參考/右鍵/加入參考/組件/System.Drawing
 */

namespace 找出圖片在另1張圖片的位置
{
    internal class Program
    {
        static void Main(string[] args)
        {
            SearchImg("t1.png", "t2.png", "output.png");
            Console.WriteLine("尋找完成");
            Console.ReadLine();
        }

        /// <summary>
        /// 尋找圖片
        /// </summary>
        /// <param name="sourceImgFilePath"></param>
        /// <param name="templateImgFIlePath"></param>
        /// <returns></returns>
        public static bool SearchImg(string sourceImgFilePath, string templateImgFIlePath, string outputImgFilePath)
        {
            try
            {
                // 轉成 BItmap格式
                Bitmap sourceImage = ConvertToFormat(System.Drawing.Image.FromFile(sourceImgFilePath), PixelFormat.Format24bppRgb);
                Bitmap template = ConvertToFormat(System.Drawing.Image.FromFile(templateImgFIlePath), PixelFormat.Format24bppRgb);

                // 相似度設定多少
                ExhaustiveTemplateMatching tm = new ExhaustiveTemplateMatching(0.921f);

                // 比對
                TemplateMatch[] matchings = tm.ProcessImage(sourceImage, template);

                // 鎖定圖片，並畫框框
                BitmapData data = sourceImage.LockBits( new Rectangle(0, 0, sourceImage.Width, sourceImage.Height), ImageLockMode.ReadWrite, sourceImage.PixelFormat);
                foreach (TemplateMatch m in matchings)
                {

                    Drawing.Rectangle(data, m.Rectangle, Color.White);
                }

                // 解除鎖定
                sourceImage.UnlockBits(data);

                // 儲存
                sourceImage.Save(outputImgFilePath);

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        /// <summary>
        /// 轉 Pixel 的格式
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format"></param>
        /// <returns></returns>
        public static Bitmap ConvertToFormat(System.Drawing.Image image, PixelFormat format)
        {
            Bitmap copy = new Bitmap(image.Width, image.Height, format);
            using (Graphics gr = Graphics.FromImage(copy))
            {
                gr.DrawImage(image, new Rectangle(0, 0, copy.Width, copy.Height));
            }
            return copy;
        }
    }
}
