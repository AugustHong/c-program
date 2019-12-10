using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Web;

/*
    參考網圵： https://blog.darkthread.net/blog/mimemapping-getmimemapping/ 

    步驟1： 去 參考 -> 加入參考 -> System.Web;
    步驟2： using System.Web;
*/

namespace 用檔名得到ConnectType
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> fileNameList = new List<string> { "a.jpg", "b.png", "c.gif", "d.tiff", "e.doc", "f.docx", "g.odt", "h.xls", "i.xlsx", "j.ods", "k.ppt", "l.pptx", "m.pdf", "n.html", "o.js", "p.css", "q.cpp", "r.bat" };

            foreach (var fileName in fileNameList)
            {
                try
                {
                    var connectType = MimeMapping.GetMimeMapping(fileName);
                    var type = fileName.Split('.')[1];
                    Console.WriteLine($"副檔名 {type}  其 ConnectType = {connectType} ");
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    continue;
                }
            }

            Console.Read();
        }
    }
}
