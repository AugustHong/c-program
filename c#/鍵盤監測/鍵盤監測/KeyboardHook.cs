using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Reflection;

namespace 鍵盤監測
{
    /// <summary>
    /// 鍵盤鉤子 (Hook ， 要監控都會掛載 Hook)
    /// 以下是參考 https://www.cnblogs.com/ayqy/p/3636427.html ，非本人原創寫法。供自己參考學習用
    /// </summary>
    public class KeyboardHook
    {
        // 宣告告事件
        public event KeyEventHandler KeyDownEvent;
        public event KeyPressEventHandler KeyPressEvent;
        public event KeyEventHandler KeyUpEvent;

        public delegate int HookProc(int nCode, Int32 wParam, IntPtr lParam);  // 宣告處理動作的Delegate
        static int hKeyboardHook = 0; //鍵盤Hook初始值

        public const int WH_KEYBOARD_LL = 13;   //Local鍵盤監聽滑乘設為2，Global鍵盤監聽滑乘設為13 (詳細值可在 Microsoft SDK的Winuser.h裡查询)
        HookProc KeyboardHookProcedure; //宣告KeyboardHookProcedure作為HookProc類型

        //鍵盤結構
        [StructLayout(LayoutKind.Sequential)]
        public class KeyboardHookStruct
        {
            public int vkCode;  //定義1個虛擬鍵碼。範圍1至254
            public int scanCode; // 指定硬體掃描碼
            public int flags;  // 標誌
            public int time; // 訊息時間戳記
            public int dwExtraInfo; // 指定額外訊息
        }


        //使用此功能，安裝Hook
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

        //使用此功能，解安裝Hook
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern bool UnhookWindowsHookEx(int idHook);

        //使用此功能，繼續取得下一個Hook
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern int CallNextHookEx(int idHook, int nCode, Int32 wParam, IntPtr lParam);

        // 取得當前程序編號(Local Hook 需要)
        [DllImport("kernel32.dll")]
        static extern int GetCurrentThreadId();

        //使用WINDOWS API函式取代當前實作的函式，防止Hook失效
        [DllImport("kernel32.dll")]
        public static extern IntPtr GetModuleHandle(string name);

        // 開始動作
        public void Start()
        {
            // 如果 Hook 是初始值，去安裝Hook
            if (hKeyboardHook == 0)
            {
                KeyboardHookProcedure = new HookProc(KeyboardHookProc);  // 給 Delegate 宣告實體

                // 安裝Hook
                hKeyboardHook = SetWindowsHookEx(WH_KEYBOARD_LL, KeyboardHookProcedure, GetModuleHandle(System.Diagnostics.Process.GetCurrentProcess().MainModule.ModuleName), 0);

                // 如果裝完Hook，值還是初始值 => 安裝失敗
                if (hKeyboardHook == 0)
                {
                    Stop();
                    throw new Exception("安裝 Hook 失敗");
                }
            }
        }

        // 停止動作
        public void Stop()
        {
            bool retKeyboard = true;

            // 若非 初始值 -> 可以解除安裝
            if (hKeyboardHook != 0)
            {
                // 解除安裝 Hook
                retKeyboard = UnhookWindowsHookEx(hKeyboardHook);
                hKeyboardHook = 0;
            }

            // 若 解除安裝失敗
            if (!(retKeyboard)) throw new Exception("解除安裝Hook失敗");
        }


        //ToAscii轉換鍵盤的值變成
        [DllImport("user32")]
        public static extern int ToAscii(int uVirtKey, int uScanCode, byte[] lpbKeyState, byte[] lpwTransKey, int fuState);

        //取得鍵盤狀態
        [DllImport("user32")]
        public static extern int GetKeyboardState(byte[] pbKeyState);

        // 取得按鍵狀態
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern short GetKeyState(int vKey);

        // 宣告各常數
        private const int WM_KEYDOWN = 0x100;//KEYDOWN
        private const int WM_KEYUP = 0x101;//KEYUP
        private const int WM_SYSKEYDOWN = 0x104;//SYSKEYDOWN
        private const int WM_SYSKEYUP = 0x105;//SYSKEYUP

        // 實際處理動作
        private int KeyboardHookProc(int nCode, Int32 wParam, IntPtr lParam)
        {
            // 監聽鍵盤事件
            if ((nCode >= 0) && (KeyDownEvent != null || KeyUpEvent != null || KeyPressEvent != null))
            {
                KeyboardHookStruct MyKeyboardHookStruct = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));

                // raise KeyDown
                if (KeyDownEvent != null && (wParam == WM_KEYDOWN || wParam == WM_SYSKEYDOWN))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    KeyDownEvent(this, e);
                }

                //按下按鍵
                if (KeyPressEvent != null && wParam == WM_KEYDOWN)
                {
                    byte[] keyState = new byte[256];
                    GetKeyboardState(keyState);

                    byte[] inBuffer = new byte[2];
                    // 轉換碼
                    if (ToAscii(MyKeyboardHookStruct.vkCode, MyKeyboardHookStruct.scanCode, keyState, inBuffer, MyKeyboardHookStruct.flags) == 1)
                    {
                        KeyPressEventArgs e = new KeyPressEventArgs((char)inBuffer[0]);
                        KeyPressEvent(this, e);
                    }
                }

                // 按起來
                if (KeyUpEvent != null && (wParam == WM_KEYUP || wParam == WM_SYSKEYUP))
                {
                    Keys keyData = (Keys)MyKeyboardHookStruct.vkCode;
                    KeyEventArgs e = new KeyEventArgs(keyData);
                    KeyUpEvent(this, e);
                }
            }

            // 如果為1，結束直接回覆。若為 0 則繼續呼叫下一個Hook (在組合鍵時用)
            return CallNextHookEx(hKeyboardHook, nCode, wParam, lParam);
        }

        // 初始化
        ~KeyboardHook()
        {
            Stop();
        }
    }
}
