using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址： https://topic.alibabacloud.com/tc/a/c--implement-screen-lock-and-prohibit-keyboard-and-mouse_1_31_32383300.html
 */

namespace 鎖住螢幕_滑鼠_鍵盤
{
    internal class Program
    {
        [DllImport("user32.dll")]
        public static extern bool LockWorkStation();//這個是調用windows的系統鎖定

        [DllImport("user32.dll")]
        static extern void BlockInput(bool Block); // 鎖住滑鼠、鍵盤

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);  //移動滑鼠

        static void Main(string[] args)
        {
            // 目前看起來滑鼠、鍵盤 還是能動，並無法真的鎖住
            // 實測： 是要用 "系統管理員" 身份執行才會啟用
            Console.WriteLine("鎖住滑鼠、鍵盤。7秒後解鎖");
            SetCursorPos(0, 0);  // 移動滑鼠
            BlockInput(true);
            System.Threading.Thread.Sleep(7000);
            BlockInput(false);
            Console.WriteLine("滑鼠、鍵盤 解除鎖定");

            // 鎖螢幕是可正常使用的
            Console.WriteLine("2秒後鎖螢幕");
            System.Threading.Thread.Sleep(2000);
            LockWorkStation();  // 鎖螢幕

            Console.ReadLine();
        }
    }
}
