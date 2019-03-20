using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutofacTextInterface; //引用剛才建立的Interface
using System.Reflection;
using Autofac;

/*
	Autofac應用：
	參考網圵： https://dotblogs.com.tw/rolence0515/2014/03/26/144530  (後來證實他的有很多錯誤，所以用我自己開發的)
	測試

	步驟：
	1.先去NuGet裝上 Autofac 和 Autofac Configuration
*/

namespace AutofaceTest
{
	public class Program
	{
		static void Main(string[] args)
		{
			//開始測試

			//平常寫在WebConfig、AppConfig中的參數。但這裡方便測試就直接用List
			List<string> source = new List<string> { "E1.dll", "E2.dll"};

			foreach(var s in source)
			{
				//載入模組(使用Reflection)
				Assembly a = Assembly.LoadFrom(s);

				//autofac 註冊流程
				ContainerBuilder c = new ContainerBuilder();
				c.RegisterAssemblyTypes(a).AsImplementedInterfaces();

				//實作
				using (var x = c.Build())
				{
					var eModule = x.Resolve<IEModule>();
					eModule.Print();
				}
			}

			Console.Read();
		}
	}
}
