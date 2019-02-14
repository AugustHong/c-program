using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

/*
	參考網圵： https://dotblogs.com.tw/atowngit/2009/12/19/12547 
	 介紹： 

	 執行緒同步化，避免資源存取衝突 ，使用 Lock；
	藉由 Lock 敘述句完成執行緒同步作業，用以控制程式上某一段資源，這時，其他的執行緒沒權限可以存取這份資源。
*/

namespace ThreadExample
{
	public class Example5_1
	{
		private int addSum;

		public static void Go()
		{
			Example5_1 oProgram = new Example5_1();

			//建立執行緒陣列
			Thread[] threads = new Thread[3];

			//依序設定陣列內容
			for (int i = 0; i < 3; i++)
			{
				Thread myThread = new Thread(new ThreadStart(oProgram.DoSum));
				threads[i] = myThread;
				threads[i].Name = "執行緒" + i;
			}

			//依序啟動執行緒
			for (int i = 0; i < 3; i++)
			{
				threads[i].Start();
			}

			Console.ReadKey();
		}

		void DoSum()
		{
			for (int i = 0; i < 7; i++)
			{
				//當目前執行緒執行這個方法時，會鎖住資源，其他執行緒無法存取，直到該執行緒工作完成
				//this 表示，目前執行緒所在的類別，也就是鎖住這個類別的資源
				lock (this)
				{
					addSum += 2;
					Thread.Sleep(1);
					Console.WriteLine(Thread.CurrentThread.Name + "，執行第 " + i + " 次，addSum =" + addSum);
				}
			}
		}
	}
}
