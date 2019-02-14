using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
	參考網圵： https://dotblogs.com.tw/atowngit/2009/12/19/12547 
	 介紹： 建立執行緒，其中 ThreadStart 是一個委派型別，可以指向沒有參數沒有回傳值得方法。
*/

namespace ThreadExample
{
	public class Example1
	{
		public static void Go()
		{
			//宣告
			Example1 oProgram = new Example1();
			//執行
			oProgram.Start();
			Console.ReadKey();
		}

		private void Start()
		{
			//建立一個執行緒，並且傳入一個委派物件 ThreadStart，並且指向 PrintOddNumber 方法。            
			Thread oThreadA = new Thread(new ThreadStart(PrintOddNumber));
			oThreadA.Name = "A Thread";

			//建立一個執行緒，並且傳入一個委派物件 ThreadStart，並且指向 PrintNumber 方法。
			Thread oThreadB = new Thread(new ThreadStart(PrintNumber));
			oThreadB.Name = "B Thread";

			//啟動執行緒物件
			oThreadA.Start();
			oThreadB.Start();
		}

		//印出奇數
		private void PrintOddNumber()
		{
			for (int i = 1; i < 10; i++)
			{
				if (i % 2 != 0)
				{
					Console.WriteLine("執行緒{0}，輸出奇數{1}", Thread.CurrentThread.Name, i);
				}
			}
		}

		//印出偶數
		private void PrintNumber()
		{
			for (int i = 1; i < 10; i++)
			{
				if (i % 2 == 0)
				{
					Console.WriteLine("執行緒{0}，輸出奇數{1}", Thread.CurrentThread.Name, i);
				}
			}
		}
	}
}
