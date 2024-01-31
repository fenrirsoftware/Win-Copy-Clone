using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace clipboard {
  public partial class Form1: Form {

    #region DLL Dosyaları
      [DllImport("CopyPasteRelease.dll", CallingConvention = CallingConvention.Cdecl)]
    public static extern IntPtr ClipBoardData();

    private bool dragging = false;
    private Point dragStartPoint;
    private bool keepRunning = true;
    private NotifyIcon notifyIcon;
    private
    const int WH_KEYBOARD_LL = 13;
    private
    const int WM_KEYDOWN = 0x0100;
    private
    const int VK_CONTROL = 0x11;
    private
    const int VK_LWIN = 0x5B;
    private
    const int VK_RWIN = 0x5C;

    private static IntPtr hookId = IntPtr.Zero;

    private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("user32.dll", SetLastError = true)]
    private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

    [DllImport("user32.dll", SetLastError = true)]
    [
      return: MarshalAs(UnmanagedType.Bool)
    ]
    private static extern bool UnhookWindowsHookEx(IntPtr hhk);

    [DllImport("user32.dll")]
    private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

    [DllImport("kernel32.dll", SetLastError = true)]
    private static extern IntPtr GetModuleHandle(string lpModuleName);

    [DllImport("dwmapi.dll", CharSet = CharSet.Unicode, PreserveSig = false)]
    internal static extern void DwmSetWindowAttribute(IntPtr hwnd,
      DWMWINDOWATTRIBUTE attribute,
      ref DWM_WINDOW_CORNER_PREFERENCE pvAttribute,
      uint cbAttribute);

    public enum DWMWINDOWATTRIBUTE {
      DWMWA_WINDOW_CORNER_PREFERENCE = 33
    }

    public enum DWM_WINDOW_CORNER_PREFERENCE {
      DWMWCP_DEFAULT = 0,
        DWMWCP_DONOTROUND = 1,
        DWMWCP_ROUND = 2,
        DWMWCP_ROUNDSMALL = 3
    }

    #endregion

    public Form1() {
      InitializeComponent();
      var attribute = DWMWINDOWATTRIBUTE.DWMWA_WINDOW_CORNER_PREFERENCE;
      var preference = DWM_WINDOW_CORNER_PREFERENCE.DWMWCP_ROUND;
      DwmSetWindowAttribute(this.Handle, attribute, ref preference, sizeof(uint));

      //bilgisayarınız win11 altı bir sürümdeyse yukarıdaki 3 satırı yorum satırı yapın.
      label1.Font = new Font("Fira Code", 9, FontStyle.Bold);
      rjButton1.Font = new Font("Fira Code", 12, FontStyle.Bold);

      notifyIcon = new NotifyIcon();
      notifyIcon.Icon = SystemIcons.Application;
      notifyIcon.Text = "Clipboard App";
      notifyIcon.Click += NotifyIcon_Click;

      using(Process curProcess = Process.GetCurrentProcess())
      using(ProcessModule curModule = curProcess.MainModule) {
        hookId = SetHook(HookCallback, GetModuleHandle(curModule.ModuleName));
      }

      Application.Run();

      UnhookWindowsHookEx(hookId);
    }

    private async void Form1_Load(object sender, EventArgs e) {
      flowLayoutPanel2.AutoScroll = true;
      await LoadAsyncData();
      this.KeyPreview = true;

    }

    private async Task LoadAsyncData() {
      while (keepRunning) {
        await Task.Delay(100);
        IntPtr result = await Task.Run(() => ClipBoardData());
        string message = Marshal.PtrToStringAnsi(result);

        if (message != previousData && message != null) {
          CreateAndFillTextBox(message);
          previousData = message;
        }
      }
    }

    private string previousData = string.Empty;
    private int textBoxCount = 0;
    private FlowLayoutPanel flowLayoutPanel1;

    private void CreateAndFillTextBox(string text) {
      RJButton newTextBox = new RJButton();
      newTextBox.Size = new Size(335, 70);
      newTextBox.BorderRadius = 8;
      newTextBox.FlatStyle = FlatStyle.Flat;
      newTextBox.FlatAppearance.BorderSize = 0;
      newTextBox.TextAlign = ContentAlignment.MiddleLeft;

      newTextBox.Text = text;
      newTextBox.Font = new Font("Fira Code", 9, FontStyle.Bold);
      newTextBox.BackColor = Color.FromArgb(40, 40, 40);

      newTextBox.Click += Button_Click;

      newTextBox.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
      flowLayoutPanel2.Controls.Add(newTextBox);
      flowLayoutPanel2.Controls.SetChildIndex(newTextBox, 0);
    }

    private void Button_Click(object sender, EventArgs e) {
      {
        if (sender is RJButton clickedButton) {

          string buttonText = clickedButton.Text;
          Clipboard.SetText(buttonText);

        }
      }
    }

    private void pictureBox1_MouseDown(object sender, MouseEventArgs e) {
      if (e.Button == MouseButtons.Left) {
        dragging = true;
        dragStartPoint = new Point(e.X, e.Y);
      }
    }

    private void pictureBox1_MouseUp(object sender, MouseEventArgs e) {
      if (e.Button == MouseButtons.Left) {
        dragging = false;
      }
    }

    private void pictureBox1_MouseMove(object sender, MouseEventArgs e) {
      if (dragging) {
        Point dragDelta = new Point(e.X - dragStartPoint.X, e.Y - dragStartPoint.Y);
        Location = new Point(Location.X + dragDelta.X, Location.Y + dragDelta.Y);
      }
    }

    private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
      keepRunning = false;
    }

    private void rjButton1_Click(object sender, EventArgs e) {
      this.Hide();
      notifyIcon.Visible = true;
    }
    private void NotifyIcon_Click(object sender, EventArgs e) {

      this.Show();
      this.WindowState = FormWindowState.Normal;
    }

    private void Form1_Deactivate(object sender, EventArgs e) {

      this.Hide();
      notifyIcon.Visible = true;
    }

    #region Keyboard Hook
    private static IntPtr SetHook(LowLevelKeyboardProc proc, IntPtr hMod) {
      using(Process curProcess = Process.GetCurrentProcess())
      using(ProcessModule curModule = curProcess.MainModule) {
        return SetWindowsHookEx(WH_KEYBOARD_LL, proc, hMod, 0);
      }
    }

    private IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam) {
      if (nCode >= 0 && wParam == (IntPtr) WM_KEYDOWN) {
        int vkCode = Marshal.ReadInt32(lParam);

        // CTRL + B tuş kombinasyonu kontrolü
        bool ctrlPressed = (Control.ModifierKeys & Keys.Control) != 0;
        bool bPressed = ((Keys) vkCode == Keys.B);

        if (ctrlPressed && bPressed) {
          this.Show();
          this.WindowState = FormWindowState.Normal;
        }
      }

      return CallNextHookEx(hookId, nCode, wParam, lParam);
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct KBDLLHOOKSTRUCT {
      public int vkCode;
      public int scanCode;
      public int flags;
      public int time;
      public IntPtr dwExtraInfo;
    }

    private enum KeyFlags {
      LLKHF_EXTENDED = 0x01,
        LLKHF_INJECTED = 0x10,
        LLKHF_ALTDOWN = 0x20,
        LLKHF_UP = 0x80,
    }

    #endregion

  }
}