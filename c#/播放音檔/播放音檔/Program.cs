using System;
using System.Collections.Generic;
using System.Linq;
using System.Media;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using WMPLib;

/*
    參考網址： https://blog.kkbruce.net/2019/03/csharpformusicplay.html
    
    第4種方式需裝 WMPLib.dll
    注意：WMPLib.dll 的路徑在不同作業系統版本裡，似乎路徑不太一樣，
    例如： C:\Program Files\Microsoft Visual Studio\2022\Community\Common7\IDE\PrivateAssemblies
    基本上直接  參考/右鍵/瀏覽/預設進到的目錄 就是 IDE 那層了，直接往下找 PrivateAssemblies 即可
 */

namespace 播放音檔
{
    public enum Music
    {
        Do = 523,
        Re = 587,
        Mi = 659,
        Fa = 698,
        So = 784,
        La = 880,
        Ti = 988,
        Do2 = 1046
    }


    internal class Program
    {
        // 第一種方式要用的 
        [DllImport("kernel32.dll")]
        private static extern int Beep(int dwFreq, int dwDuration);


        static void Main(string[] args)
        {
            #region 第一種方式

            // 音效會一直斷斷續續的，不建議使用
            Beep((int)Music.Do, 300);
            Beep((int)Music.Do, 300);
            Beep((int)Music.So, 300);
            Beep((int)Music.So, 300);
            Beep((int)Music.La, 300);
            Beep((int)Music.La, 300);
            Beep((int)Music.So, 600);

            #endregion


            #region 第二種方式 (單純播放系統音樂而己)

            //系統音效
            SystemSounds.Asterisk.Play();
            SystemSounds.Beep.Play();
            SystemSounds.Exclamation.Play();
            SystemSounds.Hand.Play();
            SystemSounds.Question.Play();

            #endregion


            #region 第三種方式 (播放 MAV檔)

            // 建議使用此方法，除非要非MAV檔的
            SoundPlayer player = new SoundPlayer();
            //player.SoundLocation = "http://billor.chsh.chc.edu.tw/sound/ambul.wav";  //可放網址，也可放檔案
            //player.Load();   // 也可用 player.LoadAsync()
            //player.Play();   // 也可用 player.PlaySync()
            //player.PlayLooping();  //循環播放

            player.SoundLocation = "test.wav";
            player.LoadAsync();
            player.PlaySync();

            #endregion


            #region 第四種方式(可播MP3檔，但需要有 Windows Media Player。且 需要引用 WMPLib.dll)

            WindowsMediaPlayerHelper ph = new WindowsMediaPlayerHelper();
            ph.Play("test.mp3");

            #endregion

            Console.WriteLine("播放結束");
            Console.ReadLine();
        }
    }


    // 第四種法式寫成Helper比較好實作
    public class WindowsMediaPlayerHelper
    {
        WindowsMediaPlayer Player;

        public WindowsMediaPlayerHelper()
        {
            Player = new WindowsMediaPlayer();
            Player.PlayStateChange += new _WMPOCXEvents_PlayStateChangeEventHandler(Player_PlayStateChange);
            Player.MediaError += new _WMPOCXEvents_MediaErrorEventHandler(Player_MediaError);
        }

        /// <summary>
        /// 播放
        /// </summary>
        /// <param name="url">檔案位置(可Url或路徑)</param>
        public void Play(string url)
        {      
            Player.URL = url;
            Player.controls.play();
        }

        /// <summary>
        /// 暫停
        /// </summary>
        public void Stop()
        {
            Console.WriteLine("主動暫停");
            Player.close();
        }

        /// <summary>
        /// 取得當前媒體
        /// </summary>
        /// <returns></returns>
        public IWMPMedia GetCurrentMedia()
        {
            return Player.currentMedia;
        }

        /// <summary>
        /// 取得當前播放清單
        /// </summary>
        /// <returns></returns>
        public List<IWMPMedia> GetCurrentPlayList()
        {
            List<IWMPMedia> result = new List<IWMPMedia>();

            for (int i = 0; i < Player.currentPlaylist.count; i++)
            {
                result.Add(Player.currentPlaylist.Item[i]);
            }

            return result;
        }

        /// <summary>
        /// 播放狀態變更
        /// </summary>
        /// <param name="NewState"></param>
        private void Player_PlayStateChange(int NewState)
        {
            var state = (WMPPlayState)NewState;
            //state.Dump();

            if (state == WMPPlayState.wmppsStopped)
            {
                Console.WriteLine("Stop");
                Player.close();
            }
        }

        /// <summary>
        /// 播放發生錯誤
        /// </summary>
        /// <param name="pMediaObject"></param>
        private void Player_MediaError(object pMediaObject)
        {
            Console.WriteLine("Error");
            Player.close();
        }
    }
}
