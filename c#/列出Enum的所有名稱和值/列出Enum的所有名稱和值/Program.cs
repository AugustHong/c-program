using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace 列出Enum的所有名稱和值
{
    public enum Test1
    {
        A1 = 0,
        A2 = 1,
        A3 = 2
    }

    public class A
    {
        public string B { get; set; }

        public enum C
        {
            C1 = 1,
            C2 = 2,
            C3 = 3
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // 取得所有 NestedType(要是 public的)
            var enumTypes = typeof(A).GetNestedTypes(BindingFlags.Public);
            var pubEnums = enumTypes.Where(t => t.IsEnum);  // 判斷是否是 enum 型別
            foreach (var enumType in pubEnums)
            {
                Console.WriteLine($"EnumName:{enumType.Name}");

                List<string> enumNameList = enumType.GetEnumNames().ToList();
                var enumValues = enumType.GetEnumValues();

                for (var i = 0; i < enumNameList.Count(); i++)
                {
                    Console.WriteLine($"{enumNameList[i]} = {(int)enumValues.GetValue(i)}");
                }
            }

            Console.WriteLine("-----------------------------------------------------------------------");

            var enumTypes2 = typeof(Test1);
            Console.WriteLine($"EnumName:{enumTypes2.Name}");

            List<string> enumNameList2 = enumTypes2.GetEnumNames().ToList();
            var enumValues2 = enumTypes2.GetEnumValues();

            for (var i = 0; i < enumNameList2.Count(); i++)
            {
                Console.WriteLine($"{enumNameList2[i]} = {(int)enumValues2.GetValue(i)}");
            }


            Console.ReadLine();
        }
    }
}
