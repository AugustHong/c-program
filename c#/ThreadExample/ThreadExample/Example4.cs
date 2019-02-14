using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
	參考網圵： https://dotblogs.com.tw/atowngit/2009/12/19/12547 
	 介紹： 

	 執行緒使用 Join()
	當執行緒本身無法決定暫停多久，必須等到其他的執行緒完成，才能繼續剩下的工作，使用這個方法，
	會封鎖目前的執行緒，直到引用這個方法的執行緒執行完成之後，再進行未完成的工作。
*/

namespace ThreadExample
{
	public class Example4
	{
		//欄位
		private Thread ThreadA;
		private Thread ThreadB;

		public static void Go()
		{
			//這邊有三個 thread :  本身應用程式的主執行緒，
			//還有另外建立的兩條執行緒 ThreadA、 ThreadB

			Example4 oProgram = new Example4();
			oProgram.Start();
			Console.WriteLine(" 暫停主執行緒 !!");

			//暫停主執行緒，等待執行緒 A 工作完畢
			oProgram.ThreadA.Join();
			Console.WriteLine(" 執行緒工作完成 !!");
			Console.ReadKey();
		}


		private void Start()
		{
			//建立執行緒 A
			ThreadA = new Thread(new ThreadStart(PrintNumber));
			ThreadA.Name = "A Thread";

			//建立執行緒 B
			ThreadB = new Thread(new ThreadStart(JoinPrintNumber));
			ThreadB.Name = "B Thread";

			//啟動執行緒
			ThreadA.Start();
			ThreadB.Start();

		}
		private void PrintNumber()
		{
			for (int i = 1; i <= 10; i++)
			{
				Console.WriteLine(Thread.CurrentThread.Name +  " 迴圈： " + i + " 次");
				Thread.Sleep(300);
				if (i == 5)
				{
					Console.WriteLine("暫停執行緒 {0} ", Thread.CurrentThread.Name);
					//暫停目前執行緒，等待 ThreadB 執行完
					ThreadB.Join();
				}

			}
		}
		private void JoinPrintNumber()
		{
			for (int i = 11; i <= 20; i++)
			{
				Console.WriteLine(Thread.CurrentThread.Name +  " 迴圈開始執行迄今第 " + i + " 次");
				Thread.Sleep(300);
			}
		}
	}
}
