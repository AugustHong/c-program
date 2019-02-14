using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
	參考網圵： https://dotblogs.com.tw/atowngit/2009/12/19/12547 
	 介紹： 暫停執行緒使用 Sleep()
*/

namespace ThreadExample
{
	public class Example3
	{
		public static void Go()
		{
			//宣告
			Example3 oProgram = new Example3();
			//執行
			oProgram.Start();
			Console.ReadKey();
		}
		private void Start()
		{
			//建立委派物件並指向 PrintNumber 方法
			ThreadStart myThreadStart = new ThreadStart(PrintNumber);
			//建立執行緒物件
			Thread myThread = new Thread(myThreadStart);
			myThread.Start(); //啟動執行緒
		}
		public void PrintNumber()
		{
			for (int i = 0; i <= 10; i++)
			{
				Console.WriteLine(Thread.CurrentThread.Name + "迴圈次數" + i + " 次");

				if (i == 5)
				{
					Console.WriteLine(" 執行緒暫停 1 秒鐘 !!");
					Thread.Sleep(1000);   // 暫停執行緒 
				}
			}
		}
	}
}
