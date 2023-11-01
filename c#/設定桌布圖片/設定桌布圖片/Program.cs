using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.InteropServices.ComTypes;
using Microsoft.Win32;

/*
    參考網址： https://jojosula001.pixnet.net/blog/post/161458838-%5Bc%23%5D%E8%A8%AD%E5%AE%9Awindows%E6%A1%8C%E9%9D%A2%E8%83%8C%E6%99%AF%E5%9C%96%E7%89%87
 */

namespace 設定桌布圖片
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 一定要給 絕對路徑
            string basePath = System.AppDomain.CurrentDomain.BaseDirectory;
            SetTableCloth(basePath + "test.jpg");
        }

        // 設定桌布用
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern Int32 SystemParametersInfo(UInt32 uiAction, UInt32 uiParam, String pvParam, UInt32 fWinIni);

        /// <summary>
        ///  設定桌布
        /// </summary>
        /// <param name="filePath">要放置的圖片</param>
        public static void SetTableCloth(string filePath)
        {
            UInt32 SPI_SETDESKWALLPAPER = 20;
            UInt32 SPIF_UPDATEINIFILE = 2;

            if (File.Exists(filePath))
            {
                SystemParametersInfo(SPI_SETDESKWALLPAPER, 0, filePath, SPIF_UPDATEINIFILE);
            }
        }
    }
}
