using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Forms;
using System.Drawing;
using System.ComponentModel;

/*
    參考網圵： https://docs.microsoft.com/zh-tw/dotnet/framework/winforms/controls/walkthrough-running-an-operation-in-the-background
    
    本次一樣使用 Form 來實作 => 去 加入參考 System.Windows.Forms 和 System.Drawing
*/

namespace CSharp使用背景程式
{
    class Program : Form
    {
        // -------進入點----------
        [STAThread]
        static void Main(string[] args)
        {
            Application.Run(new Program());
        }

        // ---------物件----------------------
        public Button startBtn = new Button();
        public Button endBtn = new Button();

        // 背景程式物件
        public BackgroundWorker bw = new BackgroundWorker(); 


        // --------主程式--------------
        public Program()
        {
            // 設定 Form 樣式
            this.Text = "C#執行背景程式作業";
            this.Size = new Size(300, 150);

            // 設定 button
            this.startBtn.Text = "開始背景程式";
            this.startBtn.Size = new Size(100, 100);
            this.startBtn.Location = new Point(0, 0);
            this.startBtn.Enabled = true;
            this.startBtn.Click += StartBtn_Click;

            this.endBtn.Text = "取消背景程式";
            this.endBtn.Size = new Size(100, 100);
            this.endBtn.Location = new Point(110, 0);
            this.endBtn.Enabled = false;
            this.endBtn.Click += EndBtn_Click;

            // 設定背景程式
            this.bw.WorkerSupportsCancellation = true;   // 一定要開啟這個，才能取消背景程式
            this.bw.DoWork += Bw_DoWork;
            this.bw.RunWorkerCompleted += Bw_RunWorkerCompleted;

            // 把 按鈕 加入至 Form 中
            this.Controls.Add(startBtn);
            this.Controls.Add(endBtn);
        }

        // -----------------------背景程式相關函式----------------------------

        // 背景程式的進入點
        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            // 先轉型
            BackgroundWorker b = sender as BackgroundWorker;

            // 得到 毫秒數
            int Msecond = (int)e.Argument;

            // 開始主程式 (如果想要回傳值的話，可以在這邊接收)
            Bw_Main(b, Msecond);

            // 如果取消要把 Cancel 屬性改為 true
            if (b.CancellationPending)
            {
                e.Cancel = true;
            }
        }

        // 背景程式執行完成後要做的事
        private void Bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Cancelled)
            {
                Console.WriteLine("背景程式已取消");
            }
            else if (e.Error != null)
            {
                Console.WriteLine($"背景程式執行時發生問題： {e.Error.Message}");
            }
            else
            {
                Console.WriteLine("背景程式執行主程式執行完成");
            }
        }

        // 背景程式主程式
        private void Bw_Main(BackgroundWorker b, int sleepPeriod)
        {
            // 背景程式 只會被進入一次 => 所以要用 While 來實作 讓他一直跑(用 sleepPeriod來控制次數)
            // 而因為有取消 ， 所以當 b.CancellationPending 變為 true 時 就取消
            while (!b.CancellationPending)
            {
                string today = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                Console.WriteLine($"現在時間是： {today}");

                // 要讓他睡 (不然他又會繼續執行)
                System.Threading.Thread.Sleep(sleepPeriod);
            }
        }


        // ----------Button Event-------------------------------
        private void EndBtn_Click(object sender, EventArgs e)
        {
            Console.WriteLine("------------取消背景程式---------------");
            this.endBtn.Enabled = false;
            this.startBtn.Enabled = true;

            // 取消 背景程式
            this.bw.CancelAsync();
        }

        private void StartBtn_Click(object sender, EventArgs e)
        {
            Console.WriteLine("--------------開始執行背景程式---------------");

            this.startBtn.Enabled = false;
            this.endBtn.Enabled = true;

            // 開始 背景程式
            int Msecond = 2000;
            this.bw.RunWorkerAsync(Msecond);
        }
    }
}
