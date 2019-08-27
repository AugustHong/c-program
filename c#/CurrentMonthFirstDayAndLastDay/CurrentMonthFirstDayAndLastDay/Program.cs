using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CurrentMonthFirstDayAndLastDay
{
    class Program
    {
        static void Main(string[] args)
        {
            //參考網圵： https://dotblogs.com.tw/codeman/archive/2011/07/29/32281.aspx
            //取到當月的第一天和最後一天
            DateTime FirstDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime LastDay = new DateTime(DateTime.Now.AddMonths(1).Year, DateTime.Now.AddMonths(1).Month, 1).AddDays(-1);

            //算出這個月有幾天
            String yy = DateTime.Now.Year.ToString();
            String mm = DateTime.Now.Month.ToString();
            String days = DateTime.DaysInMonth(int.Parse(yy), int.Parse(mm)).ToString();

            Console.WriteLine(FirstDay);
            Console.WriteLine(LastDay);
            Console.WriteLine(days);

            Console.ReadLine();
        }
    }
}
