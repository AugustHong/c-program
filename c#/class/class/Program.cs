using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace @class
{
    //一樣放在最上面
    class People{

        public int _age { get; set; }
        public string _name { get; set; }

        public People(){    //以下2個函式都是people，但是裡面參數不同（同vb一樣可以這樣做）
            _name = "王小明";
            _age = 10;
        }

        public People(int age, string name){
            _age = age;
            _name = name;
        }

        public void Eat(string food){   //這邊會要求一定要有回傳值，選擇void就是沒回傳值
            Console.WriteLine("{0} eat {1}", _name, food);
        }
    }

    //public override void Eat() 可以寫在 Student 和  Teacher中，此為覆寫，就是改掉寫成自己的function
    //原理都懂就不實作了

    class Student : People{  //繼承（學生也是人，所以學生會繼承人會做的事）
                            
        public int ID { get; set; }  //Properties的做法（而Property就是會可以有get() 和 set()函數去設定和取值）

        public void Study(string subject)
        {
            Console.WriteLine("學號{0} {1}同學在讀{2}科目", ID, this._name, subject);  //繼承後可以拿到自己的名字
        }

        public Student(int age, string name){
            this._age = age;
            this._name = name;
        }

        public Student() { }
    }

    class Teacher : People{
        public string main_subject { get; set; }  //Properties的做法（而Property就是會可以有get() 和 set()函數去設定和取值）

        public void Teach(Student student)
        {
            Console.WriteLine("{0}老師 {1}正在教{2}同學", main_subject, this._name, student._name);
        }

        public Teacher(int age, string name)
        {
            this._age = age;
            this._name = name;
        }
    }

    class Animal
    {
        private double height;

        public double Height
        {
            get { return height; }
            set { height = value + 50; }   //設定時身高都上升50
        }
    }


    class Member
    {
        //field = 欄位
        public string Name; //姓名
        private string _idCard; //身份證字號

        public int id { get; set; }  //property沒自訂

        public string IdCard
        {    //property自訂（裡面還可再多加）
            get { return _idCard; }
            set { _idCard = value; }
        }

        private string _phone;
        public string phone
        {  //property自訂
            get { return _phone; }
            set { _phone = "+886 " + value; }
        }

        public DateTime Birthday { get; set; }
        public int age { get { return DateTime.Today.Year - Birthday.Year; } }

        //mathod（打3個/可以打詳細註解）
        public void GetAllData()
        {
            Console.WriteLine("id={0} Name={1} IdCard={2} phone={3} birthday={4} age={5}", this.id, this.Name, this.IdCard, this.phone, this.Birthday.ToString("yyyy-MM-dd"), this.age);
        }

        /// <summary>
        /// 讓x和y相加
        /// </summary>
        /// <param name="x">第一個數字</param>
        /// <param name="y">第二個數字</param>
        /// <returns></returns>
        public int doAdd(int x, int y)
        {
            return x + y;
        }

        public int doAdd(int x, int y, int z){
            return x + y + z;
        }

        //靜態的mathod，所以不用new就可以用
        public static int doAdd(int x, int y, int z, int w) { return x + y + z + w; }

        //mathod只能傳一值，但可用out和ref
        public bool TrySave(int id, ref string errMessage){
            if(id < 0) { errMessage = "id <0"; return false; }
            return true;
        }

        public void Save2(int id) {
            if(id < 0) { throw new Exception("id<0"); }
            //用這個   throw new ArgumentOutOfRangeException("id")   更好
        }

        //public override string ToString() 覆寫原有的function


    }

        class Program
    {
        static void Main(string[] args){

            People p1 = new People();
            People p2 = new People(30, "王大明");

            Console.WriteLine("{0} {1}", p1._age, p1._name);
            Console.WriteLine("{0} {1}", p2._age, p2._name);

            p1._age = 15;
            p1._name = "陳小胖";
            Console.WriteLine("{0} {1}", p1._age, p1._name);

            p1.Eat("noodles");
            p2.Eat("steak");

            Console.WriteLine("_____________________________________________________________________________");

            Student s1 = new Student();
            s1._name = "Marry";
            s1._age = 5;
            s1.ID = 2;

            Student s2 = new Student(10, "jack");
            s2.ID = 10;

            Console.WriteLine("{0} {1} {2}", s1._age, s1._name, s1.ID);
            Console.WriteLine("{0} {1} {2}", s2._age, s2._name, s2.ID);

            s1.Study("math");
            s2.Study("chinese");

            s1.Eat("rice");    //因為有繼承，所以可以用他的function
            s2.Eat("fried chicken");

            Console.WriteLine("____________________________________________________________________________-");

            Teacher t1 = new Teacher(35, "Wu");
            Teacher t2 = new Teacher(65, "Chen");

            t1.main_subject = "history";
            t2.main_subject = "english";

            Console.WriteLine("{0} {1} {2}", t1._age, t1._name, t1.main_subject);
            Console.WriteLine("{0} {1} {2}", t2._age, t2._name, t2.main_subject);

            t1.Eat("烤肉");
            t2.Eat("果汁");

            t1.Teach(s1);
            t2.Teach(s2);


            Console.WriteLine("_____________________________________________________________________");
            //高階方法（繼承），如果要列出以上的name，則把他們的型態都化為父類別

            People[] p = { p1, p2, s1, s2, t1, t2};  //因為他們都繼承People，所以可以合成People，但是裡面的變數和方法也只能用People的（因為型態是People）
            
            foreach(People person in p)
            {
                Console.WriteLine(person._name);
            }

            Console.WriteLine("_____________________________________________________________________");

            Animal a1 = new Animal();
            a1.Height = 50;
            Console.WriteLine(a1.Height);


            Console.WriteLine("_____________________________________________________________________________");
            var member1 = new Member();
            member1.Name = "Jack";
            member1.id = 1;
            member1.IdCard = "A123456789";
            member1.phone = "12345678";
            member1.Birthday = new DateTime(1988, 1, 1);
            member1.GetAllData();

            Console.WriteLine(member1.doAdd(3, 5));

            Console.WriteLine("______________________________________________________________________");

            string errMessage = "";
            bool result = member1.TrySave(-1, ref errMessage);
            Console.Write("{0} => ", result);
            if (!result) { Console.WriteLine(errMessage); }

            Console.WriteLine("___________________________________________________________________");

            try{
                member1.Save2(-1);
            }catch (Exception ex){
                Console.WriteLine(ex);
            }

            Console.WriteLine("____________________________________________________________________");

            //靜態的方法，所以不用new
            Console.WriteLine(Member.doAdd(2, 4, 8, 16));

            Console.Read();

        }
    }
}
