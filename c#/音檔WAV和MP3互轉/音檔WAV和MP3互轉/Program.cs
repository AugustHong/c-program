using NAudio.MediaFoundation;
using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址： https://blog.csdn.net/yunxiaobaobei/article/details/107017022
    先去 NuGet 裝上 NAudio
 */

namespace 音檔WAV和MP3互轉
{
    internal class Program
    {
        static void Main(string[] args)
        {
            WavToMp3("test.wav", "test_c.mp3");
            Console.WriteLine("WAV TO MP3 成功");

            Mp3ToWav("test_c.mp3", "test_c.wav");
            Console.WriteLine("MP3 TO WAV 成功");

            Console.Read();
        }

        /// <summary>
        /// WAV檔轉MP3
        /// </summary>
        /// <param name="sourcePath">來源檔案</param>
        /// <param name="goalPath">轉出來的檔名</param>
        public static void WavToMp3(string sourcePath, string goalPath)
        {
            MediaFoundationApi.Startup();
            using (WaveFileReader r = new WaveFileReader(sourcePath)) 
            {
                MediaFoundationEncoder.EncodeToMp3(r, goalPath);
            }
        }

        /// <summary>
        /// Mp3 轉 Wav
        /// </summary>
        /// <param name="sourcePath">來源檔案</param>
        /// <param name="goalPath">轉出來的檔名</param>
        public static void Mp3ToWav(string sourcePath, string goalPath)
        {
            using(Mp3FileReader r = new Mp3FileReader(sourcePath))
            {
                WaveFileWriter.CreateWaveFile(goalPath, r);
            }
        }
    }
}
