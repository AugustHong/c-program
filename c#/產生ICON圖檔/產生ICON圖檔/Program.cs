using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Imaging;

namespace 產生ICON圖檔
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //-------------參數設定---------------
            string text = "F";  // 中間的文字(請自行調整) 
            string hexColor = "#FFA500"; // 亮橘色
            string dirName = "output/"; // 產出目錄名稱

            // 各個 icon 的大小
            Dictionary<string, List<int>> iconSizeList = new Dictionary<string, List<int>>();
            iconSizeList["android-icon"] = new List<int>() { 36, 48, 72, 96, 144, 192 };
            iconSizeList["apple-icon"] = new List<int>() { 57, 60, 72, 76, 114, 120, 144, 152, 180 };
            iconSizeList["favicon"] = new List<int>() { 16, 32, 96 };
            iconSizeList["ms-icon"] = new List<int>() { 144 };
            //-----------------------------------

            // 依照各大小跑迴圈
            foreach (var kvp in iconSizeList)
            {
                string iconType = kvp.Key;
                List<int> sizeList = kvp.Value;

                foreach (var size in sizeList)
                {
                    string fileName = $"{dirName}/{iconType}-{size}x{size}.png";
                    GenerateImage(size, text, hexColor, fileName);
                    Console.WriteLine($"圖片已產生：{fileName}");
                }
            }

            Console.Read();
        }

        static void GenerateImage(int size, string text, string hexColor, string fileName)
        {
            Color mainColor = ColorTranslator.FromHtml(hexColor);
            float offsetY = size * 0.05f;  // 置中有些文字會看起來在上面，所以加個往下移的量 (5%)

            using (Bitmap bmp = new Bitmap(size, size))
            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

                // 👉 背景（透明）
                g.Clear(Color.Transparent);

                int padding = size / 16;

                Rectangle circleRect = new Rectangle(
                    padding,
                    padding,
                    size - padding * 2,
                    size - padding * 2
                );

                // 畫圓
                using (Pen pen = new Pen(mainColor, size / 16f))
                {
                    g.DrawEllipse(pen, circleRect);
                }

                // 字體大小（約 75%）
                float fontSize = size * 0.75f;

                Font font;
                SizeF textSize;

                do
                {
                    font = new Font("Arial", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);
                    textSize = g.MeasureString(text, font);
                    fontSize -= 1f;

                } while ((textSize.Width > circleRect.Width || textSize.Height > circleRect.Height) && fontSize > 1);

                // 置中            
                float x = (size - textSize.Width) / 2;
                float y = (size - textSize.Height) / 2 + offsetY;

                // 畫文字（同顏色）
                using (Brush brush = new SolidBrush(mainColor))
                {
                    g.DrawString(text, font, brush, x, y);
                }

                bmp.Save(fileName, ImageFormat.Png);
            }
        }
    }
}
