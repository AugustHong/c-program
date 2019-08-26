using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/*
    參考網圵： http://blackcatj.blogspot.com/2017/04/caspnet-office-pdfodf.html
    步驟1：先去NuGet裝上 Microsoft.Office.Interop 的 Word、Excel、PowerPoint
    步驟2：再去NuGet裝上 Micorsoft.Office.Core
     */

namespace OfficeToPDForODF
{
    class Program
    {
        static void Main(string[] args)
        {
            string sourcePath = "";  //來源的檔案路徑
            string targetPath = "";  //目標的檔案路徑
        }


        //依據他的副檔名，轉去其相關的函式
        public void SaveToPDFOrOD(string sourcePath, string targetPath)
        {
            //副檔名
            string extension = sourcePath.Split('.').Last();

            switch (extension)
            {
                case "doc":
                    WordToOD(sourcePath, targetPath);
                    break;
                case "docx":
                    WordToOD(sourcePath, targetPath);
                    break;
                case "xls":
                    ExcelToOD(sourcePath, targetPath);
                    break;
                case "xlsx":
                    ExcelToOD(sourcePath, targetPath);
                    break;
                case "ppt":
                    PowerPointToOD(sourcePath, targetPath);
                    break;
                case "pptx":
                    PowerPointToOD(sourcePath, targetPath);
                    break;
            }
        }

        //Word轉PDF和ODT
        public void WordToOD(string sourcePath, string targetPath)
        {
            Microsoft.Office.Interop.Word.Application wordApp = new Microsoft.Office.Interop.Word.Application();
            Microsoft.Office.Interop.Word.Document doc = wordApp.Documents.Open(sourcePath);

            string pdfPath = targetPath + ".pdf";
            string odtPath = targetPath + ".odt";

            try
            {
                doc.SaveAs2(odtPath, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatOpenDocumentText);
                doc.SaveAs2(pdfPath, Microsoft.Office.Interop.Word.WdSaveFormat.wdFormatPDF);
                wordApp.Visible = false;
                wordApp.Quit();
            }catch(Exception ex)
            {
                wordApp.Quit();
                throw ex;
            }
        }

        //Excel轉PDF和ODS
        public void ExcelToOD(string sourcePath, string targetPath)
        {
            Microsoft.Office.Interop.Excel.Application excelApp = new Microsoft.Office.Interop.Excel.Application();
            Microsoft.Office.Interop.Excel.Workbook book = excelApp.Workbooks.Open(sourcePath);
            Microsoft.Office.Interop.Excel.XlFileFormat xlFormatPDF = (Microsoft.Office.Interop.Excel.XlFileFormat)57;

            string pdfPath = targetPath + ".pdf";
            string odsPath = targetPath + ".ods";

            try
            {
                book.SaveAs(odsPath, Microsoft.Office.Interop.Excel.XlFileFormat.xlOpenDocumentSpreadsheet);
                book.SaveAs(pdfPath, xlFormatPDF);
                excelApp.Visible = false;
                excelApp.Quit();
            }catch(Exception ex)
            {
                excelApp.Quit();
                throw ex;
            }
        }

        //PowerPoint轉PDF和ODP
        public void PowerPointToOD(string sourcePath, string targetPath)
        {
            Microsoft.Office.Interop.PowerPoint.Application powerPointApp = new Microsoft.Office.Interop.PowerPoint.Application();
            Microsoft.Office.Interop.PowerPoint.Presentation presentation = powerPointApp.Presentations.Open(sourcePath, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse, Microsoft.Office.Core.MsoTriState.msoFalse);

            string pdfPath = targetPath + ".pdf";
            string odpPath = targetPath + ".odp";

            try
            {
                presentation.SaveAs(pdfPath, Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType.ppSaveAsPDF);
                presentation.SaveAs(odpPath, Microsoft.Office.Interop.PowerPoint.PpSaveAsFileType.ppSaveAsOpenDocumentPresentation);
                powerPointApp.Quit();
                System.Runtime.InteropServices.Marshal.FinalReleaseComObject(powerPointApp);
            }catch(Exception ex)
            {
                powerPointApp.Quit();
                throw ex;
            }
        }
    }
}
