using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;
using System.Runtime.InteropServices;

/*
    參考網圵：   https://zhidao.baidu.com/question/1494381313524869859.html
                https://docs.microsoft.com/en-us/dotnet/api/system.windows.forms.notifyicon.contextmenu?redirectedfrom=MSDN&view=netframework-4.8#System_Windows_Forms_NotifyIcon_ContextMenu

    1. 先去加入參考 System.Windows.Forms
    2. 加入 System.Drawing
*/

/*
    注： 如果只想要 Pop 出桌面提示 直接在 Main 寫即可， 但是如果要 Event + 右鍵菜單 (它好像一定要有個Form)
        所以才會寫成這個樣子。但這個Form一直隱藏不了。故請自行考慮，如果只是想要單純簡單的Pop桌面提示，是不用
        寫成這種樣子的。

    A. 如果只想要 Pop 出桌面提示：
        1. 開啟一個新的 Console
        2. 在Main 寫上 
            NotifyIcon n = new NotifyIcon();
            n.Visible = true;
            n.Icon = new Icon("1.ico", 120, 300);
            n.Text = "測試";
            n.ShowBalloonTip(5000, "測試", "哈囉", ToolTipIcon.None);

    這樣即可。

    B. 有 Event + 右鍵菜單的 ：
        1. 先讓 主 Class 繼承 Form
        2. 照現在的模式去刻 (一個 Main ， 然後裡面進入實作)
*/

namespace CSharp觸發桌面提示
{
    class Program : System.Windows.Forms.Form
    {
        // -----------一開始的進入點--------------------------------------------
        [STAThread]
        static void Main(string[] args)
        {
            Application.Run(new Program());
        }

        // ----------------主執行的地方---------------------------
        public Program()
        {
            // 目的： 不要讓表單出來 (因為會變成有 Console 和 表單 同時出現在螢幕上)
            this.Text = "NotifyIcon 簡單測試";
            this.Size = new Size(0, 0);  // 會剩下上面的工具列
            this.Visible = false;  // 沒啥作用
            this.ShowInTaskbar = false;  // 不要出現在工具列中(表單)
            this.WindowState = FormWindowState.Minimized;  // 設定為最小化
            this.Hide();  // 設定自己(Form 隱藏) => 沒啥作用

            // 開始建立 NotifyIcon
            NotifyIcon n = new NotifyIcon();
            n.Visible = true;

            // 一定要設置 ICON 不然會跑不出來
            // 目前設長寬 好像沒啥作用 => 大小都一樣小

            // 官方的圖示
            // n.Icon = new Icon(SystemIcons.Exclamation, 300, 300);

            // 自已的圖示
            // 1. 先把圖片存成正方形 16x16  32x32 並存成png
            // 2. 去 https://www.convertico.com/ 進行轉換
            // 3. 得到 .ico檔  放在這邊即可
            n.Icon = new Icon("1.ico", 120, 300);

            // PS：其實主體是 工具列上的小icon
            // 工具列上的小icon 滑在上面時顯示的文字
            n.Text = "測試";

            // 小 icon 是可以建立 右鍵菜單的
            List<string> menuString = new List<string> { "第一個", "第二個", "第3個" };

            ContextMenu cm = new ContextMenu();
            cm.MenuItems.Clear();

            for (var i = 0; i < menuString.Count(); i++)
            {
                MenuItem m = new MenuItem();
                m.Index = i;
                m.Text = menuString[i];
                m.Click += new EventHandler(MenuClickEvent);
                cm.MenuItems.Add(m);
            }

            // 關閉的
            MenuItem closeMenuItem = new MenuItem();
            closeMenuItem.Index = 4;
            closeMenuItem.Text = "關閉";
            closeMenuItem.Click += new EventHandler(CloseForm);
            cm.MenuItems.Add(closeMenuItem);

            // 附與 右鍵菜單
            n.ContextMenu = cm;

            // 桌面提示 按下後的動作
            n.BalloonTipClicked += new EventHandler(NotificationClickEvent);

            // 小icon 按下後的動作
            n.Click += new EventHandler(IconClickEvent);

            // 澤鼠移到上面的動作 (小 icon)
            n.MouseMove += new MouseEventHandler(NotificationMouseEvent);

            // 跳出桌面提示
            // 如果最後面的 ToolTipIcon 設為 None => 拿 上面的 icon；反之 拿 ToolTipIcon的設置
            n.ShowBalloonTip(5000, "測試", "哈囉", ToolTipIcon.None);
        }


        // -----------------以下是設定相關Event函式-------------------------------------
        // 關閉 Form
        private void CloseForm(object sender, EventArgs e)
        {
            // 這裡的 this 是 Program (是個表單)
            this.Close();
        }

        // 設定 Menu 按到的動作
        private void MenuClickEvent(object sender, EventArgs e)
        {
            MenuItem m = (MenuItem)sender;
            Console.WriteLine($"你按到 {m.Text} ");
        }

        // EventHandler都長這個樣子 (就像 VB2010 的這種)
        // 設定 桌面提示按到的效果
        private void NotificationClickEvent(object sender, EventArgs e)
        {
            Console.WriteLine("桌面提示被按到了");
        }

        // 設定 小icon 被按到的效果
        private void IconClickEvent(object sender, EventArgs e)
        {
            Console.WriteLine("小Icon 被按到了");
        }

        // MouseEventHandler 都這這個樣子
        private void NotificationMouseEvent(object sender, MouseEventArgs e)
        {
            Console.WriteLine("滑鼠移到上面了");
        }
    }
}
