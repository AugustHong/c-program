using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;  //要引用這隻
using System.Text;
using System.Threading.Tasks;

namespace TCPServer
{
	class Program
	{
		static void Main(string[] args)
		{
			//IP（自訂）
			string IP = "127.0.0.1";

			//Port（自訂）
			int port = 36000;

			//接收的長度上限（自訂）
			int receiveLen = 2048;

			//建立 IPAddress 物件(本機)
			System.Net.IPAddress theIPAddress = System.Net.IPAddress.Parse(IP);

			//建立監聽物件（剛才建立的IPAddress物件, port）
			TcpListener myTcpListener = new TcpListener(theIPAddress, port);

			//啟動監聽
			myTcpListener.Start();

			Console.WriteLine("TCP Server啟動  \n通訊埠 36000 \n等待用戶端連線...... !!");

			//建立Socket（要接收監聽到的值，和回傳用的）
			Socket mySocket = myTcpListener.AcceptSocket();

			do
			{
				try
				{
					//偵測是否有來自用戶端的連線要求，若是
					//用戶端請求連線成功，就會秀出訊息。
					if (mySocket.Connected)
					{
						//建立byte[]來接收值（但會過長，可能有多餘的空格。而下面在送給Client時就會出現一大堆空格）
						byte[] ReceiveDataByte = new byte[receiveLen];

						//取得用戶端寫入的資料長度
						int dataLength = mySocket.Receive(ReceiveDataByte);

						//實際取得資料（去掉後面的一大堆空白）
						byte[] trueReceiveDataByte = new byte[dataLength];

						//複製取得的資料，並放入到真正長度的byte[]中
						//把他複製到trueReceiveDataByte（分別為從ReceiveDataByte的第0位置開始  copy共len個位數資料 到 trueReceiveDataByte 的第0位數）
						Array.Copy(ReceiveDataByte, 0, trueReceiveDataByte, 0, dataLength);

						Console.WriteLine("接收到的資料長度 {0} \n ", dataLength.ToString());
						Console.Write("取出用戶端寫入網路資料流的資料內容 :");

						//取出資料  (用 \0是多餘的位元數，再度檢查一次)
						string ReceiveData = Encoding.UTF8.GetString(trueReceiveDataByte, 0, dataLength).Replace("\0", "");
						Console.Write(ReceiveData + "\n");

						//將接收到的資料回傳給用戶端
						//mySocket.Send(byte[]資料, size, flag)
						mySocket.Send(trueReceiveDataByte, trueReceiveDataByte.Length, 0);
					}
				}
				catch (Exception e)
				{
					Console.WriteLine(e.Message);
					mySocket.Close();
					break;
				}

			} while (true);
		}
	}
}
