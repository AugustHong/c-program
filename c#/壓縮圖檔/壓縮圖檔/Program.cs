using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址：(參考/右鍵/加入參考/System.Drawing)
    https://blog.csdn.net/weixin_42953003/article/details/119751529
    https://www.mlplus.net/2020/04/04/csharpimagecompress/
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

            Console.WriteLine("完成");
            Console.Read();
        }
    }
}
