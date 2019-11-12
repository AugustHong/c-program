using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

/*
    參考網圵： https://dotblogs.com.tw/hatelove/archive/2012/06/05/parse-taiwan-address-with-regex.aspx
 */

namespace TaiwanAddressSplit
{

    public class Address
    {
        /// <summary>
        /// 地址組成：
        /// 1.郵遞區號: 3~5碼數字
        /// 2.縣市： xx 縣/市
        /// 3.鄉鎮市區：xx 鄉/鎮/市/區
        /// 4.其他：鄉鎮市區以後的部分
        /// 規則：開頭一定要是3或5個數字的郵遞區號，如果不是，解析不會出錯，但ZipCode為空
        /// 地址一定要有XX縣/市 + XX鄉/鎮/市/區 + 其他
        /// </summary>
        /// <param name="address"></param>
        public Address(string address)
        {
            this.OrginalAddress = address;
            this.ParseByRegex(address);
        }

        /// <summary>
        /// 縣市
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 鄉鎮市區
        /// </summary>
        public string District { get; set; }

        /// <summary>
        ///  里
        /// </summary>
        public string Village { get; set; }

        /// <summary>
        /// 是否符合pattern規範
        /// </summary>
        public bool IsParseSuccessed { get; set; }

        /// <summary>
        /// 原始傳入的地址
        /// </summary>
        public string OrginalAddress { get; private set; }

        /// <summary>
        /// 鄉鎮市區之後的地址
        /// </summary>
        public string Others { get; set; }

        /// <summary>
        /// 郵遞區號
        /// </summary>
        public string ZipCode { get; set; }

        /// <summary>
        /// 組成完整的地址
        /// </summary>
        /// <returns>完整的地址</returns>
        public override string ToString()
        {
            var result = string.Format("{0}{1}{2}{3}{4}", this.ZipCode, this.City, this.District, this.Village, this.Others);
            return result;
        }

        private void ParseByRegex(string address)
        {
            // 有 縣市 + 鄉鎮市區 + 里
            var p = @"(?<zipcode>(^\d{5}|^\d{3})?)(?<city>\D+[縣市])(?<district>\D+(鄉鎮|市區|鎮區|鎮市|[鄉鎮市區]))(?<village>\D+?[里])(?<others>.+)";
            Match m = Regex.Match(address, p);

            if (m.Success)
            {
                this.IsParseSuccessed = true;

                this.ZipCode = m.Groups["zipcode"].ToString();
                this.City = m.Groups["city"].ToString();
                this.District = m.Groups["district"].ToString();
                this.Village = m.Groups["village"].ToString();
                this.Others = m.Groups["others"].ToString();
            }
            else
            {
                // 有 縣市 + 鄉鎮市區
                var pattern = @"(?<zipcode>(^\d{5}|^\d{3})?)(?<city>\D+[縣市])(?<district>\D+?(鄉鎮|市區|鎮區|鎮市|[鄉鎮市區]))(?<others>.+)";
                Match match = Regex.Match(address, pattern);

                if (match.Success)
                {
                    this.IsParseSuccessed = true;

                    this.ZipCode = match.Groups["zipcode"].ToString();
                    this.City = match.Groups["city"].ToString();
                    this.District = match.Groups["district"].ToString();
                    this.Village = string.Empty;
                    this.Others = match.Groups["others"].ToString();
                }
                else
                {
                    // 只有 縣市
                    var pattern2 = @"(?<zipcode>(^\d{5}|^\d{3})?)(?<city>\D+([縣市]))(?<others>.+)";
                    Match match2 = Regex.Match(address, pattern2);

                    if (match2.Success)
                    {
                        this.IsParseSuccessed = true;

                        this.ZipCode = match2.Groups["zipcode"].ToString();
                        this.City = match2.Groups["city"].ToString();
                        this.District = string.Empty;
                        this.Village = string.Empty;
                        this.Others = match2.Groups["others"].ToString();
                    }
                }
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            List<Address> addresses = new List<Address>();

            addresses.Add(new Address("嘉義市西區世賢路一段687號"));
            addresses.Add(new Address("台北市重慶南路1段122號"));
            addresses.Add(new Address("111台北市重慶南路1段122號"));
            addresses.Add(new Address("金門郵政91022號信箱"));
            addresses.Add(new Address("市市市測試區xx路"));
            addresses.Add(new Address("嘉義縣大林鎮沙崙村2號"));
            addresses.Add(new Address("彰化縣田中鎮碧峰里中南路2段210號"));


            foreach (var address in addresses)
            {
                Console.WriteLine($"parse success = {address.IsParseSuccessed}, zipCode = {address.ZipCode}, city = {address.City}, district = {address.District}, village = {address.Village}, other = {address.Others}");
            }

            Console.ReadLine();

        }
    }

}
