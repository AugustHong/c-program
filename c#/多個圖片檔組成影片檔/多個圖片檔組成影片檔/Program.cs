using Splicer.Renderer;
using Splicer.Timeline;
using Splicer.WindowsMedia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    參考網圵： https://stackoverflow.com/questions/251467/how-can-i-create-a-video-from-a-directory-of-images-in-c
    1. 去 NuGet 裝上 Splicer
    2. 加入參考/ System.Drawing
 */

namespace 多個圖片檔組成影片檔
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = System.IO.Directory.GetCurrentDirectory() + "\\";
            string imgPath = rootPath + "img\\";
            string outputPath = rootPath + "output\\";

            // 目前 檔案要是 jpg => 我有特別處理
            string sourceFile = imgPath + "source.png";
            string sourceFile1 = imgPath + "source1.jpg";
            string sourceFile2 = imgPath + "source2.jpg";
            string sourceFile3 = imgPath + "source3.jpg";
            string sourceFile4 = imgPath + "source4.jpg";

            List<string> source = new List<string> { sourceFile, sourceFile1, sourceFile2, sourceFile3, sourceFile4 };
            MakeVideoHelper.MakeVideo(source, new List<string>(), outputPath + "input.wmv");
            MakeVideoHelper.MakeVideo(source, new List<string>(), outputPath + "input.mp4");
            MakeVideoHelper.MakeVideo(source, new List<string>(), outputPath + "input.avi");

            // halfDuration: 0 代表沒動畫 直接接著下一張圖 (上面的 會有個 變黑 再到下一張圖的動作)
            List<string> source1 = new List<string> { sourceFile, sourceFile1 };
            MakeVideoHelper.MakeVideo(source1, new List<string>(), outputPath + "1.wmv", halfDuration: 0);
            MakeVideoHelper.MakeVideo(source1, new List<string>(), outputPath + "1.mp4", halfDuration: 0);

            List<string> source2 = new List<string> { sourceFile2, sourceFile3, sourceFile4};
            MakeVideoHelper.MakeVideo(source2, new List<string>(), outputPath + "2.wmv", halfDuration: 0);
            MakeVideoHelper.MakeVideo(source2, new List<string>(), outputPath + "2.mp4", halfDuration: 0);

            Console.WriteLine("執行完成");
            Console.ReadLine();
        }
    }

    /// <summary>
    ///  這邊只做示範 => 之後自行調參數
    ///  目前只能 .wmv 檔
    /// </summary>
    public static class MakeVideoHelper
    {
        /// <summary>
        ///  產生影片檔
        /// </summary>
        /// <returns></returns>
        public static bool MakeVideo(List<string> sourceFilePathList, List<string> soundFilePathList, string goalFilePath, int timeLine = 25, double halfDuration = 0.5)
        {
            if (sourceFilePathList.Count() <= 0)
            {
                return false;
            }

            try
            {
                if (File.Exists(goalFilePath))
                {
                    File.Delete(goalFilePath);
                }

                using (ITimeline timeline = new DefaultTimeline(timeLine))
                {
                    // 創立一個 group
                    IGroup group = timeline.AddVideoGroup(32, 160, 100);

                    // 加入圖片
                    ITrack videoTrack = group.AddTrack();
                    AddImage(group, videoTrack, sourceFilePathList, halfDuration: halfDuration);

                    // 加入聲音檔
                    ITrack audioTrack = timeline.AddAudioGroup().AddTrack();
                    AddAudio(audioTrack, soundFilePathList, 0, videoTrack.Duration);
                    
                    // 影片檔
                    using (
                        WindowsMediaRenderer renderer =
                            new WindowsMediaRenderer(timeline, goalFilePath, WindowsMediaProfiles.HighQualityVideo))
                    {
                        renderer.Render();
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        ///  Group 加入圖片
        /// </summary>
        /// <param name="group"></param>
        /// <param name="track"></param>
        /// <param name="fileName">完整路徑</param>
        /// <param name="offset"></param>
        /// <param name="clipEnd"></param>
        /// <param name="halfDuration"></param>
        private static void AddImage(IGroup group, ITrack track, List<string> fileNameList, double offset = 0, double clipEnd = 2, double halfDuration = 0.5)
        {
            List<IClip> clipList = new List<IClip>();
            // 要先把全部圖片加載上去，才能執行下面的程式
            foreach (var fileName in fileNameList)
            {
                // 因為 他只能接受 jpg => 所以做特別處理
                string file = ChangeImageType(fileName);

                if (!string.IsNullOrEmpty(file))
                {
                    // 這裡 clipEnd 是維持幾個影格(1個1秒吧，我猜的)
                    // => 所以像這邊預設的 clipEnd = 2 -> 一張圖就是2秒了(看結果看到的)
                    IClip clip = track.AddImage(file, offset, clipEnd);
                    clipList.Add(clip);
                }
            }

            // 加入 動畫 (第一筆不能加)
            bool isfirst = true;
            foreach (var clip in clipList)
            {
                if (isfirst == false)
                {
                    // 動畫的黑畫面 (把 halfDuration 設為 0  就可以接了)
                    group.AddTransition(clip.Offset - halfDuration, halfDuration, StandardTransitions.CreateFade(), true);
                    group.AddTransition(clip.Offset, halfDuration, StandardTransitions.CreateFade(), false);
                }

                isfirst = false;
            }
        }

        /// <summary>
        ///  加入聲檔
        /// </summary>
        /// <param name="track"></param>
        /// <param name="fileName"></param>
        /// <param name="offset"></param>
        /// <param name="Duration"></param>
        private static void AddAudio(ITrack track, List<string> fileNameList, double offset = 0, double Duration = 0.5)
        {
            List<IClip> audioList = new List<IClip>();

            // 先載入聲音檔 ，再執行下面的程式
            foreach (var fileName in fileNameList)
            {
                IClip audio = track.AddAudio(fileName, 0, Duration);
                audioList.Add(audio);
            }

            // 加入 效果
            foreach (var audio in audioList)
            {
                track.AddEffect(0, audio.Duration, StandardEffects.CreateAudioEnvelope(1.0, 1.0, 1.0, audio.Duration));
            }           
        }

        /// <summary>
        ///  檔案全名
        /// </summary>
        /// <param name="filePathName"></param>
        /// <returns></returns>
        public static string ChangeImageType(string filePathName)
        {
            try
            {
                string result = string.Empty;

                List<string> tmp = filePathName.Split('.').ToList();
                string subName = tmp.LastOrDefault();  // 副檔名
                string tmpSubName = subName.ToUpper();

                if (tmpSubName != "JPG" && tmpSubName != "JPEG")
                {
                    tmp.Remove(subName);
                    result = string.Join(".", tmp) + ".jpg";

                    if (!string.IsNullOrEmpty(result))
                    {
                        if (!File.Exists(result))
                        {
                            // 要把檔案複製改名
                            File.Copy(filePathName, result, true);
                        }
                    }
                }
                else
                {
                    return filePathName;
                }

                return result;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return string.Empty;
            }
        }
    }
}
