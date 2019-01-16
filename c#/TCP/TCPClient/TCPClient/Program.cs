using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;  //要引用這一隻
using System.Text;
using System.Threading.Tasks;

/*
	參考網址：https://dotblogs.com.tw/atowngit/2010/01/16/13047 
*/

namespace TCPClient
{
	public static class Program
	{
		//宣告網路資料流變數
		public static NetworkStream myNetworkStream;

		//宣告 Tcp 用戶端物件
		public static TcpClient myTcpClient;

		static void Main(string[] args)
		{
			Console.Write("輸入連接機名稱 : ");
			//取得主機名稱
			string hostName = Console.ReadLine();

			Console.Write("輸入連接通訊埠 : ");
			//取得連線 IP 位址
			int connectPort = int.Parse(Console.ReadLine());

			//建立 TcpClient 物件
			myTcpClient = new TcpClient();


			try
			{
				//測試連線至遠端主機
				myTcpClient.Connect(hostName, connectPort);

				//建立資料流
				myNetworkStream = myTcpClient.GetStream();

				Console.WriteLine("連線成功 !!");
			}
			catch
			{
				Console.WriteLine("主機 {0} 通訊埠 {1} 無法連接  !!", hostName, connectPort);
				return;
			}

			//開始進行作業，第一個傳出去的是我的ip
			WriteData(GetLocalIP(true).FirstOrDefault());

			Console.Read();
		}

		//寫入資料
		public  static void WriteData()
		{
			Console.Write("請輸入你要傳送的文字： ");
			String strTest = Console.ReadLine();

			//將字串轉 byte 陣列，使用 UTF-8 編碼
			Byte[] myBytes = Encoding.UTF8.GetBytes(strTest);

			//將字串寫入資料流
			myNetworkStream.Write(myBytes, 0, myBytes.Length);

			//寫完後就等著收資料了
			ReadData();
		}

		//寫入資料（傳參數）
		public static void WriteData(string input)
		{
			//將字串轉 byte 陣列，使用 UTF-8 編碼
			Byte[] myBytes = Encoding.UTF8.GetBytes(input);

			//將字串寫入資料流
			myNetworkStream.Write(myBytes, 0, myBytes.Length);

			//寫完後就等著收資料了
			ReadData();
		}

		//讀取資料
		public static void ReadData()
		{
			//從網路資料流讀取資料
			int bufferSize = myTcpClient.ReceiveBufferSize;

			//資料流接收文字並讀取
			byte[] receiveDataByte = new byte[bufferSize];
			myNetworkStream.Read(receiveDataByte, 0, bufferSize);

			//取得資料並且解碼文字（用 \0是多餘的位元數）
			string receiveData = Encoding.UTF8.GetString(receiveDataByte, 0, bufferSize).Replace("\0", "");
			Console.Write("從Server中送過來的資料為：" + receiveData + "\n");

			//收完資料，繼續回到輸入資料
			WriteData();
		}


		/*
		 來自於我github中的c-program/c#/Hong.IPHelper的method
		 */

		/// <summary>
		/// 取得LocalIP
		/// </summary>
		/// <param name="onlyIPV4">是否只取IPV4（預設為false）</param>
		/// <returns></returns>
		public static List<string> GetLocalIP(bool onlyIPV4 = false)
		{
			//先取得主機名稱
			string hostName = Dns.GetHostName();

			List<string> result = new List<string>();

			IPHostEntry iphostentry = Dns.GetHostByName(hostName);   //取得本機的 IpHostEntry 類別實體

			foreach (var ip in iphostentry.AddressList)
			{
				//是否只取得IPV4
				//第一段判別式是判別是否是IPV4，所以其他的都會是false（故用or來加入 => 當後面是true則是取全部資料）
				//當後面是false是只取IPV4的資料
				if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork || !onlyIPV4)
				{
					result.Add(ip.ToString());
				}

			}

			return result;
		}

	}
}
