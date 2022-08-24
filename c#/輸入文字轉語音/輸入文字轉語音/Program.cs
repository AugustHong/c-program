using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Speech.Synthesis;

/*
	參考網址： https://www.howcando.net/ask/79734.html
      參考右鍵/加入參考/找到 System.Speech
 */

namespace 輸入文字轉語音
{
	class Program
	{
		static void Main(string[] args)
		{
			string readText = string.Empty;

			// 只能 英數字而己 (中文好像要裝什麼包)
			SpeechSynthesizer reader = new SpeechSynthesizer();
			reader.Volume = 100;  // 0...100


			while (readText.ToUpper() != "EXIT")
			{
				Console.WriteLine("請輸入文字：");
				readText = Console.ReadLine();

				if (!string.IsNullOrWhiteSpace(readText))
				{
					reader.SpeakAsync(readText);
				}
			}
		}
	}
}
