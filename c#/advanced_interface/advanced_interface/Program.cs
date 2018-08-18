using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advanced_interface
{
    public delegate void Dxxx();

    public interface Ixxx{
        //裡面類似抽象類別，只宣告不實作（不能寫public pravate……等）
        //interface裡是不能宣告field的，但可以宣告propoty
        
        string Email { get; set; }  //class也要實作出來
        void doA();
        event Dxxx DataChanged;  //可以寫event
        event EventHandler ShapeChanged;  //EventHandler是內建的
    }


    public class A : Ixxx{  //如果要繼承又實作interface，則第一個寫類別後面用,介面  例如： public class A: class, interface1, interface2
        public string Email { get; set; }
        public void doA(){  //一定會是public
            Console.WriteLine("我實作了Ixxx的doA()");
        }

        public event Dxxx DataChanged;
        public event EventHandler ShapeChanged;

        //明確實作：先將繼承Ixxx的轉成Ixxx再執行其寫的內容  例如：public string Ixxx.hello(){Console.Write("hello");}
    }


    class Program
    {
        static void Main(string[] args){
            Ixxx obj = new A();
            obj.doA();




            Console.Read();
        }
    }
}
