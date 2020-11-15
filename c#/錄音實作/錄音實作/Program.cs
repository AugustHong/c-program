using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

/*
    參考網圵： https://www.itdaan.com/tw/e7d5e5f305ed
              https://www.itread01.com/content/1548707061.html
 */

namespace 錄音實作
{
    class Program
    {
        static void Main(string[] args)
        {
            RecordMusicHelper helper = new RecordMusicHelper();
            helper.RecordStart();
            System.Threading.Thread.Sleep(10000);
            helper.RecordStop("output");

            Console.WriteLine("錄製完成");
            Console.Read();
        }
    }

    /// <summary>
    ///  錄音的 Helper
    /// </summary>
    public class RecordMusicHelper
    {
        // 最主要的 東西
        [DllImport("winmm.dll", EntryPoint = "mciSendString", CharSet = CharSet.Auto)]
        public static extern int mciSendString(
            string lpstrCommand,
            string lpstrReturnString,
            int uReturnLength,
            int hwndCallback
        );

        /// <summary>
        ///  開始錄音
        /// </summary>
        /// <param name="bit"></param>
        /// <param name="spersec"></param>
        /// <param name="channel"></param>
        public void RecordStart(int bit = 8, int spersec = 20000, int channel = 2)
        {
            mciSendString($"set wave bitpersample {bit}", "", 0, 0);
            mciSendString($"set wave samplespersec {spersec}", "", 0, 0);
            mciSendString($"set wave channels {channel}", "", 0, 0);

            // 設定 format
            mciSendString($"set wave format tag pcm", "", 0, 0);
            mciSendString($"open new type WAVEAudio alias movie", "", 0, 0);

            // 開始錄
            mciSendString($"record movie", "", 0, 0);
        }

        /// <summary>
        ///  停止錄音
        /// </summary>
        /// <param name="fileName">檔案名稱(不要加副檔名，預設是 .wav)</param>
        public void RecordStop(string fileName)
        {
            string rootPath = Directory.GetCurrentDirectory() + "\\";
            if (File.Exists(rootPath + fileName))
            {
                File.Delete(rootPath + fileName);
            }

            mciSendString("stop movie", "", 0, 0);
            mciSendString($"save movie {fileName}.wav", "", 0, 0);
            mciSendString("close movie", "", 0, 0);
        }
    }
}
