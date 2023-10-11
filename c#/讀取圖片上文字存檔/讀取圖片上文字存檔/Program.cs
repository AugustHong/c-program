using Spire.OCR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/*
    參考網址：https://blog.csdn.net/weixin_43972758/article/details/126937172

    執行步驟：
    (1) 對專案/右鍵/屬性/建置/平台目標/x64 ，再存檔
    (2) 去 NuGet 裝上 Spire.OCR
    (3) 將 packages\Spire.OCR.1.8.0\runtimes\win-x64\native 移到 bin\Debug
 */

namespace 讀取圖片上文字存檔
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /* 目前判斷大概 70% 吧， 連網頁上標準的文字也會出錯。看來實用性待考慮*/
            OcrScanner sc = new OcrScanner();
            sc.Scan("test.png");
            string text = sc.Text.ToString();
            File.WriteAllText("output.txt", text);
        }
    }
}
