using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Console程式的Read不要顯示文字
{
	class Program
	{
		static void Main(string[] args)
		{
                  string input1 = string.Empty;
                  while (true)
                  {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Enter)
                        {
                              Console.WriteLine();
                              break;
                        }

                        input1 += key.KeyChar;
                  }

                  string input2 = string.Empty;
                  while (true)
                  {
                        ConsoleKeyInfo key = Console.ReadKey(true);
                        if (key.Key == ConsoleKey.Enter)
                        {
                              Console.WriteLine();
                              break;
                        }

                        input2 += key.KeyChar;
                  }

                  Console.WriteLine($"你輸入的文字1 = {input1}");
                  Console.WriteLine($"你輸入的文字2 = {input2}");
                  Console.ReadLine();
            }
	}
}
