using UnityEngine;
using System.Collections;
using System;
using System.Runtime.InteropServices;
using System.Threading;
using System.ComponentModel;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;

public class W7TouchManager : MonoBehaviour {
    private int hHook = 0;

    #region WIN_API
    private delegate int HookProc(int code, IntPtr wParam, IntPtr lParam);
    private const int WH_GETMESSAGE = 3;
    private const int HC_ACTION = 0;
    private const int TWF_WANTPALM = 0x00000002;
    private const int WM_TOUCH = 0x0240;
    private const int WM_TIMER = 0x0113;

    public const int TOUCHEVENTF_MOVE = 0x0001; //	Movement has occurred. Cannot be combined with TOUCHEVENTF_DOWN.
    public const int TOUCHEVENTF_DOWN = 0x0002; //	The corresponding touch point was established through a new contact. Cannot be combined with TOUCHEVENTF_MOVE or TOUCHEVENTF_UP.
    public const int TOUCHEVENTF_UP = 0x0004; // A touch point was removed.
    public const int TOUCHEVENTF_INRANGE = 0x0008; // A touch point is in range. This flag is used to enable touch hover support on compatible hardware. Applications that do not want support for hover can ignore this flag.
    public const int TOUCHEVENTF_PRIMARY = 0x0010; // Indicates that this TOUCHINPUT structure corresponds to a primary contact point. See the following text for more information on primary touch points.
    public const int TOUCHEVENTF_NOCOALESCE = 0x0020; // When received using GetTouchInputInfo, this input was not coalesced.
    public const int TOUCHEVENTF_PALM = 0x0080; // The touch event came from the user's palm.

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    private static extern int GetCurrentThreadId();

    [DllImport("kernel32.dll")]
    private static extern int GetLastError();

    [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
    public static extern IntPtr GetDesktopWindow();

    [DllImport("user32.dll")]
    [return: MarshalAs(UnmanagedType.Bool)]
    public static extern bool GetWindowRect(HandleRef hwnd, out RECT lpRect);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    private static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    private static extern bool UnhookWindowsHookEx(int idHook);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    private static extern int CallNextHookEx(int idHook, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool RegisterTouchWindow(IntPtr hwnd, uint flags);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool CloseTouchInputHandle(IntPtr hTouchInput);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern bool GetTouchInputInfo(IntPtr hTouchInput, int cInputs, [In, Out] TOUCHINPUT[] pInputs, int cbSize);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern uint SetTimer(IntPtr hWnd, Int32 nIDEvent, Int32 uElapse, HookProc lpfn);

    [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall, SetLastError = true)]
    private static extern bool KillTimer(IntPtr hWnd, Int32 nIDEvent);

    [StructLayout(LayoutKind.Sequential)]
    public struct RECT {
        public int Left;        // x position of upper-left corner
        public int Top;         // y position of upper-left corner
        public int Right;       // x position of lower-right corner
        public int Bottom;      // y position of lower-right corner
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct POINT {
        public int X;
        public int Y;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct MSG {
        public IntPtr hwnd;
        public UInt32 message;
        public IntPtr wParam;
        public IntPtr lParam;
        public UInt32 time;
        public POINT pt;
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct TOUCHINPUT {
        public UInt32 x;
        public UInt32 y;
        public IntPtr hSource;
        public UInt32 dwID;
        public UInt32 dwFlags;
        public UInt32 dwMask;
        public UInt32 dwTime;
        public Int32 dwExtraInfo;
        public UInt32 cxContact;
        public UInt32 cyContact;
    }
    #endregion

    private static int screenOffsetX;
    private static int screenOffsetY;
    private static int screenWidth;
    private static int screenHeight;

    private static TOUCHINPUT[] inputs;

    private static Queue<TOUCHINPUT[]> rawTouchesQueue = new Queue<TOUCHINPUT[]>();
    private static List<W7Touch> touches = new List<W7Touch>();
    private static Dictionary<uint, uint> touchIdMap = new Dictionary<uint, uint>();
    private static uint lastTouchId = 100;

    void Start() {
        try {
            if (W7TouchCapabilities.HasMultiTouch) {
                hHook = SetWindowsHookEx(WH_GETMESSAGE, new HookProc(TouchHookProc), (IntPtr)0, GetCurrentThreadId());
                Debug.Log("Installed touch hook");

                screenOffsetX = (int)Camera.main.pixelRect.x;
                screenOffsetY = (int)Camera.main.pixelRect.y;
                screenWidth = (int)Camera.main.pixelRect.width;
                screenHeight = (int)Camera.main.pixelRect.height;

                Debug.Log(string.Format("X-Offset: {0} Y-Offset: {1} Width: {2} Height: {3}", screenOffsetX, screenOffsetY, screenWidth, screenHeight));

                Debug.Log("Touch Manager successfully started");
            } else {
                Debug.LogError("Host has not Windows 7, it has: '" + Environment.OSVersion.ToString() + "' also, check if the screen is touch capable");
            }
        } catch (Win32Exception) {
            Debug.Log("Could not install touch hook");
        }
    }

    void Update() {
        if (hHook != 0) {
            // clean previous touches
            List<W7Touch> touchesToDelete = new List<W7Touch>();

            // remove duplicate touch ids (down + move, move + move, ...)
            Dictionary<uint, W7Touch> duplicateTouches = new Dictionary<uint, W7Touch>();
            foreach (W7Touch touch in touches) {
                if (duplicateTouches.ContainsKey(touch.Id)) {
                    touchesToDelete.Add(duplicateTouches[touch.Id]);
                    duplicateTouches[touch.Id] = touch;
                } else {
                    duplicateTouches.Add(touch.Id, touch);
                }
            }
            foreach (W7Touch touch in touchesToDelete) {
                touches.Remove(touch);
            }
            touchesToDelete.Clear();

            // remove ended touches
            foreach (W7Touch touch in touches) {
                if (touch.Phase == TouchPhase.Ended || touch.Phase == TouchPhase.Canceled) {
                    touchesToDelete.Add(touch);
                }
            }
            foreach (W7Touch touch in touchesToDelete) {
                touches.Remove(touch);
            }
            touchesToDelete.Clear();

            lock (rawTouchesQueue) {
                while (rawTouchesQueue.Count > 0) {
                    TOUCHINPUT[] rawTouches = rawTouchesQueue.Dequeue();
                    for (uint i = 0; i < rawTouches.Length; ++i) {
                        TOUCHINPUT rawTouch = rawTouches[i];
                        if (!touchIdMap.ContainsKey(rawTouch.dwID)) {
                            touchIdMap.Add(rawTouch.dwID, lastTouchId);
                            lastTouchId++;
                        }

                        W7Touch touch;
                        try {
                            touch = touches.Find(t => t.Id == touchIdMap[rawTouch.dwID]);
                        } catch {
                            touch = null;
                        }

                        Vector2 pos = new Vector2(rawTouch.x * 0.01f, rawTouch.y * 0.01f);
                        if (touch != null) {
                            if ((rawTouch.dwFlags & TOUCHEVENTF_DOWN) == TOUCHEVENTF_DOWN) {
                                touch.EndTouch();

                                touchIdMap.Add(rawTouch.dwID, lastTouchId);
                                lastTouchId++;
                                touch = new W7Touch(touchIdMap[rawTouch.dwID], pos);
                                touches.Add(touch);
                            } else if ((rawTouch.dwFlags & TOUCHEVENTF_MOVE) == TOUCHEVENTF_MOVE) {
                                if (touch.Phase == TouchPhase.Moved || touch.Phase == TouchPhase.Stationary) {
                                    touch.UpdateTouch(pos);
                                } else {
                                    touch = new W7Touch(touchIdMap[rawTouch.dwID], pos);
                                    touch.UpdateTouch(pos);
                                    touches.Add(touch);
                                }
                            } else if ((rawTouch.dwFlags & TOUCHEVENTF_UP) == TOUCHEVENTF_UP) {
                                touch = new W7Touch(touchIdMap[rawTouch.dwID], pos);
                                touch.EndTouch();
                                touches.Add(touch);
                                touchIdMap.Remove(rawTouch.dwID);
                            }
                        } else {
                            touch = new W7Touch(touchIdMap[rawTouch.dwID], pos);
                            touches.Add(touch);
                            if ((rawTouch.dwFlags & TOUCHEVENTF_DOWN) == TOUCHEVENTF_DOWN) {
                            } else if ((rawTouch.dwFlags & TOUCHEVENTF_MOVE) == TOUCHEVENTF_MOVE) {
                                touch = new W7Touch(touchIdMap[rawTouch.dwID], pos);
                                touch.UpdateTouch(pos);
                                touches.Add(touch);
                            } else if ((rawTouch.dwFlags & TOUCHEVENTF_UP) == TOUCHEVENTF_UP) {
                                touch = new W7Touch(touchIdMap[rawTouch.dwID], pos);
                                touch.EndTouch();
                                touches.Add(touch);
                                touchIdMap.Remove(rawTouch.dwID);
                            }
                        }
                    }
                }
            }
            foreach (W7Touch touch in touches) {
                touch.Update();
                if (touch.Phase == TouchPhase.Canceled) {
                    var k = (from t in touchIdMap where t.Value == touch.Id select t.Key);
                    if (k.Count() > 0) {
                        touchIdMap.Remove(k.First());
                    }
                }
            }
        }
    }

    public static int GetTouchCount() {
        return touches.Count;
    }

    public static W7Touch GetTouch(int index) {
        return touches[index];
    }

    public int TouchHookProc(int nCode, IntPtr wParam, IntPtr lParam) {
        if (nCode < 0) {
            return CallNextHookEx(0, nCode, wParam, lParam);
        }
        switch (nCode) {
            case HC_ACTION:
                MSG msg = (MSG)Marshal.PtrToStructure(lParam, typeof(MSG));

                // It's ok to call this several times
                RegisterTouchWindow(msg.hwnd, TWF_WANTPALM);

                switch (msg.message) {
                    case WM_TOUCH:
                        int inputCount = LoWord(msg.wParam.ToInt32());
                        inputs = new TOUCHINPUT[inputCount];

                        if (GetTouchInputInfo(msg.lParam, inputCount, inputs, Marshal.SizeOf(typeof(TOUCHINPUT)))) {
                            lock (rawTouchesQueue) {
                                rawTouchesQueue.Enqueue(inputs);
                            }
                            CloseTouchInputHandle(msg.lParam);
                        }
                        break;
                }
                break;
        }
        return CallNextHookEx(0, nCode, wParam, lParam);
    }

    void OnDestroy() {
        if (hHook != 0) {
            if (!UnhookWindowsHookEx(hHook))
                Debug.LogError("Couldn't Unhook Window");

            Debug.Log("Window successfully Unhooked");
        }
    }

    private static int LoWord(int number) {
        return number & 0xffff;
    }
}