using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//委派
namespace delegate_test
{
    delegate int xxx(int price); //先宣告一個委派，叫作xxx他會傳入一個price回傳int
    //所以傳入的func一定要是只有一個參數且是int，回傳也要int的func才可以
    class Order{
         
        public int GetPrice(xxx func ,int price) { //等同於 public int GetPrice(Func<int, int> func, int price){
            return func(price);
        }
    }



    class Program
    {
        static void Main(string[] args){

            Order obj = new Order();
            int price = 100;

            Console.WriteLine("原價：{0}", price);
            Console.WriteLine("5折：{0}元", obj.GetPrice(promoA, price));
            Console.WriteLine("滿萬送千：{0}元", obj.GetPrice(promoB, price));

            //較正規的寫法
            xxx handler = new xxx(promoB);
            Console.WriteLine(obj.GetPrice(handler, price));

            var pro = new Func<int, int>(promoB);  //等同上方的 xxx handler = new xxx(promoB)


            Console.WriteLine("_________________________________________________________________");
            Console.WriteLine("用陣列來跑的：");

            xxx[] xx = { promoA, promoB };

            foreach(xxx x in xx) { Console.WriteLine(obj.GetPrice(x, price)); }
            Console.WriteLine("_________________________________________________________________");

            //內建泛型委派
            //有傳回值的 Func<T, TResult>  例如：Func<int, int> 最右邊是回傳的，其餘都是參數  例如：Func<int, string, bool>代表傳入一個int和一個string，且傳回bool
            //沒傳回值的 Action<T>


            //特別寫法
            Func<int, int> promoa = delegate (int money) { return money; };
            Func<int, int> promoC = (int money) => { return money; };  //這和上面是一樣的
            Func<int, int> promoD = (money) => { return money; }; //和上面是一樣的

            //如果只有一行且參數只有一個，可以這樣寫
            Func<int, int> promoE = money => money;  //此等同上面的（例參數只有一個，且只寫一行=>所以return 拿掉了）

            //______________________________________________________________________________________________________


            Console.Read();
        }


        static int promoA(int price) { return price / 2; } //5折
        static int promoB(int price) { return price >= 10000 ? price - 1000 : price ; } //滿萬送千
    }
}
