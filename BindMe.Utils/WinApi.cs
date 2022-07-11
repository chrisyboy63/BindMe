using System;
using System.Runtime.InteropServices;

namespace WinApi
{

    ///
    // Hot Key API for Registering and UnRegistering Hot Keys
    ///
    public static class HotKeyApi
    {
        [StructLayout(LayoutKind.Sequential)]
        public struct KeyboardInput
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MouseInput
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HardwareInput
        {
            public uint uMsg;
            public ushort wParamL;
            public ushort wParamH;
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct InputUnion
        {
            [FieldOffset(0)] public MouseInput mi;
            [FieldOffset(0)] public KeyboardInput ki;
            [FieldOffset(0)] public HardwareInput hi;
        }

        public struct Input
        {
            public int type;
            public InputUnion u;
        }

        [Flags]
        public enum InputType
        {
            Mouse = 0,
            Keyboard = 1,
            Hardware = 2
        }

        [Flags]
        public enum KeyEventF
        {
            KeyDown = 0x0000,
            ExtendedKey = 0x0001,
            KeyUp = 0x0002,
            Unicode = 0x0004,
            Scancode = 0x0008
        }

        [Flags]
        public enum MouseEventF
        {
            Absolute = 0x8000,
            HWheel = 0x01000,
            Move = 0x0001,
            MoveNoCoalesce = 0x2000,
            LeftDown = 0x0002,
            LeftUp = 0x0004,
            RightDown = 0x0008,
            RightUp = 0x0010,
            MiddleDown = 0x0020,
            MiddleUp = 0x0040,
            VirtualDesk = 0x4000,
            Wheel = 0x0800,
            XDown = 0x0080,
            XUp = 0x0100
        }

        [DllImport("User32.dll")]
        public static extern bool RegisterHotKey(IntPtr hWnd, int id, int modifiers, int key);
        [DllImport("User32.dll")]
        public static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        [DllImport("User32.dll")]
        public static extern uint SendInput(uint nInputs, Input[] pInputs, int cbSize);
    }

    ///
    // Windows Message API Capturing and Sending Messages to Windows
    ///
    public static class WindowsMessaging
    {
        public static readonly uint WM_HOTKEY = (uint)0x312;

        public static readonly int HwndBroadcast = 0xffff;

        public struct POINT
        {
            public int X { get; set; }
            public int Y { get; set; }
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct MSG
        {
            public IntPtr hwnd;
            public uint message;
            public UIntPtr wParam;
            public IntPtr lParam;
            public int time;
            public POINT pt;
            public int lPrivate;
        }

        [DllImport("User32.dll")]
        public static extern int GetMessage(out MSG lpMsg, IntPtr hWnd, uint msgFilterMin, uint msgFilterMax);

        [DllImport("User32.dll")]
        public static extern bool PostMessage(IntPtr hWnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport("User32.dll")]
        public static extern int RegisterWindowMessage(string message);

        [DllImport("user32.dll")]
        public static extern IntPtr GetMessageExtraInfo();

    }

    ///
    // Windows Message Box API to show Message Boxes
    ///
    public static class MessageBoxApi
    {

        public static class Buttons
        {
            public static readonly uint MB_ABORTRETRYIGNORE = (uint)0x00000002L;
            public static readonly uint MB_OK = (uint)0x00000000L;
        }

        public static class Icons
        {
            public static readonly uint MB_ICONEXCLAMATION = (uint)0x00000030L;
            public static readonly uint MB_ICONWARNING = MB_ICONEXCLAMATION;
            public static readonly uint MB_ICONINFORMATION = (uint)0x00000040L;
            public static readonly uint MB_ICONASTERISK = MB_ICONINFORMATION;
            public static readonly uint MB_ICONQUESTION = (uint)0x00000020L;
            public static readonly uint MB_ICONSTOP = (uint)0x00000010L;
            public static readonly uint MB_ICONERROR = MB_ICONSTOP;
            public static readonly uint MB_ICONHAND = MB_ICONSTOP;
        }

        [DllImport("User32.dll")]
        public static extern Int32 MessageBox(IntPtr hWnd, string lpText, string lpCaption, uint uType);

    }

}
