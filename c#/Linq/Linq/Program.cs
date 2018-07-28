using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Linq
{

    public class AB<T>{
        public AB(){}

        public void test(T test){
            T t = default(T);
            Console.WriteLine(test);
            if(test is Car) { t = test; Console.WriteLine(t); }
        }

    }

    public class Car {
        private string _name;

        public Car(string name) { _name = name; }
        public string getname() { return _name; }
    }

    public class Bus {
        public int id { get; set; }
        public string from { get; set; }
    }


    class Program
    {

        delegate string ConverMethod(string s);  //委派方法

        static void Main(string[] args){

            Car c = new Car("a");
            Console.WriteLine(c.getname());
            
            AB<Car> ab = new AB<Car>();
            ab.test(c);  //這邊就傳入的是Class 型別

            Console.WriteLine("");
            //____________________________________________________________________________________________________

            var result = new { ID = 1, name = "hello", car = new Car("bizz") };
            Console.Write("ID = {0}  name = {1}  car = {2}", result.ID, result.name, result.car.getname());


            Console.WriteLine("");
            Console.WriteLine("");
            //_____________________________________________________________________________________________________

            ConverMethod t = d => d.ToUpper();  //或者可以寫個function讓他等同 ConverMethod t = function();
            Console.WriteLine(t("abc"));

            ConverMethod t2 = d => d.ToLower();
            Console.WriteLine(t2("ABC"));

            //____________________________________________________________________________________________________
            Console.WriteLine("");

            var j = from q in "abcdefghijklmnopqrstuvwxyz".ToCharArray()   //要把字串做linq要加上 ToCharArray()
                    where Convert.ToInt32(q) >= 105
                    select q;            

            foreach (var i in j) { Console.Write(i); }

            Console.WriteLine("");
            Console.WriteLine("");

            string enstr = "abcdefghijklmnopqrstuvwxyz";
            var re = enstr.ToArray().Where(a => a == 'j');   //字串要做where要先ToArray()
            //就算資料只有一筆還是不能打 console.writeline(re);  會跑出一串東西  =>解法：(1)用foreach跑(2)console.writeline(re.first();)
            
            //select new物件
            var k = from q in "abcdefghijklmnopqrstuvwxyz".ToCharArray()   //要把字串做linq要加上 ToCharArray()
                    where Convert.ToInt32(q) > 114
                    select new { ID = Convert.ToInt32(q), ch = q};

            foreach(var m in k) { Console.WriteLine("{0} {1}", m.ID, m.ch); }
            Console.WriteLine("上面的id數字相加= {0}", k.Sum(g => g.ID));

            Console.WriteLine("");
            //_______________________________________________________________________________________________________


            //以下如拿掉註解後不能跑，代表是老師自己寫的東西（例如：函式 類別）

            //讀取xml  先在最上方加入 using System.Xml.Linq
            //XDocument doc = XDocument.Load(路徑);
            //var result2 = from xml in doc.Root.Descendants("DetailDataClass")
            //              where xml.Element("CertificateNo").Value.Contains("DHK09801")  //讀取<CertificateNO></CertificateNo>中的值有跑含DHK09801的資料
            //              select new{no = xml.Element("ProcessingNo").Value};


            //讀取excel
            //SpreadsheetDocument是老師自己寫的東西（前非一開始就有的）
            //SpreadsheetDocument excelDoc = SpreadsheetDocument.Open("路徑", false);
            // var result = from row in GetDataTableBySheetName(excelDoc, "Sheet1", 3)      //sheet1是下方的sheet名，3是幾取3欄
            //              where (string)row[1] == "2005-02858"    //row[1]是第2欄喔（從0開始）
            //              select new {id = row[0], no = row[1], name = row[2]};


            //讀EventLog
            //EventLog tlog = new EventLog();
            //tlog.log = "Application";
            //var result = from System.Diagnostics.EventLogEntry e2 in tlog.Entries
            //              where(e2.EntryType == System.Diagnostics.EventLogEntryType.Warining ||
            //              e2.EntryType == System.Diagnostics.EventLogEntryType.Error) 
            //              && e2.Message.Contains(".NET")
            //              select new {source = e2.Source, message = e2.Message, username = e2.Username};


            //讀系統（例如C槽）
            //var result = from dir in Directory.EnumerateFilesSystemEntries(@"C:\") join file in Directory.GetFiles(@"C:\") on dir equals file into grp1
            //              from file in grp1.DefaultIfEmpty() where file.Contains(".doc")
            //              select new {name = new FileInfo(dir).Name, create_date = new FileInfo(dir).CreationTime, last_access_time = new FileInfo(dir).LastAccessTime};


            //讀log檔
            //string[] lines = System.IO.File.ReadAllLines(路徑)
            //var result = from q in GetListForIISLog(lines)
            //              where q.time == "03:27:43" && q.cs_uri_stem.Contains("repository.asmx")
            //              select q;


            //__________________________________________________________________________________________________________________




            


            Console.Read();
        }
    }
}
