using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SpeechLib;

/*
	參考網址：
      https://einboch.pixnet.net/blog/post/267347720

      請去 右鍵/加入參考/COM/Microsoft Speech Object Library (有2個，選最下面的即可)
 */

namespace 輸入文字轉語音2
{
	class Program
	{
		static void Main(string[] args)
		{
			// 如果 引用 SpVoiceClass 出現  "無法內嵌 interop 型別 請改用適當的介面" 這個錯誤
			// 請去 參考/SpeechLib/右鍵/屬性/將 內嵌 interop類型 改為 False
			SpVoiceClass voice = new SpVoiceClass();
			voice.Voice = voice.GetVoices(string.Empty, string.Empty).Item(1);//Item(1)男聲
			voice.Speak("English Man 888", SpeechVoiceSpeakFlags.SVSFlagsAsync);

			System.Threading.Thread.Sleep(3000);
			voice.Voice = voice.GetVoices(string.Empty, string.Empty).Item(2);//Item(2)女聲
			voice.Speak("English woman", SpeechVoiceSpeakFlags.SVSFDefault);

			// 中文的部份一樣要裝東西才能實作，不然都不會說話
			System.Threading.Thread.Sleep(3000);
			voice.Voice = voice.GetVoices(string.Empty, string.Empty).Item(0);//Item(0)中文女聲
			voice.Speak("請說中文 Please", SpeechVoiceSpeakFlags.SVSFDefault);

			Console.Read();
		}
	}
}
