using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

/*
    參考網址： https://www.cnblogs.com/zoudiaogangqin/p/5199355.html
 */

namespace WInForm無邊框釘在桌面上
{
    public static class FixDesktop
    {
        public const int SE_SHUTDOWN_PRIVILEGE = 0x13;
        public const int WM_SYSCOMMAND = 0x0112;
        public const int SC_MOVE = 0xF010;
        public const int HTCAPTION = 0x0002;//無邊框移動

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll")]
        public static extern IntPtr SetParent(IntPtr hWndChild, IntPtr hWndNewParent);
        [DllImport("user32.dll")]
        public static extern bool SetWindowPos(IntPtr hWnd, int hWndInsertAfter, int X, int Y, int cx,
            int cy, uint uFlags);
        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);

        #region 固定在桌面上
        public static void SetToDeskTop(Form form)
        {
            try
            {
                if (Environment.OSVersion.Version.Major < 6)
                {
                    form.SendToBack();
                    IntPtr hWndNewParent = FindWindow("Progman", null);
                    SetParent(form.Handle, hWndNewParent);
                }
                else
                {
                    IntPtr desktopHwnd = GetDesktopPtr();
                    IntPtr ownHwnd = form.Handle;
                    IntPtr result = SetParent(ownHwnd, desktopHwnd);
                }

            }
            catch (ApplicationException exx)
            {
                MessageBox.Show(form, exx.Message, "錯誤訊息");
            }
        }

        public static IntPtr GetDesktopPtr()
        {
            //http://blog.csdn.net/mkdym/article/details/7018318

            // 情况一
            IntPtr hwndWorkerW = IntPtr.Zero;
            IntPtr hShellDefView = IntPtr.Zero;
            IntPtr hwndDesktop = IntPtr.Zero;
            IntPtr hProgMan = FindWindow("ProgMan", null);
            if (hProgMan != IntPtr.Zero)
            {
                hShellDefView = FindWindowEx(hProgMan, IntPtr.Zero, "SHELLDLL_DefView", null);
                if (hShellDefView != IntPtr.Zero)
                {
                    hwndDesktop = FindWindowEx(hShellDefView, IntPtr.Zero, "SysListView32", null);
                }
            }
            if (hwndDesktop != IntPtr.Zero) return hwndDesktop;

            // 情况二
            while (hwndDesktop == IntPtr.Zero)
            {//必須在桌面層  
                hwndWorkerW = FindWindowEx(IntPtr.Zero, hwndWorkerW, "WorkerW", null); //獲得WorkerW類
                                                                                       //
                if (hwndWorkerW == IntPtr.Zero) break;//未知錯誤
                hShellDefView = FindWindowEx(hwndWorkerW, IntPtr.Zero, "SysListView32", null);
                if (hShellDefView == IntPtr.Zero) continue;
                hwndDesktop = FindWindowEx(hShellDefView, IntPtr.Zero, "SysListView32", null);
            }

            return hwndDesktop;
        }
        
        public static void ActiveOrPaint(Form form)
        {
            if (Environment.OSVersion.Version.Major >= 6)
            {
                SetWindowPos(form.Handle, 1, 0, 0, 0, 0, SE_SHUTDOWN_PRIVILEGE);
            }
        }
        #endregion
    }
}
