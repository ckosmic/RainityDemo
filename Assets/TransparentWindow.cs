using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.InteropServices;
using System;

public delegate IntPtr WndProcDelegate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);

public class TransparentWindow : MonoBehaviour {

	[SerializeField]
	private Material m_Material;

	[SerializeField]
	private Camera mainCamera;

	[SerializeField]
	private LayerMask clickthrough;

	private bool clickThrough = true;
	private bool prevClickThrough = true;

	public IntPtr interactionWindow;
	IntPtr hMainWindow;
	IntPtr oldWndProcPtr;
	IntPtr newWndProcPtr;
	WndProcDelegate newWndProc;

	public static bool forceInBack = true;

	[DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
	public static extern System.IntPtr GetForegroundWindow();
	[DllImport("user32.dll")]
	static extern IntPtr SetWindowLongPtr(IntPtr hWnd, int nIndex, IntPtr dwNewLong);
	[DllImport("user32.dll")]
	static extern IntPtr SetWindowLongPtr(int hWnd, int nIndex, long dwNewLong);
	[DllImport("user32.dll")]
	static extern IntPtr CallWindowProc(IntPtr lpPrevWndFunc, IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
	[DllImport("user32.dll")]
	[return: MarshalAs(UnmanagedType.Bool)]
	static extern bool EnableWindow(IntPtr hWnd, bool bEnable);
	[DllImport("user32.dll", EntryPoint = "ShowWindow", SetLastError = true)]
	static extern bool ShowWindow(int hWnd, int nCmdShow);

	const int GWL_STYLE = -16;
	const int GWL_EXSTYLE = -20;
	const uint WS_POPUP = 0x80000000;
	const uint WS_VISIBLE = 0x10000000;
	const uint WS_EX_LAYERED = 0x00080000;
	const int HWND_TOPMOST = -1;
	const int HWND_BOTTOM = 1;
	const int WM_WINDOWPOSCHANGING = 0x46;
	const int WM_WINDOWPOSCHANGED = 0x47;
	const int WM_ACTIVATE = 0x0006;
	const long WS_CAPTION = 0x00C00000L;
	const long WS_THICKFRAME = 0x00040000L;
	const long WS_MINIMIZEBOX = 0x00020000L;
	const long WS_MAXIMIZEBOX = 0x00010000L;
	const long WS_SYSMENU = 0x00080000L;
	const long WS_EX_DLGMODALFRAME = 0x00000001L;
	const long WS_EX_CLIENTEDGE = 0x00000200L;
	const long WS_EX_STATICEDGE = 0x00020000L;

	[Flags()]
	public enum SetWindowPosFlags {
		SWP_NOSIZE = 0x1,
		SWP_NOMOVE = 0x2,
		SWP_NOZORDER = 0x4,
		SWP_NOREDRAW = 0x8,
		SWP_NOACTIVATE = 0x10,
		SWP_FRAMECHANGED = 0x20,
		SWP_DRAWFRAME = SWP_FRAMECHANGED,
		SWP_SHOWWINDOW = 0x40,
		SWP_HIDEWINDOW = 0x80,
		SWP_NOCOPYBITS = 0x100,
		SWP_NOOWNERZORDER = 0x200,
		SWP_NOREPOSITION = SWP_NOOWNERZORDER,
		SWP_NOSENDCHANGING = 0x400,
		SWP_DEFERERASE = 0x2000,
		SWP_ASYNCWINDOWPOS = 0x4000,
	}

	[Flags()]
	public enum ShowWindowFlags {
		SW_MAXIMIZE = 3,
		SW_SHOWMAXIMIZED = 3
	}

	int handle;
	int fWidth;
	int fHeight;
	WinAPI.MARGINS margins;

	// Use this for initialization
	void Start () {
		if (!Application.isEditor) {
			mainCamera = GetComponent<Camera>();

			hMainWindow = GetForegroundWindow();
			newWndProc = new WndProcDelegate(wndProc);
			newWndProcPtr = Marshal.GetFunctionPointerForDelegate(newWndProc);
			oldWndProcPtr = SetWindowLongPtr(hMainWindow, -4, newWndProcPtr);

			fWidth = Screen.currentResolution.width;
			fHeight = Screen.currentResolution.height-40;
			//margins = new WinAPI.MARGINS() {  };
			handle = WinAPI.GetActiveWindow();

			//Screen.SetResolution(fWidth, fHeight, true);
			//SetWindowLongPtr(handle, GWL_STYLE, WS_POPUP | WS_VISIBLE);
			//WinAPI.SetWindowLong(handle, -20, 524288 | 32);
			//WinAPI.SetWindowLong(handle, -20, WS_EX_LAYERED);
			//WinAPI.SetLayeredWindowAttributes(handle, 0, 255, 2);
			//WinAPI.SetWindowPos(handle, HWND_TOPMOST, 0, 0, fWidth, fHeight, 32 | 64);
			long lStyle = WinAPI.GetWindowLong(handle, GWL_STYLE);
			lStyle &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_SYSMENU);
			SetWindowLongPtr(handle, GWL_STYLE, lStyle);
			long lExStyle = WinAPI.GetWindowLong(handle, GWL_EXSTYLE);
			lExStyle &= ~(WS_EX_DLGMODALFRAME | WS_EX_CLIENTEDGE | WS_EX_STATICEDGE);
			SetWindowLongPtr(handle, GWL_EXSTYLE, lExStyle);

			WinAPI.SetWindowPos(handle, HWND_BOTTOM, 0, -40, fWidth, fHeight, (int)(SetWindowPosFlags.SWP_FRAMECHANGED | SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOOWNERZORDER));
			//WinAPI.DwmExtendFrameIntoClientArea(handle, ref margins);

			Screen.SetResolution(fWidth, fHeight, false);
		}
	}

	private static IntPtr StructToPtr(object obj) {
		var ptr = Marshal.AllocHGlobal(Marshal.SizeOf(obj));
		Marshal.StructureToPtr(obj, ptr, false);
		return ptr;
	}
	IntPtr wndProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam) {
		if (msg == WM_WINDOWPOSCHANGING && forceInBack) {
			winPosChange(hWnd, msg, wParam, lParam);
		}
		if (msg == WM_ACTIVATE) {
			//winActivate(hWnd, msg, wParam, lParam);
		}
		return CallWindowProc(oldWndProcPtr, hWnd, msg, wParam, lParam);
	}
	void winPosChange(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam) {
		long lStyle = WinAPI.GetWindowLong(handle, GWL_STYLE);
		lStyle &= ~(WS_CAPTION | WS_THICKFRAME | WS_MINIMIZEBOX | WS_MAXIMIZEBOX | WS_SYSMENU);
		SetWindowLongPtr(handle, GWL_STYLE, lStyle);
		long lExStyle = WinAPI.GetWindowLong(handle, GWL_EXSTYLE);
		lExStyle &= ~(WS_EX_DLGMODALFRAME | WS_EX_CLIENTEDGE | WS_EX_STATICEDGE);
		SetWindowLongPtr(handle, GWL_EXSTYLE, lExStyle);

		WinAPI.SetWindowPos(handle, HWND_BOTTOM, 0, -40, fWidth, fHeight, (int)(SetWindowPosFlags.SWP_FRAMECHANGED | SetWindowPosFlags.SWP_NOMOVE | SetWindowPosFlags.SWP_NOSIZE | SetWindowPosFlags.SWP_NOZORDER | SetWindowPosFlags.SWP_NOOWNERZORDER));
		WINDOWPOS wndPos = WINDOWPOS.FromMessage(hWnd, msg, wParam, lParam);
		wndPos.flags = wndPos.flags | SetWindowPosFlags.SWP_NOZORDER;
		wndPos.UpdateMessage(hWnd, msg, wParam, lParam);

		ShowWindow(handle, (int)ShowWindowFlags.SW_SHOWMAXIMIZED);
	}
	void winActivate(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam) {
		EnableWindow(hWnd, false);
	}

	void OnRenderImage(RenderTexture from, RenderTexture to) {
		Graphics.Blit(from, to, m_Material);
	}

	[StructLayout(LayoutKind.Sequential)]
	public struct WINDOWPOS {
		public IntPtr hWnd;
		public IntPtr hwndInsertAfter;
		public int x;
		public int y;
		public int cx;
		public int cy;
		public SetWindowPosFlags flags;

		public static WINDOWPOS FromMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam) {
			return (WINDOWPOS)Marshal.PtrToStructure(lParam, typeof(WINDOWPOS));
		}

		public void UpdateMessage(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam) {
			Marshal.StructureToPtr(this, lParam, true);
		}
	}
}