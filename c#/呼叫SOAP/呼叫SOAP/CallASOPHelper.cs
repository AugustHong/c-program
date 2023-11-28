using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

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

    public class CallASOPHelper
    {
        // header
        public CallHeader header { get; set; }

        // 功能代號
        public CallBody body { get; set; }

        // XML開頭結尾 (固定的)
        public string sStr = @"<?xml version=""1.0"" encoding=""utf-8""?>
	<soap:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" xmlns:soap=""http://schemas.xmlsoap.org/soap/envelope/"">
	 ";
        public string eStr = "</soap:Envelope>";

        // 呼叫的網址
        public string url = "";

        public CallASOPHelper(string url, string funcCode) 
        {
            this.url = url;
            this.header = new CallHeader();
            this.body = new CallBody(funcCode);
        }

        // 預設 timeout 2分鐘
        public string CallASOP(int timeout = 120000)
        {
            string result = string.Empty;

            // 組出資料
            string sendData = sStr + header.toHeaderString() + body.toBodyString() + eStr;

            // 固定加入的標題
            string urlFuncStr = "\"http://tempuri.org/" + body.funcCode + "\"";

            Console.WriteLine("==================開始呼叫===============================");
            Console.WriteLine("URL = " + url);
            Console.WriteLine("URL FUNC = " + urlFuncStr);
            Console.WriteLine("傳入資料：");
            Console.WriteLine(sendData);

            try
            {

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
                req.Headers.Add("SOAPAction", urlFuncStr);
                req.ContentType = "text/xml;charset=\"utf-8\"";
                req.Accept = "text/xml";
                req.Method = "POST";
                req.Timeout = timeout;

                using (Stream stm = req.GetRequestStream())
                {
                    using (StreamWriter stmw = new StreamWriter(stm))
                    {
                        stmw.Write(sendData);
                    }
                }

                Console.WriteLine("==================取得回應===============================");
                WebResponse response = req.GetResponse();

                string responseBody = string.Empty;
                using (StreamReader sr = new StreamReader(response.GetResponseStream()))
                {
                    responseBody = sr.ReadToEnd();
                }

                Console.WriteLine("回傳資料：");
                Console.WriteLine(responseBody);
            }
            catch (Exception ex)
            {
                Console.WriteLine("發生錯誤： " + ex.Message);
                throw new Exception(ex.Message);
            }

            return result;
        }
    }

    // header
    public class CallHeader
    {
        //依自己需要增加欄位

        public CallHeader()
        {
            //依自己需求增加欄位
        }

        public string toHeaderString()
        {
            string result = string.Empty;

            // 中間請自行放資料 (今天這個的沒有，所以就註解掉了)
//            result += @"<soap:Header>
//</soap:Header>";

            return result;
        }
    }

    public class CallBody
    {
        // 功能清單
        public string funcCode { get; set; }

        public List<CallParam> paramList { get; set; }

        public CallBody(string funcCode)
        {
            this.funcCode = funcCode;
            this.paramList = new List<CallParam>();
        }

        public string toBodyString()
        {
            string result = string.Empty;

            result += @"<soap:Body><" + funcCode + @" xmlns=""http://tempuri.org/"">";

            // 組 參數
            foreach (CallParam param in paramList)
            {
                result += param.toParamString();
            }

            result += "</" + funcCode + "></soap:Body>";

            return result;
        }
    }

    public class CallParam
    {
        public string paramName { get; set; }

        public string paramValue { get; set; }

        public CallParam()
        {

        }

        public CallParam(string paramName, string paramValue)
        {
            this.paramName = paramName;
            this.paramValue = paramValue;
        }

        public string toParamString()
        {
            return @"<" + paramName + ">" + paramValue + "</" + paramName + ">";
        }
    }
}
