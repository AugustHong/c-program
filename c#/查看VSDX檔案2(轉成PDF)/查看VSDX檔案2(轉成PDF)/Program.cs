using Aspose.Diagram;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址： https://products.aspose.com/diagram/zh-hant/net/conversion/vsd-to-pdf/

    去 Nuget 裝上 Aspose.Diagram
 */

namespace 查看VSDX檔案2_轉成PDF_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
                優點： 沒頁數限制
                缺點： 跑有點慢、會有浮水印
             */

            try
            {
                var visio = new Diagram("test.vsdx");
                visio.Save("output.pdf", SaveFileFormat.PDF);
            }
            catch(Exception ex)
            {
                Console.WriteLine("發生錯誤： " + ex.Message);
            }

            // 切成多個 vsdx
            Diagram diagram = new Diagram("test.vsdx");

            foreach (Page page in diagram.Pages)
            {
                Diagram dia = new Diagram();
                dia.Pages[0].Copy(page);
                dia.Save("test_p_" + page.Name + ".vsdx", SaveFileFormat.VSDX);
            }
        }
    }
}
