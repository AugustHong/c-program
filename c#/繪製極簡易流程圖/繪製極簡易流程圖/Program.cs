using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace 繪製極簡易流程圖
{
    /// <summary>
    /// 輸入資料結構
    /// </summary>
    public class FlowInputData
    {
        // 來源
        public string Source { get; set; }

        // 箭頭顯示文字
        public string Text { get; set; }

        // 目標
        public string Target { get; set; }
    }

    /// <summary>
    /// 要畫圖的物件
    /// </summary>
    public class FlowGraphicsObj
    {
        // 角色名稱
        public string role { get; set; }

        public Point point { get; set; }

        public int width { get; set; }

        public int height { get; set; }

        // 中心點位置
        public Point centerPoint {  get; set; }
    }

    internal class Program
    {
        static void Main(string[] args)
        {
            // 產出的圖檔
            string outputFilePath = "output.jpg";

            // 輸入資料 (之後可以改成讀檔)
            /*
                格式：來源,文字,目標
                例如：
                A排程,呼叫API,外部API
                外部API,回傳結果,A排程
                A排程,產生檔案,NAS
                A排程,FTP傳輸,外部FTP
             */
            List<FlowInputData> input = new List<FlowInputData>
            {
                new FlowInputData {Source = "A排程", Text = "呼叫API", Target = "外部API" },
                new FlowInputData {Source = "外部API", Text = "回傳結果", Target = "A排程" },
                new FlowInputData {Source = "A排程", Text = "產生檔案", Target = "NAS" },
                new FlowInputData {Source = "A排程", Text = "FTP傳輸", Target = "外部FTP" }
            };

            // 先抓出所有 角色 (source + target)
            List<string> allRoles = new List<string>();
            foreach (var d in input)
            {
                allRoles.Add(d.Source);
                allRoles.Add(d.Target);
            }

            allRoles = allRoles.Distinct().ToList();  // 去掉重複

            // 設定畫圖參數
            int perTextWSize = 15;  // 標題每1個文字佔多大(寬)
            int perTextHSize = 20;  // 標題每1個文字佔多大(高)
            int initY = 50;  // 初始高度從多少開始
            int initX = 50;  // 初始寬度從多少開始
            int margin = 20;  // 邊
            int roleMargin = 40; // 各角色 的 邊
            int perFlowLineSize = 60;  // 每個 指向線條間隔

            Pen pen = new Pen(Color.Black);
            Font roleFont = new Font("Arial", 20);
            Font font = new Font("Arial", 12);
            SolidBrush brush = new SolidBrush(Color.Black);

            // 設定箭頭
            System.Drawing.Drawing2D.AdjustableArrowCap lineCap = new System.Drawing.Drawing2D.AdjustableArrowCap(3, 3, true);
            lineCap.Filled = true;
            lineCap.MiddleInset = 3.1f;
            Pen arrowPen = new Pen(Brushes.Black, 2);
            arrowPen.CustomStartCap = lineCap;
            arrowPen.CustomEndCap = new System.Drawing.Drawing2D.AdjustableArrowCap(5, 5, true);

            int totalWidth = 1600; 
            int totalHeight = 1000;

            // 要畫面的物件
            List<FlowGraphicsObj> fList = new List<FlowGraphicsObj>();

            // 將各角色 放置於 最上方 (文字都假設一行)
            foreach (var r in allRoles)
            {
                int len = r.Length;
                int textWidth = len * perTextWSize;
                int textHeight = perTextHSize;  // 皆假設是一行

                // 再來要算出矩型的 (文字 的 上下左右 + margin)
                int rWidth = textWidth + (margin * 2);
                int rHeight = textHeight + (margin * 2);

                // 開始組合資料
                fList.Add(new FlowGraphicsObj
                {
                    role = r,
                    height = rHeight,
                    width = rWidth,
                    point = new Point(initX, initY),
                    centerPoint = new Point((initX + (rWidth / 2)), initY)  // Y 不變， 用 X 算中心點
                });

                // 計算下一筆的資料
                initX = initX + rWidth + roleMargin;
                initY = initY;  // 都假設一致，所以不變(更進階就是會有斷行……等複雜操作)
            }

            using (Bitmap bitmap = new Bitmap(totalWidth, totalHeight))
            {
                using (Graphics graphics = Graphics.FromImage(bitmap))
                {
                    // 設定
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;

                    // 第一步：清空
                    graphics.Clear(Color.White);

                    // 第二步：先畫出 第一列的 所有角色
                    foreach (var f in fList)
                    {
                        // 畫出矩型
                        graphics.DrawRectangle(pen, f.point.X, f.point.Y, f.width, f.height);

                        // 畫出文字
                        graphics.DrawString(f.role, font, brush, new Point((f.point.X + margin), (initY + margin)));
                    }

                    // 第三步： 畫出各個指向的線條
                    int Y = initY + (margin * 2) + perTextHSize;   // 因為 第一列 角色的 Y 都沒有動 ( initY + (margin * 2) + perTextHSize = 矩型的結束)
                    foreach (var i in input)
                    {
                        // 取出來源點 + 目標點
                        int startX = fList.FirstOrDefault(f => f.role == i.Source).centerPoint.X;
                        int endX = fList.FirstOrDefault(f => f.role == i.Target).centerPoint.X;
                        Y += perFlowLineSize;

                        // 先畫線條
                        graphics.DrawLine(arrowPen, startX, Y, endX, Y);

                        // 再寫上文字
                        int textStartX = (startX > endX) ? endX : startX;
                        textStartX += roleMargin;  // 讓其假裝置中的感覺(先不用這麼精準)
                        graphics.DrawString(i.Text, font, brush, new Point(textStartX, (Y - perTextHSize)));
                    }

                    // 第四步： 畫出各中心節點的垂直線條
                    Y += 50;  // 結束線
                    foreach (var f in fList)
                    {
                        int x = f.centerPoint.X;
                        graphics.DrawLine(pen, x, (initY + (margin * 2) + perTextHSize), x, Y);
                    }

                    bitmap.Save(outputFilePath, ImageFormat.Jpeg);
                }
            }

            // 可優化部份：
            // (1) 總大小能不寫死，用算完的
            // (2) 文字換行
            // (3) 箭頭上的文字 盡量置中

            Console.WriteLine("產檔完成");
            Console.Read();
        }
    }
}
