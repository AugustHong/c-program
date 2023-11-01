using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using IWshRuntimeLibrary;
using System.Runtime.InteropServices.ComTypes;
using System.IO;

/*
    參考網址： https://topic.alibabacloud.com/tc/a/c--create-a-shortcut_1_31_32346218.html
               https://www.796t.com/content/1543063288.html
    參考/右鍵/加入參考/COM/Windows Script Host Object Model
 */

namespace 建立捷徑
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // 取到各特殊路徑
            string favoritesPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Favorites); // 我的最受
            string desktopPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Desktop); //桌面
            string startMenu = System.Environment.GetFolderPath(System.Environment.SpecialFolder.StartMenu); //開始功能表
            string myDoc = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments); //我的文件
            string programFiles = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFiles); //ProgramFiles
            string programFiles86 = System.Environment.GetFolderPath(System.Environment.SpecialFolder.ProgramFilesX86); //ProgramFilesX86

            // 建立3種捷徑 (txt的、exe的、Url的)
            string progBasePath = System.AppDomain.CurrentDomain.SetupInformation.ApplicationBase; //程式當前位置(因為應該要給絕對路徑，所以我的資料都放在跟此程式同位置)

            // Url 的
            string url = "http://www.google.com.tw";
            string txt = progBasePath + "要建立捷徑資料\\1.txt";
            string exe = progBasePath + "要建立捷徑資料\\1.exe";

            // 建立 捷徑
            WshShell shell = new WshShell();

            //快捷鍵方式建立的位置、名稱
            string linkFileName = desktopPath + "\\testTxt.lnk";
            if (System.IO.File.Exists(linkFileName))
            {
                System.IO.File.Delete(linkFileName);
            }
            IWshShortcut shortcut = (IWshShortcut)shell.CreateShortcut(linkFileName);
            shortcut.TargetPath = txt; //目標檔案 (給絕對路徑)                                   
            shortcut.WorkingDirectory = System.Environment.CurrentDirectory; //該屬性指定應用程式的工作目錄，當用戶沒有指定一個具體的目錄時，快捷方式的目標應用程式將使用該屬性所指定的目錄來裝載或儲存檔案。
            shortcut.WindowStyle = 1; //目標應用程式的視窗狀態分為普通、最大化、最小化【1,3,7】
            shortcut.Description = "測試TXT捷徑"; //描述
            //shortcut.IconLocation = exePath + "\\logo.ico";  //快捷方式圖示
            shortcut.Arguments = "";
            //shortcut.Hotkey = "CTRL+ALT+F11"; // 快捷鍵
            shortcut.Save(); //必須呼叫儲存快捷才成建立成功

            //快捷鍵方式建立的位置、名稱
            linkFileName = desktopPath + "\\testUrl.lnk";
            if (System.IO.File.Exists(linkFileName))
            {
                System.IO.File.Delete(linkFileName);
            }
            IWshShortcut shortcut2 = (IWshShortcut)shell.CreateShortcut(linkFileName);
            shortcut2.TargetPath = url; //目標檔案 (給絕對路徑)                                   
            shortcut2.WorkingDirectory = System.Environment.CurrentDirectory; //該屬性指定應用程式的工作目錄，當用戶沒有指定一個具體的目錄時，快捷方式的目標應用程式將使用該屬性所指定的目錄來裝載或儲存檔案。
            shortcut2.WindowStyle = 1; //目標應用程式的視窗狀態分為普通、最大化、最小化【1,3,7】
            shortcut2.Description = "測試URL捷徑"; //描述
            //shortcut2.IconLocation = exePath + "\\logo.ico";  //快捷方式圖示
            shortcut2.Arguments = "";
            //shortcut2.Hotkey = "CTRL+ALT+F11"; // 快捷鍵
            shortcut2.Save(); //必須呼叫儲存快捷才成建立成功

            //快捷鍵方式建立的位置、名稱
            linkFileName = desktopPath + "\\testExe.lnk";
            if (System.IO.File.Exists(linkFileName))
            {
                System.IO.File.Delete(linkFileName);
            }
            IWshShortcut shortcut3 = (IWshShortcut)shell.CreateShortcut(linkFileName);
            shortcut3.TargetPath = exe; //目標檔案 (給絕對路徑)                                   
            shortcut3.WorkingDirectory = System.Environment.CurrentDirectory; //該屬性指定應用程式的工作目錄，當用戶沒有指定一個具體的目錄時，快捷方式的目標應用程式將使用該屬性所指定的目錄來裝載或儲存檔案。
            shortcut3.WindowStyle = 1; //目標應用程式的視窗狀態分為普通、最大化、最小化【1,3,7】
            shortcut3.Description = "測試EXE捷徑"; //描述
            //shortcut3.IconLocation = exePath + "\\logo.ico";  //快捷方式圖示
            shortcut3.Arguments = "";
            //shortcut3.Hotkey = "CTRL+ALT+F11"; // 快捷鍵
            shortcut3.Save(); //必須呼叫儲存快捷才成建立成功

            Console.Read();
        }
    }
}
