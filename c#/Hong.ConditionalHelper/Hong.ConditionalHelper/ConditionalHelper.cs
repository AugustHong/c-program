using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hong.ConditionalHelper
{
    /// <summary>
    /// 幫助查詢寫成一行。例如：
    /// var result = from data in dbResult
    ///              where ConditionalHelper.Compare(data.id , queryID, ">") && ConditionalHelper.CompareStr(data.name , queryName, "=")
    ///              select data;
    /// </summary>
    public class ConditionalHelper{

        #region 比大小
        /// <summary>
        /// 判斷日期、數值的大小（string要用下一個mathod）
        /// </summary>
        /// <typeparam name="T">型別</typeparam>
        /// <param name="x">比較值1</param>
        /// <param name="y">比較值2</param>
        /// <param name="Operator">運算子</param>
        /// <returns>回傳true或者false</returns>
        public static bool Compare<T>(T x, T y, string Operator)
        {
            //如果是字串要多判斷是否是""
            if (y == null || (y is string && (y as string == string.Empty))) { return true; }

            //看是否可以比較（所以要有實作IComparable才行）
            //字串的話會是比ascii一個一個比（如果相同繼續比下去）
            if (x is IComparable && y is IComparable)
            {
                switch (Operator)
                {
                    case "=":
                        return (x as IComparable).CompareTo(y) == 0 ? true : false;

                    case ">":
                        return (x as IComparable).CompareTo(y) > 0 ? true : false;

                    case "<":
                        return (x as IComparable).CompareTo(y) < 0 ? true : false;

                    case ">=":
                        return (x as IComparable).CompareTo(y) > 0 || (x as IComparable).CompareTo(y) == 0 ? true : false;

                    case "<=":
                        return (x as IComparable).CompareTo(y) < 0 || (x as IComparable).CompareTo(y) == 0 ? true : false;

                    default:
                        return true;
                }
            }
            else { return true; }
        }

        #endregion

        #region 字串判斷（包含於）
        /// <summary>
        /// 判斷字串（>代表x包含y；反之小於代表y包含x）
        /// </summary>
        /// <param name="x">比較值1</param>
        /// <param name="y">比較值2</param>
        /// <param name="Operator">運算子</param>
        /// <returns>回傳true或者false</returns>
        public static bool CompareStr(string x, string y, string Operator)
        {
            if (y == null || y == string.Empty) { return true; }

            switch (Operator)
            {
                case "=":
                    return x == y;

                case ">":
                    return x.Contains(y);

                case "<":
                    return y.Contains(x);

                default:
                    return true;
            }
        }
        #endregion

    }
}
