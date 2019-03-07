using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageToBinary
{
	/// <summary>
	/// 這邊是For 一些 MVC的部份 => 所以建置下一定會失敗
	/// 這部份因為連建置都不行，故都是沒測試的
	/// 只是大概寫的方向，要用的時候記得要再測一下喔
	/// </summary>
	public class Program
	{
		public HttpPostedFileBase Picture { get; set; }

		static void Main(string[] args)
		{
			/*將得到的圖片轉成二進制存起來*/

			/*方法一、 使用HttpPostedFileBase 的類型 接收 Web 上使用者上傳的圖片*/
			byte[] fileData = null;
			using (var binaryReader = new BinaryReader(Pictrue.InputStream))
			{
				fileData = binaryReader.ReadBytes(Pictrue.ContentLength);
			}

			//-----------------------------------------------------------------------------------------------------------------------------------------

			/*方法二、 使用 讀檔的方式來讀Server上的圖片檔*/
			byte[] fileData2 = null;

			FileStream myFile = File.Open(@"Test.jpg", FileMode.Open, FileAccess.ReadWrite);

			//引用myReader類別
			BinaryReader myReader = new BinaryReader(myFile);

			//取得長度（因為是byte，所以要看長度）
			int len = Convert.ToInt32(myFile.Length);

			//讀取位元陣列
			fileData2 = myReader.ReadBytes(len);

			//----------------------如此一來就可以存進資料庫了(因為圖片已經是二進制檔了)----------------------------------

			//---------------------接下來要把圖片拿出來 ， 用MVC來看的話就是用 View的部份

			//MVC的部份
			/*
			 
				View的部份：(呼叫Controller)

				<img style="width: 100%;" class="img" src='@Url.Action("GetImage", "Image") />
			 
				Controller的部份(把剛才拿到的二進制資料，轉成檔案)：

				[HttpGet]
				public ActionResult GetImage()
				{
					byte[] image = 得到的圖片二進制資料
					if (image != null)
					{
						//請看 C#/FromServerDownloadFile  中裡面有介紹到這隻
						return File(image, "image/jpeg");
					}

					return Content("")
				 }
			 */

			/*轉成Image
			System.IO.MemoryStream ms = new System.IO.MemoryStream(剛才得到的二進制資料);
			System.Drawing.Image img = System.Drawing.Image.FromStream(ms);
			*/
		}
	}
}
