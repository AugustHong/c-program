using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using tessnet2;
using mshtml;
using NuGet.Tessnet2.src;
using System.ComponentModel;
using System.Threading;
using System.IO;

/*
    參考網圵： http://limitedcode.blogspot.com/2014/06/c.html
    1. 請去 NuGet 裝上 Tessnet2
    2. 調整專案 ( 專案右鍵/屬性/建置/平台目標 改成 x86)
    3.
        App.config裡面的<startup> Tag裡加上 useLegacyV2RuntimeActivationPolicy="true"
        => 變成 <startup useLegacyV2RuntimeActivationPolicy="true">
        因為tessnet2的DLL是運行在.Net Framework 2.0上，所以加上這個屬性就可以指定應用程式執行.Net Framework 2.0

    4. 利用WinForm裡的WebBrowser元件取得網頁的DOM資訊 (所以去 加入參考/ System.Windows.Form)
    5. 去 加入參考 / Microsoft.mshtml  (要用搜尋的，有夠難找的)
    6. 在裝上 Tessnet2時會有個 Content 包 和 Ocr.cs 給你 (把 Content 那包，直接搬到 Debug 裡面 ，執行時的路徑要用)
 */

namespace 解析驗證碼
{
    class Program
    {
        static WebBrowser webBrowser = new WebBrowser();

        // Win Form 的東西都要加這個
        [STAThread]
        static void Main(string[] args)
        {
            #region 載入網頁得到圖片 (不知為何一直得不到 body)
            // 指定至網圵
            string urlString = "http://postserv.post.gov.tw/pstmail/main_mail.html";

            // 注意：因為要等整個HTML Document全部載入後才能抓到HTML元件，否則會發生錯誤
            webBrowser.Navigated += WebBrowser_Navigated;

            Uri url = new Uri(urlString); // 指定要讀取的 url
            webBrowser.Navigate(url);

            // 這邊一直讀不到資料 (整個 WebBrowser 讀不到 Document ，懷疑我是用 console 不是用 win from )
            // => 先放棄了！
            //WebBrowser_Navigated(null, null);
            #endregion

            #region 自己載入圖片
            string dir = System.IO.Directory.GetCurrentDirectory() + "\\img\\";
            Image img2 = new Bitmap(dir + "source4.png");

            //將驗證碼圖片灰階化以提高辨識的準確性
            Image newImg2 = Convert2GrayScale(img2);

            // 使用Tessnet Library辨識驗證碼
            string result2 = ParseCaptchaStr(newImg2, "eng", false);
            #endregion

            Console.WriteLine($"驗證碼的值為： {result2}");
            Console.WriteLine("----------------------------------------------------");
            Console.WriteLine("下面跑5張圖的結果：");

            // -----------------------------------------------------------------------
            // 下面是測試 5張圖的

            string tmpResult = string.Empty;

            tmpResult = LoadLocalPicToParse(dir + "source.png");
            Console.WriteLine($"得到的驗證碼是： {tmpResult}");
            tmpResult = LoadLocalPicToParse(dir + "source1.png");
            Console.WriteLine($"得到的驗證碼是： {tmpResult}");
            tmpResult = LoadLocalPicToParse(dir + "source2.png");
            Console.WriteLine($"得到的驗證碼是： {tmpResult}");
            tmpResult = LoadLocalPicToParse(dir + "source3.png");
            Console.WriteLine($"得到的驗證碼是： {tmpResult}");
            tmpResult = LoadLocalPicToParse(dir + "source4.png", numericModel: true);
            Console.WriteLine($"得到的驗證碼是： {tmpResult}");

            Console.ReadLine();
        }

        private static void WebBrowser_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            Console.WriteLine("WebBrowser Document Completed");

            // 等載入完成，要來取資料
            // 可像 js 一樣 取物件
            //var forms = webBrowser.Document.GetElementsByTagName("form");

            // 取得整個網頁的DOM (網圵不能是 https 不然取不到 document)
            var document = webBrowser.Document;
            var doc = document.DomDocument as HTMLDocument;
            var body = doc.body as HTMLBody;
            IHTMLControlRange range = body.createControlRange();

            // 取得驗證碼圖片
            //var imgEle = (forms[0].GetElementsByTagName("img"))[0].DomElement as IHTMLControlElement;

            // 取得驗證碼圖片
            // 我這邊這隻剛好是 取 img 中的 index = 3 (就是第4個啦)
            var imgEle = document.GetElementsByTagName("img")[3].DomElement as IHTMLControlElement;
            range.add(imgEle);
            range.execCommand("copy", false, Type.Missing);

            // 可以取值
            //string test = document.GetElementById("captcha").InnerHtml;
            // 也可以填值
            //document.GetElementById("captcha").SetAttribute("class", "hahaha");
            //document.GetElementById("captcha").InnerHtml = "1234";

            // 將圖片轉換成.Net物件
            Image img = Clipboard.GetImage();

            //將驗證碼圖片灰階化以提高辨識的準確性
            Image newImg = Convert2GrayScale(img);

            // 使用Tessnet Library辨識驗證碼
            string result = ParseCaptchaStr(newImg, "eng", false);

            Console.WriteLine($"驗證碼的值為： {result}");
            Console.WriteLine("----------------------------------------------------");
        }

        /// <summary>
        /// 將驗證碼圖片灰階化以提高辨識的準確性
        /// 接下來將.Net圖片物件轉換成灰階
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Image Convert2GrayScale(Image img)
        {
            Bitmap bitmap = new Bitmap(img);

            for (int i = 0; i < bitmap.Width; i++)
            {
                for (int j = 0; j < bitmap.Height; j++)
                {
                    Color pixelColor = bitmap.GetPixel(i, j);
                    byte r = pixelColor.R;
                    byte g = pixelColor.G;
                    byte b = pixelColor.B;

                    byte gray = (byte)(0.299 * (float)r + 0.587 * (float)g + 0.114 * (float)b);
                    r = g = b = gray;
                    pixelColor = Color.FromArgb(r, g, b);

                    bitmap.SetPixel(i, j, pixelColor);
                }
            }

            Image grayImage = Image.FromHbitmap(bitmap.GetHbitmap());
            bitmap.Dispose();
            return grayImage;
        }

        /// <summary>
        /// 使用Tessnet Library辨識驗證碼
        /// </summary>
        /// <param name="image"></param>
        /// <param name="numericModel">是否是數字模式(預設為false)</param>
        /// <returns></returns>
        public static string ParseCaptchaStr(Image image, string lang = "eng", bool numericModel = false)
        {
            var sb = new StringBuilder();
            //using (var ocr = new Tesseract())
            //{
            //    // 設定辨識的相關設定(以下設定為辨識數字)
            //    ocr.SetVariable("tessedit_char_whitelist", "0123456789");
            //    // 樣本資料夾位置
            //    var path = string.Concat(Application.StartupPath, @"\tessdata");
            //    int n = ocr.Init(path, "eng", true);
            //    var result = ocr.DoOCR(new Bitmap(image), Rectangle.Empty);
            //    // 將辨識結果轉換成字串
            //    result.ForEach(c =>
            //    {
            //        sb.Append(c.Text);
            //    });
            //}

            var path = System.IO.Directory.GetCurrentDirectory() + "\\Content\\tessdata";
            Ocr ocr = new Ocr(path);
            List<tessnet2.Word> result = ocr.DoOcrNormal(new Bitmap(image), lang, numericModel);

            //將辨識結果轉換成字串
            result.ForEach(c =>
            {
                sb.Append(c.Text);
            });

            return sb.ToString();
        }

        /// <summary>
        ///  丟入 圖檔路徑 進行 解析
        /// </summary>
        /// <param name="filePath"></param>
        /// <param name="numericModel">是否是數字模式(預設為false)</param>
        /// <returns></returns>
        public static string LoadLocalPicToParse(string filePath, string lang = "eng", bool numericModel = false)
        {
            string result = string.Empty;

            if (File.Exists(filePath))
            {
                try
                {
                    Image img = new Bitmap(filePath);
                    Image newImg = Convert2GrayScale(img);
                    result = ParseCaptchaStr(newImg, lang, numericModel);
                }
                catch
                {
                    return string.Empty;
                }
            }

            return result;
        }

        /// <summary>
        /// 丟入 圖檔路徑 進行 解析
        /// </summary>
        /// <param name="image"></param>
        /// <param name="lang"></param>
        /// <param name="numericModel"></param>
        /// <returns></returns>
        public static string LoadPicToParse(Image img, string lang = "eng", bool numericModel = false)
        {
            string result = string.Empty;

            try
            {
                Image newImg = Convert2GrayScale(img);
                result = ParseCaptchaStr(newImg, lang, numericModel);
            }
            catch
            {
                return string.Empty;
            }

            return result;
        }
    }
}
