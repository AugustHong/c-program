using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Timers;
using System.Windows.Forms; //Keys列舉在這裡面
using System.Drawing;
using System.Drawing.Imaging;

/*
	參考網址： https://www.796t.com/content/1550624613.html 、 https://www.796t.com/content/1547664663.html 、 https://iter01.com/140663.html
 */

namespace 模擬按鍵精靈
{
      //滑鼠事件常量
      public enum MouseEventFlag : uint
      {
            Move = 0x0001,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            XDown = 0x0080,
            XUp = 0x0100,
            Wheel = 0x0800,
            VirtualDesk = 0x4000,
            Absolute = 0x8000
      }

      //鍵盤事件常量
      public enum KeyEventFlag : int
      {
            Down = 0x0000,
            Up = 0x0002,
      }

      public struct NumBody
      {
            public int num;//數字
            public int matchNum;//匹配的個數
            public int matchSum;
            public double matchRate;//匹配度
            public Point point;
            public List<Point> bodyCollectionPoint;//該數字所有畫素在大圖中的座標
      }

      public class MouseKeyBoardHelper
      {
            /// <summary>
            ///  必要的參考
            /// </summary>
            /// <param name="dwFlags"></param>
            /// <param name="dx"></param>
            /// <param name="dy"></param>
            /// <param name="cButtons">/param>
            /// <param name="dwExtraInfo"></param>
            //滑鼠事件函式
            [DllImport("user32.dll", EntryPoint = "mouse_event")]
            public static extern void mouse_event(MouseEventFlag dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

            //滑鼠移動函式
            [DllImport("user32.dll", EntryPoint = "SetCursorPos")]
            private static extern int SetCursorPos(int x, int y);

            // 該函式根據類名和視窗名來得到視窗控制代碼,但是這個函式不能查詢子視窗,也不區分大小寫.如果要從一個視窗的子視窗查詢需要使用FIndWindowEX函式
            [DllImport("user32.dll")]
            static extern IntPtr FindWindow(string strClass, string strWindow);

            /*
             該函式獲取一個視窗的控制代碼,該視窗的類名和視窗名與給定的字串相匹配,該函式查詢子視窗時從排在給定的子視窗後面的下一個子視窗開始。
            其中引數hwnParent為要查詢子視窗的父視窗控制代碼,若該值為NULL則函式以桌面視窗為父視窗,查詢桌面視窗的所有子視窗。 
            hwndChildAfter子視窗控制代碼,查詢從在Z序中的下一個子視窗開始,子視窗必須為hwnParent直接子視窗而非後代視窗,
            若hwnChildAfter為NULL,查詢從父視窗的第一個子視窗開始。 strClass指向一個指定類名的空結束字串或一個標識類名字串的成員的指標。 
            strWindow指向一個指定視窗名(視窗標題)的空結束字串.若為NULL則所有窗體全匹配。返回值:如果函式成功,返回值為具有指定類名和視窗名的視窗控制代碼,
            如果函式失敗,返回值為NULL
             */
            [DllImport("user32.dll")]
            static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string strClass, string strWindow);

            //鍵盤事件函式
            /// <param name="bVk" >按鍵的虛擬鍵值</param>
            /// <param name= "bScan" >掃描碼，一般不用設定，用0代替就行</param>
            /// <param name= "dwFlags" >選項標誌：0：表示按下，2：表示鬆開</param>
            /// <param name= "dwExtraInfo">一般設定為0</param>
            [DllImport("user32.dll", EntryPoint = "keybd_event")]
            public static extern void keybd_event(Byte bVk, Byte bScan, KeyEventFlag dwFlags, Int32 dwExtraInfo);

            #region 滑鼠
            /// <summary>
            /// 滑鼠移動到 指定位置(x,y)
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public void mouse_move(int x, int y)
            {
                  SetCursorPos(x, y);
            }

            /// <summary>
            /// 滑鼠操作 x,y 是滑鼠距離當前位置的二維移動向量
            /// </summary>
            /// <param name="_dwFlags"></param>
            /// <param name="x"></param>
            /// <param name="y"></param>
            public void mouse_move_r(int x, int y)
            {
                  mouse_event(MouseEventFlag.Move, x, y, 0, 0);
            }

            /// <summary>
            /// 滑鼠操作
            /// </summary>
            /// <param name="button">按鍵</param>
            /// <param name="is_double">是否按2下</param>
            public void mouse_click(string button = "L", bool is_double = false)

            {
                  switch (button)
                  {
                        case "L":
                              mouse_event(MouseEventFlag.LeftDown, 0, 0, 0, 0);
                              mouse_event(MouseEventFlag.LeftUp, 0, 0, 0, 0);
                              if (is_double)
                              {
                                    mouse_event(MouseEventFlag.LeftDown, 0, 0, 0, 0);
                                    mouse_event(MouseEventFlag.LeftUp, 0, 0, 0, 0);
                              }
                              break;
                        case "R":
                              mouse_event(MouseEventFlag.RightDown, 0, 0, 0, 0);
                              mouse_event(MouseEventFlag.RightUp, 0, 0, 0, 0);
                              if (is_double)
                              {
                                    mouse_event(MouseEventFlag.RightDown, 0, 0, 0, 0);
                                    mouse_event(MouseEventFlag.RightUp, 0, 0, 0, 0);
                              }
                              break;
                        case "M":
                              mouse_event(MouseEventFlag.MiddleDown, 0, 0, 0, 0);
                              mouse_event(MouseEventFlag.MiddleUp, 0, 0, 0, 0);
                              if (is_double)
                              {
                                    mouse_event(MouseEventFlag.MiddleDown, 0, 0, 0, 0);
                                    mouse_event(MouseEventFlag.MiddleUp, 0, 0, 0, 0);
                              }
                              break;
                  }
            }

            /// <summary>
            /// 按滑鼠左鍵
            /// </summary>
            public void MouseLeftClick()
            {
                  mouse_click("L", false);
            }

            /// <summary>
            /// 按滑鼠右鍵鍵
            /// </summary>
            public void MouseRightClick()
            {
                  mouse_click("R", false);
            }

            /// <summary>
            /// 按滑鼠中鍵
            /// </summary>
            public void MouseMidClick()
            {
                  mouse_click("R", false);
            }

            /// <summary>
            /// 按滑鼠左鍵2下
            /// </summary>
            public void MouseDoubleLeftClick()
            {
                  mouse_click("L", true);
            }

            /// <summary>
            /// 按滑鼠右鍵鍵2下
            /// </summary>
            public void MouseDoubleRightClick()
            {
                  mouse_click("R", true);
            }

            /// <summary>
            /// 按滑鼠中鍵2下
            /// </summary>
            public void MouseDoubleMidClick()
            {
                  mouse_click("R", true);
            }

            /// <summary>
            /// 拖拉 (左鍵)
            /// </summary>
            /// <param name="x"></param>
            /// <param name="y"></param>
            /// <param name="isAbs">是否是絕對位置</param>
            public void MouseDrop(int x, int y, bool isAbs = true)
		{
                  mouse_event(MouseEventFlag.LeftDown, 0, 0, 0, 0);
                  if (isAbs)
                  {
                        mouse_move(x, y);
                  }
                  else
                  {
                        mouse_move_r(x, y);
                  }

                  mouse_event(MouseEventFlag.LeftUp, 0, 0, 0, 0);
            }

            #endregion

            #region 鍵盤
            /// <summary>
            /// 鍵盤操作
            /// </summary>
            /// <param name="_bVk">按鍵</param>
            /// <param name="_dwFlags">按的方式(按下/起來)</param>
            public void keybd(Byte _bVk, KeyEventFlag _dwFlags = KeyEventFlag.Down)
            {
                  keybd_event(_bVk, 0, _dwFlags, 0);
            }

            //鍵盤操作 帶自動釋放 dwFlags_time 單位:毫秒
            public void keybd(Byte __bVk, int dwFlags_time = 100)
            {
                  keybd(__bVk, KeyEventFlag.Down);
                  System.Threading.Thread.Sleep(dwFlags_time); // 間隔
                  keybd(__bVk, KeyEventFlag.Up);
            }

            // 鍵盤操作 帶自動釋放 dwFlags_time 單位:毫秒
            public void keybd(string key, int dwFlags_time = 100)
            {
                  keybd(getKeys(key), dwFlags_time);
            }

            //鍵盤操作 組合鍵 帶釋放
            public void keybd(Byte[] _bVk, int dwFlags_time = 100)
            {
                  if (_bVk.Length >= 2)
                  {
                        //按下所有鍵
                        foreach (Byte __bVk in _bVk)
                        {
                              keybd(__bVk, KeyEventFlag.Down);
                        }

                        //反轉按鍵排序
                        _bVk = (Byte[])_bVk.Reverse().ToArray();
                        System.Threading.Thread.Sleep(dwFlags_time); // 間隔

                        //鬆開所有鍵
                        foreach (Byte __bVk in _bVk)
                        {
                              keybd(__bVk, KeyEventFlag.Up);
                        }
                  }
            }

            //鍵盤操作 組合鍵 帶釋放
            public void keybd(List<string> keyList, int dwFlags_time = 100)
            {
                  List<Byte> s = new List<byte>();

                  foreach (var key in keyList)
                  {
                        s.Add(getKeys(key));
                  }

                  if (s.Count() > 0)
                  {
                        keybd(s.ToArray(), dwFlags_time);
                  }
            }

            //獲取鍵碼 這一部分 就是根據字串 獲取 鍵碼 這裡只列出了一部分 可以自己修改
            public Byte getKeys(string key)
            {
                  switch (key)
                  {
                        case "A": return (Byte)Keys.A;
                        case "B": return (Byte)Keys.B;
                        case "C": return (Byte)Keys.C;
                        case "D": return (Byte)Keys.D;
                        case "E": return (Byte)Keys.E;
                        case "F": return (Byte)Keys.F;
                        case "G": return (Byte)Keys.G;
                        case "H": return (Byte)Keys.H;
                        case "I": return (Byte)Keys.I;
                        case "J": return (Byte)Keys.J;
                        case "K": return (Byte)Keys.K;
                        case "L": return (Byte)Keys.L;
                        case "M": return (Byte)Keys.M;
                        case "N": return (Byte)Keys.N;
                        case "O": return (Byte)Keys.O;
                        case "P": return (Byte)Keys.P;
                        case "Q": return (Byte)Keys.Q;
                        case "R": return (Byte)Keys.R;
                        case "S": return (Byte)Keys.S;
                        case "T": return (Byte)Keys.T;
                        case "U": return (Byte)Keys.U;
                        case "V": return (Byte)Keys.V;
                        case "W": return (Byte)Keys.W;
                        case "X": return (Byte)Keys.X;
                        case "Y": return (Byte)Keys.Y;
                        case "Z": return (Byte)Keys.Z;
                        case "Add": return (Byte)Keys.Add;
                        case "Back": return (Byte)Keys.Back;
                        case "Cancel": return (Byte)Keys.Cancel;
                        case "Capital": return (Byte)Keys.Capital;
                        case "CapsLock": return (Byte)Keys.CapsLock;
                        case "Clear": return (Byte)Keys.Clear;
                        case "Crsel": return (Byte)Keys.Crsel;
                        case "ControlKey": return (Byte)Keys.ControlKey;
                        case "D0": return (Byte)Keys.D0;
                        case "D1": return (Byte)Keys.D1;
                        case "D2": return (Byte)Keys.D2;
                        case "D3": return (Byte)Keys.D3;
                        case "D4": return (Byte)Keys.D4;
                        case "D5": return (Byte)Keys.D5;
                        case "D6": return (Byte)Keys.D6;
                        case "D7": return (Byte)Keys.D7;
                        case "D8": return (Byte)Keys.D8;
                        case "D9": return (Byte)Keys.D9;
                        case "Decimal": return (Byte)Keys.Decimal;
                        case "Delete": return (Byte)Keys.Delete;
                        case "Divide": return (Byte)Keys.Divide;
                        case "Down": return (Byte)Keys.Down;
                        case "End": return (Byte)Keys.End;
                        case "Enter": return (Byte)Keys.Enter;
                        case "Escape": return (Byte)Keys.Escape;
                        case "F1": return (Byte)Keys.F1;
                        case "F2": return (Byte)Keys.F2;
                        case "F3": return (Byte)Keys.F3;
                        case "F4": return (Byte)Keys.F4;
                        case "F5": return (Byte)Keys.F5;
                        case "F6": return (Byte)Keys.F6;
                        case "F7": return (Byte)Keys.F7;
                        case "F8": return (Byte)Keys.F8;
                        case "F9": return (Byte)Keys.F9;
                        case "F10": return (Byte)Keys.F10;
                        case "F11": return (Byte)Keys.F11;
                        case "F12": return (Byte)Keys.F12;
                        case "Help": return (Byte)Keys.Help;
                        case "Home": return (Byte)Keys.Home;
                        case "Insert": return (Byte)Keys.Insert;
                        case "LButton": return (Byte)Keys.LButton;
                        case "LControl": return (Byte)Keys.LControlKey;
                        case "Left": return (Byte)Keys.Left;
                        case "LMenu": return (Byte)Keys.LMenu;
                        case "LShift": return (Byte)Keys.LShiftKey;
                        case "LWin": return (Byte)Keys.LWin;
                        case "MButton": return (Byte)Keys.MButton;
                        case "Menu": return (Byte)Keys.Menu;
                        case "Multiply": return (Byte)Keys.Multiply;
                        case "Next": return (Byte)Keys.Next;
                        case "NumLock": return (Byte)Keys.NumLock;
                        case "NumPad0": return (Byte)Keys.NumPad0;
                        case "NumPad1": return (Byte)Keys.NumPad1;
                        case "NumPad2": return (Byte)Keys.NumPad2;
                        case "NumPad3": return (Byte)Keys.NumPad3;
                        case "NumPad4": return (Byte)Keys.NumPad4;
                        case "NumPad5": return (Byte)Keys.NumPad5;
                        case "NumPad6": return (Byte)Keys.NumPad6;
                        case "NumPad7": return (Byte)Keys.NumPad7;
                        case "NumPad8": return (Byte)Keys.NumPad8;
                        case "NumPad9": return (Byte)Keys.NumPad9;
                        case "PageDown": return (Byte)Keys.PageDown;
                        case "PageUp": return (Byte)Keys.PageUp;
                        case "Process": return (Byte)Keys.ProcessKey;
                        case "RButton": return (Byte)Keys.RButton;
                        case "Right": return (Byte)Keys.Right;
                        case "RControl": return (Byte)Keys.RControlKey;
                        case "RMenu": return (Byte)Keys.RMenu;
                        case "RShift": return (Byte)Keys.RShiftKey;
                        case "Scroll": return (Byte)Keys.Scroll;
                        case "Space": return (Byte)Keys.Space;
                        case "Tab": return (Byte)Keys.Tab;
                        case "Up": return (Byte)Keys.Up;
                  }
                  return 0;
            }

            #endregion

            //---------------------------------進階--------------------------------------------------------------------------

            #region 其他函式 (下面進階找東西用的)
            bool ColorAEqualColorB(System.Drawing.Color colorA, System.Drawing.Color colorB, byte errorRange = 10)
            {
                  return colorA.A <= colorB.A + errorRange && colorA.A >= colorB.A - errorRange &&
                      colorA.R <= colorB.R + errorRange && colorA.R >= colorB.R - errorRange &&
                      colorA.G <= colorB.G + errorRange && colorA.G >= colorB.G - errorRange &&
                      colorA.B <= colorB.B + errorRange && colorA.B >= colorB.B - errorRange;

            }
            bool ListContainsPoint(List<System.Drawing.Point> listPoint, System.Drawing.Point point, double errorRange = 10)
            {
                  bool isExist = false;
                  foreach (var item in listPoint)
                  {
                        if (item.X <= point.X + errorRange && item.X >= point.X - errorRange && item.Y <= point.Y + errorRange && item.Y >= point.Y - errorRange)
                        {
                              isExist = true;
                        }
                  }
                  return isExist;
            }
            bool ListTextBodyContainsPoint(List<NumBody> listPoint, System.Drawing.Point point, double errorRange = 10)
            {
                  bool isExist = false;
                  foreach (var item in listPoint)
                  {

                        if (item.point.X <= point.X + errorRange && item.point.X >= point.X - errorRange && item.point.Y <= point.Y + errorRange && item.point.Y >= point.Y - errorRange)
                        {
                              isExist = true;
                        }
                  }
                  return isExist;
            }
            #endregion

            #region 找顏色
            /// <summary>
            /// 找顏色
            /// </summary>
            /// <param name="parPic">查詢的圖片的絕對路徑</param>
            /// <param name="searchColor">查詢的16進位制顏色值，如#0C5FAB</param>
            /// <param name="searchRect">查詢的矩形區域範圍內</param>
            /// <param name="errorRange">容錯</param>
            /// <returns></returns>
            Point FindColor(string parPic, string searchColor, Rectangle searchRect, byte errorRange = 10)
            {
                  var colorX = ColorTranslator.FromHtml(searchColor);
                  var parBitmap = new Bitmap(parPic);
                  var parData = parBitmap.LockBits(new Rectangle(0, 0, parBitmap.Width, parBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                  var byteArraryPar = new byte[parData.Stride * parData.Height];
                  Marshal.Copy(parData.Scan0, byteArraryPar, 0, parData.Stride * parData.Height);
                  if (searchRect.IsEmpty)
                  {
                        searchRect = new Rectangle(0, 0, parBitmap.Width, parBitmap.Height);
                  }
                  var searchLeftTop = searchRect.Location;
                  var searchSize = searchRect.Size;
                  var iMax = searchLeftTop.Y + searchSize.Height;//行
                  var jMax = searchLeftTop.X + searchSize.Width;//列
                  int pointX = -1; int pointY = -1;
                  for (int m = searchRect.Y; m < iMax; m++)
                  {
                        for (int n = searchRect.X; n < jMax; n++)
                        {
                              int index = m * parBitmap.Width * 4 + n * 4;
                              var color = Color.FromArgb(byteArraryPar[index + 3], byteArraryPar[index + 2], byteArraryPar[index + 1], byteArraryPar[index]);
                              if (ColorAEqualColorB(color, colorX, errorRange))
                              {
                                    pointX = n;
                                    pointY = m;
                                    goto END;
                              }
                        }
                  }

                  END:
                  parBitmap.UnlockBits(parData);
                  return new Point(pointX, pointY);
            }
            #endregion

            #region 找圖

            /// <summary>
            /// 查詢圖片，不能鏤空
            /// </summary>
            /// <param name="subPic"></param>
            /// <param name="parPic"></param>
            /// <param name="searchRect">如果為empty，則預設查詢整個影象</param>
            /// <param name="errorRange">容錯，單個色值範圍內視為正確0~255</param>
            /// <param name="matchRate">圖片匹配度，預設90%</param>
            /// <param name="isFindAll">是否查詢所有相似的圖片</param>
            /// <returns>返回查詢到的圖片的中心點座標</returns>
            List<Point> FindPicture(string subPic, string parPic, Rectangle searchRect, byte errorRange, double matchRate = 0.9, bool isFindAll = false)
            {
                  List<Point> ListPoint = new List<Point>();
                  var subBitmap = new Bitmap(subPic);
                  var parBitmap = new Bitmap(parPic);
                  int subWidth = subBitmap.Width;
                  int subHeight = subBitmap.Height;
                  int parWidth = parBitmap.Width;
                  int parHeight = parBitmap.Height;
                  if (searchRect.IsEmpty)
                  {
                        searchRect = new Rectangle(0, 0, parBitmap.Width, parBitmap.Height);
                  }

                  var searchLeftTop = searchRect.Location;
                  var searchSize = searchRect.Size;
                  Color startPixelColor = subBitmap.GetPixel(0, 0);
                  var subData = subBitmap.LockBits(new Rectangle(0, 0, subBitmap.Width, subBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                  var parData = parBitmap.LockBits(new Rectangle(0, 0, parBitmap.Width, parBitmap.Height), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
                  var byteArrarySub = new byte[subData.Stride * subData.Height];
                  var byteArraryPar = new byte[parData.Stride * parData.Height];
                  Marshal.Copy(subData.Scan0, byteArrarySub, 0, subData.Stride * subData.Height);
                  Marshal.Copy(parData.Scan0, byteArraryPar, 0, parData.Stride * parData.Height);

                  var iMax = searchLeftTop.Y + searchSize.Height - subData.Height;//行
                  var jMax = searchLeftTop.X + searchSize.Width - subData.Width;//列

                  int smallOffsetX = 0, smallOffsetY = 0;
                  int smallStartX = 0, smallStartY = 0;
                  int pointX = -1; int pointY = -1;
                  for (int i = searchLeftTop.Y; i < iMax; i++)
                  {
                        for (int j = searchLeftTop.X; j < jMax; j++)
                        {
                              //大圖x，y座標處的顏色值
                              int x = j, y = i;
                              int parIndex = i * parWidth * 4 + j * 4;
                              var colorBig = Color.FromArgb(byteArraryPar[parIndex + 3], byteArraryPar[parIndex + 2], byteArraryPar[parIndex + 1], byteArraryPar[parIndex]);
                              ;
                              if (ColorAEqualColorB(colorBig, startPixelColor, errorRange))
                              {
                                    smallStartX = x - smallOffsetX;//待找的圖X座標
                                    smallStartY = y - smallOffsetY;//待找的圖Y座標
                                    int sum = 0;//所有需要比對的有效點
                                    int matchNum = 0;//成功匹配的點
                                    for (int m = 0; m < subHeight; m++)
                                    {
                                          for (int n = 0; n < subWidth; n++)
                                          {
                                                int x1 = n, y1 = m;
                                                int subIndex = m * subWidth * 4 + n * 4;
                                                var color = Color.FromArgb(byteArrarySub[subIndex + 3], byteArrarySub[subIndex + 2], byteArrarySub[subIndex + 1], byteArrarySub[subIndex]);

                                                sum++;
                                                int x2 = smallStartX + x1, y2 = smallStartY + y1;
                                                int parReleativeIndex = y2 * parWidth * 4 + x2 * 4;//比對大圖對應的畫素點的顏色
                                                var colorPixel = Color.FromArgb(byteArraryPar[parReleativeIndex + 3], byteArraryPar[parReleativeIndex + 2], byteArraryPar[parReleativeIndex + 1], byteArraryPar[parReleativeIndex]);
                                                if (ColorAEqualColorB(colorPixel, color, errorRange))
                                                {
                                                      matchNum++;
                                                }
                                          }
                                    }
                                    if ((double)matchNum / sum >= matchRate)
                                    {
                                          Console.WriteLine((double)matchNum / sum);
                                          pointX = smallStartX + (int)(subWidth / 2.0);
                                          pointY = smallStartY + (int)(subHeight / 2.0);
                                          var point = new Point(pointX, pointY);
                                          if (!ListContainsPoint(ListPoint, point, 10))
                                          {
                                                ListPoint.Add(point);
                                          }
                                          if (!isFindAll)
                                          {
                                                goto FIND_END;
                                          }
                                    }
                              }
                              //小圖x1,y1座標處的顏色值
                        }
                  }

                  FIND_END:
                  subBitmap.UnlockBits(subData);
                  parBitmap.UnlockBits(parData);
                  subBitmap.Dispose();
                  parBitmap.Dispose();
                  GC.Collect();
                  return ListPoint;
            }

            #endregion

            #region 查詢數字

            /// <summary>
            /// 在指定區域裡面查詢數字
            /// </summary>
            /// <param name="numDic"></param>
            /// <param name="parPic"></param>
            /// <param name="searchRect"></param>
            /// <param name="errorRange"></param>
            /// <returns></returns>
            int FindNumbers(Dictionary<int, string> numDic, string parPic, System.Drawing.Rectangle searchRect, byte errorRange = 8, double matchRate = 0.9)
            {
                  //同一個區域找到多個相同的圖片
                  List<NumBody> ListBody = new List<NumBody>();
                  foreach (var item in numDic)
                  {
                        var listPoint = FindText(item.Value, parPic, searchRect, errorRange, matchRate, true);
                        foreach (var point in listPoint)
                        {
                              ListBody.Add(new NumBody() { num = item.Key, matchNum = point.matchNum, matchSum = point.matchSum, matchRate = point.matchRate, point = point.point, bodyCollectionPoint = point.bodyCollectionPoint });
                        }
                  }

                  SearchNumbersByMatchNum(ref ListBody);
                  var myList = from body in ListBody orderby body.point.X ascending select body;
                  string number = "0";
                  foreach (var item in myList)
                  {
                        number += item.num;
                  }
                  int num = Int32.Parse(number);
                  return num;
            }
            /// <summary>
            /// 搜尋同一個數字的時候，出現重疊的地方，用匹配度去過濾掉匹配度低的
            /// 比如同樣是1，在控制匹配度允許下，一個（83,95）和（84,95）這兩個點明顯是同一個數字
            /// 此時誰的匹配度低過濾掉誰
            /// </summary>
            /// <param name="ListBody"></param>
            void SearchNumbersByMatchNum(ref List<NumBody> ListBody)
            {
                  bool isValid = true;
                  for (int i = 0; i < ListBody.Count; i++)
                  {
                        var body = ListBody[i];

                        for (int j = i; j < ListBody.Count; j++)
                        {

                              var bodyX = ListBody[j];
                              if (!bodyX.Equals(body))
                              {
                                    int sameNum = 0;
                                    foreach (var item in body.bodyCollectionPoint)
                                    {
                                          if (bodyX.bodyCollectionPoint.Contains(item))
                                          {
                                                sameNum++;
                                          }
                                    }
                                    if (sameNum >= 1)//有1個以上點重合，表面影象重疊，刪除畫素點數少的影象
                                    {
                                          isValid = false;

                                          //如果某個數字100%匹配，那就不用比較了，這個數字肯定是對的
                                          double maxRate = 1;
                                          if (bodyX.matchRate >= maxRate)
                                          {
                                                ListBody.Remove(body);
                                          }
                                          else if (body.matchRate >= maxRate)
                                          {
                                                ListBody.Remove(bodyX);
                                          }
                                          else
                                          {
                                                if (bodyX.matchNum >= body.matchNum)//影象包含的所有畫素個數
                                                {
                                                      ListBody.Remove(body);
                                                }
                                                else
                                                {
                                                      ListBody.Remove(bodyX);
                                                }
                                          }
                                          SearchNumbersByMatchNum(ref ListBody);
                                    }
                              }
                        }
                  }
                  if (isValid)
                  {
                        return;
                  }
            }
            #endregion

            #region 找字

            /// <summary>
            /// 找文字，鏤空的圖片文字
            /// </summary>
            /// <param name="subPic"></param>
            /// <param name="parPic"></param>
            /// <param name="searchRect"></param>
            /// <param name="errorRange"></param>
            /// <param name="matchRate"></param>
            /// <param name="isFindAll"></param>
            /// <returns></returns>
            List<NumBody> FindText(string subPic, string parPic, System.Drawing.Rectangle searchRect, byte errorRange, double matchRate = 0.9, bool isFindAll = false)
            {

                  List<NumBody> ListPoint = new List<NumBody>();
                  var subBitmap = new Bitmap(subPic);
                  var parBitmap = new Bitmap(parPic);
                  int subWidth = subBitmap.Width;
                  int subHeight = subBitmap.Height;
                  int parWidth = parBitmap.Width;
                  int parHeight = parBitmap.Height;
                  var bgColor = subBitmap.GetPixel(0, 0);//背景紅色
                  if (searchRect.IsEmpty)
                  {
                        searchRect = new System.Drawing.Rectangle(0, 0, parBitmap.Width, parBitmap.Height);
                  }
                  var searchLeftTop = searchRect.Location;
                  var searchSize = searchRect.Size;
                  var subData = subBitmap.LockBits(new System.Drawing.Rectangle(0, 0, subBitmap.Width, subBitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                  var parData = parBitmap.LockBits(new System.Drawing.Rectangle(0, 0, parBitmap.Width, parBitmap.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                  var byteArrarySub = new byte[subData.Stride * subData.Height];
                  var byteArraryPar = new byte[parData.Stride * parData.Height];
                  Marshal.Copy(subData.Scan0, byteArrarySub, 0, subData.Stride * subData.Height);
                  Marshal.Copy(parData.Scan0, byteArraryPar, 0, parData.Stride * parData.Height);
                  var iMax = searchLeftTop.Y + searchSize.Height - subData.Height;//行
                  var jMax = searchLeftTop.X + searchSize.Width - subData.Width;//列
                  System.Drawing.Color startPixelColor = System.Drawing.Color.FromArgb(0, 0, 0);
                  int smallOffsetX = 0, smallOffsetY = 0;
                  int smallStartX = 0, smallStartY = 0;
                  int pointX = -1; int pointY = -1;


                  for (int m = 0; m < subHeight; m++)
                  {
                        for (int n = 0; n < subWidth; n++)
                        {
                              smallOffsetX = n;
                              smallOffsetY = m;
                              int subIndex = m * subWidth * 4 + n * 4;
                              var color = System.Drawing.Color.FromArgb(byteArrarySub[subIndex + 3], byteArrarySub[subIndex + 2], byteArrarySub[subIndex + 1], byteArrarySub[subIndex]);
                              if (!ColorAEqualColorB(color, bgColor, errorRange))
                              {
                                    startPixelColor = color;
                                    goto END;
                              }
                        }
                  }

                  END:
                  for (int i = searchLeftTop.Y; i < iMax; i++)
                  {
                        for (int j = searchLeftTop.X; j < jMax; j++)
                        {
                              //大圖x，y座標處的顏色值
                              int x = j, y = i;
                              int parIndex = i * parWidth * 4 + j * 4;
                              var colorBig = System.Drawing.Color.FromArgb(byteArraryPar[parIndex + 3], byteArraryPar[parIndex + 2], byteArraryPar[parIndex + 1], byteArraryPar[parIndex]);


                              List<System.Drawing.Point> myListPoint = new List<System.Drawing.Point>();
                              if (ColorAEqualColorB(colorBig, startPixelColor, errorRange))
                              {
                                    smallStartX = x - smallOffsetX;//待找的圖X座標
                                    smallStartY = y - smallOffsetY;//待找的圖Y座標
                                    int sum = 0;//所有需要比對的有效點
                                    int matchNum = 0;//成功匹配的點
                                    for (int m = 0; m < subHeight; m++)
                                    {
                                          for (int n = 0; n < subWidth; n++)
                                          {
                                                int x1 = n, y1 = m;
                                                int subIndex = m * subWidth * 4 + n * 4;
                                                var color = System.Drawing.Color.FromArgb(byteArrarySub[subIndex + 3], byteArrarySub[subIndex + 2], byteArrarySub[subIndex + 1], byteArrarySub[subIndex]);
                                                if (color != bgColor)
                                                {
                                                      sum++;
                                                      int x2 = smallStartX + x1, y2 = smallStartY + y1;
                                                      int parReleativeIndex = y2 * parWidth * 4 + x2 * 4;//比對大圖對應的畫素點的顏色
                                                      var colorPixel = System.Drawing.Color.FromArgb(byteArraryPar[parReleativeIndex + 3], byteArraryPar[parReleativeIndex + 2], byteArraryPar[parReleativeIndex + 1], byteArraryPar[parReleativeIndex]);
                                                      if (ColorAEqualColorB(colorPixel, color, errorRange))
                                                      {
                                                            matchNum++;
                                                      }
                                                      myListPoint.Add(new System.Drawing.Point(x2, y2));
                                                }
                                          }
                                    }

                                    double rate = (double)matchNum / sum;
                                    if (rate >= matchRate)
                                    {
                                          Console.WriteLine((double)matchNum / sum);
                                          pointX = smallStartX + (int)(subWidth / 2.0);
                                          pointY = smallStartY + (int)(subHeight / 2.0);
                                          var point = new System.Drawing.Point(pointX, pointY);
                                          if (!ListTextBodyContainsPoint(ListPoint, point, 1))
                                          {
                                                ListPoint.Add(new NumBody() { point = point, matchNum = matchNum, matchSum = sum, matchRate = rate, bodyCollectionPoint = myListPoint });
                                          }
                                          SearchNumbersByMatchNum(ref ListPoint);
                                          if (!isFindAll)
                                          {
                                                goto FIND_END;
                                          }
                                    }
                              }
                              //小圖x1,y1座標處的顏色值
                        }
                  }
                  FIND_END:
                  subBitmap.UnlockBits(subData);
                  parBitmap.UnlockBits(parData);
                  subBitmap.Dispose();
                  parBitmap.Dispose();
                  GC.Collect();
                  return ListPoint;
            }
            #endregion
      }
}
