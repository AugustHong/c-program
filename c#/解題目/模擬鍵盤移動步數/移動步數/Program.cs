using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 移動步數
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*
             寫一個方法(string pad, string input) return int

第一個參數pad固定長度9，是1到9不重複但順序隨機的數字，想像成一個3x3的鍵盤
    例如傳入 357916284，此鍵盤呈現
    357
    916
    284
第二個參數input傳入長度至少為2的字串，一樣是1到9，但，可重複。
    意義上如同按下鍵盤按鍵的順序
此方法要回傳的，是對特定鍵盤，按下指定按鍵序列，總共需要花多少秒

規則
    1.手指移動到第一個指定位置的時間是0秒
    2.移動到另一個位置時候需要的秒數，取決於相鄰移動的次數，相鄰的定義包含斜角，也就是說正中央的數字朝任何其他數字移動，都算1秒
             */


            // 這邊我想把題目變體，改成無上限的方式 (就不只 3X3 ，可以 5X4、 4X4……等，這邊使用 4X3)
            string pad = "ABCDEFGHIJKL";
            int lineLength = 4;  // 一行的長度
            string input = "ALIGGHB";

            // 先印出題目
            List<string> padChars = pad.ToCharArray().Select(x => x.ToString()).ToList();

            Console.WriteLine("===============參數設定===================");
            Console.WriteLine($"pad = {pad}");
            Console.WriteLine($"每列長度 = {lineLength}");
            Console.WriteLine($"input = {input}");
            Console.WriteLine("===============參數設定===================");
            Console.WriteLine("");

            for (int i = 0; i < padChars.Count; i++) {
                Console.Write($"{padChars[i]} ");

                if (i % lineLength == (lineLength - 1))
                {
                    Console.WriteLine("");
                }
            }

            Console.WriteLine("");

            // 開始走步數
            List<string> inputChars = input.ToCharArray().Select(x => x.ToString()).ToList();

            int totalStep = 0;
            for (int i = 0; i < inputChars.Count() - 1; i++)
            {
                string start = inputChars[i];
                string end = inputChars[i + 1];
                int step = MoveStep(padChars, start, end, lineLength);
                totalStep += step;
                Console.WriteLine($"從 {start} 走到 {end} 要花 {step} 步");
            }
            Console.WriteLine("-----------------------------------");
            Console.WriteLine($"總共要花 {totalStep} 步");

            Console.ReadLine();

        }
    
        /// <summary>
        /// 算出步數
        /// 方法： 先算出 X 軸的差異數 (用 mod 完後相減)
        ///        再算出 Y 轉的差異數 (用 / 無條件捨去)
        ///        2者取大的回傳即可
        /// </summary>
        /// <param name="source"></param>
        /// <param name="start"></param>
        /// <param name="end"></param>
        /// <returns></returns>
        static int MoveStep(List<string> source, string start, string end, int lineLength)
        {
            int sIndex = source.IndexOf(start);
            int eIndex = source.IndexOf(end);

            // 算出 X 軸差異 (用 mod)
            int sX = sIndex % lineLength;
            int eX = eIndex % lineLength;
            int dX = Math.Abs(eX - sX);

            // 算出 Y 軸差異 (用 / 無條件捨去)
            int sY = (int)Math.Floor((double)(sIndex / lineLength));
            int eY = (int)Math.Floor((double)(eIndex / lineLength));
            int dY = Math.Abs(eY - sY);

            // 2者取大的回傳
            return dX > dY ? dX : dY;
        }
    }
}
