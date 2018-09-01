using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MVC_test.Models
{
    public class Order
    {
        public static int GetTax(int? price, out int total)
        {
            if(price.HasValue == false || price <= 0) { throw new Exception("未稅金必需是正整數"); }

            decimal taxM = (decimal)price / 20M;

            int tax = (int)Math.Round(taxM, MidpointRounding.AwayFromZero);

            total = price.Value + tax;

            return tax;
        }
    }
}