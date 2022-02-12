using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
	參考網址： https://www.itread01.com/content/1547572868.html
 */

namespace 開啟圖片顯示器
{
	class Program
	{
		static void Main(string[] args)
		{
			// 取得當前目錄
			string path = System.IO.Directory.GetCurrentDirectory() + "\\";
			string filename = "test.png";
			//建立新的系統程序  
			System.Diagnostics.Process process = new System.Diagnostics.Process();
			//設定檔名，此處為圖片的真實路徑+檔名  
			process.StartInfo.FileName = path + "Img\\" + filename;
			//此為關鍵部分。設定程序執行引數，此時為最大化視窗顯示圖片。  
			process.StartInfo.Arguments = "rundll32.exe C://WINDOWS//system32//shimgvw.dll,ImageView_Fullscreen";
			//此項為是否使用Shell執行程式，因系統預設為true，此項也可不設，但若設定必須為true  
			process.StartInfo.UseShellExecute = true;
			//此處可以更改程序所開啟窗體的顯示樣式，可以不設  
			process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
			process.Start();
			process.Close();
		}
	}
}
