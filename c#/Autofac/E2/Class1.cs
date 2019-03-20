using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutofacTextInterface;  //引用剛才建立的Interface

/*
	Autofac應用：
	參考網圵： https://dotblogs.com.tw/rolence0515/2014/03/26/144530
	本專案是產生第二個類別檔
*/

namespace E2
{
    public class E2 : IEModule
    {
		public void Print()
		{
			Console.WriteLine("這裡是E2，回答中……");
		}
	}
}
