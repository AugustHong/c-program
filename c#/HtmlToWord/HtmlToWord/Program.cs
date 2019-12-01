using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.Office.Interop.Word;

/*
    參考網圵 (有 html 轉 word 和 word to html，但這邊只講第一個)：
    https://www.itread01.com/article/1511489115.html
 */

namespace HtmlToWord
{
    class Program
    {
        static void Main(string[] args)
        {
            string html = "<table border=\"1\" cellspacing=\"0\" cellpadding=\"10\" style='width:100%;'>";
            html += "<tr><td rowspan='2' style='width:30%;'>這是111</td><td colspan='2' style='width:60%;'>This is 222</td></tr>";
            html += "<tr><td style='width:30%;'>333</td><td style='width:30%;'>444</td></tr>";
            html += "</table>";

            // 開始轉 word
            StringBuilder sb = new StringBuilder();
            sb.Append(
            "<html xmlns:v=\"urn:schemas-microsoft-com:vml\" xmlns:o=\"urn:schemas-microsoft-com:office:office\" xmlns:w=\"urn:schemas-microsoft-com:office:word\" xmlns:m=\"http://schemas.microsoft.com/office/2004/12/omml\"xmlns = \"http://www.w3.org/TR/REC-html40\">");
            sb.Append(html);
            sb.Append("</html>");

            string path = System.IO.Directory.GetCurrentDirectory() + "\\";

            // 如果要給使用者下載 
            // 目前只能 doc 不能用 docx
            // return File(Encoding.Default.GetBytes(sb.ToString()), "application/msword", path + "測試.doc");

            // 如果寫檔的話
            try
            {
                byte[] fileBytes = Encoding.Default.GetBytes(sb.ToString());
                // 目前只能 doc 不能用 docx
                using (Stream file = File.OpenWrite(path + "測試.doc"))
                {
                    file.Write(fileBytes, 0, fileBytes.Length);
                }
               
                Console.WriteLine("產生Word完成");

                // 因為只能產生 doc 檔 => 故自行轉檔
                // 參考網圵： https://social.msdn.microsoft.com/Forums/vstudio/en-US/c26e1bf4-2e01-45a1-a00c-0fefc8cf7e88/how-to-convert-doc-file-to-docx-file-using-c?forum=worddev
                // 裝上 Microsoft.Office.Interop.Word ( 詳細可看我的 WriteWord 那隻)
                while (!File.Exists(path + "測試.doc"))
                {
                    // 確保 產生完檔案了
                }
                Application wordApp = new Application() { Visible = false };
                // 這裡路徑要給明確，不然他吃不到
                Document doc = wordApp.Documents.Open(path + "測試.doc");
                doc.SaveAs2(path + "測試.docx", WdSaveFormat.wdFormatDocumentDefault);
                Console.WriteLine("轉成DOCX");
                doc.Close();
                wordApp.Quit();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }


            // 注： 產生 odt 看來是正常 (但別隻用的時候，卻轉成亂碼)
          
            Console.Read();
        }
    }
}
