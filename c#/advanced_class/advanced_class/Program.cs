using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace advanced_class
{
    //class event（事件）
    //繼承時，事件會有點問題
    //會變成obj.DataChanged += 還是可以正常運作（因它會依父類別做父類別的事）
    //但是在子類別裡面不能寫DataChanged() => 把 它寫成一個mathod（在父類別中寫），讓子來呼叫
    delegate void DataChangedEventHandler();
    delegate void DataChangedEventHandler2(Member m);  //裡面可放參數的，而放自己的class到時傳參數就可以放this
    class Member
    {
        public event DataChangedEventHandler DataChanged;  //事件寫法   public event 委派名 變數名
        public event DataChangedEventHandler2 DataChanged2;

        private string _name;
        public string Name { get { return _name; }
            set { _name = value;
                Console.WriteLine(value);
                if (DataChanged != null && DataChanged2 != null) { DataChanged(); DataChanged2(this); }//那DataChanged()是讓它觸發
            }
        }
    }


    //子類別繼承父類別，在其裡面打override後面就會自動跑出你能覆寫哪些

    //---------------------------------------------------------------------------------------------------------

    //抽象類別（不能被new）
    abstract public class Animal{
        public void Save(string content) { }  //正常的
        public abstract void Read(string path);   //抽象mathod，不用{} 但前面加上abstract
        //有設定抽像mathod的，其繼承他的子類別一定要實作出這個mathod
    }

    //partical class 在"不同檔案"有相同class，編議後會自動合併（但裡面的mathod不能名字一樣，除非那mathod也是partical）

    //擴充方法（class一定要是靜態的，且要擴充的mathod也一定要是靜態的，再者第一個參數前面一定要加this而那個string是代表我們要擴充string這個）
    static class ExtString{
        public static string myLeft(this string value) { return value; }
        //此value即是a本身（所以可以value.xxx）
        public static string myLeft(this string value, int length) { return value + " " +length.ToString(); }
    }


    class Program
    {
        static void Main(string[] args){

            Member obj = new Member();

            //先設定事件
            obj.DataChanged += Obj_DataChanged;  //寫obj.DataChanged += 再按2下tab下面的private static void Obj_DataChanged()就會跑出來了

            obj.DataChanged += Obj_DataChanged2;  //一個事件可以被觸發1次以上
            obj.DataChanged += myfunc;  //自己寫的函式（但要跟著其委派的格式）

            obj.DataChanged2 += Obj_DataChanged21;

            obj.Name = "Jack";

            Console.WriteLine("----------------------------------------------------------------------");

            string a = "AAA";
            Console.WriteLine(a.myLeft());
            Console.WriteLine(a.myLeft(10));


            Console.Read();
        }

        private static void Obj_DataChanged21(Member m)
        {
            Console.WriteLine("{0}  呼叫Obj_DataChanged21", m.Name);
        }

        private static void Obj_DataChanged(){
            Console.WriteLine("有資料被動過了");
        }

        private static void Obj_DataChanged2()
        {
            Console.WriteLine("有資料被動過2次了");
        }

        public static void myfunc() { Console.WriteLine("hello"); }
    }
}
