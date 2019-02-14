using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
	參考網圵： https://dotblogs.com.tw/atowngit/2009/12/19/12547 
	 介紹： 

	終止執行緒，可以使用 Interrupt 方法，這個方法會阻斷處於 WaitSleepJoin 狀態下的執行緒。

	msdn :
		如果這個執行緒目前沒有封鎖於等候、休眠或聯結 (Join) 狀態中，就會在下一次要開始封鎖時被插斷。
		插斷的執行緒中會擲回 ThreadInterruptedException，但必須等到執行緒封鎖後才會擲回。如果執行緒一直沒有封鎖，
		就永遠不會擲回此例外狀況，因此執行緒完成時可能完全沒有插斷。
*/

namespace ThreadExample
{
	public class Example6
	{
		long fSum1 = 0;
		long fSum2 = 2;
		Thread Threading1;
		Thread Threading2;

		public static void Go()
		{
			Example6 oProgram = new Example6();
			oProgram.Start();

			Console.ReadKey();
		}

		private void Start()
		{
			Threading1 = new Thread(new ThreadStart(FibonnacciSeries1));
			Threading1.Name = "ThreadA";

			Threading2 = new Thread(new ThreadStart(FibonnacciSeries2));
			Threading2.Name = "ThreadB";

			Threading1.Start(); //啟動第一個執行緒
			Threading2.Start(); //啟動第二個執行緒 

			Threading1.Join();
			Threading2.Join();
		}

		private void FibonnacciSeries1()
		{
			try
			{
				for (int i = 0; i < 10; i++)
				{
					Thread.Sleep(300);

					fSum1 += i;
					if (i > 5)
					{
						//終止執行緒
						Threading1.Interrupt();
					}
					Console.WriteLine(Thread.CurrentThread.Name + " : " + " 目前總合為  = " + fSum1);
				}
			}                   
			catch (ThreadInterruptedException)
			{
				//捕捉例外
				Console.WriteLine(Thread.CurrentThread.Name + " 終止");
			}
		}

		private void FibonnacciSeries2()
		{
			try
			{
				for (int i = 0; i < 10; i++)
				{
					Thread.Sleep(300);
					fSum2 += i;
					Console.WriteLine(Thread.CurrentThread.Name + " : " + " 目前總合為  = " + fSum2);
				}
			}
			catch (ThreadInterruptedException)
			{
				Console.WriteLine(Thread.CurrentThread.Name + " 終止");
			}
		}
	}
}
