using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 圖片背景白色變透明_類去背_
{
	class Program
	{
		static void Main(string[] args)
		{
			string inputFileName = "a.png";
			Image input = Image.FromFile(@"" +inputFileName);
			Bitmap tmpB = new Bitmap(input);
			tmpB.MakeTransparent(Color.White);  //變更背景色白色為透明
			tmpB.Save("a_result.png");

			Console.WriteLine("--------------------轉換完成--------------------");
			Console.ReadLine();

			// 目前實測是可行，但會變成有點糊糊的部份
		}
	}
}
