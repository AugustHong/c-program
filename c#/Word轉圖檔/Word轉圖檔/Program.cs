using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;

/*
    參考網圵： https://dotblogs.com.tw/eganblog/2016/11/06/doc_convert_to_img
    1. 去 NuGet 裝上 Microsoft.Office.Interop.Word
    2. 去 加入參考/ System.Drawing
 */

namespace Word轉圖檔
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = Directory.GetCurrentDirectory();
            string sourceFilePath = rootPath + "\\" + "source.docx";

            WordConvertToImage(sourceFilePath, rootPath, "Image_{{page}}");
            Console.WriteLine("執行完成");
            Console.ReadLine();
        }

        /// <summary>
        ///  Word 轉成 Image (預設給 png) => 之後自己實作可自行調整
        /// </summary>
        /// <param name="sourceFilePath">來源路徑(整完路徑)</param>
        /// <param name="goalDir">目標資料夾路徑(最後面不要加上 \\)</param>
        /// <param name="goalFileName">檔名(可輸入 {{page}} 代表頁數； 可輸入 {{index}} 代表序號) (例： 輸入 File_{{page}}_{{index}} => 則第1頁出來的會是 File_1_0)</param>
        /// <returns></returns>
        static bool WordConvertToImage(string sourceFilePath, string goalDir, string goalFileName)
        {
            try
            {
                var msWordApp = new Microsoft.Office.Interop.Word.Application();
                msWordApp.Visible = false;

                Microsoft.Office.Interop.Word.Document doc = msWordApp.Documents.Open(sourceFilePath);

                foreach (Microsoft.Office.Interop.Word.Window window in doc.Windows)
                {
                    foreach (Microsoft.Office.Interop.Word.Pane pane in window.Panes)
                    {
                        for (var i = 1; i <= pane.Pages.Count; i++)
                        {
                            var page = pane.Pages[i];
                            object bits = page.EnhMetaFileBits;//Returns a Object that represents a picture 【Read-only】
                                                               //以下Try Catch區段為將圖片的背景填滿為白色，不然輸出的圖片背景會是透明的
                            try
                            {
                                using (var ms = new MemoryStream((byte[])bits))
                                {
                                    System.Drawing.Image image = System.Drawing.Image.FromStream(ms);

                                    using (var backGroundWritePNG = new Bitmap(image.Width, image.Height))
                                    {
                                        //設定圖片的解析度
                                        backGroundWritePNG.SetResolution(image.HorizontalResolution, image.VerticalResolution);

                                        using (Graphics graphis = Graphics.FromImage(backGroundWritePNG))
                                        {
                                            graphis.Clear(Color.White);
                                            graphis.DrawImageUnscaled(image, 0, 0);
                                        }

                                        string name = goalFileName.Replace("{{page}}", i.ToString()).Replace("{{index}}", (i - 1).ToString()) + ".png";
                                        string outPutFilePath = goalDir + "\\" + name;

                                        if (File.Exists(outPutFilePath))
                                        {
                                            File.Delete(outPutFilePath);
                                        }

                                        backGroundWritePNG.Save(outPutFilePath, ImageFormat.Png);//輸出圖片
                                    }
                                }
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine(ex.ToString());
                            }
                        }
                    }
                }

                //關閉Word，釋放資源
                doc.Close(Type.Missing, Type.Missing, Type.Missing);
                msWordApp.Quit(Type.Missing, Type.Missing, Type.Missing);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(doc);
                System.Runtime.InteropServices.Marshal.ReleaseComObject(msWordApp);

                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return false;
            }
        }
    }
}
