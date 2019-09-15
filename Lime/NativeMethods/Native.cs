using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using static Lime.Helper.PreventSleep;

namespace Lime.NativeMethods
{
    public static class Native
    {

        [DllImport("kernel32", CharSet = CharSet.Ansi, EntryPoint = "GetVolumeInformationA", ExactSpelling = true, SetLastError = true)]
        public static extern int GVI([MarshalAs(UnmanagedType.VBByRefStr)] ref string IP, [MarshalAs(UnmanagedType.VBByRefStr)] ref string V, int T, ref int H, ref int Q, ref int G, [MarshalAs(UnmanagedType.VBByRefStr)] ref string J, int X);

        [DllImport("user32", CharSet = CharSet.Ansi, EntryPoint = "GetForegroundWindow", ExactSpelling = true, SetLastError = true)]
        public static extern IntPtr GFW();

        [DllImport("user32", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int cch);

        [DllImport("avicap32.dll", CharSet = CharSet.Ansi, ExactSpelling = true, SetLastError = true)]
        public static extern bool capGetDriverDescriptionA(short wDriver, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpszName, int cbName, [MarshalAs(UnmanagedType.VBByRefStr)] ref string lpszVer, int cbVer);

        [DllImport("kernel32.dll", SetLastError = true)]
        public static extern EXECUTION_STATE SetThreadExecutionState(EXECUTION_STATE esFlags);
    }
}
