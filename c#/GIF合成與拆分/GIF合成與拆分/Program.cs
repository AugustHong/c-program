using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Gif.Components;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;

/*
    參考網圵： https://www.itread01.com/articles/1478487926.html
    1. 去下載 Git.Components.dll (我這邊有)
 */

namespace GIF合成與拆分
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = Directory.GetCurrentDirectory() + "\\";

            string pngPath = rootPath + "PNG\\";
            string gifPath = rootPath + "GIF\\";

            // 合成 GIF
            List<string> pngSource = new List<string> { "source.png", "source1.png", "source2.png", "source3.png" };
            pngSource = pngSource.Select(x => pngPath + x).ToList();
            Console.WriteLine("開始合成 GIF ");
            GifHelper.MakeGIF(gifPath + "合成.gif", pngSource);

            // ----------------------------------------------------------------------------------------------

            // 拆分 GIF
            // 方法一：
            Console.WriteLine("開始拆分 GIF ");
            GifHelper.SplitGIF(gifPath + "合成.gif", rootPath + "PNG", "png");

            /*
                方法二：
                    Image imgGif = Image.FromFile(fileNames[i]);//加載gif文件
                    FrameDimension ImgFrmDim = new FrameDimension( imgGif.FrameDimensionsList[0] );
                    int nFrameCount = imgGif.GetFrameCount(ImgFrmDim);

                    for( int a = 0; a < nFrameCount; a++)
                    {
                      imgGif.SelectActiveFrame( ImgFrmDim, a );
                      imgGif.Save(rootPath + "PNG\\" + $"導出{a}.png", ImageFormat.Png);
                    }
             */

            Console.WriteLine("製作完成");
        }
    }

    public static class GifHelper
    {
        /// <summary>
        /// 產生 GIF
        /// </summary>
        /// <param name="gifPath">gif儲存路徑(完整路徑)</param>
        /// <param name="dealy">幀率(預設200)(毫秒)</param>
        /// <param name="height">長(如 < 0 給 第一張圖的 長)(預設0)</param>
        /// <param name="width">寬(如 < 0 給 第一張圖的 寬)(預設0)</param>
        /// <param name="replace">是否重複(預計否)</param>
        /// <param name="sourcePicPathList">來源圖片路徑(完整路徑)</param>
        public static void MakeGIF(string gifPath, List<string> sourcePicPathListint, int dealy = 200, int height = 0, int width = 0, bool replace = false)
        {
            if (sourcePicPathListint.Count() > 0)
            {
                if (!string.IsNullOrEmpty(gifPath))
                {
                    // 把所有的 string 轉成 Gif.Components.Image
                    List<Image> imgList = new List<Image>();
                    foreach (var picPath in sourcePicPathListint)
                    {
                        try
                        {
                            Image img = Image.FromFile(picPath);
                            imgList.Add(img);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"{picPath} 轉成圖片發生錯誤 ： {ex.Message}");
                        }
                    }

                    // 丟到下一隻函式
                    MakeGIF(gifPath, imgList, dealy, height, width, replace);
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="gifPath">gif儲存路徑(完整路徑)</param>
        /// <param name="dealy">幀率(預設200)(毫秒)</param>
        /// <param name="height">長(如 < 0 給 第一張圖的 長)(預設0)</param>
        /// <param name="width">寬(如 < 0 給 第一張圖的 寬)(預設0)</param>
        /// <param name="replace">是否重複(預計否)</param>
        /// <param name="sourcePic">來源圖片</param>
        public static void MakeGIF(string gifPath, List<Image> sourcePic, int dealy = 200, int height = 0, int width = 0, bool replace = false)
        {
            if (sourcePic.Count() > 0)
            {
                if (!string.IsNullOrEmpty(gifPath))
                {
                    if (File.Exists(gifPath))
                    {
                        File.Delete(gifPath);
                    }

                    // 處理相關參數
                    Image first = sourcePic[0];
                    dealy = dealy <= 0 ? 200 : dealy;
                    height = height <= 0 ? first.Height : height;
                    width = width <= 0 ? first.Width : width;
                    int replaceNum = replace ? 1 : 0;

                    // 開始實作
                    AnimatedGifEncoder MyGif = new AnimatedGifEncoder();
                    MyGif.Start(gifPath);//保存路徑

                    MyGif.SetDelay(dealy);//幀率
                    MyGif.SetSize(width, height);//尺寸
                    MyGif.SetRepeat(replaceNum);//重復

                    foreach (var img in sourcePic)
                    {
                        MyGif.AddFrame(img);
                    }

                    MyGif.Finish();
                }
            }
        }

        /// <summary>
        ///  拆分 GIF
        /// </summary>
        /// <param name="gifPath">gif路徑(完整路徑)</param>
        /// <param name="splitDir">輸出目錄(請不要加 \\)</param>
        /// <param name="type">副檔名類型 (請輸入 png, jpg, bmp, ico, tif)</param>
        public static void SplitGIF(string gifPath, string splitDir, string type = "png")
        {
            if (File.Exists(gifPath) && (!string.IsNullOrEmpty(type)))
            {
                if (Directory.Exists(splitDir))
                {
                    // 看副檔名
                    (string, ImageFormat) tmp = GetImageFormat(type);
                    ImageFormat format = tmp.Item2;
                    string subName = tmp.Item1;

                    // 開始實作
                    GifDecoder gifDecoder = new GifDecoder();
                    gifDecoder.Read(gifPath);

                    for (int i = 0, count = gifDecoder.GetFrameCount(); i < count; i++)
                    {
                        Image frame = gifDecoder.GetFrame(i); // frame i

                        string filePath = $"{splitDir}\\{i}{subName}";
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        frame.Save(filePath, format);
                    }

                }
            }
        }

        /// <summary>
        ///  得到 Image 的 Format
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static (string, ImageFormat) GetImageFormat(string type)
        {
            ImageFormat format = ImageFormat.Png;
            string subName = ".png";

            if (!string.IsNullOrEmpty(type))
            {
                type = type.ToUpper();
                switch (type)
                {
                    case "PNG":
                        subName = ".png";
                        format = ImageFormat.Png;
                        break;
                    case "JPG":
                    case "JPEG":
                        subName = ".jpg";
                        format = ImageFormat.Jpeg;
                        break;
                    case "BMP":
                        subName = ".bmp";
                        format = ImageFormat.Bmp;
                        break;
                    case "ICO":
                        subName = ".ico";
                        format = ImageFormat.Icon;
                        break;
                    case "TIF":
                        subName = ".tif";
                        format = ImageFormat.Tiff;
                        break;
                    default:
                        break;
                }
            }

            return (subName, format);
        }
    }
}
