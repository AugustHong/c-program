using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection; //assembly會用到的
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 僅做測試使用（一般來說是要拿dll裡的class中的mathod，但這裡只是測試，所以把class寫在這裡）
/// 還沒研究出寫法
/// 這個執行會是錯的喔！
/// 這個執行會是錯的喔！
/// 這個執行會是錯的喔！
/// </summary>
//用字串"Member" new 出 Member class （或者mathod也行）
namespace Reflection
{
    public class A
    {
        public int doA() { return -1; }
        public int doA(int x, int y) { return x + y; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            string dllName = "Reflection";
            string className = "Reflection.A";

            //建立物件
            Assembly assembly = Assembly.Load(dllName);
            var obj = assembly.CreateInstance(className); //等同於 new xxx，但型別一定要用var，其實它是object 型別
            //所以要轉型（但為了活一點，會轉成interface => 例如 var obj = (Ixxx)assembly.CreateInstance(className);

            //呼叫doA()
            string mathodName = "doA";
            Type type = obj.GetType();

            //我們想要的是有2個int參數的doA（而不是沒參數的doA）
            Type[] typeOfArgs = new Type[] { typeof(int), typeof(int) };
            MethodInfo methodInfo = type.GetMethod(mathodName, typeOfArgs);

            //呼叫mathod
            object[] parameters = new object[] { 1, 99 };
            object result = methodInfo.Invoke(obj, parameters);
            Console.WriteLine(result);


            Console.Read();

        }
    }
}
