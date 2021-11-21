using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 進制轉換
{
	class Program
	{
		static void Main(string[] args)
		{
			Console.WriteLine("-----------------測試 2 進制----------------------");
			BaseDecimalHelper bin = new BaseDecimalHelper(2);
			Console.WriteLine("2進制的 101 轉 10 進制 = " + bin.To10("101"));
			Console.WriteLine("10進制的 5 轉 2 進制 = " + bin.ToBase(5));
			Console.WriteLine("-----------------測試 2 進制----------------------");

			Console.WriteLine("-----------------測試 8 進制----------------------");
			BaseDecimalHelper oct = new BaseDecimalHelper(8);
			Console.WriteLine("8進制的 710 轉 10 進制 = " + oct.To10("710"));
			Console.WriteLine("10進制的 456 轉 8 進制 = " + oct.ToBase(456));
			Console.WriteLine("-----------------測試 8 進制----------------------");

			Console.WriteLine("-----------------測試 16 進制----------------------");
			BaseDecimalHelper hex = new BaseDecimalHelper(16);
			Console.WriteLine("16進制的 6FA 轉 10 進制 = " + hex.To10("6FA"));
			Console.WriteLine("10進制的 1786 轉 16 進制 = " + hex.ToBase(1786));
			Console.WriteLine("-----------------測試 16 進制----------------------");

			Console.WriteLine("-----------------測試 36 進制----------------------");
			BaseDecimalHelper base36 = new BaseDecimalHelper(36);
			Console.WriteLine("36進制的 AGZ 轉 10 進制 = " + base36.To10("AGz"));
			Console.WriteLine("10進制的 13571 轉 36 進制 = " + base36.ToBase(13571));
			Console.WriteLine("-----------------測試 36 進制----------------------");

			Console.WriteLine("-----------------測試 62 進制----------------------");
			BaseDecimalHelper base62 = new BaseDecimalHelper(62);
			Console.WriteLine("62進制的 zzz 轉 10 進制 = " + base62.To10("zzz"));
			Console.WriteLine("10進制的 238327 轉 62 進制 = " + base62.ToBase(238327));
			Console.WriteLine("-----------------測試 62 進制----------------------");


			Console.ReadLine();
		}
	}

	// 進制轉換處理
	public class BaseDecimalHelper
	{
		/*
		 1、BIN：binary，二進制的;
             2、OCT：octal，八進制的;
             3、HEX：hexadecimal，十六進制的;
             4、DEC：decimal，十進制的。

		名字較難取，所以就統一叫 decimal
		 */

		// 不用放 0 是因為，不在列表的都歸為0
		public string words = "123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

		// 進制位數
		public int baseNum = 2;

		// 要處理的制串
		public string word = string.Empty;

		public BaseDecimalHelper(int baseNum)
		{
			this.baseNum = baseNum;

			// 最少 2 進制；最大 62進制
			if(this.baseNum < 2) { this.baseNum = 2; }
			if(this.baseNum > 62) { this.baseNum = 62; }

			// 切出字串 (要 - 1 是因為從0開始)
			word = words.Substring(0, (this.baseNum - 1));
		}

		// 轉成10進制
		public double To10(string source)
		{
			double result = 0;

			// 如果是 36進制內，要轉大寫
			string s = this.baseNum > 36 ? source.Trim() : source.Trim().ToUpper();

			int len = s.Length;
			int index = 0;

			while (index < len)
			{
				string tmp = s.Substring(index, 1);

				int i = word.IndexOf(tmp);
				if (i < 0) { i = 0; }
				else { i++; }  // 因為從0開始，所以 1的index = 0 ，但是要算1

				result += i * Math.Pow(this.baseNum, (len - index - 1));
				index++;
			}

			return result;
		}
	
	      // 10進制轉 指定進制
		public string ToBase(int source)
		{
			string result = "";

			bool run = true;
			int newData_M = 0;
			int newData_D = 0;

			while (run)
			{
				newData_M = (source / this.baseNum);  //因為是 int型別，所以自動只取整數
				newData_D = source % this.baseNum;

				if(newData_D <= 9)
				{
					result = newData_D.ToString() + result;
				}
				else
				{
					// A-Z 和 a-z (數字轉英文)
					byte[] array = new byte[1];
					// A 為 10 => 55 + 10 = 65
					// a 為 36 => 61 + 36 = 97
					int tmp = newData_D <= 35 ? 55 : 61;
					array[0] = (byte)(Convert.ToInt32(newData_D + tmp)); //ASCII碼強制轉換二進位
					result = Convert.ToString(System.Text.Encoding.ASCII.GetString(array)) + result;
				}

				if (newData_M == 0)
				{
					run = false;
				}
				else
				{
					source = newData_M;
				}
			}

			return result;
		}
	}
}
