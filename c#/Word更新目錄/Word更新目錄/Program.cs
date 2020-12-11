using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.IO;
using Microsoft.Office.Interop.Word;

/*
    參考網圵： https://blog.csdn.net/adliy_happy/article/details/77882971?utm_medium=distribute.pc_relevant.none-task-blog-BlogCommendFromMachineLearnPai2-8.control&depth_1-utm_source=distribute.pc_relevant.none-task-blog-BlogCommendFromMachineLearnPai2-8.control
    1. 去 NuGet 裝上 Microsoft.Office.Interop.Word
 */

namespace Word更新目錄
{
    class Program
    {
        static void Main(string[] args)
        {
            /*
                註： 我特地改成 標楷體 => 但新出來的還是一樣 (但 "目錄" 這文字 倒是有變化)
             */

            string rootPath = Directory.GetCurrentDirectory() + "\\";
            string sourcePath = rootPath + "input.docx";
            string goalPath = rootPath + "output.docx";

            Application wordApp = new Application() { Visible = false };
            // 這裡路徑要給明確，不然他吃不到
            Document doc = wordApp.Documents.Open(sourcePath);
            int count = doc.TablesOfContents.Count;
            for (int i = 0; i < count; i++)
            {
                doc.TablesOfContents[i + 1].Update();
            }
            doc.SaveAs2(goalPath, WdSaveFormat.wdFormatDocumentDefault);

            Console.WriteLine("執行完成");
            Console.ReadLine();
        }
    }
}
