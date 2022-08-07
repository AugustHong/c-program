using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

/*
	參考網址： https://dotblogs.com.tw/chou/2009/03/08/7410
      (1) 右鍵/加入參考/System.Drawing 和 System.Windows.Forms
 */

namespace 螢幕截圖
{
	class Program
	{
		static void Main(string[] args)
		{
			string rootPath = System.IO.Directory.GetCurrentDirectory() + "\\";

			ScreenShot screenShoot = new ScreenShot();
			screenShoot.Shot(rootPath + "\\Output\\test.jpg");

			// 錄影形式
			screenShoot.ContinuousShot(2, rootPath + "\\Output\\Movie\\MovieTest_{{N}}.jpg", 5);

			Console.WriteLine("結束測試");
			Console.Read();
		}
	}

	// 螢幕截圖
	public class ScreenShot
	{
		// 起始點 (左上角)
		public int StartPointX { get; set; }
		public int StartPointY { get; set; }

		// 圖片大小
		public int PicHeight { get; set; }
		public int PicWidth { get; set; }


		public ScreenShot()
		{
			// 預設螢幕截圖
			this.StartPointX = 0;
			this.StartPointY = 0;
			this.PicHeight = Screen.PrimaryScreen.Bounds.Height;
			this.PicWidth = Screen.PrimaryScreen.Bounds.Width;
		}

		public ScreenShot(int StartPointX, int StartPointY, int PicHeight, int PicWidth)
		{
			this.StartPointX = StartPointX;
			this.StartPointY = StartPointY;
			this.PicHeight = PicHeight;
			this.PicWidth = PicWidth;
		}

		// 螢幕截圖
		public void Shot(string fileName)
		{
			Bitmap myImage = new Bitmap(PicWidth, PicHeight);
			Graphics g = Graphics.FromImage(myImage);
			g.CopyFromScreen(new Point(StartPointX, StartPointY), new Point(0, 0), new Size(PicWidth, PicHeight)); //螢幕截圖主要
			IntPtr dc1 = g.GetHdc();
			g.ReleaseHdc(dc1);

			if (File.Exists(fileName))
			{
				File.Delete(fileName);
			}

			myImage.Save(fileName);
		}

		/// <summary>
		/// 做成像螢幕錄影一樣，只是轉成好幾個圖片檔
		/// </summary>
		/// <param name="fps">fps就是幀數 (每秒顯示影格數) => 所以在這邊就是每秒呈現多少 圖片 (圖會會很大量喔~)</param>
		/// <param name="fileName">輸入檔名 (裡面加上 {{N}}就是序號， 例如： ABC_{{N}}.jpg)</param>
		/// <param name="runTime">錄幾秒</param>
		public void ContinuousShot(int fps, string fileName, int runTime)
		{
			// 1秒要產出幾張
			int sleepTime = (int)((1000) / fps);
			int index = 1;
			int startTime = DateTime.Now.Year * 24 + DateTime.Now.Minute * 60 + DateTime.Now.Second;
			int endTime = startTime + runTime;

			while((DateTime.Now.Year * 24 + DateTime.Now.Minute * 60 + DateTime.Now.Second) <= endTime)
			{
				string indexStr = (index).ToString().PadLeft(8, '0');
				string newFileName = fileName.Replace("{{N}}", indexStr);
				index++;

				// 螢幕截圖
				this.Shot(newFileName);

				// 停下來時間
				System.Threading.Thread.Sleep(sleepTime);
			}
		}
	}
}
