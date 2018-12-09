using System;
using System.Web;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Hong.IPHelper
{
    /// <summary>
    /// 取得本機IP和實體IP的位址
    /// </summary>
    public static class IPHelper
    {
        /// <summary>
        /// 得到真正的ip
        /// </summary>
        /// <param name="Request">傳入System.Web.HttpContext.Current.Request</param>
        /// <returns></returns>
        public static string GetIP(this HttpRequest Request)
        {
            if (Request.Headers["CF-CONNECTING-IP"] != null) return Request.Headers["CF-CONNECTING-IP"].ToString();

            if (Request.ServerVariables["HTTP_X_FORWARDED_FOR"] != null) return Request.ServerVariables["HTTP_X_FORWARDED_FOR"].ToString();

            return Request.UserHostAddress;
        }


        /// <summary>
        /// 取得實體IP位址（dns出來的）
        /// </summary>
        /// <param name="dns">DNS的服務商（例如Hinet就輸入 dns.hinet.net）</param>
        /// <returns></returns>
        public static List<string> GetHostIP(string dns = "dns.hinet.net")
        {
            List<string> result = new List<string>();

            //讀取實際的ip（一樣會很多組）
            //dns.hinet.net是DNS網路服務商
            IPAddress[] hostIPList = Dns.GetHostEntry(dns).AddressList;

            foreach(var ip in hostIPList)
            {
                result.Add(ip.ToString());
            }

            return result;
        }

        /// <summary>
        /// 回傳主機名稱
        /// </summary>
        /// <returns></returns>
        public static string GetHostName()
        {
            return Dns.GetHostName();
        }


        /// <summary>
        /// 取得LocalIP
        /// </summary>
        /// <param name="onlyIPV4">是否只取IPV4（預設為false）</param>
        /// <returns></returns>
        public static List<string> GetLocalIP(bool onlyIPV4 = false)
        {
            //先取得主機名稱
            string hostName = GetHostName();

            List<string> result = new List<string>();

            IPHostEntry iphostentry = Dns.GetHostByName(hostName);   //取得本機的 IpHostEntry 類別實體

            foreach(var ip in iphostentry.AddressList)
            {
                //是否只取得IPV4
                //第一段判別式是判別是否是IPV4，所以其他的都會是false（故用or來加入 => 當後面是true則是取全部資料）
                //當後面是false是只取IPV4的資料
                if (ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork  || !onlyIPV4)
                {
                    result.Add(ip.ToString());
                }
              
            }

            return result;
        }
    }
}
