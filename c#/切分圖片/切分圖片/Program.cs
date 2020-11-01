using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 切分圖片
{
    class Program
    {
        static void Main(string[] args)
        {
            // 先讀取出檔案
            string rootPath = System.IO.Directory.GetCurrentDirectory() + "\\";
            string sourcePath = rootPath + "Input";
            DirectoryInfo sourceDir = new DirectoryInfo(sourcePath);

            List<string> useFile = new List<string> { ".PNG", ".GIF", ".JPG", ".JPEG" };
            List<string> sourceFileNameList = sourceDir.EnumerateFiles().ToList().Where(w => useFile.Where(u => w.FullName.ToUpper().Contains(u)).Count() > 0).Select(w => w.Name).ToList();

            Console.WriteLine($"已找出符合的項目 {sourceFileNameList.Count()} 筆");

            sourcePath = sourcePath + "\\";
            // 目標路徑
            string goalPath = rootPath + "Output\\";
            Console.WriteLine("------------------------------------------------------------------------");

            foreach (var file in sourceFileNameList)
            {
                Console.WriteLine($"處理 {file}");

                string filePath = sourcePath + file;

                // 得到名字(去掉副檔名)
                List<string> tmp = file.Split('.').ToList();
                string subName = tmp.LastOrDefault();
                tmp.Remove(subName);
                string name = string.Join(",", tmp);

                // 得到 高, 寬
                Image img = Image.FromFile(filePath);
                int height = img.Height;
                int width = img.Width;
                Console.WriteLine($"高 =  {height},  寬 = {width}");

                Console.Write("請輸入 水平 要切幾份？ ");
                int horizontal = IntTryParse(Console.ReadLine());
                horizontal = horizontal <= 0 ? 1 : horizontal;

                Console.Write("請輸入 縱向 要切幾份？ ");
                int vertical = IntTryParse(Console.ReadLine());
                vertical = vertical <= 0 ? 1 : vertical;

                Console.Write("請輸入如果無法整除的話，是否要去掉？(預設是)(請輸入 y or Y or n or N)  ： ");
                bool method = Console.ReadLine().ToUpper() == "N" ? false : true;

                // 開始實作
                int perHeigth = height / vertical;
                int perWidth = width / horizontal;
                Console.WriteLine($"切出來的 高 = {perHeigth}  , 寬 = {perWidth}  (如果無法整除的選 N 的話 會有幾個不是這樣)");

                // 如果是 false 的話，最後的高 和寬
                int lastHeigth = 0;
                int lastWidth = 0;

                if (method == false)
                {
                    if (vertical > 1)
                    {
                        lastHeigth = height - ((vertical - 1) * perHeigth);
                    }
                    else
                    {
                        lastHeigth = height;
                    }

                    if (horizontal > 1)
                    {
                        lastWidth = width - ((horizontal - 1) * perWidth);
                    }
                    else
                    {
                        lastWidth = width;
                    }
                }

                // 算序號
                int index = 1;

                for (var i = 1; i <= horizontal; i++)
                {
                    for (var j = 1; j <= vertical; j++)
                    {
                        Rectangle rect = new Rectangle();

                        if ((method == false) && ((i >= horizontal) || (j >= vertical)))
                        {
                            rect.X = (i - 1) * perWidth;
                            rect.Y = (j - 1) * perHeigth;
                            rect.Width = (i == horizontal) ? lastWidth : perWidth;
                            rect.Height = (j == vertical) ? lastHeigth : perHeigth;
                        }
                        else
                        {
                            // 每個固定大小
                            rect.X = (i - 1) * perWidth;
                            rect.Y = (j - 1) * perHeigth;
                            rect.Width = perWidth;
                            rect.Height = perHeigth;
                        }

                        // 切圖片
                        string goalFilePath = goalPath + $"{name}_{index}.{subName}";
                        CutPicHelper.CutPic(filePath, goalFilePath, rect);

                        // 序號++
                        index++;
                    }
                }
            }

            Console.WriteLine("============================================================");
            Console.WriteLine("執行完畢");
            Console.ReadLine();
        }

        static int IntTryParse(string source)
        {
            int result = 0;

            if (!string.IsNullOrEmpty(source))
            {
                int.TryParse(source, out result);
            }

            return result;
        }
    }

    /// <summary>
    /// 切割圖片 Helper
    /// </summary>
    public static class CutPicHelper
    {
        /// <summary>
        ///  得到 (高, 寬)
        /// </summary>
        /// <param name="filePath">完整檔案路徑</param>
        /// <returns></returns>
        public static (int, int) GetImageWidthAndWidth(string filePath)
        {
            int height = 0;
            int width = 0;

            if (File.Exists(filePath))
            {
                try
                {
                    Image img = Image.FromFile(filePath);
                    height = img.Height;
                    width = img.Width;
                }
                catch
                {

                }
            }

            return (height, width);
        } 

        /// <summary>
        /// 切圖
        /// </summary>
        /// <param name="image">圖源</param>
        /// <param name="fileSaveUrl">要保存的文件名(完整路徑)</param>
        /// <param name="rect">位置大小</param>
        public static bool CutPic(Image image, string fileName, Rectangle range, PixelFormat format = PixelFormat.Format32bppArgb)
        {
            try
            {
                if (File.Exists(fileName))
                {
                    File.Delete(fileName);
                }

                var tmpImg = new Bitmap(image);
                var bitCrop = tmpImg.Clone(range, format);
                bitCrop.Save(fileName);
                bitCrop.Dispose();
                tmpImg.Dispose();
                image.Dispose();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// 切圖
        /// </summary>
        /// <param name="imagePath">圖源完整路徑</param>
        /// <param name="fileName">要保存的文件名(完整路徑)</param>
        /// <param name="rect">位置大小</param>
        public static bool CutPic(string imagePath, string fileName, Rectangle range, PixelFormat format = PixelFormat.Format32bppArgb)
        {
            try
            {
                Image image = Image.FromFile(imagePath);
                return CutPic(image, fileName, range, format);
            }
            catch
            {
                return false;
            }
        }
    }
}
