using Microsoft.Office.Interop.PowerPoint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    1. 去 NuGet 裝上 Microsoft.Office.Interop.PowerPoint
 */

namespace PPT和PPTX互轉_轉成圖片檔和影片檔和PDF
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = System.IO.Directory.GetCurrentDirectory() + "\\";
            string sourcePath = rootPath + "source.pptx";

            Application pptApp = new Application();

            // 這裡路徑要給明確，不然他吃不到
            // ppt 的隱藏 並不是 pptApp.Visible = Microsoft.Office.Core.MsoTriState.msoFalse 這個會爆錯
            // 要用下面的方法
            Presentation ppt = pptApp.Presentations.Open(sourcePath, WithWindow: Microsoft.Office.Core.MsoTriState.msoFalse);
            
            // 轉檔
            ppt.SaveAs(rootPath + "output.ppt", PpSaveAsFileType.ppSaveAsPresentation);
            ppt.SaveAs(rootPath + "output.pdf", PpSaveAsFileType.ppSaveAsPDF);
            ppt.SaveAs(rootPath + "output.pptx", PpSaveAsFileType.ppSaveAsDefault);
            ppt.SaveAs(rootPath + "output.png", PpSaveAsFileType.ppSaveAsPNG);
            ppt.SaveAs(rootPath + "output.gif", PpSaveAsFileType.ppSaveAsGIF);

            // 影片檔的要這樣轉
            int second = 0;
            ppt.CreateVideo("output.wmv");
            ppt.SaveCopyAs(rootPath + "output.wmv", PpSaveAsFileType.ppSaveAsWMV, Microsoft.Office.Core.MsoTriState.msoCTrue);
            Console.WriteLine("output.wmv 轉檔中…");
            while (ppt.CreateVideoStatus == PpMediaTaskStatus.ppMediaTaskStatusInProgress)
            {
                System.Threading.Thread.Sleep(1000);
                second++;
            }
            Console.WriteLine($"output.wmv 共花費 {second} 秒");

            second = 0;
            ppt.CreateVideo("output.mp4");
            ppt.SaveCopyAs(rootPath + "output.mp4", PpSaveAsFileType.ppSaveAsMP4, Microsoft.Office.Core.MsoTriState.msoCTrue);
            Console.WriteLine("output.mp4 轉檔中…");
            while (ppt.CreateVideoStatus == PpMediaTaskStatus.ppMediaTaskStatusInProgress)
            {
                System.Threading.Thread.Sleep(1000);
                second++;
            }
            Console.WriteLine($"output.mp4 共花費 {second} 秒");

            ppt.Close();
            pptApp.Quit();

            Console.WriteLine("執行完成");
            Console.ReadLine();
        }
    }
}
