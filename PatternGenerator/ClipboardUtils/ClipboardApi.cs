using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace PatternGenerator.ClipboardUtils
{
    internal class ClipboardApi
    {
        [DllImport("User32.dll")]
        public static extern bool OpenClipboard(IntPtr windowHandle);
        [DllImport("User32.dll")]
        public static extern bool CloseClipboard();
        [DllImport("User32.dll")]
        public static extern bool EmptyClipboard();
        [DllImport("User32.dll")]
        public static extern bool SetClipboardData(uint uFormat, IntPtr handle);
        [DllImport("User32.dll")]
        public static extern uint RegisterClipboardFormatW([MarshalAs(UnmanagedType.LPWStr)] string name);

    }
}
