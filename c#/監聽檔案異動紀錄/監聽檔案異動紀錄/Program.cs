using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址： https://dotblogs.com.tw/kyleshen/2013/11/29/131980
               https://learn.microsoft.com/zh-tw/dotnet/api/system.io.filesystemwatcher?view=net-7.0
 */

namespace 監聽檔案異動紀錄
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string watchPath = "";  // 要監聽的路徑(絕對路徑)
            string watchFilter = "";  //要篩選的條件

            Console.Write("請輸入要監聽的路徑(絕對路徑)： ");
            watchPath = Console.ReadLine();
            Console.Write("請輸入要篩選的條件(全部： *.*  , 篩選 txt： *.txt  )：");
            watchFilter = Console.ReadLine();

            // 開始執行監聽
            var watcher = new FileSystemWatcher(watchPath);

            // 監聽的項目
            watcher.NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size;

            watcher.Filter = watchFilter;  //篩選條件
            watcher.IncludeSubdirectories = true; //是否連子資料夾都要偵測
            watcher.EnableRaisingEvents = true; ////開啟監聽

            // 設置各項處置
            watcher.Changed += OnChanged;
            watcher.Created += OnCreated;
            watcher.Deleted += OnDeleted;
            watcher.Renamed += OnRenamed;
            watcher.Error += OnError;

            Console.WriteLine("===============================開始執行監聽======================================");

            Console.WriteLine("請按下 Enter 以結束程式");
            Console.ReadLine();
        }

        // 取得是檔案還是資料夾
        public static string IsFIleOrDir(string path)
        {
            string result = "檔案";

            try
            {
                DirectoryInfo d = new DirectoryInfo(path);
                if (string.IsNullOrEmpty(d.Extension))
                {
                    result = "資料夾";
                }
            }
            catch
            {
                result = "無效(路徑不存在)";
            }

            return result;
        }

        public static void OnChanged(object sender, FileSystemEventArgs e)
        {
            if (e.ChangeType != WatcherChangeTypes.Changed)
            {
                return;
            }
            Console.WriteLine($"Changed {IsFIleOrDir(e.FullPath)}: {e.FullPath}");
        }

        private static void OnCreated(object sender, FileSystemEventArgs e)
        {
            string value = $"Created {IsFIleOrDir(e.FullPath)}: {e.FullPath}";
            Console.WriteLine(value);
        }

        private static void OnDeleted(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine($"Deleted {IsFIleOrDir(e.FullPath)}: {e.FullPath}");
        }         

        private static void OnRenamed(object sender, RenamedEventArgs e)
        {
            Console.WriteLine($"Renamed {IsFIleOrDir(e.FullPath)}:  Old: {e.OldFullPath}  New: {e.FullPath}");
        }

        private static void OnError(object sender, ErrorEventArgs e)
        {
            Exception ex = e.GetException();
            Console.WriteLine($"Error:  {ex.Message}");
        }
    }
}
