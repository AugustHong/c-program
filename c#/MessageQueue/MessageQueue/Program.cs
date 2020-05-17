using System;
using System.Collections.Generic;
using System.Linq;
using System.Messaging;
using System.Text;
using System.Threading.Tasks;

/*
MessageQueue 基本介紹：
https://www.itread01.com/content/1550349026.html
可以想成他就是一個 Queue (先進先出的資料儲存地方)

.Net 實作 MessageQueue：
https://dotblogs.com.tw/calm/2013/12/01/132098

實作步驟：
1. 先確認是否 傳送/接收端都有 MSMQ
2. 如果沒有請進行以下
3. 控制台>程式和功能>開啟或關閉Windows功能，選取"Microsoft Message Queue(MSMQ) 伺服器/Microsoft Message Queue(MSMQ) 伺服器核心"
4. 安裝後，在  控制台>電腦管理>訊息佇列 即可看到有多出"訊息佇列"這個功能 (WIN8 在 系統管理工具裡 按下去 會有一大堆捷徑，選 "系統管理" 即可)
5. WIN8 的電腦管理 https://support.microsoft.com/zh-tw/help/2781927
6. 去Nuget 裝上 System.Messaging 和 解析xml(System.Xml + System.Xml.Serialization) 的 或 json 的 (Newtonsoft.Json)
*/

namespace MessageQueueTest
{
	//自訂訊息內容(要發送/接收的資料格式)
	public class MyData
	{
		public int id;
		public string text;
		public DateTime now;
		public double unm { get; set; }
	}

	class Program
	{
		static void Main(string[] args)
		{
			//發送訊息
			SendMessage();

			//接收訊息
			MyData result = ReceiveMessage();
		}

		static void SendMessage()
		{
			//string queuePath = @"FormatName:DIRECT=TCP:192.168.1.1\private$\myqueue"; // 使用遠程IP指定訊息佇列位置
			string queuePath = @".\private$\myqueue"; //使用本機方式指定訊息佇列位置 (private$ 代表是私有佇列)
			// 你去 控制台/電腦管理/訊息佇列 會看到有3種 ：傳出佇列、私有佇列、系統佇列

			if (!MessageQueue.Exists(queuePath))//判斷 myqueue訊息佇列是否存在
			{
				MessageQueue.Create(queuePath);//建立用來接受/發送的訊息佇列
			}
			MessageQueue myQueue = new MessageQueue(queuePath);

			//要發送的內容
			MyData data = new MyData();
			data.id = 1;
			data.text = "Holle";
			data.now = DateTime.Now;
			data.unm = DateTime.Now.Second;

			//發送訊息
			myQueue.Send(data, "MY 標題");
		}

		static MyData ReceiveMessage()
		{
			string queuePath = @".\private$\myqueue";//使用本機方式指定訊息佇列位置
			MessageQueue myQueue = new MessageQueue(queuePath);

			myQueue.Formatter = new XmlMessageFormatter(new Type[] { typeof(MyData) });//設定接收訊息內容的型別
			Message message = myQueue.Receive();//接收訊息佇列內的訊息
			MyData data = (MyData)message.Body;//將訊息內容轉成正確型別
			return data;
		}
	}
}
