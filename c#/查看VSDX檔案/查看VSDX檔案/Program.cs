using GroupDocs.Viewer.Options;
using GroupDocs.Viewer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址：https://products.groupdocs.com/zh-hant/viewer/net/vsdx/
              https://www.nuget.org/packages/groupdocs.viewer

    請去 NuGet 裝上 GroupDocs.Viewer
 */

namespace 查看VSDX檔案
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string inputFileName = "test.vsdx";

            /*
                目前發現 VSDX 有多個流程圖，還是只會變成1張 或 2張。
                且上面都會有浮水印
            */


            // 存成 html
            string goalHtmlFileName = "test_{0}.html";
            using (Viewer viewer = new Viewer(inputFileName))
            {
                HtmlViewOptions viewOptions = HtmlViewOptions.ForEmbeddedResources(goalHtmlFileName);
                viewOptions.CadOptions.RenderLayouts = true;
                viewer.View(viewOptions);
            }
            Console.WriteLine("製作成 html 完成");

            //存成 pdf
            string goalPdfFileName = "test.pdf";
            using (Viewer viewer = new Viewer(inputFileName))
            {
                PdfViewOptions options = new PdfViewOptions(goalPdfFileName);
                viewer.View(options);
            }
            Console.WriteLine("製作成 pdf 完成");

            //存成 png
            string goalPngFileName = "test_{0}.png";
            using (Viewer viewer = new Viewer(inputFileName))
            {
                PngViewOptions options = new PngViewOptions(goalPngFileName);
                viewer.View(options);
            }
            Console.WriteLine("製作成 png 完成");

            /*
                額外附註： Word 轉成需要密碼的 PDF
             */
            /*
             string filePath = "output.pdf";
using (Viewer viewer = new Viewer("test.docx"))
{
    // set PDF file security
    Security security = new Security();
    security.DocumentOpenPassword = "123";
    security.PermissionsPassword = "123";
    security.Permissions = Permissions.AllowAll ^ Permissions.DenyPrinting;

    PdfViewOptions options = new PdfViewOptions(filePath);
    options.Security = security;

    viewer.View(options);
}
             */
        }
    }
}

 