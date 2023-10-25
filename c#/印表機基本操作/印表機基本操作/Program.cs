using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Printing;
using System.IO;
using System.Reflection;

/*
    參考網址： https://dotblogs.com.tw/chou/2009/10/10/10989 、 https://learn.microsoft.com/zh-tw/dotnet/api/system.drawing.printing.printdocument.print?view=dotnet-plat-ext-7.0
    參考/右鍵/加入參考/組件/System.Drawing
 */

namespace 印表機基本操作
{
    internal class Program
    {
        public static Font printFont;
        public static StreamReader streamToPrint;

        static void Main(string[] args)
        {
            // 取得預設的印表機
            PrintDocument printDoc = new PrintDocument();
            string sDefaultPrinter = printDoc.PrinterSettings.PrinterName;  // 取得預設的印表機名稱
            var paperSize = printDoc.PrinterSettings.PaperSizes;  //支援此印表機的紙張大小
            bool supportColor = printDoc.PrinterSettings.SupportsColor; //此印表機是否支援彩色列印
            bool isPlotter = printDoc.PrinterSettings.IsPlotter;  //此印表機是否是繪圖機
            int maxCopies = printDoc.PrinterSettings.MaximumCopies; //取得一次最大可列印份數


            Console.WriteLine($"預設印表機： {sDefaultPrinter}");
            Console.WriteLine("=========================================================");

            // 取得全部有裝的印表機
            int i = 1;
            foreach (string strPrinter in PrinterSettings.InstalledPrinters)
            {
                Console.WriteLine($"印表機 {i} = {strPrinter}");
                i++;
            }

            // 開始列印 (未實測過)

            string fileName = "test.docx";
            //streamToPrint = new StreamReader(fileName);
            //try
            //{
            //    printFont = new Font("Arial", 10);  //設定字型

            //    PrintDocument pd = new PrintDocument();
            //    pd.PrintPage += new PrintPageEventHandler(pd_PrintPage);
            //    pd.PrinterSettings.PrinterName = sDefaultPrinter;  //設定要列印的印表機
            //                                                       //pd.PrinterSettings.CanDuplex = true;   //是否支援雙面列印
            //                                                       //pd.PrinterSettings.Collate = true;  //是否要自動分頁
            //                                                       //pd.PrinterSettings.Copies = 1; //文件列印份數
            //                                                       //pd.PrinterSettings.FromPage = 1; //取得/設定 第1頁要列印的頁碼(從第幾頁開始列印的概念)
            //                                                       //pd.PrinterSettings.PrintRange = PrintRange.CurrentPage;  //設定指定列印頁數
            //                                                       //pd.PrinterSettings.ToPage = 10; // 取得/設定 要列印的最後1頁頁碼


            //    // 橫向列印
            //    //pd.DefaultPageSettings.Landscape = true;

            //    if (pd.PrinterSettings.IsValid)
            //    {
            //        pd.Print();
            //    }
            //    else
            //    {
            //        Console.WriteLine("Printer is invalid.");
            //    }
            //}
            //finally
            //{
            //    streamToPrint.Close();
            //}

            Console.ReadLine();
        }

        // 列印功能
        public static void pd_PrintPage(object sender, PrintPageEventArgs ev)
        {
            float linesPerPage = 0;
            float yPos = 0;
            int count = 0;
            float leftMargin = ev.MarginBounds.Left;
            float topMargin = ev.MarginBounds.Top;
            string line = null;

            // 計算每頁行數
            linesPerPage = ev.MarginBounds.Height /  printFont.GetHeight(ev.Graphics);

            // 跑過每行
            while (count < linesPerPage && ((line = streamToPrint.ReadLine()) != null))
            {
                yPos = topMargin + (count * printFont.GetHeight(ev.Graphics));
                ev.Graphics.DrawString(line, printFont, Brushes.Black, leftMargin, yPos, new StringFormat());
                count++;
            }

            // 查看是否還有其他頁
            if (line != null)
                ev.HasMorePages = true;
            else
                ev.HasMorePages = false;
        }
    }
}
