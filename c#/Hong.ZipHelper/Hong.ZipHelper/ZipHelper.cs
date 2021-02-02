using Ionic.Zip;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

/*
    去 NuGet 裝上 DotNetZip
 */

namespace Hong.ZipHelper
{
    public static class ZipHelper
    {
        /// <summary>
        /// 加密資料夾
        /// </summary>
        /// <param name="directoryPath">目標實體路徑資料夾</param>
        /// <param name="OutDir">匯出實體路徑資料夾 </param>
        /// <returns></returns>
        public static (bool, string) ZipFile(string directoryPath, string outDir, string zipPassword = "", bool isDeleteFile = false)
        {
            bool result = false;

            Guid guidName = Guid.NewGuid();
            string today = DateTime.Now.ToString("yyyyMMddHH");
            string newPath = outDir.TrimEnd('/').TrimEnd('\\') + "//" + guidName + "_" + today + ".zip";

            if (Directory.Exists(directoryPath))
            {
                bool usePassword = !string.IsNullOrWhiteSpace(zipPassword);  // 是否使用 密碼
                using (ZipFile zip = new ZipFile(newPath))
                {
                    if (usePassword)
                    {
                        zip.Password = zipPassword;
                    }

                    zip.AddDirectory(directoryPath);
                    zip.Save();
                }

                if (isDeleteFile)
                {
                    Directory.Delete(directoryPath, true);
                }

                result = true;
            }

            return (result, newPath);
        }

        /// <summary>
        /// 解ZIP  檔案
        /// </summary>
        /// <param name="zipFileName">ZIP 檔案實體路徑</param>
        /// <param name="directoryPath">解壓實體路徑</param>
        /// <returns></returns>
        public static bool OpenZipFile(string zipFileName, string directoryFileName, string zipPassword = "", bool isDeleteFile = false)
        {
            bool result = false;
            using (var zip = Ionic.Zip.ZipFile.Read(zipFileName))
            {
                try
                {
                    bool usePassword = !string.IsNullOrWhiteSpace(zipPassword);  // 是否使用 密碼

                    // 解壓縮
                    foreach (var zipEntry in zip)
                    {
                        if (usePassword)
                        {
                            zip.Password = zipPassword;
                        }

                        zipEntry.Extract(directoryFileName, ExtractExistingFileAction.OverwriteSilently);
                    }

                    result = true;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                    result = false;
                }
            }

            // 要解壓縮成功才刪除
            if (result && isDeleteFile)
            {
                File.Delete(zipFileName);
            }

            return result;
        }

        /// <summary>
        /// 把 Zip 轉為 Byte[]
        /// </summary>
        /// <param name="zipFileName">ZIP 檔案實體路徑</param>
        /// <param name="isDeleteFile">是否刪掉檔案</param>
        /// <returns></returns>
        public static byte[] ZipToBytes(string zipFileName, bool isDeleteFile = false)
        {
            try
            {
                if (string.IsNullOrEmpty(zipFileName) || !File.Exists(zipFileName))
                {
                    return new byte[0];
                }

                FileStream zipFile = File.Open(zipFileName, FileMode.Open, FileAccess.Read);
                BinaryReader zipReader = new BinaryReader(zipFile);
                int zipLen = Convert.ToInt32(zipFile.Length);
                byte[] zipByte = zipReader.ReadBytes(zipLen);
                zipReader.Close();
                zipFile.Close();

                if (isDeleteFile)
                {
                    File.Delete(zipFileName);
                }

                return zipByte;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new byte[0];
            }
        }
    }
}
