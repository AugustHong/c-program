using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.IO;
using System.ComponentModel;
using System.Diagnostics;
using System.Threading;

namespace 每日定時鬧鐘
{
    // ToDoList 的項目 Class
    public class Item
    {
        public int hour { get; set; }

        public int minute { get; set; }

        public string title { get; set; }

        public string content { get; set; }

        public int timeout { get; set; }
    }

    // 主程式
    class Program : Form
    {
        // ---------------------控制 Console 的開關 相關方法 (主要看這邊)
        #region 控制 Console 的開關 相關方法
        // 1. 先定義系統API

        // 釋放控制台
        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public static extern bool FreeConsole();

        // 2. 實作
        // 關閉
        public static bool CloseConsole()
        {
            return FreeConsole();
        }
        #endregion

        // 要跑的 ToDo 項目 
        public List<Item> toDoList = new List<Item>
        {
            new Item{hour = 11, minute = 57, title = "快要到吃午餐的時候了", content = "忙了一上午，肚子也餓了！要吃飽才能應付下午的行程，要準備去吃午餐囉~", timeout = 2000},
            new Item{hour = 12, minute = 0, title = "午餐時間到", content = "該吃午餐囉！要吃飽才有精神繼續下午的活動", timeout = 4000},
            new Item{hour = 13, minute = 0, title = "午休結束", content = "起床動動身囉", timeout = 2000},
            new Item{hour = 15, minute = 30, title = "下午茶休息時間到", content = "隨著音樂的響起，又是3點半的時候到了", timeout = 2000},
            new Item{hour = 17, minute = 27, title = "快要準備下班囉~", content = "收拾今天的忙碌，該準備收拾一下了。順便確認明天的行程喔", timeout = 2000},
            new Item{hour = 17, minute = 30, title = "下班時間到", content = "忙了一整天了，是時候該休息了~ 下班到囉！準備收東西吧", timeout = 2000},
        };

        // 要跑 cmd的
        public Process process = new Process();

        // notify
        public NotifyIcon n = new NotifyIcon();

        // 背景程式
        public BackgroundWorker bw = new BackgroundWorker();

        [STAThread]
        static void Main(string[] args)
        {
            Application.Run(new Program());
        }

        public Program()
        {
            // 關掉 console
            bool t = CloseConsole();
            System.Diagnostics.Debug.WriteLine(t);

            // 設定表單
            // 目的： 不要讓表單出來 (因為會變成有 Console 和 表單 同時出現在螢幕上)
            this.Text = "每日定時鬧鐘";
            this.Size = new Size(0, 0);  // 會剩下上面的工具列
            this.Visible = false;  // 沒啥作用
            this.ShowInTaskbar = false;  // 不要出現在工具列中(表單)
            this.WindowState = FormWindowState.Minimized;  // 設定為最小化
            this.Hide();  // 設定自己(Form 隱藏) => 沒啥作用

            // https://docs.microsoft.com/zh-tw/dotnet/framework/winforms/controls/walkthrough-running-an-operation-in-the-background
            // 設定 Notify
            // 開始建立 NotifyIcon
            this.n = new NotifyIcon();
            this.n.Visible = true;
            this.n.Icon = new Icon("1.ico", 120, 300);
            this.n.Text = "每日定時鬧鐘";
            this.n.DoubleClick += new EventHandler(N_DoubleClick);  // 按2下是關掉Form
            this.n.BalloonTipClicked += new EventHandler(N_BalloonTipClicked);

            // 因為 原本的While寫在這，但是它會Hold住資源 => 讓 Notify的那些事件觸發不了
            // 解決方式： 把原本的 While 抽到背景程式
            // 結論： 成功

            // 背景程式相關設定
            this.bw.WorkerSupportsCancellation = true;   // 支援非同步取消背景程式(要設這個才能用 bw.CancelAsync();)
            this.bw.DoWork += Bw_DoWork;
            this.bw.RunWorkerCompleted += Bw_RunWorkerCompleted;


            // 開始執行背景程式 (我只要跑一次即可 => 所以 86400 = 24 * 60 * 60)
            this.bw.RunWorkerAsync(86400);
        }

        // --------------背景程式相關-----------------------------------------
        // 背景程式的進入點
        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            // 轉型
            BackgroundWorker b = sender as BackgroundWorker;

            // 得到秒數設定
            int second = (int)e.Argument;

            // 開始執行背景主程式
            BW_Main(b, second);

            // 如果取消的話，設定 Cancel屬性為 true
            if (b.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        // 背景程式執行完要做啥
        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            System.Diagnostics.Debug.WriteLine("背景程式執行完成");
        }

        // 背景程式 執行 主程式
        private void BW_Main(BackgroundWorker b, int sleepPeriod)
        {
            int currentIndex = 0;
            this.toDoList = this.toDoList.OrderBy(todo => todo.hour).ThenBy(todo => todo.minute).ToList();  // 排序

            // 開始跑迴圈
            while (!b.CancellationPending)
            {
                Item currentItem = this.toDoList[currentIndex];
                DateTime nowTime = DateTime.Now;

                // 如果時間相同的話 => 跳出桌面提示
                if (nowTime.Hour == currentItem.hour && nowTime.Minute == currentItem.minute)
                {
                    this.n.ShowBalloonTip(currentItem.timeout, currentItem.title, currentItem.content, ToolTipIcon.Info);
                    currentIndex++;
                }

                // 如果超過上限 => 今天結束 => 關掉 Form
                if (currentIndex > (this.toDoList.Count() - 1))
                {
                    // 要先讓最後一個的 桌面提示跑完
                    Item lastItem = this.toDoList[this.toDoList.Count() - 1];
                    System.Threading.Thread.Sleep(lastItem.timeout);

                    // 取消背景程式
                    this.bw.CancelAsync();
                    // 關閉 Form
                    this.Close();
                }
                else
                {
                    // 有 ++ 過 => 所以 next 會是最新的
                    Item nextItem = this.toDoList[currentIndex];

                    // 計算 差幾秒 (不要讓迴圈一直跑，所以直接等待到下一個到)
                    nowTime = DateTime.Now;
                    TimeSpan t1 = new TimeSpan(nowTime.Hour, nowTime.Minute, 0);
                    TimeSpan t2 = new TimeSpan(nextItem.hour, nextItem.minute, nowTime.Second);

                    // 差幾秒
                    int dSecond = (int)((t2 - t1).TotalSeconds);

                    if (dSecond < 0)
                    {
                        for (var i = 0; i < this.toDoList.Count(); i++)
                        {
                            Item item = this.toDoList[i];

                            if ((nowTime.Hour < item.hour) || ((nowTime.Hour == item.hour) && (nowTime.Minute < item.minute)))
                            {
                                currentIndex = i;
                                break;
                            }
                        }
                    }
                    else
                    {
                        // 讓它停在這，等到下一個出來
                        System.Threading.Thread.Sleep(dSecond);
                    }
                }
            }
        }

        // ---------Event--------------------
        private void N_DoubleClick(object sender, EventArgs e)
        {
            // 取消背景程式
            this.bw.CancelAsync();

            // 關掉Form
            this.Close();
        }

        private void N_BalloonTipClicked(object sender, EventArgs e)
        {
            // 如果時間 Hour = 11 或 12 的話 => 叫起來 cmd 執行 chrome.exe 午餐吃什麼的html
            DateTime dt = DateTime.Now;
            if (dt.Hour == 11 || dt.Hour == 12)
            {
                CallCmdToCallHtml();
            }
        }

        // -----------Func----------------------
        private void CallCmdToCallHtml()
        {
            // 呼叫 cmd 去執行 把 午餐吃什麼.html 叫起來           
            try
            {
                // 相關設定 (基本上就照抄即可)
                this.process.StartInfo.UseShellExecute = false;   //是否使用作業系統shell啟動 
                this.process.StartInfo.CreateNoWindow = true;   //是否在新視窗中啟動該程序的值 (不顯示程式視窗)
                this.process.StartInfo.RedirectStandardInput = true;  // 接受來自呼叫程式的輸入資訊 
                this.process.StartInfo.RedirectStandardOutput = true;  // 由呼叫程式獲取輸出資訊
                this.process.StartInfo.RedirectStandardError = true;  //重定向標準錯誤輸出

                this.process.StartInfo.FileName = "cmd.exe";   // 要執行的程式名

                this.process.Start();                         // 啟動程式

                // 後面依自已位置要做變化
                this.process.StandardInput.WriteLine("cd C:/Program Files (x86)/Google/Chrome/Application"); //向cmd視窗傳送輸入資訊
                this.process.StandardInput.AutoFlush = true;  // 自動刷新

                // 後面依自已位置要做變化
                this.process.StandardInput.WriteLine("chrome.exe file:///C:/Users/green/Desktop/%E5%8D%88%E9%A4%90%E5%90%83%E4%BB%80%E9%BA%BC/Index.html"); //向cmd視窗傳送輸入資訊
                this.process.StandardInput.AutoFlush = true;  // 自動刷新

                // 關閉 Process
                this.process.Close();
            }
            catch (Win32Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);

                // 關閉 Process
                this.process.Close();
            }
        }
    }
}
