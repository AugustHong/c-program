using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;

/*
	參考網圵： https://codertw.com/%E7%A8%8B%E5%BC%8F%E8%AA%9E%E8%A8%80/41677/

*/

namespace Hong.FTPHelper
{
	/// <summary>
	/// 處理FTP相關 Ftp各種操作,上傳,下載,刪除檔案,建立目錄,刪除目錄,獲得檔案列表等
	/// </summary>
	public class FTPHelper
	{
		#region 相關變數
		/// <summary>
		/// FTP的IP
		/// </summary>
		private string ftpServerIP;

		/// <summary>
		/// 使用者帳號
		/// </summary>
		private string ftpUserID;

		/// <summary>
		/// 使用者密碼
		/// </summary>
		private string ftpPassword;

		/// <summary>
		/// 所要執行的操作
		/// </summary>
		private FtpWebRequest reqFTP;

		#endregion


		/// <summary>
		/// 建構子
		/// </summary>
		/// <param name="ftpServerIP">FTP站台ip</param>
		/// <param name="ftpUserID">使用者帳號</param>
		/// <param name="ftpPassword">使用者密碼</param>
		public FTPHelper(string ftpServerIP, string ftpUserID, string ftpPassword)
		{
			this.ftpServerIP = ftpServerIP;
			this.ftpUserID = ftpUserID;
			this.ftpPassword = ftpPassword;
		}

		/// <summary>
		/// 連線ftp
		/// </summary>
		/// <param name="path">路徑</param>
		private void Connect(String path)//連線ftp
		{
			// 根據uri建立FtpWebRequest物件
			reqFTP = (FtpWebRequest)FtpWebRequest.Create(new Uri(path));

			// 指定資料傳輸型別(這邊是指使用二進制)
			reqFTP.UseBinary = true;

			// ftp使用者名稱和密碼
			reqFTP.Credentials = new NetworkCredential(ftpUserID, ftpPassword);
		}

		/// <summary>
		/// 獲取ftp伺服器上的檔案列表
		/// </summary>
		/// <param name="path">路徑</param>
		/// <param name="WRMethods">所要使用的方法(取檔名、下載……等)，預設是讀取資料</param>
		/// <returns></returns>
		private List<string> GetFileList(string path, string WRMethods = "NLST")//上面的程式碼示例瞭如何從ftp伺服器上獲得檔案列表
		{
			List<string> result = new List<string>();

			try
			{
				//連線
				Connect(path);
				//給方法
				reqFTP.Method = WRMethods;

				//創建一個回應
				WebResponse response = reqFTP.GetResponse();

				//讀取出來的結果
				StreamReader reader = new StreamReader(response.GetResponseStream(), System.Text.Encoding.Default);//中文檔名

				//讀取每一行
				string line = reader.ReadLine();

				while (line != null)
				{
					//加入資料
					result.Add(line);
					line = reader.ReadLine();
				}

				//關閉相關資源
				reader.Close();
				response.Close();

				return result;
			}
			catch (Exception ex)
			{
				throw new Exception("FTP進行相關作業 -- 讀取檔案列表發生錯誤： " + ex.Message);
			}

		}

		/// <summary>
		/// 利用上面的方法來讀取檔案列表
		/// </summary>
		/// <param name="path">路徑(不用加上FTP的ip，會自動幫你加完)，如輸入空字串即是取得根目錄下的檔案列表(預設是空字串)</param>
		/// <returns></returns>
		public List<string> GetFileList(string path = "")
		{
			try
			{
				// WebRequestMethods.Ftp.ListDirectory 得出來的就是 "NLST"，故上方的方法預設值即是這個
				return GetFileList("ftp://" + ftpServerIP + "/" + path, WebRequestMethods.Ftp.ListDirectory);
			}
			catch (Exception ex)
			{
				throw new Exception("GetFileList: 讀取檔案列表失敗 ：" + ex.Message);
			}
		}

		/// <summary>
		/// 跟上方的方法類似，但這次是取得檔案明細
		/// </summary>
		/// <param name="path">路徑(不用加上FTP的ip，會自動幫你加完)，如輸入空字串即是取得根目錄下的檔案列表(預設是空字串)</param>
		/// <returns></returns>
		public List<string> GetFilesDetailList(string path = "")
		{
			try
			{
				return GetFileList("ftp://" + ftpServerIP + "/" + path, WebRequestMethods.Ftp.ListDirectoryDetails);
			}
			catch (Exception ex)
			{
				throw new Exception("GetFilesDetailList  讀取檔案明細失敗： " + ex.Message);
			}
		}

		/// <summary>
		/// 檔案上傳
		/// </summary>
		/// <param name="filename">完整檔案路徑</param>
		public void Upload(string filename) //上面的程式碼實現了從ftp伺服器上載檔案的功能
		{
			//確定檔案存在
			if (!File.Exists(filename)){ throw new Exception("檔案不存在，Upload失敗"); }

			//取得檔案資訊
			FileInfo fileInf = new FileInfo(filename);
			string uri = "ftp://" +  ftpServerIP +  "/" +  fileInf.Name;

			//開始執行動作
			try
			{
				//連線
				Connect(uri);   

				// 預設為true，連線不會被關閉
				// 在一個命令之後被執行
				reqFTP.KeepAlive = false;

				// 指定執行什麼命令(執行上傳)
				reqFTP.Method = WebRequestMethods.Ftp.UploadFile;

				// 上傳檔案時通知伺服器檔案的大小
				reqFTP.ContentLength = fileInf.Length;

				// 緩衝大小設定為kb
				int buffLength = 2048;

				byte[] buff = new byte[buffLength];

				//內容長度
				int contentLen;

				// 開啟一個檔案流(System.IO.FileStream) 去讀上傳的檔案
				FileStream fs = fileInf.OpenRead();

				// 把上傳的檔案寫入流
				Stream strm = reqFTP.GetRequestStream();

				// 每次讀檔案流的kb
				contentLen = fs.Read(buff, 0, buffLength);

				// 資料流內容沒有結束
				while (contentLen != 0)
				{
					// 把內容從file stream 寫入upload stream
					strm.Write(buff, 0, contentLen);

					// 每次讀檔案流的kb
					contentLen = fs.Read(buff, 0, buffLength);
				}

				// 關閉兩個流
				strm.Close();
				fs.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("FTP檔案上傳失敗： " + ex.Message);
			}
		}

		/// <summary>
		/// 檔案下載
		/// </summary>
		/// <param name="fileSavePath">要儲存的路徑(即載下來的時候要放去哪)</param>
		/// <param name="fileName">完整路徑檔名</param>
		/// <param name="errorinfo">錯誤資訊</param>
		/// <returns></returns>
		public bool Download(string fileSavePath, string fileName, out string errorinfo)
		{
			try
			{
				//取得檔名
				String onlyFileName = Path.GetFileName(fileName);

				//儲存的路徑(Local)
				string newFileName = fileSavePath +  "/" +  onlyFileName;

				//如果檔案已在本地(Local)中存在
				if (File.Exists(newFileName))
				{
					errorinfo = string.Format("本地檔案{0}已存在,無法下載", newFileName);
					return false;
				}

				//FTP路徑
				string url = "ftp://" +  ftpServerIP +  "/" +  fileName;

				//連線
				Connect(url); 

				//得到Response
				FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

				//讀取資料流
				Stream ftpStream = response.GetResponseStream();

				//內容長度
				long cl = response.ContentLength;

				//緩沖區大小
				int bufferSize = 2048;
				
				//讀取數量
				int readCount;

				byte[] buffer = new byte[bufferSize];

				readCount = ftpStream.Read(buffer, 0, bufferSize);

				FileStream outputStream = new FileStream(newFileName, FileMode.Create);

				//如果資料流還沒結束
				while (readCount > 0)
				{
					//寫入資料進入Output中
					outputStream.Write(buffer, 0, readCount);

					//再讀取
					readCount = ftpStream.Read(buffer, 0, bufferSize);
				}

				//關閉資源
				ftpStream.Close();
				outputStream.Close();
				response.Close();

				errorinfo = "";

				return true;
			}
			catch (Exception ex)
			{
				throw new Exception("FTP無法下載此檔案： " + ex.Message);
			}

		}

		/// <summary>
		/// 刪除檔案
		/// </summary>
		/// <param name="fileName">檔名(完整路徑，但不用包含FTP的ip那段)</param>
		public void DeleteFile(string fileName)
		{
			try
			{
				string uri = "ftp://" +  ftpServerIP +  "/" + fileName;

				//連線 
				Connect(uri);        

				// 預設為true，連線不會被關閉
				// 在一個命令之後被執行
				reqFTP.KeepAlive = false;

				// 指定執行什麼命令(此次是刪除)
				reqFTP.Method = WebRequestMethods.Ftp.DeleteFile;

				//得到Response(因為刪除是一次性動作，所以不用得到其回傳值。他已經刪除了)
				FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();
				
				//關閉資料
				response.Close();

			}
			catch (Exception ex)
			{
				throw new Exception("FTP刪除案發生錯誤： " + ex.Message);
			}
		}
	
		/// <summary>
		/// 建立目錄
		/// </summary>
		/// <param name="dirName">完整目錄名稱</param>
		public void MkDir(string dirName)
		{

			try
			{
				string uri = "ftp://" +  ftpServerIP +  "/" +  dirName;

				//連線 
				Connect(uri);     

				//執行的動作
				reqFTP.Method = WebRequestMethods.Ftp.MakeDirectory;

				//同上面一樣，因為是一個動作而已，所以不用特別再對Response做事
				FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

				//關閉資料
				response.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("FTP創建目錄失敗： " + ex.Message);
			}
		}

		/// <summary>
		/// 刪除目錄
		/// </summary>
		/// <param name="dirName">完整目錄名稱</param>
		public void RmDir(string dirName)
		{
			try
			{
				string uri = "ftp://" +  ftpServerIP +  "/" + dirName;

				//連線 
				Connect(uri);     

				//執行動作(刪除資料夾)
				reqFTP.Method = WebRequestMethods.Ftp.RemoveDirectory;

				//同上面一樣，因為是一個動作而已，所以不用特別再對Response做事
				FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

				//關閉資源
				response.Close();
			}
			catch (Exception ex)
			{
				throw new Exception("FTP刪除資料夾發生錯誤： " + ex.Message);
			}
		}

		/// <summary>
		/// 獲得檔案大小
		/// </summary>
		/// <param name="filename">完整檔案路徑</param>
		/// <returns></returns>
		public long GetFileSize(string filename)
		{
			//輸出的檔案大小
			long result = 0;

			try
			{
				string uri = "ftp://" +  ftpServerIP +  "/" + filename;

				//連線  
				Connect(uri);    

				//執行的動作(取檔案大小)
				reqFTP.Method = WebRequestMethods.Ftp.GetFileSize;

				//得到Response(這次會要繼續動作，所以不是直接關閉資源)
				FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

				//得到檔案大小
				result = response.ContentLength;

				//關閉資源
				response.Close();

				return result;
			}
			catch (Exception ex)
			{
				throw new Exception("FTP取得檔案 " + "ftp://" + ftpServerIP + "/" + filename + " 之大小發生錯誤" + ex.Message);
			}
		}

		/// <summary>
		/// 檔案改名
		/// </summary>
		/// <param name="currentFilename">當前檔案完整路徑</param>
		/// <param name="newFilename">新檔案名稱</param>
		public void Rename(string currentFilename, string newFilename)
		{
			try
			{
				string uri = "ftp://" +  ftpServerIP +  "/" + currentFilename;

				//連線
				Connect(uri);

				//動作(改名)
				reqFTP.Method = WebRequestMethods.Ftp.Rename;

				//設定改名後的名稱
				reqFTP.RenameTo = newFilename;

				//得到Response(後面註解的部份是讀檔那些，看以後是否要擴充)
				FtpWebResponse response = (FtpWebResponse)reqFTP.GetResponse();

				//Stream ftpStream = response.GetResponseStream();
				//ftpStream.Close();

				//關閉資源
				response.Close();
			}
			catch (Exception ex)
			{
				throw new Exception($"FTP檔名更改失敗 ， 由 {currentFilename} 改名至 {newFilename} 發生錯誤， {ex.Message}");
			}
		}
	}
}
