using GroupDocs.Parser.Data;
using GroupDocs.Parser;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址： https://blog.groupdocs.com/zh-hant/parser/scan-qr-code-using-csharp/
    先去 NuGet 裝上 GroupDocs.Parser
    缺點：掃描會跑有點久喔！ 且 不確定非付費版有沒有限制
 */

namespace 掃描二維條碼
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 使用 C# 掃描二維碼
            string filePath = "urlTest.png";

            Console.WriteLine($"=======================傳入檔案： {filePath}========================");

            foreach (PageBarcodeArea barcode in GetAllBarcode(filePath))
            {
                Console.WriteLine($"條碼類型 = {barcode.CodeTypeName} , 值 = {barcode.Value}");
            }

            filePath = "textTest.png";

            Console.WriteLine($"=======================傳入檔案： {filePath}========================");

            foreach (PageBarcodeArea barcode in GetAllBarcode(filePath))
            {
                Console.WriteLine($"條碼類型 = {barcode.CodeTypeName} , 值 = {barcode.Value}");
            }

            filePath = "測試.docx";

            Console.WriteLine($"=======================傳入檔案： {filePath}========================");

            foreach (PageBarcodeArea barcode in GetAllBarcode(filePath))
            {
                Console.WriteLine($"條碼類型 = {barcode.CodeTypeName} , 值 = {barcode.Value}");
            }

            Console.ReadLine();
        }

        /// <summary>
        /// 取得 所有條碼
        /// </summary>
        /// <param name="filePath">檔案路徑</param>
        /// <returns></returns>
        public static List<PageBarcodeArea> GetAllBarcode(string filePath) 
        {
            List<PageBarcodeArea> barcodes = null;

            using (Parser parser = new Parser(filePath))
            {
                // 取得所有條碼
                barcodes = parser.GetBarcodes().ToList();
            }

            return barcodes;
        }
    }
}
