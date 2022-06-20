using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 圖片上新增圖片和文字
{
	class Program
	{
		static void Main(string[] args)
		{
			PicAddHelper helper = new PicAddHelper("background.png");
			helper.AddPic("insert.png", 300, 500);
			helper.AddText("你好啊！", 800, 100, 36, null, null, null);
			helper.Save("result.png");
		}
	}
}
