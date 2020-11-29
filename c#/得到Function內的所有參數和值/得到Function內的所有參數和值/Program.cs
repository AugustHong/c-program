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
    參考網圵： https://dotblogs.com.tw/H20/2017/12/05/121148
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
            MethodBase method = frames[0].GetMethod();  // 如果要取到上一個函式 用 frames[1].GetMethod() 後面會說到

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

        /// <summary>
        /// 取得 目前正在執行的 Function Info 資訊
        /// </summary>
        /// <returns></returns>
        public static String GetCurrentMethodInfo()
        {
            string showString = "";
            //取得當前方法類別命名空間名稱

            showString += "Namespace:" + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.Namespace + "\n";
            //取得當前類別名稱

            showString += "class Name:" + System.Reflection.MethodBase.GetCurrentMethod().DeclaringType.FullName + "\n";
            //取得當前所使用的方法

            showString += "Method:" + System.Reflection.MethodBase.GetCurrentMethod().Name + "\n";

            return showString;
        }

        /// <summary>
        /// 取得父類別的相關資訊(共用的Functiond可用)
        /// 用法類型 DoSomething 函式裡的 (所以才說 可以得到呼叫的上一層函式)
        /// 例如： A函式 呼叫此函式 會得到 A 函式的資料
        /// 註： 而且 如果寫在 Exception 中的話， 某個函式 throw new 你建的Exception ，就可以在 Exception 得到是哪個函式的資料了
        /// </summary>
        /// <returns></returns>
        public static String GetParentInfo()
        {
            String showString = "";
            StackTrace ss = new StackTrace(true);

            //取得呼叫當前方法之上一層類別(GetFrame(1))的屬性
            MethodBase mb = ss.GetFrame(1).GetMethod();

            //取得呼叫當前方法之上一層類別(父方)的命名空間名稱
            showString += mb.DeclaringType.Namespace + "\n";

            //取得呼叫當前方法之上一層類別(父方)的function 所屬class Name
            showString += mb.DeclaringType.Name + "\n";

            //取得呼叫當前方法之上一層類別(父方)的Full class Name
            showString += mb.DeclaringType.FullName + "\n";

            //取得呼叫當前方法之上一層類別(父方)的Function Name
            showString += mb.Name + "\n";

            return showString;
        }
    }

    /// 得到 函式 名稱的函式 (就是用 GetParentInfo() 來寫的 )
    /// 因為 別人呼叫此 Helper 的話，就得要得到上一層 Func 的資料才對
    /// 這邊只寫一個做範例，之後視情況自己照上面的內容擴充
    public static class GetFuncInfoHelper{
        public static string GetFuncName(){
            string result = string.Empty;

            StackTrace ss = new StackTrace(true);

            //取得呼叫當前方法之上一層類別(GetFrame(1))的屬性
            MethodBase mb = ss.GetFrame(1).GetMethod();

            //取得呼叫當前方法之上一層類別(父方)的Function Name
            result = mb.Name;

            return result;
        }
    }
}
