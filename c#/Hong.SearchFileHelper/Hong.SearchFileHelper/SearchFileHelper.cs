using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hong.SearchFileHelper
{
	/// <summary>
	/// 取得出來的檔案相關資料
	/// </summary>
	public class FileData
	{
		/// <summary>
		/// 檔案名稱
		/// </summary>
		public string Name { get; set; }

		/// <summary>
		/// 創建日期
		/// </summary>
		public DateTime CreateDate { get; set; }

		/// <summary>
		/// 最後存取時間
		/// </summary>
		public DateTime LastAccessTime { get; set; }
	}

	/// <summary>
	/// 取得此目錄下的檔案（可以加入篩選）
	/// </summary>
	public static class SearchFileHelper
	{
		/// <summary>
		/// 取得此目錄下的所有檔案
		/// </summary>
		/// <param name="directoryPath">完整資料夾路徑</param>
		/// <returns></returns>
		public static List<FileData> GetAllFiles(string directoryPath)
		{
			//如果最後一個字元不是"/"的話，加上去
			if(directoryPath.Substring(directoryPath.Length - 1) != "/") { directoryPath += "/"; }

			List<FileData> result = (from dir in Directory.EnumerateFileSystemEntries(@"" + directoryPath)
							 join file in Directory.GetFiles(@"" + directoryPath) on dir equals file into grp1
							 from file in grp1.DefaultIfEmpty()
							 where file.Contains("")
							 select new FileData
							 {
								 Name = new FileInfo(dir).Name,
								 CreateDate = new FileInfo(dir).CreationTime,
								 LastAccessTime = new FileInfo(dir).LastAccessTime
							 }).ToList();

			return result;
		}

		/// <summary>
		/// 找到此目錄下包含此檔案名稱的檔案
		/// </summary>
		/// <param name="directoryPath">完整資料夾路徑</param>
		/// <param name="fileName">搜尋的檔名</param>
		/// <returns></returns>
		public static List<FileData> SearchFileByFileName(string directoryPath, string fileName)
		{
			//得到此目錄下的所有檔案
			List<FileData> allFiles = GetAllFiles(directoryPath);

			//Filter
			return allFiles.Where(f => f.Name.Contains(fileName)).ToList();
		}


		/// <summary>
		/// 找到此目錄下包含此陣列中的檔案名稱的檔案(or)
		/// </summary>
		/// <param name="directoryPath">完整資料夾路徑</param>
		/// <param name="fileNameList">要搜尋的檔案名稱陣列</param>
		/// <returns></returns>
		public static List<FileData> SearchFileByFileNameList(string directoryPath, List<string> fileNameList)
		{
			//得到此目錄下的所有檔案
			List<FileData> allFiles = GetAllFiles(directoryPath);

			//Filter
			return allFiles.Where(f => fileNameList.Where(l => f.Name.Contains(l)).Count() > 0).ToList();
		}

		/// <summary>
		/// 找到此目錄下包含此陣列中的檔案名稱的檔案(or)
		/// </summary>
		/// <param name="directoryPath">完整資料夾路徑</param>
		/// <param name="fileNameList">要搜尋的檔案名稱陣列（中間都用 , 隔開）</param>
		/// <returns></returns>
		public static List<FileData> SearchFileByFileNameList(string directoryPath, string fileNameList)
		{
			//把空白弄掉
			fileNameList = fileNameList.Replace(" ", "");

			//得到用,分開後的資料
			List<string> splitList = fileNameList.Split(',').ToList();

			//呼叫上一隻方法
			return SearchFileByFileNameList(directoryPath, splitList);
		}

		/// <summary>
		///  找到此目錄下包含此副檔名的檔案
		/// </summary>
		/// <param name="directoryPath">完整資料夾路徑</param>
		/// <param name="extension">搜尋的副檔名</param>
		/// <returns></returns>
		public static List<FileData> SearchFileByExtension(string directoryPath, string extension)
		{
			//得到此目錄下的所有檔案
			List<FileData> allFiles = GetAllFiles(directoryPath);

			//Filter
			return allFiles.Where(f => f.Name.Contains("." + extension)).ToList();
		}

		/// <summary>
		/// 找到此目錄下包含此陣列中的副檔名(or)
		/// </summary>
		/// <param name="directoryPath">完整資料夾路徑</param>
		/// <param name="extensionList">要搜尋的副檔名陣列</param>
		/// <returns></returns>
		public static List<FileData> SearchFileByExtensionList(string directoryPath, List<string> extensionList)
		{
			//得到此目錄下的所有檔案
			List<FileData> allFiles = GetAllFiles(directoryPath);

			//Filter
			return allFiles.Where(f => extensionList.Where(l => f.Name.Contains("." + l)).Count() > 0).ToList();
		}

		/// <summary>
		/// 找到此目錄下包含此陣列中的副檔名(or)
		/// </summary>
		/// <param name="directoryPath">完整資料夾路徑</param>
		/// <param name="fileNameList">要搜尋的副檔名陣列（中間都用 , 隔開）</param>
		/// <returns></returns>
		public static List<FileData> SearchFileByExtensionList(string directoryPath, string extensionList)
		{
			//把空白弄掉
			extensionList = extensionList.Replace(" ", "");

			//得到用,分開後的資料
			List<string> splitList = extensionList.Split(',').ToList();

			//呼叫上一隻方法
			return SearchFileByFileNameList(directoryPath, splitList);
		}
	}
}
