using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace @interface
{
    //一樣寫在外面
    interface ISampleInterface{
        void SampleMethod();
    }

    interface IPoint{
        // Property signatures:
        int x{get;set;}
        int y{get;set;}

        void x_add_y();
    }

    class Point : IPoint{
        private int _x;
        private int _y;


        // Constructor:
        public Point(int x, int y){
            _x = x;
            _y = y;
        }

        // Property implementation:
        public int x{
            get{return _x;}
            set{ _x = value;}
        }

        public int y{
            get{return _y;}
            set{_y = value;}
        }

        void IPoint.x_add_y(){   //上面有宣告的要寫在自己class中
            Console.WriteLine(_x + _y);
        }
    }

    interface IControl{
        void Paint();
    }
    interface ISurface{
        void Paint();
    }

    class SampleClass : IControl, ISurface{
        public void Paint(){
            Console.WriteLine("Paint method in SampleClass");
        }
    }

    class SampleClass2 : IControl, ISurface
    {
        void IControl.Paint(){
            Console.WriteLine("Paint method in IControl");
        }
        void ISurface.Paint(){
            Console.WriteLine("Paint method in ISurface");
        }
        public void Paint(){
            Console.WriteLine("Paint method in SampleClass");
        }
    }


    interface ILeft{
        int P { get; }
    }
    interface IRight{
        int P();
    }

    class Middle : ILeft, IRight{
        public int P() { return 0; }
        int ILeft.P { get { return 0; } }
    }




    class Program:ISampleInterface
    {
        //interfacee可被class繼承
        void ISampleInterface.SampleMethod(){
            Console.WriteLine("this is ISampleInterface.SampleMethod()");
        }

        static void PrintPoint(IPoint p){  //跟上面不同的是多了static ，因為上方在interface時有宣告過，而Ipoint沒有（因是寫在沒被繼承的class中）
            Console.WriteLine("x={0}, y={1}", p.x, p.y);
        }

        static void Main(string[] args){

            //注意看格式   介面名稱 變數 = new 繼承此介面的class;
            ISampleInterface obj = new Program();
            obj.SampleMethod();

            //看以下沒有差異（是相同的）
            IPoint p = new Point(2, 3);
            Console.Write("My Point: ");
            PrintPoint(p);
            p.x_add_y();

            Point p1 = new Point(2, 3);
            Console.Write("My Point: ");
            PrintPoint(p1);
            //p1.x_add_y() 無此法，因為p1並非為IPoint interface類型，所以要像下面這樣子 ex: IPoint p2 = (IPoint)p1;
            //p2.x_add_y()


            SampleClass sc = new SampleClass();  
            //跟class原本有很大的差異，照理來說自己內的函式自己可以觸發，但現在因繼承interface變成要轉才能觸發
            //（因為上方函式是寫IPoint.x_add_y()，如自己再寫一個x_add_y()則自己可觸發自己的）
            IControl ctrl = (IControl)sc;
            ISurface srfc = (ISurface)sc;

            // The following lines all call the same method.（結果皆相同）
            sc.Paint();
            ctrl.Paint();
            srfc.Paint();

            Console.WriteLine("__________________________________________________________________");

            //_________________________________________________________________________這次做不一樣的改變，答案就不同了
            SampleClass2 sc2 = new SampleClass2();
            IControl ctrl2 = (IControl)sc2;
            ISurface srfc2 = (ISurface)sc2;

            sc2.Paint();
            ctrl2.Paint();
            srfc2.Paint();


            Console.Read();

        }
    }
}
