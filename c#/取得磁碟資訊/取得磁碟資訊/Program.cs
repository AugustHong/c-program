using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址： https://dotblogs.com.tw/chou/2011/05/31/26665
 */

namespace 取得磁碟資訊
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 取得 本機上所有磁碟
            List<DriveInfo> driveList = DriveInfo.GetDrives().ToList();

            // 取到 C 槽
            DriveInfo drive = new DriveInfo("C:\\");

            // 跑過所有磁碟得到資料
            int gb = 1024 * 1024 * 1024;
            foreach (DriveInfo d in driveList) 
            {
                Console.WriteLine($"磁碟是否準備好 = {d.IsReady}");
                Console.WriteLine($"標籤 = {d.VolumeLabel}");
                Console.WriteLine($"名稱 = {d.Name}");
                Console.WriteLine($"可用空間 = {d.AvailableFreeSpace / gb} GB");
                Console.WriteLine($"可用大小 = {d.TotalSize / gb} GB");
                Console.WriteLine($"可用總空間 = {d.TotalFreeSpace / gb} GB");
                Console.WriteLine($"格式 = {d.DriveFormat}");  //NTFS 或 FAT32
                Console.WriteLine($"型態 = {d.DriveType}");
                Console.WriteLine($"根目錄 = {d.RootDirectory}");
                Console.WriteLine("---------------------------------------------------------------");
            }

            Console.ReadLine();
        }
    }
}
