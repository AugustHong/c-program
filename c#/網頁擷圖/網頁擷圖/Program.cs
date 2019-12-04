using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Threading;
using System.IO;

// 下面5個要手動去 加入參考 (加入 System.Windows.Forms 和 System.Web.Extensions 和 System.Drawing)
using System.Windows.Forms;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Drawing.Imaging;

/*
    參考網圵： https://www.itread01.com/article/1452129826.html 
*/

namespace 網頁擷圖
{
    class Program
    {
        static void Main(string[] args)
        {
            // 用下面這2條網圵都可以，但是用 http://www.google.com.tw 就出現一片白 => 所以其他的都要測一下
            //WebSiteThumbnail process = new WebSiteThumbnail("http://www.google.cn", 600, 600, 600, 600);
            WebSiteThumbnail process = new WebSiteThumbnail("https://tw.yahoo.com/", 1000, 1000, 1000, 1000);
            process.Start();
            process.Save("a.png", ImageFormat.Png);
            Console.WriteLine("產生完成");

            Console.Read();
        }
    }

    public class WebSiteThumbnail
    {
        Bitmap m_Bitmap;
        public string m_Url { get; set; }
        public int m_BrowserWidth { get; set; }

        public int m_BrowserHeight { get; set; }

        public int m_ThumbnailWidth { get; set; }

        public int m_ThumbnailHeight { get; set; }

        /// <summary>
        ///  初使化
        /// </summary>
        /// <param name="Url">網圵</param>
        /// <param name="BrowserWidth">網頁寬</param>
        /// <param name="BrowserHeight">網頁高</param>
        /// <param name="ThumbnailWidth">存成的圖片寬</param>
        /// <param name="ThumbnailHeight">存成的圖片高</param>
        public WebSiteThumbnail(string Url, int BrowserWidth, int BrowserHeight, int ThumbnailWidth, int ThumbnailHeight)
        {
            m_Url = Url;
            m_BrowserHeight = BrowserHeight;
            m_BrowserWidth = BrowserWidth;
            m_ThumbnailWidth = ThumbnailWidth;
            m_ThumbnailHeight = ThumbnailHeight;
        }

        /// <summary>
        ///  開始擷取
        /// </summary>
        /// <returns></returns>
        public Bitmap Start()
        {
            return this.Generate();
        }

        /// <summary>
        ///  存檔
        /// </summary>
        /// <param name="goalPath">目標路徑(含副檔名)</param>
        /// <param name="format">儲存的類型(例如：png => ImageFormat.Png)</param>
        /// <returns></returns>
        public bool Save(string goalPath, ImageFormat format)
        {
            try
            {
                this.m_Bitmap.Save(goalPath, format);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
        }

        /// <summary>
        ///  產生圖片
        /// </summary>
        /// <returns></returns>
        public Bitmap Generate()
        {
            Thread m_thread = new Thread(new ThreadStart(_GenerateThread));
            m_thread.SetApartmentState(ApartmentState.STA);
            m_thread.Start();
            m_thread.Join();

            return m_Bitmap;
        }

        /// <summary>
        ///  執行的 Thread
        /// </summary>
        private void _GenerateThread()
        {
            WebBrowser m_WebBrowser = new WebBrowser();
            m_WebBrowser.ScrollBarsEnabled = false;
            m_WebBrowser.Navigate(m_Url);
            m_WebBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(WebBrowser_DocumentCompleted);
            while (m_WebBrowser.ReadyState != WebBrowserReadyState.Complete)
                Application.DoEvents();
            m_WebBrowser.Dispose();
        }

        /// <summary>
        ///  上面函式要呼叫的
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            WebBrowser m_WebBrowser = (WebBrowser)sender;
            m_WebBrowser.ClientSize = new Size(this.m_BrowserWidth, this.m_BrowserHeight);
            m_WebBrowser.ScrollBarsEnabled = false;

            m_Bitmap = new Bitmap(m_WebBrowser.Bounds.Width, m_WebBrowser.Bounds.Height);
            m_WebBrowser.BringToFront();
            m_WebBrowser.DrawToBitmap(m_Bitmap, m_WebBrowser.Bounds);
            m_Bitmap = (Bitmap)m_Bitmap.GetThumbnailImage(m_ThumbnailWidth, m_ThumbnailHeight, null, IntPtr.Zero);
        }
    }
}
