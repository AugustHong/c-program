using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using NAudio.Wave.SampleProviders;
using System.Threading;

/*
    參考網址： https://topic.alibabacloud.com/tc/a/c--recording-and-pitch-modulation_1_31_30840512.html
               https://www.twblogs.net/a/5dba75b6bd9eee310da08c6a

    先去 NuGet 裝上 NAudio
 */

namespace 錄音實作2_變聲
{
      internal class Program
      {
            static void Main(string[] args)
            {
                  string fileName = "a.wav";

                  // 錄音
                  NAudioRecorderHelper nrh = new NAudioRecorderHelper();
			nrh.StartRec(fileName);
			Console.WriteLine("開始執行錄音");

			string t = Console.ReadLine();

			if (!string.IsNullOrEmpty(t))
			{
				nrh.StopRec();
			}

			/*
                        目前結論： 檔案有出來但都是沒聲音的。不確定是不是我測試的問題，用麥克風講也是
                   */

			// 變聲
			nrh.ChangeSound("test.mp3", "UP", true);
                  nrh.ChangeSound("test.wav", "DOWN", false);
                  Console.WriteLine("變聲完成");

                  // 變聲有成功，但一樣是存檔那邊是失敗的


                  Console.Read();
            }
      }

      public class NAudioRecorderHelper
      {
            public WaveIn waveSource = null;
            public WaveInEvent waveSourceE = null;
            public WaveFileWriter waveFile = null;

            public NAudioRecorderHelper(int hz = 16000, int bit = 16, int channels = 1)
            {
                  Init(hz, bit, channels);
            }

            /// <summary>
            /// 初始化
            /// </summary>
            /// <param name="hz"></param>
            /// <param name="bit"></param>
            /// <param name="channels"></param>
            public void Init(int hz = 16000, int bit = 16, int channels = 1)
		{
                  try
                  {
                        waveSource = new WaveIn();  // 背景程式在用的，如果是 Console 要用 WaveInEvent
                        waveSource.WaveFormat = new WaveFormat(hz, bit, channels); // 16KHz, 16bit ,Mono的錄音格式
                        waveSource.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
                        //waveSource.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);

                  }
                  catch
                  {
                        waveSourceE = new WaveInEvent();
                        waveSourceE.WaveFormat = new WaveFormat(hz, bit, channels); // 16KHz, 16bit ,Mono的錄音格式
                        waveSourceE.DataAvailable += new EventHandler<WaveInEventArgs>(waveSource_DataAvailable);
                        //waveSourceE.RecordingStopped += new EventHandler<StoppedEventArgs>(waveSource_RecordingStopped);
                  }
            }

            /// <summary>
            /// 開始錄音
            /// </summary>
            /// <param name="outputFilePath">輸出檔案(限MAV檔)</param>
            public void StartRec(string outputFilePath)
            {
                  if (waveSource != null)
                  {
                        waveFile = new WaveFileWriter(outputFilePath, waveSource.WaveFormat);
                        waveSource.StartRecording();
                  }
                  else
                  {
                        if (waveSourceE != null)
                        {
                              waveFile = new WaveFileWriter(outputFilePath, waveSourceE.WaveFormat);
                              waveSourceE.StartRecording();
                        }
                        else
                        {
                              throw new Exception("沒有成功建立的錄音");
                        }
                  }
            }


            /// <summary>
            /// 停止錄音
            /// </summary>
            public void StopRec()
            {
                  if (waveSource != null)
                  {
                        waveSource.StopRecording();
                        waveSource.Dispose();
                        waveSource = null;
                  }

                  if (waveSourceE != null)
                  {
                        waveSourceE.StopRecording();
                        waveSourceE.Dispose();
                        waveSourceE = null;
                  }

                  if (waveFile != null)
                  {
                        waveFile.Dispose();
                        waveFile = null;
                  }
            }


            /// <summary>
            /// 錄音執行動作 (寫入檔案)
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void waveSource_DataAvailable(object sender, WaveInEventArgs e)
            {
                  if (waveFile != null)
                  {
                        waveFile.Write(e.Buffer, 0, e.BytesRecorded);
                        waveFile.Flush();
                  }
            }


            /// <summary>
            /// 錄音結束動作
            /// </summary>
            /// <param name="sender"></param>
            /// <param name="e"></param>
            private void waveSource_RecordingStopped(object sender, StoppedEventArgs e)
            {
                  if (waveSource != null)
                  {
                        waveSource.Dispose();
                        waveSource = null;
                  }

                  if (waveSourceE != null)
                  {
                        waveSourceE.Dispose();
                        waveSourceE = null;
                  }

                  if (waveFile != null)
                  {
                        waveFile.Dispose();
                        waveFile = null;
                  }
            }

            /// <summary>
            /// 變聲並播放
            /// </summary>
            /// <param name="filePath">原檔</param>
            /// <param name="upOrDown">UP (上升Key) or DOWN (下降Key)</param>
            /// <param name="saveFile">是否存檔(預設 True，存的檔名為 原檔_{UP or DOWN})</param>
            public void ChangeSound(string filePath, string upOrDown = "UP", bool saveFile = true)
            {
                  // 取得要上升還下降
                  string ud = upOrDown.ToUpper();

                  string fileExtension = Path.GetExtension(filePath);
                  string newFileName = filePath.Replace(fileExtension, "") + "_" +  ud + ".wav";

                  var semitone = Math.Pow(2, 1.0 / 12);
                  var upOneTone = semitone * semitone;
                  var downOneTone = 1.0 / upOneTone;
                  using (var reader = new MediaFoundationReader(filePath))
                  {
                        var pitch = new SmbPitchShiftingSampleProvider(reader.ToSampleProvider());
                        using (var device = new WaveOutEvent())
                        {
                              pitch.PitchFactor = (float)((ud == "UP" ? upOneTone : downOneTone) * 2.0f); // or downOneTone

					if (saveFile)
					{
                                    Init();
                                    StartRec(newFileName);
					}

					device.Init(pitch);
                              device.Play();

                              while (device.PlaybackState == PlaybackState.Playing)
                              {
                                    Thread.Sleep(500);
                              }

					if (saveFile)
					{
						StopRec();
					}
				}
                  }
            }
      }
}
