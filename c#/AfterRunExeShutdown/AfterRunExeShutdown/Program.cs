using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AfterRunExeShutdown
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if (args.Length > 0)
            {
                Console.WriteLine(args[0]);
            }

            System.Threading.Thread.Sleep(3000);
        }
    }
}
