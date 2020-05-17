using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

/*
    這是一個 只要寫好 Struct ，就可以把 字串 依照長度 轉進 各 變數中 

    參考網圵：
    https://dotblogs.com.tw/larrynung/2011/03/19/21957
*/

namespace StructLayout
{
    #region 宣告 Class

    // 單層的
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class MyStruct
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string fname;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string lname;
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 7)]
        public string phone;
    }

    // 雙層的
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class A
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 4)]
        public string B;

        // 因為他是 下一個的 結構 (第二層) => 所以不能設定長度
        public C C;
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
    public class C
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 1)]
        public string D;

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 2)]
        public string E;
    }

    #endregion

    class Program
    {
        static void Main(string[] args)
        {
            // 先轉 一個 的
            MyStruct ms = ConvertToStructLayout<MyStruct>("abcd12341234567");
            Console.WriteLine("fname is: {0}", ms.fname);
            Console.WriteLine("lname is: {0}", ms.lname);
            Console.WriteLine("phone is: {0}", ms.phone);

            Console.WriteLine("--------------------------------------------------------------------------");

            // 這個是轉 成 List<T> 的 ，後面的數字就是你要用多少來切你的字串
            List<MyStruct> msList = ConvertToStructLayout<MyStruct>("abcd12341234567cc cd dd0000000", 15);
            Console.WriteLine(msList.Count());

            for (var i = 0; i < msList.Count(); i++)
            {
                Console.WriteLine("第 {0} 筆的 fname is:  {1}", i, msList[i].fname);
                Console.WriteLine("第 {0} 筆的 lname is:  {1}", i, msList[i].lname);
                Console.WriteLine("第 {0} 筆的 phone is:  {1}", i, msList[i].phone);
            }


            Console.WriteLine("--------------------------------------------------------------------------");

            // 轉換多層式的
            A a = ConvertToStructLayout<A>("BBBBdee");
            Console.WriteLine("A-B is {0}", a.B);
            Console.WriteLine("A-C-D is {0}", a.C.D);
            Console.WriteLine("A-C-E is {0}", a.C.E);

            Console.WriteLine("--------------------------------------------------------------------------");

            List<string> fieldNameList = GetAllFieldNameList<MyStruct>();
            foreach (var fieldName in fieldNameList)
            {
                int stringLen = GetStructLayoutStringLenByFieldName<MyStruct>(fieldName);
                Console.WriteLine($"FieldName = {fieldName}, StringLen = {stringLen}");
            }

            Console.WriteLine("--------------------------------------------------------------------------");

            fieldNameList = GetAllFieldNameList<A>();
            foreach (var fieldName in fieldNameList)
            {
                int stringLen = GetStructLayoutStringLenByFieldName<A>(fieldName);
                Console.WriteLine($"FieldName = {fieldName}, StringLen = {stringLen}");
            }

            Console.WriteLine("--------------------------------------------------------------------------");

            fieldNameList = GetAllFieldNameList<C>();
            foreach (var fieldName in fieldNameList)
            {
                int stringLen = GetStructLayoutStringLenByFieldName<C>(fieldName);
                Console.WriteLine($"FieldName = {fieldName}, StringLen = {stringLen}");
            }

            Console.WriteLine("--------------------------------------------------------------------------");

            MyStruct ms4 = ConvertToStructLayout<MyStruct>("abc 12 4   4567");
            ms4.fname = "aa";
            ms4.lname = "bb";
            ms4.phone = "123";
            Console.WriteLine($"沒有補空白的 {GetStructLayoutParentString<MyStruct>(ms4)}");
            Console.WriteLine($"有補空白的 {GetStructLayoutParentString<MyStruct>(ms4, true)}");

            Console.WriteLine("----------------------------------------------------------------------------");

            // 將值為設為空白
            MyStruct ms5 = SetStructLayoutAllFieldValueISSpace<MyStruct>();

            Console.ReadLine();
        }

        #region 方法 (除第1個外，其他都是我擴充的)

        /// <summary>
        ///  做轉換 (如果超過 字元上限，只會轉前面)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <returns></returns>
        static T ConvertToStructLayout<T>(string val) where T : class
        {
            if (!string.IsNullOrEmpty(val))
            {
                IntPtr valPoint = Marshal.StringToBSTR(val);
                T ret = (T)Marshal.PtrToStructure(valPoint, typeof(T));
                Marshal.FreeBSTR(valPoint);
                return ret;
            }
            else
            {
                return (T)Activator.CreateInstance(typeof(T));
            }
        }

        /// <summary>
        ///  做轉換 (會依據你輸入的 splitLen 來切割， 並轉成 List<T>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="val"></param>
        /// <param name="splitLen"></param>
        /// <returns></returns>
        static List<T> ConvertToStructLayout<T>(string val, int splitLen) where T : class
        {
            if (splitLen > 0 && !string.IsNullOrEmpty(val))
            {
                int currentIndex = 0;
                int stringLen = val.Length;

                List<T> result = new List<T>();

                while (currentIndex < stringLen)
                {
                    string item = val.Substring(currentIndex, splitLen);

                    result.Add(ConvertToStructLayout<T>(item));

                    currentIndex += splitLen;
                }

                return result;
            }
            else
            {
                return new List<T>();
            }
        }

        // ------------------ 以下都是 For 單層的。多層較少使用 且較為麻煩，能不用盡量不要用

        /// <summary>
        ///  得到 List FieldInfo 
        ///  注意： 一般是得到 Property ，但 StructLayout 只能用 Field => 所以才用 得到 All Field
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static List<FieldInfo> GetClassFieldInfoList<T>()
        {
            try
            {
                T t = (T)Activator.CreateInstance(typeof(T));
                List<FieldInfo> fieldInfoList = t.GetType().GetFields().ToList();
                return fieldInfoList;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return new List<FieldInfo>();
            }
        }

        /// <summary>
        ///  得到 FieldInfo ， 但用 name 來查詢
        ///  結果有可能是 null 喔
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        static FieldInfo GetClassFieldInfoByName<T>(string name)
        {
            try
            {
                T t = (T)Activator.CreateInstance(typeof(T));
                return t.GetType().GetField(name);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return null;
            }
        }

        /// <summary>
        ///  得到 所有的 FieldName 的 List
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        static List<string> GetAllFieldNameList<T>()
        {
            return GetClassFieldInfoList<T>().Select(x => x.Name).ToList();
        }

        /// <summary>
        ///  我們在 StructLayout 中有設定 字串長度，接下來這邊告訴你怎麼取到
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="name"></param>
        /// <returns></returns>
        static int GetStructLayoutStringLenByFieldName<T>(string name)
        {
            int result = 0;
            FieldInfo info = GetClassFieldInfoByName<T>(name);
            if (info != null)
            {
                // 取法是我用監看式得到的 (雖然寫的很死， 但 如果你都正常的用應該是不用改)
                var customerAttr = info.CustomAttributes.FirstOrDefault();
                if (customerAttr != null)
                {
                    try
                    {
                        result = (int)customerAttr.NamedArguments[2].TypedValue.Value;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        return 0;
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///  跟上面一樣的作用 只是這是傳入 FieldInfo 資料
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="info"></param>
        /// <returns></returns>
        static int GetStructLayoutStringLenByFieldInfo<T>(FieldInfo info)
        {
            int result = 0;
            if (info != null)
            {
                // 取法是我用監看式得到的 (雖然寫的很死， 但 如果你都正常的用應該是不用改)
                var customerAttr = info.CustomAttributes.FirstOrDefault();
                if (customerAttr != null)
                {
                    try
                    {
                        result = (int)customerAttr.NamedArguments[2].TypedValue.Value;
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.Message);
                        return 0;
                    }
                }
            }

            return result;
        }

        /// <summary>
        ///  把這個物件裡面的 Field 全部的值組起來
        ///  isAddSpace = 是否要加入空白(例如：字元長度上限為3 ，但你的值只有2位 ，要多補一個空白嗎)
        ///  預設 isAddSpace = false
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <param name="isAddSpace"></param>
        /// <returns></returns>
        static string GetStructLayoutParentString<T>(T source, bool isAddSpace = false)
        {
            string result = string.Empty;

            try
            {
                List<FieldInfo> infoList = GetClassFieldInfoList<T>();
                foreach (var info in infoList)
                {
                    string value = (string)info.GetValue(source);
                    value = string.IsNullOrEmpty(value) ? string.Empty : value;

                    string tmp = string.Empty;

                    if (isAddSpace)
                    {
                        int stringLen = GetStructLayoutStringLenByFieldInfo<T>(info);

                        /*
                            不滿特定長度的字串，後面補空白
                            String.Format(“{0,-10}”, “Hello”);    //「Hello     」
                            參考網圵：http://jashliao.eu/wordpress/2016/06/15/c-string-format-%E8%A3%9C-0%E7%A9%BA%E7%99%BD/
                         */
                        string format = "{0,-" + stringLen.ToString() + "}";
                        tmp = String.Format(format, value);
                    }
                    else
                    {
                        tmp = value;
                    }

                    result += tmp;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return string.Empty;
            }

            return result;
        }

        /// <summary>
        ///  設定 讓 傳入的 source 裡面的值 全變成 空白
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        static T SetStructLayoutAllFieldValueISSpace<T>()
        {
            try
            {
                T t = (T)Activator.CreateInstance(typeof(T));
                List<FieldInfo> infoList = GetClassFieldInfoList<T>();

                foreach (var info in infoList)
                {
                    string v = string.Empty;
                    info.SetValue(t, v);
                }

                return t;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                return (T)Activator.CreateInstance(typeof(T));
            }
        }
        #endregion
    }
}
