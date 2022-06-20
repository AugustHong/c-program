using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 模擬按鍵精靈
{
	class Program
	{
		static void Main(string[] args)
		{
			MouseKeyBoardHelper helper = new MouseKeyBoardHelper();

			//單鍵
			helper.keybd("CapsLock");
			Console.WriteLine("單鍵");
			System.Threading.Thread.Sleep(2000);

			//組合鍵
			//工作管理員 Ctrl+ESC
			List<string> s = new List<string> { "LControl", "Escape" };
			helper.keybd(s);
			Console.WriteLine("組合鍵");
			System.Threading.Thread.Sleep(2000);

			//滑鼠相對移動
			helper.mouse_move_r(100, 0);
			Console.WriteLine("相對移動");
			System.Threading.Thread.Sleep(2000);

			//滑鼠移動到指定位置
			helper.mouse_move(100, 100);
			Console.WriteLine("絕對移動");
			System.Threading.Thread.Sleep(2000);

			//滑鼠 右鍵
			helper.MouseRightClick();
			Console.WriteLine("右鍵");
			System.Threading.Thread.Sleep(2000);

			//滑鼠 左鍵 雙擊
			Console.WriteLine("雙擊左鍵");
			helper.MouseDoubleLeftClick();
		}
	}
}
