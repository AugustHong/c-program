using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

/*
    參考網圵： https://bbs.csdn.net/topics/390797738 
*/

namespace 得到Function內的所有參數和值
{
    class Program
    {
        static void Main(string[] args)
        {
            TestFunction t = new TestFunction();
            string output;
            t.DoSomething("A", 1, DateTime.Now, out output);
            Console.Read();
        }
    }

    public class TestFunction
    {
        /// <summary>
        ///  測試的 函式
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="d"></param>
        /// <param name="output"></param>
        /// <param name="de"></param>
        public void DoSomething(string a, int b, DateTime d, out string output, decimal de = 100)
        {
            // 這邊可以取到 此DoSomething 函式裡的 所有變數的名稱 和 type
            StackTrace t = new StackTrace();
            StackFrame[] frames = t.GetFrames();
            MethodBase method = frames[0].GetMethod();

            ParameterInfo[] infos = method.GetParameters();

            foreach (ParameterInfo info in infos)
            {
                Type parType = info.ParameterType;
                string parName = info.Name;
                bool IsOut = info.IsOut;   // 是否是 out 參數
                object defaultValue = info.DefaultValue;  // 預設值
                bool isHaveDefaultValue = info.HasDefaultValue;  // 是否有預計值
                int position = info.Position;   // 是第幾個參數 (從 0開始)

                Console.WriteLine($"第 {position + 1} 個參數 --> Type: {parType} , Name: {parName} , 是否是 out 變數 ： {IsOut}");
            }

            /*
                 目前找了很多，都是找不到 值 如何取得
             */

            output = "Success";
        }
    }
}
