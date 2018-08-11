using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL_Example
{
    //要public別人才能用
    public class Order{

        public int GetTax(int price){
            //營業稅是直接進位的
            int tax = 0;
            int t = Convert.ToInt32(price * 0.05); //double轉int會是找比原先小的最大整數 10.5=>10  11.5=>11

            //偶數的直接把剛才的加1就可，奇數的則用4捨6入5成雙
            //10.5照理來說要變成11，所以用t+1   11.5照理來說要變成12，所以用round即可
            if(t % 2 == 0) { tax = t + 1; } else { tax = (int)Math.Round(price*0.05); }
            return tax;
        }
    }
}
