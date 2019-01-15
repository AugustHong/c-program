using Spire.Xls;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
	先去NuGet裝上 Free spire.XLS 
*/

namespace Hong.ExcelAndCsvConversionHelper
{
	/// <summary>
	/// 用於做 Excel 和 Csv互換的工具
	/// </summary>
	public class ExcelAndCsvConversionHelper
	{
		/// <summary>
		/// 處理Csv的部份
		/// </summary>
		public static class CsvHelper
		{
			/// <summary>
			/// 將傳入的csv檔案路徑 轉成 excel
			/// </summary>
			/// <param name="filePath">完整檔案路徑</param>
			/// <param name="outputFileName">output的檔名(不含副檔名。例如： hello)（預設空的，即為原檔案名稱）</param>
			/// <param name="isVersion2013">轉成xlsx檔嗎（預設為true）</param>
			/// <returns></returns>
			public static string CsvConversionExcel(string filePath, string outputFileName = "", bool isVersion2013 = true)
			{
				//取得檔案基本資料
				FileBasicData file = filePath.ToFileBasicData();

				//如果副檔名不是csv則回傳空字串
				if(file.Extension.ToUpper() != "CSV") { return string.Empty; }

				//如果沒有給輸出檔名，就拿原本的
				if (string.IsNullOrEmpty(outputFileName)) { outputFileName = file.FileName; }

				//載入檔案
				Workbook workbook = new Workbook();
				workbook.LoadFromFile(filePath, ",", 1, 1);

				//輸出檔案路徑（ 先不加副檔名是因為要依據是否為xlsx）
				string result = file.DirectoryPath + outputFileName + ".";

				//依據是否用xlsx來做輸出
				if (isVersion2013)
				{
					result += "xlsx";
					workbook.SaveToFile(result, ExcelVersion.Version2013);
				}
				else
				{
					result += "xls";
					workbook.SaveToFile(result, ExcelVersion.Version97to2003);
				}

				return result;
			}


			/// <summary>
			/// 找尋此目錄中的所有csv檔並轉成excel
			/// </summary>
			/// <param name="directoryPath">完整目錄路徑</param>
			/// <returns></returns>
			public static List<string> CsvConversionExcelByDirectory(string directoryPath)
			{
				List<string> result = new List<string>();

				//如果目錄沒加上 / 幫他加
				if(directoryPath.Substring(directoryPath.Length - 1) != "/") { directoryPath += "/"; }

				//全部檔名
				var directoryFiles = (from dir in Directory.EnumerateFileSystemEntries(@"" + directoryPath)
							    join file in Directory.GetFiles(@"" + directoryPath) on dir equals file into grp1
							    from file in grp1.DefaultIfEmpty()
							    where file.Contains(".csv")
							    select  new FileInfo(dir).Name ).ToList();

				if(directoryFiles.Count() > 0)
				{
					//將全部檔案 跑過 上一支function
					foreach(var file in directoryFiles)
					{
						//因為取出來的只有 xxx.csv 所以還要加上 原本路徑
						string output = CsvConversionExcel(directoryPath + file);
						result.Add(output);
					}
				}

				return result;
			}
		}

		/// <summary>
		/// 處理Excel的部份
		/// </summary>
		public static class ExcelHelper
		{
			/// <summary>
			/// Excel的版本互轉（xlsx <=> xls）
			/// </summary>
			/// <param name="filePath">完路檔案路徑</param>
			/// <param name="outputFileName">輸出檔名(不含副檔名。例如： hello)（預設原檔名）</param>
			/// <returns></returns>
			public static string ConversionVersion(string filePath, string outputFileName = "")
			{
				//其本檔案內容
				FileBasicData file = filePath.ToFileBasicData();

				//副檔名不對，傳回空字串
				if(file.Extension.ToUpper() != "XLS" && file.Extension.ToUpper() != "XLSX") { return string.Empty; }

				//如果是預設的輸出檔名，給其值
				if (string.IsNullOrEmpty(outputFileName)) { outputFileName = file.FileName; }

				//載入檔案
				Workbook workbook = new Workbook();
				workbook.LoadFromFile(filePath);

				//副檔名結果
				string result = file.DirectoryPath + outputFileName + ".";

				//依據副檔名做事
				switch (file.Extension.ToUpper())
				{
					//把 xls => xlsx
					case "XLS":
						result += "xlsx";
						workbook.SaveToFile(result, ExcelVersion.Version2013);
						break;

					//把 xlsx => xls
					case "XLSX":
						result += "xls";
						workbook.SaveToFile(result, ExcelVersion.Version97to2003);
						break;

					default:
						return string.Empty;
				}

				return result;
			}

			/// <summary>
			/// 做Excel轉成CSV
			/// </summary>
			/// <param name="filePath">完整檔案路徑</param>
			/// <param name="outputFileName">輸出檔名(不含副檔名。例如： hello)（預設 原檔名_sheetNo）</param>
			/// <param name="sheetNo">指定轉換第幾個sheet（預設為1）</param>
			/// <param name="conversionAllSheet">是否要此Excel檔中的所有sheet都轉（預設為false）</param>
			/// <returns></returns>
			public static List<string> ExcelConversionCsv(string filePath, string outputFileName = "", int sheetNo = 1, bool conversionAllSheet = false)
			{
				List<string> result = new List<string>();

				//取得檔案基本資料
				FileBasicData file = filePath.ToFileBasicData();

				//如果副檔名不是excel的則回傳空字串List
				if (file.Extension.ToUpper() != "XLS" && file.Extension.ToUpper() != "XLSX") { return new List<string>(); }

				//如果沒有給輸出檔名，就拿原本的
				if (string.IsNullOrEmpty(outputFileName)) { outputFileName = file.FileName; }

				//載入檔案
				Workbook workbook = new Workbook();
				workbook.LoadFromFile(filePath);

				//如果他裡面有sheet的話
				if(workbook.Worksheets.Count() > 0)
				{
					//如果要轉換全部的話
					if (conversionAllSheet)
					{
						for(var i = 0; i < workbook.Worksheets.Count(); i++)
						{
							//取得sheet資料
							Worksheet sheet = workbook.Worksheets[i];

							string output = outputFileName + "_" + i.ToString() + ".csv";

							//存入成csv檔
							sheet.SaveToFile(output, ",", Encoding.UTF8);

							result.Add(output);
						}
					}
					else
					{
						//反之只轉換指定的sheet

						//陣列從0開始，而使用者下習慣是第1張sheet
						sheetNo--;

						//如果sheet沒有他指定的那麼多，自動把他變為0（第1個sheet）  ，且它的
						if (workbook.Worksheets.Count() <= sheetNo) { sheetNo = 0; }

						//取得sheet資料
						Worksheet sheet = workbook.Worksheets[sheetNo];

						string output = outputFileName + "_" + sheetNo.ToString() + ".csv";

						//存入成csv檔
						sheet.SaveToFile(output, ",", Encoding.UTF8);

						result.Add(output);
					}
				}

				return result;
			}


			/// <summary>
			/// 找尋此目錄中的所有excel檔並轉成csv（預設只取第一個sheet，或者conversionAllSheet設為true把所有都轉）
			/// </summary>
			/// <param name="directoryPath">完整目錄路徑</param>
			/// <param name="conversionAllSheet">是否要此Excel檔中的所有sheet都轉（預設為false）</param>
			/// <returns></returns>
			public static List<string> ExcelConversionCsvByDirectory(string directoryPath, bool conversionAllSheet = false)
			{
				List<string> result = new List<string>();

				//如果目錄沒加上 / 幫他加
				if (directoryPath.Substring(directoryPath.Length - 1) != "/") { directoryPath += "/"; }

				//全部檔名
				var directoryFiles = (from dir in Directory.EnumerateFileSystemEntries(@"" + directoryPath)
							    join file in Directory.GetFiles(@"" + directoryPath) on dir equals file into grp1
							    from file in grp1.DefaultIfEmpty()
							    where file.Contains(".xls") || file.Contains(".xlsx")
							    select new FileInfo(dir).Name).ToList();

				if (directoryFiles.Count() > 0)
				{
					//將全部檔案 跑過 上一支function
					foreach (var file in directoryFiles)
					{
						List<string> output = new List<string>();

						//因為取出來的只有 xxx.xls 所以還要加上 原本路徑
						if (conversionAllSheet)
						{
							output = ExcelConversionCsv(directoryPath + file, conversionAllSheet: true);
						}
						else
						{
							output = ExcelConversionCsv(directoryPath + file);
						}
						
						result.AddRange(output);
					}
				}

				return result;
			}
		}
	}


	/// <summary>
	/// 檔案的基本內容
	/// </summary>
	public class FileBasicData
	{
		/// <summary>
		/// 此檔案目錄總路徑
		/// </summary>
		public string DirectoryPath { get; set; }

		/// <summary>
		/// 純檔名（未含副檔名）
		/// </summary>
		public string FileName { get; set; }

		/// <summary>
		/// 副檔名
		/// </summary>
		public string Extension { get; set; }
	}

	/// <summary>
	/// 擴充方法（class一定要是靜態的)
	/// 讓一段路徑分割出 資料夾路徑、檔名、副檔名
	/// </summary>
	public static class FileBasicConvert
	{
		//讓string 變成SecureString
		public static FileBasicData ToFileBasicData(this string value)
		{

			FileBasicData result = new FileBasicData();

			//用 / 分割（而檔名就是最後一個）
			string[] splitData = value.Split('/');

			//取出含有副檔名的檔案名稱（即上面分割出來的最後一個）
			string includeExtensionFileName = splitData[splitData.Length - 1];

			//用 . 來分割出 檔名、副檔名
			string[] fileNameData = includeExtensionFileName.Split('.');

			if(fileNameData.Count() == 2)
			{
				result = new FileBasicData
				{
					//檔案資料夾路徑 = 原本的值 取代掉 取出來的 含副檔名之檔名
					DirectoryPath = value.Replace(includeExtensionFileName, ""),
					FileName = fileNameData[0],
					Extension = fileNameData[1]
				};
			}

			return result;
		}
	}
}
