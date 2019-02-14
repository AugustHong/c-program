using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
	參考網圵： https://dotblogs.com.tw/atowngit/2009/12/19/12547 
	 介紹： 建立可以傳入參數的執行緒 ，ParameterizedThreadStart。
*/

namespace ThreadExample
{
	public class Example2
	{
		public static void Go()
		{
			//宣告
			Example2 oProgram = new Example2();
			//執行
			oProgram.Start();
			Console.ReadKey();
		}

		private void Start()
		{
			//建立一個執行緒，並且傳入一個委派物件 ParameterizedThreadStart，'
			//並且設定指向 PrintOddNumber 方法。               
			Thread oThreadA = new Thread(new ParameterizedThreadStart(PrintOddNumber));

			//設定執行緒的 Name
			oThreadA.Name = "A Thread";

			//建立一個執行緒，並且傳入一個委派物件 PrintNumber，'
			//並且設定指向 PrintOddNumber 方法。               
			Thread oThreadB = new Thread(new ParameterizedThreadStart(PrintNumber));

			//設定執行緒的 Name
			oThreadB.Name = "B Thread";

			//啟動執行緒物件，並且傳入參數
			oThreadA.Start(10);
			oThreadB.Start(10);
		}

		//印出奇數
		private void PrintOddNumber(object value)
		{
			int Number = Convert.ToInt32(value);
			for (int i = 1; i <= Number; i++)
			{
				if (i % 2 != 0)
				{
					Console.WriteLine("執行緒{0}，輸出奇數{1}", Thread.CurrentThread.Name, i);
				}
			}
		}

		//印出偶數
		private void PrintNumber(object value)
		{
			int Number = Convert.ToInt32(value);
			for (int i = 1; i <= Number; i++)
			{
				if (i % 2 == 0)
				{
					Console.WriteLine("執行緒{0}，輸出奇數{1}", Thread.CurrentThread.Name, i);
				}
			}
		}
	}
}
