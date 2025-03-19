using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    先去 Nuget 裝上 SkiaSharp 和 SkiaSharp.NativeAssets.Win32 和 DoNotUse.SkiaSharp.NativeAssets.Win32
    手動將 packages\SkiaSharp.NativeAssets.Win32.3.118.0-preview.2.3\runtimes\{依據你的執行環境 x64 還是 x86}\native\libSkiaSharp.dll 複製到 bin裡面

    備註：
    (1) 這邊的特效都是請Copilot 產的範例(所以都很慘)，實際還是依情況產生。主要是要說明怎麼用 SkiaSharp 而已
    (2) 用 <<請使用 SkiaSharp 產生 C# 語法，以產生 下雨 的動畫所需的圖片>> 來問問AI
 */

namespace SkiaSharp實作
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("請輸入一段文字（例如：火、風、水、岩石、雷、雪、閃電、爆炸、殞石、土石流、下雨、濺血、龍卷風）：");
            string inputText = Console.ReadLine();
            Console.WriteLine($"生成 {inputText} 的動畫中");
            Action<int, string> action = null;

            switch (inputText)
            {
                case "火":
                    action = GenerateFireFrame;
                    break;
                case "風":
                    action = GenerateWindFrame;
                    break;
                case "水":
                    action = GenerateWaterFrame;
                    break;
                case "岩石":
                    action = GenerateRockRollingFrame;
                    break;
                case "雷":
                    action = GenerateThunderFrame;
                    break;
                case "雪":
                    action = GenerateSnowFrame;
                    break;
                case "閃電":
                    action = GenerateLightningFrame;
                    break;
                case "爆炸":
                    action = GenerateExplosionFrame;
                    break;
                case "殞石":
                    action = GenerateMeteorFrame;
                    break;
                case "土石流":
                    action = GenerateLandslideFrame;
                    break;
                case "下雨":
                    action = GenerateRainFrame;
                    break;
                case "濺血":
                    action = GenerateBloodSplatterFrame;
                    break;
                case "龍卷風":
                    action = GenerateTornadoFrame;
                    break;
                default:
                    Console.WriteLine("目前尚不支持該文字效果：" + inputText);
                    break;
            }

            GenerateAnimation(inputText, action);
        }

        /// <summary>
        /// 產生逐幀動畫
        /// </summary>
        /// <param name="effectName"></param>
        /// <param name="frameGenerator"></param>
        static void GenerateAnimation(string effectName, Action<int, string> frameGenerator)
        {
            const int frameCount = 30; // 幀數
            const int width = 800;    // 圖像寬度
            const int height = 600;   // 圖像高度

            for (int i = 0; i < frameCount; i++)
            {
                string fileName = $"frame_{i:D3}.png";
                frameGenerator(i, fileName);
                Console.WriteLine($"幀 {fileName} 已生成。");
            }

            // 使用 FFmpeg 將圖片序列轉換為 MP4 動畫
            string outputPath = $"{effectName}_animation.mp4";
            CreateVideoWithFFmpeg("frame_%03d.png", outputPath);
        }

        /// <summary>
        /// 組成 影片檔 (必需要先裝有 FFmpeg)
        /// </summary>
        /// <param name="inputPattern"></param>
        /// <param name="outputPath"></param>
        static void CreateVideoWithFFmpeg(string inputPattern, string outputPath)
        {
            /*
            string ffmpegPath = "ffmpeg"; // 確保 FFmpeg 已安裝並配置到系統環境變數
            string args = $"-framerate 30 -i {inputPattern} -c:v libx264 -pix_fmt yuv420p {outputPath}";

            Process ffmpeg = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = ffmpegPath,
                    Arguments = args,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    UseShellExecute = false,
                    CreateNoWindow = true
                }
            };

            ffmpeg.Start();
            ffmpeg.WaitForExit();

            if (ffmpeg.ExitCode == 0)
            {
                Console.WriteLine($"動畫生成成功，檔案保存至：{outputPath}");
            }
            else
            {
                Console.WriteLine("動畫生成失敗，請檢查素材和 FFmpeg 配置。");
            }
            */
        }

        /// <summary>
        /// 將檔案存檔
        /// </summary>
        /// <param name="bitmap"></param>
        /// <param name="fileName"></param>
        static void SaveBitmapToFile(SKBitmap bitmap, string fileName)
        {
            using (var image = SKImage.FromBitmap(bitmap))
            using (var data = image.Encode(SKEncodedImageFormat.Png, 100))
            {
                File.WriteAllBytes(fileName, data.ToArray());
            }
        }

        #region 特效區

        /// <summary>
        /// 生成火特效
        /// </summary>
        /// <param name="frameIndex"></param>
        /// <param name="fileName"></param>
        static void GenerateFireFrame(int frameIndex, string fileName)
        {
            using (var bitmap = new SKBitmap(800, 600))
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.Black);

                // 火焰效果
                using (var paint = new SKPaint())
                {
                    paint.Style = SKPaintStyle.Fill;
                    var random = new Random(frameIndex);
                    for (int i = 0; i < 50; i++)
                    {
                        paint.Color = new SKColor(
                            (byte)random.Next(200, 255),
                            (byte)random.Next(100, 150),
                            0,
                            (byte)random.Next(150, 255)
                        );

                        float x = random.Next(0, 800);
                        float y = 600 - random.Next(0, 300);
                        float size = random.Next(10, 40);

                        canvas.DrawCircle(x, y, size, paint);
                    }
                }

                SaveBitmapToFile(bitmap, fileName);
            }
        }

        /// <summary>
        /// 生成風特效
        /// </summary>
        /// <param name="frameIndex"></param>
        /// <param name="fileName"></param>
        static void GenerateWindFrame(int frameIndex, string fileName)
        {
            using (var bitmap = new SKBitmap(800, 600))
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.LightBlue);

                // 風效果：繪製曲線
                using (var paint = new SKPaint())
                {
                    paint.Style = SKPaintStyle.Stroke;
                    paint.StrokeWidth = 3;
                    paint.Color = SKColors.White;
                    var random = new Random(frameIndex);

                    for (int i = 0; i < 10; i++)
                    {
                        float startX = random.Next(0, 800);
                        float startY = random.Next(0, 600);
                        float endX = startX + random.Next(50, 200);
                        float endY = startY + random.Next(-50, 50);

                        var path = new SKPath();
                        path.MoveTo(startX, startY);
                        path.CubicTo(startX + 30, startY - 30, endX - 30, endY + 30, endX, endY);

                        canvas.DrawPath(path, paint);
                    }
                }

                SaveBitmapToFile(bitmap, fileName);
            }
        }

        /// <summary>
        /// 生成水特效
        /// </summary>
        /// <param name="frameIndex"></param>
        /// <param name="fileName"></param>
        static void GenerateWaterFrame(int frameIndex, string fileName)
        {
            using (var bitmap = new SKBitmap(800, 600))
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.Blue);

                // 水效果：繪製波浪
                using (var paint = new SKPaint())
                {
                    paint.Style = SKPaintStyle.Stroke;
                    paint.StrokeWidth = 4;
                    paint.Color = SKColors.Cyan;

                    float waveHeight = 20;
                    float waveLength = 100;
                    float y = 300 + (float)Math.Sin(frameIndex * 0.2) * waveHeight;

                    for (int x = 0; x < 800; x += 10)
                    {
                        float waveY = y + (float)Math.Sin((x + frameIndex * 5) * 0.1) * waveHeight;
                        canvas.DrawLine(x, waveY, x + 10, waveY, paint);
                    }
                }

                SaveBitmapToFile(bitmap, fileName);
            }
        }

        /// <summary>
        /// 生成岩石特效
        /// </summary>
        /// <param name="frameIndex"></param>
        /// <param name="fileName"></param>
        static void GenerateRockRollingFrame(int frameIndex, string fileName)
        {
            using (var bitmap = new SKBitmap(800, 600))
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.LightGray);

                // 繪製地面
                using (var groundPaint = new SKPaint())
                {
                    groundPaint.Style = SKPaintStyle.Fill;
                    groundPaint.Color = SKColors.DarkGray;
                    canvas.DrawRect(0, 500, 800, 100, groundPaint);
                }

                // 繪製滾動的岩石
                using (var rockPaint = new SKPaint())
                {
                    rockPaint.Style = SKPaintStyle.Fill;
                    rockPaint.Color = SKColors.Brown;

                    // 計算岩石的位置
                    int radius = 50;
                    int x = (frameIndex * 20) % 800 + radius; // 模擬滾動效果
                    int y = 500 - radius;

                    canvas.DrawCircle(x, y, radius, rockPaint);
                }

                SaveBitmapToFile(bitmap, fileName);
            }
        }

        /// <summary>
        /// 生成雷特效
        /// </summary>
        /// <param name="frameIndex"></param>
        /// <param name="fileName"></param>
        static void GenerateThunderFrame(int frameIndex, string fileName)
        {
            using (var bitmap = new SKBitmap(800, 600))
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.DarkBlue);

                using (var paint = new SKPaint())
                {
                    paint.Style = SKPaintStyle.Stroke;
                    paint.StrokeWidth = 5;
                    paint.Color = SKColors.Yellow;

                    Random random = new Random(frameIndex);
                    float startX = random.Next(0, 800);
                    float startY = 0;
                    float endX = startX + random.Next(-50, 50);
                    float endY = 600;

                    canvas.DrawLine(startX, startY, endX, endY, paint);
                }

                SaveBitmapToFile(bitmap, fileName);
            }
        }

        /// <summary>
        /// 生成雪特效
        /// </summary>
        /// <param name="frameIndex"></param>
        /// <param name="fileName"></param>
        static void GenerateSnowFrame(int frameIndex, string fileName)
        {
            using (var bitmap = new SKBitmap(800, 600))
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.White);

                using (var paint = new SKPaint())
                {
                    paint.Style = SKPaintStyle.Fill;
                    paint.Color = SKColors.LightGray;

                    Random random = new Random(frameIndex);
                    for (int i = 0; i < 50; i++)
                    {
                        float x = random.Next(0, 800);
                        float y = (frameIndex * 10 + random.Next(0, 600)) % 600;
                        canvas.DrawCircle(x, y, random.Next(5, 10), paint);
                    }
                }

                SaveBitmapToFile(bitmap, fileName);
            }
        }

        /// <summary>
        /// 生成閃電特效
        /// </summary>
        /// <param name="frameIndex"></param>
        /// <param name="fileName"></param>
        static void GenerateLightningFrame(int frameIndex, string fileName)
        {
            using (var bitmap = new SKBitmap(800, 600))
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.Black);

                using (var paint = new SKPaint())
                {
                    paint.Style = SKPaintStyle.Stroke;
                    paint.StrokeWidth = 10;
                    paint.Color = SKColors.White;

                    Random random = new Random(frameIndex);
                    float x = random.Next(0, 800);
                    float y = 0;

                    for (int i = 0; i < 5; i++)
                    {
                        float nextX = x + random.Next(-50, 50);
                        float nextY = y + random.Next(50, 100);
                        canvas.DrawLine(x, y, nextX, nextY, paint);
                        x = nextX;
                        y = nextY;
                    }
                }

                SaveBitmapToFile(bitmap, fileName);
            }
        }

        /// <summary>
        /// 生成爆炸特效
        /// </summary>
        /// <param name="frameIndex"></param>
        /// <param name="fileName"></param>
        static void GenerateExplosionFrame(int frameIndex, string fileName)
        {
            using (var bitmap = new SKBitmap(800, 600))
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.Black);

                using (var paint = new SKPaint())
                {
                    paint.Style = SKPaintStyle.Fill;

                    Random random = new Random(frameIndex);
                    for (int i = 0; i < 50; i++)
                    {
                        paint.Color = new SKColor(
                            (byte)random.Next(200, 255),
                            (byte)random.Next(50, 150),
                            (byte)random.Next(50, 100),
                            (byte)random.Next(150, 255)
                        );

                        float x = 400 + random.Next(-150, 150);
                        float y = 300 + random.Next(-150, 150);
                        float size = random.Next(10, 40);

                        canvas.DrawCircle(x, y, size, paint);
                    }
                }

                SaveBitmapToFile(bitmap, fileName);
            }
        }

        /// <summary>
        /// 生成殞石特效
        /// </summary>
        /// <param name="frameIndex"></param>
        /// <param name="fileName"></param>
        static void GenerateMeteorFrame(int frameIndex, string fileName)
        {
            using (var bitmap = new SKBitmap(800, 600))
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.Black);

                using (var paint = new SKPaint())
                {
                    paint.Style = SKPaintStyle.Fill;

                    Random random = new Random(frameIndex);
                    int x = random.Next(0, 800);
                    int y = frameIndex * 20 % 600; // 模擬下墜

                    paint.Color = SKColors.Gray;
                    canvas.DrawCircle(x, y, 20, paint); // 殞石本體

                    // 殞石拖尾效果
                    paint.Color = SKColors.LightGray;
                    canvas.DrawOval(new SKRect(x - 10, y - 50, x + 10, y), paint);
                }

                SaveBitmapToFile(bitmap, fileName);
            }
        }

        /// <summary>
        /// 生成土石流特效
        /// </summary>
        /// <param name="frameIndex"></param>
        /// <param name="fileName"></param>
        static void GenerateLandslideFrame(int frameIndex, string fileName)
        {
            using (var bitmap = new SKBitmap(800, 600))
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.DarkGreen);

                using (var paint = new SKPaint())
                {
                    paint.Style = SKPaintStyle.Fill;

                    Random random = new Random(frameIndex);

                    // 模擬土石流的粒子
                    for (int i = 0; i < 100; i++)
                    {
                        paint.Color = random.Next(0, 2) == 0 ? SKColors.Brown : SKColors.Gray;

                        float x = random.Next(0, 800);
                        float y = (frameIndex * 10 + random.Next(0, 600)) % 600;

                        canvas.DrawCircle(x, y, random.Next(3, 7), paint);
                    }
                }

                SaveBitmapToFile(bitmap, fileName);
            }
        }

        /// <summary>
        /// 生成下雨特效
        /// </summary>
        /// <param name="frameIndex"></param>
        /// <param name="fileName"></param>
        static void GenerateRainFrame(int frameIndex, string fileName)
        {
            using (var bitmap = new SKBitmap(800, 600))
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.Black);

                using (var paint = new SKPaint())
                {
                    paint.StrokeWidth = 2;

                    Random random = new Random(frameIndex);

                    // 模擬雨滴的粒子
                    for (int i = 0; i < 200; i++)
                    {
                        paint.Color = SKColor.Parse("#88C0D0"); // 雨滴顏色

                        float x = random.Next(0, 800);
                        float y = (frameIndex * 10 + random.Next(0, 600)) % 600;

                        // 畫出雨滴
                        canvas.DrawLine(x, y, x, y + 10, paint);
                    }
                }

                SaveBitmapToFile(bitmap, fileName);
            }
        }

        /// <summary>
        /// 生成 濺血 動畫
        /// </summary>
        /// <param name="frameIndex"></param>
        /// <param name="fileName"></param>
        static void GenerateBloodSplatterFrame(int frameIndex, string fileName)
        {
            using (var bitmap = new SKBitmap(800, 600))
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.White);

                using (var paint = new SKPaint())
                {
                    paint.Style = SKPaintStyle.Fill;
                    Random random = new Random(frameIndex);

                    // 模擬血滴
                    for (int i = 0; i < 50; i++)
                    {
                        paint.Color = SKColors.Red;
                        float x = random.Next(0, 800);
                        float y = random.Next(0, 600);
                        float radius = random.Next(5, 20);

                        canvas.DrawCircle(x, y, radius, paint);
                    }
                }

                SaveBitmapToFile(bitmap, fileName);
            }
        }

        /// <summary>
        /// 生成 龍卷風 動畫
        /// </summary>
        /// <param name="frameIndex"></param>
        /// <param name="fileName"></param>
        static void GenerateTornadoFrame(int frameIndex, string fileName)
        {
            using (var bitmap = new SKBitmap(800, 600))
            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(SKColors.SkyBlue);

                using (var paint = new SKPaint())
                {
                    paint.Style = SKPaintStyle.StrokeAndFill;
                    Random random = new Random(frameIndex);

                    // 模擬龍捲風粒子
                    for (int i = 0; i < 200; i++)
                    {
                        paint.Color = SKColors.Gray;

                        float angle = random.Next(0, 360) * (float)Math.PI / 180;
                        float radius = random.Next(10, 200);
                        float centerX = 400 + radius * (float)Math.Cos(angle) / ((frameIndex % 50) + 1);
                        float centerY = 300 + radius * (float)Math.Sin(angle) / 2;

                        canvas.DrawCircle(centerX, centerY, random.Next(3, 6), paint);
                    }
                }

                SaveBitmapToFile(bitmap, fileName);
            }
        }

        #endregion
    }
}
