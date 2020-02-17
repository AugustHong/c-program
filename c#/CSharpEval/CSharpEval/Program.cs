using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

// 自訂測試用class
using TestC;

/*
    c# 執行 像是 js 的 Eval 的功能
    參考網圵： https://blog.csdn.net/kangrydotnet/article/details/44039481

    目標：加以擴充到 可以任意Class
*/

namespace CSharpEval
{
    // 要產生物件組出來的Model(負責參數)
    public class Arg
    {
        public Type _type;
        public string _name;

        /// <summary>
        /// 為了讓所有Class 都可以執行 => 所以改成接收泛型再轉成Type
        /// </summary>
        /// <typeparam name="T">傳入物件的類型</typeparam>
        /// <param name="paramName">物件名稱</param>
        public Arg TypeStringToTypeClass<T>(string paramName)
        {
            this._type = typeof(T);
            this._name = paramName;

            return this;
        }
    }

    // 自已加的 (要讓可以各 Class 都執行 => 要改 Arg)

    // 主程式
    public class CSharpEvalProvider
    {
        private string errorMessage = "";

        /// <summary>
        /// 得到錯誤訊息
        /// </summary>
        /// <returns>錯誤訊息</returns>
        public string getErrorMessage()
        {
            return this.errorMessage;
        }

        /// <summary>
        /// 產生 Method 供後面使用(可重複使用)
        /// </summary>
        /// <typeparam name="ReturnType">回傳的類型</typeparam>
        /// <param name="args">參數集合</param>
        /// <param name="code">要執行的code</param>
        /// <param name="usingStatements">using的集合(自己選擇加入的，預設為null)</param>
        /// <param name="assemblies">assembly的集合(自己選擇加入的，預設為null)</param>
        /// <returns></returns>
        public MethodInfo CreateMethod<ReturnType>(List<Arg> args, string code, IEnumerable<string> usingStatements = null, IEnumerable<string> assemblies = null)
        {
            // 產生 using 的 hashset (用於到時候組出上面的using)
            // 第一個先把 System 加上去
            HashSet<string> includeUsings = new HashSet<string>(new[] { "System" });

            // 再來加入 return類型的 Namespace
            Type returnType = typeof(ReturnType);
            includeUsings.Add(returnType.Namespace);

            // 把所有參數 組合起來；並且把他們要的using 加入進 includeUsings
            string argStr = "";

            foreach (var arg in args)
            {
                try
                {
                    Type t = arg._type;

                    // 加入 using 字串集合
                    includeUsings.Add(t.Namespace);

                    // 組出參數字串 => 會變成 ,string A, int b     這樣子
                    argStr += ", " + arg._type + " " + arg._name;
                }
                catch
                {
                    errorMessage = "uncompleted arg type: " + arg._type;
                    return null;
                }
            }

            // 如果有值的話， 把第一個 , 拿掉
            if (!string.IsNullOrEmpty(argStr))
            {
                argStr = argStr.Substring(2);
            }

            // 如果 使用者提供的 using 字串有值的話 => 加入進 includeUsings 裡
            if (usingStatements != null)
            {
                foreach (var usingStatement in usingStatements)
                {
                    includeUsings.Add(usingStatement);
                }
            }

            // 前面該組的都組完了，這邊要開始正式合成了
            MethodInfo method;
            using (CSharpCodeProvider compiler = new CSharpCodeProvider())
            {
                // 產生隨機的namespace
                var name = "F" + Guid.NewGuid().ToString().Replace("-", string.Empty);

                // 加入組件(有using，但沒組件會無法執行)， 第一個加入的就是 system.dll
                var includeAssemblies = new HashSet<string>(new[] { "system.dll" });

                // 加入自選的組件
                if (assemblies != null)
                {
                    foreach (var assembly in assemblies)
                    {
                        includeAssemblies.Add(assembly);
                    }
                }

                // 把 上面的組件 編譯 起來
                // 如果要用到 特別的Class 的話，要記得自己把 組件寫到 assemblies 裡喔
                var parameters = new CompilerParameters(includeAssemblies.ToArray())
                {
                    GenerateInMemory = true
                };

                // 要新建出一個 Class
                string source = string.Format(@"
                    // using
                    {0}

                    namespace {1}
                    {{
	                    public static class EvalClass
	                    {{
		                    public static {2} Eval({3})
		                    {{
                                // code
			                    {4}
		                    }}
	                    }}
                    }}"
                    , GetUsing(includeUsings), name, returnType.Name, argStr, code);

                // compiler 組件 + 上面這個 範本
                var compilerResult = compiler.CompileAssemblyFromSource(parameters, source);
                // 得到 組件
                var compiledAssembly = compilerResult.CompiledAssembly;
                // 得到我們的 EvalClass 的 Type (因為要執行 函式)
                Type type = compiledAssembly.GetType(string.Format("{0}.EvalClass", name));
                // 得到 Eval 這隻 Method 的變數 => 之後就要執行這個函式要用到
                method = type.GetMethod("Eval");
            }
            return method;
        }

        /// <summary>
        ///  跟上面類似，但不用自己用 => 直接灌參數進來
        ///  適用於： 只用1次這Method的即可
        /// </summary>
        /// <typeparam name="ReturnType">回傳的類型</typeparam>
        /// <param name="args">參數集合</param>
        /// <param name="code">要執行的code</param>
        /// <param name="objList">傳進來的參數各值</param>
        /// <param name="usingStatements">using的集合(自己選擇加入的，預設為null)</param>
        /// <param name="assemblies">assembly的集合(自己選擇加入的，預設為null)</param>
        /// <returns></returns>
        public ReturnType Eval<ReturnType>(List<Arg> args, string code, List<object> objList, IEnumerable<string> usingStatements = null, IEnumerable<string> assemblies = null)
        {
            // 原本是想放在 Arg 裡面多放一個值 => 但想到不要依賴性太高 => 分開來做
            try
            {
                MethodInfo method = this.CreateMethod<ReturnType>(args, code, usingStatements, assemblies);

                if (method == null) { return default(ReturnType); }

                return (ReturnType)method.Invoke(null, objList.ToArray());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return default(ReturnType);
            }
        }

        // 組出 Using 字串 (就最上面的 using)
        private string GetUsing(HashSet<string> usingStatements)
        {
            StringBuilder result = new StringBuilder();
            foreach (string usingStatement in usingStatements)
            {
                result.AppendLine(string.Format("using {0};", usingStatement));
            }
            return result.ToString();
        }
    }


    class Program
    {
        static void Main(string[] args)
        {
            // 開始執行
            CSharpEvalProvider eval = new CSharpEvalProvider();
            List<Arg> argList = new List<Arg>();

            // 注入 參數相關
            argList.Add(new Arg().TypeStringToTypeClass<int>("a"));
            argList.Add(new Arg().TypeStringToTypeClass<int>("b"));
            argList.Add(new Arg().TypeStringToTypeClass<string>("c"));

            // 開始執行
            var method = eval.CreateMethod<string>(argList, @"return ""Hello world "" + (a + b) + c;");
            object result = method.Invoke(null, new object[] { 2, 2, " never mind！" });
            Console.WriteLine((string)result);

            // ------上面執行是可以成功的，換下面(自訂Class的用法)-----------------------

            // 注入 參數相關
            List<Arg> argList2 = new List<Arg>();
            argList2.Add(new Arg().TypeStringToTypeClass<TestClass>("test"));

            // 因為用自己的 Class (所以要把組件加上去)
            // 如果要用自己的，記得把它們都特別抽去另一隻變成dll ， 才能執行
            List<string> ass = new List<string>();
            ass.Add("TestC.dll");

            TestClass t = new TestClass();
            t.A = "這是測試代碼：";
            t.B = 168;

            // 開始執行
            var method2 = eval.CreateMethod<string>(argList2, @"return test.A + ""  "" + test.B.ToString();", assemblies: ass);
            object result2 = method2.Invoke(null, new object[] { t });
            Console.WriteLine((string)result2);

            // -----------第3種(自已多寫的： 省略下面的步驟)-------------------------------

            // 如果想要回傳的是 void 的話 => 就讓它回傳 int ； 最後再用 return 1;  即可
            // 這次用沒傳參數的
            List<Arg> argList3 = new List<Arg>();
            List<object> obj = new List<object>();
            var result3 = eval.Eval<int>(argList3, @" Console.WriteLine(""這是第3次的測試""); return 0;", obj);
            Console.WriteLine(result3);


            Console.ReadLine();
        }
    }
}
