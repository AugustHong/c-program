using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 平常以下情況都用if 但是很不彈性
/// Allen Kuo ==> Allen(firstName) , Kuo(lastname)
/// Kuo, Allen ==>Allen(firstName) , Kuo(lastname)
/// </summary>
namespace EasyFactory_DesignPattern  //設計模式 之 "簡單工廠" pattern
    //相近的東西（工廠裡的都是相近的），每個class支援一個格式
{
    class Program
    {
        static void Main(string[] args){

            string name = "Allen Kuo";
            string name2 = "Kuo,Allen";

            UserName u1 = UserNameFactory.GetUserName(name);
            UserName u2 = UserNameFactory.GetUserName(name2);

            Console.WriteLine("firstName = {0}, LastName = {1}", u1.FirstName, u1.LastName);
            Console.WriteLine("firstName = {0}, LastName = {1}", u2.FirstName, u2.LastName);

            Console.Read();
        }
    }


    public class UserNameFactory
    {
        public static UserName GetUserName(string value)
        {
            if (value.IndexOf(",") > 0)
            {
                return new UserNameB(value);
            }
            else {
                return new UserNameA(value);
            }

            //也可寫成一行（但前面要加上轉型成UserName)
            //return value.IndexOf(",") > 0 ? (UserName)new UserNameB(value) : (UserName)new UserNameA(value);
        }
    }


    public class UserName{
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }

    public class UserNameA : UserName
    {
        public UserNameA(string value)
        {
            var arr = value.Split(' ');
            this.FirstName = arr[0];
            this.LastName = arr[1];
        }
    }

    public class UserNameB : UserName
    {
        public UserNameB(string value)
        {
            var arr = value.Split(',');
            this.FirstName = arr[1];
            this.LastName = arr[0];
        }
    }
}
