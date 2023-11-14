using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

/*
    參考網址： https://topic.alibabacloud.com/tc/a/a-new-method-for-quickly-comparing-images-in-c-_1_31_31858004.html
 */

namespace 圖像比對差異
{
    internal class Program
    {
        static void Main(string[] args)
        {
            MatchDiff("t1.png", "t2.png");
            Console.WriteLine("比較完成");
            Console.ReadLine();
        }

        /// <summary>
        /// 比較2張圖片
        /// </summary>
        /// <param name="sourceFIlePath1"></param>
        /// <param name="sourceFIlePath2"></param>
        public static bool MatchDiff(string sourceFIlePath1, string sourceFIlePath2)
        {
            // 結果資料
            List<Point> diffPointList = new List<Point>();
            Bitmap b1, b2;
            string e1, e2;
            string n1, n2;

            try
            {
                e1 = Path.GetExtension(sourceFIlePath1);
                e2 = Path.GetExtension(sourceFIlePath2);

                n1 = sourceFIlePath1.Replace(e1, "") + "_output" + e1;
                n2 = sourceFIlePath2.Replace(e2, "") + "_output" + e2;

                // 先轉為  Bitmap
                b1 = new Bitmap(sourceFIlePath1);
                b2 = new Bitmap(sourceFIlePath2);
            }
            catch
            {
                return false;
            }

            // 比對差異並紀錄
            if (b1.Width == b2.Width && b1.Height == b2.Height)
            {
                // 差異的所有點位
                for(int i = 0; i < b1.Height; i++)
                {
                    for (int j = 0; j < b1.Width; j++)
                    {
                        string fPixel = b1.GetPixel(j, i).ToString();
                        string sPixel = b2.GetPixel(j, i).ToString();

                        if (fPixel != sPixel)
                        {
                            diffPointList.Add(new Point(j, i));
                        }
                    }
                }

                // 將點位分群 (將是同1個框的放在一起，判斷方式就是 y 沒有連續的)
                int range = 3;  //代表 只要不超過 3都算是同一群的資料
                List<int> groupList = new List<int>();
                groupList.Add(0);  //第1筆要列入，後面就依照切在哪邊斷開就加誰

                int currentY = 0;
                for (int i = 0; i < diffPointList.Count; i++)
                {
                    Point p = diffPointList[i];
                    if (i == 0)
                    {
                        currentY = p.Y;
                    }
                    else
                    {
                        if (p.Y - currentY > range)
                        {
                            // 分割群組
                            groupList.Add(i);
                        }

                        currentY = p.Y;
                    }

                    if (i == diffPointList.Count - 1)
                    {
                        groupList.Add(i + 1);
                    }
                }

                // 依據群組進行畫框 (要多減1是因為最後1筆不用算)
                Graphics g1 = Graphics.FromImage(b1);
                Graphics g2 = Graphics.FromImage(b2);
                Brush brush = new SolidBrush(Color.FromKnownColor(KnownColor.White));
                Pen pen = new Pen(brush);

                for (int i = 0; i < groupList.Count - 1; i++)
                {
                    int startGroupNum = groupList[i];
                    int endGroupNum = groupList[i + 1] - 1;

                    int minX = 0, minY = 0, maxX = 0, maxY = 0;

                    // 算出最大區域
                    for (int j = startGroupNum; j <= endGroupNum; j++)
                    {
                        Point p = diffPointList[j];
                        if (j == startGroupNum)
                        {
                            // 給初始值
                            minX = maxX = p.X;
                            minY = maxY = p.Y;
                        }
                        else
                        {
                            if (p.X < minX) { minX = p.X; }
                            if (p.Y < minY) { minY = p.Y; }
                            if (p.X > maxX) { maxX = p.X; }
                            if (p.Y > maxY) { maxY = p.Y; }
                        }
                    }

                    // 畫框
                    g1.DrawRectangle(pen, minX, minY, (maxX - minX), (maxY - minY));
                    g2.DrawRectangle(pen, minX, minY, (maxX - minX), (maxY - minY));
                }

                // 儲存
                b1.Save(n1);
                b2.Save(n2);

                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
