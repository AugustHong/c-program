using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// QRCode 請去 NuGet 裝上 QRCoder
// 參考網圵： http://dog0416.blogspot.com/2020/04/aspnet-core-qrcoder-qr-code.html
//           https://github.com/codebude/QRCoder/wiki/Advanced-usage---QR-Code-renderers
using QRCoder;

// BarCode 請去 NuGet 裝上 BarcodeLib
// 參考網圵： https://www.cjavapy.com/article/786/
using BarcodeLib;

// 都要使用到 System.Drawing (參考 / 加入參考 / System.Drawing)
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace 產生條碼
{
    class Program
    {
        static void Main(string[] args)
        {
            string rootPath = Directory.GetCurrentDirectory() + "\\";

            // QR Code
            CodeHelper.CreateQRCodeAndSave("這是第一個QRCode", ImageFormat.Jpeg, "firstQRCode.jpg");
            CodeHelper.CreateQRCodeAndSave("這是特別設的QRCode", ImageFormat.Png, "secondQRCode.png", Color.Red, Color.White, 40);

            // Bar Code (輸入字 只能 英文+數字，且要符合 CODE39 的格式) => 我有寫驗證的函式
            CodeHelper.CreateBarCodeAndSave("Y20150513256", ImageFormat.Jpeg, "firstBarCode.jpg");
            CodeHelper.CreateBarCodeAndSave("E20110202112", ImageFormat.Png, "secondBarCode.png", Color.Red, Color.Black, new Font("Verdana", 30), height: 200, width: 300);

            // 驗證 Bar Code 格式
            Console.WriteLine(CodeHelper.IsBarCodeType("CC0110202112"));
            Console.WriteLine(CodeHelper.IsBarCodeType("This is Bar"));

            Console.WriteLine("執行完成");
            Console.ReadLine();
        }
    }

    /// <summary>
    /// 處理 BarCode 和 QRCode
    /// </summary>
    public static class CodeHelper
    {
        /// <summary>
        ///  Image 轉 Byte
        /// </summary>
        /// <param name="image">圖片</param>
        /// <param name="saveType">儲存類型</param>
        /// <returns></returns>
        public static byte[] ImageToByte(Image image, ImageFormat imageFormat)
        {
            byte[] result = new byte[0];

            if (image != null && imageFormat != null)
            {
                using (MemoryStream oMemoryStream = new MemoryStream())
                {
                    //建立副本
                    using (Bitmap oBitmap = new Bitmap(image))
                    {
                        //儲存圖片到 MemoryStream 物件，並且指定儲存影像之格式
                        oBitmap.Save(oMemoryStream, imageFormat);

                        //設定資料流位置
                        oMemoryStream.Position = 0;

                        //設定 buffer 長度
                        result = new byte[oMemoryStream.Length];

                        //將資料寫入 buffer
                        oMemoryStream.Read(result, 0, Convert.ToInt32(oMemoryStream.Length));

                        //將所有緩衝區的資料寫入資料流
                        oMemoryStream.Flush();
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///  儲存 Image
        /// </summary>
        /// <param name="filePath">儲存路徑(完整路徑)</param>
        /// <param name="image">圖片</param>
        /// <param name="saveType">儲存類型</param>
        /// <returns></returns>
        public static bool SaveImage(string filePath, Image image, ImageFormat imageFormat)
        {
            bool result = false;

            if (image != null && imageFormat != null)
            {
                try
                {
                    // 如果已存在 => 刪掉
                    if (File.Exists(filePath))
                    {
                        File.Delete(filePath);
                    }

                    image.Save(filePath, imageFormat);
                    result = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    result = false;
                }
            }

            return result;
        }

        /// <summary>
        /// 產生 QR Code
        /// </summary>
        /// <param name="data">要轉成QR Code的文字</param>
        /// <param name="lineColor">線條顏色</param>
        /// <param name="backgroundColor">背景顏色</param>
        /// <param name="size">像素大小(預設 30)</param>
        /// <param name="eCCLevel">容錯率(預設 Q = 25%)</param>
        /// <param name="forceUtf8">是否強制Utf8編碼</param>
        /// <param name="utf8BOM">是否為Utf8 BOM編碼</param>
        /// <returns></returns>
        public static Image CreateQRCode(string data, Color lineColor, Color backgroundColor, int size = 30, QRCodeGenerator.ECCLevel eCCLevel = QRCodeGenerator.ECCLevel.Q, bool forceUtf8 = true, bool utf8BOM = true)
        {
            // QRCodeGenerator.ECCLevel 為 QR Code 的修正等： L 為 7%、 M 為 15%、 Q 為 25%  或 H 為30%
            //Bitmap qrCodeImage = QRCodeHelper.GetQRCode(要轉成QR Code的文字, 像素大小, QR Code線條顏色, QR Code背景顏色, 容錯率, 是否強制Utf8編碼, 是否為Utf8 BOM編碼);
            return QRCodeHelper.GetQRCode(data, size, lineColor, backgroundColor, eCCLevel, forceUtf8, utf8BOM);
        }

        /// <summary>
        /// 產生 QR Code
        /// </summary>
        /// <param name="data">要轉成QR Code的文字</param>
        /// <param name="imageFormat">存成的格式(因為轉byte，會跟你的類型有關)</param>
        /// <param name="lineColor">線條顏色</param>
        /// <param name="backgroundColor">背景顏色</param>
        /// <param name="size">像素大小(預設 30)</param>
        /// <param name="eCCLevel">容錯率(預設 Q = 25%)</param>
        /// <param name="forceUtf8">是否強制Utf8編碼</param>
        /// <param name="utf8BOM">是否為Utf8 BOM編碼</param>
        /// <returns></returns>
        public static byte[] CreateQRCodeBytes(string data, ImageFormat imageFormat, Color lineColor, Color backgroundColor, int size = 30, QRCodeGenerator.ECCLevel eCCLevel = QRCodeGenerator.ECCLevel.Q, bool forceUtf8 = true, bool utf8BOM = true)
        {
            return ImageToByte(CreateQRCode(data, lineColor, backgroundColor, size, eCCLevel, forceUtf8, utf8BOM), imageFormat);
        }

        /// <summary>
        /// 產生 QR Code 並儲存
        /// </summary>
        /// <param name="data">要轉成QR Code的文字</param>
        /// <param name="imageFormat">存成的格式(因為轉byte，會跟你的類型有關)</param>
        /// <param name="filePath">儲存檔案路徑(完整路徑)</param>
        /// <param name="lineColor">線條顏色</param>
        /// <param name="backgroundColor">背景顏色</param>
        /// <param name="size">像素大小(預設 30)</param>
        /// <param name="eCCLevel">容錯率(預設 Q = 25%)</param>
        /// <param name="forceUtf8">是否強制Utf8編碼</param>
        /// <param name="utf8BOM">是否為Utf8 BOM編碼</param>
        /// <returns></returns>
        public static (Image, bool) CreateQRCodeAndSave(string data, ImageFormat imageFormat, string filePath, Color lineColor, Color backgroundColor, int size = 30, QRCodeGenerator.ECCLevel eCCLevel = QRCodeGenerator.ECCLevel.Q, bool forceUtf8 = true, bool utf8BOM = true)
        {
            Image result = CreateQRCode(data, lineColor, backgroundColor, size, eCCLevel, forceUtf8, utf8BOM);
            bool success = SaveImage(filePath, result, imageFormat);
            return (result, success);
        }

        /// <summary>
        /// 用上面的方法 (因為 Color 預設沒辦法給，所有在這邊給)
        /// </summary>
        /// <param name="data">要轉成QR Code的文字</param>
        /// <returns></returns>
        public static Image CreateQRCode(string data)
        {
            return CreateQRCode(data, Color.Black, Color.White);
        }

        /// <summary>
        /// 用上面的方法 (因為 Color 預設沒辦法給，所有在這邊給)
        /// </summary>
        /// <param name="data">要轉成QR Code的文字</param>
        /// <param name="imageFormat">存成的格式(因為轉byte，會跟你的類型有關)</param>
        /// <returns></returns>
        public static byte[] CreateQRCodeBytes(string data, ImageFormat imageFormat)
        {
            return ImageToByte(CreateQRCode(data), imageFormat);
        }

        /// <summary>
        /// 產生 QR Code 並儲存
        /// </summary>
        /// <param name="data">要轉成QR Code的文字</param>
        /// <param name="imageFormat">存成的格式(因為轉byte，會跟你的類型有關)</param>
        /// <param name="filePath">儲存檔案路徑(完整路徑)</param>
        /// <returns></returns>
        public static (Image, bool) CreateQRCodeAndSave(string data, ImageFormat imageFormat, string filePath)
        {
            return CreateQRCodeAndSave(data, imageFormat, filePath, Color.Black, Color.White);
        }

        /*
            QRCode 正常寫法：

            1. 轉 圖檔
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                // data 為掃描出來後想呈現的資料
                // QRCodeGenerator.ECCLevel 為 QR Code 的修正等： L 為 7%、 M 為 15%、 Q 為 25%  或 H 為30%。
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);
                QRCode qrCode = new QRCode(qrCodeData);
                // 取得QR Code圖檔
                Bitmap qrCodeImage = qrCode.GetGraphic(30);

            2. 轉 byte
                QRCodeGenerator qrGenerator = new QRCodeGenerator();
                // data 為掃描出來後想呈現的資料
                // QRCodeGenerator.ECCLevel 為 QR Code 的修正等： L 為 7%、 M 為 15%、 Q 為 25%  或 H 為30%。
                QRCodeData qrCodeData = qrGenerator.CreateQrCode(data, QRCodeGenerator.ECCLevel.Q);

                // 取得QR Code 二進位資料
                BitmapByteQRCode qrCode = new BitmapByteQRCode(qrCodeData);
                byte[] qrCodeAsBitmapByteArr = qrCode.GetGraphic(30);
         */

        /// <summary>
        ///  確認 BarCode 格式是否正確
        /// </summary>
        /// <param name="data"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsBarCodeType(string data, TYPE type = TYPE.CODE39)
        {
            bool result = false;

            using (Barcode barcode = new Barcode())
            {
                try
                {
                    barcode.Encode(type, data, Color.Black, Color.White, 290, 120);
                    result = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }

            return result;
        }

        /// <summary>
        /// 因為要用 using (所以把主邏輯抽出去)
        /// </summary>
        /// <param name="barcode">傳入的 物件</param>
        /// <param name="data">文字</param>
        /// <param name="lineColor">線條顏色</param>
        /// <param name="backgroundColor">背景顏色</param>
        /// <param name="font">字體</param>
        /// <param name="alignment">對齊方式(預設 置中)</param>
        /// <param name="height">高度(預設 120)</param>
        /// <param name="width">寬度(預設 290)</param>
        /// <param name="type">條碼種類(預設 Code39)</param>
        /// <param name="IncludeLabel">是否要顯示Label (預設 true)</param>
        /// <returns></returns>
        private static Image BarCodeHandler(Barcode barcode, string data, Color lineColor, Color backgroundColor, Font font, AlignmentPositions alignment = AlignmentPositions.CENTER, int height = 120, int width = 290, TYPE type = TYPE.CODE39, bool IncludeLabel = true)
        {
            Image result = null;

            // 主邏輯
            barcode.IncludeLabel = IncludeLabel; //顯示文字標籤
            barcode.LabelFont = font;//文字標籤字型、大小
            barcode.Alignment = alignment;  // 文字對齊

            try
            {
                // b.Encode(條碼種類, 內容, 條碼色, 背景色, 寬度, 高度)
                result = barcode.Encode(type, data, lineColor, backgroundColor, width, height);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return result;
        }

        /// <summary>
        /// 產生BarCode
        /// </summary>
        /// <param name="data">文字</param>
        /// <param name="lineColor">線條顏色</param>
        /// <param name="backgroundColor">背景顏色</param>
        /// <param name="font">字體</param>
        /// <param name="alignment">對齊方式(預設 置中)</param>
        /// <param name="height">高度(預設 120)</param>
        /// <param name="width">寬度(預設 290)</param>
        /// <param name="type">條碼種類(預設 Code39)</param>
        /// <param name="IncludeLabel">是否要顯示Label (預設 true)</param>
        /// <returns></returns>
        public static Image CreateBarCode(string data, Color lineColor, Color backgroundColor, Font font, AlignmentPositions alignment = AlignmentPositions.CENTER, int height = 120, int width = 290, TYPE type = TYPE.CODE39, bool IncludeLabel = true)
        {
            Image result = null;
            using (Barcode barcode = new Barcode())
            {
                // 主邏邏
                result = BarCodeHandler(barcode, data, lineColor, backgroundColor, font, alignment, height, width, type, IncludeLabel);
            }

            return result;
        }

        /// <summary>
        /// 產生BarCode
        /// </summary>
        /// <param name="data">文字</param>
        /// <param name="lineColor">線條顏色</param>
        /// <param name="backgroundColor">背景顏色</param>
        /// <param name="font">字體</param>
        /// <param name="imageFormat">存成的格式(因為轉byte，會跟你的類型有關)</param>
        /// <param name="alignment">對齊方式(預設 置中)</param>
        /// <param name="height">高度(預設 120)</param>
        /// <param name="width">寬度(預設 290)</param>
        /// <param name="type">條碼種類(預設 39)</param>
        /// <param name="IncludeLabel">是否要顯示Label (預設 true)</param>
        /// <returns></returns>
        public static byte[] CreateBarCodeBytes(string data, ImageFormat imageFormat, Color lineColor, Color backgroundColor, Font font, AlignmentPositions alignment = AlignmentPositions.CENTER, int height = 120, int width = 290, TYPE type = TYPE.CODE39, bool IncludeLabel = true)
        {
            byte[] b = new byte[0];
            using (Barcode barcode = new Barcode())
            {
                // 主邏邏
                Image result = BarCodeHandler(barcode, data, lineColor, backgroundColor, font, alignment, height, width, type, IncludeLabel);
                if (result != null)
                {
                    b = ImageToByte(result, imageFormat);
                }
            }
            
            return b;
        }

        /// <summary>
        /// 產生BarCode 並儲存
        /// </summary>
        /// <param name="data">文字</param>
        /// <param name="lineColor">線條顏色</param>
        /// <param name="backgroundColor">背景顏色</param>
        /// <param name="font">字體</param>
        /// <param name="imageFormat">存成的格式(因為轉byte，會跟你的類型有關)</param>
        /// <param name="filePath">儲存檔案路徑(完整路徑)</param>
        /// <param name="alignment">對齊方式(預設 置中)</param>
        /// <param name="height">高度(預設 120)</param>
        /// <param name="width">寬度(預設 290)</param>
        /// <param name="type">條碼種類(預設 39)</param>
        /// <param name="IncludeLabel">是否要顯示Label (預設 true)</param>
        /// <returns></returns>
        public static (Image, bool) CreateBarCodeAndSave(string data, ImageFormat imageFormat, string filePath, Color lineColor, Color backgroundColor, Font font, AlignmentPositions alignment = AlignmentPositions.CENTER, int height = 120, int width = 290, TYPE type = TYPE.CODE39, bool IncludeLabel = true)
        {
            bool success = false;
            Image result;

            using (Barcode barcode = new Barcode())
            {
                // 主邏邏
                result = BarCodeHandler(barcode, data, lineColor, backgroundColor, font, alignment, height, width, type, IncludeLabel);
                if (result != null)
                {
                    success = SaveImage(filePath, result, imageFormat);
                }
            }        
            
            return (result, success);
        }

        /// <summary>
        /// 用上面的方法 (因為 Color 預設沒辦法給，所有在這邊給)
        /// </summary>
        /// <param name="data">文字</param>
        /// <returns></returns>
        public static Image CreateBarCode(string data)
        {
            return CreateBarCode(data, Color.Black, Color.White, new Font("Verdana", 40));
        }

        /// <summary>
        /// 用上面的方法 (因為 Color 預設沒辦法給，所有在這邊給)
        /// </summary>
        /// <param name="data">文字</param>
        /// <param name="imageFormat">存成的格式(因為轉byte，會跟你的類型有關)</param>
        /// <returns></returns>
        public static byte[] CreateBarCodeBytes(string data, ImageFormat imageFormat)
        {
            return CreateBarCodeBytes(data, imageFormat, Color.Black, Color.White, new Font("Verdana", 40));
        }

        /// <summary>
        /// 用上面的方法 (因為 Color 預設沒辦法給，所有在這邊給)
        /// </summary>
        /// <param name="data">文字</param>
        /// <param name="imageFormat">存成的格式(因為轉byte，會跟你的類型有關)</param>
        /// <param name="filePath">儲存檔案路徑(完整路徑)</param>
        /// <returns></returns>
        public static (Image, bool) CreateBarCodeAndSave(string data, ImageFormat imageFormat, string filePath)
        {
            return CreateBarCodeAndSave(data, imageFormat, filePath, Color.Black, Color.White, new Font("Verdana", 40));
        }
    }
}
