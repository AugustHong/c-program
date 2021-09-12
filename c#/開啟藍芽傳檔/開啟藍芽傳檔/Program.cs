using InTheHand.Net;
using InTheHand.Net.Bluetooth;
using InTheHand.Windows.Forms;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

/*
 參考網圵： https://www.itread01.com/article/1460102862.html
 先去 NuGet 裝上 32feet.Net
 */

namespace 開啟藍芽傳檔
{
	
	class Program
	{
		[STAThread]
		static void Main(string[] args)
		{
			// 先檢查藍芽資訊
			Console.WriteLine("-------------------檢查藍芽資訊---------------------------");
			BluetoothRadio bluetoothRadio = BluetoothRadio.PrimaryRadio;
			if (bluetoothRadio == null)
			{
				Console.WriteLine("沒有找到本機藍芽裝置!");
			}
			else
			{
				Console.WriteLine("ClassOfDevice: " + bluetoothRadio.ClassOfDevice);
				Console.WriteLine("HardwareStatus: " + bluetoothRadio.HardwareStatus);
				Console.WriteLine("HciRevision: " + bluetoothRadio.HciRevision);
				Console.WriteLine("HciVersion: " + bluetoothRadio.HciVersion);
				Console.WriteLine("LmpSubversion: " + bluetoothRadio.LmpSubversion);
				Console.WriteLine("LmpVersion: " + bluetoothRadio.LmpVersion);
				Console.WriteLine("LocalAddress: " + bluetoothRadio.LocalAddress);
				Console.WriteLine("Manufacturer: " + bluetoothRadio.Manufacturer);
				Console.WriteLine("Mode: " + bluetoothRadio.Mode);
				Console.WriteLine("Name: " + bluetoothRadio.Name);
				Console.WriteLine("Remote:" + bluetoothRadio.Remote);
				Console.WriteLine("SoftwareManufacturer: " + bluetoothRadio.SoftwareManufacturer);
				Console.WriteLine("StackFactory: " + bluetoothRadio.StackFactory);
			}
			Console.WriteLine("-------------------檢查藍芽資訊---------------------------");

			Console.WriteLine("--------------------開始執行作業---------------------------------");
			BluetoothStart(bluetoothRadio);
			Console.WriteLine("--------------------結束程式--------------------------------");

			Console.ReadLine();
		}

		//執行藍芽作業
		static void BluetoothStart(BluetoothRadio radio) {
			string sendFileName = null; //傳送檔名 
			BluetoothAddress sendAddress = null; //傳送目的地址 
			string recDir = null; //接受檔案存放目錄 

			// 選擇遠端藍芽
			sendAddress = SelectBluetooth();
			if (sendAddress == null)
			{
				Console.WriteLine("未選擇對方藍芽，結束程式！");
			}

			// 選擇檔案
			sendFileName = SelectSendFile();
			if (string.IsNullOrEmpty(sendFileName))
			{
				Console.WriteLine("未選擇檔案，結束程式！");
			}


			// 送檔
			BluetoothSendFile btSendFile = new BluetoothSendFile(sendFileName, sendAddress);
			btSendFile.Start();

			// 選擇接收目錄 (不做接收的示範)
			//recDir = SelectReciveFileDir();
			//if (string.IsNullOrEmpty(recDir))
			//{
			//Console.WriteLine("未選擇接收目錄，結束程式！");
			//}

			// 收檔 (這邊不執行)
			/*
			BluetoothReciveFile btReciveFile = new BluetoothReciveFile(recDir, radio);
			btReciveFile.Start();
			System.Threading.Thread.Sleep(5000);
			btReciveFile.End();
			*/
		}

		// 選擇選端藍芽裝置
		static BluetoothAddress SelectBluetooth()
		{
			BluetoothAddress result = null;


			SelectBluetoothDeviceDialog dialog = new SelectBluetoothDeviceDialog();
			dialog.ShowRemembered = true;//顯示已經記住的藍芽裝置 
			dialog.ShowAuthenticated = true;//顯示認證過的藍芽裝置 
			dialog.ShowUnknown = true;//顯示位置藍芽裝置 
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				result = dialog.SelectedDevice.DeviceAddress;//獲取選擇的遠端藍芽地址 
			}

			return result;
		}

		// 選擇傳送檔案
		static string SelectSendFile()
		{
			string result = "";

			OpenFileDialog dialog = new OpenFileDialog();
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				result = dialog.FileName;//設定檔名 
			}

			return result;
		}

		// 選擇接收目錄
		static string SelectReciveFileDir()
		{
			string result = string.Empty;
			FolderBrowserDialog dialog = new FolderBrowserDialog();
			dialog.Description = "請選擇藍芽接收檔案的存放路徑";
			if (dialog.ShowDialog() == DialogResult.OK)
			{
				result = dialog.SelectedPath;
			}

			return result;
		}


	}

	// 送檔
	class BluetoothSendFile
	{
		public string sendFileName { get; set; }
		public BluetoothAddress sendAddress { get; set; }
		public Thread sendThread { get; set; }

		public BluetoothSendFile(string sendFileName, BluetoothAddress sendAddress)
		{
			this.sendAddress = sendAddress;
			this.sendFileName = sendFileName;
		}

		public void Start()
		{
			this.sendThread = new Thread(SendFileByBluetooth);//開啟發送檔案執行緒 
			this.sendThread.Start();
		}

		// 實 際執行作業
		public void SendFileByBluetooth()
		{
			ObexWebRequest request = new ObexWebRequest(sendAddress, Path.GetFileName(sendFileName));//建立網路請求 
			WebResponse response = null;
			try
			{
				request.ReadFile(sendFileName);//傳送檔案 
				Console.WriteLine("開始傳送檔案： " + sendFileName);
				response = request.GetResponse();//獲取迴應 
				Console.WriteLine("傳送完成！");
			}
			catch (System.Exception ex)
			{
				Console.WriteLine("傳送失敗 ： " + ex.Message);
			}
			finally
			{
				if (response != null)
				{
					response.Close();
				}

				End();
			}
		}

		public void End()
		{
			if (sendThread != null)
			{
				sendThread.Abort();
			}
		}
	}

	// 收檔
	class BluetoothReciveFile
	{
		public string recDir { get; set; } //接受檔案存放目錄 
		public ObexListener listener { get; set; } //監聽器
		public Thread listenThread { get; set; }

		public BluetoothRadio radio { get; set; }

		public BluetoothReciveFile(string recDir, BluetoothRadio radio)
		{
			this.recDir = recDir;
			this.radio = radio;
		}

		public void Start()
		{
			if (listener == null || !listener.IsListening)
			{
				radio.Mode = RadioMode.Discoverable;//設定本地藍芽可被檢測 
				listener = new ObexListener(ObexTransport.Bluetooth);//建立監聽 
				listener.Start();
				if (listener.IsListening)
				{
					Console.WriteLine("開始監聽");
					listenThread = new Thread(ReciveFileByBluetooth);//開啟監聽執行緒 
					listenThread.Start();
				}
			}
			else
			{
				listener.Stop();
				Console.WriteLine("停止監聽");
			}
		}

		// 實際作業
		public void ReciveFileByBluetooth()
		{
			ObexListenerContext context = null;
			ObexListenerRequest request = null;
			while (listener.IsListening)
			{
				context = listener.GetContext();//獲取監聽上下文 
				if (context == null)
				{
					break;
				}
				request = context.Request;//獲取請求 
				string uriString = Uri.UnescapeDataString(request.RawUrl);//將uri轉換成字串 
				string recFileName = recDir + uriString;
				request.WriteFile(recFileName);//接收檔案 
				Console.WriteLine("收到檔案" + uriString.TrimStart(new char[] { '/' }));
			}
		}

		public void End()
		{
			if (listenThread != null)
			{
				listenThread.Abort();
			}
			if (listener != null && listener.IsListening)
			{
				listener.Stop();
			}
		}
	}
}
