using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutofacTextInterface;  //引用剛才建立的Interface

/*
	Autofac應用：
	參考網圵： https://dotblogs.com.tw/rolence0515/2014/03/26/144530
	本專案是第二步： 建立一個類別檔
*/

namespace E1
{
    public class E1 : IEModule
    {
		public void Print()
		{
			Console.WriteLine("這裡是E1，聽到請回答");
		}
    }
}
