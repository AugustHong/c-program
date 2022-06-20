using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

/*
	參考網址：https://www.796t.com/content/1549138537.html 、 https://blog.csdn.net/u012577474/article/details/90764888
 */

namespace 圖片上新增圖片和文字
{
	public class PicAddHelper
	{
		public Graphics backgroundPic = null;
		public Bitmap backgroundB = null;

		public PicAddHelper(string backgroundFileName)
		{
			if (File.Exists(backgroundFileName))
			{
				Image image = Image.FromFile(backgroundFileName);
				backgroundB = new Bitmap(image, image.Width, image.Height);
				backgroundPic = Graphics.FromImage(backgroundB);
			}
		}

		/// <summary>
		///  儲存
		/// </summary>
		/// <param name="goalFileName">儲存新檔名</param>
		/// <param name="format">儲存格式(Png、Jpeg)，預設Png</param>
		/// <param name="isRealse">是否釋放資料(預設是)</param>
		/// <returns></returns>
		public string Save(string goalFileName, System.Drawing.Imaging.ImageFormat format = null, bool isRealse = true)
		{
			string errMsg = "";

			if (format == null)
			{
				format = System.Drawing.Imaging.ImageFormat.Png;
			}

			if (backgroundPic == null)
			{
				errMsg = "無底圖";
			}
			else
			{
				try
				{
					backgroundB.Save(goalFileName, format);

					if (isRealse)
					{
						backgroundPic.Dispose();
						backgroundB.Dispose();
					}
				}
				catch (Exception ex)
				{
					errMsg = ex.Message;
				}
			}

			return errMsg;
		}

		/// <summary>
		/// 加入圖片
		/// </summary>
		/// <param name="picFileName">加入的圖片</param>
		/// <param name="x">圖片要放的位置X軸</param>
		/// <param name="y">圖片要放的位置Y軸</param>
		/// <returns></returns>
		public string AddPic(string picFileName, int x, int y)
		{
			string errMsg = string.Empty;

			if (File.Exists(picFileName))
			{
				if (backgroundPic == null)
				{
					errMsg = "無底圖";
				}
				else
				{
					Image image = Image.FromFile(picFileName);
					backgroundPic.DrawImage(image, x, y, image.Width, image.Height);
				}
			}
			else
			{
				errMsg = "無此加入圖片的路徑檔案";
			}

			return errMsg;
		}

		/// <summary>
		/// 加入文字
		/// </summary>
		/// <param name="text">訊息內容</param>
		/// <param name="x">文字要放的位置X軸</param>
		/// <param name="y">文字要放的位置Y軸</param>
		/// <param name="fontSize">字體大小</param>
		/// <param name="font">字型(給null視同 新細明體)</param>
		/// <param name="fontColor">文字顏色(給null視同黑色)</param>
		/// <param name="backgroundColor">背景顏色(沒有請給null)</param>
		/// <returns></returns>
		public string AddText(string text, int x, int y, int fontSize, Font font, Color? fontColor, Color? backgroundColor)
		{
			string errMsg = string.Empty;

			if (!string.IsNullOrWhiteSpace(text))
			{
				if (backgroundPic == null)
				{
					errMsg = "無底圖";
				}
				else
				{
					if (font == null)
					{
						font = new Font("新細明體", fontSize, FontStyle.Regular);
					}

					if (fontColor == null)
					{
						fontColor = Color.Black;
					}

					int range = 20;
					float rectWidth = text.Length * (fontSize + range);
					float rectHeight = fontSize + range;

					//宣告矩形域
					RectangleF textArea = new RectangleF(x, y, rectWidth, rectHeight);

					if (backgroundColor != null)
					{
						Brush backgroundBrush = new SolidBrush((Color)backgroundColor);
						backgroundPic.FillRectangle(backgroundBrush, x, y, rectWidth, rectHeight);
					}

					Brush textBrush = new SolidBrush((Color)fontColor);
					backgroundPic.DrawString(text, font, textBrush, textArea);
				}
			}
			else
			{
				errMsg = "文字請必須輸入";
			}

			return errMsg;
		}
	}
}
