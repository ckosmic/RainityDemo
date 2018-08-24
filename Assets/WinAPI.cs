using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System.Text;

public class WinAPI : MonoBehaviour {

	public static WinAPI instance = null;

	[StructLayout(LayoutKind.Sequential)]
	public struct RECT
	{
		public int Left, Top, Right, Bottom;

		public RECT(int left, int top, int right, int bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public int X
		{
			get { return Left; }
			set { Right -= (Left - value); Left = value; }
		}

		public int Y
		{
			get { return Top; }
			set { Bottom -= (Top - value); Top = value; }
		}

		public int Height
		{
			get { return Bottom - Top; }
			set { Bottom = value + Top; }
		}

		public int Width
		{
			get { return Right - Left; }
			set { Right = value + Left; }
		}

		public static bool operator ==(RECT r1, RECT r2)
		{
			return r1.Equals(r2);
		}

		public static bool operator !=(RECT r1, RECT r2)
		{
			return !r1.Equals(r2);
		}

		public bool Equals(RECT r)
		{
			return r.Left == Left && r.Top == Top && r.Right == Right && r.Bottom == Bottom;
		}

		public override string ToString()
		{
			return string.Format(System.Globalization.CultureInfo.CurrentCulture, "{{Left={0},Top={1},Right={2},Bottom={3}}}", Left, Top, Right, Bottom);
		}
	}

	public struct MARGINS
	{
		public int cxLeftWidth;
		public int cxRightWidth;
		public int cyTopHeight;
		public int cyBottomHeight;
	}

	[DllImport("user32.dll")]
	public static extern bool GetWindowRect(int hWnd, out RECT lpRect);
	[DllImport("user32.dll", EntryPoint = "GetActiveWindow")]
	public static extern int GetActiveWindow();
	[DllImport("user32.dll")]
	public static extern int GetForegroundWindow();
	[DllImport("user32.dll")]
	public static extern bool IsWindowVisible(int hWnd);
	[DllImport("user32.dll", EntryPoint = "SetWindowLongA")]
	public static extern int SetWindowLong(int hwnd, int nIndex, long dwNewLong);
	[DllImport("user32.dll")]
	public static extern bool ShowWindowAsync(int hWnd, int nCmdShow);
	[DllImport("user32.dll")]
	public static extern bool ShowWindow(int hWnd, int nCmdShow);
	[DllImport("user32.dll", EntryPoint = "SetLayeredWindowAttributes")]
	public static extern int SetLayeredWindowAttributes(int hwnd, int crKey, byte bAlpha, int dwFlags);
	[DllImport("user32.dll", EntryPoint = "GetWindowLong")]
	public static extern long GetWindowLong(int hwnd, int nIndex);
	[DllImport("user32.dll", EntryPoint = "SetWindowPos")]
	public static extern int SetWindowPos(int hwnd, int hwndInsertAfter, int x, int y, int cx, int cy, int uFlags);
	[DllImport("user32.dll")]
	public static extern bool MoveWindow(int hWnd, int x, int y, int nWidth, int nHeight, bool repaint);
	[DllImport("user32.dll")]
	public static extern bool SetForegroundWindow(int hWnd);
	[DllImport("user32.dll")]
	public static extern bool PrintWindow(int handle, System.IntPtr hdc, uint flags);
	[DllImport("user32.dll")]
	public static extern int GetDC(int handle);
	[DllImport("user32.dll")]
	public static extern int ReleaseDC(int handle, System.IntPtr hdc);
	[DllImport("user32.dll")]
	public static extern int GetDesktopWindow();
	[DllImport("user32.dll")]
	public static extern int UpdateLayeredWindow(int hwnd, int crKey, byte bAlpha, int dwFlags);

	[DllImport("Dwmapi.dll")]
	public static extern uint DwmExtendFrameIntoClientArea (int hWnd, ref MARGINS margins);

	[DllImport("Gdi32.dll")]
	public static extern bool BitBlt(System.IntPtr hdc, int xDest, int yDest, int width, int height, System.IntPtr hdcSrc, int xSrc, int ySrc, int dwRop);
	[DllImport("Gdi32.dll")]
	public static extern System.IntPtr CreateCompatibleDC(System.IntPtr hdc);
	[DllImport("Gdi32.dll")]
	public static extern int GetDeviceCaps(System.IntPtr hdc, int index);
	[DllImport("Gdi32.dll")]
	public static extern System.IntPtr SelectObject(System.IntPtr hdc, System.IntPtr hgdiobj);
	[DllImport("Gdi32.dll")]
	public static extern int DeleteDC(System.IntPtr hdc);
	[DllImport("Gdi32.dll")]
	public static extern System.IntPtr DeleteObject(System.IntPtr ptr);


	[DllImport("shell32.dll")]
	public static extern System.IntPtr ExtractAssociatedIcon(int hInst, StringBuilder lpIconPath, out ushort lpiIcon);
	[DllImport("shell32.dll")]
	public static extern System.IntPtr ExtractIcon(System.IntPtr hInst, string lpszExeFileName, int nIconIndex);

	public static int activeWindow;
	public static int foregroundWindow;
	public static int thisGame;
	public static int desktopWindow;
	public static RECT windowRect;

	void Awake() {
		if (instance == null) instance = this;
		else if (instance != this) Destroy(gameObject);
		DontDestroyOnLoad(gameObject);
	}

	void Start() {
		thisGame = GetActiveWindow();
		desktopWindow = GetDesktopWindow();
	}

	void Update() {
		activeWindow = GetActiveWindow();
		foregroundWindow = GetForegroundWindow();
		GetWindowRect(foregroundWindow, out windowRect);
	}
}
