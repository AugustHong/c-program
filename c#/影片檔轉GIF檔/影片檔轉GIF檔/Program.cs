using NReco.VideoConverter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    參考網圵： https://stackoverflow.com/questions/1196322/how-to-create-an-animated-gif-in-net
    1. 去 NuGet 裝上 NReco.VideoConverter
 */

namespace 影片檔轉GIF檔
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = System.IO.Directory.GetCurrentDirectory() + "\\";
            string sourceFilePath = rootPath + "input.mp4"; 
            string goalFilePath = rootPath + "outputMP4.gif";

            // 實作 影片檔 轉 GIF (測試成功)
            var ffmpeg = new NReco.VideoConverter.FFMpegConverter();
            ffmpeg.ConvertMedia(sourceFilePath, null, goalFilePath, null, new ConvertSettings());
            ffmpeg.ConvertMedia(rootPath + "input.wmv", null, rootPath + "outputWMV.gif", null, new ConvertSettings());
            ffmpeg.ConvertMedia(rootPath + "input.avi", null, rootPath + "outputAVI.gif", null, new ConvertSettings());

            // 合併影片檔 (測試成功)
            string file1 = rootPath + "1.mp4";
            string file2 = rootPath + "2.mp4";
            string[] source = (new List<string> { file1, file2 }).ToArray();

            string goal = "result.mp4";
            ffmpeg.ConcatMedia(source, goal, NReco.VideoConverter.Format.mp4, new ConcatSettings());       

            // 因為我沒有 .mp4 的影片檔可以測 => 所以此篇 還沒測過喔

            Console.WriteLine("執行完成");
            Console.ReadLine();
        }
    }
}
