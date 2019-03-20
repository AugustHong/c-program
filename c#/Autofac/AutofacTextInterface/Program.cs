using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
	Autofac應用：
	參考網圵： https://dotblogs.com.tw/rolence0515/2014/03/26/144530
	本專案是第一步： 建立一個共用的Interface
*/

namespace AutofacTextInterface
{
	class Program
	{
		static void Main(string[] args)
		{
		}
	}

	/// <summary>
	/// 建立一個共用的Interface
	/// </summary>
	public interface IEModule
	{
		void Print();
	}
}
