using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址：(參考/右鍵/加入參考/System.Drawing)
    https://blog.csdn.net/weixin_42953003/article/details/119751529
    https://www.mlplus.net/2020/04/04/csharpimagecompress/

    https://vocus.cc/article/65ba6d24fd8978000167f2c8
 */

namespace 壓縮圖檔
{
      internal class Program
      {
            static void Main(string[] args)
            {
                  DirectoryInfo id = new DirectoryInfo("input");
                  DirectoryInfo od1 = new DirectoryInfo("output1");
                  DirectoryInfo od2 = new DirectoryInfo("output2");
                  DirectoryInfo od3 = new DirectoryInfo("output3");
                  DirectoryInfo od4 = new DirectoryInfo("output4");

                  string fileName = "abc.jpg";  // 用 png 都會變肥
                  string inputFileName = id.FullName + "/" + fileName;

                  // 用 品質壓縮
                  CompressionImageHelper.CompressionImageByQuality(inputFileName, od1.FullName, 1);

                  // 壓到特定KB (這邊指定 30KB)，看來也不能太誇張的壓縮
                  bool result = CompressionImageHelper.CompressionImageByMaxFileSize(inputFileName, od2.FullName, 25, 10);
                  string success = result ? "是" : "否";
                  Console.WriteLine($"是否有壓縮至符合項目： {success}");

                  // 依大小
                  CompressionImageHelper.CompressionImageByPicSize(inputFileName, od3.FullName, 20, 20);

                  // 使用第二種作法壓縮(品質目前不變，但效果感覺更好)
                  CompressionImage(inputFileName, (od4.FullName + "/" + fileName), 50);

                  Console.WriteLine("完成");
                  Console.Read();
            }

            // 第二種作法
            /// <summary>
            /// 依百分比壓縮圖檔
            /// </summary>
            /// <param name="inputFileName">來源圖檔路徑</param>
            /// <param name="outFileName">輸出圖檔路徑</param>
            /// <param name="range">壓縮百分比</param>
            static void CompressionImage(string inputFileName, string outputFileName, double range = 100)
            {
                  MemoryStream oMS = new MemoryStream();

                  // 百分比換算
                  double persent = (range / 100);

                  using (Bitmap inputImage = new Bitmap(inputFileName))
                  {
                        int newWidth = (int)(inputImage.Width * persent);
                        int newHeight = (int)(inputImage.Height * persent);

                        /*
                            若想用固定的長度or寬度可以用以下寫法：
                            --int newWidth = 240;
                            --int newHeight = (int)(inputImage.Height * ((float)newWidth / inputImage.Width));
                         */

                        using (Bitmap outputImage = new Bitmap(newWidth, newHeight))
                        {
                              using (Graphics g = Graphics.FromImage(outputImage))
                              {
                                    g.DrawImage(inputImage, 0, 0, newWidth, newHeight);
                              }

                              // 不知為何會一直出現這個錯誤：在GDI+中發生泛型錯誤
                              // 所以不能用下面這一行
                              // outputImage.Save(outputFileName, ImageFormat.Jpeg);
                              // 要改用下面這段
                              //將outputImage儲存（指定）到記憶體串流中
                              outputImage.Save(oMS, ImageFormat.Jpeg);
                        }
                  }

                  //將串流整個讀到陣列中，寫入某個路徑中的某個檔案裡
                  using (FileStream oFS = File.Open(outputFileName, FileMode.OpenOrCreate))
                  {
                        oFS.SetLength(0);  //清空
                        oFS.Write(oMS.ToArray(), 0, oMS.ToArray().Length);
                  }
            }
      }
}
