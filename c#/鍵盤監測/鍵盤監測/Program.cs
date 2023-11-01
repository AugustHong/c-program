using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;   //调用WINDOWS API函数时要用到
using Microsoft.Win32;  //寫入注冊表用
using System.Windows.Forms;
using System.Runtime.CompilerServices;
using System.Drawing;

/*
    參考網址： https://www.cnblogs.com/ayqy/p/3636427.html
    先去 參考/右鍵/加入參考/組件/System.Windows.Forms 和 System.Drawing
 */

namespace 鍵盤監測
{
    // 繼承Form 是不要像 Console 一直卡住
    public class Program : Form
    {
        KeyboardHook k_hook;
        List<int> keyTmpList = new List<int>(); // 因為長壓會一直觸發，想要他真的是接2遍。所以就放入陣列裡，等到Up才取消

        // 釋放控制台
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool FreeConsole();

        // 啟動程式
        [STAThread]
        static void Main(string[] args)
        {
            // 直接啟動
            Application.Run(new Program());
        }

        public Program()
        {
            // 這邊看你要不要隱藏控制台(因為我是用 Console 建立，但 去繼承 Form 來呈現 WinForm的感覺)
            // 我這邊是測試用所以就沒隱藏Console (因為我要看結果)
            //FreeConsole();

            // 設定表單
            // 目的： 不要讓表單出來 (因為會變成有 Console 和 表單 同時出現在螢幕上)
            this.Text = "鍵盤監測";
            this.Size = new Size(0, 0);  // 會剩下上面的工具列
            this.Visible = false;  // 沒啥作用
            this.ShowInTaskbar = false;  // 不要出現在工具列中(表單)
            this.WindowState = FormWindowState.Minimized;  // 設定為最小化
            this.Hide();  // 設定自己(Form 隱藏) => 沒啥作用

            //安装鍵盤Hook
            k_hook = new KeyboardHook();
            k_hook.KeyDownEvent += new KeyEventHandler(hook_KeyDown);//Key Down
            k_hook.KeyUpEvent += new KeyEventHandler(hook_KeyUp);//Key Up
            k_hook.Start();//開始動作
        }

        // 實際 Hook 要執行的動作 (可以把想要處理的寫在這) ，按下時的動作
        public void hook_KeyDown(object sender, KeyEventArgs e)
        {
            bool isUse = false;  // 是否己被觸發 (用於判斷不要重複被觸發到)

            // 一直長壓A，會一直觸發A。 (故寫了1個 KeyUpEvent 來處理)
            // 若按鍵在 陣列中 -> 代表還沒觸發過 KeyUp -> 代表一直壓著
            // 並且這樣就可以寫快捷鍵了 (例如 A + D) 因為都會長壓著 A
            if (keyTmpList.Where(x => x == e.KeyValue).Count() <= 0)
            {
                // 先加入陣列中
                keyTmpList.Add(e.KeyValue);

                // 後面才是主邏輯
                Console.Write(e.KeyCode + "： ");  // 得到 A, B, LControlKey 這種文字

                // 如果長壓著A，而接下來的是D
                if (keyTmpList.Contains((int)Keys.A) && e.KeyValue == (int)Keys.D && isUse == false)
                {
                    Console.WriteLine("輸入快捷鍵 A + D");
                    isUse = true;
                }

                if (e.KeyValue == (int)Keys.LControlKey && isUse == false)
                {
                    Console.WriteLine("按下了 左CTRL");
                    isUse = true;
                }

                if (e.KeyValue == (int)Keys.RControlKey && isUse == false)
                {
                    Console.WriteLine("按下了 右CTRL");
                    isUse = true;
                }

                if (e.KeyValue == (int)Keys.F1 && isUse == false)
                {
                    Console.WriteLine("按下了 F1");
                    isUse = true;
                }

                //判断按下的键（Alt + A）
                if (e.KeyValue == (int)Keys.A && (int)Control.ModifierKeys == (int)Keys.Alt && isUse == false)
                {
                    Console.WriteLine("按下了指定快捷鍵組合 （Alt + A）");
                    isUse = true;
                }

                if (e.KeyValue == (int)Keys.A && isUse == false)
                {
                    Console.WriteLine("按下了 A");
                    isUse = true;
                }

                if (e.KeyValue == (int)Keys.B && isUse == false)
                {
                    Console.WriteLine("按下了 B");
                    isUse = true;
                }

                if (e.KeyValue == (int)Keys.C && isUse == false)
                {
                    Console.WriteLine("按下了 C");
                    isUse = true;

                    // 結束
                    k_hook.Stop();
                    // 關閉程式
                    this.Close();
                }
            }      
        }

        // Key Up 主要是為了解決長壓的問題
        public void hook_KeyUp(object sender, KeyEventArgs e)
        {
            if (keyTmpList.Where(x => x == e.KeyValue).Count() > 0)
            {
                // 將暫存的刪掉
                keyTmpList.Remove(e.KeyValue);
            }
        }
    }
}
