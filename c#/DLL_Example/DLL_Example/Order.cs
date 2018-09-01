using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DLL_Example
{
    //要public別人才能用
    /// <summary>
    /// Order
    /// </summary>
    public class Order{

        /// <summary>
        /// 顯示稅金
        /// </summary>
        /// <param name="price">總价</param>
        /// <param name="total">要傳出去的總金額（含稅）</param>
        /// <returns></returns>
        public static int GetTax(int? price, out int total)
        {
            if (price.HasValue == false || price <= 0) { throw new Exception("未稅金必需是正整數"); }

            decimal taxM = (decimal)price / 20M;

            int tax = (int)Math.Round(taxM, MidpointRounding.AwayFromZero); //一律進位（不會受4捨6入5成雙影響）

            total = price.Value + tax;

            return tax;
        }
    }
}
