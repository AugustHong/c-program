using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
/// <summary>
/// 責任鍊模式
/// 情境：巢狀if
/// 解法：寫好幾個class串在一起（一個接一個class，如條件不符，就給下一個處理）
/// </summary>
namespace Chain_of_Responsibility_Design_Pattern
{
    class Program
    {
        static void Main(string[] args)
        {
            Customer customer = new Customer()
            {
                sex = 0, //0=女，1=男
                age = 60
            };

            FeeHandlerBase handler = FeeHandlerBase.getHandlers();
            Console.WriteLine(handler.HandleRequest(customer).ToString());

            Console.Read();
        }
    }

    public class Customer
    {
        public int sex, age;
    }

    public abstract class FeeHandlerBase
    {
        public static FeeHandlerBase getHandlers()
        {
            //先把全部都new完，且把下一位都先拉好
            FeeHandler01 h01 = new FeeHandler01();
            FeeHandler02 h02 = new FeeHandler02();
            FeeHandler03 h03 = new FeeHandler03();

            h01.successor = h02;
            h02.successor = h03;
            return h01;

        }

        public FeeHandlerBase successor = null;
        protected abstract bool canHandle();
        protected abstract float parse();

        protected Customer _customer;
        public float HandleRequest(Customer customer)
        {
            _customer = customer;
            if (canHandle())
                return parse();
            else
            {
                if (successor != null)
                    return successor.HandleRequest(customer);
                else
                    return 1f;
            }

        }
    }

    /// <summary>
    /// >=90以上免費
    /// </summary>
    public class FeeHandler01 : FeeHandlerBase
    {
        protected override bool canHandle()
        {
            return _customer.age >= 90;
        }

        protected override float parse()
        {
            return 0f;
        }
    }

    /// <summary>
    /// 男性65以上打8折
    /// </summary>
    public class FeeHandler02 : FeeHandlerBase
    {
        protected override bool canHandle()
        {
            return (_customer.sex == 1 && _customer.age >= 65);
        }

        protected override float parse()
        {
            return 0.8f;
        }
    }
    /// <summary>
    /// 女性60以上打7折
    /// </summary>
    public class FeeHandler03 : FeeHandlerBase
    {
        protected override bool canHandle()
        {
            return (_customer.sex == 0 && _customer.age >= 60);
        }

        protected override float parse()
        {
            return 0.7f;
        }
    }
}
