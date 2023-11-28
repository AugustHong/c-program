using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址： https://dotblogs.com.tw/rainmaker/2015/05/13/151278
 */

namespace 呼叫SOAP
{
    /*
        目標傳出去的資料： (soap:Body 之前都是固定的)，而 HelloWorld 就是 要呼叫的 功能名稱代號

        <?xml version=""1.0"" encoding=""utf-8""?>
	<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
	  <soap:Body>
		<HelloWorld xmlns:rm=""http://tempuri.org/"">
		  <name>Rainmaker</name>
		</HelloWorld>
	  </soap:Body>
	</soap:Envelope>
     */

    internal class Program
    {
        static void Main(string[] args)
        {
            string url = "http://localhost:60591/WebService1.asmx";
            string funcCode = "HelloWorld";
            CallASOPHelper cah = new CallASOPHelper(url, funcCode);
            cah.body.paramList.Add(new CallParam { paramName = "name", paramValue = "Rainmaker" });
            cah.CallASOP();

            Console.Read();
        }
    }
}
