using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
    參考網圵： https://blog.csdn.net/softimite_zifeng/article/details/54343341 

    因為直接用 Console關閉 就關了 => 看不出效果 => 用跟 Notify一樣的 Form來實作
    => 去 加入參考 選 System.Windows.Forms 和 System.Drawing 加入
*/

namespace 開關Console
{
    class Program : Form
    {
        // ---------------------控制 Console 的開關 相關方法 (主要看這邊)
        #region 控制 Console 的開關 相關方法
        // 1. 先定義系統API

        // 啟動控制台
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool AllocConsole();

        // 釋放控制台
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool FreeConsole();

        // 禁用上方 X 按鈕
        [DllImport("Kernel32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetConsoleWindow();
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr GetSystemMenu(IntPtr hWnd, IntPtr bRevert);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr RemoveMenu(IntPtr hMenu, uint uPosition, uint uFlags);

        // 2. 實作

        // 啟動
        public static bool OpenConsole()
        {
            return AllocConsole();
        }

        // 關閉
        public static bool CloseConsole()
        {
            return FreeConsole();
        }

        // 禁用 X 按鈕
        public static bool OpenConsoleForNoClose()
        {
            bool flag = AllocConsole();
            if (flag)
            {
                //禁用 X 按鈕
                IntPtr windowHandle = GetConsoleWindow();
                IntPtr closeMenu = GetSystemMenu(windowHandle, IntPtr.Zero);
                const uint SC_CLOSE = 0xF060;
                RemoveMenu(closeMenu, SC_CLOSE, 0x0);
            }
            return flag;
        }
        #endregion


        // --------共用物件
        public Button closeBtn = new Button();
        public Button openBtn = new Button();
        public Button openForNoCloseBtn = new Button();


        // --------- 進入點 ---------------
        static void Main(string[] args)
        {
            Application.Run(new Program());
        }

        // -----主程式 (含一些 Form的基本操作)------------
        public Program()
        {
            // 一開始先讓 Console不見
            bool t = CloseConsole();

            // 設定 Form 的值
            this.Text = "開關Console";
            this.Size = new Size(300, 125);
            // this.Visible = false;  // 沒啥作用
            // this.ShowInTaskbar = false;  // 不要出現在工具列中(表單)
            // this.WindowState = FormWindowState.Minimized;  // 設定為最小化
            // this.Hide();  // 設定自己(Form 隱藏) => 沒啥作用

            // 自已的圖示
            // 1. 先把圖片存成正方形 16x16  32x32 並存成png
            // 2. 去 https://www.convertico.com/ 進行轉換
            // 3. 得到 .ico檔  放在這邊即可
            this.Icon = new Icon("1.ico", 120, 300);  // 設定 ICON

            // -------------------------------------------------------------

            // 設定 3個按鈕的屬性
            this.closeBtn.Text = "關閉Console";
            this.closeBtn.Click += CloseBtn_Click;
            this.closeBtn.Enabled = false;
            this.closeBtn.Size = new Size(80, 80);
            this.closeBtn.Location = new Point(0, 0);

            this.openBtn.Text = "開啟Console";
            this.openBtn.Click += OpenBtn_Click;
            this.openBtn.Size = new Size(80, 80);
            this.openBtn.Location = new Point(90, 0);

            this.openForNoCloseBtn.Text = "開啟Console但禁用X鍵";
            this.openForNoCloseBtn.Click += OpenForNoCloseBtn_Click;
            this.openForNoCloseBtn.Size = new Size(80, 80);
            this.openForNoCloseBtn.Location = new Point(190, 0);

            // 新增 物件 至 Form 表單中
            this.Controls.Add(this.closeBtn);
            this.Controls.Add(this.openBtn);
            this.Controls.Add(this.openForNoCloseBtn);
        }


        // -------相關 Event 事件
        private void CloseBtn_Click(object sender, EventArgs e)
        {
            // 關閉 Console
            bool isSuccess = CloseConsole();

            // 把關閉的 Enabled 關起來，其餘打開
            this.closeBtn.Enabled = false;
            this.openBtn.Enabled = true;
            this.openForNoCloseBtn.Enabled = true;
        }

        private void OpenBtn_Click(object sender, EventArgs e)
        {
            // 開啟 Console
            bool isSuccess = OpenConsole();

            // 把關閉打開，其餘關閉
            this.closeBtn.Enabled = true;
            this.openBtn.Enabled = false;
            this.openForNoCloseBtn.Enabled = false;
        }

        private void OpenForNoCloseBtn_Click(object sender, EventArgs e)
        {
            // 開啟 Console
            bool isSuccess = OpenConsoleForNoClose();

            // 把關閉打開，其餘關閉
            this.closeBtn.Enabled = true;
            this.openBtn.Enabled = false;
            this.openForNoCloseBtn.Enabled = false;
        }
    }
}
