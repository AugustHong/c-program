using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

/*
    參考網址： https://boywhy.blogspot.com/2015/01/c-c.html 、 http://luke0701.blogspot.com/2013/11/c.html
    基本上要讀取 讀卡機都要用 APDU 指定來實作
 */

namespace 讀取讀卡機健保卡資訊
{
    internal class Program
    {
        // 卡片 Request 欄位
        public struct SCARD_IO_REQUEST
        {
            public int dwProtocol;
            public int cbPciLength;
        }

        #region 引用 PC/SC(Personal Computer/Smart Card) API WinScard.dll
        // 建立 Smart Card API
        [DllImport("WinScard.dll")]
        public static extern int SCardEstablishContext([In] Int32 dwScope,
            [In] int nNotUsed1, [In] int nNotUsed2, [In, Out] ref int phContext);

        [DllImport("WinScard.dll")]
        public static extern int SCardReleaseContext(int phContext);

        // 連線
        [DllImport("WinScard.dll")]
        public static extern int SCardConnect(int hContext, string cReaderName,
            uint dwShareMode, uint dwPrefProtocol, ref int phCard, ref int ActiveProtocol);

        // 中斷連線
        [DllImport("WinScard.dll")]
        public static extern int SCardDisconnect(int hCard, int Disposition);

        // 讀取有哪些讀卡機
        [DllImport("WinScard.dll")]
        public static extern int SCardListReaders(int hContext, string cGroups,
            ref string cReaderLists, ref int nReaderCount);

        // 執行指令操作
        [DllImport("WinScard.dll")]
        public static extern int SCardTransmit(int hCard,
            ref SCARD_IO_REQUEST pioSendPci, byte[] pbSendBuffer, int cbSendLength,
            ref SCARD_IO_REQUEST pioRecvPci, ref byte pbRecvBuffer, ref int pcbRecvLength);

        #endregion

        static void Main(string[] args)
        {
            int ContextHandle = 0;
            int CardHandle = 0;
            int ActiveProtocol = 0;
            int ReaderCount = -1;
            string ReaderList = string.Empty; //讀卡機名稱列表

            SCARD_IO_REQUEST SendPci, RecvPci;

            byte[] SelectAPDU = { 0x00, 0xA4, 0x04, 0x00, 0x10, 0xD1, 0x58, 0x00, 0x00, 0x01, 0x00, 0x00,
        0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x11, 0x00 };
            byte[] ReadProfileAPDU = { 0x00, 0xca, 0x11, 0x00, 0x02, 0x00, 0x00 };
            byte[] SelectRecvBytes = new byte[2]; //應回 90 00
            int SelectRecvLength = 2;
            byte[] ProfileRecvBytes = new byte[59]; //接收Profile的 Byte Array
            int ProfileRecvLength = 59;


            //建立 Smart Card API (要插入讀取機才會讀取到， = 0 是回傳成功)
            if (SCardEstablishContext(0, 0, 0, ref ContextHandle) == 0)
            {
                Console.WriteLine($"ContextHandle = {ContextHandle}");

                //列出可用的 Smart Card 讀卡機 (=0 是成功)
                if (SCardListReaders(0, null, ref ReaderList, ref ReaderCount) == 0)
                {
                    Console.WriteLine($"ReaderList = {ReaderList}   ,   ReaderCount = {ReaderCount}");

                    //建立 Smart Card 連線 ( = 0 是成功)
                    if (SCardConnect(ContextHandle, ReaderList, 1, 2, ref CardHandle, ref ActiveProtocol) == 0)
                    {
                        // 設定 Request
                        SendPci.dwProtocol = RecvPci.dwProtocol = ActiveProtocol;
                        SendPci.cbPciLength = RecvPci.cbPciLength = 8;

                        //下達 Select Profile 檔的 APDU
                        if (SCardTransmit(CardHandle, ref SendPci, SelectAPDU, SelectAPDU.Length, ref RecvPci, ref SelectRecvBytes[0], ref SelectRecvLength) == 0)
                        {
                            //下達讀取Profile指令
                            if (SCardTransmit(CardHandle, ref SendPci, ReadProfileAPDU, ReadProfileAPDU.Length, ref RecvPci, ref ProfileRecvBytes[0], ref ProfileRecvLength) == 0)
                            {
                                 // 全部樣式
                                 string all = Encoding.Default.GetString(ProfileRecvBytes);

                                // 健保卡ID
                                string healthID = Encoding.Default.GetString(ProfileRecvBytes, 0, 12);
                                // 姓名
                                string name = Encoding.Default.GetString(ProfileRecvBytes, 12, 6);
                                // 身份證字號
                                string idNo = Encoding.Default.GetString(ProfileRecvBytes, 32, 10);
                                // 生日
                                string birthDay = $"{Encoding.Default.GetString(ProfileRecvBytes, 43, 2)}/{Encoding.Default.GetString(ProfileRecvBytes, 45, 2)}/{Encoding.Default.GetString(ProfileRecvBytes, 47, 2)}";
                                // 姓別
                                string gender = Encoding.Default.GetString(ProfileRecvBytes, 49, 1);
                                // 發卡日期
                                string cardCreateDate = $"{Encoding.Default.GetString(ProfileRecvBytes, 51, 2)}/{Encoding.Default.GetString(ProfileRecvBytes, 53, 2)}/{Encoding.Default.GetString(ProfileRecvBytes, 55, 2)}";

                                Console.WriteLine($"健保卡ID:{healthID}\n" +
                                    $"姓名:{name}\n" +
                                    $"身份証字號:{idNo}\n" +
                                    $"生日:{birthDay}\n" +
                                    $"姓別:{gender}\n" +
                                    $"發卡日期:{cardCreateDate}");
                            }
                        }
                    }
                    else
                    {
                        Console.WriteLine("建立連線失敗");
                    }
                }
                else
                {
                    Console.WriteLine("無可用的讀卡機");
                }
            }
            else
            {
                Console.WriteLine($"建立失敗， ContextHandle = {ContextHandle}");
            }

            Console.ReadKey();
        }
    }
}
